CREATE PROCEDURE [dbo].[spCentralizedPrintingPrintErrorSetHandled]
	@PrintOrFaxSequence Varchar(50)
AS
	UPDATE PRNT_DAT_PrintingErrors SET ErrorHandled = GETDATE() WHERE PrintSeqNum = @PrintOrFaxSequence
RETURN 0
