CREATE PROCEDURE RequeueArcAddProcessing
(
	@region VARCHAR(11),
	@ArcAddProcessingId INT
)
AS

IF @region = 'uheaa'
	BEGIN
		UPDATE
			ULS..ArcAddProcessing
		SET
			ProcessedAt = NULL
		WHERE
			ArcAddProcessingId = @ArcAddProcessingId
	END
ELSE IF @region = 'cornerstone'
	BEGIN
		UPDATE
			CLS..ArcAddProcessing
		SET
			ProcessedAt = NULL
		WHERE
			ArcAddProcessingId = @ArcAddProcessingId
	END
ELSE
	BEGIN
		RAISERROR('RequeueArcAddProcessing - Unknown region: %s', 16, 1, @region)
	END