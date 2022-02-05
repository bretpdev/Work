CREATE PROCEDURE spCentralizedPrintingEojFaxesSentToday
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT BusinessUnitId
	FROM CentralizedPrintingFax
	WHERE DATEDIFF(d, Faxed, GETDATE()) = 0
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingEojFaxesSentToday] TO [db_executor]
    AS [dbo];



