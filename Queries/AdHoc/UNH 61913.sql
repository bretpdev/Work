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
	History = 'Christopher Fowlks - 06/10/2019 11:55 AM - Complete

I had Kellie clear her cookies and now it is working so we are all set! thanks!

Nghia Nguyen - 06/10/2019 11:30 AM - Complete

Chris, Please have this user try a different browser as well as clear their cookies and try again. Please let us know if this work? Thanks

Nghia Nguyen - 06/10/2019 09:57 AM - Complete

updated: added onelink tiers to SADB

Nghia Nguyen - 06/10/2019 09:56 AM - Complete

added to SADB

Jordan Bobo - 06/10/2019 08:43 AM - Complete

Update: meant SADB not CSYS.

Jordan Bobo - 06/10/2019 08:42 AM - Complete

Moving back to Nghia to update CSYS and reverify

Jeremy Blair - 06/07/2019 03:02 PM - Complete

The BU logged ticket 61852 stating the User is not able to access the Imaging system. Will you please verify she has been given the correct access.

Thank you!

Nghia Nguyen - 05/30/2019 12:09 PM - Complete

Court changed from David Halladay to Jordan Bobo

Nghia Nguyen - 05/30/2019 12:08 PM - Complete

confirmed UH NSLDS

Nghia Nguyen - 05/29/2019 09:59 AM - In Progress

submitted signed forms

Nghia Nguyen - 05/24/2019 07:55 AM - In Progress

UH NSLDS generated 506003.
added to e-oscar

Nghia Nguyen - 05/20/2019 08:36 AM - In Progress

assigned FSA Token AVT849842815
sent new user to BU.

Nghia Nguyen - 05/17/2019 10:14 AM - In Progress

added to csys
added to cddb
atx4f
added to arcs and queues
added onelink tiers and group SEQUENCE 03280, 3281
added to clearing house
ccc 77453 submitted for page center 

Nghia Nguyen - 05/17/2019 09:52 AM - In Progress

oars completed UT02597 assigned.

Nghia Nguyen - 05/15/2019 08:14 AM - In Progress

CNOC 64998 submitted.
Oars 245838 submitted

Wendy Hack - 04/25/2019 01:05 PM - In Progress

Moving to Nghia. 

Wendy Hack - 04/25/2019 01:04 PM - In Progress

Dorothy Bailey - 04/25/2019 11:00 AM - DS Approval

HR will submit a new system access ticket when the employee is cleared to work in UHEAA Subcontract Processing.

Dorothy Bailey - 04/25/2019 10:59 AM - DS Approval

Submitted ticket on behalf of Julie Vincent.

Dorothy Bailey - 04/25/2019 10:59 AM - Review

Issue:
Kellie Wright starts in UHEAA Pre-Processing as a Rep I on 5/20/19. Please place her in the role of UHEAA Processing Rep Temp.

HR will submit a new IT ticket when the employee is cleared to work in UHEAA Subcontract Processing.

'
WHERE
	Ticket = 61275

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		NeedHelpUheaa..DAT_TicketsAssociatedUserID
	SET
		SqlUserId = 4064
	WHERE
		[Role] = 'Court'
		AND Ticket = 61275

	SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT, @ERROR = @ERROR + @@ERROR

	UPDATE
		NeedHelpUheaa..DAT_TicketsAssociatedUserID
	SET
		SqlUserId = 4097
	WHERE
		[Role] = 'PreviousCourt'
		AND Ticket = 61275

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