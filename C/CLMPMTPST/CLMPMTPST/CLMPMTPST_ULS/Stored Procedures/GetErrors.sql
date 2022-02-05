CREATE PROCEDURE [clmpmtpst].[GetErrors]
	@RunStart DATETIME,
	@RunEnd DATETIME
AS

	SELECT
		AllErrors.ErrorId,
		GenErrors.[Description] AS GeneralErrorDescription,
		ClaimErrors.[Description] AS ClaimErrorDescription,
		ClaimErrors.AccountNumber AS AccountNumber,
		ClaimErrors.PaymentAmount AS PaymentAmount,
		ClaimErrors.EffectiveDate AS EffectiveDate,
		ClaimErrors.LoanSequence AS LoanSequence
	FROM
		clmpmtpst.Errors AllErrors
		LEFT JOIN
		(
			SELECT
				E.ErrorId,
				ED.[Description]
			FROM
				clmpmtpst.Errors E
				INNER JOIN clmpmtpst.ErrorDescriptions ED
					ON ED.DescriptionId = E.ErrorDescriptionId
				LEFT JOIN clmpmtpst.ClaimPayments CP
					ON CP.ErrorId = E.ErrorId
			WHERE
				E.CreatedAt BETWEEN @RunStart AND @RunEnd
				AND CP.ErrorId IS NULL
		) GenErrors
			ON GenErrors.ErrorId = AllErrors.ErrorId
		LEFT JOIN
		(
			SELECT
				E.ErrorId,
				ED.[Description],
				CP.AccountNumber,
				CP.PaymentAmount,
				CP.EffectiveDate,
				CP.LoanSequence
			FROM
				clmpmtpst.Errors E
				INNER JOIN clmpmtpst.ErrorDescriptions ED
					ON ED.DescriptionId = E.ErrorDescriptionId
				INNER JOIN clmpmtpst.ClaimPayments CP
					ON CP.ErrorId = E.ErrorId
			WHERE
				E.CreatedAt BETWEEN @RunStart AND @RunEnd
		) ClaimErrors
			ON GenErrors.ErrorId = AllErrors.ErrorId
	WHERE
		AllErrors.CreatedAt BETWEEN @RunStart AND @RunEnd


RETURN 0
