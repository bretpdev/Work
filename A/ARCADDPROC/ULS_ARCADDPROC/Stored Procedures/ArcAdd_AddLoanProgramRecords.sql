﻿CREATE PROCEDURE [dbo].[ArcAdd_AddLoanProgramRecords]
(
	@LoanProgram LoanProgramRecords READONLY,
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

		INSERT INTO dbo.ArcLoanProgramSelection(ArcAddProcessingId, LoanProgram)
		SELECT @ArcAddProcessingId, LoanProgram FROM @LoanProgram

		SELECT @ArcAddProcessingId

END