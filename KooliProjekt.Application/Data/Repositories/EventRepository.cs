using System.Threading.Tasks;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Data.Repositories
{
    public class EventRepository : BaseRepository<Event>, IEventRepository
    {
        public EventRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
