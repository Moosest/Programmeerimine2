using System.Threading.Tasks;

namespace KooliProjekt.Application.Data.Repositories
{
    public interface IPaymentRepository
    {
        Task<Payment> GetByIdAsync(int id);
        Task SaveAsync(Payment entity);
        Task DeleteAsync(Payment entity);
    }
}
