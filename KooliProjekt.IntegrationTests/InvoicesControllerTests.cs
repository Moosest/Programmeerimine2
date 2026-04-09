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
    public class InvoicesControllerTests : TestBase
    {
        [Theory]
        [InlineData("/api/Invoices?Page=1&PageSize=10")]
        public async Task List_Invoices_ReturnsSuccess(string url)
        {
            var response = await Client.GetAsync(url);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_Invoice_ReturnsSuccess()
        {
            // Create an invoice first
            var command = new
            {
                Id = 0,
                InvoiceNo = "INV-GET",
                InvoiceDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(30),
                Subtotal = 100.00m,
                Shipping = 5.00m,
                Discount = 0.1m,
                GrandTotal = 95.50m
            };
            var json = JsonConvert.SerializeObject(command);
            await Client.PostAsync("/api/Invoices/Save", new StringContent(json, Encoding.UTF8, "application/json"));

            var response = await Client.GetAsync("/api/Invoices/Get?id=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Save_Invoice_ReturnsSuccess()
        {
            var command = new
            {
                Id = 0,
                InvoiceNo = "INV-001",
                InvoiceDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(30),
                Subtotal = 100.00m,
                Shipping = 5.00m,
                Discount = 0.1m,
                GrandTotal = 95.50m
            };

            var json = JsonConvert.SerializeObject(command);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/api/Invoices/Save", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_Invoice_ReturnsSuccess()
        {
            // First create an invoice to delete
            var command = new
            {
                Id = 0,
                InvoiceNo = "INV-DEL",
                InvoiceDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(30),
                Subtotal = 50.00m,
                Shipping = 0.00m,
                Discount = 0.0m,
                GrandTotal = 50.00m
            };

            var json = JsonConvert.SerializeObject(command);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            await Client.PostAsync("/api/Invoices/Save", content);

            // Then delete
            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/Invoices/Delete")
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { Id = 1 }), Encoding.UTF8, "application/json")
            };

            var response = await Client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
