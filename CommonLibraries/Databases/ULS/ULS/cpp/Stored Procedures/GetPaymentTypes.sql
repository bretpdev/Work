CREATE PROCEDURE [cpp].[GetPaymentTypes]
AS
	SELECT
		PaymentTypeId,
		TivaFileLoanType,
		CompassLoanType
	FROM
		PaymentTypes
	WHERE
		Active = 1

RETURN 0

GRANT EXECUTE ON [cpp].[GetPaymentTypes] TO db_executor