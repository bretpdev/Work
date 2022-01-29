BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE [NeedHelpUheaa].[dbo].[DAT_Ticket]
	SET UNIT = 62
	WHERE TicketCode = 'SASR'
		AND Ticket IN (
		 28704
		,28706
		,28756
		,28906
		,29024
		,29081
		,28526
		,28761
		,28346
		,28571
		,28705
		,28757
		,29073
		,28534
		,28884
		,28902
		,29052
		,28920
	)--18
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 18 AND @ERROR = 0
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
