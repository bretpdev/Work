CREATE PROCEDURE [olqtskbldr].[GetSasList]
AS
	SELECT 
		[FileName], 
		Empty, 
		NoFile, 
		MultiFile 
	FROM 
		QBLR_LST_QueueBuilderLists 
	WHERE 
		[System] = 'OneLINK'
RETURN 0
