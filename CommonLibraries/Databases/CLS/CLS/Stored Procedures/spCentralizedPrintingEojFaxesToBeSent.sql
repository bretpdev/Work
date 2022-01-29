CREATE PROCEDURE spCentralizedPrintingEojFaxesToBeSent
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT BusinessUnitId
	FROM CentralizedPrintingFax
	WHERE Faxed IS NULL
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingEojFaxesToBeSent] TO [db_executor]
    AS [dbo];



