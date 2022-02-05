CREATE PROCEDURE [dbo].[spCentralizedPrintingFaxErrorSetHandled]
	@PrintOrFaxSequence varchar(50)
AS
	UPDATE PRNT_DAT_FaxingErrors SET ErrorHandled = GETDATE() WHERE FaxSeqNum = @PrintOrFaxSequence
RETURN 0
