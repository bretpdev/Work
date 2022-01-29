CREATE PROCEDURE [cpp].[InsertPaymentType]
	@CompassLoanType varchar(6),
	@TivaFileLoanType char(1)
AS
	INSERT INTO PaymentTypes(CompassLoanType, TivaFileLoanType)
	VALUES(@CompassLoanType, @TivaFileLoanType)

RETURN 0

GRANT EXECUTE ON [cpp].[InsertPaymentType] TO db_executor