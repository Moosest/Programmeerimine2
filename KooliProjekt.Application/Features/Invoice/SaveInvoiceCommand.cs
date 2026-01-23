using KooliProjekt.Application.Infrastructure.Results;
using MediatR;
using System;

namespace KooliProjekt.Application.Features.Invoices
{
    public class SaveInvoiceCommand : IRequest<OperationResult>
    {
        public int Id { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime InvoiceDate { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Shipping { get; set; }
        public decimal Discount { get; set; }
        public decimal GrandTotal { get; set; }
    }
}