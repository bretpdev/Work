CREATE PROCEDURE [dbo].[UpdatePathType]
	@PathTypeId INT,
	@Description NVARCHAR(32),
	@RootPath NVARCHAR(256)
AS

UPDATE 
	dbo.PathTypes
SET 
	[Description] = @Description,
	RootPath = @RootPath
WHERE 
	PathTypeId = @PathTypeId
