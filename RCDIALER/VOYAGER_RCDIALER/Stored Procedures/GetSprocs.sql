CREATE PROCEDURE [rcdialer].[GetSprocs]
	@DAY VARCHAR(9)
AS
	SELECT
		SP.SprocName,
		SP.IsAlternateWeek
	FROM
		rcdialer.SprocMapping SM
		INNER JOIN rcdialer.[DaysOfWeek] DW
			ON DW.DaysOfWeekId = SM.DaysOfWeekId
		INNER JOIN rcdialer.Sprocs SP
			ON SP.SprocsId = SM.SprocsId
	WHERE
		DW.[DayOfWeek] = @DAY
		AND DATEDIFF(WK, 0, GETDATE()) % 2 = SP.IsAlternateWeek
		AND SP.DeletedAt IS NULL