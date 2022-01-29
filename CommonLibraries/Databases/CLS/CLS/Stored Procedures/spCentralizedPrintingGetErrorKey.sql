CREATE PROCEDURE spCentralizedPrintingGetErrorKey
	@BusinessUnitId	INT,
	@ErrorType		VARCHAR(5),
	@KeyType		VARCHAR(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT Value
	FROM CentralizedPrintingBusinessUnitErrorKey
	WHERE BusinessUnitId = @BusinessUnitId
		AND ErrorType = @ErrorType
		AND KeyType = @KeyType
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spCentralizedPrintingGetErrorKey] TO [db_executor]
    AS [dbo];



