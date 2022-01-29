CREATE PROCEDURE [rtrnmailuh].[GetCaliforniaBorrower]
	@AccountIdentifier varchar(10)
AS

BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	-- Pulls Borrower data when Borrower account number sent in
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID [AccountNumber],
		PD10.DF_SPE_ACC_ID + ',' + RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) + ',' + email.EmailAddress [EmailData],
		0 [Priority],
		PD30.DC_DOM_ST [State]
	FROM
		PD10_PRS_NME PD10
		INNER JOIN PD30_PRS_ADR PD30
			ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
		INNER JOIN calc.EmailAddress email
			ON email.DF_PRS_ID = PD10.DF_PRS_ID
		INNER JOIN LN10_LON LN10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		LEFT JOIN
		(
			SELECT
				BF_SSN
			FROM
				DW01_DW_CLC_CLU DW01
				INNER JOIN PD10_PRS_NME PD10
					ON PD10.DF_PRS_ID = DW01.BF_SSN
			WHERE
				WC_DW_LON_STA IN (02, 03, 04, 05, 06, 07, 12, 14, 22) --Excludes death, disability
		) DW01
			ON DW01.BF_SSN = PD10.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (PD10.DF_SPE_ACC_ID, PD10.DF_PRS_ID)
		AND DW01.BF_SSN IS NOT NULL

END