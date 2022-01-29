USE CLS

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ExpectedCount INT = X


--Fed loans
UPDATE 
	CLS..PreTransferServicer
SET
	PaymentAddress = 'Department of Education PHEAA P.O. Box XXXXXX ST. Louis, MO XXXXX-XXXX'
WHERE
	pretransferservicerid in (X,XX)

	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
--Cornerstone
UPDATE 
	CLS..PreTransferServicer
SET
	PaymentAddress = 'Department of Education CornerStone UHEAA P.O. Box XXXXXX ST. Louis, MO XXXXX-XXXX'
WHERE
	PreTransferServicerId IN(XX,XX,XX)

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
--MOHELA
UPDATE 
	CLS..PreTransferServicer
SET
	PaymentAddress = 'Department of Education MOHELA P.O. Box XXXXXX ST. Louis, MO XXXXX-XXXX'
WHERE
	PreTransferServicerId IN(XX,XX)

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

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





  
  
 
  
