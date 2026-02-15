using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class EventFileDetailsDto
    {
        public int Id { get; set; }
        public int EventId { get; set; }

        public string FilePath { get; set; }

        public string FileName { get; set; }

        public DateTime UploadedAt { get; set; }
        public List<EventFileItemDto> Items { get; set; } = new List<EventFileItemDto>();
    }
}
