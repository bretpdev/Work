BEGIN TRANSACTION
DECLARE @ERROR INT = 0
DECLARE @ROWCOUNT INT = 0
DECLARE @ExpectedRowCount INT = 22

--Query Body
UPDATE
	[MauiDUDE].[calls].[Reasons]
SET
	[Enabled] = 0
WHERE
	ReasonText in 
('Delinquency Call-Complete Loans',
 'Skip call-Complete Loans',
 'Incomplete Documentation (forms)--Complete Loans',
 'ACH update--Complete Loans',
 'Payment Refund--Complete Loans',
 'Correspondence Follow Up--Complete Loans',
 'Service Members-SCRA--Complete Loans',
 'Techincal Issues Follow Up--Complete Loans',
 'Technical Issues Follow Up--Complete Loans',
 'Death Certificate--Complete Loans',
 'Returning call per 3rd party/borrower''s request-Complete Loans')

SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT



--Query Body
SELECT @ERROR = @@ERROR

IF @ROWCOUNT = @ExpectedRowCount AND @ERROR = 0
	BEGIN
		PRINT 'Transaction committed'
		COMMIT TRANSACTION
	END
ELSE
	BEGIN
		PRINT 'ROWCOUNT:  ' + CAST(@ROWCOUNT as VARCHAR(10))
		PRINT 'ERROR:  ' + CAST(@ERROR as VARCHAR(10))
		PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.'
		ROLLBACK TRANSACTION
	END
