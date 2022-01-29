CREATE PROCEDURE [idrxmldata].[InsertNewBorrXmlData]
	@SSN CHAR(9) = NULL,
	@AccountNumber VARCHAR(10) = NULL,
	@FirstName VARCHAR(35) = NULL,
	@LastName VARCHAR(35) = NULL,
	@MiddleName VARCHAR(35) = NULL
AS
	
	INSERT INTO [dbo].Borrowers(SSN, account_number, first_name, last_name, middle_name, created_at)
	VALUES(@SSN,@AccountNumber,@FirstName,@LastName,@MiddleName,GETDATE())

	SELECT CAST(SCOPE_IDENTITY() AS INT)