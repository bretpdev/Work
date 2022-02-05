CREATE PROCEDURE [pmtlnhist].[GetLoanSummary]
	@AccountIdentifier VARCHAR(10),
	@Date DATE
AS
	SELECT
		AccountNumber,
		[Name],
		Principal,
		Interest,
		LegalCosts,
		OtherCosts,
		CollectionCosts,
		ProjectedCollectionCosts,
		Principal + Interest + LegalCosts + OtherCosts + CollectionCosts + ProjectedCollectionCosts AS TotalBalance
	FROM
		OLS.pmtlnhist.BorrowerLoanPayment BP
		INNER JOIN ODW..PD01_PDM_INF PD01
			ON BP.AccountNumber IN (PD01.DF_PRS_ID, PD01.DF_SPE_ACC_ID)
	WHERE
		@AccountIdentifier IN (PD01.DF_PRS_ID, PD01.DF_SPE_ACC_ID)
		AND CAST(AddedAt AS DATE) = @Date