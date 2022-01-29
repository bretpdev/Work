-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/28/2013
-- Description:	Gets all active letters from [dbo].[BatchLettersFed]
-- =============================================
CREATE PROCEDURE [dbo].[GetActiveBatchLetters]

AS
BEGIN

	SET NOCOUNT ON;

	SELECT 
		[BatchLettersFedId],
		[LetterId],
		[SasFilePattern],
		[StateFieldCodeName],
		[AccountNumberFieldName],
		[AccountNumberIndex],
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
	WHERE Active = 1
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetActiveBatchLetters] TO [db_executor]
    AS [dbo];



