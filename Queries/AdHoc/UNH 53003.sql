USE BSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @Counter INT = 0

	SET @Counter = (SELECT COUNT(*) FROM [BSYS].[dbo].[GENR_REF_AuthAccess] WHERE TypeKey = 'LMCollectorLetters' AND WinUName NOT IN ('amcook','kjorgensen','rcarlow','rwestwater'))

	DELETE
		LM
	FROM
		[BSYS].[dbo].[GENR_REF_AuthAccess] LM
	WHERE
		TypeKey = 'LMCollectorLetters'
		AND
		WinUName NOT IN ('amcook','kjorgensen','rcarlow','rwestwater')

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	INSERT INTO [BSYS].[dbo].[GENR_REF_AuthAccess](TypeKey, WinUName)
	VALUES('LMCollectorLetters', 'acecena')

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

IF @ROWCOUNT = @Counter + 1 AND @ERROR = 0
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
