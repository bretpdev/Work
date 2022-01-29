USE BSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 32

	INSERT INTO GENR_DAT_WebUserAllowableOptions(UserName, AllowOption)
	VALUES --new agents
		 ('aisom','AddQueueToQueueStats')
		,('aisom','Email Recipients')
		,('aisom','NCSP Degree Set Up')
		,('aisom','NCSP District')
		,('aisom','NCSP Eligibility Ended Set Up')
		,('aisom','NCSP High School')
		,('aisom','NCSP Institution Set Up')
		,('aisom','NCSP Tuition')
		,('aisom','NCSP User Set Up')
		,('aisom','Reports')
		,('aisom','TILP School')
		,('aisom','TILP School Info Set Up')
		,('aisom','TILP User Set Up')
		,('aisom','UpdateQueuesInQueueStats')
		,('adespain','AddQueueToQueueStats')
		,('adespain','Email Recipients')
		,('adespain','NCSP Degree Set Up')
		,('adespain','NCSP District')
		,('adespain','NCSP Eligibility Ended Set Up')
		,('adespain','NCSP High School')
		,('adespain','NCSP Institution Set Up')
		,('adespain','NCSP Tuition')
		,('adespain','NCSP User Set Up')
		,('adespain','Reports')
		,('adespain','TILP School')
		,('adespain','TILP School Info Set Up')
		,('adespain','TILP User Set Up')
		,('adespain','UpdateQueuesInQueueStats')
		,('ccole','NCSP Degree Set Up') --was missing this access
		,('jblair','NCSP Degree Set Up') --was missing this access

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
	

	INSERT INTO GENR_DAT_WebUserLoginInfo(UserName, [Password])
	VALUES --new agents
		('aisom', 'Welcome1'),
		('adespain', 'Welcome1')

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
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
