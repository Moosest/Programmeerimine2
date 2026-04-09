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
    public class SystemUsersControllerTests : TestBase
    {
        [Theory]
        [InlineData("/api/SystemUsers?Page=1&PageSize=10")]
        public async Task List_SystemUsers_ReturnsSuccess(string url)
        {
            var response = await Client.GetAsync(url);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_SystemUser_ReturnsSuccess()
        {
            // Create a system user first
            var command = new
            {
                Id = 0,
                Username = "getuser",
                PasswordHash = "hashedpassword",
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            };
            var json = JsonConvert.SerializeObject(command);
            await Client.PostAsync("/api/SystemUsers/Save", new StringContent(json, Encoding.UTF8, "application/json"));

            var response = await Client.GetAsync("/api/SystemUsers/Get?id=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Save_SystemUser_ReturnsSuccess()
        {
            var command = new
            {
                Id = 0,
                Username = "testuser",
                PasswordHash = "hashedpassword123",
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            };

            var json = JsonConvert.SerializeObject(command);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/api/SystemUsers/Save", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_SystemUser_ReturnsSuccess()
        {
            // First create a user to delete
            var command = new
            {
                Id = 0,
                Username = "deleteuser",
                PasswordHash = "hashedpassword456",
                Role = "User",
                CreatedAt = DateTime.UtcNow
            };

            var json = JsonConvert.SerializeObject(command);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await Client.PostAsync("/api/SystemUsers/Save", content);

            // Then delete
            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/SystemUsers/Delete")
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { Id = 1 }), Encoding.UTF8, "application/json")
            };

            var response = await Client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
