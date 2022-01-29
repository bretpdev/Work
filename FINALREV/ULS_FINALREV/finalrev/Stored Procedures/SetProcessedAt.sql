CREATE PROCEDURE [finalrev].[SetProcessedAt]
	@BorrowerRecordId INT
AS
	UPDATE
		finalrev.BorrowerRecord
	SET
		ProcessedAt = GETDATE()
	WHERE
		BorrowerRecordID = @BorrowerRecordId