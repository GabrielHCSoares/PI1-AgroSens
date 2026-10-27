using SQLite;

namespace Agrosens.Models
{
    [Table("calendar_events")]
    public class CalendarEvent
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [NotNull, MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1000)]
        public string Description { get; set; } = string.Empty;

        public DateTime EventDate { get; set; }

        public TimeSpan StartTime { get; set; } = TimeSpan.Zero;

        public TimeSpan EndTime { get; set; } = TimeSpan.Zero;

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string CreatedBy { get; set; } = string.Empty;

        public int Status { get; set; } = 0;
    }
}
