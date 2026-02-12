using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IInvoiceLineRepository
    {
        Task<InvoiceLine> GetByIdAsync(int id);
        Task SaveAsync(InvoiceLine entity);
        Task DeleteAsync(InvoiceLine entity);
    }
}
