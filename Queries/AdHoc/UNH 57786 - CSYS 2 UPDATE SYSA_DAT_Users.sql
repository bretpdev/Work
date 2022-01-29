
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	--Set values for row to update
	DECLARE @UNIT VARCHAR(50) = 'Training & Communications'	-- business unit name
	DECLARE @SQUID INT = 1823                           -- SqlUserId to update

	UPDATE
		CSYS..[SYSA_DAT_Users]
	SET BusinessUnit = (SELECT ID FROM [CSYS]..[GENR_LST_BusinessUnits]	WHERE Name = @UNIT)
	WHERE  SqlUserId = @SQUID

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 1 AND @ERROR = 0
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