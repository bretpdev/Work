CREATE PROCEDURE [cntrprnt].[MarkFaxDeleted]
	@SeqNum BIGINT
AS

UPDATE 
	dbo.PRNT_DAT_Fax
SET
	DeletedAt = GETDATE()
WHERE
	SeqNum = @SeqNum

RETURN 0