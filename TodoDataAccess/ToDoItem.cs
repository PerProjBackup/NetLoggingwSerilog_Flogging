using Flogging.Data.Entity;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.Infrastructure.Interception;

namespace TodoDataAccess
{
  public class ToDoItem
  {
    public int Id { get; set; }
    public string Item { get; set; }
    public bool Completed { get; set; }
  }

  public class ToDoDbContext : DbContext
  {
    public ToDoDbContext(string connectionString) : base(connectionString)
    { DbInterception.Add(new FloggerEFInterceptor()); }

    public ToDoDbContext()
    { DbInterception.Add(new FloggerEFInterceptor()); }

    public DbSet<ToDoItem> ToDoItems { get; set; }

    //protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //{
    //  modelBuilder.Entity<ToDoItem>()
    //    .Property(t => t.Item).HasMaxLength(25);
    //}


  }
}
