CREATE PROCEDURE [dbo].[DeleteProjectFile]
	@ProjectFileId int
AS
	update ProjectFiles set [Retired] = 1 where ProjectFileId = @ProjectFileId
RETURN 0
