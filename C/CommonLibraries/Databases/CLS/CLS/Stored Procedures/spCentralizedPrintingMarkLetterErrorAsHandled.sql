CREATE PROCEDURE spCentralizedPrintingMarkLetterErrorAsHandled
	@LetterSeqNum	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CentralizedPrintingLetterError
	SET Handled = GETDATE()
	WHERE LetterSeqNum = @LetterSeqNum
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingMarkLetterErrorAsHandled] TO [db_executor]
    AS [dbo];



