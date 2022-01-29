USE NeedHelpUheaa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		NeedHelpUheaa..DAT_Ticket
	SET
		[Status] = 'In Progress',
		PreviousStatus = 'Submitting',
		History = 'Karleann Westerman - 08/16/2021 06:44 AM - In Progress

	Moving to Wendy to assign for verification that all files are out in        X:\Archive\SCRA\2021\08-2021

	Karleann Westerman - 08/16/2021 06:43 AM - In Progress

	Need to make sure the following JAMS jobs succeed:
	UTLWD43 - second Tuesday SUCCEEDED
	UTLWS75 - daily at 9 am  SUCCEEDED
	SCRA Defaulted Accounts - 15th  SUCCEEDED
	SCRA Interest Updates - 5 am daily SUCCEEDED

	Karleann Westerman - 08/05/2021 08:45 AM - In Progress

	Uploaded files to SCRA website
	Downloaded result files

	Karleann Westerman - 08/05/2021 08:25 AM - In Progress

	Issue:
	SCRA August 2021
	'
	WHERE
		Ticket = 72221

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		NeedHelpUheaa..DAT_TicketsAssociatedUserID
	SET
		SqlUserId = 1152
	WHERE
		Ticket = 72221
		AND [Role] = 'Court'

	SELECT @ROWCOUNT += @@ROWCOUNT, @ERROR += @@ERROR

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