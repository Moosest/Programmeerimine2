using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public class EventSchedulesApiClient : CrudApiClientBase<EventSchedule>, IEventSchedulesApiClient
    {
        public EventSchedulesApiClient(HttpClient httpClient)
            : base(httpClient, "api/EventSchedules/")
        {
        }
    }
}
