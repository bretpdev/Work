CREATE PROCEDURE [dbo].[QBLR_GetQueueBuilderList]
	@System varchar(7)
AS
	SELECT
		[FileName],
		[Empty],
		NoFile,
		MultiFile
	FROM
		QBLR_LST_QueueBuilderLists
	WHERE
		[System] = @System
RETURN 0

GRANT EXECUTE ON QBLR_GetQueueBuilderList TO db_executor