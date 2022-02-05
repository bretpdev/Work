CREATE PROCEDURE [fp].[GetBulkLoadCount]
AS
	SELECT COUNT(*) FROM [fp]._BulkLoad
RETURN 0
GO
GRANT EXECUTE
    ON OBJECT::[fp].[GetBulkLoadCount] TO [db_executor]
    AS [UHEAA\Developers];

