CREATE PROCEDURE [accurint].[OL_SetAddressTaskQueueId]
	@AddressTaskQueueId INT,
	@DemosId INT
AS
		UPDATE
			[accurint].DemosProcessingQueue_OL
		SET
			AddressTaskQueueId = @AddressTaskQueueId
		WHERE
			DemosId = @DemosId
RETURN 0