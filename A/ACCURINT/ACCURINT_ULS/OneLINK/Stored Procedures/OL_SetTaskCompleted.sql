CREATE PROCEDURE [accurint].[OL_SetTaskCompleted]
	@DemosId INT
AS
		UPDATE
			[accurint].DemosProcessingQueue_OL
		SET
			TaskCompletedAt = GETDATE()
		WHERE
			DemosId = @DemosId
RETURN 0
