-- =============================================
-- Author:		Jarom Ryan
-- Create date: 03/14/2012
-- =============================================
CREATE PROCEDURE [dbo].[spCompassPifDeleteBwrAndLoanData]

	@AccountNumber Varchar(10)
	
AS
BEGIN

	SET NOCOUNT ON;


	Delete 
	From CompassPifBwrLevel 
	Where @AccountNumber = AccountNumber
	
	Delete 
	From CompassPifLoanLevel 
	Where @AccountNumber = AccountNumber
END
