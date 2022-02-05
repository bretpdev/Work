CREATE PROCEDURE [accurint].[OL_SetRequestCommentAdded]
	@DemosId INT
AS
		UPDATE
			[accurint].DemosProcessingQueue_OL
		SET
			RequestCommentAdded = 1
		WHERE
			DemosId = @DemosId
RETURN 0