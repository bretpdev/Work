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