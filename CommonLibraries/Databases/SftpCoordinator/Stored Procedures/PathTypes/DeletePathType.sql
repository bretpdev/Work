CREATE PROCEDURE [dbo].[DeletePathType]
	@PathTypeId int = 0
AS
	delete from dbo.PathTypes
	 where PathTypeId = @PathTypeID
RETURN 0
