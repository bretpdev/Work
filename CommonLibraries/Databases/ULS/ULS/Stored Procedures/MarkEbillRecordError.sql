CREATE PROCEDURE [dbo].[MarkEbillRecordError]
	@EbillId int
AS
	UPDATE 
		EBill
	SET 
		HadError = 1,
		ErroredAt = GETDATE()
	WHERE 
		EbillId = @EbillId
RETURN 0
