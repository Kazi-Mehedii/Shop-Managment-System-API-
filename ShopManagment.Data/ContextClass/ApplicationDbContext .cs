using Microsoft.EntityFrameworkCore;
using ShopManagment.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopManagment.Data.ContextClass
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        public DbSet<Purchase> Purchase { get; set; }

        public DbSet<Sale> Sale { get; set; }

        public DbSet<SaleItem> SaleItem { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Stock> Stocks { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Report> Reports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           modelBuilder.Entity<Report>().HasNoKey();
        }

    }
}
