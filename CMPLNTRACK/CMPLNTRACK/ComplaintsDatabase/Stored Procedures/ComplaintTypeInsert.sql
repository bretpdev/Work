CREATE PROCEDURE [complaints].[ComplaintTypeInsert]
	@ComplaintTypeName NVARCHAR(100)
AS

	INSERT INTO [complaints].[ComplaintTypes] (TypeName)
	VALUES (@ComplaintTypeName)

RETURN 0