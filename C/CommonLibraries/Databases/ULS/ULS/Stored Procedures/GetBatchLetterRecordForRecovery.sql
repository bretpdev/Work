CREATE PROCEDURE [dbo].[GetBatchLetterRecordForRecovery]
	@Id int 
	
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
	WHERE 
		BatchLettersId = @Id
END


GRANT EXECUTE
    ON OBJECT::[dbo].[GetBatchLetterRecordForRecovery] TO [db_executor]
    AS [dbo];
RETURN 0
