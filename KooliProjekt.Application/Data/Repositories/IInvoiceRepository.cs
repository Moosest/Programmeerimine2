using System.Threading.Tasks;
using KooliProjekt.Application.Infrastructure.Paging;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IInvoiceRepository
    {
        Task<Invoice> GetByIdAsync(int id);
        Task SaveAsync(Invoice entity);
        Task DeleteAsync(Invoice entity);
        Task<PagedResult<Invoice>> ListAsync(int page, int pageSize);
    }
}
