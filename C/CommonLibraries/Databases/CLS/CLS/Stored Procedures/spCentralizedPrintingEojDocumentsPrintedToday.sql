CREATE PROCEDURE spCentralizedPrintingEojDocumentsPrintedToday
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT BusinessUnitId
	FROM CentralizedPrintingLetter
	WHERE DATEDIFF(d, Printed, GETDATE()) = 0
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingEojDocumentsPrintedToday] TO [db_executor]
    AS [dbo];



