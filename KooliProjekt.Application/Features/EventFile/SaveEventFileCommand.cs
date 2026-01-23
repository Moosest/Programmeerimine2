using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;

namespace KooliProjekt.Application.Features.EventFiles
{
    public class SaveEventFileCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}