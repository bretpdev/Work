CREATE PROCEDURE spDemographicUpdateGetLocate
	-- Add the parameters for the stored procedure here
	@SourceName	VARCHAR(50)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT LocateType AS [Type],
		LocateDescription AS [Description]
	FROM DemographicUpdateSystemCodes
	WHERE SourceName = @SourceName
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDemographicUpdateGetLocate] TO [db_executor]
    AS [dbo];



