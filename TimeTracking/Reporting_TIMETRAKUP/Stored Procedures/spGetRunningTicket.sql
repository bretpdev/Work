CREATE PROCEDURE [dbo].[spGetRunningTicket]
	@TicketID int = 0, 
	@SqlUserID int = 0
AS
	SELECT
		StartTime
	FROM
		TimeTracking
	WHERE
		SqlUserID = @SqlUserID
		AND TicketID = @TicketID
		AND EndTime IS NULL
		AND Region = 'uheaa'