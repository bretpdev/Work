USE NeedHelpUheaa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		NeedHelpUheaa..DAT_Ticket
	SET
		History = 'Matthew Jones - 08/26/2019 09:20 AM - In Progress

	Issue:
	Process Log 8-26-19 to 8-30-19',
		[Status] = 'In Progress',
		PreviousStatus = 'Submitting'
	WHERE
		Ticket = 63012

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR
		
	UPDATE
		NeedHelpUheaa..DAT_TicketsAssociatedUserID
	SET
		SqlUserId = 4039
	WHERE
		Ticket = 63012
		AND [Role] = 'Court'

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	UPDATE
		NeedHelpUheaa..DAT_TicketsAssociatedUserID
	SET
		SqlUserId = NULL
	WHERE
		Ticket = 63012
		AND [Role] = 'PreviousCourt'

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