CREATE PROCEDURE [accurint].[OL_SetPhoneTaskQueueId]
	@PhoneTaskQueueId INT,
	@DemosId INT
AS
		UPDATE
			[accurint].DemosProcessingQueue_OL
		SET
			PhoneTaskQueueId = @PhoneTaskQueueId
		WHERE
			DemosId = @DemosId
RETURN 0
