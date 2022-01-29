CREATE PROCEDURE [olqtskbldr].[InsertSingleRecordToQueues]
	@TargetId VARCHAR(10),
	@QueueName VARCHAR(10),
	@InstitutionId VARCHAR(20),
	@InstitutionType VARCHAR(20),
	@DateDue DATE,
	@TimeDue TIME,
	@Comment VARCHAR(200),
	@SourceFilename VARCHAR(200) NULL
AS
	DECLARE @ExistingQueueId INT = 
	(
		SELECT TOP 1 --Just adding a top 1 to prevent people breaking duplication with direct inserts causing issues
			QueueId
		FROM 
			[olqtskbldr].Queues 
		WHERE 
			TargetId = @TargetId 
			AND QueueName = @QueueName 
			AND InstitutionId = @InstitutionId
			AND InstitutionType = @InstitutionType
			AND DateDue = @DateDue
			AND TimeDue = @TimeDue
			AND Comment = @Comment
			AND ISNULL(SourceFilename,'') = ISNULL(@SourceFilename,'')
			AND CAST(AddedAt AS DATE) = CAST(GETDATE() AS DATE)
			AND DeletedAt IS NULL
			AND DeletedBy IS NULL
	)

	IF @ExistingQueueId IS NULL
		BEGIN
			INSERT INTO [olqtskbldr].Queues(TargetId,QueueName,InstitutionId,InstitutionType,DateDue,TimeDue,Comment,SourceFilename,AddedAt,AddedBy)
			VALUES
			(
				@TargetId,
				@QueueName,
				@InstitutionId,
				@InstitutionType,
				@DateDue,
				@TimeDue,
				@Comment,
				@SourceFilename,
				GETDATE(),
				SUSER_NAME()
			)
		END
RETURN 0
