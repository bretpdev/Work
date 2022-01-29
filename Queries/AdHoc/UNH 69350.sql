USE UDW;
GO

SELECT DISTINCT
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	PD10.DM_PRS_1,
	--PD10.DM_PRS_MID,
	PD10.DM_PRS_LST,
	--PD10.DM_PRS_LST_SFX,
	CONCAT
		(
			LTRIM(RTRIM(PD10.DM_PRS_1)),   ' ',
			--LTRIM(RTRIM(PD10.DM_PRS_MID)), ' ',
			LTRIM(RTRIM(PD10.DM_PRS_LST))--, ' ',
			--LTRIM(RTRIM(PD10.DM_PRS_LST_SFX))
		) AS BorrowerFirstAndLastName,
	EM.EmailAddress
	--VALIDATION FIELDS:
	--,LN10.BF_SSN
	--,LN10.LN_SEQ
	--,LN10.LC_STA_LON10
	--,LN10.LA_CUR_PRI
	--,DW01.WA_TOT_BRI_OTS
	--,ISNULL(LN10.LA_CUR_PRI,0) + ISNULL(DW01.WA_TOT_BRI_OTS,0) AS OutstandingBalance
FROM
	LN10_LON LN10
	INNER JOIN PD10_PRS_NME PD10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN DW01_DW_CLC_CLU DW01
		ON LN10.BF_SSN = DW01.BF_SSN
		AND LN10.LN_SEQ = DW01.LN_SEQ
	INNER JOIN
	(--get all valid emails (see UDWREFSH2 SASR_4768 for soon to be promoted job)
	--note: this section should all be OK
		SELECT DISTINCT
			EmailOrdering.DF_PRS_ID,
			EmailOrdering.EmailAddress
		FROM
			(--orders emails according to priority
				SELECT
					EmailPrioritizing.DF_PRS_ID,
					EmailPrioritizing.EmailAddress,
					ROW_NUMBER() OVER (PARTITION BY EmailPrioritizing.DF_PRS_ID ORDER BY EmailPrioritizing.PriorityNumber) AS EmailPriority --number in order of Email.PriorityNumber
				FROM
					(--assign priority to emails by type
						SELECT DISTINCT
							PD10.DF_PRS_ID,
							ISNULL(PH05.DX_CNC_EML_ADR, PD32.DX_ADR_EML) AS EmailAddress,
							CASE 
								WHEN PH05.DX_CNC_EML_ADR IS NOT NULL THEN 1 --PH05 takes highest priority
								WHEN PD32.DC_ADR_EML = 'H' THEN 2 --home
								WHEN PD32.DC_ADR_EML = 'A' THEN 3 --alternate
								WHEN PD32.DC_ADR_EML = 'W' THEN 4 --work
							END AS PriorityNumber
						FROM
							PD10_PRS_NME PD10
							LEFT JOIN PH05_CNC_EML PH05
								ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
								AND PH05.DI_VLD_CNC_EML_ADR = 'Y' --valid email address
							LEFT JOIN PD32_PRS_ADR_EML PD32
								ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
								AND PD32.DI_VLD_ADR_EML = 'Y' --valid email address
								AND PD32.DC_STA_PD32 = 'A' --active email address record
						WHERE
							PD10.DF_PRS_ID LIKE '[0-9]%' --to exclude acct#'s beginning with P
					) EmailPrioritizing
				WHERE
					EmailPrioritizing.EmailAddress IS NOT NULL --excludes borrowers without email address
			) EmailOrdering
		WHERE
			EmailOrdering.EmailPriority = 1 --highest priority email only
	) EM
		ON EM.DF_PRS_ID = LN10.BF_SSN
	LEFT JOIN UDW..PD21_GTR_DTH PD21
		ON PD10.DF_PRS_ID = PD21.DF_PRS_ID
		AND PD21.DC_DTH_STA = '02' --verified death
WHERE
	PD21.DF_PRS_ID IS NULL --exclude verified death
	AND LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 = 'R'
	AND PD10.DF_PRS_ID LIKE '[0-9]%'
	AND DW01.WC_DW_LON_STA NOT IN 
		(
			'17', --Death Verified
			'19', --Disability Verified
			'21'  --Bankruptcy Verified
		)
;
