CREATE PROCEDURE [finalrev].[InsertBorrowerSchool]
	@BorrowerRecordID INT,
	@SchoolsId INT
AS
	INSERT INTO finalrev.BorrowerSchools(BorrowerRecordId, SchoolsId)
	VALUES(@BorrowerRecordID, @SchoolsId)

	UPDATE
		finalrev.BorrowerRecord
	SET
		SchoolLetterNeeded = 1
	WHERE
		BorrowerRecordID = @BorrowerRecordID