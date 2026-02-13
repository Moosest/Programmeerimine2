using System.Threading.Tasks;
using KooliProjekt.Application.Infrastructure.Paging;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> GetByIdAsync(int id);
        Task SaveAsync(Payment entity);
        Task DeleteAsync(Payment entity);
        Task<PagedResult<Payment>> ListAsync(int page, int pageSize);
    }
}
