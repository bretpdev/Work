-- =============================================
-- Author:		Jarom Ryan
-- Create date: 02/14/2014
-- Description:	Gets a recovery record for batch letters
-- =============================================
CREATE PROCEDURE [dbo].[GetBatchLetterRecordForRecovery] 

@Id int

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
	WHERE 
		BatchLettersFedId = @Id
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetBatchLetterRecordForRecovery] TO [db_executor]
    AS [dbo];



