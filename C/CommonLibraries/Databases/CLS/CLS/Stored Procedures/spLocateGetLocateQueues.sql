
CREATE PROCEDURE spLocateGetLocateQueues 

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	[Queue]
			,SubQueue
	FROM	LocateQueues
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spLocateGetLocateQueues] TO [db_executor]
    AS [dbo];



