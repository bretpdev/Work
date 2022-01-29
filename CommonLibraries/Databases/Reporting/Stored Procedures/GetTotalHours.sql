CREATE PROCEDURE [dbo].[GetTotalHours]
	@SqlUserId int,
	@StartDate DateTime,
	@EndDate DateTime
AS
	SELECT
		StartTime,
		EndTime
	FROM
		TimeTracking
	WHERE
		StartTime > @StartDate
		AND EndTime < @EndDate
		
RETURN 0

GRANT EXECUTE ON [dbo].[GetTotalHours] TO db_executor
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[GetTotalHours] TO [db_executor]
    AS [dbo];

