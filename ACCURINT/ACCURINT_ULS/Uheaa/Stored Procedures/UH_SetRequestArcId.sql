CREATE PROCEDURE [accurint].[UH_SetRequestArcId]
	@RequestArcId BIGINT,
	@DemosId INT
AS
		UPDATE
			[accurint].DemosProcessingQueue_UH
		SET
			RequestArcId = @RequestArcId
		WHERE
			DemosId = @DemosId
RETURN 0