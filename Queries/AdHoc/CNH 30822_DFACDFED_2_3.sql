BEGIN TRANSACTION
DECLARE @ERROR INT = X
DECLARE @ROWCOUNT INT = X
DECLARE @ExpectedRowCount INT = XXX

--Query Body
UPDATE
	[CDW].[dbo].[LTXX_LetterRequests]
SET
	PrintedAt = NULL
WHERE 
	RM_DSC_LTR_PRC IN ('TSXXBDXXX', 'TSXXBFXXX', 'TSXXBFXXXC', 'TSXXBFXXXJ')
AND
	PrintedAt BETWEEN 'X/X/XXXX X:XX PM' AND 'X/X/XXXX'
--Query Body

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END


