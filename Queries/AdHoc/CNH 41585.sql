USE [CLS]
GO
/****** Object:  StoredProcedure [dbo].[ArcAdd_AddRecord]    Script Date: X/XX/XXXX XX:XX:XX AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


ALTER PROCEDURE [dbo].[ArcAdd_AddRecord]
	@AccountNumber		CHAR(XX),
	@ArcTypeId			INT,
	@RecipientId		CHAR(XX) = null,
	@Arc				VARCHAR(X),
	@ScriptId			CHAR(XX),
	@ProcessOn			DATETIME = null,
	@Comment			VARCHAR(XXXX) = null,
	@IsReference		BIT = X,
	@IsEndorser			BIT = X,
	@ProcessTo			DATETIME = null,
	@ProcessFrom		DATETIME = null,
	@NeededBy			DATETIME = null,
	@RegardsTo			CHAR(X) = null,
	@RegardsCode		CHAR(X) = null,
	@ArcResponseCode	VARCHAR(X) = NULL
AS
BEGIN

	IF @ProcessOn IS NULL
	SET @ProcessOn = GETDATE()

	DECLARE @ArcResponseCodeId INT = NULL
	IF @ArcResponseCode IS NOT NULL
		BEGIN
			SET @ArcResponseCodeId = (SELECT TOP X ArcResponseCodeId FROM ArcResponseCodes WHERE ResponseCode = @ArcResponseCode)
			IF @ArcResponseCodeId IS NULL
				BEGIN
					INSERT INTO ArcResponseCodes(ResponseCode)
					VALUES(@ArcResponseCode)
					SET @ArcResponseCodeId = SCOPE_IDENTITY()
				END
	END

	INSERT INTO dbo.ArcAddProcessing (AccountNumber, ArcTypeId, ArcResponseCodeId, RecipientId, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, ProcessTo, ProcessFrom, NeededBy, RegardsTo, RegardsCode)
	VALUES(@AccountNumber, @ArcTypeId, @ArcResponseCodeId, @RecipientId, @Arc, @ScriptId, @ProcessOn, @Comment, @IsReference, @IsEndorser, @ProcessTo, @ProcessFrom, @NeededBy, @RegardsTo, @RegardsCode)

	SELECT SCOPE_IDENTITY() 

END




