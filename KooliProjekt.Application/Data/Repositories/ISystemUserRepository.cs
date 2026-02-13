using System.Threading.Tasks;
using KooliProjekt.Application.Infrastructure.Paging;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface ISystemUserRepository
    {
        Task<SystemUser> GetByIdAsync(int id);
        Task SaveAsync(SystemUser entity);
        Task DeleteAsync(SystemUser entity);
        Task<PagedResult<SystemUser>> ListAsync(int page, int pageSize);
    }
}
