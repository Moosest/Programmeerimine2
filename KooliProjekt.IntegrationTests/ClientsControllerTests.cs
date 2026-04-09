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
    public class ClientsControllerTests : TestBase
    {
        [Theory]
        [InlineData("/api/Clients?Page=1&PageSize=10")]
        public async Task List_Clients_ReturnsSuccess(string url)
        {
            var response = await Client.GetAsync(url);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_Client_ReturnsSuccess()
        {
            // Create a client first
            var command = new
            {
                Id = 0,
                Name = "Get Test",
                Email = "get@example.com",
                Phone = "1234567890",
                Address = "Get Address",
                Discount = 0.0m
            };
            var json = JsonConvert.SerializeObject(command);
            await Client.PostAsync("/api/Clients/Save", new StringContent(json, Encoding.UTF8, "application/json"));

            var response = await Client.GetAsync("/api/Clients/Get?id=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Save_Client_ReturnsSuccess()
        {
            var command = new
            {
                Id = 0,
                Name = "Test Client",
                Email = "test@example.com",
                Phone = "1234567890",
                Address = "Test Address 1",
                Discount = 0.1m
            };

            var json = JsonConvert.SerializeObject(command);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/api/Clients/Save", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_Client_ReturnsSuccess()
        {
            // First create a client to delete
            var command = new
            {
                Id = 0,
                Name = "To Delete",
                Email = "delete@example.com",
                Phone = "1234567890",
                Address = "Delete Address",
                Discount = 0.0m
            };

            var json = JsonConvert.SerializeObject(command);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await Client.PostAsync("/api/Clients/Save", content);

            // Then delete
            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/Clients/Delete")
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { Id = 1 }), Encoding.UTF8, "application/json")
            };

            var response = await Client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
