CREATE PROCEDURE [cntrprnt].[GetUnprocessedFaxRecords]
AS

SELECT 
	SeqNum,
	BusinessUnit,
	LetterID,
	FaxNumber,
	AccountNumber,
	CommentsAddedTo
FROM 
	dbo.PRNT_DAT_Fax
WHERE 
	FaxedAt IS NULL
	AND	DeletedAt IS NULL

RETURN 0