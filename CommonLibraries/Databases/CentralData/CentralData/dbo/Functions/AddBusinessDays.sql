CREATE FUNCTION [dbo].[AddBusinessDays]
(
	@Date DATE,
	@N INT -- number of days to move add/subtract
)

RETURNS DATE AS 

BEGIN

	DECLARE 
		@D INT,
		@CorporateCalendarDays SMALLINT,
		@CalculatedDate DATE

	SET @D = 4-SIGN(@N)*(4-DATEPART(DW, @Date));

	-- add/subtract weekdays
	SET @CalculatedDate = DATEADD(DAY, @N+((ABS(@N)+@D-2)/5)*2*SIGN(@N)-@D/7, @Date)
	
	-- add/subtract Corporate Calendar days
	SET @CalculatedDate = DATEADD(DAY, (SELECT COUNT(*) FROM CentralData.dbo.CorporateCalendar WHERE CalendarDate BETWEEN @Date AND @CalculatedDate OR CalendarDate BETWEEN @CalculatedDate AND @Date) * SIGN(@N), @CalculatedDate)

	-- add/subtract additional non-busness day(s) if initial @CalculatedDate lands on a Corporate Calendar date or a weekend
	WHILE (SELECT COUNT(*) FROM CentralData.dbo.CorporateCalendar WHERE CalendarDate = @CalculatedDate OR DATEPART(DW, @CalculatedDate) IN (1, 7)) != 0
		BEGIN
			SET @CalculatedDate = DATEADD(DAY, 1 * SIGN(@N), @CalculatedDate)
		END

	RETURN @CalculatedDate
END 

GO
GRANT EXECUTE
    ON OBJECT::[dbo].[AddBusinessDays] TO [UHEAA\UHEAAUsers]
    AS [dbo];

