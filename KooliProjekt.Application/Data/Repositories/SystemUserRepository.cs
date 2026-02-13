using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;

namespace KooliProjekt.Application.Data.Repositories
{
    public class SystemUserRepository : BaseRepository<SystemUser>, ISystemUserRepository
    {
        public SystemUserRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<PagedResult<SystemUser>> ListAsync(int page, int pageSize)
        {
            return await DbContext
                .SystemUsers
                .OrderBy(su => su.Username)
                .GetPagedAsync(page, pageSize);
        }
    }
}
