using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Clients
{
    public class SaveClientsCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public string Name {get; set; }
        public string Email {get; set; }
        public string Phone {get; set; }
        public string Address {get; set; }
        public decimal Discount {get; set; }
    }
}
