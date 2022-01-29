CREATE PROCEDURE [pendingnocalls].[BorrowerCallSuspensionInsert]
	@AccountNumber char(10),
	@StartDate datetime,
	@EndDate datetime
AS
	
	insert into BorrowerCallSuspensions (AccountNumber, StartDate, EndDate)
	values (@AccountNumber, @StartDate, @EndDate)

RETURN 0
