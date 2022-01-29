USE CDW
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @Expected INT = XXXXXX

UPDATE CDW.FsaInvMet.Monthly_LoanLevel SET LN_DLQ_MAX = LN_DLQ_MAX + X WHERE LN_DLQ_MAX > X --XXXXXX
SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
UPDATE CDW.FsaInvMet.Monthly_LoanLevel SET PerformanceCategory = 'XX' WHERE LN_DLQ_MAX = X --XXX
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
UPDATE CDW.FsaInvMet.Monthly_LoanLevel SET PerformanceCategory = 'XX' WHERE LN_DLQ_MAX = XX --X
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
UPDATE CDW.FsaInvMet.Monthly_LoanLevel SET PerformanceCategory = 'XX' WHERE LN_DLQ_MAX = XX --XXX
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
UPDATE CDW.FsaInvMet.Monthly_LoanLevel SET PerformanceCategory = 'XX' WHERE LN_DLQ_MAX = XXX --XXX
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
UPDATE CDW.FsaInvMet.Monthly_LoanLevel SET PerformanceCategory = 'XX' WHERE LN_DLQ_MAX = XXX --XXX
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = @Expected AND @ERROR = X
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