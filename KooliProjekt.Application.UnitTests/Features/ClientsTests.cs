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
using KooliProjekt.Application.Features.Clients;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class ClientTests : TestBase
    {
        [Fact]
        public void Get_throws_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetClientsQueryHandler(null);
            });
        }
        [Fact]
        public async Task Get_throws_if_request_is_null()
        {
            var handler = new GetClientsQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_not_query_db_when_id_is_zero_or_negative(int id)
        {
            // Arrange
            var query = new GetClientsQuery { Id = id };
            var handler = new GetClientsQueryHandler(DbContext);

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
            var query = new GetClientsQuery { Id = 1 };
            var client = new Client { Name = "Test Client", Email = "test@test.com", Phone = "1234567", Address = "Test St 1" };
            var handler = new GetClientsQueryHandler(DbContext);
            await DbContext.Clients.AddAsync(client);
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
            var query = new GetClientsQuery { Id = 101 };
            var handler = new GetClientsQueryHandler(DbContext);

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
                new ListClientsQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_throws_if_request_is_null()
        {
            var handler = new ListClientsQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task List_throws_if_page_is_zero_or_negative(int page)
        {
            var handler = new ListClientsQueryHandler(DbContext);
            var query = new ListClientsQuery { Page = page, PageSize = 5 };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task List_throws_if_pagesize_is_zero_or_negative(int pageSize)
        {
            var handler = new ListClientsQueryHandler(DbContext);
            var query = new ListClientsQuery { Page = 1, PageSize = pageSize };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task List_throws_if_pagesize_exceeds_maximum()
        {
            var handler = new ListClientsQueryHandler(DbContext);
            var query = new ListClientsQuery { Page = 1, PageSize = ListClientsQueryHandler.MaxPageSize + 1 };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            var dbContext = (ApplicationDbContext)null;
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteClientCommandHandler(dbContext);
            });

            Assert.Equal(nameof(dbContext), exception.ParamName);
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteClientCommand)null;
            var handler = new DeleteClientCommandHandler(DbContext);

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
            var command = new DeleteClientCommand { Id = id };
            var handler = new DeleteClientCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_return_when_client_does_not_exist()
        {
            // Arrange
            var command = new DeleteClientCommand { Id = 999 };
            var handler = new DeleteClientCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_client()
        {
            // Arrange
            var client = new Client { Name = "Test Client", Email = "test@test.com", Phone = "1234567", Address = "Test St 1" };
            await DbContext.Clients.AddAsync(client);
            await DbContext.SaveChangesAsync();

            var command = new DeleteClientCommand { Id = client.Id };
            var handler = new DeleteClientCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            var deletedClient = await DbContext.Clients.FindAsync(client.Id);
            Assert.Null(deletedClient);
        }

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveClientsCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveClientsCommand)null;
            var handler = new SaveClientsCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_save_new_client()
        {
            // Arrange
            var request = new SaveClientsCommand
            {
                Id = 0,
                Name = "New Client",
                Email = "newclient@test.com",
                Phone = "1234567890",
                Address = "123 Main St",
                Discount = 0.1m
            };
            var handler = new SaveClientsCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var savedClient = await DbContext.Clients.FirstOrDefaultAsync(c => c.Name == "New Client");

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedClient);
            Assert.Equal("New Client", savedClient.Name);
        }

        [Fact]
        public async Task Save_should_save_existing_client()
        {
            // Arrange
            var client = new Client { Name = "Old Client", Email = "old@test.com", Phone = "9876543210", Address = "456 Oak Ave", Discount = 0.05m };
            await DbContext.Clients.AddAsync(client);
            await DbContext.SaveChangesAsync();

            var request = new SaveClientsCommand
            {
                Id = client.Id,
                Name = "Updated Client",
                Email = "updated@test.com",
                Phone = "5555555555",
                Address = "789 Elm Rd",
                Discount = 0.15m
            };
            var handler = new SaveClientsCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var updatedClient = await DbContext.Clients.FindAsync(client.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(updatedClient);
            Assert.Equal("Updated Client", updatedClient.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("01234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890")]
        public void SaveValidator_should_return_false_when_name_is_invalid(string name)
        {
            var validator = new SaveClientsCommandValidator(DbContext);
            var command = new SaveClientsCommand { Name = name, Email = "client@test.com", Phone = "123456", Address = "Addr", Discount = 0.1m };

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveClientsCommand.Name), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData("not-an-email")]
        [InlineData("")]
        public void SaveValidator_should_return_false_when_email_is_invalid(string email)
        {
            var validator = new SaveClientsCommandValidator(DbContext);
            var command = new SaveClientsCommand { Name = "Client", Email = email, Phone = "123456", Address = "Addr", Discount = 0.1m };

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, x => x.PropertyName == nameof(SaveClientsCommand.Email));
        }

        [Theory]
        [InlineData(-0.01)]
        [InlineData(0.91)]
        public void SaveValidator_should_return_false_when_discount_is_invalid(decimal discount)
        {
            var validator = new SaveClientsCommandValidator(DbContext);
            var command = new SaveClientsCommand { Name = "Client", Email = "client@test.com", Phone = "123456", Address = "Addr", Discount = discount };

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, x => x.PropertyName == nameof(SaveClientsCommand.Discount));
        }

        [Fact]
        public void SaveValidator_should_return_true_when_command_is_valid()
        {
            var validator = new SaveClientsCommandValidator(DbContext);
            var command = new SaveClientsCommand
            {
                Id = 0,
                Name = "Client",
                Email = "client@test.com",
                Phone = "1234567",
                Address = "Tallinn",
                Discount = 0.2m
            };

            var result = validator.Validate(command);

            Assert.True(result.IsValid);
        }
    }
}
