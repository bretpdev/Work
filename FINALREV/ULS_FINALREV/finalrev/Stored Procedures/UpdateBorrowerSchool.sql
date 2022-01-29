CREATE PROCEDURE [finalrev].[UpdateBorrowerSchool]
	@BorrwerRecordId INT,
	@School VARCHAR(10)
AS
	UPDATE
		BorrowerRecord
	SET
		SchoolLetterNeeded = 1
	WHERE
		BorrowerRecordID = @BorrwerRecordId