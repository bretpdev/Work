
CREATE PROCEDURE [dbo].[GetUnprocessedLenderUpdates]
AS
	SELECT 
		[LenderUpdateId]
		,[MOD]
		,[LenderId]
		,[FullName]
		,[ShortName]
		,[Address1]
		,[Address2]
		,[City]
		,[State]
		,[Zip]
		,[Valid]
		,[DateVerified]
		,[Type]
		,[AddedAt]
		,[AddedBy]
		,[ProcessedAt]
		,[ProcessedBy]
	FROM [LenderUpdates]
	WHERE [ProcessedAt]  IS NULL

RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnprocessedLenderUpdates] TO [db_executor]
    AS [dbo];

