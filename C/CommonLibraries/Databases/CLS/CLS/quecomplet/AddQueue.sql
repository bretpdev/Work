CREATE PROCEDURE [quecomplet].[AddQueue]
	@Queue varchar(2),
	@Subqueue varchar(2),
	@TaskControlNumber varchar(20),
	@AccountNumber varchar(10),
	@TaskStatusId int,
	@ActionResponseId int,
	@AddedBy varchar(100)
AS
	IF NOT EXISTS (SELECT * FROM CLS.quecomplet.Queues WHERE [Queue] = @Queue AND SubQueue = @Subqueue AND TaskControlNumber = @TaskControlNumber)
	BEGIN
		INSERT INTO CLS.[quecomplet].Queues([Queue], SubQueue, TaskControlNumber, AccountIdentifier, TaskStatusId, ActionResponseId, AddedBy)
		VALUES(@Queue, @Subqueue, @TaskControlNumber, @AccountNumber, @TaskStatusId, @ActionResponseId, @AddedBy)
	END
RETURN 0