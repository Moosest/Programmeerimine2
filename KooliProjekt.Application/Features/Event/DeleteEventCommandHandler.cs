using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Events
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand, OperationResult>
    {
        private readonly IEventRepository _eventRepository;

        public DeleteEventCommandHandler(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<OperationResult> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var entity = await _eventRepository.GetByIdAsync(request.Id);
            if (entity != null)
            {
                await _eventRepository.DeleteAsync(entity);
            }

            return result;
        }
    }
}
