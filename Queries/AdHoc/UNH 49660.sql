USE CSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	INSERT INTO [CSYS].[dbo].[COST_DAT_BusinessUnitCostCenters]
	(
		   [BusinessUnitId]
		  ,[CostCenterId]
		  ,[Weight]
		  ,[EffectiveBegin]
		  ,[EffectiveEnd]
	)
	VALUES (55,8,100,'2016-07-01',NULL) --Loan Servicing to LPP Portfolio Servicing
		  ,(25,8,100,'2016-07-01',NULL) --Loan Servicing to LPP Portfolio Servicing
		  ,(56,9,100,'2016-07-01',NULL) --Private Loans to LPP Supplemental Loan Program Servicing
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
	
IF @ROWCOUNT = 3 AND @ERROR = 0
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
