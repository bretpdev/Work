CREATE PROCEDURE [dbo].[InsertNewSpouseXmlData]
	@SSN CHAR(9),
	@BirthDate DATETIME,
	@FirstName VARCHAR(13),
	@LastName VARCHAR(23),
	@MiddleName VARCHAR(13) = null,
	@Separated BIT,
	@AccessToIncomeInfo BIT
AS
	INSERT INTO [dbo].Spouses(SSN, birth_date, first_name, last_name, middle_name, separated_from_spouse, access_spouse_income_info, created_at, updated_at)
	VALUES(@SSN,@BirthDate,@FirstName,@LastName,@MiddleName,@Separated,@AccessToIncomeInfo,GETDATE(),NULL)

	SELECT CAST(SCOPE_IDENTITY() AS INT)
RETURN 0