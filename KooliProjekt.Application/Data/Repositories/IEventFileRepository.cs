using System.Threading.Tasks;
using KooliProjekt.Application.Infrastructure.Paging;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IEventFileRepository
    {
        Task<EventFile> GetByIdAsync(int id);
        Task SaveAsync(EventFile entity);
        Task DeleteAsync(EventFile entity);
        Task<PagedResult<EventFile>> ListAsync(int page, int pageSize);
    }
}
