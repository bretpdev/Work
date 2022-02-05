CREATE PROCEDURE [accurint].[OL_SetAddedToRequestFile]
	@DemosId INT
AS
		UPDATE
			[accurint].DemosProcessingQueue_OL
		SET
			AddedToRequestFileAt = GETDATE()
		WHERE
			DemosId = @DemosId
RETURN 0