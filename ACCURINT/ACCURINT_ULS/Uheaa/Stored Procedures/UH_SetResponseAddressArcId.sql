CREATE PROCEDURE [accurint].[UH_SetResponseAddressArcId]
	@ResponseAddressArcId BIGINT,
	@DemosId INT
AS
		UPDATE
			[accurint].DemosProcessingQueue_UH
		SET
			ResponseAddressArcId = @ResponseAddressArcId
		WHERE
			DemosId = @DemosId
RETURN 0