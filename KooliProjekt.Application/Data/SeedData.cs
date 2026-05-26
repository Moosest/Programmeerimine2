using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace KooliProjekt.Application.Data
{
    /// <summary>
    /// 14.11.2025
    /// Testandmete generaator
    /// 
    /// Testandmed genereeritakse ainult siis kui mõni oluline 
    /// tabel on tühi.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class SeedData
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IList<Client> _clients = new List<Client>();
        private readonly IList<Event> _events = new List<Event>();
        private readonly IList<SystemUser> _systemUsers = new List<SystemUser>();
        private readonly IList<Invoice> _invoices = new List<Invoice>();

        public SeedData(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Genereerib andmed
        /// </summary>
        public void Generate()
        {
            // Ära tee midagi kui andmed on juba olemas
            if (_dbContext.Clients.Any() ||
                _dbContext.Events.Any() ||
                _dbContext.SystemUsers.Any() ||
                _dbContext.Invoices.Any())
            {
                return;
            }

            GenerateClients();
            GenerateEvents();
            GenerateSystemUsers();
            GenerateInvoices();

            // Save principal entities first to get generated IDs
            _dbContext.SaveChanges();

            GenerateInvoiceLines();
            GenerateEventSchedules();
            GenerateEventFiles();
            GeneratePayments();

            _dbContext.SaveChanges();
        }

        private void GenerateClients()
        {
            for (var i = 0; i < 10; i++)
            {
                _clients.Add(new Client
                {
                    Name = $"Client {i + 1}",
                    Email = $"client{i + 1}@test.com",
                    Phone = $"555-000{i + 1}",
                    Address = $"Test Address {i + 1}",
                    Discount = 0.05m * (i % 5)
                });
            }

            _dbContext.Clients.AddRange(_clients);
        }

        private void GenerateEvents()
        {
            for (var i = 0; i < 10; i++)
            {
                _events.Add(new Event
                {
                    Name = $"Event {i + 1}",
                    StartTime = DateTime.UtcNow.AddDays(i),
                    Description = $"Description for event {i + 1}",
                    Location = $"Location {i + 1}",
                    MaxSeats = 50 + i,
                    Price = 10 + i,
                    Summary = $"Summary {i + 1}",
                    IsActive = i % 2 == 0
                });
            }

            _dbContext.Events.AddRange(_events);
        }

        private void GenerateSystemUsers()
        {
            for (var i = 0; i < 10; i++)
            {
                _systemUsers.Add(new SystemUser
                {
                    Username = $"user{i + 1}",
                    PasswordHash = "testhash",
                    Role = i % 2 == 0 ? "admin" : "user",
                    CreatedAt = DateTime.UtcNow.AddDays(-i)
                });
            }

            _dbContext.SystemUsers.AddRange(_systemUsers);
        }

        private void GenerateInvoices()
        {
            for (var i = 0; i < 10; i++)
            {
                var subtotal = 100 + i * 10;
                var shipping = 5 + i;
                var discount = 0.05m * (i % 5);

                _invoices.Add(new Invoice
                {
                    InvoiceNo = $"INV-{i + 1:0000}",
                    InvoiceDate = DateTime.UtcNow.AddDays(-i),
                    DueDate = DateTime.UtcNow.AddDays(30 - i),
                    Subtotal = subtotal,
                    Shipping = shipping,
                    Discount = discount,
                    GrandTotal = subtotal + shipping - (subtotal * discount)
                });
            }

            _dbContext.Invoices.AddRange(_invoices);
        }

        private void GenerateInvoiceLines()
        {
            foreach (var invoice in _invoices)
            {
                for (var i = 0; i < 3; i++)
                {
                    var unitPrice = 10 + i;
                    var quantity = 1 + i;
                    var discount = 0.05m * (i % 3);
                    var vatRate = 0.2m;

                    _dbContext.InvoiceLines.Add(new InvoiceLine
                    {
                        InvoiceId = invoice.Id,
                        LineItem = $"{invoice.InvoiceNo} Item {i + 1}",
                        UnitPrice = unitPrice,
                        Quantity = quantity,
                        VatRate = vatRate,
                        Discount = discount,
                        Total = unitPrice * quantity * (1 - discount) * (1 + vatRate)
                    });
                }
            }
        }

        private void GenerateEventSchedules()
        {
            foreach (var ev in _events)
            {
                for (var i = 0; i < 2; i++)
                {
                    _dbContext.EventSchedules.Add(new EventSchedule
                    {
                        EventId = ev.Id,
                        StartTime = ev.StartTime.AddHours(i * 2),
                        FilePath = $"/schedules/{ev.Id}_{i + 1}.ics",
                        FileName = $"schedule_{ev.Id}_{i + 1}.ics",
                        UploadedAt = DateTime.UtcNow.AddDays(-i)
                    });
                }
            }
        }

        private void GenerateEventFiles()
        {
            foreach (var ev in _events)
            {
                _dbContext.EventFiles.Add(new EventFile
                {
                    EventId = ev.Id,
                    FilePath = $"/events/{ev.Id}/brochure.pdf",
                    FileName = $"event_{ev.Id}_brochure.pdf",
                    UploadedAt = DateTime.UtcNow
                });
            }
        }

        private void GeneratePayments()
        {
            for (var i = 0; i < _invoices.Count; i++)
            {
                var invoice = _invoices[i];
                var user = _systemUsers[i % _systemUsers.Count];

                _dbContext.Payments.Add(new Payment
                {
                    InvoiceId = invoice.Id,
                    Amount = invoice.GrandTotal,
                    PaymentDate = invoice.InvoiceDate.AddDays(5),
                    Method = i % 2 == 0 ? "Card" : "BankTransfer",
                    TransactionRef = $"TXN{i + 1:0000}",
                    ModifiedBy = user.Id
                });
            }
        }
    }
}
