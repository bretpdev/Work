USE BSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	INSERT INTO GENR_DAT_WebUserAllowableOptions(UserName, AllowOption)
	VALUES('nnguyen', 'AddQueueToQueueStats')
	,('nnguyen', 'Email Recipients')
	,('nnguyen', 'NCSP Degree Set Up')
	,('nnguyen', 'NCSP District')
	,('nnguyen', 'NCSP Eligibility Ended Set Up')
	,('nnguyen', 'NCSP High School')
	,('nnguyen', 'NCSP Institution Set Up')
	,('nnguyen', 'NCSP Tuition')
	,('nnguyen', 'NCSP User Set Up')
	,('nnguyen', 'Reports')
	,('nnguyen', 'TILP School')
	,('nnguyen', 'TILP School Info Set Up')
	,('nnguyen', 'TILP User Set Up')
	,('nnguyen', 'UpdateQueuesInQueueStats')
	
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