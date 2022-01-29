USE NeedHelpUheaa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0

	UPDATE
		DAT_Ticket
	SET
		History = 'Jeremy Blair - 08/16/2017 11:17 AM - Discussion

		Submitted tiket 52847 to remove the SSN form the Issue and History of this tickdet

		Jeremy Blair - 08/16/2017 11:14 AM - Discussion

		According to AES, the Accoun Number is included in the Key Data. This Borrower''s account number is 5826642427.

		Wendy Hack - 08/08/2017 08:08 AM - Discussion

		I updated the issue. AES never provided the account number for this. 

		Wendy Hack - 08/04/2017 07:52 AM - Discussion

		I updated the issue asking for the account number. I will also keep you updated on the fix. Also, moving to my court. 

		Wendy Hack - 08/04/2017 07:52 AM - Discussion

		Issue:
		AES submitted CCC issue 74145. 

		We received the following message out of the UTTDIH Claim Extract job last night: * JOB: UTTDJIH STEP: UT000300 DATE: 08/03/17 TIME: 00:25:53 * PROGRAM: TDXIH PARAGRAPH: TDXIH MAX PHN BOR * ACCESS: PGM ACTION: FILE/TABLE/MOD: STATUS: * KEY DATA: 0001 * MESSAGE TEXT: * WARNING: WORKING STORAGE BORROWER HME PHN SKIP TBL IS FULL - INCREMENT What this means is that we overflowed an internal table for this borrower that holds 25 occurrences of borrower home phone # skip data. IT believes it is because there are 60 rows on the PD41 Phone History for the borrower. The claim was bypassed and the ''CL037'' task was cancelled by the process. If the claim is re-attempted by the Claim Extract process it will fail again for this condition. I am researching to verify if there is a work-around from the business side to get the claim to process manually OR if we should be looking into making a PSEUDO change to increase the table occurrences enough so that the claim can process correctly through the system.'
	WHERE
		Ticket = 52663

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
