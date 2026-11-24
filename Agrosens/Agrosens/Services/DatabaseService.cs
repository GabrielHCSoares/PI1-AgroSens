using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Agrosens.Models;
using Android.App;
using Android.Util;
using SQLite;
using SQLitePCL;

namespace Agrosens.Services
{
    public class DatabaseService
    {
        private const string DbFileName = "agrosens.db3";
        private static readonly Lazy<DatabaseService> _instance = new Lazy<DatabaseService>(() => new DatabaseService());
        private readonly Lazy<Task<SQLiteAsyncConnection>> _lazyConnection;
        private bool _initialized = false;
        private bool _pclInitialized = false;
        private readonly object _lock = new object();

        public static DatabaseService Instance => _instance.Value;

        private DatabaseService()
        {
            _lazyConnection = new Lazy<Task<SQLiteAsyncConnection>>(async () =>
            {
                var dbPath = GetDbPath();
                var connection = new SQLiteAsyncConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create, storeDateTimeAsTicks: true);
                return connection;
            });
        }

        private async Task<SQLiteAsyncConnection> GetConnectionAsync()
        {
            return await _lazyConnection.Value.ConfigureAwait(false);
        }

        private static string GetDbPath()
        {
            string pasta;
            try
            {
                pasta = Android.App.Application.Context.FilesDir.AbsolutePath;
            }
            catch
            {
                pasta = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            }

            Directory.CreateDirectory(pasta);
            return Path.Combine(pasta, DbFileName);
        }

        private static async Task EnsureColumnsAsync(SQLiteAsyncConnection conn)
        {
            foreach (var sql in new[]
            {
                "ALTER TABLE calendar_events ADD COLUMN StartTime INTEGER NOT NULL DEFAULT 0",
                "ALTER TABLE calendar_events ADD COLUMN EndTime INTEGER NOT NULL DEFAULT 0",
                "ALTER TABLE calendar_events ADD COLUMN Status INTEGER NOT NULL DEFAULT 0",
                "ALTER TABLE calendar_events ADD COLUMN Category TEXT",
                "ALTER TABLE calendar_events ADD COLUMN ColorHex TEXT",
                "ALTER TABLE sensores ADD COLUMN ValorAtual REAL",
                "ALTER TABLE sensores ADD COLUMN PorteiraAberta INTEGER NOT NULL DEFAULT 0",
                "ALTER TABLE agro_tasks ADD COLUMN Category TEXT"
            })
            {
                try
                {
                    await conn.ExecuteAsync(sql).ConfigureAwait(false);
                }
                catch
                {
                }
            }
        }

        public async Task InitializeAsync()
        {
            if (_initialized)
                return;

            lock (_lock)
            {
                if (_initialized)
                    return;
            }

            try
            {
                lock (_lock)
                {
                    if (!_pclInitialized)
                    {
                        SQLitePCL.Batteries_V2.Init();
                        _pclInitialized = true;
                    }
                }

                var conn = await GetConnectionAsync().ConfigureAwait(false);
                var dbPath = GetDbPath();
                Android.Util.Log.Info("Agrosens.DB", $"Caminho do banco SQLite: {dbPath}");
                await conn.CreateTableAsync<CalendarEvent>().ConfigureAwait(false);
                await conn.CreateTableAsync<AgroTask>().ConfigureAwait(false);
                await conn.CreateTableAsync<Sensor>().ConfigureAwait(false);
                await conn.CreateTableAsync<WeatherDay>().ConfigureAwait(false);
                await EnsureColumnsAsync(conn).ConfigureAwait(false);

                int removidas = await RemoverTarefaSementeAsync(conn).ConfigureAwait(false);
                Android.Util.Log.Info("Agrosens.DB",
                    $"Tarefas semeadas removidas: {removidas}");

                lock (_lock)
                {
                    _initialized = true;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Agrosens.DB", ex.ToString());
                lock (_lock)
                {
                    _initialized = false;
                }
                throw;
            }
        }

        public async Task<List<CalendarEvent>> GetEventsAsync()
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            return await conn.Table<CalendarEvent>().OrderBy(e => e.EventDate).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<CalendarEvent>> GetEventsForMonthAsync(int year, int month)
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            var start = new DateTime(year, month, 1);
            var end = start.AddMonths(1);
            return await conn.Table<CalendarEvent>()
                .Where(e => e.EventDate >= start && e.EventDate < end)
                .OrderBy(e => e.EventDate)
                .ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<CalendarEvent>> GetEventsForDayAsync(DateTime day)
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            var start = DateTime.SpecifyKind(day.Date, DateTimeKind.Unspecified);
            var end = start.AddDays(1);
            return await conn.Table<CalendarEvent>()
                .Where(e => e.EventDate >= start && e.EventDate < end && e.Status == 0)
                .OrderBy(e => e.StartTime)
                .ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<CalendarEvent>> GetEventsForWeekAsync(DateTime start, DateTime end)
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            var ini = DateTime.SpecifyKind(start.Date, DateTimeKind.Unspecified);
            var fim = DateTime.SpecifyKind(end.Date.AddDays(1), DateTimeKind.Unspecified);
            return await conn.Table<CalendarEvent>()
                .Where(e => e.EventDate >= ini && e.EventDate < fim)
                .OrderBy(e => e.EventDate)
                .ThenBy(e => e.StartTime)
                .ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<CalendarEvent>> GetAllEventsForDayAsync(DateTime day)
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            var start = DateTime.SpecifyKind(day.Date, DateTimeKind.Unspecified);
            var end = start.AddDays(1);
            return await conn.Table<CalendarEvent>()
                .Where(e => e.EventDate >= start && e.EventDate < end)
                .OrderBy(e => e.StartTime)
                .ToListAsync().ConfigureAwait(false);
        }

        // ---------- Clima (cache local do Open-Meteo) ----------

        /// <summary>Grava (insere ou substitui) a previsão baixada da API.</summary>
        public async Task SaveWeatherAsync(IEnumerable<DayWeather> dias, double lat, double lng)
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            var agora = DateTime.Now;
            var registros = dias.Select(d => new WeatherDay
            {
                DateKey = d.Date.ToString("yyyy-MM-dd"),
                Code = d.Code,
                TempMax = d.TempMax,
                TempMin = d.TempMin,
                RainProbability = d.RainProbability,
                RainSum = d.RainSum,
                WindMax = d.WindMax,
                WindDirection = d.WindDirection,
                FeelsMax = d.FeelsMax,
                FeelsMin = d.FeelsMin,
                HumidityMean = d.HumidityMean,
                DewPointMean = d.DewPointMean,
                Latitude = lat,
                Longitude = lng,
                FetchedAt = agora
            }).ToList();

            await conn.RunInTransactionAsync(tran =>
            {
                foreach (var r in registros)
                    tran.InsertOrReplace(r);
            }).ConfigureAwait(false);
            Android.Util.Log.Info("Agrosens.DB", $"Clima salvo no banco: {registros.Count} dias");
        }

        /// <summary>Lê o clima salvo localmente (funciona sem internet).</summary>
        public async Task<Dictionary<DateTime, DayWeather>> GetWeatherAsync()
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            var rows = await conn.Table<WeatherDay>().ToListAsync().ConfigureAwait(false);
            var result = new Dictionary<DateTime, DayWeather>();
            foreach (var r in rows)
            {
                if (!DateTime.TryParseExact(r.DateKey, "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out var data))
                    continue;
                result[data.Date] = new DayWeather
                {
                    Date = data.Date,
                    Code = r.Code,
                    TempMax = r.TempMax,
                    TempMin = r.TempMin,
                    RainProbability = r.RainProbability,
                    RainSum = r.RainSum,
                    WindMax = r.WindMax,
                    WindDirection = r.WindDirection,
                    FeelsMax = r.FeelsMax,
                    FeelsMin = r.FeelsMin,
                    HumidityMean = r.HumidityMean,
                    DewPointMean = r.DewPointMean,
                    FetchedAt = r.FetchedAt
                };
            }
            return result;
        }

        public async Task<int> SaveEventAsync(CalendarEvent ev)
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            int linhas = ev.Id == 0
                ? await conn.InsertAsync(ev).ConfigureAwait(false)
                : await conn.UpdateAsync(ev).ConfigureAwait(false);
            Android.Util.Log.Info("Agrosens.DB",
                $"SaveEventAsync: Id={ev.Id}, linhas afetadas={linhas}, tabela=calendar_events");
            return linhas;
        }

        public async Task<int> DeleteEventAsync(CalendarEvent ev)
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            return await conn.DeleteAsync(ev).ConfigureAwait(false);
        }

        public async Task<List<AgroTask>> GetTasksAsync()
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            return await conn.Table<AgroTask>().OrderBy(t => t.IsCompleted).ThenBy(t => t.CreatedAt).ToListAsync().ConfigureAwait(false);
        }

        public async Task<List<AgroTask>> GetPendingTasksAsync()
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            return await conn.Table<AgroTask>()
                .Where(t => !t.IsCompleted)
                .OrderBy(t => t.CreatedAt)
                .ToListAsync().ConfigureAwait(false);
        }

        public async Task<int> SaveTaskAsync(AgroTask task)
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            if (task.Id == 0)
                return await conn.InsertAsync(task).ConfigureAwait(false);
            else
                return await conn.UpdateAsync(task).ConfigureAwait(false);
        }

        public async Task<int> ToggleCompletedAsync(AgroTask task)
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            task.IsCompleted = !task.IsCompleted;
            return await conn.UpdateAsync(task).ConfigureAwait(false);
        }

        public async Task<int> DeleteTaskAsync(AgroTask task)
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            return await conn.DeleteAsync(task).ConfigureAwait(false);
        }

        public async Task<List<Sensor>> GetSensoresAsync()
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            return await conn.Table<Sensor>().OrderByDescending(s => s.CreatedAt).ToListAsync().ConfigureAwait(false);
        }

        public async Task<int> SaveSensorAsync(Sensor sensor)
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            if (sensor.Id == 0)
                return await conn.InsertAsync(sensor).ConfigureAwait(false);
            else
                return await conn.UpdateAsync(sensor).ConfigureAwait(false);
        }

        public async Task<int> DeleteSensorAsync(Sensor sensor)
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            return await conn.DeleteAsync(sensor).ConfigureAwait(false);
        }

        // Apaga todos os eventos do calendário e todos os sensores
        public async Task ClearCalendarAndSensorsAsync()
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            int eventos = await conn.DeleteAllAsync<CalendarEvent>().ConfigureAwait(false);
            int sensores = await conn.DeleteAllAsync<Sensor>().ConfigureAwait(false);
            Log.Info("Agrosens.DB",
                $"ClearCalendarAndSensorsAsync: eventos removidos={eventos}, sensores removidos={sensores}");
        }

        // Apaga todos os eventos do calendário
        public async Task ClearCalendarAsync()
        {
            await InitializeAsync().ConfigureAwait(false);
            var conn = await GetConnectionAsync().ConfigureAwait(false);
            int eventos = await conn.DeleteAllAsync<CalendarEvent>().ConfigureAwait(false);
            Log.Info("Agrosens.DB",
                $"ClearCalendarAsync: eventos removidos={eventos}");
        }

        private async Task<int> RemoverTarefaSementeAsync(SQLiteAsyncConnection conn)
        {
            try
            {
                return await conn.ExecuteAsync(
                    "DELETE FROM agro_tasks WHERE Title = ? AND CreatedBy = ?",
                    "Tarefa de demonstração", "Pedro").ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Log.Error("Agrosens.DB", $"Falha ao remover tarefa semeada: {ex}");
                return 0;
            }
        }
    }
}
