-- =============================================
-- Author:		Jarom Ryan
-- Create date: 03/13/2012
-- =============================================
CREATE PROCEDURE [dbo].[spCompassPifGetLoanSeqForComments]

	@AccountNumber	VARCHAR(10)
AS
BEGIN

	SET NOCOUNT ON;


	SELECT LoanSeq
	FROM dbo.CompassPifLoanLevel
	WHERE @AccountNumber = AccountNumber
END
