CREATE PROCEDURE [cpp].[UpdatePaymentType]
	@PaymentTypeId int,
	@CompassLoanType varchar(6),
	@TivaFileLoanType char(1)
AS
	UPDATE
		PaymentTypes
	SET
		CompassLoanType = @CompassLoanType,
		TivaFileLoanType = @TivaFileLoanType
	WHERE
		PaymentTypeId = @PaymentTypeId

RETURN 0

GRANT EXECUTE ON [cpp].[UpdatePaymentType] TO db_executor