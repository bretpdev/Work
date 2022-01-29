CREATE PROCEDURE [dbo].[GetAllProjects]
AS
	SELECT ProjectId, Name, Notes, IsFederal
	  FROM [dbo].[Projects]
	 where [Retired] = 0
RETURN 0

grant execute on [dbo].[GetAllProjects] to [db_executor]