-- =============================================
-- Author:		Jarom Ryan
-- Create date: 03/06/2012
-- Description:	This sp will get all of the loan level information for each borrower in 
-- the CompassPifLoanLevel table
-- =============================================
CREATE PROCEDURE [dbo].[spCompassPifGetLoanInfo]

AS
BEGIN

	SET NOCOUNT ON;

	SELECT AccountNumber,
			UniqueId,
			GuarDate,
			GuarAmt,
			LoanPgm
	FROM CompassPifLoanLevel
END
