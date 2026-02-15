using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class ClientDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Address { get; set; }

        public decimal Discount { get; set; }
        public List<ClientItemDto> Items { get; set; } = new List<ClientItemDto>();
    }
}