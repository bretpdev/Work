CREATE PROCEDURE [dbo].[UpdatePathType]
	@PathTypeId int,
	@Description nvarchar(32),
	@RootPath nvarchar(256)
AS
	update dbo.PathTypes
	   set [Description] = @Description,
	       RootPath = @RootPath
	 where PathTypeId = @PathTypeId
RETURN 0
