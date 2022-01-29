CREATE PROCEDURE [mssasgndft].[InsertQueue]
	@QueueName VARCHAR(8),
	@FutureDated BIT
AS
	INSERT INTO mssasgndft.Queues(QueueName, FutureDated)
	VALUES(@QueueName, @FutureDated)