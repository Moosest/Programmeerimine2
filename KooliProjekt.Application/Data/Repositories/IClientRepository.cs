using System.Threading.Tasks;
using KooliProjekt.Application.Infrastructure.Paging;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IClientRepository
    {
        Task<Client> GetByIdAsync(int id);
        Task SaveAsync(Client entity);
        Task DeleteAsync(Client entity);
        Task<PagedResult<Client>> ListAsync(int page, int pageSize);
    }
}
