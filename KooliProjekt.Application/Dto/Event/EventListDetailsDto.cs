using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class EventDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DateTime StartTime { get; set; }

        public string Description { get; set; }

        public string Location { get; set; }

        public int MaxSeats { get; set; }

        public decimal Price { get; set; }

        public string Summary { get; set; }

        public bool IsActive { get; set; }
        public List<EventItemDto> Items { get; set; } = new List<EventItemDto>();
    }
}
