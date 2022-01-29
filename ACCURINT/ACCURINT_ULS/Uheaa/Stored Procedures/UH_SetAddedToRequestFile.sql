CREATE PROCEDURE [accurint].[UH_SetAddedToRequestFile]
	@DemosId INT
AS
		UPDATE
			[accurint].DemosProcessingQueue_UH
		SET
			AddedToRequestFileAt = GETDATE()
		WHERE
			DemosId = @DemosId
RETURN 0