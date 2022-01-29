GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @SR INT = XXXX
	DECLARE @STATUS VARCHAR(XX) = 'Promotion'
	DECLARE @COURT VARCHAR(XX) = 'Evan Walker'

--UPDATE DAT_ScriptRequests
	UPDATE 
		BSYS..SCKR_DAT_ScriptRequests	
	SET 
		CurrentStatus = @STATUS
		,StatusDate = GETDATE()
		,Court = @COURT
		,CourtDate = GETDATE()
	WHERE 
		Request = @SR

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

--UPDATE REF_Status
	UPDATE 
		BSYS..SCKR_REF_Status	
	SET 
		[End] = GETDATE()
	WHERE 
		Request = @SR
		AND [Class] = 'Scr'
		AND [End] IS NULL

	-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

--INSERT new row into REF_Status
	INSERT INTO BSYS..SCKR_REF_Status ([Request],[Class],[Status],[Begin],[Court])
	VALUES (@SR, 'Scr', @STATUS, GETDATE(),  @COURT)


	-- Update the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = X AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END