-- =============================================
-- Author:		Jarom Ryan	
-- Create date: 12/27/2012
-- Description:	This sp will get all of the letter types for Loan Servicing Letters Script
-- =============================================
CREATE PROCEDURE [dbo].[spLSLGetLetterTypes]

AS
BEGIN

	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT LetterType
	FROM dbo.LoanServicingLetters
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spLSLGetLetterTypes] TO [db_executor]
    AS [dbo];



