
BEGIN TRANSACTION
	DECLARE 
		@ERROR INT = 0,
		@ROWCOUNT INT = 0

	UPDATE
		[NeedHelpUheaa].dbo.[DAT_Ticket]
	SET
		History = REPLACE(CAST(History as VARCHAR(MAX)), CHAR(13) + CHAR(10) + '.' + CHAR(13) + CHAR(10), CHAR(13) + CHAR(10))
	WHERE
		PATINDEX('%' + CHAR(13) + CHAR(10) + '.' + CHAR(13) + CHAR(10)+ '%', CAST(History as VARCHAR(MAX))) > 0

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ERROR = @@ERROR, @ROWCOUNT = @@ROWCOUNT


IF @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		PRINT CAST(@ROWCOUNT AS VARCHAR(40)) + ' histories fixed.'
		PRINT 'Run again until 0 histories are fixed.'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!! Transaction NOT committed; Contact a member of Application Developmnet to have this error corrected.'
		ROLLBACK TRANSACTION
	END