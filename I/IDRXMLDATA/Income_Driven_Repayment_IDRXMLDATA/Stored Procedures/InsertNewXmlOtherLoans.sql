CREATE PROCEDURE [idrxmldata].[InsertNewXmlOtherLoans]
	@ApplicationId INT,
	@SpouseIndicator BIT,
	@OwnerLender VARCHAR(50),
	@LoanType VARCHAR(10),
	@OutstandingBalance MONEY,
	@InterestRate DECIMAL(18,2),
	@OutstandingInterest MONEY
AS
	INSERT INTO dbo.Other_Loans(application_id, spouse_indicator, loan_type, owner_lender, outstanding_balance, interest_rate, outstanding_interest)
	VALUES(@ApplicationId,@SpouseIndicator,@LoanType,@OwnerLender,@OutstandingBalance,@InterestRate,@OutstandingInterest)
