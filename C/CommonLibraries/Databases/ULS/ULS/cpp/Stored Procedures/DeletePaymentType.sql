CREATE PROCEDURE [cpp].[DeletePaymentType]
	@PaymentTypeId int
AS
	UPDATE
		PaymentTypes
	SET
		Active = 0
	WHERE
		PaymentTypeId = @PaymentTypeId

RETURN 0

GRANT EXECUTE ON [cpp].[DeletePaymentType] TO db_executor