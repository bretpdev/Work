CREATE PROCEDURE [dbo].[spCentralizedPrintingGetFaxErrors]
	@PrintOrFaxSequence varchar(50)
AS
	SELECT COUNT(*) FROM PRNT_DAT_FaxingErrors WHERE FaxSeqNum = @PrintOrFaxSequence

RETURN 0
