using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;

namespace KooliProjekt.Application.Features.EventSchedules
{
    public class SaveEventScheduleCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public DateTime StartTime { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}