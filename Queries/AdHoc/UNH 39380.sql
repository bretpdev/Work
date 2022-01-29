USE CSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE [CSYS].[dbo].[GENR_LST_BusinessUnits]
	SET Name = 'IT'
	WHERE NAME = 'Computer Services'
		AND ID = 9
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE [CSYS].[dbo].[GENR_LST_BusinessUnits]
	SET Name = 'Application Development'
	WHERE ID = 30
		AND Name = 'Process Automation'
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	
	DELETE FROM [CSYS].[dbo].[GENR_LST_BusinessUnits]
	WHERE Name = 'Audit Coordination'
		AND ID = 54
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	
	UPDATE [CSYS].[dbo].[GENR_LST_BusinessUnits]
	SET Name = 'Audit Coordination'
	WHERE ID = 57
		AND Name = 'Customer Request Processing'
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	
	UPDATE [CSYS].[dbo].[COST_DAT_CostCenters]
	SET CostCenter = 'Compliance & QC'
	WHERE CostCenter = 'Compliance'
		AND CostCenterId = 28
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	UPDATE [CSYS].[dbo].[COST_DAT_BusinessUnitCostCenters]
	SET EffectiveEnd = '2016-06-30'
	WHERE BusinessUnitCostCenterId = 168
		AND BusinessUnitId = 12
		AND CostCenterId = 8
		AND [Weight] = 100
		AND EffectiveBegin = '2014-01-01'
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	INSERT INTO [CSYS].[dbo].[GENR_LST_BusinessUnits] (Name)
	VALUES ('Customer Request Processing')
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	INSERT INTO [CSYS].[dbo].[COST_DAT_CostCenters] 
	(
		[CostCenter]
		,[IsBillable]
		,[IsChargedOverHead]
	)
	VALUES ('Customer Request Processing',1,1)
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	INSERT INTO [CSYS].[dbo].[COST_DAT_BusinessUnitCostCenters]
	(
		[BusinessUnitId]
		,[CostCenterId]
		,[Weight]
		,[EffectiveBegin]
		,[EffectiveEnd]
	)
	VALUES (60,29,100,'2016-10-01',NULL)
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	--ALTER TABLE [CSYS].[dbo].[GENR_LST_BusinessUnits]
	--ADD [Status] VARCHAR(10)

	--UPDATE [CSYS].[dbo].[GENR_LST_BusinessUnits]
	--SET [Status] = 'Defunct'
	--WHERE ID IN (4,6,7,8,12,13,20,25,28,44,51)
	----11

	--DELETE FROM [CSYS].[dbo].[GENR_LST_BusinessUnits]
	--WHERE ID IN (4,6,7,8,12,13,20,25,28,44,51)
	--11
	-- Save/Set the row count and error number (if any) from the previously executed statement
	--SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


IF @ROWCOUNT = 9 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END