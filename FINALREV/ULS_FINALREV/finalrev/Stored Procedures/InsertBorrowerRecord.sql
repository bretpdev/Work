CREATE PROCEDURE [finalrev].[InsertBorrowerRecord]
	@Ssn VARCHAR(9),
	@Step INT
AS
	INSERT INTO finalrev.BorrowerRecord(Ssn, Step)
	VALUES(@Ssn, @Step)

SELECT SCOPE_IDENTITY()