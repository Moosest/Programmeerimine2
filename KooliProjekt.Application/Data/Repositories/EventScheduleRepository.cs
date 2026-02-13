using System.Linq;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;

namespace KooliProjekt.Application.Data.Repositories
{
    public class EventScheduleRepository : BaseRepository<EventSchedule>, IEventScheduleRepository
    {
        public EventScheduleRepository(ApplicationDbContext dbContext) : base(dbContext) { }

        public async Task<PagedResult<EventSchedule>> ListAsync(int page, int pageSize)
        {
            return await DbContext
                .EventSchedules
                .OrderBy(es => es.StartTime)
                .GetPagedAsync(page, pageSize);
        }
    }
}
