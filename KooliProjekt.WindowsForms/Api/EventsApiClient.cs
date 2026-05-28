using KooliProjekt.WindowsForms.Api;

namespace KooliProjekt.WindowsForms
{
    public class EventsApiClient : CrudApiClientBase<Event>, IEventsApiClient
    {
        public EventsApiClient(HttpClient httpClient)
            : base(httpClient, "api/Events/")
        {
        }
    }
}
