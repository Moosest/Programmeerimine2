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
using KooliProjekt.Application.Features.EventSchedules;
using Xunit;

namespace KooliProjekt.Application.UnitTests.Features
{
    public class EventScheduleTests : TestBase
    {
        [Fact]
        public void Get_throws_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GetEventScheduleQueryHandler(null);
            });
        }
        [Fact]
        public async Task Get_throws_if_request_is_null()
        {
            var handler = new GetEventScheduleQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_not_query_db_when_id_is_zero_or_negative(int id)
        {
            // Arrange
            var query = new GetEventScheduleQuery { Id = id };
            var handler = new GetEventScheduleQueryHandler(DbContext);

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
            var query = new GetEventScheduleQuery { Id = 1 };
            var eventSchedule = new EventSchedule { FileName = "Test EventSchedule", FilePath = "/test/path" };
            var handler = new GetEventScheduleQueryHandler(DbContext);
            await DbContext.EventSchedules.AddAsync(eventSchedule);
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
            var query = new GetEventScheduleQuery { Id = 101 };
            var handler = new GetEventScheduleQueryHandler(DbContext);

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
                new ListEventSchedulesQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_throws_if_request_is_null()
        {
            var handler = new ListEventSchedulesQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task List_throws_if_page_is_zero_or_negative(int page)
        {
            var handler = new ListEventSchedulesQueryHandler(DbContext);
            var query = new ListEventSchedulesQuery { Page = page, PageSize = 5 };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task List_throws_if_pagesize_is_zero_or_negative(int pageSize)
        {
            var handler = new ListEventSchedulesQueryHandler(DbContext);
            var query = new ListEventSchedulesQuery { Page = 1, PageSize = pageSize };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task List_throws_if_pagesize_exceeds_maximum()
        {
            var handler = new ListEventSchedulesQueryHandler(DbContext);
            var query = new ListEventSchedulesQuery { Page = 1, PageSize = ListEventSchedulesQueryHandler.MaxPageSize + 1 };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            var dbContext = (ApplicationDbContext)null;
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteEventScheduleCommandHandler(dbContext);
            });

            Assert.Equal(nameof(dbContext), exception.ParamName);
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteEventScheduleCommand)null;
            var handler = new DeleteEventScheduleCommandHandler(DbContext);

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
            var command = new DeleteEventScheduleCommand { Id = id };
            var handler = new DeleteEventScheduleCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_return_when_eventschedule_does_not_exist()
        {
            // Arrange
            var command = new DeleteEventScheduleCommand { Id = 999 };
            var handler = new DeleteEventScheduleCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_eventschedule()
        {
            // Arrange
            var eventSchedule = new EventSchedule { EventId = 1, FileName = "Test Schedule", FilePath = "/test/path", StartTime = DateTime.Now, UploadedAt = DateTime.Now };
            await DbContext.EventSchedules.AddAsync(eventSchedule);
            await DbContext.SaveChangesAsync();

            var command = new DeleteEventScheduleCommand { Id = eventSchedule.Id };
            var handler = new DeleteEventScheduleCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            var deletedEventSchedule = await DbContext.EventSchedules.FindAsync(eventSchedule.Id);
            Assert.Null(deletedEventSchedule);
        }

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveEventScheduleCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveEventScheduleCommand)null;
            var handler = new SaveEventScheduleCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_save_new_eventschedule()
        {
            // Arrange
            var request = new SaveEventScheduleCommand
            {
                Id = 0,
                EventId = 1,
                StartTime = DateTime.Now.AddDays(1),
                FilePath = "/schedules/new.txt",
                FileName = "new.txt",
                UploadedAt = DateTime.Now
            };
            var handler = new SaveEventScheduleCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var savedSchedule = await DbContext.EventSchedules.FirstOrDefaultAsync(s => s.FileName == "new.txt");

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedSchedule);
            Assert.Equal(1, savedSchedule.EventId);
        }

        [Fact]
        public async Task Save_should_save_existing_eventschedule()
        {
            // Arrange
            var schedule = new EventSchedule { EventId = 1, StartTime = DateTime.Now, FilePath = "/schedules/old.txt", FileName = "old.txt", UploadedAt = DateTime.Now };
            await DbContext.EventSchedules.AddAsync(schedule);
            await DbContext.SaveChangesAsync();

            var request = new SaveEventScheduleCommand
            {
                Id = schedule.Id,
                EventId = 2,
                StartTime = DateTime.Now.AddDays(2),
                FilePath = "/schedules/updated.txt",
                FileName = "updated.txt",
                UploadedAt = DateTime.Now
            };
            var handler = new SaveEventScheduleCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var updatedSchedule = await DbContext.EventSchedules.FindAsync(schedule.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(updatedSchedule);
            Assert.Equal("updated.txt", updatedSchedule.FileName);
        }

        [Fact]
        public async Task Save_should_not_fail_when_eventschedule_does_not_exist()
        {
            // Arrange
            var request = new SaveEventScheduleCommand
            {
                Id = 999,
                EventId = 1,
                StartTime = DateTime.Now,
                FilePath = "/schedules/file.txt",
                FileName = "file.txt",
                UploadedAt = DateTime.Now
            };
            var handler = new SaveEventScheduleCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }
    }
}
