CREATE PROCEDURE [dbo].[GetPovertyGuideline]
	@CurrentDate DATETIME = NULL 
AS
	DECLARE @Year INT = DATEPART(YEAR, ISNULL(@CurrentDate, GETDATE()))
	DECLARE @MaxYear INT = (SELECT MAX([year]) FROM dbo.Poverty_Guidelines)
	IF (@Year > @MaxYear)
		SET @Year = @MaxYear

	SELECT
		[year], 
		continental_income, 
		alaska_income, 
		hawaii_income, 
		continental_increment, 
		alaska_increment, 
		hawaii_increment
	FROM
		dbo.Poverty_Guidelines
	WHERE
		[year] = @Year

RETURN 0
