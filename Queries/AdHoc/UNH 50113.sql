USE CSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE [dbo].[COST_DAT_MailCodeCostCenters]
	SET [EffectiveEnd] = '2016-10-01'
	WHERE [MailCodeCostCenterId] = 6
		AND MailCode = 'MA4059';
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	INSERT INTO [dbo].[COST_DAT_MailCodeCostCenters] 
		(
		   [MailCode]
		  ,[CostCenterId]
		  ,[Weight]
		  ,[EffectiveBegin]
		  ,[EffectiveEnd]
		)
	VALUES
		('MA4059',26,100,'2016-10-01',NULL)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	UPDATE [dbo].[COST_DAT_CostCenters]
	SET [IsChargedOverHead] = 0
	WHERE [CostCenterId] IN (27,11,30,24,15,1,9,23)
		
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = 10 AND @ERROR = 0
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
