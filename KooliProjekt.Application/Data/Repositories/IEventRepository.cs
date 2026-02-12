using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IEventRepository
    {
        Task<Event> GetByIdAsync(int id);
        Task SaveAsync(Event entity);
        Task DeleteAsync(Event entity);
    }
}
