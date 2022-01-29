CREATE PROCEDURE dbo.spPRNT_GetFaxRecsToConfirm 

AS

SELECT SeqNum, BusinessUnit, LetterID, FaxNumber, AccountNumber, CommentsAddedTo, RightFaxHandle
FROM dbo.PRNT_DAT_Fax
WHERE RightFaxHandle IS NOT NULL 
AND FaxConfirmationDate IS NULL