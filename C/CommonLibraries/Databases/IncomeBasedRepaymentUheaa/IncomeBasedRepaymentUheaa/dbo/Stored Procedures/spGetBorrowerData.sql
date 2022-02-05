-- =============================================
-- Author:		JAROM RYAN
-- Create date: 05/28/2013
-- Description:	WILL CHECK TO SEE IF A BORROWER ALREADY EXISTS IN THE TABLE
-- =============================================
CREATE PROCEDURE [dbo].[spGetBorrowerData]

@AccountNumber CHAR(10)


AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		borrower_id
	FROM 
		dbo.Borrowers
	WHERE 
		account_number = @AccountNumber
END
