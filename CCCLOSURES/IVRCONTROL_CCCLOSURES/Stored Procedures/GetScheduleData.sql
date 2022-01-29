CREATE PROCEDURE [dbo].[GetScheduleData]
	
AS
	SELECT 
		SS.StatusScheduleId,
		SS.RegionId,
		R.RegionName,
		SS.StatusCodeId,
		SC.StatusCodeName,
		SS.StartAt,
		SS.EndAt
	FROM
		StatusSchedules SS
		INNER JOIN Regions R
			ON R.RegionId = SS.RegionId
		INNER JOIN StatusCodes SC
			ON SC.StatusCodeId = SS.StatusCodeId
	WHERE
		SS.EndAt > DATEADD(DAY, -30, GETDATE())
		AND SS.DeletedAt IS NULL
RETURN 0