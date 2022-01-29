USE NobleCalls

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedCount INT = 1

UPDATE
	DCM
SET
	DCM.ResponseCodeId = 1
FROM
	NobleCalls..DispositionCodeMapping DCM
	INNER JOIN NobleCalls..ResponseCodes RC
		ON RC.ResponseCodeId = DCM.ResponseCodeId
WHERE
	DCM.DispositionCode = 'RP'

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END





  
  
 
  
