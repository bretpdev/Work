CREATE PROCEDURE [dbo].[spCentralizedPrintingSetRightFaxHandle]
	@RightFaxHandle int,
	@SeqNum int
AS
	UPDATE PRNT_DAT_Fax SET RightFaxHandle = @RightFaxHandle WHERE SeqNum = @SeqNum

RETURN 0
