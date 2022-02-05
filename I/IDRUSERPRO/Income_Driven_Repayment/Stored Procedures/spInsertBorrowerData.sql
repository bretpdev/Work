-- =============================================
-- Author:		Jarom Ryan
-- Create date: 05/28/2013
-- Description:	Will enter the data entered by a user with the IDRProcessingFed script
-- =============================================
CREATE PROCEDURE [dbo].[spInsertBorrowerData]
	@Ssn CHAR(9) = NULL,
	@AccountNumber CHAR(10) = NULL,
	@FirstName CHAR(35) = NULL,
	@LastName CHAR(35) = NULL,
	@MiddleName CHAR(35) = NULL
AS
BEGIN

	INSERT INTO dbo.Borrowers ( SSN, account_number, first_name, last_name, middle_name)
	VALUES(@Ssn, @AccountNumber, @FirstName, @LastName, @MiddleName)
	
	SELECT 
		borrower_id
	FROM 
		dbo.Borrowers
	WHERE 
		account_number = @AccountNumber
END