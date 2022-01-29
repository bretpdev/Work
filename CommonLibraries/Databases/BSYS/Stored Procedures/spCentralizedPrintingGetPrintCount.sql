CREATE PROCEDURE [dbo].[spCentralizedPrintingGetPrintCount]
	@LetterSeqNum int
AS
	SELECT COUNT(*) FROM PRNT_DAT_Print WHERE SeqNum = @LetterSeqNum

RETURN 0
