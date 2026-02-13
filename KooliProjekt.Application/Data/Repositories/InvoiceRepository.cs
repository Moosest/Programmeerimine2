using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;

namespace KooliProjekt.Application.Data.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<PagedResult<Invoice>> ListAsync(int page, int pageSize)
        {
            return await DbContext
                .Invoices
                .OrderBy(i => i.InvoiceNo)
                .GetPagedAsync(page, pageSize);
        }
    }
}
