
CREATE PROCEDURE spLocateGetLocateTypes 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT	LocateType
			,LocateType + ' - ' + ShortDescription AS DisplayText
			,ShortDescription
			,LongDescription
			,Ord
	FROM	LocateTypes
	ORDER BY Ord
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[spLocateGetLocateTypes] TO [db_executor]
    AS [dbo];



