using System.Threading.Tasks;
using KooliProjekt.Application.Infrastructure.Paging;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IEventScheduleRepository
    {
        Task<EventSchedule> GetByIdAsync(int id);
        Task SaveAsync(EventSchedule entity);
        Task DeleteAsync(EventSchedule entity);
        Task<PagedResult<EventSchedule>> ListAsync(int page, int pageSize);
    }
}
