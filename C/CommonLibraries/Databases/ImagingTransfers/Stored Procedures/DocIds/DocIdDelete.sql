CREATE PROCEDURE [dbo].[DocIdDelete]
	@DocIdId smallint
AS
	update dbo.DocIds set RemovedBy = SYSTEM_USER
	 where DocIdId = @DocIdId
RETURN 0

grant execute on [dbo].[DocIdDelete] to [db_executor]