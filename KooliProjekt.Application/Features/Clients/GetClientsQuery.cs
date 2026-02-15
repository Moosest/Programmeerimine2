using KooliProjekt.Application.Dto;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Clients
{
    public class GetClientsQuery : IRequest<OperationResult<ClientDetailsDto>>
    {
        public int Id { get; set; }
        public string Name {get; set; }
        public string Email {get; set; }
        public string Phone {get; set; }
        public string Address {get; set; }
        public decimal Discount {get; set; }
    }
}

























