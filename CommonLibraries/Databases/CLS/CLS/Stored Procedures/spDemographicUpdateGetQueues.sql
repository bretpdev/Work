CREATE PROCEDURE spDemographicUpdateGetQueues
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT [Queue]
	FROM DemographicUpdateQueue
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spDemographicUpdateGetQueues] TO [db_executor]
    AS [dbo];



