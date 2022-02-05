CREATE PROCEDURE [dbo].[DeleteProject]
	@ProjectId INT
AS

UPDATE 
	Projects 
SET
	Retired = 1
WHERE 
	ProjectId = @ProjectId