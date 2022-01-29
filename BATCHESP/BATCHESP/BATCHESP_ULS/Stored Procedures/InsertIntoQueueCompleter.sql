CREATE PROCEDURE [batchesp].[InsertIntoQueueCompleter]
	@Queue CHAR(2),
	@SubQueue CHAR(2),
	@TaskControlNumber VARCHAR(18),
	@AccountNumber CHAR(10)

AS
	
DECLARE 
	@TaskStatusId INT = (SELECT TaskStatusId FROM [quecomplet].[TaskStatuses] WHERE TaskStatus = 'C'),
	@ActionResponseId INT = (SELECT ActionResponseId FROM [quecomplet].[ActionResponses] WHERE ActionResponse = '')
	
INSERT INTO [quecomplet].[Queues](Queue,SubQueue,TaskControlNumber, AccountIdentifier ,TaskStatusID, ActionResponseId,AddedAt,AddedBy)	
SELECT
	@Queue [Queue],
	@SubQueue [SubQueue],
	@TaskControlNumber [TaskControlNumber],
	@AccountNumber [AccountIdentifier],
	@TaskStatusId [TaskStatusID],
	@ActionResponseId [ActionResponseId],
	GETDATE() [AddedAt],
	SUSER_NAME() [AddedBy]

RETURN 0
