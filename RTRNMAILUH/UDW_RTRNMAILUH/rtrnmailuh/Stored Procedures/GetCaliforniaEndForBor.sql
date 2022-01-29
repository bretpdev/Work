CREATE PROCEDURE [rtrnmailuh].[GetCaliforniaEndForBor]
	@AccountIdentifier varchar(10)
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	-- Pulls Endorser data when using the Borrower account number
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID [AccountNumber],
		LN20.BF_SSN [BorSsn],
		LN20.LF_EDS [EndSsn],
		PD10.DF_SPE_ACC_ID + ',' + RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) + ',' + LN20.EmailAddress [EmailData],
		2 [Priority],
		PD30.DC_DOM_ST [State]
	FROM
		PD10_PRS_NME PD10
		INNER JOIN
		(
			SELECT
				LN20_End.BF_SSN,
				LN20_End.LF_EDS,
				PD10_End.DF_SPE_ACC_ID,
				email.EmailAddress,
				PD10_End.DM_PRS_1,
				PD10_End.DM_PRS_LST
			FROM
				LN20_EDS LN20_End
				INNER JOIN PD10_PRS_NME PD10_End
					ON PD10_End.DF_PRS_ID = LN20_End.LF_EDS
					AND LN20_End.LC_STA_LON20 = 'A'
					AND LN20_End.LC_EDS_TYP = 'M'
				INNER JOIN calc.EmailAddress email
					ON email.DF_PRS_ID = PD10_End.DF_PRS_ID
		) LN20
			ON LN20.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN PD30_PRS_ADR PD30
			ON PD30.DF_PRS_ID = LN20.LF_EDS
		LEFT JOIN
		(
			SELECT
				BF_SSN
			FROM
				DW01_DW_CLC_CLU DW01
				INNER JOIN PD10_PRS_NME PD10
					ON PD10.DF_PRS_ID = BF_SSN
			WHERE
				WC_DW_LON_STA IN (02, 03, 04, 05, 06, 07, 12, 14, 22) --Excludes death, disability
		) DW01
			ON DW01.BF_SSN = PD10.DF_PRS_ID
	WHERE
		@AccountIdentifier IN (PD10.DF_SPE_ACC_ID, PD10.DF_PRS_ID)
		AND DW01.BF_SSN IS NOT NULL
END