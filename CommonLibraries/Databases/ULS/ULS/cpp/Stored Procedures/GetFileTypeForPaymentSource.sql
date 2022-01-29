CREATE PROCEDURE [cpp].[GetFileTypeForPaymentSource]
	@PaymentSourceId int
AS
	SELECT
		FT.FileTypeId,
		FT.FileType
	FROM
		FileTypes FT
	JOIN PaymentSources PS
		ON FT.FileTypeId = PS.FileTypeId
	WHERE
		PS.PaymentSourcesId = @PaymentSourceId

RETURN 0

GRANT EXECUTE ON [cpp].[GetFileTypeForPaymentSource] TO db_executor