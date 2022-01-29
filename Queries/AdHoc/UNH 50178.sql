BEGIN TRANSACTION
DECLARE @ERROR INT = 0
DECLARE @ROWCOUNT INT = 0
DECLARE @ExpectedRowCount INT = 2

--Query Body

UPDATE
	[NeedHelpUheaa].[dbo].[DAT_Ticket]
SET
	[Status] = 'Complete', PreviousStatus = 'In Progress'
WHERE 
	[Ticket] = 49865

SELECT @ROWCOUNT = @@ROWCOUNT

DELETE FROM
	[NeedHelpUheaa].[dbo].[REF_Status]
WHERE
	[Sequence] = 216123

--Query Body

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END

