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
    public class InvoiceLinesControllerTests : TestBase
    {
        [Theory]
        [InlineData("/api/InvoiceLines?Page=1&PageSize=10")]
        public async Task List_InvoiceLines_ReturnsSuccess(string url)
        {
            var response = await Client.GetAsync(url);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_InvoiceLine_ReturnsSuccess()
        {
            // Create invoice first
            var invoiceCommand = new
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
            await Client.PostAsync("/api/Invoices/Save", new StringContent(JsonConvert.SerializeObject(invoiceCommand), Encoding.UTF8, "application/json"));

            // Create invoice line
            var command = new
            {
                Id = 0,
                InvoiceId = 1,
                LineItem = "Get Test Item",
                UnitPrice = 25.00m,
                Quantity = 1.0m,
                VatRate = 0.2m,
                Discount = 0.0m,
                Total = 30.00m
            };
            await Client.PostAsync("/api/InvoiceLines/Save", new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));

            var response = await Client.GetAsync("/api/InvoiceLines/Get?id=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Save_InvoiceLine_ReturnsSuccess()
        {
            // First create an invoice for the foreign key
            var invoiceCommand = new
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
            await Client.PostAsync("/api/Invoices/Save", new StringContent(JsonConvert.SerializeObject(invoiceCommand), Encoding.UTF8, "application/json"));

            var command = new
            {
                Id = 0,
                InvoiceId = 1,
                LineItem = "Test Item",
                UnitPrice = 25.00m,
                Quantity = 2.0m,
                VatRate = 0.2m,
                Discount = 0.0m,
                Total = 60.00m
            };

            var json = JsonConvert.SerializeObject(command);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/api/InvoiceLines/Save", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_InvoiceLine_ReturnsSuccess()
        {
            // Create invoice first
            var invoiceCommand = new
            {
                Id = 0,
                InvoiceNo = "INV-002",
                InvoiceDate = DateTime.UtcNow,
                DueDate = DateTime.UtcNow.AddDays(30),
                Subtotal = 50.00m,
                Shipping = 0.00m,
                Discount = 0.0m,
                GrandTotal = 50.00m
            };
            await Client.PostAsync("/api/Invoices/Save", new StringContent(JsonConvert.SerializeObject(invoiceCommand), Encoding.UTF8, "application/json"));

            // Create invoice line to delete
            var command = new
            {
                Id = 0,
                InvoiceId = 1,
                LineItem = "Delete Item",
                UnitPrice = 10.00m,
                Quantity = 1.0m,
                VatRate = 0.2m,
                Discount = 0.0m,
                Total = 12.00m
            };
            await Client.PostAsync("/api/InvoiceLines/Save", new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));

            // Then delete
            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/InvoiceLines/Delete")
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { Id = 1 }), Encoding.UTF8, "application/json")
            };

            var response = await Client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
