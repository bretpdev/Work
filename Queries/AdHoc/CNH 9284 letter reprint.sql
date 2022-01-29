USE CDW

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

update cdw..LTXX_LetterRequests set PrintedAt = null, InactivatedAt = null, SystemLetterExclusionReasonId = null where RM_DSC_LTR_PRC = 'TSXXBAPIDR' and printedat >='XXXX-XX-XX'


-- Save/Set the row count and error number (if any) from the previously executed statement
SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = XX AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END