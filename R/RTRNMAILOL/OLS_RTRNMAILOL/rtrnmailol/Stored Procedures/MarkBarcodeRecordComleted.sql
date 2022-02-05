CREATE PROCEDURE [rtrnmailol].[MarkBarcodeRecordCompleted]
	@BarcodeDataId INT
AS
	UPDATE
		[rtrnmailol].BarcodeData
	SET
		ProcessedAt = GETDATE()
	WHERE
		BarcodeDataId = @BarcodeDataId
RETURN 0