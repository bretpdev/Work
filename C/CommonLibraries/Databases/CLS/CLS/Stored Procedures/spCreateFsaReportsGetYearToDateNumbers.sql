-- =============================================
-- Author:		Jarom Ryan
-- Create date: 03/11/2013
-- Description:	returns FiscalYearToDateBorrowers, and FiscalYearToDateLoans
-- =============================================
CREATE PROCEDURE [dbo].[spCreateFsaReportsGetYearToDateNumbers]

	@CornerStone bit
AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		FiscalYearToDateBorrowers,
		FiscalYearToDateLoans,
		FiscalYearToDateAmount
	FROM dbo.BankruptcyReport
	Where CornerStone = @CornerStone
	
END
