USE NeedHelpUheaa
GO

BEGIN TRANSACTION
	DECLARE @ERROR INT = 0
	DECLARE @ROWCOUNT INT = 0
	DECLARE @Expected INT = 1

UPDATE 
	NeedHelpUheaa..DAT_Ticket 
SET 
	Issue = 'Received the following via CCC    The following row of data was removed from the GDG when UTTSJGW ABENDed on 01/06/2020. All dates in CCYYMMDD Format Header 01 20200106 <= Notify Date CM <= Source 000749 <= Guarantor when CM or GR - else spaces or zeros Detail Record 02 XXX-XX-6625 <= SSN XXX-XX-6625 <= Student SSN XXXXXXXXX <= School Code 11 <= Enrollment Status 20240402 <= Sep. Date 20191218 <= Cert. Date 20191028 <= Enrollment Status Effective Date (ESED) CH <= Source <= Loan Term Begin Date can be zeroes or spaces <= Loan Term End Date can be zeroes or spaces Please review the account and determine if any updates are needed.',  
	History = 'Nghia Nguyen - 01/09/2020 01:32 PM - Discussion    Please review this borrower enrollment.    David Halladay - 01/09/2020 11:33 AM - Discussion    This is from CCC 78706 and the borrower account is 57 5981 5207    David Halladay - 01/09/2020 11:23 AM - Discussion    Issue:  Received the following via CCC    The following row of data was removed from the GDG when UTTSJGW ABENDed on 01/06/2020. All dates in CCYYMMDD Format Header 01 20200106 <= Notify Date CM <= Source 000749 <= Guarantor when CM or GR - else spaces or zeros Detail Record 02 XXX-XX-6625 <= SSN XXX-XX-6625 <= Student SSN XXXXXXXXX <= School Code 11 <= Enrollment Status 20240402 <= Sep. Date 20191218 <= Cert. Date 20191028 <= Enrollment Status Effective Date (ESED) CH <= Source <= Loan Term Begin Date can be zeroes or spaces <= Loan Term End Date can be zeroes or spaces Please review the account and determine if any updates are needed.    '
WHERE
	ticket = 65090

SELECT @ROWCOUNT = @@ROWCOUNT, @ERROR = @@ERROR

IF @ROWCOUNT = @Expected AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
		--ROLLBACK TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END