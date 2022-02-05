CREATE PROCEDURE [dbo].[ArcAdd_AddLoanSequenceRecords]
(
	@LoanSequence		LoanSequenceRecords READONLY,
	@AccountNumber		CHAR(10),
	@ArcTypeId			INT,
	@RecipientId		CHAR(10),
	@Arc				VARCHAR(5),
	@ScriptId			CHAR(10),
	@ProcessOn			DATETIME = null,
	@Comment			VARCHAR(300) = null,
	@IsReference		BIT = 0,
	@IsEndorser			BIT = 0,
	@ProcessTo			DATETIME = null,
	@ProcessFrom		DATETIME = null,
	@NeededBy			DATETIME = null,
	@RegardsTo			CHAR(9) = null,
	@RegardsCode		CHAR(1) = null,
	@ArcResponseCode	varchar(5) = NULL
)

AS
BEGIN
		IF @ProcessOn IS NULL
		SET @ProcessOn = GETDATE()

		DECLARE @ArcResponseCodeId INT = NULL

		IF @ArcResponseCode IS NOT NULL
			BEGIN
				SET @ArcResponseCodeId = (SELECT TOP 1 ArcResponseCodeId FROM ArcResponseCodes WHERE ResponseCode = @ArcResponseCode)
				IF @ArcResponseCodeId IS NULL
					BEGIN
						INSERT INTO ArcResponseCodes(ResponseCode)
						VALUES(@ArcResponseCode)
						SET @ArcResponseCodeId = SCOPE_IDENTITY()
					END
		END

		DECLARE @ArcAddProcessingId int
		INSERT INTO dbo.ArcAddProcessing(AccountNumber, ArcTypeId, ArcResponseCodeId, RecipientId, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, ProcessTo, ProcessFrom, NeededBy, RegardsTo, RegardsCode)
		VALUES(@AccountNumber, @ArcTypeId, @ArcResponseCodeId, @RecipientId, @Arc, @ScriptId, @ProcessOn, @Comment, @IsReference, @IsEndorser, @ProcessTo, @ProcessFrom, @NeededBy, @RegardsTo, @RegardsCode)

		SET @ArcAddProcessingId = (SELECT SCOPE_IDENTITY())

		INSERT INTO dbo.ArcLoanSequenceSelection(ArcAddProcessingId, LoanSequence)
		SELECT @ArcAddProcessingId, LoanSequence FROM @LoanSequence

		SELECT @ArcAddProcessingId

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_AddLoanSequenceRecords] TO [db_executor]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_AddLoanSequenceRecords] TO [UHEAA\SystemAnalysts]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_AddLoanSequenceRecords] TO [UHEAA\CornerStoneUsers]
    AS [dbo];

