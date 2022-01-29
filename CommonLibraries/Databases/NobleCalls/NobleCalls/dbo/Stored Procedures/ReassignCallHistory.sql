CREATE PROCEDURE ReassignCallHistory 
(
	@NobleCallHistoryId INT,
	@ArcAddProcessingId INT
)
AS
BEGIN 

	BEGIN TRANSACTION
		DECLARE @ERROR INT = 0
		DECLARE @ROWCOUNT INT = 0

		UPDATE 
			[NobleCalls].[dbo].[NobleCallHistory]
		SET
			CallCampaign = 'UHEX',
			RegionId = 2
		WHERE
			NobleCallHistoryId = @NobleCallHistoryId

		-- Save/Set the row count and error number (if any) from the previously executed statement
		SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR	

		INSERT INTO 
			ULS..ArcAddProcessing([ArcTypeId],[ArcResponseCodeId],[AccountNumber],[RecipientId],[ARC],[ScriptId],[ProcessOn],[Comment],[IsReference],[IsEndorser],[ProcessFrom],[ProcessTo],[NeededBy],[RegardsTo],[RegardsCode],[CreatedAt],[CreatedBy])
		SELECT
			[ArcTypeId],
			UARC.[ArcResponseCodeId],
			[AccountNumber],
			[RecipientId],
			[ARC],
			[ScriptId],
			[ProcessOn],
			[Comment],
			[IsReference],
			[IsEndorser],
			[ProcessFrom],
			[ProcessTo],
			[NeededBy],
			[RegardsTo],
			[RegardsCode],
			[CreatedAt],
			[CreatedBy]
		FROM
			CLS..ArcAddProcessing AAP
			INNER JOIN CLS..ArcResponseCodes CARC ON CARC.ArcResponseCodeId = AAP.ArcResponseCodeId
			INNER JOIN ULS..ArcResponseCodes UARC ON UARC.ResponseCode = CARC.ResponseCode
		WHERE
			ArcAddProcessingId = @ArcAddProcessingId

		-- Save/Set the row count and error number (if any) from the previously executed statement
		SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	IF @ROWCOUNT = 2 AND @ERROR = 0
		BEGIN
			PRINT 'Transaction committed'
			COMMIT TRANSACTION
			--ROLLBACK TRANSACTION
		END
	ELSE
		BEGIN
			PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
			PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
			PRINT 'Transaction NOT committed'
			ROLLBACK TRANSACTION
		END
END