CREATE PROCEDURE [acurintc].[GetQueueInfo]
AS
	
	SELECT
		QueueInfoId,
		Queue,
		SubQueue,
		DemographicsReviewQueue,
		ForeignReviewQueue,
		ParserId,
		ProcessorId,
		AddedAt,
		AddedBy
	FROM
		QueueInfo
RETURN 0
