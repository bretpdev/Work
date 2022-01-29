USE ULS

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedCount INT = 687359


--Set all data as not deleted unless it should have been
UPDATE
	[Live]
SET
	[Live].EmailSentAt = [Backup].EmailSentAt,
	[Live].ProcessingAttempts =	[Backup].ProcessingAttempts,
	[Live].DeletedAt = [Backup].DeletedAt,
	[Live].DeletedBy = [Backup].DeletedBy
FROM 
	[ULS2AM].[emailbatch].[EmailProcessing] [Backup]
	INNER JOIN ULS.emailbatch.EmailProcessing [Live]
		ON [Live].EmailProcessingId = [Backup].EmailProcessingId
		AND [Live].DeletedBy = 'alarson'
--654860
SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR


--Fix 32k emails that were resent back to correct date
UPDATE
	[Live]
SET 
	[Live].EmailSentAt = [Backup].EmailSentAt,
	[Live].ProcessingAttempts = [Backup].ProcessingAttempts,
	[Live].DeletedAt = [Backup].DeletedAt,
	[Live].DeletedBy = [Backup].DeletedBy
FROM 
	[ULS2AM].[emailbatch].[EmailProcessing] [Backup]
	INNER JOIN ULS.emailbatch.EmailProcessing [Live]
		ON [Live].EmailProcessingId = [Backup].EmailProcessingId
		AND [Live].DeletedBy = 'UNH 61881'
--32499
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

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





  
  
 
  
