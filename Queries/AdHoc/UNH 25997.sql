USE BSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	INSERT INTO GENR_DAT_WebUserAllowableOptions(UserName, AllowOption)
	VALUES('spratt', 'AddQueueToQueueStats'),
		('spratt', 'Email Recipients'),
		('spratt', 'NCSP Degree Set Up'),
		('spratt', 'NCSP District'),
		('spratt', 'NCSP Eligibility Ended Set Up'),
		('spratt', 'NCSP High School'),
		('spratt', 'NCSP Institution Set Up'),
		('spratt', 'NCSP Tuition'),
		('spratt', 'NCSP User Set Up'),
		('spratt', 'Reports'),
		('spratt', 'TILP School'),
		('spratt', 'TILP School Info Set Up'),
		('spratt', 'TILP User Set Up'),
		('spratt', 'UpdateQueuesInQueueStats'),
		('dhalladay', 'AddQueueToQueueStats'),
		('dhalladay', 'Email Recipients'),
		('dhalladay', 'NCSP Degree Set Up'),
		('dhalladay', 'NCSP District'),
		('dhalladay', 'NCSP Eligibility Ended Set Up'),
		('dhalladay', 'NCSP High School'),
		('dhalladay', 'NCSP Institution Set Up'),
		('dhalladay', 'NCSP Tuition'),
		('dhalladay', 'NCSP User Set Up'),
		('dhalladay', 'Reports'),
		('dhalladay', 'TILP School'),
		('dhalladay', 'TILP School Info Set Up'),
		('dhalladay', 'TILP User Set Up'),
		('dhalladay', 'UpdateQueuesInQueueStats'),
		('hgibson', 'AddQueueToQueueStats'),
		('hgibson', 'Email Recipients'),
		('hgibson', 'NCSP Degree Set Up'),
		('hgibson', 'NCSP District'),
		('hgibson', 'NCSP Eligibility Ended Set Up'),
		('hgibson', 'NCSP High School'),
		('hgibson', 'NCSP Institution Set Up'),
		('hgibson', 'NCSP Tuition'),
		('hgibson', 'NCSP User Set Up'),
		('hgibson', 'Reports'),
		('hgibson', 'TILP School'),
		('hgibson', 'TILP School Info Set Up'),
		('hgibson', 'TILP User Set Up'),
		('hgibson', 'UpdateQueuesInQueueStats')

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 42 AND @ERROR = 0
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
