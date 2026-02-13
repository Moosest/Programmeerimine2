using System.Threading.Tasks;
using KooliProjekt.Application.Infrastructure.Paging;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IEventRepository
    {
        Task<Event> GetByIdAsync(int id);
        Task SaveAsync(Event entity);
        Task DeleteAsync(Event entity);
        Task<PagedResult<Event>> ListAsync(int page, int pageSize);
    }
}
