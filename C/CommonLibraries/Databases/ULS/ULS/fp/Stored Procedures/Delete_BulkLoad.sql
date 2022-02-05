CREATE PROCEDURE [fp].[Delete_BulkLoad]
AS
	DELETE FROM [fp]._BulkLoad
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[fp].[Delete_BulkLoad] TO [db_executor]
    AS [UHEAA\Developers];

