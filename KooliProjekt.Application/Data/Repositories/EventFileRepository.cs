using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;

namespace KooliProjekt.Application.Data.Repositories
{
    public class EventFileRepository : BaseRepository<EventFile>, IEventFileRepository
    {
        public EventFileRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<PagedResult<EventFile>> ListAsync(int page, int pageSize)
        {
            return await DbContext
                .EventFiles
                .OrderBy(ef => ef.FileName)
                .GetPagedAsync(page, pageSize);
        }
    }
}
