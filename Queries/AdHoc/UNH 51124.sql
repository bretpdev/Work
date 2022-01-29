USE NeedHelpUheaa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		[NeedHelpUheaa].[dbo].[DAT_Ticket]
	SET
		[Status] = 'Complete',
		PreviousStatus = 'In Progress',
		History = 'Jeremy Blair - 03/14/2017 01:01 PM - Complete

		IT ticket has been completed. Verified CSYS is correct. This ticket should now be complete

		Jeremy Blair - 02/16/2017 03:04 PM - In Progress

		Submitted IT ticket 47075

		Wendy Hack - 02/01/2017 02:19 PM - In Progress

		E Oscar has been setup. Login info sent to Austin. 

		Wendy Hack - 01/09/2017 09:28 AM - In Progress

		Dorothy Bailey - 01/06/2017 04:27 PM - DS Approval

		Submitted ticket on behalf of Julie Vincent.

		Dorothy Bailey - 01/06/2017 04:27 PM - DS Approval

		Dorothy Bailey - 01/06/2017 04:27 PM - Review

		Issue:
		Austin Turner transferred into UHEAA Loan Servicing on 6/1/16. He is a Rep II.  

		HR was not notified to submit a NH ticket at the time employee transfered into his new role.  

		'
	WHERE
		Ticket = 50273

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		[NeedHelpUheaa].[dbo].[DAT_TicketsAssociatedUserID]
	SET
		SqlUserId = 1519
	WHERE
		[Role] = 'Court'
		AND Ticket = 50273

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	UPDATE
		[NeedHelpUheaa].[dbo].[DAT_TicketsAssociatedUserID]
	SET
		SqlUserId = 1198
	WHERE
		[Role] = 'PreviousCourt'
		AND Ticket = 50273

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
