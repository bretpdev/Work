CREATE PROCEDURE [dbo].[InsertNewXmlOtherLoans]
	@ApplicationId CHAR(10) = NULL,
	@SpouseIndicator BIT,
	@OwnerLender VARCHAR(20),
	@LoanType VARCHAR(50),
	@OutstandingBalance DECIMAL(14,2),
	@InterestRate DECIMAL(14,2)
AS
	INSERT INTO dbo.Other_Loans(application_id, spouse_indicator, loan_type, owner_lender, outstanding_balance, interest_rate)
	VALUES(@ApplicationId,@SpouseIndicator,@LoanType,@OwnerLender,@OutstandingBalance,@InterestRate)

RETURN 0