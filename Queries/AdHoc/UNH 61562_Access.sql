USE BSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	INSERT INTO GENR_DAT_WebUserAllowableOptions(UserName, AllowOption)
	VALUES('jbobo', 'AddQueueToQueueStats')
	,('jbobo', 'Email Recipients')
	,('jbobo', 'NCSP Degree Set Up')
	,('jbobo', 'NCSP District')
	,('jbobo', 'NCSP Eligibility Ended Set Up')
	,('jbobo', 'NCSP High School')
	,('jbobo', 'NCSP Institution Set Up')
	,('jbobo', 'NCSP Tuition')
	,('jbobo', 'NCSP User Set Up')
	,('jbobo', 'Reports')
	,('jbobo', 'TILP School')
	,('jbobo', 'TILP School Info Set Up')
	,('jbobo', 'TILP User Set Up')
	,('jbobo', 'UpdateQueuesInQueueStats')
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 14 AND @ERROR = 0
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