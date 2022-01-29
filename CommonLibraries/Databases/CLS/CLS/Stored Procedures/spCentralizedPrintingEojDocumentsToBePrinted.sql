CREATE PROCEDURE spCentralizedPrintingEojDocumentsToBePrinted
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT BusinessUnitId
	FROM CentralizedPrintingLetter
	WHERE Printed IS NULL
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingEojDocumentsToBePrinted] TO [db_executor]
    AS [dbo];



