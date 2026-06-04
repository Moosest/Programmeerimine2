using KooliProjekt.WpfApplication;

namespace KooliProjekt.WpfApplication
{
    public class EventSchedulesApiClient : CrudApiClientBase<EventSchedule>, IEventSchedulesApiClient
    {
        public EventSchedulesApiClient(HttpClient httpClient)
            : base(httpClient, "api/EventSchedules/")
        {
        }
    }
}

