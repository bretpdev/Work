USE CDW

GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		LR
	SET
		LR.PrintedAt = NULL
	--SELECT 	*
	FROM
		CDW..LTXX_LetterRequests LR
	WHERE
		CAST(LR.PrintedAt AS DATE)  = CAST(GETDATE() AS DATE)
		AND
		LR.RM_DSC_LTR_PRC IN
		(
			'TSXXBDXXX',
			'TSXXBFXXX',
			'TSXXBFXXXC',
			'TSXXBFXXXJ',
			'TSXXBTSA'
		)

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR


IF @ROWCOUNT = XXX AND @ERROR = X
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
