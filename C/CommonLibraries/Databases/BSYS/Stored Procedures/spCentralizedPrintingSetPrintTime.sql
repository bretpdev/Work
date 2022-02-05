CREATE PROCEDURE [dbo].[spCentralizedPrintingSetPrintTime]
	@SeqNum int
AS
	UPDATE PRNT_DAT_Print SET ActualPrintedTime = GETDATE() WHERE SeqNum = @SeqNum

RETURN 0
