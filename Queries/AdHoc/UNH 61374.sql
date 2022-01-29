USE ULS
GO

BEGIN TRANSACTION

	ALTER TABLE [ULS].[print].[Letters]
	ADD Instructions VARCHAR(500)
	GO

	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		uls.[print].Letters
	SET
		Instructions = 'Special Handling - Deliver to Account Resolution'
	WHERE
		LETTER IN ('MNGARLT', 'DIROPSLT')

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 2 AND @ERROR = 0
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