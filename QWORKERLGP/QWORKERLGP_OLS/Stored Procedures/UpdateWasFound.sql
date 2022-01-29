CREATE PROCEDURE [qworkerlgp].[UpdateWasFound]
	@WasFound BIT,
	@QueueId INT
AS
BEGIN
	UPDATE
		[qworkerlgp].Queues
	SET
		WasFound = @WasFound
	WHERE
		QueueId = @QueueId
END

GO