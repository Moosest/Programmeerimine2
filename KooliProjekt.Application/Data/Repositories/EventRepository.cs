using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;

namespace KooliProjekt.Application.Data.Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<PagedResult<Event>> ListAsync(int page, int pageSize)
        {
            return await DbContext
                .Events
                .OrderBy(e => e.Name)
                .GetPagedAsync(page, pageSize);
        }
    }
}
