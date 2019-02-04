using Flogging.Data.Entity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FloggingConsole.Models
{
  public class Customer
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal TotalPurchases { get; set; }
    public decimal TotalReturns { get; set; }
  }

  public class CustomerDbContext : DbContext
  {
    public CustomerDbContext() : base("DefaultConnection")
    { DbInterception.Add(new FloggerEFInterceptor()); }

    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
      modelBuilder.Entity<Customer>()
         .Property(c => c.Name).HasMaxLength(10);
    }
  }
}
