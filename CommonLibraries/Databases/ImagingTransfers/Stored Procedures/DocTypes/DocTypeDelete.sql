CREATE PROCEDURE [dbo].[DocTypeDelete]
	@DocTypeId int
AS
	update dbo.DocTypes
	   set RemovedBy = SYSTEM_USER
	 where DocTypeId = @DocTypeId
RETURN 0

grant execute on [dbo].[DocTypeDelete] to [db_executor]