USE BSYS

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

INSERT INTO [BSYS].[dbo].[GENR_DAT_WebUserAllowableOptions] ([UserName],[AllowOption])
VALUES 
	('mjones','AddQueueToQueueStats'),
	('mjones','Email Recipients'),
	('mjones','NCSP Degree Set Up'),
	('mjones','NCSP District'),
	('mjones','NCSP Eligibility Ended Set Up'),
	('mjones','NCSP High School'),
	('mjones','NCSP Institution Set Up'),
	('mjones','NCSP Tuition'),
	('mjones','NCSP User Set Up'),
	('mjones','Reports'),
	('mjones','TILP School'),
	('mjones','TILP School Info Set Up'),
	('mjones','TILP User Set Up'),
	('mjones','UpdateQueuesInQueueStats')

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
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END