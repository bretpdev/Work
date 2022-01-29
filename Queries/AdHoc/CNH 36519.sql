USE CLS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		CLS.emailbtcf.EmailCampaigns
	SET
		SubjectLine = 'Your CornerStone Account Is Delinquent - You May Qualify For Income-Driven Repayment'
	WHERE
		EmailCampaignId = XX

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		CLS.emailbtcf.EmailCampaigns
	SET
		SubjectLine = 'This Is Your Last Chance to Avoid Default'
	WHERE
		EmailCampaignId = XX

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	UPDATE
		CLS.emailbtcf.EmailCampaigns
	SET
		SubjectLine = 'Your Student Loan Account Is Severely Delinquent and in Danger of Default'
	WHERE
		EmailCampaignId = XX

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = X AND @ERROR = X
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(XX))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(XX))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END