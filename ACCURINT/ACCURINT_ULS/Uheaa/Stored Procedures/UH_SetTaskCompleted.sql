CREATE PROCEDURE [accurint].[UH_SetTaskCompleted]
	@DemosId INT
AS
		UPDATE
			[accurint].DemosProcessingQueue_UH
		SET
			TaskCompletedAt = GETDATE()
		WHERE
			DemosId = @DemosId
RETURN 0