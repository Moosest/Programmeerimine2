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
    public class EventFilesControllerTests : TestBase
    {
        [Theory]
        [InlineData("/api/EventFiles?Page=1&PageSize=10")]
        public async Task List_EventFiles_ReturnsSuccess(string url)
        {
            var response = await Client.GetAsync(url);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_EventFile_ReturnsSuccess()
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

            // Create event file
            var command = new
            {
                Id = 0,
                EventId = 1,
                FilePath = "/files/get.pdf",
                FileName = "get.pdf",
                UploadedAt = DateTime.UtcNow
            };
            await Client.PostAsync("/api/EventFiles/Save", new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));

            var response = await Client.GetAsync("/api/EventFiles/Get?id=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Save_EventFile_ReturnsSuccess()
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
            var eventJson = JsonConvert.SerializeObject(eventCommand);
            await Client.PostAsync("/api/Events/Save", new StringContent(eventJson, Encoding.UTF8, "application/json"));

            var command = new
            {
                Id = 0,
                EventId = 1,
                FilePath = "/files/test.pdf",
                FileName = "test.pdf",
                UploadedAt = DateTime.UtcNow
            };

            var json = JsonConvert.SerializeObject(command);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/api/EventFiles/Save", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_EventFile_ReturnsSuccess()
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

            // Create event file to delete
            var command = new
            {
                Id = 0,
                EventId = 1,
                FilePath = "/files/delete.pdf",
                FileName = "delete.pdf",
                UploadedAt = DateTime.UtcNow
            };
            await Client.PostAsync("/api/EventFiles/Save", new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));

            // Then delete
            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/EventFiles/Delete")
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { Id = 1 }), Encoding.UTF8, "application/json")
            };

            var response = await Client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
