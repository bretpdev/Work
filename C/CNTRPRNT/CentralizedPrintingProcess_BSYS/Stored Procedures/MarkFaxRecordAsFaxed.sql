CREATE PROCEDURE [cntrprnt].[MarkFaxRecordAsFaxed]
	@SeqNum BIGINT
AS

UPDATE 
	dbo.PRNT_DAT_Fax
SET
	FaxedAt = GETDATE()
WHERE
	SeqNum = @SeqNum
	

RETURN 0
