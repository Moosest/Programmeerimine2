using System.Threading.Tasks;
using KooliProjekt.Application.Features.Payments;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class PaymentsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public PaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ListPaymentsQuery query)
        {
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetPaymentQuery { Id = id };
            var response = await _mediator.Send(query);
            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SavePaymentCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }
    }
}
