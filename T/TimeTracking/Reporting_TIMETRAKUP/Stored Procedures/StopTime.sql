CREATE PROCEDURE [dbo].[StopTime] 
	@TimeTrackingId int,
	@EndTime datetime
AS
	UPDATE
		TimeTracking
	SET
		EndTime = @EndTime
	WHERE
		TimeTrackingId = @TimeTrackingId