-- =============================================
-- Author:		Jarom Ryan
-- Create date: 10/29/2013
-- Description:	adds a record to batch letters fed
-- =============================================
CREATE PROCEDURE [dbo].[AddRecordBatchLetter]

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
@CreatedBy varchar(250),
@BorrowerSsnIndex int  = -1,
@DoNotProcessEcorr bit

AS
BEGIN

	SET NOCOUNT ON;

	INSERT INTO [dbo].[BatchLettersFed]([LetterId],[SasFilePattern],[StateFieldCodeName],[AccountNumberFieldName],[CostCenterFieldCodeName],[OkIfMissing],[ProcessAllFiles],[Arc],[Comment],[CreatedBy],[AccountNumberIndex], [BorrowerSsnIndex], [DoNotProcessEcorr])
	VALUES(@LetterId, @SasFilePattern, @StateFieldCodeName, @AccountNumberFieldName, @CostCenterFieldCodeName, @OkIfMissing, @ProcessAllFiles, @Arc, @Comment, @CreatedBy, @AccountNumberFieldIndex, @BorrowerSsnIndex, @DoNotProcessEcorr)
	
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddRecordBatchLetter] TO [db_executor]
    AS [dbo];



