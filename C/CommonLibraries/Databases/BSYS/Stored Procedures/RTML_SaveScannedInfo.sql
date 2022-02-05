CREATE PROCEDURE [dbo].[RTML_SaveScannedInfo]
	@RecipientId varchar(10),
	@LetterId varchar(10),
	@CreateDate datetime
AS
	INSERT INTO RTML_DAT_BarcodeData(RecipientId, LetterId, CreateDate)
	VALUES(@RecipientId, @LetterId, @CreateDate)
RETURN 0

GRANT EXECUTE ON RTML_SaveScannedInfo TO db_executor