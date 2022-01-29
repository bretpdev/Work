CREATE PROCEDURE [pmtlnhist].[InsertLoanData]
	@AccountNumber VARCHAR(10)
AS

MERGE
	pmtlnhist.LoanData ExistingData
USING
(
	SELECT
		PD01.DF_SPE_ACC_ID AS [AccountNumber],
		DC01.LF_CLM_ID AS [ClaimID],
		DC11.AF_APL_ID + DC11.AF_APL_ID_SFX AS [UniqueId],
		DC11.LD_TRX_EFF AS [LoanDate],
		LK01_TRX.PX_SHO_DSC AS [PaymentType],
		-DC11.LA_TRX AS [PaymentAmount],
		CASE
			WHEN DC11.LC_REV_IND_TYP IN ('PE', 'BC') THEN 0
			ELSE -DC11.LA_APL_PRI
		END AS [Principal],
		CASE
			WHEN DC11.LC_REV_IND_TYP IN ('PE', 'BC') THEN 0
			ELSE -DC11.LA_APL_INT
		END AS [Interest],
		CASE
			WHEN DC11.LC_REV_IND_TYP IN ('PE', 'BC') THEN 0
			ELSE -DC11.LA_APL_LEG_CST
		END AS [LegalCosts],
		CASE
			WHEN DC11.LC_REV_IND_TYP IN ('PE', 'BC') THEN 0
			ELSE -DC11.LA_APL_OTH_CHR
		END AS [OtherCosts],
		CASE
			WHEN DC11.LC_REV_IND_TYP IN ('PE', 'BC') THEN 0
			ELSE -DC11.LA_APL_COL_CST
		END AS [CollectionCosts],
		CASE
			WHEN DC11.LC_REV_IND_TYP IN ('PE', 'BC') THEN DC11.LA_TRX
			ELSE DC11.LA_OV_PAY
		END AS [Amount],
		ISNULL(LK01_REV.PX_SHO_DSC, '') AS [Type]
	FROM
		ODW..PD01_PDM_INF PD01
		INNER JOIN ODW..DC11_LON_FAT DC11
			ON DC11.BF_SSN = PD01.DF_PRS_ID
		INNER JOIN ODW..DC01_LON_CLM_INF DC01
			ON DC01.BF_SSN = DC11.BF_SSN
			AND DC01.AF_APL_ID = DC11.AF_APL_ID
			AND DC01.AF_APL_ID_SFX = DC11.AF_APL_ID_SFX
		LEFT JOIN ODW..LK01_LGS_CDE_LKP LK01_TRX
			ON LK01_TRX.PM_ATR = 'LC-TRX-TYP'
			AND LK01_TRX.PX_CDE_VAL = DC11.LC_TRX_TYP
		LEFT JOIN ODW..LK01_LGS_CDE_LKP LK01_REV
			ON LK01_REV.PM_ATR = 'LC-REV-IND-TYP'
			AND LK01_REV.PX_CDE_VAL = DC11.LC_REV_IND_TYP
	WHERE
		@AccountNumber = PD01.DF_SPE_ACC_ID
		AND DC01.LF_CLM_ID != ''
		AND
		(
			DC11.LD_TRX_ADJ IS NULL
			OR (DC11.LD_TRX_ADJ IS NOT NULL AND DC11.LC_REV_IND_TYP != '')
		)
		AND LK01_TRX.PX_SHO_DSC != 'ADVICE'
) NewData
	ON NewData.AccountNumber = ExistingData.AccountNumber
	AND NewData.ClaimId = ExistingData.ClaimId
	AND NewData.UniqueId = ExistingData.UniqueId
	AND NewData.LoanDate = ExistingData.LoanDate
	AND NewData.PaymentType = ExistingData.PaymentType
	AND NewData.PaymentAmount = ExistingData.PaymentAmount
	AND NewData.Principal = ExistingData.Principal
	AND NewData.Interest = ExistingData.Interest
	AND NewData.LegalCosts = ExistingData.LegalCosts
	AND NewData.OtherCosts = ExistingData.OtherCosts
	AND NewData.CollectionCosts = ExistingData.CollectionCosts
	AND NewData.Amount = ExistingData.Amount
	AND NewData.[Type] = ExistingData.[Type]
	AND CAST(ExistingData.AddedAt AS DATE) = CAST(GETDATE() AS DATE)
WHEN NOT MATCHED THEN
INSERT
(
	AccountNumber,
	ClaimId,
	UniqueId,
	LoanDate,
	PaymentType,
	PaymentAmount,
	Principal,
	Interest,
	LegalCosts,
	OtherCosts,
	CollectionCosts,
	Amount,
	[Type]
)
VALUES
(
	NewData.AccountNumber,
	NewData.ClaimId,
	NewData.UniqueId,
	NewData.LoanDate,
	NewData.PaymentType,
	NewData.PaymentAmount,
	NewData.Principal,
	NewData.Interest,
	NewData.LegalCosts,
	NewData.OtherCosts,
	NewData.CollectionCosts,
	NewData.Amount,
	NewData.[Type]
);