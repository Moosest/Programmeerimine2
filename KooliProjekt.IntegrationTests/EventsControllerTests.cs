using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using KooliProjekt.IntegrationTests.Helpers;
using Newtonsoft.Json;
using Xunit;

namespace KooliProjekt.IntegrationTests
{
    public class EventsControllerTests : TestBase
    {
        [Theory]
        [InlineData("/api/Events?Page=1&PageSize=10")]
        public async Task List_Events_ReturnsSuccess(string url)
        {
            var response = await Client.GetAsync(url);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_Event_ReturnsSuccess()
        {
            // Create an event first
            var command = new
            {
                Id = 0,
                Name = "Get Test Event",
                StartTime = DateTime.UtcNow.AddDays(1),
                Description = "Get Test",
                Location = "Get Location",
                MaxSeats = 100,
                Price = 10.00m,
                Summary = "Get Summary",
                IsActive = true
            };
            var json = JsonConvert.SerializeObject(command);
            await Client.PostAsync("/api/Events/Save", new StringContent(json, Encoding.UTF8, "application/json"));

            var response = await Client.GetAsync("/api/Events/Get?id=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Save_Event_ReturnsSuccess()
        {
            var command = new
            {
                Id = 0,
                Name = "Test Event",
                StartTime = DateTime.UtcNow.AddDays(1),
                Description = "Test Description",
                Location = "Test Location",
                MaxSeats = 100,
                Price = 25.00m,
                Summary = "Test Summary",
                IsActive = true
            };

            var json = JsonConvert.SerializeObject(command);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/api/Events/Save", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_Event_ReturnsSuccess()
        {
            // First create an event to delete
            var command = new
            {
                Id = 0,
                Name = "To Delete",
                StartTime = DateTime.UtcNow.AddDays(1),
                Description = "Delete Description",
                Location = "Delete Location",
                MaxSeats = 50,
                Price = 10.00m,
                Summary = "Delete Summary",
                IsActive = true
            };

            var json = JsonConvert.SerializeObject(command);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await Client.PostAsync("/api/Events/Save", content);

            // Then delete
            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/Events/Delete")
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { Id = 1 }), Encoding.UTF8, "application/json")
            };

            var response = await Client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
