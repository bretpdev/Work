CREATE PROCEDURE [dbo].[ArcAdd_CleanUpArcs]

AS
BEGIN

SET NOCOUNT ON;
BEGIN TRANSACTION

	UPDATE
		ArcAddProcessing
	SET
		ProcessedAt = NULL
	WHERE
		LN_ATY_SEQ IS NULL
		AND DATEDIFF(HOUR, ProcessedAt, GETDATE()) > 2
		AND ProcessedAt IS NOT NULL
		AND ProcessingAttempts < 2
		AND ProcessOn <= GETDATE()
		AND RTRIM(ISNULL(ActivityType,'')) = '' --Added to make sure this does not interfere with Onelink AAP

	IF @@ERROR = 0
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

