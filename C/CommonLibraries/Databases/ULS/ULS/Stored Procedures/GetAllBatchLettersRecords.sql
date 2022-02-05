CREATE PROCEDURE [dbo].[GetAllBatchLettersRecords]

AS
BEGIN
	SELECT 
		[BatchLettersId],
		[LetterId],
		[SasFilePattern],
		[StateFieldCodeName],
		[AccountNumberFieldName],
		[CostCenterFieldCodeName],
		[IsDuplex],
		[OkIfMissing],
		[ProcessAllFiles],
		[ARC],
		[Comment],
		[CreatedAt],
		[CreatedBy],
		[UpdatedAt],
		[UpdatedBy],
		[Active]

	FROM
		[dbo].[BatchLetters]
END


GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllBatchLettersRecords] TO [db_executor]
    AS [dbo];
RETURN 0

