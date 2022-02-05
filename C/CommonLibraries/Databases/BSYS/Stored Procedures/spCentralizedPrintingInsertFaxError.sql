CREATE PROCEDURE [dbo].[spCentralizedPrintingInsertFaxError]
	@PrintOrFaxSequence varchar(50)
AS
	INSERT INTO PRNT_DAT_FaxingErrors (FaxSeqNum) VALUES (@PrintOrFaxSequence)
RETURN 0
