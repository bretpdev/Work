CREATE PROCEDURE [cntrprnt].[MarkLetterRecordAsPrinted]
	@SeqNum BIGINT
AS

UPDATE 
	dbo.PRNT_DAT_Print
SET
	PrintedAt = GETDATE()
WHERE
	SeqNum = @SeqNum
	

RETURN 0
