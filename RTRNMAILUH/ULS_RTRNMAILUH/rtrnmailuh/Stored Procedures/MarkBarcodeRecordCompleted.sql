CREATE PROCEDURE [rtrnmailuh].[MarkBarcodeRecordCompleted]
	@BarcodeDataId INT
AS
	UPDATE
		[rtrnmailuh].BarcodeData
	SET
		ProcessedAt = GETDATE()
	WHERE
		BarcodeDataId = @BarcodeDataId
RETURN 0