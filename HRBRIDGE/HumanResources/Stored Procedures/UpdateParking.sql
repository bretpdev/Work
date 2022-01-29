CREATE PROCEDURE [hrbridge].[UpdateParking]
	@Parking ParkingRecord_BambooHR READONLY
AS
BEGIN TRANSACTION
BEGIN TRY

--Everytime an employees records are updated, the API sends all records for that employee so we can remove existing records
DELETE PARK
FROM 
	hrbridge.Parking_BambooHR PARK
	INNER JOIN 
	(
		SELECT DISTINCT
			UPDATED_PARK.EmployeeId
		FROM
			@Parking UPDATED_PARK
	) UPDATED_PARK
		ON UPDATED_PARK.EmployeeId = PARK.EmployeeId
	
INSERT INTO hrbridge.Parking_BambooHR(EmployeeId,UpdatedAt,Garage,[Type],FobId)
SELECT
	PARK.EmployeeId,
	PARK.UpdatedAt,
	PARK.Garage,
	PARK.[Type],
	PARK.FobId
FROM
	@Parking PARK

COMMIT TRANSACTION
PRINT 'Transaction committed'
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed'
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;
RETURN 0
