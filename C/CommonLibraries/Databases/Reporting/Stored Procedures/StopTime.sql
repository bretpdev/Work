CREATE PROCEDURE [dbo].[StopTime] 
	@TimeTrackingId int,
	@EndTime datetime
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE
		TimeTracking
	SET
		EndTime = @EndTime
	WHERE
		TimeTrackingId = @TimeTrackingId
END


GO
GRANT EXECUTE
    ON OBJECT::[dbo].[StopTime] TO [db_executor]
    AS [dbo];