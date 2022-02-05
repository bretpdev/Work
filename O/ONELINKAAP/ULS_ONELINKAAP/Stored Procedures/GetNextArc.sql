CREATE PROCEDURE [onelinkaap].[GetNextArc]

AS
BEGIN

SET NOCOUNT ON;
BEGIN TRANSACTION

	DECLARE @ArcAddProcessingId BIGINT
	DECLARE @ERROR INT

	DECLARE @AAPIds
		TABLE
		(
			ArcAddProcessingId BIGINT NOT NULL
		);

	SELECT @ERROR = @@ERROR

	UPDATE TOP (1)
		ArcAddProcessing
	SET
		ProcessedAt = GETDATE(),
		ProcessingAttempts = (ProcessingAttempts + 1)
	OUTPUT inserted.ArcAddProcessingId
	INTO @AAPIds
	WHERE
		ProcessedAt IS NULL
		AND ProcessOn <= GETDATE()
		AND RTRIM(ISNULL(ActivityType,'')) != '' --Added for onelink only aap update
	
	SELECT @ERROR = @ERROR + @@ERROR

	SELECT
		AAP.ArcAddProcessingId,
		AAP.ArcTypeId,
		AAP.AccountNumber,
		AAP.RecipientId,
		AAP.ARC,
		AAP.ScriptId,
		AAP.Comment,
		AAP.IsReference,
		AAP.IsEndorser,
		AAP.ProcessFrom,
		AAP.ProcessTo,
		AAP.NeededBy,
		AAP.RegardsTo,
		AAP.RegardsCode,
		ARC.ResponseCode,
		AAP.ActivityType,
		AAP.ActivityContact,
		AAP.ProcessingAttempts
	FROM 
		ArcAddProcessing AAP
		INNER JOIN @AAPIds AAPIds
			ON AAPIds.ArcAddProcessingId = AAP.ArcAddProcessingId
		LEFT JOIN ArcResponseCodes ARC
			ON ARC.ArcResponseCodeId = AAP.ArcResponseCodeId
	
	
	SELECT @ERROR = @ERROR + @@ERROR

	IF @ERROR = 0
		BEGIN
			COMMIT TRANSACTION
		END
	ELSE
		BEGIN
			RAISERROR ('Failed to retrieve dbo.ArcAddProcessing record for processing.', -- error message
				   16, -- Severity.
				   1 -- State.
				   );
			ROLLBACK TRANSACTION
		END
	END
