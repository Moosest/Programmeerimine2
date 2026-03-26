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
        public async Task Get_throws_if_request_is_null()
        {
            var handler = new GetEventFileQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));
        }
        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task Get_should_not_query_db_when_id_is_zero_or_negative(int id)
        {
            // Arrange
            var query = new GetEventFileQuery { Id = id };
            var handler = new GetEventFileQueryHandler(DbContext);

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
            var query = new GetEventFileQuery { Id = 1 };
            var eventFile = new EventFile { FileName = "Test EventFile", FilePath = "/test/path" };
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

        [Fact]
        public void List_throws_if_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new ListEventFilesQueryHandler(null);
            });
        }

        [Fact]
        public async Task List_throws_if_request_is_null()
        {
            var handler = new ListEventFilesQueryHandler(DbContext);
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                handler.Handle(null, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task List_throws_if_page_is_zero_or_negative(int page)
        {
            var handler = new ListEventFilesQueryHandler(DbContext);
            var query = new ListEventFilesQuery { Page = page, PageSize = 5 };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task List_throws_if_pagesize_is_zero_or_negative(int pageSize)
        {
            var handler = new ListEventFilesQueryHandler(DbContext);
            var query = new ListEventFilesQuery { Page = 1, PageSize = pageSize };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task List_throws_if_pagesize_exceeds_maximum()
        {
            var handler = new ListEventFilesQueryHandler(DbContext);
            var query = new ListEventFilesQuery { Page = 1, PageSize = ListEventFilesQueryHandler.MaxPageSize + 1 };
            await Assert.ThrowsAsync<ArgumentException>(() =>
                handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task List_should_filter_by_search()
        {
            await DbContext.EventFiles.AddAsync(new EventFile { EventId = 1, FilePath = "/docs/agenda.pdf", FileName = "agenda.pdf", UploadedAt = DateTime.Now });
            await DbContext.EventFiles.AddAsync(new EventFile { EventId = 1, FilePath = "/docs/poster.png", FileName = "poster.png", UploadedAt = DateTime.Now });
            await DbContext.SaveChangesAsync();

            var handler = new ListEventFilesQueryHandler(DbContext);
            var query = new ListEventFilesQuery { Page = 1, PageSize = 10, Search = "agenda" };

            var result = await handler.Handle(query, CancellationToken.None);

            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Results);
            Assert.Equal("agenda.pdf", result.Value.Results.First().FileName);
        }

        [Fact]
        public void Delete_should_throw_when_dbcontext_is_null()
        {
            var dbContext = (ApplicationDbContext)null;
            var exception = Assert.Throws<ArgumentNullException>(() =>
            {
                new DeleteEventFileCommandHandler(dbContext);
            });

            Assert.Equal(nameof(dbContext), exception.ParamName);
        }

        [Fact]
        public async Task Delete_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (DeleteEventFileCommand)null;
            var handler = new DeleteEventFileCommandHandler(DbContext);

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
            var command = new DeleteEventFileCommand { Id = id };
            var handler = new DeleteEventFileCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_return_when_eventfile_does_not_exist()
        {
            // Arrange
            var command = new DeleteEventFileCommand { Id = 999 };
            var handler = new DeleteEventFileCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public async Task Delete_should_delete_existing_eventfile()
        {
            // Arrange
            var eventFile = new EventFile { EventId = 1, FileName = "Test File", FilePath = "/test/path", UploadedAt = DateTime.Now };
            await DbContext.EventFiles.AddAsync(eventFile);
            await DbContext.SaveChangesAsync();

            var command = new DeleteEventFileCommand { Id = eventFile.Id };
            var handler = new DeleteEventFileCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            var deletedEventFile = await DbContext.EventFiles.FindAsync(eventFile.Id);
            Assert.Null(deletedEventFile);
        }

        [Fact]
        public void Save_should_throw_when_dbcontext_is_null()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new SaveEventFileCommandHandler(null);
            });
        }

        [Fact]
        public async Task Save_should_throw_when_request_is_null()
        {
            // Arrange
            var request = (SaveEventFileCommand)null;
            var handler = new SaveEventFileCommandHandler(DbContext);

            // Act && Assert
            var ex = await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                await handler.Handle(request, CancellationToken.None);
            });
            Assert.Equal("request", ex.ParamName);
        }

        [Fact]
        public async Task Save_should_save_new_eventfile()
        {
            // Arrange
            var request = new SaveEventFileCommand
            {
                Id = 0,
                EventId = 1,
                FilePath = "/files/newfile.pdf",
                FileName = "newfile.pdf",
                UploadedAt = DateTime.Now
            };
            var handler = new SaveEventFileCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var savedEventFile = await DbContext.EventFiles.FirstOrDefaultAsync(f => f.FileName == "newfile.pdf");

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(savedEventFile);
            Assert.Equal(1, savedEventFile.EventId);
        }

        [Fact]
        public async Task Save_should_save_existing_eventfile()
        {
            // Arrange
            var eventFile = new EventFile { EventId = 1, FilePath = "/files/oldfile.pdf", FileName = "oldfile.pdf", UploadedAt = DateTime.Now };
            await DbContext.EventFiles.AddAsync(eventFile);
            await DbContext.SaveChangesAsync();

            var request = new SaveEventFileCommand
            {
                Id = eventFile.Id,
                EventId = 2,
                FilePath = "/files/updated.pdf",
                FileName = "updated.pdf",
                UploadedAt = DateTime.Now
            };
            var handler = new SaveEventFileCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);
            var updatedEventFile = await DbContext.EventFiles.FindAsync(eventFile.Id);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
            Assert.NotNull(updatedEventFile);
            Assert.Equal("updated.pdf", updatedEventFile.FileName);
        }

        [Fact]
        public async Task Save_should_not_fail_when_eventfile_does_not_exist()
        {
            // Arrange
            var request = new SaveEventFileCommand
            {
                Id = 999,
                EventId = 1,
                FilePath = "/files/file.pdf",
                FileName = "file.pdf",
                UploadedAt = DateTime.Now
            };
            var handler = new SaveEventFileCommandHandler(DbContext);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasErrors);
        }

        [Fact]
        public void SaveValidator_should_return_false_when_event_id_is_invalid()
        {
            var validator = new SaveEventFileCommandValidator(DbContext);
            var command = new SaveEventFileCommand { EventId = 0, FilePath = "/a", FileName = "a.txt", UploadedAt = DateTime.Now };

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Equal(nameof(SaveEventFileCommand.EventId), result.Errors.First().PropertyName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public void SaveValidator_should_return_false_when_file_path_is_invalid(string filePath)
        {
            var validator = new SaveEventFileCommandValidator(DbContext);
            var command = new SaveEventFileCommand { EventId = 1, FilePath = filePath, FileName = "a.txt", UploadedAt = DateTime.Now };

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, x => x.PropertyName == nameof(SaveEventFileCommand.FilePath));
        }

        [Fact]
        public void SaveValidator_should_return_false_when_file_path_is_too_long()
        {
            var validator = new SaveEventFileCommandValidator(DbContext);
            var command = new SaveEventFileCommand
            {
                EventId = 1,
                FilePath = new string('a', 501),
                FileName = "a.txt",
                UploadedAt = DateTime.Now
            };

            var result = validator.Validate(command);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, x => x.PropertyName == nameof(SaveEventFileCommand.FilePath));
        }

        [Fact]
        public void SaveValidator_should_return_true_when_command_is_valid()
        {
            var validator = new SaveEventFileCommandValidator(DbContext);
            var command = new SaveEventFileCommand
            {
                Id = 0,
                EventId = 1,
                FilePath = "/files/file.pdf",
                FileName = "file.pdf",
                UploadedAt = DateTime.Now
            };

            var result = validator.Validate(command);

            Assert.True(result.IsValid);
        }
    }
}
