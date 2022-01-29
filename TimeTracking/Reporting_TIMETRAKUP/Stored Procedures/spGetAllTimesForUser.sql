CREATE PROCEDURE [dbo].[spGetAllTimesForUser] 
	@SqlUserID varchar(50),
	@UnstoppedTime bit,
	@TicketId int = null
AS
	SELECT
		TimeTrackingId,
		TicketID,
		StartTime,
		EndTime,
		Region
	FROM
		Reporting.dbo.TimeTracking
	WHERE
		((@SqlUserID > 0 AND SqlUserID = @SqlUserID) OR @SqlUserID = 0)
		AND (EndTime IS NULL OR @UnstoppedTime = 0)
		AND (@TicketId IS NULL OR TicketID = @TicketId)