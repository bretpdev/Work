USE CDW
GO

BEGIN TRANSACTION
       DECLARE @ERROR INT = X
       DECLARE @ROWCOUNT INT = X

UPDATE
	LTXX_LTR_REQ_PRC
SET 
	InactivatedAt = GETDATE()
WHERE
	RM_DSC_LTR_PRC = 'TSDCRTSFED'

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR


IF @ROWCOUNT = X AND @ERROR = X
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
