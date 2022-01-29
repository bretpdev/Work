CREATE PROCEDURE [mssasgndft].[UpdateQueue]
	@QueueId INT,
	@FutureDated BIT
AS
BEGIN
	UPDATE
		[mssasgndft].[Queues]
	SET
		FutureDated = @FutureDated
	WHERE
		QueueId = @QueueId
END