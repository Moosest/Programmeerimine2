using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Features.Events;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class EventTests : TestBase
    {
        [Fact]
        public void Get_throws_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetEventQueryHandler(null);
            });
        }
        [Fact]
        public async Task Get_throws_if_request_is_null()
        {
            var handler = new GetEventQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_not_query_db_when_id_is_zero_or_negative(int id)
        {
            // Arrange
            var query = new GetEventQuery { Id = id };
            var handler = new GetEventQueryHandler(DbContext);

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
            var query = new GetEventQuery { Id = 1 };
            var @event = new Event { Name = "Test Event", Description = "Desc", Location = "Loc", Summary = "Sum" };
            var handler = new GetEventQueryHandler(DbContext);
            await DbContext.Events.AddAsync(@event);
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
            var query = new GetEventQuery { Id = 101 };
            var handler = new GetEventQueryHandler(DbContext);

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
                new ListEventsQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_throws_if_request_is_null()
        {
            var handler = new ListEventsQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task List_throws_if_page_is_zero_or_negative(int page)
        {
            var handler = new ListEventsQueryHandler(DbContext);
            var query = new ListEventsQuery { Page = page, PageSize = 5 };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task List_throws_if_pagesize_is_zero_or_negative(int pageSize)
        {
            var handler = new ListEventsQueryHandler(DbContext);
            var query = new ListEventsQuery { Page = 1, PageSize = pageSize };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task List_throws_if_pagesize_exceeds_maximum()
        {
            var handler = new ListEventsQueryHandler(DbContext);
            var query = new ListEventsQuery { Page = 1, PageSize = ListEventsQueryHandler.MaxPageSize + 1 };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            var dbContext = (ApplicationDbContext)null;
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteEventCommandHandler(dbContext);
            });

            Assert.Equal(nameof(dbContext), exception.ParamName);
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteEventCommand)null;
            var handler = new DeleteEventCommandHandler(DbContext);

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
            var command = new DeleteEventCommand { Id = id };
            var handler = new DeleteEventCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_return_when_event_does_not_exist()
        {
            // Arrange
            var command = new DeleteEventCommand { Id = 999 };
            var handler = new DeleteEventCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_event()
        {
            // Arrange
            var @event = new Event { Name = "Test Event", Description = "Desc", Location = "Loc", Summary = "Sum" };
            await DbContext.Events.AddAsync(@event);
            await DbContext.SaveChangesAsync();

            var command = new DeleteEventCommand { Id = @event.Id };
            var handler = new DeleteEventCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            var deletedEvent = await DbContext.Events.FindAsync(@event.Id);
            Assert.Null(deletedEvent);
        }

        [Fact]
        public async Task Delete_should_delete_related_event_files()
        {
            // Arrange
            var @event = new Event { Name = "Test Event", Description = "Desc", Location = "Loc", Summary = "Sum" };
            await DbContext.Events.AddAsync(@event);
            await DbContext.SaveChangesAsync();

            var eventFile = new EventFile { EventId = @event.Id, FilePath = "/path", FileName = "file.txt", UploadedAt = DateTime.Now };
            await DbContext.EventFiles.AddAsync(eventFile);
            await DbContext.SaveChangesAsync();

            var command = new DeleteEventCommand { Id = @event.Id };
            var handler = new DeleteEventCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            var deletedEventFile = await DbContext.EventFiles.FindAsync(eventFile.Id);
            Assert.Null(deletedEventFile);
        }

        [Fact]
        public async Task Delete_should_delete_related_event_schedules()
        {
            // Arrange
            var @event = new Event { Name = "Test Event", Description = "Desc", Location = "Loc", Summary = "Sum" };
            await DbContext.Events.AddAsync(@event);
            await DbContext.SaveChangesAsync();

            var schedule = new EventSchedule { EventId = @event.Id, StartTime = DateTime.Now, FilePath = "/path", FileName = "schedule.txt", UploadedAt = DateTime.Now };
            await DbContext.EventSchedules.AddAsync(schedule);
            await DbContext.SaveChangesAsync();

            var command = new DeleteEventCommand { Id = @event.Id };
            var handler = new DeleteEventCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            var deletedSchedule = await DbContext.EventSchedules.FindAsync(schedule.Id);
            Assert.Null(deletedSchedule);
        }

        [Fact]
        public async Task Delete_should_delete_event_and_all_related_entities()
        {
            // Arrange
            var @event = new Event { Name = "Test Event", Description = "Desc", Location = "Loc", Summary = "Sum" };
            await DbContext.Events.AddAsync(@event);
            await DbContext.SaveChangesAsync();

            var eventFile = new EventFile { EventId = @event.Id, FilePath = "/path", FileName = "file.txt", UploadedAt = DateTime.Now };
            var schedule = new EventSchedule { EventId = @event.Id, StartTime = DateTime.Now, FilePath = "/path", FileName = "schedule.txt", UploadedAt = DateTime.Now };
            
            await DbContext.EventFiles.AddAsync(eventFile);
            await DbContext.EventSchedules.AddAsync(schedule);
            await DbContext.SaveChangesAsync();

            var command = new DeleteEventCommand { Id = @event.Id };
            var handler = new DeleteEventCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            
            var deletedEvent = await DbContext.Events.FindAsync(@event.Id);
            Assert.Null(deletedEvent);
            
            var deletedEventFile = await DbContext.EventFiles.FindAsync(eventFile.Id);
            Assert.Null(deletedEventFile);
            
            var deletedSchedule = await DbContext.EventSchedules.FindAsync(schedule.Id);
            Assert.Null(deletedSchedule);
        }
    }
}
