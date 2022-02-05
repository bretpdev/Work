CREATE PROCEDURE [dbo].[RTML_GetBarcodeInfoToInvalidate]
AS
	SELECT
		RecipientId,
		LetterId,
		CreateDate
	FROM
		RTML_DAT_BarcodeData
	WHERE
		AddressInvalidatedDate IS NULL
RETURN 0

GRANT EXECUTE ON RTML_GetBarcodeInfoToInvalidate TO db_executor