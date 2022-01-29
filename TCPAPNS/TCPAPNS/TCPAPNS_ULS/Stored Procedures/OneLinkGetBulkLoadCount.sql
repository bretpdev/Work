CREATE PROCEDURE [tcpapns].[OneLinkGetBulkLoadCount]
AS
	SELECT COUNT(*) FROM [tcpapns]._OneLinkBulkLoad
RETURN 0