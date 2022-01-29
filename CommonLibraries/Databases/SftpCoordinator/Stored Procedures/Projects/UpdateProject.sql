CREATE PROCEDURE [dbo].[UpdateProject]
	@ProjectId int,
	@Name nvarchar(max),
	@Notes nvarchar(max),
	@IsFederal bit
AS
	update Projects
	   set Name = @Name, Notes = @Notes, IsFederal = @IsFederal
	 where ProjectId = @ProjectId
RETURN 0

grant execute on [dbo].[UpdateProject] to [db_executor]