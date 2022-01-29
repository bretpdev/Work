CREATE PROCEDURE [dbo].[spCentralizedPrintingGetUnhandledPrintErrors]
	@PrintOrFaxSequence Varchar(50)
AS
	SELECT COUNT(*) FROM PRNT_DAT_PrintingErrors WHERE PrintSeqNum = @PrintOrFaxSequence AND ErrorHandled IS NULL
RETURN 0
