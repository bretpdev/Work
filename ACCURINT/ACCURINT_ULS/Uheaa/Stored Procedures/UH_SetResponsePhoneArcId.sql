CREATE PROCEDURE [accurint].[UH_SetResponsePhoneArcId]
	@ResponsePhoneArcId BIGINT,
	@DemosId INT
AS
		UPDATE
			[accurint].DemosProcessingQueue_UH
		SET
			ResponsePhoneArcId = @ResponsePhoneArcId
		WHERE
			DemosId = @DemosId
RETURN 0
