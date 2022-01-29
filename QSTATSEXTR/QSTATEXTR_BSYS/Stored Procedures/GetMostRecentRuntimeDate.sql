CREATE PROCEDURE [qstatsextr].[GetMostRecentRuntimeDate]
AS
	
	SELECT
		MAX(RuntimeDate)
	FROM
		QSTA_DAT_QueueData

RETURN 0
