CREATE FUNCTION [dbo].[BusinessDaysDiff]
(
    @StartDate as DATETIME,
    @EndDate as DATETIME
)
RETURNS INT
AS
BEGIN
    DECLARE @Days int

	SET @Days = 	
		(DATEDIFF(DD, @StartDate, @EndDate) + 1) -- calendar days + 1
		- (CASE WHEN DATEPART(DW, @StartDate) = 7 /*Saturday*/ 
					THEN (DATEDIFF(WK, @StartDate, @EndDate) * 2) - 1 -- weeks * 2
					ELSE DATEDIFF(WK, @StartDate, @EndDate) * 2 
			END)
		- (CASE WHEN DATEPART(DW, @StartDate) = 1 OR DATEPART(DW, @StartDate) = 7 
					THEN 1 
					ELSE 0 
			END) -- starting day is a weekend day
		- (SELECT COUNT(*) FROM CentralData.dbo.CorporateCalendar WHERE CalendarDate BETWEEN @StartDate AND @EndDate) -- corporate calendar days
	
	RETURN @Days
END

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[BusinessDaysDiff] TO [UHEAA\UHEAAUsers]
    AS [dbo];

