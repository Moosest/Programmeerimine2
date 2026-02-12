using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface ISystemUserRepository
    {
        Task<SystemUser> GetByIdAsync(int id);
        Task SaveAsync(SystemUser entity);
        Task DeleteAsync(SystemUser entity);
    }
}
