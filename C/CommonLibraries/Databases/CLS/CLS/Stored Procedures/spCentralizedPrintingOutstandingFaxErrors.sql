CREATE PROCEDURE spCentralizedPrintingOutstandingFaxErrors
	@FaxSeqNum	INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT COUNT(*)
	FROM CentralizedPrintingFaxError
	WHERE FaxSeqNum = @FaxSeqNum
		AND Handled IS NULL
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingOutstandingFaxErrors] TO [db_executor]
    AS [dbo];



