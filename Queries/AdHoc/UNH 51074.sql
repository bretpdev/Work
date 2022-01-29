BEGIN TRANSACTION
DECLARE @ERROR INT = 0
DECLARE @ROWCOUNT INT = 0
DECLARE @ExpectedRowCount INT = 2

UPDATE
	[BSYS].[dbo].[PRNT_DAT_Print]
SET
	DeletedAt = GETDATE()
WHERE
	SeqNum IN 
	(
		SELECT
			SeqNum
		FROM 
			Bsys..PRNT_DAT_Print
		WHERE 
			RequestedDate IN ('2017-03-09 17:22:41.907','2017-03-08 09:58:57.330')
	)--2

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

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

