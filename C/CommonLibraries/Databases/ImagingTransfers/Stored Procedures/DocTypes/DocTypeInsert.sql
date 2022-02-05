CREATE PROCEDURE [dbo].[DocTypeInsert]
	@DocTypeValue char(4)
AS
	insert into dbo.DocTypes (DocTypeValue)
	values (@DocTypeValue)

	select SCOPE_IDENTITY() as DocTypeId
RETURN 0

grant execute on [dbo].[DocTypeInsert] to [db_executor]