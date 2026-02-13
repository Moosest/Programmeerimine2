using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Events
{
    public class SaveEventCommandHandler : IRequestHandler<SaveEventCommand, OperationResult>
    {
        private readonly IEventRepository _eventRepository;

        public SaveEventCommandHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<OperationResult> Handle(SaveEventCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var list = new Event();
            if (request.Id != 0)
            {
                list = await _eventRepository.GetByIdAsync(request.Id);
            }

            list.Name = request.Name;
            list.StartTime = request.StartTime;
            list.Description = request.Description;
            list.Location = request.Location;
            list.MaxSeats = request.MaxSeats;
            list.Price = request.Price;
            list.Summary = request.Summary;
            list.IsActive = request.IsActive;

            await _eventRepository.SaveAsync(list);

            return result;
        }
    }
}
