using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IEventFileRepository
    {
        Task<EventFile> GetByIdAsync(int id);
        Task SaveAsync(EventFile entity);
        Task DeleteAsync(EventFile entity);
    }
}
