CREATE PROCEDURE [pendingnocalls].[GetNextEligibleCallSuspensionDate]
	@AccountNumber char(10)
AS
	
	declare @StartDate datetime = getdate()

	while exists(
		  select * 
		    from pendingnocalls.BorrowerCallSuspensions 
	       where AccountNumber = @AccountNumber 
	         and ABS(datediff(dd, StartDate, @StartDate)) <= 30
	)
	begin
		set @StartDate = dateadd(dd, 1, @StartDate)
	end

	select @StartDate

RETURN 0
