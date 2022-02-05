CREATE PROCEDURE [complaints].[ComplaintTypeInsert]
	@ComplaintTypeName nvarchar(100)
AS

	insert into [complaints].[ComplaintTypes] (TypeName)
	values (@ComplaintTypeName)

RETURN 0