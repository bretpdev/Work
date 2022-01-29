CREATE PROCEDURE [cpp].[SetOverpaymentTramsmittal]
	@AccountNumber varchar(10),
	@LoanSequence int,
	@FirstDisbursementDate datetime,
	@ManifestNumber varchar(15),
	@LoanType varchar(1),
	@OriginatorLoanId varchar(13),
	@NsldsId varchar(17)
AS
	INSERT INTO OverpaymentTransmittals(AccountNumber, LoanSequence, FirstDisbursementDate, ManifestNumber, LoanType, OriginatorLoanId, NsldsId)
	VALUES(@AccountNumber, @LoanSequence, @FirstDisbursementDate, @ManifestNumber, @LoanType, @OriginatorLoanId, @NsldsId)

RETURN 0

GRANT EXECUTE ON [cpp].[SetOverpaymentTramsmittal] TO db_executor