USE NeedHelpUheaa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		NeedHelpUheaa..DAT_Ticket
	SET
		[Status] = 'In Progress',
		PreviousStatus = 'Approval',
		History = 'David Halladay - 04/28/2021 11:09 AM - In Progress

	I apologize for the misunderstanding. I have updated the assigned froup fro UCSK ACD routing to be group 1 and removed the hold time and next DNIS. Moving to Kennedy.

	Nathaniel Kennedy - 04/16/2021 10:24 AM - In Progress

	So, we want this option for DIAL NOW to be available to agents in Group 1 instead of Group 19. 

	The updates made looks like it''s sending the call to Group 1 if it''s been on hold for 120 seconds. That''s not what we need.

	We''re working on consolidating everything to just 2-3 groups in Noble for us to use, and we want the DIAL NOW DNIS set up currently for UCSK in Group 19 to be changed to be in Group 1. 

	Basically, we don''t want anyone to be able to select UCSK in Group 19, only in Group 1. 

	David Halladay - 04/16/2021 08:35 AM - In Progress

	Kennedy, we can only update the routing to go to one other group. I have updated the UCSK DIAL NOW group to route to the DIAL NOW DNIS assigned to group 1. I set the hold time for 120 seconds. 

	Attached are screen shots. Please review and let me know if there are any questions/concerns. 

	Nathaniel Kennedy - 04/05/2021 02:55 PM - In Progress

	Is there an update on this? 

	Wendy Hack - 03/03/2021 10:53 AM - In Progress

	Teri Vig - 03/03/2021 10:25 AM - Approval

	David Halladay - 03/03/2021 10:18 AM - QC Approval

	Nathaniel Kennedy - 03/03/2021 09:08 AM - Review

	Issue:
	Please update the ACD Routing for UCSK to move from Group 19 to Group 1 and Group 15. This is a manual outbound campaign for manual SKIP calls. 

	If we can''t do both Groups, Group 1 will suffice.'
	WHERE
		Ticket = 70728

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		DAT_TicketsAssociatedUserID
	SET
		SqlUserId = 1152
	WHERE
		Ticket = 70728
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