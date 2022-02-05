CREATE FUNCTION dbo.PaymentsDue
(
	@StartDate DATE,
	@EndDate DATE,
	@DayOfMonth TINYINT
)

RETURNS INT
AS
BEGIN
	RETURN DATEDIFF(MONTH, @StartDate, @EndDate) + CASE WHEN DAY(@StartDate) <= @DayOfMonth THEN 1 ELSE 0 END + CASE WHEN DAY(@EndDate) >= @DayOfMonth THEN 1 ELSE 0 END -1
END;
GO
GRANT EXECUTE
    ON OBJECT::[dbo].[PaymentsDue] TO [UHEAA\UHEAAUsers]
    AS [dbo];

