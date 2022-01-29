GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @SASR INT = 2897
	DECLARE @STATUS VARCHAR(50) = 'Scheduling'
	DECLARE @COURT VARCHAR(50) = 'Savanna Gregory'

--UPDATE DAT_SASRequests
	UPDATE 
		BSYS..SCKR_DAT_SASRequests	
	SET 
		CurrentStatus = @STATUS
		,StatusDate = GETDATE()
		,Court = @COURT
		,CourtDate = GETDATE()
	WHERE 
		Request = @SASR

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

--UPDATE REF_Status
	UPDATE 
		BSYS..SCKR_REF_Status	
	SET 
		[End] = GETDATE()
	WHERE 
		Request = @SASR
		AND [Class] = 'SAS'
		AND [End] IS NULL

	-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

--INSERT new row into REF_Status
	INSERT INTO BSYS..SCKR_REF_Status ([Request],[Class],[Status],[Begin],[Court])
	VALUES (@SASR, 'SAS', @STATUS, GETDATE(),  @COURT)


	-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

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
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END