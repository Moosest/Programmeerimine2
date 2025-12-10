using System.Threading.Tasks;
using KooliProjekt.Application.Features.EventSchedules;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class EventSchedulesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public EventSchedulesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ListEventSchedulesQuery query)
        {
            var response = await _mediator.Send(query);

            return Result(response);
        }
    }
}
