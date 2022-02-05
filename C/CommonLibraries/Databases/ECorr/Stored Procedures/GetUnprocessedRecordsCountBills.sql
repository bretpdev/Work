CREATE PROCEDURE [dbo].[GetUnprocessedRecordsCountBills]
	AS
BEGIN

	SET NOCOUNT ON;

	SELECT
		count(*)

	FROM 
		[dbo].[DocumentDetails] DD
	INNER JOIN [dbo].[Letters] LTRS
		ON LTRS.LetterId = DD.LetterId
	WHERE 
		[Printed] IS NULL
		and dd.LetterId in (1027,1028,1029)
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetUnprocessedRecordsCountBills] TO [db_executor]
    AS [dbo];
