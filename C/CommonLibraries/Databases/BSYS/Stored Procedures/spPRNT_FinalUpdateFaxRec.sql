CREATE PROCEDURE dbo.spPRNT_FinalUpdateFaxRec 

@RightFaxHandle			VARCHAR(50),
@FinalStatus				VARCHAR(20)		

AS

UPDATE dbo.PRNT_DAT_Fax 
SET FaxConfirmationDate = GETDATE(), FinalStatus = @FinalStatus 
WHERE RightFaxHandle = @RightFaxHandle