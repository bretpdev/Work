CREATE PROCEDURE [dbo].[DocTypesSelectAll]
AS
	select DocTypeId, DocTypeValue
	  from dbo.DocTypes
	 where RemovedBy is null
RETURN 0

grant execute on [dbo].[DocTypesSelectAll] to [db_executor]