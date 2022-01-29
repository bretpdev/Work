CREATE PROCEDURE [dbo].[spCentralizedPrintingGetFaxCount]
	@FaxSeqNum int
AS
	SELECT COUNT(*) FROM PRNT_DAT_Fax WHERE SeqNum = @FaxSeqNum
RETURN 0
