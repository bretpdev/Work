CREATE PROCEDURE [batchesp].[AddTs26ScrapedLoanInformation]
	@BorrowerSsn CHAR(9),
	@LoanSequence SMALLINT,
	@LoanStatus VARCHAR(40),
	@RepaymentStartDate DATE = NULL
AS

	INSERT INTO batchesp.Ts26ScrapedLoanInformation(BorrowerSsn, LoanSequence, LoanStatus, RepaymentStartDate)
	VALUES (@BorrowerSsn, @LoanSequence, @LoanStatus, @RepaymentStartDate)

RETURN 0
