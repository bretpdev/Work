USE NeedHelpUheaa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		NeedHelpUheaa..DAT_Ticket
	SET
		PreviousStatus = 'In Progress',
		[Status] = 'Complete',
		History = 'David Halladay - 02/05/2021 08:39 AM - Complete

	Command(s) completed successfully.

	Wendy Hack - 02/05/2021 06:00 AM - In Progress

	Candice Cole - 02/04/2021 04:13 PM - DCR Approval

	Jacob Kramer - 02/04/2021 03:52 PM - Review

	I''ve attached the script to Update the Sproc

	Issue:
	It looks like ULS.[print].[InsertOneLinkPrintProcessingRecord] has an extra optional parameter in test. The common code adds the parameter, and the live code does not have the optional parameter.  Since the parameter is being provided but does not exist in live, We need to update the sproc so that DPALETTERS functions properly.

	'
	WHERE
		Ticket = 70428

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		NeedHelpUheaa..DAT_TicketsAssociatedUserID
	SET
		SqlUserId = 1519
	WHERE
		Ticket = 70428
		AND [Role] = 'Court'

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