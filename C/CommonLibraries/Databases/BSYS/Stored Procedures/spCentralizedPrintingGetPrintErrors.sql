CREATE PROCEDURE [dbo].[spCentralizedPrintingGetPrintErrors]
	@PrintOrFaxSequence Varchar(50)
AS
	SELECT COUNT(*) FROM PRNT_DAT_PrintingErrors WHERE PrintSeqNum = @PrintOrFaxSequence
RETURN 0
