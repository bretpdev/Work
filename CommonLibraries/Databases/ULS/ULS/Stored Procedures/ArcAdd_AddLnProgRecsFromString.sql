CREATE PROCEDURE [dbo].[ArcAdd_AddLnProgRecsFromString]
(
	@AccountNumber		CHAR(10),
	@ArcTypeId			INT,
	@RecipientId		CHAR(10) = null,
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
	@LoanPrograms		VARCHAR(300) = null
)

AS
BEGIN
		IF @ProcessOn IS NULL
		SET @ProcessOn = GETDATE()

		DECLARE @ArcAddProcessingId BIGINT
		INSERT INTO dbo.ArcAddProcessing (AccountNumber, ArcTypeId, RecipientId, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, ProcessTo, ProcessFrom, NeededBy, RegardsTo, RegardsCode)
		VALUES(@AccountNumber, @ArcTypeId, @RecipientId, @Arc, @ScriptId, @ProcessOn, @Comment, @IsReference, @IsEndorser, @ProcessTo, @ProcessFrom, @NeededBy, @RegardsTo, @RegardsCode)

		SET @ArcAddProcessingId = (SELECT SCOPE_IDENTITY())

		INSERT INTO dbo.ArcLoanProgramSelection(ArcAddProcessingId, LoanProgram)
		SELECT @ArcAddProcessingId, val FROM SplitString(',',@LoanPrograms)

		SELECT @ArcAddProcessingId

END
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_AddLnProgRecsFromString] TO [db_executor]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_AddLnProgRecsFromString] TO [UHEAA\SystemAnalysts]
    AS [dbo];




GO
GRANT EXECUTE
    ON OBJECT::[dbo].[ArcAdd_AddLnProgRecsFromString] TO [UHEAA\CornerStoneUsers]
    AS [dbo];


