-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION GetBusinessDays
(
	-- Add the parameters for the function here
	@StartDate DATETIME,
	@EndDate DATETIME
)
RETURNS INT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @ResultVar INT

	SELECT @ResultVar =
   (DATEDIFF(dd, @StartDate, @EndDate) + 1)
  -(DATEDIFF(wk, @StartDate, @EndDate) * 2)
  -(CASE WHEN DATENAME(dw, @StartDate) = 'Sunday' THEN 1 ELSE 0 END)
  -(CASE WHEN DATENAME(dw, @EndDate) = 'Saturday' THEN 1 ELSE 0 END)

	-- Return the result of the function
	RETURN @ResultVar

END