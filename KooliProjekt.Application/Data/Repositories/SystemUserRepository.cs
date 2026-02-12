using System.Threading.Tasks;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Data.Repositories
{
    public class SystemUserRepository : BaseRepository<SystemUser>, ISystemUserRepository
    {
        public SystemUserRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
