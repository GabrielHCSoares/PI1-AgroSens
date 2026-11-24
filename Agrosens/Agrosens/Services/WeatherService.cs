using System.Globalization;
using System.Text.Json;

namespace Agrosens.Services
{
    /// <summary>Previsão de um dia (dados do Open-Meteo).</summary>
    public class DayWeather
    {
        public DateTime Date { get; set; }
        public int Code { get; set; }
        public double? TempMax { get; set; }
        public double? TempMin { get; set; }
        public int? RainProbability { get; set; }
        public double? RainSum { get; set; }
        public double? WindMax { get; set; }
        public double? WindDirection { get; set; }
        public double? FeelsMax { get; set; }
        public double? FeelsMin { get; set; }
        public double? HumidityMean { get; set; }
        public double? DewPointMean { get; set; }

        /// <summary>Quando foi baixado da API (preenchido ao ler do banco local).</summary>
        public DateTime? FetchedAt { get; set; }

        public string Emoji => WeatherService.EmojiFor(Code);
        public string Description => WeatherService.DescriptionFor(Code);
    }

    /// <summary>Cliente simples da API gratuita do Open-Meteo (sem chave de API).</summary>
    public static class WeatherService
    {
        private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(10) };

        /// <summary>Busca a previsão diária cobrindo de 7 dias atrás até 7 dias à frente.</summary>
        public static async Task<Dictionary<DateTime, DayWeather>> GetDailyAsync(double lat, double lng)
        {
            var inv = CultureInfo.InvariantCulture;
            string url = "https://api.open-meteo.com/v1/forecast" +
                         $"?latitude={lat.ToString(inv)}&longitude={lng.ToString(inv)}" +
                         "&daily=weather_code,temperature_2m_max,temperature_2m_min," +
                         "precipitation_probability_max,precipitation_sum,wind_speed_10m_max," +
                         "wind_direction_10m_dominant,apparent_temperature_max,apparent_temperature_min," +
                         "relative_humidity_2m_mean,dew_point_2m_mean" +
                         "&timezone=auto&past_days=7&forecast_days=7";

            var result = new Dictionary<DateTime, DayWeather>();
            using var stream = await Http.GetStreamAsync(url).ConfigureAwait(false);
            using var doc = await JsonDocument.ParseAsync(stream).ConfigureAwait(false);
            var daily = doc.RootElement.GetProperty("daily");

            var times = daily.GetProperty("time");
            for (int i = 0; i < times.GetArrayLength(); i++)
            {
                if (!DateTime.TryParse(times[i].GetString(), inv, DateTimeStyles.None, out var date))
                    continue;

                var w = new DayWeather
                {
                    Date = date.Date,
                    Code = (int)(Num(daily, "weather_code", i) ?? 0),
                    TempMax = Num(daily, "temperature_2m_max", i),
                    TempMin = Num(daily, "temperature_2m_min", i),
                    RainProbability = Num(daily, "precipitation_probability_max", i) is double p ? (int)Math.Round(p) : null,
                    RainSum = Num(daily, "precipitation_sum", i),
                    WindMax = Num(daily, "wind_speed_10m_max", i),
                    WindDirection = Num(daily, "wind_direction_10m_dominant", i),
                    FeelsMax = Num(daily, "apparent_temperature_max", i),
                    FeelsMin = Num(daily, "apparent_temperature_min", i),
                    HumidityMean = Num(daily, "relative_humidity_2m_mean", i),
                    DewPointMean = Num(daily, "dew_point_2m_mean", i)
                };
                result[w.Date] = w;
            }
            return result;
        }

        private static double? Num(JsonElement daily, string name, int index)
        {
            if (!daily.TryGetProperty(name, out var arr) || arr.ValueKind != JsonValueKind.Array) return null;
            if (index >= arr.GetArrayLength()) return null;
            var el = arr[index];
            return el.ValueKind == JsonValueKind.Number ? el.GetDouble() : null;
        }

        public static string EmojiFor(int code) => code switch
        {
            0 => "☀️",                       // ☀️
            1 => "\U0001F324️",                   // 🌤️
            2 => "⛅",                             // ⛅
            3 => "☁️",                       // ☁️
            45 or 48 => "\U0001F32B️",            // 🌫️
            >= 51 and <= 57 => "\U0001F326️",     // 🌦️
            >= 61 and <= 67 => "\U0001F327️",     // 🌧️
            >= 71 and <= 77 => "❄️",         // ❄️
            >= 80 and <= 82 => "\U0001F327️",     // 🌧️
            85 or 86 => "❄️",
            >= 95 => "⛈️",                   // ⛈️
            _ => "⛅"
        };

        public static string DescriptionFor(int code) => code switch
        {
            0 => "Céu limpo",
            1 => "Predominantemente limpo",
            2 => "Parcialmente nublado",
            3 => "Nublado",
            45 or 48 => "Neblina",
            >= 51 and <= 57 => "Garoa",
            >= 61 and <= 67 => "Chuva",
            >= 71 and <= 77 => "Neve",
            >= 80 and <= 82 => "Pancadas de chuva",
            85 or 86 => "Pancadas de neve",
            >= 95 => "Tempestade",
            _ => "Tempo variável"
        };
    }
}
