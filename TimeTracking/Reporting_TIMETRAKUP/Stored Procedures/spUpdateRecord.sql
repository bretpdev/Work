CREATE PROCEDURE [dbo].[spUpdateRecord]
	@TimeTrackingId int,
	@StartTime datetime,
	@EndTime datetime,
	@SqlUserId int
AS
	UPDATE
		TimeTracking
	SET
		StartTime = @StartTime,
		EndTime = @EndTime
	WHERE
		TimeTrackingId = @TimeTrackingId
		
	SELECT @@ROWCOUNT