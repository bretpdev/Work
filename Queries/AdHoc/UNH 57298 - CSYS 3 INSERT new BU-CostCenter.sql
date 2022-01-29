
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	--Set values for row to insert
	DECLARE @UNIT VARCHAR(50) = 'Repayment Services'	-- business unit name
	DECLARE @COSTCNTRID INT = 8							-- cost center ID
	DECLARE @WEIGHT INT	= 100							--percent of unit allocated to cost center
	DECLARE @BEGIN VARCHAR(10) = '2018-07-01'			--begin date of quarter being reported

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

IF @ROWCOUNT = 1 AND @ERROR = 0
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