CREATE PROCEDURE [dbo].[InsertNewBorrXmlData]
	@SSN CHAR(9) = NULL,
	@AccountNumber VARCHAR(10) = NULL,
	@FirstName VARCHAR(13) = NULL,
	@LastName VARCHAR(23) = NULL,
	@MiddleName VARCHAR(13) = NULL
AS
	
	INSERT INTO [dbo].Borrowers(SSN, account_number, first_name, last_name, middle_name, created_at)
	VALUES(@SSN,@AccountNumber,@FirstName,@LastName,@MiddleName,GETDATE())

	SELECT CAST(SCOPE_IDENTITY() AS INT)

RETURN 0