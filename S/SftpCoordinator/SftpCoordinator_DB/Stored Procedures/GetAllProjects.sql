CREATE PROCEDURE [dbo].[GetAllProjects]
AS

SELECT 
	ProjectId, 
	[Name], 
	Notes
FROM 
	Projects
WHERE 
	Retired = 0