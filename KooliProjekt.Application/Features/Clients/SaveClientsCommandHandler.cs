using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KooliProjekt.Application.Data;
using KooliProjekt.Application.Infrastructure.Paging;
using KooliProjekt.Application.Infrastructure.Results;
using MediatR;

namespace KooliProjekt.Application.Features.Clients
{
    public class SaveClientsCommandHandler : IRequestHandler<SaveClientsCommand, OperationResult>
    {
        private readonly ApplicationDbContext _dbContext;

        public SaveClientsCommandHandler(ApplicationDbContext dbContext)
        {
            if (dbContext == null) throw new ArgumentNullException(nameof(dbContext));
            _dbContext = dbContext;
        }

        public async Task<OperationResult> Handle(SaveClientsCommand request, CancellationToken cancellationToken)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));
            var result = new OperationResult();

            var list = new Client();
            if(request.Id == 0)
            {
                await _dbContext.Clients.AddAsync(list);
            }
            else
            {
                list = await _dbContext.Clients.FindAsync(request.Id);
                //_dbContext.ToDoLists.Update(list);
            }

            list.Name = request.Name;
            list.Email = request.Email;
            list.Phone = request.Phone;
            list.Address = request.Address;
            list.Discount = request.Discount;
            
            await _dbContext.SaveChangesAsync();

            return result;
        }
    }
}
