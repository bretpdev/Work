CREATE PROCEDURE spCentralizedPrintingSetRightFaxHandle
	@Handle		VARCHAR(50),
	@FaxSeqNum	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CentralizedPrintingFax
	SET RightFaxHandle = @Handle
	WHERE SeqNum = @FaxSeqNum
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingSetRightFaxHandle] TO [db_executor]
    AS [dbo];



