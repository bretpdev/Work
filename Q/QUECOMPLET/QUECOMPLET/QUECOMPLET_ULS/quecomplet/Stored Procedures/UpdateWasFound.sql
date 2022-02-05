CREATE PROCEDURE [quecomplet].[UpdateWasFound]
	@WasFound BIT,
	@QueueId INT
AS
BEGIN
	UPDATE
		[quecomplet].Queues
	SET
		WasFound = @WasFound
	WHERE
		QueueId = @QueueId
END

GO