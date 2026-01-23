using KooliProjekt.Application.Behaviors;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.VisualBasic;
using System;

namespace KooliProjekt.Application.Features.Events
{
    public class SaveEventCommand : IRequest<OperationResult>, ITransactional
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartTime { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }
        public int MaxSeats { get; set; }
        public decimal Price { get; set; }
        public string Summary { get; set; }
        public bool IsActive { get; set; }
    }
}
