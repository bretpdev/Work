USE MauiDUDE
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	INSERT INTO [MauiDUDE].[dbo].[MenuOptionsScriptsAndServices](InternalOrExternal, ParentMenu, DisplayName, SubToBeCalled, ToBeCalledImm, ToBeCalledAtEnd, HomePage, DataForFunctionCall, CallForNoteDUDECleanUp, CompletionFile, DisableKey, DLLToLoad, DLLsToCopy, ObjectToCreate)
	VALUES('Internal', 'Letters', 'Parent Plus Deferment Letter', 'AddLetterArc', 'False', 'True', 'Borrower Services', 'HXXXA', 'False', 'Nothing', 'Nothing', NULL, NULL, NULL)
	
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