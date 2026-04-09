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
    public class EventSchedulesControllerTests : TestBase
    {
        [Theory]
        [InlineData("/api/EventSchedules?Page=1&PageSize=10")]
        public async Task List_EventSchedules_ReturnsSuccess(string url)
        {
            var response = await Client.GetAsync(url);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_EventSchedule_ReturnsSuccess()
        {
            // Create event first
            var eventCommand = new
            {
                Id = 0,
                Name = "Get Event",
                StartTime = DateTime.UtcNow.AddDays(1),
                Description = "Test",
                Location = "Test",
                MaxSeats = 100,
                Price = 10.00m,
                Summary = "Test",
                IsActive = true
            };
            await Client.PostAsync("/api/Events/Save", new StringContent(JsonConvert.SerializeObject(eventCommand), Encoding.UTF8, "application/json"));

            // Create event schedule
            var command = new
            {
                Id = 0,
                EventId = 1,
                StartTime = DateTime.UtcNow.AddDays(2),
                FilePath = "/files/get.pdf",
                FileName = "get.pdf",
                UploadedAt = DateTime.UtcNow
            };
            await Client.PostAsync("/api/EventSchedules/Save", new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));

            var response = await Client.GetAsync("/api/EventSchedules/Get?id=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Save_EventSchedule_ReturnsSuccess()
        {
            // First create an event for the foreign key
            var eventCommand = new
            {
                Id = 0,
                Name = "Test Event",
                StartTime = DateTime.UtcNow.AddDays(1),
                Description = "Test",
                Location = "Test",
                MaxSeats = 100,
                Price = 10.00m,
                Summary = "Test",
                IsActive = true
            };
            await Client.PostAsync("/api/Events/Save", new StringContent(JsonConvert.SerializeObject(eventCommand), Encoding.UTF8, "application/json"));

            var command = new
            {
                Id = 0,
                EventId = 1,
                StartTime = DateTime.UtcNow.AddDays(2),
                FilePath = "/files/schedule.pdf",
                FileName = "schedule.pdf",
                UploadedAt = DateTime.UtcNow
            };

            var json = JsonConvert.SerializeObject(command);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/api/EventSchedules/Save", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_EventSchedule_ReturnsSuccess()
        {
            // Create event first
            var eventCommand = new
            {
                Id = 0,
                Name = "Test Event",
                StartTime = DateTime.UtcNow.AddDays(1),
                Description = "Test",
                Location = "Test",
                MaxSeats = 100,
                Price = 10.00m,
                Summary = "Test",
                IsActive = true
            };
            await Client.PostAsync("/api/Events/Save", new StringContent(JsonConvert.SerializeObject(eventCommand), Encoding.UTF8, "application/json"));

            // Create event schedule to delete
            var command = new
            {
                Id = 0,
                EventId = 1,
                StartTime = DateTime.UtcNow.AddDays(2),
                FilePath = "/files/delete.pdf",
                FileName = "delete.pdf",
                UploadedAt = DateTime.UtcNow
            };
            await Client.PostAsync("/api/EventSchedules/Save", new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));

            // Then delete
            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/EventSchedules/Delete")
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { Id = 1 }), Encoding.UTF8, "application/json")
            };

            var response = await Client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
