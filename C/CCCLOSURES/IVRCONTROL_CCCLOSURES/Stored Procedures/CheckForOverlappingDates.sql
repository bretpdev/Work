
CREATE PROCEDURE [dbo].[CheckForOverlappingDates]
	@StartDate DATETIME,
	@EndDate DATETIME,
	@RegionId INT,
	@StatusScheduleId INT = NULL
AS
	SELECT 
		COUNT(*)
	FROM
		StatusSchedules SS
		INNER JOIN
		(
			SELECT
				*
			FROM
				StatusSchedules
			WHERE
				StatusScheduleId != @StatusScheduleId
				AND RegionId = @RegionId
				AND DeletedAt IS NULL
		)POP
			ON POP.StatusScheduleId = SS.StatusScheduleId
	WHERE
		(@StartDate BETWEEN SS.StartAt AND SS.EndAt)
		OR 
		(@EndDate BETWEEN SS.StartAt AND SS.EndAt)
		OR
		(ss.StartAt BETWEEN @StartDate AND @EndDate)
		OR 
		(SS.EndAt BETWEEN	@StartDate AND @EndDate)
		
RETURN 0