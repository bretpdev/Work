USE [EmailTracking]
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE [dbo].[EmailGroups]
	SET 
		Recipient = 'amcook@utahsbr.edu'	
		,Copied1 = 'rgreen@utahsbr.edu'
		,Copied2 = 'ramador@utahsbr.edu'
		,Copied3 = 'ekamibayashi@utahsbr.edu'
		,Copied4 = 'dlino@utahsbr.edu'
		,Copied5 = 'rwestwater@utahsbr.edu'
	WHERE 
		TheGroup = 'uheaahelp'
		AND Number = 44
	
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
	UPDATE [dbo].[EmailGroups]
	SET 
		Recipient = 'amcook@utahsbr.edu'	
		,Copied1 = 'rgreen@utahsbr.edu'
		,Copied2 = 'ramador@utahsbr.edu'
		,Copied3 = 'ekamibayashi@utahsbr.edu'
		,Copied4 = 'dlino@utahsbr.edu'
		,Copied5 = 'rwestwater@utahsbr.edu'
	WHERE 
		TheGroup = 'Collections'
		AND Number = 46
	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

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


