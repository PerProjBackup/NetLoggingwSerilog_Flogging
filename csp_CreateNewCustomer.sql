USE CustomerDbContext

/****** Object: SqlProcedure [dbo].[CreateNewCustomer] ******/

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[CreateNewCustomer]
		@Name NVARCHAR(MAX),
		@TotalPurchases Money,
		@TotalReturns MONEY
AS
	INSERT INTO dbo.Customers
	VALUES ( @Name, @TotalPurchases, @TotalReturns )
