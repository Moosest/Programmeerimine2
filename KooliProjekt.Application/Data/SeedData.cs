using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KooliProjekt.Application.Data
{
    /// <summary>
    /// 14.11.2025
    /// Testandmete generaator
    /// 
    /// Testandmed genereeritakse ainult siis kui mõni oluline 
    /// tabel on tühi.
    /// </summary>
    public class SeedData
    {
        private readonly ApplicationDbContext _dbContext;

        public SeedData(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Genereerib andmed
        /// </summary>
        public void Generate()
        {
            // Clients
            if (!_dbContext.Clients.Any())
            {
                for (var i = 0; i < 10; i++)
                {
                    _dbContext.Clients.Add(new Client
                    {
                        Name = $"Client {i+1}",
                        Email = $"client{i+1}@test.com",
                        Phone = $"555-000{i+1}",
                        Address = $"Test Address {i+1}",
                        Discount = 0.05m * (i % 5)
                    });
                }
            }

            // Events
            if (!_dbContext.Events.Any())
            {
                for (var i = 0; i < 10; i++)
                {
                    _dbContext.Events.Add(new Event
                    {
                        Name = $"Event {i+1}",
                        StartTime = DateTime.UtcNow.AddDays(i),
                        Description = $"Description for event {i+1}",
                        Location = $"Location {i+1}",
                        MaxSeats = 50 + i,
                        Price = 10 + i,
                        Summary = $"Summary {i+1}",
                        IsActive = i % 2 == 0
                    });
                }
            }

            // EventFiles
            if (!_dbContext.EventFiles.Any())
            {
                for (var i = 0; i < 10; i++)
                {
                    _dbContext.EventFiles.Add(new EventFile
                    {
                        EventId = 1,
                        FilePath = $"/files/eventfile_{i+1}.pdf",
                        FileName = $"eventfile_{i+1}.pdf",
                        UploadedAt = DateTime.UtcNow.AddDays(-i)
                    });
                }
            }

            // EventSchedules
            if (!_dbContext.EventSchedules.Any())
            {
                for (var i = 0; i < 10; i++)
                {
                    _dbContext.EventSchedules.Add(new EventSchedule
                    {
                        EventId = 1,
                        StartTime = DateTime.UtcNow.AddDays(i),
                        FilePath = $"/schedules/schedule_{i+1}.ics",
                        FileName = $"schedule_{i+1}.ics",
                        UploadedAt = DateTime.UtcNow.AddDays(-i)
                    });
                }
            }

            // Invoices
            if (!_dbContext.Invoices.Any())
            {
                for (var i = 0; i < 10; i++)
                {
                    var invoice = new Invoice
                    {
                        InvoiceNo = $"INV{i+1:0000}",
                        InvoiceDate = DateTime.UtcNow.AddDays(-i),
                        DueDate = DateTime.UtcNow.AddDays(30-i),
                        Subtotal = 100 + i * 10,
                        Shipping = 5,
                        Discount = 0.05m * (i % 5),
                        GrandTotal = 100 + i * 10 + 5 - (100 + i * 10) * 0.05m * (i % 5),
                        InvoiceLines = new List<InvoiceLine>()
                    };
                    for (var j = 0; j < 2; j++)
                    {
                        invoice.InvoiceLines.Add(new InvoiceLine
                        {
                            LineItem = $"Product {j+1}",
                            UnitPrice = 50 + j * 10,
                            Quantity = 1 + j,
                            VatRate = 0.2m,
                            Discount = 0.05m * j,
                            Total = (50 + j * 10) * (1 + j) * (1 - 0.05m * j)
                        });
                    }
                    _dbContext.Invoices.Add(invoice);
                }
            }

            // Payments
            if (!_dbContext.Payments.Any())
            {
                for (var i = 0; i < 10; i++)
                {
                    _dbContext.Payments.Add(new Payment
                    {
                        InvoiceId = 1,
                        Amount = 100 + i * 10,
                        PaymentDate = DateTime.UtcNow.AddDays(-i),
                        Method = i % 2 == 0 ? "Card" : "BankTransfer",
                        TransactionRef = $"TXN{i+1:0000}",
                        ModifiedBy = 1
                    });
                }
            }

            // SystemUsers
            if (!_dbContext.SystemUsers.Any())
            {
                for (var i = 0; i < 10; i++)
                {
                    _dbContext.SystemUsers.Add(new SystemUser
                    {
                        Username = $"user{i+1}",
                        PasswordHash = "testhash",
                        Role = i % 2 == 0 ? "admin" : "user",
                        CreatedAt = DateTime.UtcNow.AddDays(-i)
                    });
                }
            }

            _dbContext.SaveChanges();
        }
    }
}
