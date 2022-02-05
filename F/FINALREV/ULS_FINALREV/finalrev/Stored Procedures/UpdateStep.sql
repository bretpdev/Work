CREATE PROCEDURE [finalrev].[UpdateStep]
	@BorrowerRecordId INT,
	@Step INT
AS
	UPDATE
		finalrev.BorrowerRecord
	SET
		Step = @Step
	WHERE
		BorrowerRecordID = @BorrowerRecordId
RETURN 0