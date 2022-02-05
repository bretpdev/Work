CREATE PROCEDURE [rtrnmailol].[InsertArcAddId]
	@BarcodeDataId INT,
	@ArcAddProcessingId INT
AS
	UPDATE
		rtrnmailol.BarcodeData
	SET
		ArcAddProcessingId = @ArcAddProcessingId
	WHERE
		BarcodeDataId = @BarcodeDataId