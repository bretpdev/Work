CREATE PROCEDURE spCentralizedPrintingUpdateFaxRecord
	@FaxSeqNum	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	IF (SELECT COUNT(*) FROM CentralizedPrintingFaxError WHERE FaxSeqNum = @FaxSeqNum) > 0
		--IF FAX FAILED BECAUSE THE DOC WASN'T OUT ON THE NETWORK
		BEGIN
			UPDATE CentralizedPrintingFax
			SET Faxed = GETDATE(),
				Confirmed = GETDATE(),
				FinalStatus = 'NO DOC FOUND'
			WHERE SeqNum = @FaxSeqNum
		END
	ELSE
		--IF FAX WAS INITIATED
		BEGIN
			UPDATE CentralizedPrintingFax
			SET Faxed = GETDATE()
			WHERE SeqNum = @FaxSeqNum
		END
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingUpdateFaxRecord] TO [db_executor]
    AS [dbo];



