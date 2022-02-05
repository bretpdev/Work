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
		((@StartDate <= SS.EndAt) AND (SS.EndAt >= @EndDate)) 
RETURN 0
