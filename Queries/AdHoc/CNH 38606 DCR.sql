USE CDW

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedCount INT = X

UPDATE
	CDW..LTXX_LTR_REQ_PRC
SET 
	OnEcorr = X
WHERE 
	OnEcorr = X 
	AND RM_DSC_LTR_PRC = 'TSXXBTSA' 
	AND RT_RUN_SRT_DTS_PRC >= 'XXXX-XX-XX' 
	AND PrintedAt IS NULL
	AND DF_SPE_ACC_ID = 'XXXXXXXXXX'

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedCount AND @ERROR = X
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





  
  
 
  
