-- =============================================
-- Author:		Jarom Ryan	
-- Create date: 03/13/2013
-- Description:	This sp will updated totals for dbo.BankruptcyReport table
-- =============================================
CREATE PROCEDURE [dbo].[spCreateFsaReportsUpdateTotals] 
	
	@Borrowers INT,
	@Loans INT,
	@Dollars DECIMAL,
	@CornerStone BIT
	
	
AS
BEGIN

	SET NOCOUNT ON;

	UPDATE dbo.BankruptcyReport
	SET FiscalYearToDateBorrowers = @Borrowers,
		FiscalYearToDateLoans = @Loans,
		FiscalYearToDateAmount = @Dollars
	WHERE CornerStone = @CornerStone
	
	
END
