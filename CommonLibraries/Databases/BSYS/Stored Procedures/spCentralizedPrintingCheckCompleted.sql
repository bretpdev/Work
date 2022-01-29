CREATE PROCEDURE [dbo].[spCentralizedPrintingCheckCompleted]

AS
	SELECT PrintingCompletedFor FROM PRNT_DAT_PrintingCompletion WHERE DATEDIFF(dd, PrintingCompletedFor, GETDATE()) = 0
RETURN 0
