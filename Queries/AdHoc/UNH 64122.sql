USE NeedHelpUheaa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		NeedHelpUheaa..DAT_Ticket
	SET
		[Status] = 'DCR Approval',
		PreviousStatus = 'Review',
		History = 'Tiffany Ames - 11/04/2019 11:32 AM - DCR Approval

Per updated guidance from Compliance, IBR removal is no longer necessary for these accounts. Please stop the removal of approved IBR. 

Matthew Jones - 11/04/2019 09:33 AM - DCR Approval

DCR U20191104103127 submitted

Matthew Jones - 11/01/2019 08:26 AM - Review

CCC 78399 submitted to review my DCR

Tiffany Ames - 11/01/2019 07:49 AM - Review

3880655260 needs to go back to the IL plan they were on 03/11/2019.

Tiffany Ames - 11/01/2019 07:41 AM - Review

Court changed from Tiffany Ames to Matthew Jones

Matthew Jones - 10/24/2019 11:38 AM - Review

Tiffany,

For 3880655260 it appears they are already in IL, are we moving them back to the previous IL with first due date 3/11/19?


Matthew Jones - 10/24/2019 08:48 AM - Review

Court changed from David Halladay to Matthew Jones

Matthew Jones - 10/23/2019 12:30 PM - Review

Additional questions required returning to Review Status

Wendy Hack - 10/17/2019 02:17 PM - In Progress

Moving back to Matthew. 

Wendy Hack - 10/17/2019 02:16 PM - In Progress

Matthew Jones - 10/17/2019 02:10 PM - DCR Approval

Tiffany Ames - 10/16/2019 10:55 AM - Review

The IB plan needs to be removed, and the account needs to go back to the IL schedule because the IBR application were supposed to be denied, not approved.

Tiffany Ames - 10/16/2019 10:55 AM - Review

Court changed from Tiffany Ames to Matthew Jones

Matthew Jones - 10/16/2019 07:49 AM - Review

Tiffany,

do you want to reactivate the previous repayment IB schedules or will you be redisclosing them to new plans afterwards?



Tiffany Ames - 10/15/2019 08:23 AM - Review

Issue:
The following accounts need their active IBR schedule removed. Per Compliance the IBR plans were incorrectly approved for the borrowers.

0404821699 (spouse income docs are not acceptable)
3880655260 (need clarification of marital status)
8536079822 (Need updated income documentation)
2569633007 (conflicting marital status info in section 4, can’t determine spouse’s frequency of pay)
5246530799 (need documentation of family size)
3236179333 (SS statement is too old, need updated SS statement if it is a taxable form of income)

Please let me know if I need to submit this NH ticket in a different way. Thanks for your help. '

	-- Save/Set the row count and error number (if any) from the previously executed statement
	SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

	UPDATE
		NeedHelpUheaa..DAT_TicketsAssociatedUserID
	SET
		SqlUserId = 4039
	WHERE
		Ticket = 63807
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