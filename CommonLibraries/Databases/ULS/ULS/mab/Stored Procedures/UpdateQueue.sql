
CREATE PROCEDURE [mab].[UpdateQueue]
	@QueueId int,
	@FutureDated bit
AS
BEGIN
	UPDATE
		[mab].[CollQueue]
	SET
		FutureDated = @FutureDated
	WHERE
		QueueId = @QueueId
END
GO
GRANT EXECUTE
    ON OBJECT::[mab].[UpdateQueue] TO [db_executor]
    AS [dbo];

