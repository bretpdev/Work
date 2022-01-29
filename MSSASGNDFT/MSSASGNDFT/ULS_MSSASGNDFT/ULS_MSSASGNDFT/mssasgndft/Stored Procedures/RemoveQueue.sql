CREATE PROCEDURE [mssasgndft].[RemoveQueue]
	@QueueId VARCHAR(8)
AS
	UPDATE
		mssasgndft.Queues
	SET
		DeletedAt = GETDATE(),
		DeletedBy = SYSTEM_USER
	WHERE
		QueueId = @QueueId