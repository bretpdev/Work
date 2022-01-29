CREATE PROCEDURE [rtrnmailuh].[InsertArcAddId]
	@BarcodeDataId INT,
	@ArcAddProcessingId INT
AS
	UPDATE
		rtrnmailuh.BarcodeData
	SET
		ArcAddProcessingId = @ArcAddProcessingId
	WHERE
		BarcodeDataId = @BarcodeDataId