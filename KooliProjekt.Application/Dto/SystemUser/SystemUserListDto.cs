using System;

namespace KooliProjekt.Application.Dto
{
    public class SystemUserItemDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDone { get; set; }
    }
}
