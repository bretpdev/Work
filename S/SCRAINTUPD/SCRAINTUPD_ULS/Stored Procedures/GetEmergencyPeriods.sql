CREATE PROCEDURE [scra].[GetEmergencyPeriods]
AS
	SELECT
		[Name],
		StartDate,
		EndDate
	FROM
		 [scra].[EmergencyPeriod]
	WHERE
		DeletedAt IS NULL