using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Cache;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace KooliProjekt.Application.Features.Events
{
    public class SaveEventCommandHandler : IRequestHandler<SaveEventCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveEventCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveEventCommand request, CancellationToken cancellationToken)
        {
            var result = new OperationResult();

            var list = new Event();
            if (request.Id == 0)
            {
                await _dbContext.Events.AddAsync(list);
            }
            else
            {
                list = await _dbContext.Events.FindAsync(request.Id);
                //_dbContext.ToDoLists.Update(list);
            }

            list.Name = request . Name;
            list.StartTime = request.StartTime;
            list.Description = request.Description;
            list.Location = request.Location;
            list.MaxSeats = request.MaxSeats;
            list.Price = request.Price;
            list.Summary = request.Summary;
            list.IsActive = request.IsActive;

            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
