using System.Threading.Tasks;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Data.Repositories
{
    public class EventScheduleRepository : BaseRepository<EventSchedule>, IEventScheduleRepository
    {
        public EventScheduleRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
