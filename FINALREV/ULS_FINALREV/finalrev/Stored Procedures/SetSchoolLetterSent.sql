CREATE PROCEDURE [finalrev].[SetSchoolLetterSent]
	@BorrowerRecordId INT
AS
	UPDATE
		finalrev.BorrowerRecord
	SET
		SchoolLetterSent = GETDATE()
	WHERE
		BorrowerRecordID = @BorrowerRecordId