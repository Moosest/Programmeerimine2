using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EventSchedules
{
    public class DeleteEventScheduleCommandHandler : IRequestHandler<DeleteEventScheduleCommand, OperationResult>
    {
        private readonly IEventScheduleRepository _eventScheduleRepository;

        public DeleteEventScheduleCommandHandler(IEventScheduleRepository eventScheduleRepository)
        {
            _eventScheduleRepository = eventScheduleRepository;
        }

        public async Task<OperationResult> Handle(DeleteEventScheduleCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var entity = await _eventScheduleRepository.GetByIdAsync(request.Id);
            if (entity != null)
            {
                await _eventScheduleRepository.DeleteAsync(entity);
            }

            return result;
        }
    }
}
