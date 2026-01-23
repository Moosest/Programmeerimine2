using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;

namespace KooliProjekt.Application.Features.SystemUsers
{
    public class SaveSystemUserCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}