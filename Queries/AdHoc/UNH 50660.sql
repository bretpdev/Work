USE BSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	--sql code
	INSERT INTO GENR_DAT_WebUserAllowableOptions(UserName, AllowOption)
	VALUES('ccole','TILP School')
	,('ccole','TILP School Info Set Up')
	,('ccole','UpdateQueuesInQueueStats')
	,('ccole','TILP School')
	,('ccole','TILP School Info Set Up')
	,('ccole','UpdateQueuesInQueueStats')

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	
IF @ROWCOUNT = 6 AND @ERROR = 0
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
