CREATE PROCEDURE [tcpapns].[GetBulkLoadCount]
AS
	SELECT COUNT(*) FROM [tcpapns]._BulkLoad
RETURN 0