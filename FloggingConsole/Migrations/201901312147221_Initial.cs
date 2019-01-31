namespace FloggingConsole.Migrations
{
  using System;
  using System.Data.Entity.Migrations;

  public partial class Initial : DbMigration
  {
    public override void Up()
    {
      CreateTable(
          "dbo.Customers",
          c => new
          {
            Id = c.Int(nullable: false, identity: true),
            Name = c.String(maxLength: 10),
            TotalPurchases = c.Decimal(nullable: false, precision: 18, scale: 2),
            TotalReturns = c.Decimal(nullable: false, precision: 18, scale: 2),
          })
          .PrimaryKey(t => t.Id);

      Sql(@"CREATE PROCEDURE dbo.CreateNewCustomer
		            @Name NVARCHAR(MAX),
		            @TotalPurchases Money,
		            @TotalReturns MONEY
            AS
	            INSERT INTO dbo.Customers
	            VALUES ( @Name, @TotalPurchases, @TotalReturns )");

    }

    public override void Down()
    {
      DropTable("dbo.Customers");
      Sql("DROP PROCEDURE dbo.CreateNewCustomer");
    }
  }
}
