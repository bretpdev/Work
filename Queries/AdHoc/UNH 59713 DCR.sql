USE ULS
GO
BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @ExpectedRowCount INT = 3

--3
UPDATE
	EP
SET
	EP.DeletedAt = GETDATE(),
	EP.DeletedBy = 'UNH 59713'
FROM
	ULS.emailbatch.HTMLFiles H 
	INNER JOIN ULS.emailbatch.EmailCampaigns EC
		ON EC.HTMLFileId = H.HTMLFileId
	INNER JOIN ULS.emailbatch.EmailProcessing EP
		ON EP.EmailCampaignId = EC.EmailCampaignId
WHERE 
	H.HTMLFile = 'CUR5EMLUH.html'
	AND EmailSentAt IS NULL

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT AS VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR AS VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END