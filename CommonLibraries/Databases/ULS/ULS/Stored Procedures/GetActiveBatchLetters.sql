CREATE PROCEDURE [dbo].[GetActiveBatchLetters]

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
	WHERE Active = 1
END


GRANT EXECUTE
    ON OBJECT::[dbo].[GetActiveBatchLetters] TO [db_executor]
    AS [dbo];
RETURN 0

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetActiveBatchLetters] TO [db_executor]
    AS [dbo];

