CREATE PROCEDURE [hrbridge].[GetMaxParkingUpdated]
AS
	SELECT
		ISNULL(MAX(UpdatedAt), CAST('01-01-1900' AS DATETIME))
	FROM
		hrbridge.Parking_BambooHR
RETURN 0
