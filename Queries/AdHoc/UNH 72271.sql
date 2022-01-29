USE BSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	DELETE FROM BSYS..QKIL_LST_QueueKiller

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	INSERT INTO BSYS..QKIL_LST_QueueKiller([Queue], Department)
	VALUES('KCREFCLL', 'SKP')
	,('KAREFCLL', 'SKP')
	,('KDREFCLL', 'SKP')
	,('PREREHAB', 'PRE')

	SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

IF @ROWCOUNT = 11 AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END