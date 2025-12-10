using System.Threading.Tasks;
using KooliProjekt.Application.Features.SystemUsers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace KooliProjekt.WebAPI.Controllers
{
    public class SystemUsersController : ApiControllerBase
    {
        private readonly IMediator _mediator;

        public SystemUsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List([FromQuery] ListSystemUsersQuery query)
        {
            var response = await _mediator.Send(query);

            return Result(response);
        }
    }
}
