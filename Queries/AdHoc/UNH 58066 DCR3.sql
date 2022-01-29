USE BSYS

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

--3 Update SCKR_DAT_PIR to close PIR records
	UPDATE
		BSYS..SCKR_DAT_PIR
	SET 
		Comments = 'PIR closed by DCR UNH_58066', [Status] = 'Approved', [StatusDate] = GETDATE(), [CurrentStatus] = 'Complete', [CurrentStatusDate] = GETDATE(), [Court] = '', CourtDate = GETDATE()
	WHERE
		CurrentStatus NOT IN ('Complete','Returned','Closed')
		AND DATEDIFF(MONTH,CurrentStatusDate,GETDATE()) > 6

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
	


IF @ROWCOUNT = 2462 AND @ERROR = 0
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


