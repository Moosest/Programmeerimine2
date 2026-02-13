using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;

namespace KooliProjekt.Application.Data.Repositories
{
    public class ClientRepository : BaseRepository<Client>, IClientRepository
    {
        public ClientRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<PagedResult<Client>> ListAsync(int page, int pageSize)
        {
            return await DbContext
                .Clients
                .OrderBy(c => c.Name)
                .GetPagedAsync(page, pageSize);
        }
    }
}
