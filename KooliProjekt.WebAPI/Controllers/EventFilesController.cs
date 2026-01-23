using System.Threading.Tasks;
using KooliProjekt.Application.Features.EventFiles;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class EventFilesController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public EventFilesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ListEventFilesQuery query)
        {
            var response = await _mediator.Send(query);

            return Result(response);
        }

        [HttpGet]
        [Route("Get")]
        public async Task<IActionResult> Get(int id)
        {
            var query = new GetEventFileQuery { Id = id };
            var response = await _mediator.Send(query);
            return Result(response);
        }

        [HttpPost]
        [Route("Save")]
        public async Task<IActionResult> Save(SaveEventFileCommand command)
        {
            var response = await _mediator.Send(command);
            return Result(response);
        }
    }
}
