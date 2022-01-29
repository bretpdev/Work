USE CDW
GO

SELECT DISTINCT TOP XXXXXX
	PDXX.DF_SPE_ACC_ID,
	LTRIM(RTRIM(PDXX.DM_PRS_X)) AS DM_PRS_X,
	LTRIM(RTRIM(PDXX.DM_PRS_LST)) AS DM_PRS_LST,
	PDXX.DX_ADR_EML
FROM 
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN 
	( -- email address
		SELECT
			*,
			ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) [EmailPriority] -- number in order of Email.PriorityNumber
		FROM
		(
			SELECT
				PDXX.DF_PRS_ID,
				PDXX.DX_ADR_EML,
				CASE 
					WHEN DC_ADR_EML = 'H' THEN X -- home
					WHEN DC_ADR_EML = 'A' THEN X -- alternate
					WHEN DC_ADR_EML = 'W' THEN X -- work
				END [PriorityNumber]
			FROM
				CDW..PDXX_PRS_ADR_EML PDXX
			WHERE
				PDXX.DI_VLD_ADR_EML = 'Y' -- valid email address
				AND PDXX.DC_STA_PDXX = 'A' -- active email address record
		) Email
	) PDXX 
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
		AND PDXX.EmailPriority = X --highest priority email only
	LEFT JOIN 
	(
		SELECT	
			BF_SSN
		FROM		
			CDW..DWXX_DW_CLC_CLU DWXX
		WHERE 
			DWXX.WC_DW_LON_STA IN ('XX', 'XX', 'XX','XX', 'XX', 'XX', 'XX')
	) DWXX
		ON DWXX.BF_SSN = LNXX.BF_SSN
	LEFT JOIN
	(
		SELECT	
			AccountNumber
		FROM
			CLS.emailbtcf.CampaignData
		WHERE
			EmailCampaignId = XX

	) EMAIL_SENT
		on EMAIL_SENT.AccountNumber = pdXX.DF_SPE_ACC_ID
WHERE
	LNXX.LC_STA_LONXX = 'R'
	AND LNXX.LA_CUR_PRI > X.XX
	AND DWXX.BF_SSN IS NULL
	AND EMAIL_SENT.AccountNumber IS NULL