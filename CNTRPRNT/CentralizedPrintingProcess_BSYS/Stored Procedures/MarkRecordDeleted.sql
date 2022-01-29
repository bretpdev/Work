CREATE PROCEDURE [cntrprnt].[MarkRecordDeleted]
	@SeqNum BIGINT
AS
	
UPDATE 
	dbo.PRNT_DAT_Print
SET
	DeletedAt = GETDATE()
WHERE
	SeqNum = @SeqNum

RETURN 0