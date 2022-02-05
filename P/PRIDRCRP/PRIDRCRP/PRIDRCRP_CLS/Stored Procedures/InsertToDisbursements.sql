CREATE PROCEDURE [pridrcrp].[InsertToDisbursements]
(
	@DisbursementRecord DisbursementRecord READONLY,
	@BorrowerInformationId INT
)
AS

INSERT INTO [pridrcrp].[Disbursements] (BorrowerInformationId, DisbursementDate, InterestRate, LoanType, DisbursementNumber, LoanId, DisbursementAmount, CapInterest, RefundCancel, BorrPaidPrin, PrinOutstanding, PaidInterest)
SELECT
	@BorrowerInformationId,
	DR.[DisbursementDate],
	DR.[InterestRate],
	DR.[LoanType],
	DR.[DisbursementNumber],
	DR.[LoanId],
	DR.[DisbursementAmount],
	DR.[CapInterest],
	DR.[RefundCancel],
	DR.[BorrPaidPrin],
	DR.[PrinOutstanding], 
	DR.[PaidInterest]
FROM
	@DisbursementRecord DR
	LEFT JOIN [pridrcrp].[Disbursements] D
		ON DR.[DisbursementDate] = D.[DisbursementDate]
		AND DR.[InterestRate] = D.[InterestRate]
		AND DR.[LoanType] = D.[LoanType]
		AND DR.[DisbursementNumber] = D.[DisbursementNumber]
		AND DR.[LoanId] = D.[LoanId]
		AND DR.[DisbursementAmount] = D.[DisbursementAmount]
		AND DR.[CapInterest] = D.[CapInterest]
		AND (DR.[RefundCancel] = D.[RefundCancel] OR (DR.[RefundCancel] IS NULL AND D.[RefundCancel] IS NULL))
		AND DR.[BorrPaidPrin] = D.[BorrPaidPrin]
		AND DR.[PrinOutstanding] = D.[PrinOutstanding]
		AND DR.[PaidInterest] = D.[PaidInterest]
		AND D.BorrowerInformationId = @BorrowerInformationId
WHERE
	D.DisbursementId IS NULL

