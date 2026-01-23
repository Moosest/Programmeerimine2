using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.SystemUsers
{
    public class SaveSystemUserCommandHandler : IRequestHandler<SaveSystemUserCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveSystemUserCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveSystemUserCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();
            SystemUser user;
            if (request.Id == 0)
            {
                user = new SystemUser();
                await _dbContext.SystemUsers.AddAsync(user, cancellationToken);
            }
            else
            {
                user = await _dbContext.SystemUsers.FindAsync(new object[] { request.Id }, cancellationToken);
                if (user == null)
                {
                    return result;
                }
            }
            user.Username = request.Username;
            user.PasswordHash = request.PasswordHash;
            user.Role = request.Role;
            user.CreatedAt = request.CreatedAt;
            await _dbContext.SaveChangesAsync(cancellationToken);
            return result;
        }
    }
}