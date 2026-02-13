using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;

namespace KooliProjekt.Application.Data.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<PagedResult<Payment>> ListAsync(int page, int pageSize)
        {
            return await DbContext
                .Payments
                .OrderBy(p => p.PaymentDate)
                .GetPagedAsync(page, pageSize);
        }
    }
}
