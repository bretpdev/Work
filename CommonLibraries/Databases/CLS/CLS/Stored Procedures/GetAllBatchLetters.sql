
-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/28/2013
-- Description:	Gets all  letters from [dbo].[BatchLettersFed]
-- =============================================
CREATE PROCEDURE [dbo].[GetAllBatchLetters]

AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		[BatchLettersFedId],
		[LetterId],
		[SasFilePattern],
		[StateFieldCodeName],
		[AccountNumberFieldName],
		[AccountNumberIndex] AS AccountNumberFieldIndex,
		[CostCenterFieldCodeName],
		[OkIfMissing],
		[ProcessAllFiles],
		[ARC],
		[Comment],
		[CreatedAt],
		[CreatedBy],
		[UpdatedAt],
		[UpdatedBy],
		[Active],
		[BorrowerSsnIndex],
		[DoNotProcessEcorr]

	FROM
		[dbo].[BatchLettersFed]
END

grant execute on [dbo].[GetAllBatchLetters] to db_executor


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetAllBatchLetters] TO [db_executor]
    AS [dbo];



