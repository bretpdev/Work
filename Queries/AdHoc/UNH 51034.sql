BEGIN TRANSACTION
DECLARE @ERROR INT = 0
DECLARE @ROWCOUNT INT = 0
DECLARE @ExpectedRowCount INT = 72

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
			RequestedDate >='2017-03-06 ' 
			AND RequestedDate <= '2017-03-08'
			AND PrintedAt IS NULL 
			AND StateMail2DDocSeqNum IS NULL
			AND DeletedAt IS NULL
	)--72

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

