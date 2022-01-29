-- Business Unit Add New - CSYS 3 INSERT new BU-CostCenter.sql

-- add a new record to the [CSYS].[dbo].[COST_DAT_BusinessUnitCostCenters] table to allocate work done for the new business unit to the appropriate cost center
-- a copy of this script is needed for each cost center to which the new business unit is allocated
-- see notes below to update values


GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	--Set values for row to insert
	DECLARE @UNIT VARCHAR(50) = 'Training & Communications'    -- update this with the name of the new business unit
	DECLARE @COSTCNTRID INT = 8							       -- update this with the cost center ID from CSYS.[dbo].[COST_DAT_CostCenters]
	DECLARE @WEIGHT INT	= 100							       -- update this with the percent of the unit allocated to the cost center
	DECLARE @BEGIN VARCHAR(10) = '2018-07-01'			       -- update this with the begin date of the current quarter

	INSERT INTO 
		[CSYS].[dbo].[COST_DAT_BusinessUnitCostCenters] ([BusinessUnitId],[CostCenterId],[Weight],[EffectiveBegin])
	SELECT 
		ID
		,@COSTCNTRID
		,@WEIGHT
		,@BEGIN
	FROM
		[CSYS]..[GENR_LST_BusinessUnits]
	WHERE
		Name = @UNIT

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 1 AND @ERROR = 0 -- only one row should be affected
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END