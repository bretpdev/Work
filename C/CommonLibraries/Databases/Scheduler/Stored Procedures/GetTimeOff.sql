CREATE PROCEDURE GetTimeOff
(
	@StartDate DATETIME,
	@Employee VARCHAR(40)
)
AS

SELECT [Date]
FROM TimeOff
WHERE [Date] >= DATEADD(SECOND, 0, DATEADD(DAY, DATEDIFF(DAY, 0, @StartDate), 0)) AND UserId = @Employee

