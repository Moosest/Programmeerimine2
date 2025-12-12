using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace KooliProjekt.Application.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<EventFile> EventFiles { get; set; }
        public DbSet<EventSchedule> EventSchedules { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceLine> InvoiceLines { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<SystemUser> SystemUsers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Invoice>()
                .HasIndex(i => i.InvoiceNo)
                .IsUnique();

            modelBuilder.Entity<InvoiceLine>()
                .HasOne(il => il.Invoice)
                .WithMany(i => i.InvoiceLines)
                .HasForeignKey(il => il.InvoiceId);

            modelBuilder.Entity<Event>().Property(e => e.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>().Property(i => i.Subtotal).HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>().Property(i => i.Shipping).HasPrecision(18, 2);
            modelBuilder.Entity<Invoice>().Property(i => i.GrandTotal).HasPrecision(18, 2);
            modelBuilder.Entity<Payment>().Property(p => p.Amount).HasPrecision(18, 2);
            modelBuilder.Entity<InvoiceLine>().Property(il => il.UnitPrice).HasPrecision(18, 2);
            modelBuilder.Entity<InvoiceLine>().Property(il => il.Total).HasPrecision(18, 2);

            modelBuilder.Entity<InvoiceLine>().Property(il => il.Quantity).HasPrecision(10, 2);
            modelBuilder.Entity<Client>().Property(c => c.Discount).HasPrecision(5, 4);
            modelBuilder.Entity<Invoice>().Property(i => i.Discount).HasPrecision(5, 4);
            modelBuilder.Entity<InvoiceLine>().Property(il => il.VatRate).HasPrecision(5, 4);
            modelBuilder.Entity<InvoiceLine>().Property(il => il.Discount).HasPrecision(5, 4);

            base.OnModelCreating(modelBuilder);
        }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
    }
}
