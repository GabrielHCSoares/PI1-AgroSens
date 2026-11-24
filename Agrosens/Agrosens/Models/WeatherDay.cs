using SQLite;

namespace Agrosens.Models
{
    /// <summary>Previsão/observação diária guardada localmente (cache do Open-Meteo).</summary>
    [Table("weather_days")]
    public class WeatherDay
    {
        /// <summary>Data no formato yyyy-MM-dd (chave: um registro por dia).</summary>
        [PrimaryKey]
        public string DateKey { get; set; } = string.Empty;

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

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        /// <summary>Quando este registro foi baixado da API.</summary>
        public DateTime FetchedAt { get; set; } = DateTime.Now;
    }
}
