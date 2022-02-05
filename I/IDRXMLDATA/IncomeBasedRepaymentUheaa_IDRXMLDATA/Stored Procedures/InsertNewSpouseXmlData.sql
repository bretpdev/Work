CREATE PROCEDURE [idrxmldata].[InsertNewSpouseXmlData]
	@SSN CHAR(9),
	@BirthDate DATETIME,
	@FirstName VARCHAR(35),
	@LastName VARCHAR(35),
	@MiddleName VARCHAR(35) = NULL,
	@Separated BIT,
	@AccessToIncomeInfo BIT
AS
	INSERT INTO [dbo].Spouses(SSN, birth_date, first_name, last_name, middle_name, separated_from_spouse, access_spouse_income_info, created_at, updated_at, person_role_id)
	VALUES(@SSN,@BirthDate,@FirstName,@LastName,@MiddleName,@Separated,@AccessToIncomeInfo,GETDATE(),NULL,4)

	SELECT CAST(SCOPE_IDENTITY() AS INT)
RETURN 0
