CREATE PROCEDURE dbo.spPRNT_GetFaxRecs

 AS

SELECT SeqNum, BusinessUnit, LetterID, FaxNumber, AccountNumber, CommentsAddedTo, RightFaxHandle
FROM dbo.PRNT_DAT_Fax
WHERE FaxDate IS NULL