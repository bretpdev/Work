-- =============================================
-- Author:		Bret Pehrson
-- Create date: 11/6/2014
-- Description:	Checks to see if the borrower is on Ecorr
-- =============================================
CREATE PROCEDURE CheckBorrowerOnEcorr 
	@AccountNumber VARCHAR(10)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		[DI_CNC_EBL_OPI]
	FROM
		PH05_ContactEmail
	WHERE
		DF_SPE_ACC_ID = @AccountNumber
END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[CheckBorrowerOnEcorr] TO [db_executor]
    AS [dbo];

