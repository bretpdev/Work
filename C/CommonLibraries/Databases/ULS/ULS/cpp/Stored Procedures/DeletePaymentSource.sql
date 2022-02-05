CREATE PROCEDURE [cpp].[DeletePaymentSource]
	@PaymentSourceId int = 0
AS
	UPDATE
		PaymentSources
	SET
		Active = 0
	WHERE 
		PaymentSourcesId = @PaymentSourceId

RETURN 0

GRANT EXECUTE ON [cpp].[DeletePaymentSource] TO db_executor