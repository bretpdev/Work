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
		History = 'Jeremy Blair - 08/28/2018 09:55 AM - Complete

		Moving to Makeli to verify

		Matthew Jones - 08/28/2018 09:24 AM - Complete

		SADB
		Marking Complete

		Matthew Jones - 08/27/2018 08:25 AM - In Progress

		Maestro
		CNOC 58972 Completed
		CSYS

		Matthew Jones - 08/24/2018 12:58 PM - In Progress

		Onelink Tiers 
		Onelink groups SEQUENCE 02719 & 2720
		User ID by role 
		Roles & mapping 
		OARS completed


		Nghia Nguyen - 08/23/2018 09:45 AM - In Progress

		Created Arcs and Ques for both regions

		John Hyde - 08/23/2018 07:22 AM - In Progress

		TX4F

		Dorothy Bailey - 08/21/2018 02:23 PM - In Progress

		UDPATE: Tentative 5C clearance date is now 8/21/18.

		John Hyde - 08/21/2018 02:07 PM - In Progress

		CS NSLDS confir # 487025

		Moving back to Matthew

		Matthew Jones - 08/21/2018 09:16 AM - In Progress

		Moving to John for CornerStone NSLDS access

		Matthew Jones - 08/21/2018 09:11 AM - In Progress

		OARS 243436 submitted 

		Matthew Jones - 08/17/2018 09:30 AM - In Progress

		CNOC 58972

		Debbie Phillips - 08/07/2018 11:57 AM - In Progress

		Court changed from Candice Cole to Matthew Jones

		Debbie Phillips - 08/07/2018 11:56 AM - In Progress

		Julie Vincent - 08/07/2018 09:33 AM - DS Approval

		Approved.

		Sounthala Tran - 08/06/2018 04:15 PM - Review

		Issue:
		Wesley Egbert starts training in UHEAA Customer Solutions as a Rep T-3 (federal) on 8/27/18. eQip will be completed on 8/13/18.

		Tentative 5C clearance date is 8/24/18.

		'
	WHERE
		Ticket = 57824

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		[NeedHelpUheaa].[dbo].[DAT_TicketsAssociatedUserID]
	SET
		SqlUserId = 1779
	WHERE
		Ticket = 57824
		AND [Role] = 'Court'

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	UPDATE
		[NeedHelpUheaa].[dbo].[DAT_TicketsAssociatedUserID]
	SET
		SqlUserId = 1198
	WHERE
		Ticket = 57824
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