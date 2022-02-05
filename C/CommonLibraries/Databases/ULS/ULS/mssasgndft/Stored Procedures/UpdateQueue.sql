CREATE PROCEDURE [mssasgndft].[UpdateQueue]
	@QueueId int,
	@FutureDated bit
AS
BEGIN
	UPDATE
		[mssasgndft].[Queues]
	SET
		FutureDated = @FutureDated
	WHERE
		QueueId = @QueueId
END

GRANT EXECUTE ON [mssasgndft].[CollQueue] TO db_executor