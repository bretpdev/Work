CREATE PROCEDURE [dbo].[UpdateProject]
	@ProjectId INT,
	@Name NVARCHAR(MAX),
	@Notes NVARCHAR(MAX)
AS

UPDATE 
	Projects
SET	
	[Name] = @Name, 
	Notes = @Notes
WHERE 
	ProjectId = @ProjectId
