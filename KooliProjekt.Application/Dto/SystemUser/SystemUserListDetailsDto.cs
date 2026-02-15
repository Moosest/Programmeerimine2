using System;
using System.Collections.Generic;

namespace KooliProjekt.Application.Dto
{
    public class SystemUserDetailsDto
    {
        public int Id { get; set; }
        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public string Role { get; set; }

        public DateTime CreatedAt { get; set; }
        public List<SystemUserItemDto> Items { get; set; } = new List<SystemUserItemDto>();
    }
}
