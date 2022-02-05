CREATE PROCEDURE [olpayrevr].[GetErrorReport]

AS
	
	SELECT
		Ssn,
		PaymentAmount,
		PaymentEffectiveDate,
		PaymentPostDate,
		PaymentType, 
		PaymentAlreadyReversed,
		HadError,
		ErrorDescription
	FROM
		OLS.olpayrevr.ReversalsProcessingQueue
	WHERE
		HadError = 1
		OR PaymentAlreadyReversed = 1
		OR ProcessedAt IS NULL
	ORDER BY
		Ssn,
		PaymentAmount

RETURN 0
