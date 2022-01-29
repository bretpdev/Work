CREATE PROCEDURE spCentralizedPrintingMarkLetterErrorsAsRePrinted
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	UPDATE CentralizedPrintingLetterError
	SET RePrinted = GETDATE()
	WHERE RePrinted IS NULL
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingMarkLetterErrorsAsRePrinted] TO [db_executor]
    AS [dbo];



