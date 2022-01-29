CREATE PROCEDURE [cntrprnt].[MarkRecordEcorrStatus]
	@SeqNum BIGINT,
	@IsOnEcorr BIT,
	@EcorrDocumentCreatedAt DATETIME = NULL
AS

UPDATE 
	dbo.PRNT_DAT_Print
SET
	IsOnEcorr = @IsOnEcorr,
	EcorrDocumentCreatedAt = @EcorrDocumentCreatedAt
WHERE
	SeqNum = @SeqNum

RETURN 0
