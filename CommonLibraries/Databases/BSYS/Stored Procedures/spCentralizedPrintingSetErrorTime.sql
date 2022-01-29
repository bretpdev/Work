CREATE PROCEDURE [dbo].[spCentralizedPrintingSetErrorTime]

AS
	UPDATE PRNT_DAT_PrintingErrors SET ErrorPrinted = GETDATE() WHERE ErrorPrinted IS NULL
RETURN 0
