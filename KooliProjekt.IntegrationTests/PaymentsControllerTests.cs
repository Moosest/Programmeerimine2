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
    public class PaymentsControllerTests : TestBase
    {
        [Theory]
        [InlineData("/api/Payments?Page=1&PageSize=10")]
        public async Task List_Payments_ReturnsSuccess(string url)
        {
            var response = await Client.GetAsync(url);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Get_Payment_ReturnsSuccess()
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

            // Create payment
            var command = new
            {
                Id = 0,
                InvoiceId = 1,
                Amount = 95.50m,
                PaymentDate = DateTime.UtcNow,
                Method = "Credit Card",
                TransactionRef = "TXN-GET",
                ModifiedBy = 1
            };
            await Client.PostAsync("/api/Payments/Save", new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));

            var response = await Client.GetAsync("/api/Payments/Get?id=1");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Save_Payment_ReturnsSuccess()
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
                Amount = 95.50m,
                PaymentDate = DateTime.UtcNow,
                Method = "Credit Card",
                TransactionRef = "TXN-001",
                ModifiedBy = 1
            };

            var json = JsonConvert.SerializeObject(command);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/api/Payments/Save", content);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task Delete_Payment_ReturnsSuccess()
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

            // Create payment to delete
            var command = new
            {
                Id = 0,
                InvoiceId = 1,
                Amount = 50.00m,
                PaymentDate = DateTime.UtcNow,
                Method = "Cash",
                TransactionRef = "TXN-DEL",
                ModifiedBy = 1
            };
            await Client.PostAsync("/api/Payments/Save", new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json"));

            // Then delete
            var request = new HttpRequestMessage(HttpMethod.Delete, "/api/Payments/Delete")
            {
                Content = new StringContent(JsonConvert.SerializeObject(new { Id = 1 }), Encoding.UTF8, "application/json")
            };

            var response = await Client.SendAsync(request);
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}
