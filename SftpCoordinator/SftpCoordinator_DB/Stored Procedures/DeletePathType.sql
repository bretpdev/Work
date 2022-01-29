CREATE PROCEDURE [dbo].[DeletePathType]
	@PathTypeId INT = 0
AS

DELETE FROM
	dbo.PathTypes
WHERE 
	PathTypeId = @PathTypeID
