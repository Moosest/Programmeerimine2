using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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
        public async Task List_should_filter_by_search()
        {
            await DbContext.Invoices.AddAsync(new Invoice { InvoiceNo = "INV-ALPHA", InvoiceDate = DateTime.Now, DueDate = DateTime.Now, Subtotal = 10, Shipping = 0, Discount = 0, GrandTotal = 10 });
            await DbContext.Invoices.AddAsync(new Invoice { InvoiceNo = "INV-BETA", InvoiceDate = DateTime.Now, DueDate = DateTime.Now, Subtotal = 20, Shipping = 0, Discount = 0, GrandTotal = 20 });
            await DbContext.SaveChangesAsync();

            var handler = new ListInvoicesQueryHandler(DbContext);
            var query = new ListInvoicesQuery { Page = 1, PageSize = 10, Search = "ALPHA" };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("INV-ALPHA", result.Value.Results.First().InvoiceNo);
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

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveInvoiceCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveInvoiceCommand)null;
            var handler = new SaveInvoiceCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_save_new_invoice()
        {
            // Arrange
            var request = new SaveInvoiceCommand
            {
                Id = 0,
                InvoiceNo = "INV001",
                InvoiceDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                Subtotal = 1000m,
                Shipping = 50m,
                Discount = 0.1m,
                GrandTotal = 1090m
            };
            var handler = new SaveInvoiceCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var savedInvoice = await DbContext.Invoices.FirstOrDefaultAsync(i => i.InvoiceNo == "INV001");

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedInvoice);
            Assert.Equal("INV001", savedInvoice.InvoiceNo);
        }

        [Fact]
        public async Task Save_should_save_existing_invoice()
        {
            // Arrange
            var invoice = new Invoice
            {
                InvoiceNo = "INV002",
                InvoiceDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                Subtotal = 500m,
                Shipping = 25m,
                Discount = 0.05m,
                GrandTotal = 545m
            };
            await DbContext.Invoices.AddAsync(invoice);
            await DbContext.SaveChangesAsync();

            var request = new SaveInvoiceCommand
            {
                Id = invoice.Id,
                InvoiceNo = "INV002-UPDATED",
                InvoiceDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                Subtotal = 2000m,
                Shipping = 100m,
                Discount = 0.2m,
                GrandTotal = 2080m
            };
            var handler = new SaveInvoiceCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var updatedInvoice = await DbContext.Invoices.FindAsync(invoice.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(updatedInvoice);
            Assert.Equal("INV002-UPDATED", updatedInvoice.InvoiceNo);
        }

        [Fact]
        public async Task Save_should_not_fail_when_invoice_does_not_exist()
        {
            // Arrange
            var request = new SaveInvoiceCommand
            {
                Id = 999,
                InvoiceNo = "INV999",
                InvoiceDate = DateTime.Now,
                DueDate = DateTime.Now.AddDays(30),
                Subtotal = 500m,
                Shipping = 25m,
                Discount = 0.05m,
                GrandTotal = 545m
            };
            var handler = new SaveInvoiceCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("0123456789012345")]
        public void SaveValidator_should_return_false_when_invoice_no_is_invalid(string invoiceNo)
        {
            var validator = new SaveInvoiceCommandValidator(DbContext);
            var command = new SaveInvoiceCommand
            {
                InvoiceNo = invoiceNo,
                InvoiceDate = DateTime.Today,
                DueDate = DateTime.Today,
                Subtotal = 100,
                Shipping = 10,
                Discount = 0.1m,
                GrandTotal = 110
            };

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveInvoiceCommand.InvoiceNo), result.Errors.First().PropertyName);
        }

        [Fact]
        public void SaveValidator_should_return_false_when_due_date_is_before_invoice_date()
        {
            var validator = new SaveInvoiceCommandValidator(DbContext);
            var command = new SaveInvoiceCommand
            {
                InvoiceNo = "INV-1",
                InvoiceDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(-1),
                Subtotal = 100,
                Shipping = 10,
                Discount = 0.1m,
                GrandTotal = 110
            };

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, x => x.PropertyName == nameof(SaveInvoiceCommand.DueDate));
        }

        [Theory]
        [InlineData(-0.01)]
        [InlineData(0.91)]
        public void SaveValidator_should_return_false_when_discount_is_invalid(decimal discount)
        {
            var validator = new SaveInvoiceCommandValidator(DbContext);
            var command = new SaveInvoiceCommand
            {
                InvoiceNo = "INV-2",
                InvoiceDate = DateTime.Today,
                DueDate = DateTime.Today,
                Subtotal = 100,
                Shipping = 10,
                Discount = discount,
                GrandTotal = 110
            };

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, x => x.PropertyName == nameof(SaveInvoiceCommand.Discount));
        }

        [Fact]
        public void SaveValidator_should_return_true_when_command_is_valid()
        {
            var validator = new SaveInvoiceCommandValidator(DbContext);
            var command = new SaveInvoiceCommand
            {
                Id = 0,
                InvoiceNo = "INV-100",
                InvoiceDate = DateTime.Today,
                DueDate = DateTime.Today.AddDays(14),
                Subtotal = 200,
                Shipping = 20,
                Discount = 0.1m,
                GrandTotal = 220
            };

            var result = validator.Validate(command);

            Assert.True(result.IsValid);
        }
    }
}
