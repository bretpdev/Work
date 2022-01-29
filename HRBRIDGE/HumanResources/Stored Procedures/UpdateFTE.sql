CREATE PROCEDURE [hrbridge].[UpdateFTE]
	@FTE FTERecord_BambooHR READONLY
AS
BEGIN TRANSACTION
BEGIN TRY

--Everytime an employees records are updated, the API sends all records for that employee so we can remove existing records
DELETE FTE
FROM 
	hrbridge.FTE_BambooHR FTE
	INNER JOIN 
	(
		SELECT DISTINCT
			UPDATED_FTE.EmployeeId
		FROM
			@FTE UPDATED_FTE
	) UPDATED_FTE
		ON UPDATED_FTE.EmployeeId = FTE.EmployeeId
	
INSERT INTO hrbridge.FTE_BambooHR(EmployeeId,UpdatedAt,FTEEffectiveDate,FTE,Notes,FTERaw)
SELECT
	FTE.EmployeeId,
	FTE.UpdatedAt,
	FTE.FTEEffectiveDate,
	TRY_CONVERT(DECIMAL(18,10), FTE.FTE),
	FTE.Notes,
	FTE.FTE AS FTERaw
FROM
	@FTE FTE

COMMIT TRANSACTION
PRINT 'Transaction committed'
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed'
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;
RETURN 0
