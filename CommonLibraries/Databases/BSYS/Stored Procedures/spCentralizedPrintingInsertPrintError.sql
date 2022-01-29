CREATE PROCEDURE [dbo].[spCentralizedPrintingInsertPrintError]
	@PrintOrFaxSequence varchar(50)
AS
	INSERT INTO PRNT_DAT_PrintingErrors (PrintSeqNum) VALUES (@PrintOrFaxSequence)
RETURN 0
