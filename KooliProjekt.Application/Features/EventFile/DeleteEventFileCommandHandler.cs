using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data.Repositories;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.EventFiles
{
    public class DeleteEventFileCommandHandler : IRequestHandler<DeleteEventFileCommand, OperationResult>
    {
        private readonly IEventFileRepository _eventFileRepository;

        public DeleteEventFileCommandHandler(IEventFileRepository eventFileRepository)
        {
            _eventFileRepository = eventFileRepository;
        }

        public async Task<OperationResult> Handle(DeleteEventFileCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var entity = await _eventFileRepository.GetByIdAsync(request.Id);
            if (entity != null)
            {
                await _eventFileRepository.DeleteAsync(entity);
            }

            return result;
        }
    }
}
