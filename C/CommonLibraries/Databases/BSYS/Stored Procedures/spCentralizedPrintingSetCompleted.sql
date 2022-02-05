CREATE PROCEDURE [dbo].[spCentralizedPrintingSetCompleted]

AS
	INSERT INTO PRNT_DAT_PrintingCompletion (PrintingCompletedFor) VALUES (GETDATE())
RETURN 0
