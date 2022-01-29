USE CDW

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		CDW.dbo.LTXX_LETTERREQUESTS
	SET
		InactivatedAt = GETDATE()
	WHERE
		PrintedAt is NULL
		AND
		RM_DSC_LTR_PRC = 'TSXXBFNBIL'
		AND
		DF_SPE_ACC_ID IN
		(
			'XXXXXXXXXX',
			'XXXXXXXXXX',
			'XXXXXXXXXX'
		)
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR


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
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END
