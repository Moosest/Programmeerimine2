using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IClientRepository
    {
        Task<Client> GetByIdAsync(int id);
        Task SaveAsync(Client entity);
        Task DeleteAsync(Client entity);
    }
}
