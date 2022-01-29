
CREATE PROCEDURE [dbo].[GetOALetters]
AS
BEGIN
	SELECT
		LetterTypeId,
		LetterType,
		Letter,
		IsCompany,
		HasPaymentSource,
		HasEffectiveDate,
		HasAccountNumber
	FROM
		OperationalAccountingLetters
	WHERE
		DeletedAt IS NULL
END