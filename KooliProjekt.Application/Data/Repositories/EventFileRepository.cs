using System.Threading.Tasks;
using KooliProjekt.Application.Data;

namespace KooliProjekt.Application.Data.Repositories
{
    public class EventFileRepository : BaseRepository<EventFile>, IEventFileRepository
    {
        public EventFileRepository(ApplicationDbContext dbContext) : base(dbContext) { }
    }
}
