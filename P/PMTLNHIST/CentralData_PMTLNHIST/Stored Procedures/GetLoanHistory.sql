CREATE PROCEDURE [pmtlnhist].[GetLoanHistory]
	@AccountIdentifier VARCHAR(10),
	@Date DATE
AS
	SELECT
		ClaimId,
		UniqueId,
		CONVERT(VARCHAR, LoanDate, 101) AS [LoanDate],
		PaymentType,
		PaymentAmount,
		Principal,
		Interest,
		LegalCosts,
		OtherCosts,
		CollectionCosts,
		Amount,
		[Type]
	FROM
		OLS.pmtlnhist.LoanData LD
		INNER JOIN ODW..PD01_PDM_INF PD01
			ON PD01.DF_SPE_ACC_ID = LD.AccountNumber
	WHERE
		@AccountIdentifier IN (PD01.DF_PRS_ID, PD01.DF_SPE_ACC_ID)
		AND CAST(AddedAt AS DATE) = @Date
	ORDER BY
		ClaimId,
		CAST(LoanDate AS DATE),
		Interest