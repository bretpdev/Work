USE  NeedHelpCornerStone
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = X
	DECLARE @ROWCOUNT INT = X

	UPDATE
		DAT_Ticket
	SET
		[Status] = 'In Progress',
		PreviousStatus = 'Review',
		History = 'Eliezer Cadena - XX/XX/XXXX XX:XX AM - In Progress

		No questions about the "AS" record update.  

		Jeremy Blair - XX/XX/XXXX XX:XX AM - Review

		I believe there are several different types of NSLDS error reports.

		This CR is in regards to an update to the "AS" records that are sent to NSLDS.

		Do you have specific questions you would like to ask AES about this "AS" record update?

		Eliezer Cadena - XX/XX/XXXX XX:XX AM - Review

		What stands out are the possible errors that could occur. Isn''t there already NSLDS error report that''s generated? 

		Jeremy Blair - XX/XX/XXXX XX:XX AM - Review

		Please review and let me know if you have any questions.

		Thank you!

		Jeremy Blair - XX/XX/XXXX XX:XX AM - Review

		Issue:
		Received the following from AES:
		CR XXXXXX was submitted to correct deferment/forbearance reporting to NSLDS.

		Change Request Description:
		When different types of deferments or forbearance exist consecutively (back-to-back dates), the defer/forbs are reported to NSLDS (on the AS
		record) grouped together at a defer/forb level but with just one defer/forb type. Each type of defer/forb needs to be reported separately with consecutive periods of the same type merged together as one record. If two or more deferments of the same type exist and are consecutive, populate with the date stops of the last deferment. If two or more forbearances of the same type exist and are consecutive, populate with the date stops of the last forbearance.

		Change Request Background:
		Issue with defr/forb reporting to NSLDS. When we have X or more consecutive deferments/forbearance of different types, they are being merged together under the same type. So a MN forb from X/X/XX to XX/XX/XX and then a DC forb from X/X/XX to XX/XX/XX will be reported as a X/X/XX to XX/XX/XX DC.
		When different types of deferments or forbearance exist consecutively (back-to-back dates), the defer/forbs are reported to NSLDS (on the AS
		record) grouped together at a defer/forb level but with just one defer/forb type. Each type of defer/forb needs to be reported separately with consecutive periods of the same type merged together as one record

		Change Request Requirements:
		(See attached file: CR XXXXXX AS Record Requirements.docx)

		Change Request Production Target Date:
		This CR is tentatively scheduled for a X/XX/XX production date.'
	WHERE
		Ticket = XXXXX

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		DAT_TicketsAssociatedUserID
	SET
		SqlUserId = XXXX
	WHERE
		Ticket = XXXXX
		AND
		[Role] = 'Court'

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

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