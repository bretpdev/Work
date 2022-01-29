USE [ULS]
GO
/****** Object:  StoredProcedure [dbo].[ArcAdd_AddRecord]    Script Date: 4/27/2020 10:20:04 AM ******/
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
ALTER PROCEDURE [dbo].[ArcAdd_AddRecord]
	@AccountNumber		CHAR(10),
	@ArcTypeId			INT,
	@RecipientId		CHAR(10) = null,
	@Arc				VARCHAR(5),
	@ScriptId			CHAR(10),
	@ProcessOn			DATETIME = null,
	@Comment			VARCHAR(1233) = null,
	@IsReference		BIT = 0,
	@IsEndorser			BIT = 0,
	@ProcessTo			DATETIME = null,
	@ProcessFrom		DATETIME = null,
	@NeededBy			DATETIME = null,
	@RegardsTo			CHAR(9) = null,
	@RegardsCode		CHAR(1) = null,
	@ArcResponseCode varchar(5) = NULL
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

	INSERT INTO ArcAddProcessing(AccountNumber, ArcTypeId,ArcResponseCodeId, RecipientId, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, ProcessTo, ProcessFrom, NeededBy, RegardsTo, RegardsCode)
	VALUES(@AccountNumber, @ArcTypeId,@ArcResponseCodeId, @RecipientId, @Arc, @ScriptId, @ProcessOn, @Comment, @IsReference, @IsEndorser, @ProcessTo, @ProcessFrom, @NeededBy, @RegardsTo, @RegardsCode)

	SELECT SCOPE_IDENTITY()
END
