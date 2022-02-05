CREATE PROCEDURE [pridrcrp].[InsertToPaymentHistory]
(
	@PaymentHistoryRecord PaymentHistoryRecord READONLY,
	@BorrowerInformationId INT
)
AS

INSERT INTO [pridrcrp].[PaymentHistory] (BorrowerInformationId, [Description], ActionDate, EffectiveDate, TotalPaid, InterestPaid, PrincipalPaid, LcNsfPaid, AccruedInterest, LcNsfDue, PrincipalBalance)
SELECT
	@BorrowerInformationId,
	PHR.[Description],
	PHR.[ActionDate],
	PHR.[EffectiveDate],
	PHR.[TotalPaid],
	PHR.[InterestPaid],
	PHR.[PrincipalPaid],
	PHR.[LcNsfPaid],
	PHR.[AccruedInterest],
	PHR.[LcNsfDue],
	PHR.[PrincipalBalance]
FROM	
	@PaymentHistoryRecord PHR
	LEFT JOIN PaymentHistory PH
		ON PHR.[Description] = PH.[Description]
		AND (PHR.[ActionDate] = PH.[ActionDate] OR (PHR.[ActionDate] IS NULL AND PH.[ActionDate] IS NULL))
		AND (PHR.[EffectiveDate] = PH.[EffectiveDate] OR (PHR.[EffectiveDate] IS NULL AND PH.[EffectiveDate] IS NULL))
		AND (PHR.[TotalPaid] = PH.[TotalPaid] OR (PHR.[TotalPaid] IS NULL AND PH.[TotalPaid] IS NULL))
		AND (PHR.[InterestPaid] = PH.[InterestPaid] OR (PHR.[InterestPaid] IS NULL AND PH.[InterestPaid] IS NULL))
		AND (PHR.[PrincipalPaid] = PH.[PrincipalPaid] OR (PHR.[PrincipalPaid] IS NULL AND PH.[InterestPaid] IS NULL))
		AND (PHR.[LcNsfPaid] = PH.[LcNsfPaid] OR (PHR.[LcNsfPaid] IS NULL AND PH.[LcNsfPaid] IS NULL))
		AND (PHR.[AccruedInterest] = PH.[AccruedInterest] OR (PHR.[AccruedInterest] IS NULL AND PH.[AccruedInterest] IS NULL))
		AND (PHR.[LcNsfDue] = PH.[LcNsfDue] OR (PHR.[LcNsfDue] IS NULL AND PH.[LcNsfDue] IS NULL))
		AND (PHR.[PrincipalBalance] = PH.[PrincipalBalance] OR (PHR.[PrincipalBalance] IS NULL AND PH.[PrincipalBalance] IS NULL))
		AND @BorrowerInformationId = PH.BorrowerInformationId
WHERE
	PH.TransactionId IS NULL
