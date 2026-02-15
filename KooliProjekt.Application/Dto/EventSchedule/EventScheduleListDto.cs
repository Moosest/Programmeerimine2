using System;

namespace KooliProjekt.Application.Dto
{
    public class EventScheduleItemDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public DateTime StartTime { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public DateTime UploadedAt { get; set; }
        public bool IsDone { get; set; }
    }
}
