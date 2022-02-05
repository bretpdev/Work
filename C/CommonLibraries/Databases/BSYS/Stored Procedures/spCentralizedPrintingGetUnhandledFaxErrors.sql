CREATE PROCEDURE [dbo].[spCentralizedPrintingGetUnhandledFaxErrors]
	@PrintOrFaxSequence Varchar(50)
AS
	SELECT COUNT(*) FROM PRNT_DAT_FaxingErrors WHERE FaxSeqNum = @PrintOrFaxSequence AND ErrorHandled IS NULL
RETURN 0
