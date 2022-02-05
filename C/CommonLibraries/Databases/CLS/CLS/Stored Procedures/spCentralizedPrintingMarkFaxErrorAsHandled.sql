CREATE PROCEDURE spCentralizedPrintingMarkFaxErrorAsHandled
	@FaxSeqNum	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CentralizedPrintingFaxError
	SET Handled = GETDATE()
	WHERE FaxSeqNum = @FaxSeqNum
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingMarkFaxErrorAsHandled] TO [db_executor]
    AS [dbo];



