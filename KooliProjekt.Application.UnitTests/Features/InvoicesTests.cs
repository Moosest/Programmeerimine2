using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Invoices;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class InvoiceTests : TestBase
    {
        [Fact]
        public void Get_throws_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetInvoiceQueryHandler(null);
            });
        }
        [Fact]
        public async Task Get_throws_if_request_is_null()
        {
            var handler = new GetInvoiceQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_not_query_db_when_id_is_zero_or_negative(int id)
        {
            // Arrange
            var query = new GetInvoiceQuery { Id = id };
            var handler = new GetInvoiceQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
            Assert.Equal(0, result.Value.Id);
        }

        [Fact]
        public async Task Get_should_return_object_if_object_exists()
        {
            // Arrange
            var query = new GetInvoiceQuery { Id = 1 };
            var invoice = new Invoice { InvoiceNo = "Test Invoice" };
            var handler = new GetInvoiceQueryHandler(DbContext);
            await DbContext.Invoices.AddAsync(invoice);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.HasErrors);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Get_should_return_null_if_object_does_not_exist()
        {
            // Arrange
            var query = new GetInvoiceQuery { Id = 101 };
            var handler = new GetInvoiceQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }

        [Fact]
        public void List_throws_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListInvoicesQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_throws_if_request_is_null()
        {
            var handler = new ListInvoicesQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task List_throws_if_page_is_zero_or_negative(int page)
        {
            var handler = new ListInvoicesQueryHandler(DbContext);
            var query = new ListInvoicesQuery { Page = page, PageSize = 5 };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task List_throws_if_pagesize_is_zero_or_negative(int pageSize)
        {
            var handler = new ListInvoicesQueryHandler(DbContext);
            var query = new ListInvoicesQuery { Page = 1, PageSize = pageSize };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task List_throws_if_pagesize_exceeds_maximum()
        {
            var handler = new ListInvoicesQueryHandler(DbContext);
            var query = new ListInvoicesQuery { Page = 1, PageSize = ListInvoicesQueryHandler.MaxPageSize + 1 };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            var dbContext = (ApplicationDbContext)null;
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteInvoiceCommandHandler(dbContext);
            });

            Assert.Equal(nameof(dbContext), exception.ParamName);
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteInvoiceCommand)null;
            var handler = new DeleteInvoiceCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Delete_should_return_when_request_id_is_zero_or_negative(int id)
        {
            // Arrange
            var command = new DeleteInvoiceCommand { Id = id };
            var handler = new DeleteInvoiceCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_return_when_invoice_does_not_exist()
        {
            // Arrange
            var command = new DeleteInvoiceCommand { Id = 999 };
            var handler = new DeleteInvoiceCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_invoice()
        {
            // Arrange
            var invoice = new Invoice { InvoiceNo = "Test Invoice", InvoiceDate = DateTime.Now, DueDate = DateTime.Now, Subtotal = 100, Shipping = 10, Discount = 0.1m, GrandTotal = 99 };
            await DbContext.Invoices.AddAsync(invoice);
            await DbContext.SaveChangesAsync();

            var command = new DeleteInvoiceCommand { Id = invoice.Id };
            var handler = new DeleteInvoiceCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            var deletedInvoice = await DbContext.Invoices.FindAsync(invoice.Id);
            Assert.Null(deletedInvoice);
        }

        [Fact]
        public async Task Delete_should_delete_related_invoice_lines()
        {
            // Arrange
            var invoice = new Invoice { InvoiceNo = "Test Invoice", InvoiceDate = DateTime.Now, DueDate = DateTime.Now, Subtotal = 100, Shipping = 10, Discount = 0.1m, GrandTotal = 99 };
            await DbContext.Invoices.AddAsync(invoice);
            await DbContext.SaveChangesAsync();

            var invoiceLine = new InvoiceLine { InvoiceId = invoice.Id, LineItem = "Item 1", UnitPrice = 50, Quantity = 1, VatRate = 0.2m, Discount = 0, Total = 50 };
            await DbContext.InvoiceLines.AddAsync(invoiceLine);
            await DbContext.SaveChangesAsync();

            var command = new DeleteInvoiceCommand { Id = invoice.Id };
            var handler = new DeleteInvoiceCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            var deletedInvoiceLine = await DbContext.InvoiceLines.FindAsync(invoiceLine.Id);
            Assert.Null(deletedInvoiceLine);
        }

        [Fact]
        public async Task Delete_should_delete_invoice_and_all_related_invoice_lines()
        {
            // Arrange
            var invoice = new Invoice { InvoiceNo = "Test Invoice", InvoiceDate = DateTime.Now, DueDate = DateTime.Now, Subtotal = 100, Shipping = 10, Discount = 0.1m, GrandTotal = 99 };
            await DbContext.Invoices.AddAsync(invoice);
            await DbContext.SaveChangesAsync();

            var invoiceLine1 = new InvoiceLine { InvoiceId = invoice.Id, LineItem = "Item 1", UnitPrice = 50, Quantity = 1, VatRate = 0.2m, Discount = 0, Total = 50 };
            var invoiceLine2 = new InvoiceLine { InvoiceId = invoice.Id, LineItem = "Item 2", UnitPrice = 25, Quantity = 2, VatRate = 0.2m, Discount = 0, Total = 50 };
            
            await DbContext.InvoiceLines.AddAsync(invoiceLine1);
            await DbContext.InvoiceLines.AddAsync(invoiceLine2);
            await DbContext.SaveChangesAsync();

            var command = new DeleteInvoiceCommand { Id = invoice.Id };
            var handler = new DeleteInvoiceCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            
            var deletedInvoice = await DbContext.Invoices.FindAsync(invoice.Id);
            Assert.Null(deletedInvoice);
            
            var deletedInvoiceLine1 = await DbContext.InvoiceLines.FindAsync(invoiceLine1.Id);
            Assert.Null(deletedInvoiceLine1);
            
            var deletedInvoiceLine2 = await DbContext.InvoiceLines.FindAsync(invoiceLine2.Id);
            Assert.Null(deletedInvoiceLine2);
        }
    }
}
