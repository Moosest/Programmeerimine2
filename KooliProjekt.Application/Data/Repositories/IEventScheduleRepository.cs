using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IEventScheduleRepository
    {
        Task<EventSchedule> GetByIdAsync(int id);
        Task SaveAsync(EventSchedule entity);
        Task DeleteAsync(EventSchedule entity);
    }
}
