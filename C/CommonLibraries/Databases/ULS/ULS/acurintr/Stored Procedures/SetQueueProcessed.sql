CREATE PROCEDURE [acurintr].[SetQueueProcessed]
	@CompassQueueId int
AS
	
	UPDATE
		acurintr.CompassQueue
	SET
		ProcessedAt = GETDATE()
	WHERE
		CompassQueueID = @CompassQueueId

RETURN 0