CREATE PROCEDURE [achrevfed].[AchReviewGetUnprocessedAccounts]
AS

	SELECT DISTINCT
		[ACHReviewId],
		[AccountNumber],
		PD10.DF_PRS_ID AS Ssn,
		[Name],
		[DaysDelq],
		[HomeConsent],
		[HomePhone],
		[AltConsent],
		[AltPhone],
		[WorkConsent],
		[WorkPhone],
		[DX_STR_ADR_1],
		[DX_STR_ADR_2],
		[DX_STR_ADR_3],
		[DM_CT],
		[DC_DOM_ST],
		[DF_ZIP_CDE],
		[LF_LON_CUR_OWN],
		[AmountDue],
		[TotalBalance]
	FROM
		achrevfed.ACHReview AR
		INNER JOIN CDW..PD10_PRS_NME PD10
			ON PD10.DF_SPE_ACC_ID = AR .AccountNumber
	WHERE
		ProcessedOn IS NULL
		AND DeletedAt IS NULL

RETURN 0
