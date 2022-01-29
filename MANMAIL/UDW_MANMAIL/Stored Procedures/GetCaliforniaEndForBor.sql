CREATE PROCEDURE [bcsretmail].[GetCaliforniaEndForBor]
	@AccountNumber varchar(10)
AS
BEGIN

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

	-- Pulls Endorser data when using the Borrower account number
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID [AccountNumber],
		PD10.DF_SPE_ACC_ID + ',' + RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) + ',' + ISNULL(LN20.DX_ADR_EML, LN20.DX_CNC_EML_ADR) [EmailData],
		1 [ArcNeeded],
		LN20.EmailPriority [EmailPriority],
		2 [Priority],
		PD30.DC_DOM_ST [State]
	FROM
		PD10_PRS_NME PD10
		INNER JOIN DW01_DW_CLC_CLU DW01
			ON PD10.DF_PRS_ID = DW01.BF_SSN
		INNER JOIN
			(
				SELECT
					LN20_End.BF_SSN,
					LN20_End.LF_EDS,
					PD10_End.DF_SPE_ACC_ID,
					PD32.DX_ADR_EML,
					PD10_End.DM_PRS_1,
					PD10_End.DM_PRS_LST,
					PD32.EmailPriority,
					PD32.DX_CNC_EML_ADR
				FROM
					LN20_EDS LN20_End
					INNER JOIN PD10_PRS_NME PD10_End
						ON LN20_End.LF_EDS = PD10_End.DF_PRS_ID
						AND LN20_End.LC_STA_LON20 = 'A'
						AND LN20_End.LC_EDS_TYP = 'M'
					INNER JOIN
						( -- email address
							SELECT
								*,
								ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) [EmailPriority] -- number in order of Email.PriorityNumber
							FROM
							(
								SELECT
									PD10.DF_PRS_ID,
									PD32.DX_ADR_EML,
									PH05.DX_CNC_EML_ADR,
									CASE 
										WHEN PD32.DC_ADR_EML = 'H' THEN 1 -- home
										WHEN PD32.DC_ADR_EML = 'A' THEN 2 -- alternate
										WHEN PD32.DC_ADR_EML = 'W' THEN 3 -- work
									END [PriorityNumber]
								FROM
									PD10_PRS_NME PD10
									LEFT JOIN PD32_PRS_ADR_EML PD32
										ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
										AND PD32.DI_VLD_ADR_EML = 'Y' -- valid email address
										AND PD32.DC_STA_PD32 = 'A' -- active email address record
									LEFT JOIN PH05_CNC_EML PH05
										ON PD10.DF_SPE_ACC_ID = PH05.DF_SPE_ID
										AND PH05.DI_VLD_CNC_EML_ADR = 'Y'
							) Email
						) PD32
					ON PD32.DF_PRS_ID = PD10_End.DF_PRS_ID
					AND PD32.EmailPriority = 1
			) LN20
			ON
			PD10.DF_PRS_ID = LN20.BF_SSN
		INNER JOIN PD30_PRS_ADR PD30
			ON LN20.LF_EDS = PD30.DF_PRS_ID
	WHERE
		@AccountNumber IN (PD10.DF_SPE_ACC_ID, PD10.DF_PRS_ID)
		--We want to filter out the non CA borrowers in the code
		--AND
		--PD30.DC_DOM_ST = 'CA'
		AND DW01.WC_DW_LON_STA NOT IN (16, 17, 18, 19, 20, 21)
		AND	(LN20.DX_ADR_EML IS NOT NULL OR LN20.DX_CNC_EML_ADR IS NOT NULL)
END
