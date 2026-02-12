using System.Threading.Tasks;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Data.Repositories
{
    public class PaymentRepository : BaseRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
