using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
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
        public async Task Get_should_return_object_if_object_exists()
        {
            // Arrange 
            var query = new GetToDoListQuery { Id = 1 };
            var client = new Client { Title = "Test Client" };
            var handler = new GetClientsQueryHandler(DbContext);
            await DbContext.Clients.AddAsync(toDoList);
            await DbContext.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert 
            Assert.False(result.HasErrors);
            Assert.NotNull(result.HasErrors);
            Assert.Equal(1, result.Value.Id);
        }

        [Fact]
        public async Task Get_should_return_null_if_object_does_not_exist()
        {
            // Arrange
            var query = new GetClientQuery { Id = 101 };
            var ClientList = new ClientList { Title = "Test Client List" };
            var handler = new GetClientQueryHandler(DbContext);
            await DbContext.Clients.SaveChangesAsync();

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}

