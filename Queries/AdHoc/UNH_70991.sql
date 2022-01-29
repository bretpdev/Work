USE ULS
GO

BEGIN TRANSACTION
       DECLARE @ERROR INT = 0
       DECLARE @ROWCOUNT INT = 0

UPDATE [print].PrintProcessing
SET
	LetterData = '#PUERLEGHLH0326L5#,AMMARON VAHAI,3815 SUSAN DR APT J1,,SAN BRUNO,CA,940666133,,,4373213965,MA4119'
WHERE
	PrintProcessingId = '5988253'

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = 1 AND @ERROR = 0
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