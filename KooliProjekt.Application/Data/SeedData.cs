using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
            // Use TRUNCATE TABLE ... CASCADE to fully clear tables and reset sequences
            _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE \"InvoiceLines\" CASCADE;");
            _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE \"Payments\" CASCADE;");
            _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE \"Invoices\" CASCADE;");
            _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE \"EventFiles\" CASCADE;");
            _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE \"EventSchedules\" CASCADE;");
            _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE \"Events\" CASCADE;");
            _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE \"Clients\" CASCADE;");
            _dbContext.Database.ExecuteSqlRaw("TRUNCATE TABLE \"SystemUsers\" CASCADE;");

            // Clients
            for (var i = 0; i < 10; i++)
            {
                var client = new Client
                {
                    Name = $"Client {i+1}",
                    Email = $"client{i+1}@test.com",
                    Phone = $"555-000{i+1}",
                    Address = $"Test Address {i+1}",
                    Discount = 0.05m * (i % 5)
                };
                // Do not set Id explicitly; let DB assign
                _dbContext.Clients.Add(client);
            }

            // Events
            for (var i = 0; i < 10; i++)
            {
                var ev = new Event
                {
                    Name = $"Event {i+1}",
                    StartTime = DateTime.UtcNow.AddDays(i),
                    Description = $"Description for event {i+1}",
                    Location = $"Location {i+1}",
                    MaxSeats = 50 + i,
                    Price = 10 + i,
                    Summary = $"Summary {i+1}",
                    IsActive = i % 2 == 0
                };
                // Do not set Id explicitly; let DB assign
                _dbContext.Events.Add(ev);
            }

            // SystemUsers
            for (var i = 0; i < 10; i++)
            {
                var user = new SystemUser
                {
                    Username = $"user{i+1}",
                    PasswordHash = "testhash",
                    Role = i % 2 == 0 ? "admin" : "user",
                    CreatedAt = DateTime.UtcNow.AddDays(-i)
                };
                // Do not set Id explicitly; let DB assign
                _dbContext.SystemUsers.Add(user);
            }

            // Invoices
            var invoices = new List<Invoice>();
            for (var i = 0; i < 10; i++)
            {
                var invoice = new Invoice
                {
                    InvoiceNo = $"INV-{i+1:0000}",
                    InvoiceDate = DateTime.UtcNow.AddDays(-i),
                    DueDate = DateTime.UtcNow.AddDays(30 - i),
                    Subtotal = 100 + i * 10,
                    Shipping = 5 + i,
                    Discount = 0.05m * (i % 5),
                    GrandTotal = 100 + i * 10 + 5 + i - (100 + i * 10) * (0.05m * (i % 5))
                };
                invoices.Add(invoice);
                _dbContext.Invoices.Add(invoice);
            }
            _dbContext.SaveChanges();

            // Use the actual first invoice's ID for InvoiceLines
            var firstInvoiceId = invoices.First().Id;
            for (var i = 0; i < 10; i++)
            {
                var line = new InvoiceLine
                {
                    InvoiceId = firstInvoiceId,
                    LineItem = $"Item {i+1}",
                    UnitPrice = 10 + i,
                    Quantity = 1 + i,
                    VatRate = 0.2m,
                    Discount = 0.05m * (i % 5),
                    Total = (10 + i) * (1 + i) * (1 - 0.05m * (i % 5)) * 1.2m
                };
                _dbContext.InvoiceLines.Add(line);
            }

            _dbContext.SaveChanges();

            // Get actual IDs for references
            var firstEvent = _dbContext.Events.OrderBy(e => e.Id).FirstOrDefault();
            var firstInvoice = _dbContext.Invoices.OrderBy(i => i.Id).FirstOrDefault();
            var firstUser = _dbContext.SystemUsers.OrderBy(u => u.Id).FirstOrDefault();

            // EventSchedules
            if (firstEvent != null)
            {
                for (var i = 0; i < 10; i++)
                {
                    _dbContext.EventSchedules.Add(new EventSchedule
                    {
                        EventId = firstEvent.Id,
                        StartTime = DateTime.UtcNow.AddDays(i),
                        FilePath = $"/schedules/schedule_{i+1}.ics",
                        FileName = $"schedule_{i+1}.ics",
                        UploadedAt = DateTime.UtcNow.AddDays(-i)
                    });
                }
            }

            // Payments
            if (firstInvoice != null && firstUser != null)
            {
                for (var i = 0; i < 10; i++)
                {
                    _dbContext.Payments.Add(new Payment
                    {
                        InvoiceId = firstInvoice.Id,
                        Amount = 100 + i * 10,
                        PaymentDate = DateTime.UtcNow.AddDays(-i),
                        Method = i % 2 == 0 ? "Card" : "BankTransfer",
                        TransactionRef = $"TXN{i+1:0000}",
                        ModifiedBy = firstUser.Id
                    });
                }
            }

            _dbContext.SaveChanges();

            // Reset identity/sequence for PostgreSQL tables
            _dbContext.Database.ExecuteSqlRaw("ALTER SEQUENCE \"Clients_Id_seq\" RESTART WITH 1;");
            _dbContext.Database.ExecuteSqlRaw("ALTER SEQUENCE \"Events_Id_seq\" RESTART WITH 1;");
            _dbContext.Database.ExecuteSqlRaw("ALTER SEQUENCE \"EventFiles_Id_seq\" RESTART WITH 1;");
            _dbContext.Database.ExecuteSqlRaw("ALTER SEQUENCE \"EventSchedules_Id_seq\" RESTART WITH 1;");
            _dbContext.Database.ExecuteSqlRaw("ALTER SEQUENCE \"Invoices_Id_seq\" RESTART WITH 1;");
            _dbContext.Database.ExecuteSqlRaw("ALTER SEQUENCE \"InvoiceLines_Id_seq\" RESTART WITH 1;");
            _dbContext.Database.ExecuteSqlRaw("ALTER SEQUENCE \"Payments_Id_seq\" RESTART WITH 1;");
            _dbContext.Database.ExecuteSqlRaw("ALTER SEQUENCE \"SystemUsers_Id_seq\" RESTART WITH 1;");
        }
    }
}
