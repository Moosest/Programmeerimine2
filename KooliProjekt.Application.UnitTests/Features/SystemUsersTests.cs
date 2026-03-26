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
using KooliProjekt.Application.Features.SystemUsers;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class SystemUserTests : TestBase
    {
        [Fact]
        public void Get_throws_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetSystemUserQueryHandler(null);
            });
        }
        [Fact]
        public async Task Get_throws_if_request_is_null()
        {
            var handler = new GetSystemUserQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_not_query_db_when_id_is_zero_or_negative(int id)
        {
            // Arrange
            var query = new GetSystemUserQuery { Id = id };
            var handler = new GetSystemUserQueryHandler(DbContext);

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
            var query = new GetSystemUserQuery { Id = 1 };
            var systemUser = new SystemUser { Username = "Test SystemUser", PasswordHash = "hash", Role = "Admin" };
            var handler = new GetSystemUserQueryHandler(DbContext);
            await DbContext.SystemUsers.AddAsync(systemUser);
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
            var query = new GetSystemUserQuery { Id = 101 };
            var handler = new GetSystemUserQueryHandler(DbContext);

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
                new ListSystemUsersQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_throws_if_request_is_null()
        {
            var handler = new ListSystemUsersQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task List_throws_if_page_is_zero_or_negative(int page)
        {
            var handler = new ListSystemUsersQueryHandler(DbContext);
            var query = new ListSystemUsersQuery { Page = page, PageSize = 5 };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task List_throws_if_pagesize_is_zero_or_negative(int pageSize)
        {
            var handler = new ListSystemUsersQueryHandler(DbContext);
            var query = new ListSystemUsersQuery { Page = 1, PageSize = pageSize };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task List_throws_if_pagesize_exceeds_maximum()
        {
            var handler = new ListSystemUsersQueryHandler(DbContext);
            var query = new ListSystemUsersQuery { Page = 1, PageSize = ListSystemUsersQueryHandler.MaxPageSize + 1 };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task List_should_filter_by_search()
        {
            await DbContext.SystemUsers.AddAsync(new SystemUser { Username = "admin.user", PasswordHash = "h", Role = "Admin", CreatedAt = DateTime.Now });
            await DbContext.SystemUsers.AddAsync(new SystemUser { Username = "guest.user", PasswordHash = "h", Role = "Guest", CreatedAt = DateTime.Now });
            await DbContext.SaveChangesAsync();

            var handler = new ListSystemUsersQueryHandler(DbContext);
            var query = new ListSystemUsersQuery { Page = 1, PageSize = 10, Search = "admin" };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("admin.user", result.Value.Results.First().Username);
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            var dbContext = (ApplicationDbContext)null;
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteSystemUserCommandHandler(dbContext);
            });

            Assert.Equal(nameof(dbContext), exception.ParamName);
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteSystemUserCommand)null;
            var handler = new DeleteSystemUserCommandHandler(DbContext);

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
            var command = new DeleteSystemUserCommand { Id = id };
            var handler = new DeleteSystemUserCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_return_when_systemuser_does_not_exist()
        {
            // Arrange
            var command = new DeleteSystemUserCommand { Id = 999 };
            var handler = new DeleteSystemUserCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_systemuser()
        {
            // Arrange
            var systemUser = new SystemUser { Username = "testuser", PasswordHash = "hash123", Role = "Admin" };
            await DbContext.SystemUsers.AddAsync(systemUser);
            await DbContext.SaveChangesAsync();

            var command = new DeleteSystemUserCommand { Id = systemUser.Id };
            var handler = new DeleteSystemUserCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            var deletedSystemUser = await DbContext.SystemUsers.FindAsync(systemUser.Id);
            Assert.Null(deletedSystemUser);
        }

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveSystemUserCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveSystemUserCommand)null;
            var handler = new SaveSystemUserCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_save_new_systemuser()
        {
            // Arrange
            var request = new SaveSystemUserCommand
            {
                Id = 0,
                Username = "newuser",
                PasswordHash = "hash123",
                Role = "Admin",
                CreatedAt = DateTime.Now
            };
            var handler = new SaveSystemUserCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var savedUser = await DbContext.SystemUsers.FirstOrDefaultAsync(u => u.Username == "newuser");

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedUser);
            Assert.Equal("newuser", savedUser.Username);
        }

        [Fact]
        public async Task Save_should_save_existing_systemuser()
        {
            // Arrange
            var user = new SystemUser { Username = "olduser", PasswordHash = "oldhash", Role = "User", CreatedAt = DateTime.Now };
            await DbContext.SystemUsers.AddAsync(user);
            await DbContext.SaveChangesAsync();

            var request = new SaveSystemUserCommand
            {
                Id = user.Id,
                Username = "updateduser",
                PasswordHash = "newhash",
                Role = "Admin",
                CreatedAt = DateTime.Now
            };
            var handler = new SaveSystemUserCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var updatedUser = await DbContext.SystemUsers.FindAsync(user.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(updatedUser);
            Assert.Equal("Admin", updatedUser.Role);
        }

        [Fact]
        public async Task Save_should_not_fail_when_systemuser_does_not_exist()
        {
            // Arrange
            var request = new SaveSystemUserCommand
            {
                Id = 999,
                Username = "testuser",
                PasswordHash = "testhash",
                Role = "User",
                CreatedAt = DateTime.Now
            };
            var handler = new SaveSystemUserCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("012345678901234567890123456789012345678901234567890")]
        public void SaveValidator_should_return_false_when_username_is_invalid(string username)
        {
            var validator = new SaveSystemUserCommandValidator(DbContext);
            var command = new SaveSystemUserCommand { Username = username, PasswordHash = "hash", Role = "User", CreatedAt = DateTime.Now };

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveSystemUserCommand.Username), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SaveValidator_should_return_false_when_password_hash_is_invalid(string passwordHash)
        {
            var validator = new SaveSystemUserCommandValidator(DbContext);
            var command = new SaveSystemUserCommand { Username = "user", PasswordHash = passwordHash, Role = "User", CreatedAt = DateTime.Now };

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, x => x.PropertyName == nameof(SaveSystemUserCommand.PasswordHash));
        }

        [Fact]
        public void SaveValidator_should_return_true_when_command_is_valid()
        {
            var validator = new SaveSystemUserCommandValidator(DbContext);
            var command = new SaveSystemUserCommand
            {
                Id = 0,
                Username = "admin",
                PasswordHash = "hash123",
                Role = "Admin",
                CreatedAt = DateTime.Now
            };

            var result = validator.Validate(command);

            Assert.True(result.IsValid);
        }
    }
}
