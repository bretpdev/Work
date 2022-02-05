CREATE PROCEDURE spCentralizedPrintingCompleted
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO CentralizedPrintingCompletion (PrintingCompletedFor)
	VALUES (GETDATE())
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingCompleted] TO [db_executor]
    AS [dbo];



