CREATE PROCEDURE [dbo].[DeleteProject]
	@ProjectId int
AS
	update Projects set [Retired] = 1
	 where ProjectId = @ProjectId
RETURN 0

grant execute on [dbo].[DeleteProject] to [db_executor]