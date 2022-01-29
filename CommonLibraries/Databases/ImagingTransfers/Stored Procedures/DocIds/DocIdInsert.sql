CREATE PROCEDURE [dbo].[DocIdInsert]
	@DocIdValue char(5),
	@DocTypeId smallint
AS
	insert into dbo.DocIds (DocIdValue, DocTypeId)
	values (@DocIdValue, @DocTypeId)

	select SCOPE_IDENTITY() as DocIdId
RETURN 0

grant execute on [dbo].[DocIdInsert] to [db_executor]
