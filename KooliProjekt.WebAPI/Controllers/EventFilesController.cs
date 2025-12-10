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
    }
}
