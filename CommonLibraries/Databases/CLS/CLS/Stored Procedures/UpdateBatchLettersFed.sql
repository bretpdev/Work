-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/29/2013
-- Description:	Updates a batch letters fed record
-- =============================================
CREATE PROCEDURE [dbo].[UpdateBatchLettersFed]


@BatchLettersFedId int,
@LetterId VARCHAR(10),
@SasFilePattern VARCHAR(50),
@StateFieldCodeName VARCHAR(25),
@AccountNumberFieldName VARCHAR(25),
@AccountNumberFieldIndex INT,
@CostCenterFieldCodeName VARCHAR(25) = NULL,
@OkIfMissing BIT,
@ProcessAllFiles BIT,
@Arc VARCHAR(5) = NULL,
@Comment VARCHAR(1200) = NULL,
@UpdatedBy varchar(250),
@Active Bit,
@BorrowerSsnIndex int = -1,
@DoNotProcessEcorr bit



AS
BEGIN
	SET NOCOUNT ON;

	UPDATE
		[dbo].[BatchLettersFed]
	SET 
		[LetterId] = @LetterId,
		[SasFilePattern] = @SasFilePattern,
		[StateFieldCodeName] = @StateFieldCodeName,
		[AccountNumberFieldName] = @AccountNumberFieldName,
		[CostCenterFieldCodeName] = @CostCenterFieldCodeName,
		[OkIfMissing] = @OkIfMissing,
		[ProcessAllFiles] = @ProcessAllFiles,
		[Arc] = @Arc,
		[Comment] = @Comment,
		[UpdatedBy] = @UpdatedBy,
		[UpdatedAt] = GetDate(),
		[Active] = @Active,
		[AccountNumberIndex] = @AccountNumberFieldIndex,
		[BorrowerSsnIndex] = @BorrowerSsnIndex,
		[DoNotProcessEcorr] = @DoNotProcessEcorr
	WHERE
		[BatchLettersFedId] = @BatchLettersFedId
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[UpdateBatchLettersFed] TO [db_executor]
    AS [dbo];



