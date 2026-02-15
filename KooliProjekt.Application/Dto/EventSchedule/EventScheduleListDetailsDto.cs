using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class EventScheduleDetailsDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }

        public DateTime StartTime { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public DateTime UploadedAt { get; set; }
        public List<EventScheduleItemDto> Items { get; set; } = new List<EventScheduleItemDto>();
    }
}
