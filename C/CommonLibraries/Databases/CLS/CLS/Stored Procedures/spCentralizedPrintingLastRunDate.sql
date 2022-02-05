CREATE PROCEDURE spCentralizedPrintingLastRunDate
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COALESCE(MAX(PrintingCompletedFor), GETDATE() - 1)
	FROM CentralizedPrintingCompletion
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingLastRunDate] TO [db_executor]
    AS [dbo];



