USE BSYS
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		BSYS..SCKR_DAT_ScriptRequests
	SET
		CurrentStatus = 'Complete',
		PreviousStatus = 'Promotion',
		Court = '',
		CourtDate = NULL,
		StatusDate = '2021-10-11 00:00:00',
		History = 'Steven Ostler - 10/11/2021  01:23 PM - Complete
Request promoted and complete.  PIR record created for Deac Cox to perform post-implementation review.

Promoted before SR 5168 & SR 5169

Had SS run the 5 promotion scripts.

Deleted everything in the folder: X:\Sessions\UHEAA Codebase\BCSRETMAIL.
Copied the contents of X:\PADU\UheaaCodeBase\BCSRETMAIL\ and pasted it in X:\Sessions\UHEAA Codebase\BCSRETMAIL.

Opened the Access application in ACDC.
In the first table ''Add Keys & Roles'', in the right box ''Add a Key to a System'', selected the system ACDC. Then, typed in the Key Name field ''BCSRETMAIL''. The description entered was ''Barcode Scanning Software''.
Next, selected tab ''Add & Remove Keys to Role''.
Selected the Roles: ROLE - Document Services Supervisor and ROLE - UHEAA Processing Supervisor - with Docs, then chose System: ''ACDC''.  In the ADD section, selected the BCSRETMAIL key and clicked the Add Access button. 
Selected the Roles: ROLE - Doc Services Rep and ROLE - UHEAA Processing Rep - with Docs and kept the system as ACDC. Again, chose the BCSRETMAIL key and clicked the Add Access button. Access was granted.

Opened ACDC and chose the Application Settings.
Selected EXE from the Type of Application box. Then filled out each row with the following:
Application Name: BCSRETMAIL
Access Key: BCSRETMAIL
Starting EXE: BCSRETMAIL
Source Path Folder: X:\Sessions\UHEAA Codebase\BCSRETMAIL
Mode: Choose ''MODE (string)''
Icon:  X:\Sessions\UHEAA Codebase\BCSRETMAIL\BCSRETMAIL.jpg

Clicked Save and it was added to ACDC.

Melanie Garfield - 10/06/2021  05:11 PM - Promotion

Confirmed CCB approval received 10/06/2021

Steven Ostler - 10/06/2021  09:18 AM - Promotion

CCB approval received 10/06/2021

Bret Pehrson - 10/05/2021  10:18 AM - Promotion

Merged code into master, verified the promotion scripts are updated. Ready to promote

Steven Ostler - 09/29/2021  01:17 PM - Promotion
Training complete and request returned to Bret Pehrson to be promoted.

Training Email was sent out

Steven Ostler - 09/29/2021  01:17 PM - Training
Training commenced.

Melanie Garfield - 09/29/2021  01:10 PM - Trainer Queue
Business Systems Script testing approved and request forwarded to Dean Cox for training.

Melanie Garfield - 09/29/2021  01:04 PM - Test Approval

The SACKER buttons were greyed out for Steve, so I was able to push it forward for him due to the technical issues.

Steven Ostler - 09/29/2021  12:56 PM - Test Approval

Late Note - Catching Sacker Up: Required Test Stage Gate approvals received and are located in the supporting documents

Melanie Garfield - 09/29/2021  12:54 PM - Test Approval
BS testing completed and request forwarded for testing approval.

Steven Ostler - 09/29/2021  12:41 PM - Testing
Request released from hold and returned to Testing.

Eliezer Cadena - 09/29/2021  12:38 PM - Hold - Test Stage Gate

Placing back to Steve''s court.

Eliezer Cadena - 09/29/2021  12:37 PM - Hold - Test Stage Gate

signed off on training.

Bret Pehrson - 09/28/2021  04:25 PM - Hold - Test Stage Gate

Merged code into master, verified the promotion scripts are updated. Ready to promote after all approvals.

Barbra Emery - 09/28/2021  10:55 AM - Hold - Test Stage Gate

Placing in Eli''s court.

Barbra Emery - 09/28/2021  10:55 AM - Hold - Test Stage Gate

tranining complete

Barbra Emery - 09/28/2021  10:55 AM - Hold - Test Stage Gate

training complete

Steven Ostler - 09/24/2021  09:59 AM - Hold - Test Stage Gate
Request put on hold.  See below for details.

Test Stage Gate sent out.  Requested responses NLT COB Tuesday, 09/28/2021

Steven Ostler - 07/09/2021  08:44 AM - Testing

Regression Testing Estimates:
   - 80 hours
   - EDC 7/22/21

Steven Ostler - 07/09/2021  08:42 AM - Testing
Request released from hold and returned to Testing.

Steven Ostler - 02/25/2021  07:28 AM - Hold - BU Testing
Request put on hold.  See below for details.

BU Testing

Steven Ostler - 01/27/2021  08:12 AM - Testing
BS testing commenced.

Melanie Garfield - 11/23/2020  11:22 AM - Tester Queue
Request assigned to Steven Ostler to test.

Bret Pehrson - 11/23/2020  11:10 AM - Tester Assignment
Coding completed and request forwarded for tester assignment.

Jarom Ryan - 11/23/2020  09:01 AM - Coding
Request released from hold and returned to Coding.

Code Stage Gate Approved

Colton McComb - 11/18/2020  11:50 AM - Hold - Code Stage Gate

Created "System_Security_Impact_Analysis_UH_SR5157" and placed in Q:\Change Control Board\Security Impact Assessments\UHEAA\Pending Submission

Jarom Ryan - 11/11/2020  01:31 PM - Hold - Code Stage Gate

Colt this needs a SIA form

Bret Pehrson - 11/09/2020  02:51 PM - Hold - Code Stage Gate
Request put on hold.  See below for details.

Ready for code stage gate.

Jacob Kramer - 11/06/2020  08:28 AM - Coding
Request released from hold and returned to Coding.

Code review attached

Bret Pehrson - 11/05/2020  02:52 PM - Hold - Code Review
Request put on hold.  See below for details.

Ready for code review.

Bret Pehrson - 08/10/2020  09:21 AM - Coding
Coding commenced.

Jarom Ryan - 08/05/2020  01:59 PM - Programmer Queue
Request assigned to Bret Pehrson to code.

Melanie Garfield - 08/03/2020  08:57 AM - Programmer Assignment
Request reviewed by Business Systems and submitted for programmer assignment.

Steven Ostler - 08/03/2020  07:56 AM - BS Review
Request approved for Document Services and submitted for BS Review.

Required BR Stage Gate approvals received. All others are implicit at this point. Documentation is in the Supporting Documents tab.

Steven Ostler - 08/03/2020  07:52 AM - Draft
Request released from hold and returned to Draft.

Steven Ostler - 07/29/2020  03:57 PM - Hold - BR Stage Gate
Request put on hold.  See below for details.

Sent BR Stage Gate with approvals requested NLT COB Friday, July 31, 2020. Documentation is in the Supporting Documents tab.

Steven Ostler - 07/29/2020  03:54 PM - Draft

BU approved of the merger. Email is documented in the Supporting Documents tab.

Steven Ostler - 07/29/2020  03:54 PM - Draft
Request released from hold and returned to Draft.

Steven Ostler - 07/16/2020  07:34 AM - Hold - Input Requested
Request put on hold.  See below for details.

Need BU approval, Dean OOO until next week, will place on hold until then.

'
	WHERE
		Request = 5157

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