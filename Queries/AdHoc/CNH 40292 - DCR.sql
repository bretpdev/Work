USE CDW
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @Expected INT = XXX


UPDATE CDW.FsaInvMet.Monthly_LoanLevel SET LoanStatusPriority = X WHERE BF_SSN = 'XXXXXXXXX' AND LN_SEQ = X
SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
UPDATE CDW.FsaInvMet.Monthly_LoanLevel SET LoanStatusPriority = X WHERE BF_SSN = 'XXXXXXXXX' AND LN_SEQ = X
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
UPDATE CDW.FsaInvMet.Monthly_LoanLevel SET LoanStatusPriority = X WHERE BF_SSN = 'XXXXXXXXX' AND LN_SEQ = X
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
UPDATE CDW.FsaInvMet.Monthly_LoanLevel SET LoanStatusPriority = X WHERE BF_SSN = 'XXXXXXXXX' AND LN_SEQ = X
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
UPDATE CDW.FsaInvMet.Monthly_LoanLevel SET LoanStatusPriority = X WHERE BF_SSN = 'XXXXXXXXX' AND LN_SEQ = X
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
UPDATE CDW.FsaInvMet.Monthly_LoanLevel SET LoanStatusPriority = X WHERE BF_SSN = 'XXXXXXXXX' AND LN_SEQ = X
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
UPDATE CDW.FsaInvMet.Monthly_LoanLevel SET LoanStatusPriority = X WHERE BF_SSN = 'XXXXXXXXX' AND LN_SEQ = X
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


--XXX
UPDATE CDW.FsaInvMet.Monthly_LoanLevel SET LN_DLQ_MAX = LN_DLQ_MAX + X, PerformanceCategory = 'XX' WHERE BF_SSN IN ('XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX') AND LN_DLQ_MAX = XX
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
--XXX
UPDATE CDW.FsaInvMet.Monthly_LoanLevel SET LN_DLQ_MAX = LN_DLQ_MAX + X, PerformanceCategory = 'XX' WHERE BF_SSN IN ('XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX','XXXXXXXXX') AND LN_DLQ_MAX = XXX 
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