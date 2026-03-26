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
using KooliProjekt.Application.Features.Payments;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class PaymentTests : TestBase
    {
        [Fact]
        public void Get_throws_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetPaymentQueryHandler(null);
            });
        }
        [Fact]
        public async Task Get_throws_if_request_is_null()
        {
            var handler = new GetPaymentQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_not_query_db_when_id_is_zero_or_negative(int id)
        {
            // Arrange
            var query = new GetPaymentQuery { Id = id };
            var handler = new GetPaymentQueryHandler(DbContext);

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
            var query = new GetPaymentQuery { Id = 1 };
            var payment = new Payment { Method = "Test Payment", TransactionRef = "TX001" };
            var handler = new GetPaymentQueryHandler(DbContext);
            await DbContext.Payments.AddAsync(payment);
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
            var query = new GetPaymentQuery { Id = 101 };
            var handler = new GetPaymentQueryHandler(DbContext);

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
                new ListPaymentsQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_throws_if_request_is_null()
        {
            var handler = new ListPaymentsQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task List_throws_if_page_is_zero_or_negative(int page)
        {
            var handler = new ListPaymentsQueryHandler(DbContext);
            var query = new ListPaymentsQuery { Page = page, PageSize = 5 };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task List_throws_if_pagesize_is_zero_or_negative(int pageSize)
        {
            var handler = new ListPaymentsQueryHandler(DbContext);
            var query = new ListPaymentsQuery { Page = 1, PageSize = pageSize };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task List_throws_if_pagesize_exceeds_maximum()
        {
            var handler = new ListPaymentsQueryHandler(DbContext);
            var query = new ListPaymentsQuery { Page = 1, PageSize = ListPaymentsQueryHandler.MaxPageSize + 1 };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            var dbContext = (ApplicationDbContext)null;
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new DeletePaymentCommandHandler(dbContext);
            });

            Assert.Equal(nameof(dbContext), exception.ParamName);
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeletePaymentCommand)null;
            var handler = new DeletePaymentCommandHandler(DbContext);

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
            var command = new DeletePaymentCommand { Id = id };
            var handler = new DeletePaymentCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_return_when_payment_does_not_exist()
        {
            // Arrange
            var command = new DeletePaymentCommand { Id = 999 };
            var handler = new DeletePaymentCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_payment()
        {
            // Arrange
            var payment = new Payment { Method = "Test Payment", TransactionRef = "TX001", Amount = 100, PaymentDate = DateTime.Now };
            await DbContext.Payments.AddAsync(payment);
            await DbContext.SaveChangesAsync();

            var command = new DeletePaymentCommand { Id = payment.Id };
            var handler = new DeletePaymentCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            var deletedPayment = await DbContext.Payments.FindAsync(payment.Id);
            Assert.Null(deletedPayment);
        }

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SavePaymentCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SavePaymentCommand)null;
            var handler = new SavePaymentCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_save_new_payment()
        {
            // Arrange
            var request = new SavePaymentCommand
            {
                Id = 0,
                InvoiceId = 1,
                Amount = 500m,
                PaymentDate = DateTime.Now,
                Method = "Credit Card",
                TransactionRef = "TXN001",
                ModifiedBy = 1
            };
            var handler = new SavePaymentCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var savedPayment = await DbContext.Payments.FirstOrDefaultAsync(p => p.TransactionRef == "TXN001");

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedPayment);
            Assert.Equal("TXN001", savedPayment.TransactionRef);
        }

        [Fact]
        public async Task Save_should_save_existing_payment()
        {
            // Arrange
            var payment = new Payment { InvoiceId = 1, Amount = 100m, PaymentDate = DateTime.Now, Method = "Cash", TransactionRef = "TXN002", ModifiedBy = 1 };
            await DbContext.Payments.AddAsync(payment);
            await DbContext.SaveChangesAsync();

            var request = new SavePaymentCommand
            {
                Id = payment.Id,
                InvoiceId = 2,
                Amount = 200m,
                PaymentDate = DateTime.Now,
                Method = "Check",
                TransactionRef = "TXN002-UPDATED",
                ModifiedBy = 1
            };
            var handler = new SavePaymentCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var updatedPayment = await DbContext.Payments.FindAsync(payment.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(updatedPayment);
            Assert.Equal("Check", updatedPayment.Method);
        }

        [Fact]
        public async Task Save_should_not_fail_when_payment_does_not_exist()
        {
            // Arrange
            var request = new SavePaymentCommand
            {
                Id = 999,
                InvoiceId = 1,
                Amount = 500m,
                PaymentDate = DateTime.Now,
                Method = "Credit Card",
                TransactionRef = "TXN999",
                ModifiedBy = 1
            };
            var handler = new SavePaymentCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public void SaveValidator_should_return_false_when_invoice_id_is_invalid()
        {
            var validator = new SavePaymentCommandValidator(DbContext);
            var command = new SavePaymentCommand { InvoiceId = 0, Amount = 10, Method = "Card", TransactionRef = "TX1", ModifiedBy = 1, PaymentDate = DateTime.Now };

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Equal(nameof(SavePaymentCommand.InvoiceId), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("0123456789012345678901234567890123456789012345678901")]
        public void SaveValidator_should_return_false_when_method_is_invalid(string method)
        {
            var validator = new SavePaymentCommandValidator(DbContext);
            var command = new SavePaymentCommand { InvoiceId = 1, Amount = 10, Method = method, TransactionRef = "TX1", ModifiedBy = 1, PaymentDate = DateTime.Now };

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, x => x.PropertyName == nameof(SavePaymentCommand.Method));
        }

        [Fact]
        public void SaveValidator_should_return_false_when_modified_by_is_invalid()
        {
            var validator = new SavePaymentCommandValidator(DbContext);
            var command = new SavePaymentCommand { InvoiceId = 1, Amount = 10, Method = "Card", TransactionRef = "TX1", ModifiedBy = 0, PaymentDate = DateTime.Now };

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, x => x.PropertyName == nameof(SavePaymentCommand.ModifiedBy));
        }

        [Fact]
        public void SaveValidator_should_return_true_when_command_is_valid()
        {
            var validator = new SavePaymentCommandValidator(DbContext);
            var command = new SavePaymentCommand
            {
                Id = 0,
                InvoiceId = 1,
                Amount = 100,
                Method = "Card",
                TransactionRef = "TX-123",
                ModifiedBy = 2,
                PaymentDate = DateTime.Now
            };

            var result = validator.Validate(command);

            Assert.True(result.IsValid);
        }
    }
}
