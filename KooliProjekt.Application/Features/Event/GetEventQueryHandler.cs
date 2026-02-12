using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Events
{
    public class GetEventQueryHandler : IRequestHandler<GetEventQuery, OperationResult<object>>
    {
        private readonly IEventRepository _eventRepository;

        public GetEventQueryHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<OperationResult<object>> Handle(GetEventQuery request, CancellationToken cancellationToken)
        {
            var result = new OperationResult<object>();

            var e = await _eventRepository.GetByIdAsync(request.Id);
            if (e != null)
            {
                result.Value = new
                {
                    e.Id,
                    e.StartTime,
                    e.Description,
                    e.Location,
                    e.MaxSeats,
                    e.Price,
                    e.Summary,
                    e.IsActive
                };
            }
            else
            {
                result.Value = null;
            }

            return result;
        }
    }
}
