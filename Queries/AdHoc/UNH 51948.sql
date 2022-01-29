BEGIN TRANSACTION
DECLARE @ERROR INT = 0
DECLARE @ROWCOUNT INT = 0
DECLARE @ExpectedRowCount INT = 77

--Query Body
UPDATE
	[UDW].[dbo].[LT20_LTR_REQ_PRC]
SET
	PrintedAt = NULL
WHERE
	RM_DSC_LTR_PRC IN ('US06BF101', 'US06BIBRXP', 'US09B10P', 'US09B160', 'US09BFDLP')
	AND 
	PrintedAt BETWEEN '6/5/17' AND '6/6/17'
--Query Body

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


