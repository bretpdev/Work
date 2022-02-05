CREATE PROCEDURE [rtrnmailuh].[GetCaliforniaBorForEnd]
	@AccountIdentifier varchar(10)
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	-- Pulls Borrower when using Co-borrower account number
	SELECT DISTINCT
		PD10_Bor.DF_SPE_ACC_ID [AccountNumber],
		PD10_Bor.DF_SPE_ACC_ID + ',' + RTRIM(PD10_Bor.DM_PRS_1) + ' ' + RTRIM(PD10_Bor.DM_PRS_LST) + ',' + email.EmailAddress [EmailData],
		1 [Priority],
		PD30.DC_DOM_ST [State]
	FROM
		PD10_PRS_NME PD10_End
		INNER JOIN LN20_EDS LN20
			ON LN20.LF_EDS = PD10_End.DF_PRS_ID
			AND LN20.LC_STA_LON20 = 'A'
			AND LN20.LC_EDS_TYP = 'M'
		INNER JOIN PD10_PRS_NME PD10_Bor
			ON PD10_Bor.DF_PRS_ID = LN20.BF_SSN
		INNER JOIN PD30_PRS_ADR PD30
			ON PD30.DF_PRS_ID = PD10_Bor.DF_PRS_ID
		INNER JOIN calc.EmailAddress email
			ON email.DF_PRS_ID = PD10_Bor.DF_PRS_ID
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
			ON DW01.BF_SSN = PD10_Bor.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (PD10_End.DF_SPE_ACC_ID, PD10_End.DF_PRS_ID)
		AND DW01.BF_SSN IS NOT NULL
END