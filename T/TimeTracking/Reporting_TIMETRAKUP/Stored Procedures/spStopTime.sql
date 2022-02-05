CREATE PROCEDURE [dbo].[spStopTime]
	@SqlUserID int = 0,
	@TicketID int = 0,
	@EndTime datetime
AS
	UPDATE
		TimeTracking
	SET
		EndTime = @EndTime
	WHERE
		SqlUserID = @SqlUserID
		AND TicketID = @TicketID
		AND EndTime IS NULL
		AND Region = 'uheaa'