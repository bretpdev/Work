CREATE PROCEDURE [dbo].[RTML_MarkBarcodeRecordCompleted]
	@RecipientId varchar(10),
	@LetterId varchar(10),
	@CreateDate datetime
AS
	UPDATE
		RTML_DAT_BarcodeData
	SET
		AddressInvalidatedDate = GETDATE()
	WHERE
		RecipientId = @RecipientId
		AND LetterId = @LetterId
		AND CreateDate = @CreateDate
RETURN 0

GRANT EXECUTE ON RTML_MarkBarcodeRecordCompleted TO db_executor