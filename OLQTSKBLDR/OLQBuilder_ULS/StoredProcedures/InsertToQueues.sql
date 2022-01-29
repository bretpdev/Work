CREATE PROCEDURE [olqtskbldr].[InsertToQueues]
	@QueueTable QueueTable READONLY,
	@SourceFilename VARCHAR(200) NULL
AS
	DECLARE @Now DATETIME = GETDATE()
	INSERT INTO [olqtskbldr].Queues(TargetId,QueueName,InstitutionId,InstitutionType,DateDue,TimeDue,Comment,SourceFilename,AddedAt,AddedBy)
	SELECT
		NewQueues.TargetId,
		NewQueues.QueueName,
		NewQueues.InstitutionId,
		NewQueues.InstitutionType,
		NewQueues.DateDue,
		NewQueues.TimeDue,
		NewQueues.Comment,
		@SourceFilename,
		@Now,
		SUSER_NAME()
	FROM
		@QueueTable NewQueues
		LEFT JOIN [olqtskbldr].Queues Queues
			ON NewQueues.TargetId = Queues.TargetId
			AND NewQueues.QueueName = Queues.QueueName 
			AND NewQueues.InstitutionId = Queues.InstitutionId
			AND NewQueues.InstitutionType = Queues.InstitutionType
			AND NewQueues.DateDue = Queues.DateDue
			AND NewQueues.TimeDue = Queues.TimeDue
			AND NewQueues.Comment = Queues.Comment
			AND ISNULL(Queues.SourceFilename,'') = ISNULL(@SourceFilename,'')
			AND CAST(Queues.AddedAt AS DATE) = CAST(@Now AS DATE) 
			AND Queues.DeletedAt IS NULL
			AND Queues.DeletedBy IS NULL
	WHERE
		Queues.QueueId IS NULL