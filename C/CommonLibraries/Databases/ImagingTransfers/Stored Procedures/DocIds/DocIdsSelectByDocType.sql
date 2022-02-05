CREATE PROCEDURE [dbo].[DocIdsSelectByDocType]
	@DocTypeId smallint
AS
	select DocIdId, DocIdValue
	  from dbo.DocIds
	 where RemovedBy is null
	   and DocTypeId = @DocTypeId
RETURN 0

grant execute on [dbo].[DocIdsSelectByDocType] to [db_executor]