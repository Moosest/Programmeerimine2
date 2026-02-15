using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.EventFiles;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class EventFileTests : TestBase
    {
        [Fact]
        public void Get_throws_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetEventFileQueryHandler(null);
            });
        }
        [Fact]
        public async Task Get_should_return_object_if_object_exists()
        {
            // Arrange
            var query = new GetEventFileQuery { Id = 1 };
            var eventFile = new EventFile { FileName = "Test EventFile" };
            var handler = new GetEventFileQueryHandler(DbContext);
            await DbContext.EventFiles.AddAsync(eventFile);
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
            var query = new GetEventFileQuery { Id = 101 };
            var handler = new GetEventFileQueryHandler(DbContext);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.False(result.HasErrors);
            Assert.Null(result.Value);
        }
    }
}
