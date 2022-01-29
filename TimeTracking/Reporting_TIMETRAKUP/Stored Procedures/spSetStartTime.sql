CREATE PROCEDURE [dbo].[spSetStartTime]
	@SqlUserID int = 0,
	@TicketID int = 0,
	@Region varchar(11) = 'uheaa',
	@StartTime datetime
AS
	UPDATE
		TimeTracking
	SET
		EndTime = GETDATE()
	WHERE
		EndTime IS NULL
		AND SqlUserID = @SqlUserID

	INSERT INTO TimeTracking(SqlUserID, TicketID, StartTime, Region)
	VALUES(@SqlUserID, @TicketID, @StartTime, @Region)