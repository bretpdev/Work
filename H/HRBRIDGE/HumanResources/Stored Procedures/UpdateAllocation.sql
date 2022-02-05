CREATE PROCEDURE [hrbridge].[UpdateAllocation]
	@Allocation AllocationRecord_BambooHR READONLY
AS
BEGIN TRANSACTION
BEGIN TRY

--Everytime an employees records are updated, the API sends all records for that employee so we can remove existing records
DELETE ALLOC 
FROM 
	hrbridge.Allocations_BambooHR ALLOC
	INNER JOIN 
	(
		SELECT DISTINCT
			UPDATED_ALLOC.EmployeeId
		FROM
			@Allocation UPDATED_ALLOC
	) UPDATED_ALLOC
		ON UPDATED_ALLOC.EmployeeId = ALLOC.EmployeeId
	
INSERT INTO hrbridge.Allocations_BambooHR(EmployeeId,UpdatedAt,BusinessUnit,CostCenter,Account,FTE,AllocationEffectiveDate,SquareFootage,FTERaw)
SELECT
	ALLOC.EmployeeId,
	ALLOC.UpdatedAt,
	ALLOC.BusinessUnit,
	ALLOC.CostCenter,
	ALLOC.Account,
	TRY_CONVERT(DECIMAL(18,10),ALLOC.FTE),
	ALLOC.AllocationEffectiveDate,
	ALLOC.SquareFootage,
	ALLOC.FTE AS FTERaw
FROM
	@Allocation ALLOC

COMMIT TRANSACTION
PRINT 'Transaction committed'
END TRY
BEGIN CATCH
	PRINT 'Transaction NOT committed'
	ROLLBACK TRANSACTION;
	THROW;
END CATCH;

RETURN 0
