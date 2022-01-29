USE NeedHelpUheaa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	--sql code
	UPDATE
		[NeedHelpUheaa].[dbo].[DAT_Ticket]
	SET
		[Status] = 'Complete',
		PreviousStatus = 'In Progress'
	WHERE
		Ticket = 50636

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR
	
	UPDATE
		[NeedHelpUheaa].[dbo].[DAT_TicketsAssociatedUserID]
	SET
		SqlUserId = 1519
	WHERE
		Ticket = 50636
		AND [Role] = 'Court'

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR	
	
	UPDATE
		[NeedHelpUheaa].[dbo].[DAT_TicketsAssociatedUserID]
	SET
		SqlUserId = 1161
	WHERE
		Ticket = 50636
		AND [Role] = 'PreviousCourt'

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR	
	
IF @ROWCOUNT = 3 AND @ERROR = 0
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
