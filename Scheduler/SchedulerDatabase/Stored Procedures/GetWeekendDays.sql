CREATE PROCEDURE GetWeekendDays
(
	@StartDate datetime,
	@Days decimal(6,2)
)
AS

Declare @WeekendDays int
set @WeekendDays = (SELECT
  (DATEDIFF(wk, @StartDate, Dateadd(day, @Days, @StartDate)) * 2)
   +(CASE WHEN DATENAME(dw, @StartDate) = 'Sunday'   THEN 1 ELSE 0 END)
   +(CASE WHEN DATENAME(dw, Dateadd(day, @Days, @StartDate))   = 'Saturday' THEN 1 ELSE 0 END))
Select @WeekendDays

