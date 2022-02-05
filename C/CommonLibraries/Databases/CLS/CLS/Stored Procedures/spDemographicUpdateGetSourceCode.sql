CREATE PROCEDURE spDemographicUpdateGetSourceCode
	-- Add the parameters for the stored procedure here
	@SourceName	VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT SourceCode
	FROM DemographicUpdateSystemCodes
	WHERE SourceName = @SourceName
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDemographicUpdateGetSourceCode] TO [db_executor]
    AS [dbo];



