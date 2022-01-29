BEGIN TRANSACTION
DECLARE @ERROR INT = 0
DECLARE @ROWCOUNT INT = 0
DECLARE @ExpectedRowCount INT = 28

--Query Body
UPDATE
	[MauiDUDE].[calls].[Reasons]
SET
	[Enabled] = 0
WHERE
	(ReasonText = 'Questions' AND CategoryId = 11) OR
	ReasonText in 
('ACH Benefits- Complete Loans', 
 'Dispute/change- Complete Loans', 
 'Loans defaulted- Complete Loans', 
 'Demos updates- Complete Loans', 
 'Add/remove E-Corr- Complete Loans',
 'Loan Origination-Complete Loans',
 'SCRA- Complete Loans',
 'Military Deferment- Complete Loans',
 'ACH Add/Change- Complete Loans',
 'ACH Delete/suspend- Complete Loans',
 'Check By Phone- Complete Loans',
 'Questions- Complete Loans',
 'balance/billing inquiry',
 'Pay Off Request- Complete Loans',
 'Due Date Change- Complete Loans',
 'Balance/billing/interest inquiry- Complete Loans',
 'Request workout plan- Complete Loans',
 'Delinquent- Complete Loans',
 'Other- Complete Loans',
 'Needs tax info- Complete Loans',
 'Campus Door Login issues')
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT

DECLARE @CompleteLoansCategoryId int = 27
INSERT INTO [MauiDUDE].calls.Categories (CategoryId, Title) VALUES (27, 'Complete Loans')
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT

INSERT INTO [MauiDUDE].calls.Reasons (CategoryId, ReasonText, Cornerstone, Uheaa, Inbound, Outbound, [Enabled]) VALUES
(@CompleteLoansCategoryId, 'Loan Application Process', 0, 1, 1, 0, 1),
(@CompleteLoansCategoryId, 'Servicing Transferred', 0, 1, 1, 0, 1)
SELECT @ROWCOUNT = @ROWCOUNT + @@ROWCOUNT

DECLARE @RepaymentCategoryId INT = 11
INSERT INTO [MauiDUDE].calls.Reasons (CategoryId, ReasonText, Cornerstone, Uheaa, Inbound, Outbound, [Enabled]) VALUES
(@RepaymentCategoryId, 'Balance Inquiry', 1, 1, 1, 0, 1),
(@RepaymentCategoryId, 'Billing Inquiry', 1, 1, 1, 0, 1),
(@RepaymentCategoryId, 'Difficulty Making Payments', 1, 1, 1, 0, 1)
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
