CREATE PROCEDURE [covidforb].[GetEndDate]
AS
	SELECT
		E.EndDate,
		E.CARESStartDate
	FROM
		covidforb.EndDate E
RETURN 0
