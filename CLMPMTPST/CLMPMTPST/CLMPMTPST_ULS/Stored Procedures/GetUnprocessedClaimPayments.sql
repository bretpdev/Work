CREATE PROCEDURE [clmpmtpst].[GetUnprocessedClaimPayments]
AS
	
	SELECT 
		CP.ClaimPaymentId,
		CP.AccountNumber,
		PD10.DF_PRS_ID AS Ssn,
		CP.PaymentAmount,
		CP.EffectiveDate,
		CP.LoanSequence,
		CP.LastName
	FROM
		clmpmtpst.ClaimPayments CP
		INNER JOIN UDW..PD10_PRS_NME PD10
			ON PD10.DF_SPE_ACC_ID = CP.AccountNumber
	WHERE
		CP.ProcessedAt IS NULL
		AND CP.DeletedAt IS NULL

RETURN 0
