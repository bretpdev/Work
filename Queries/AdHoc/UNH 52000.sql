USE UDW
GO

BEGIN TRANSACTION
DECLARE @ERROR INT = 0
DECLARE @ROWCOUNT INT = 0
DECLARE @ExpectedRowCount INT = 209

--Query Body
UPDATE
	UDW..LT20_LTR_REQ_PRC
SET
	PrintedAt = NULL 
WHERE 
	RM_DSC_LTR_PRC in ('US06BCAP','US06BD101','US06BDMP','US06BF101','US06BF601','US06BIBRXP','US06BTCRD1','US06BTPDSS','US06BTRT4','US08B48TPD','US09B10P','US09B160','US09BFDLP','US11BISF') 
	AND DATEDIFF(DAY,PrintedAt,'2017-06-07') = 0 
	AND OnEcorr = 0
--Query Body

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

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



