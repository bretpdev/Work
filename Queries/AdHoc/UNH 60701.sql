USE NeedHelpUheaa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

UPDATE
	NeedHelpUheaa..DAT_Ticket
SET
	[Status] = 'In Progress',
	PreviousStatus = 'Review',
	History = 'Nghia Nguyen - 03/14/2019 12:16 PM - In Progress

atx4f
added arcs and ques
added onelink tiers and group 3153, 3154

Nghia Nguyen - 03/14/2019 09:58 AM - In Progress

oars completed, UT02524 assigned.

Nghia Nguyen - 03/12/2019 11:30 AM - In Progress

CNOC 63382 submitted
Oars 245317 submitted

Wendy Hack - 03/11/2019 05:18 AM - In Progress

Moving to Nghia. 

Wendy Hack - 03/11/2019 05:18 AM - In Progress

Sounthala Tran - 03/08/2019 04:55 PM - DS Approval

Submitting on behalf of Julie V. 

Sounthala Tran - 03/08/2019 04:55 PM - Review

Issue:
Jacob Johansen currently works in UHEAA Customer Solutions as a Rep T-3-SKIP. Please change his role to Rep T-3 effective Monday, 3/18/19. 

'
WHERE
	Ticket = 60623

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = 1 AND @ERROR = 0
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