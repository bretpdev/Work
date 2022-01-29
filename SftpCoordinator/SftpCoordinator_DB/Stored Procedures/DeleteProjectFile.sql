CREATE PROCEDURE [dbo].[DeleteProjectFile]
	@ProjectFileId INT
AS

UPDATE 
	ProjectFiles 
SET
	Retired = 1 
WHERE
	ProjectFileId = @ProjectFileId