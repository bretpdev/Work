CREATE PROCEDURE [pifltr].[UpdateQueueCompleter]
@ProcessQueueId INT,
@Ssn VARCHAR(9),
@TaskControlNumber VARCHAR(18),
@Queue VARCHAR(2),
@SubQueue VARCHAR(2)

AS

DECLARE @QueueCompleteIds TABLE(QueueId INT, TaskControlNumber VARCHAR(18), AccountIdentifier VARCHAR(9))

DECLARE @TaskStatusId INT =(SELECT TaskStatusId From [Uls].[quecomplet].[TaskStatuses] WHERE TaskStatus = 'C')
DECLARE @ActionResponseId INT = (SELECT ActionResponseId FROM [ULS].[quecomplet].[ActionResponses] WHERE ActionResponse = '')
	
	INSERT INTO [Uls].[quecomplet].[Queues](Queue,SubQueue,TaskControlNumber, AccountIdentifier ,TaskStatusID, ActionResponseId,AddedAt,AddedBy)
	OUTPUT INSERTED.QueueId, INSERTED.TaskControlNumber, INSERTED.AccountIdentifier INTO @QueueCompleteIds(QueueId, TaskControlNumber,AccountIdentifier )

Select
	@Queue AS Queue,
	@SubQueue AS SubQueue,
	@TaskControlNumber AS TaskControlNumber,
	@Ssn as AccountIdentifier,
	@TaskStatusId AS TaskStatusID,
	@ActionResponseId As ActionResponseId,
	GETDATE() AS AddedAt,
	SUSER_NAME() AS AddedBy


UPDATE   
	PQ
SET 
	QueueCompleterId = QC.QueueId
FROM
	ULS.pifltr.ProcessingQueue PQ	
	INNER  JOIN @QueueCompleteIds QC
		ON QC.TaskControlNumber = PQ.TaskControlNumber
		AND QC.AccountIdentifier = AccountIdentifier
WHERE 
	PQ.ProcessQueueId = @ProcessQueueId
		
RETURN 0
GO