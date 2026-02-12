using System.Threading.Tasks;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Data.Repositories
{
    public class InvoiceLineRepository : BaseRepository<InvoiceLine>, IInvoiceLineRepository
    {
        public InvoiceLineRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
