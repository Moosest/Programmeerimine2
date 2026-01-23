using System.Diagnostics.Contracts;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Clients
{
    public class GetClientsQuery : IRequest<OperationResult<object>>
    {
        public int Id { get; set; }
        public string Name {get; set; }
        public string Email {get; set; }
        public string Phone {get; set; }
        public string Address {get; set; }
        public decimal Discount {get; set; }
    }
}

























