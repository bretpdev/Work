USE MauiDUDE
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X
	DECLARE @ScriptId INT = (SELECT ScriptId FROM MauiDUDE..MenuOptionsScriptsAndServices WHERE DisplayName = 'Parent Plus Deferment Letter')

	UPDATE
		MauiDUDE..MenuOptionsScriptsAndServices
	SET
		ToBeCalledImm = 'FALSE',
		ToBeCalledAtEnd = 'TRUE',
		CallForNoteDUDECleanUp = 'FALSE'
	WHERE
		ScriptID = @ScriptId
	
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
		PRINT 'Transaction NOT committed'
		ROLLBACK TRANSACTION
	END