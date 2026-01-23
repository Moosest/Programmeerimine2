using System.Threading.Tasks;
using KooliProjekt.Application.Features.Events;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class EventsController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public EventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ListEventsQuery query)
        {
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetEventQuery { Id = id };
            var response = await _mediator.Send(query);
            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveEventCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }
    }
}
