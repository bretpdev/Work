CREATE PROCEDURE [cpp].[GetPaymentSources]
AS
	SELECT
		PS.PaymentSourcesId,
		PS.PaymentSource,
		PS.InstitutionId,
		PS.[FileName]
	FROM
		PaymentSources PS
	JOIN FileTypes FT
		ON PS.FileTypeId = FT.FileTypeId
	WHERE
		PS.Active = 1

RETURN 0

GRANT EXECUTE ON [cpp].[GetPaymentSources] TO db_executor