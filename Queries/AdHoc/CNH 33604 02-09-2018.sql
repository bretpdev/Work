USE CDW
GO
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED


IF OBJECT_ID('tempdb..#PDXX') IS NOT NULL 
	DROP TABLE #PDXX
IF OBJECT_ID('tempdb..#PDXX') IS NOT NULL 
	DROP TABLE #PDXX
IF OBJECT_ID('tempdb..#ALL_POP') IS NOT NULL 
	DROP TABLE #ALL_POP
IF OBJECT_ID('tempdb..#FINAL_POP') IS NOT NULL 
	DROP TABLE #FINAL_POP

SELECT
	*
INTO #PDXX
FROM OPENQUERY(LEGEND,
'
SELECT
	PDXX.DF_PRS_ID,
	DM_PRS_X,
	DM_PRS_LST,
	DF_ZIP_CDE_HST,
	DD_CRT_PDXX
FROM
	PKUB.PDXX_PRS_INA PDXX
	INNER JOIN PKUB.PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
WHERE
	SUBSTR(DF_ZIP_CDE_HST,X,X) IN 
	(
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX'',
		''XXXXX''


	)
	AND SUBSTR(PDXX.DF_PRS_ID,X,X) != ''P''
	AND DC_ADR_HST = ''L''
	AND DF_LST_DTS_PDXX BETWEEN ''XX/XX/XXXX'' AND CURRENT DATE
	AND DC_STA_PDEMXX = ''H''
')


SELECT 'OQ DONE'

SELECT DISTINCT
		pdXX.df_prs_id as ssn,
			RTRIM(PDXX.DM_PRS_X) + ' ' 	+ RTRIM(PDXX.DM_PRS_LST) as name,
			PDXX.DD_VER_ADR
		into #PDXX
	FROM
		LNXX_LON LNXX
		INNER JOIN PDXX_PRS_NME PDXX 
			ON PDXX.DF_PRS_ID = LNXX.BF_SSN
		INNER JOIN 
		(
			SELECT
				ADR.DF_PRS_ID,
				ADR.DX_STR_ADR_X,
				ADR.DX_STR_ADR_X,
				ADR.DM_CT,
				ADR.DC_DOM_ST,
				ADR.DF_ZIP_CDE,
				ADR.DM_FGN_CNY,
				ADR.DM_FGN_ST,
				adr.DD_VER_ADR,
				ROW_NUMBER() OVER (PARTITION BY ADR.DF_PRS_ID ORDER BY ADR.PriorityNumber) [AddressPriority] -- number in order of Address
			FROM
			(
				SELECT
					PDXX.DF_PRS_ID,
					PDXX.DX_STR_ADR_X,
					PDXX.DX_STR_ADR_X,
					PDXX.DM_CT,
					PDXX.DC_DOM_ST,
					PDXX.DF_ZIP_CDE,
					PDXX.DM_FGN_CNY,
					PDXX.DM_FGN_ST,
					CASE	  
 						WHEN DC_ADR = 'L' THEN X -- legal 
 						WHEN DC_ADR = 'B' THEN X -- billing
 						WHEN DC_ADR = 'D' THEN X -- disbursement
 					END AS PriorityNumber,
					pdXX.DD_VER_ADR
				FROM
					PDXX_PRS_ADR PDXX
				WHERE
					PDXX.DI_VLD_ADR = 'Y'
					AND	SUBSTRING(PDXX.DF_ZIP_CDE, X, X) IN 
					(
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX',
						'XXXXX'


					) 
			) ADR
		) PDXX 
			ON PDXX.DF_PRS_ID = LNXX.BF_SSN
			AND PDXX.AddressPriority = X
		LEFT OUTER JOIN 
		(
			SELECT
				DWXX.BF_SSN
			FROM
				LNXX_LON LNXX
				INNER JOIN DWXX_DW_CLC_CLU DWXX 
					ON DWXX.BF_SSN = LNXX.BF_SSN
					AND DWXX.LN_SEQ = LNXX.LN_SEQ 
			WHERE
				DWXX.WC_DW_LON_STA IN ('XX','XX','XX','XX','XX','XX','XX','XX','XX','XX') --exclude all borrowers that have any of these statuses on any loan (BKY, death, claim)
		) DWXX
			ON DWXX.BF_SSN = LNXX.BF_SSN
		
		LEFT JOIN LNXX_LON_DLQ_HST LNXX 
			ON LNXX.BF_SSN = LNXX.BF_SSN 
			AND	LNXX.LN_DLQ_MAX > X 
			AND LNXX.LC_STA_LONXX = 'X'	-- Active, delinquent
			AND CAST(LNXX.LD_DLQ_MAX AS DATE) BETWEEN CAST(GETDATE() - X AS DATE) AND CAST(GETDATE() AS DATE)
		
		LEFT JOIN PHXX_CNC_EML PHXX 
			ON PHXX.DF_SPE_ID = PDXX.DF_SPE_ACC_ID
			AND	PHXX.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
			AND PHXX.DI_CNC_ELT_OPI = 'Y' --on ecorr
	    LEFT JOIN
		( -- email address 
			SELECT 
				DF_PRS_ID, 
				Email.EM [ALT_EM],
 				ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) [EmailPriority] -- number in order of Email.PriorityNumber 
 			FROM 
 			( 
 				SELECT 
 					PDXX.DF_PRS_ID, 
 					PDXX.DX_ADR_EML [EM], 
 					CASE	  
 						WHEN DC_ADR_EML = 'H' THEN X -- home 
 						WHEN DC_ADR_EML = 'A' THEN X -- alternate 
 						WHEN DC_ADR_EML = 'W' THEN X -- work 
 					END AS PriorityNumber
 				FROM 
 					PDXX_PRS_ADR_EML PDXX 
 				WHERE 
 					PDXX.DI_VLD_ADR_EML = 'Y' -- valid email address 
 					AND PDXX.DC_STA_PDXX = 'A' -- active email address record 

 			) Email 
		) PDXX 
			ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
			AND PDXX.EmailPriority = X
	WHERE 
		LNXX.LC_STA_LONXX = 'R'	-- Released
		AND LNXX.LA_CUR_PRI > X.XX
		AND DWXX.BF_SSN IS NULL


SELECT
	P.*
INTO #ALL_POP
FROM
(
	SELECT 
		* 
	FROM 
		#PDXX
	
	UNION ALL  

	SELECT
		DF_PRS_ID AS SSN,
		LTRIM(RTRIM(DM_PRS_X)) + ' ' +  LTRIM(RTRIM(DM_PRS_LST)) AS NAME,
		DD_CRT_PDXX AS DD_VER_ADR
	FROM
		#PDXX 
		LEFT JOIN #PDXX 
			ON #PDXX.DF_PRS_ID = #PDXX.ssn
	WHERE
		#PDXX.ssn IS NULL
) P

SELECT * FROM #ALL_POP --inital pop


DECLARE @Begin DATE = 'XX/XX/XXXX'
DECLARE @End DATE = DATEADD(DAY,XXX,@Begin)

SELECT DISTINCT
	AP.*,
	CASE 
		WHEN LNXX.LC_STA_LONXX = 'X' THEN 'CURRENTLY PAST DUE'
		WHEN LNXX.LC_STA_LONXX = 'X' THEN 'WAS PAST DUE'
		ELSE NULL
	END AS ACT_STATUS,
	CASE WHEN DWXX.WX_OVR_DW_LON_STA = 'DELINQUENCY TRANSFER' THEN 'YES' ELSE 'NO' END AS DEFAULTED,
	CONVERT(VARCHAR(XX) ,LNXX.LD_DLQ_OCC, XXX) AS DATE_BWR_WENT_PAST_DUE,
	LNXX.LN_DLQ_MAX AS NUMBER_OF_DAYS_BWR_WENT_PAST_DUE,
	CASE 
		WHEN LNXX.LC_STA_LONXX = 'X' THEN CONVERT(VARCHAR(XX), LNXX.LF_LST_DTS_LNXX, XXX)
		ELSE NULL
	END AS DATE_DELQ_CLEARED,
	CASE WHEN DIA_FORB.BF_SSN IS NOT NULL THEN 'YES' ELSE NULL END AS HAD_DISASTER_FORB,
	CASE WHEN DEF.BF_SSN IS NOT NULL THEN 'YES' ELSE NULL END AS HAD_DEFERMENT,
	CASE WHEN ZERO_PAYMENT.BF_SSN IS NOT NULL THEN 'YES' WHEN RS.BF_SSN IS NOT NULL THEN 'YES' ELSE NULL END AS HAD_ZERO_IDR,
	CASE WHEN DWXX_X.BF_SSN IS NOT NULL THEN 'YES' ELSE NULL END AS DEATH_TPD
INTO #FINAL_POP
FROM 
	#ALL_POP AP
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = AP.ssn
	LEFT JOIN CDW..DWXX_DW_CLC_CLU DWXX
		ON DWXX.BF_SSN = LNXX.BF_SSN
		and lnXX.ln_seq = dwXX.ln_seq
	LEFT JOIN
	(
		SELECT DISTINCT
			FBXX.BF_SSN
		FROM
			CDW..FBXX_BR_FOR_REQ FBXX
		INNER JOIN CDW..LNXX_BR_FOR_APV LNXX
			ON LNXX.BF_SSN = FBXX.BF_SSN
			AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND FBXX.LC_FOR_TYP = 'XX'
			AND 
			(
				LNXX.LD_FOR_BEG BETWEEN @Begin AND @End
				OR
				LNXX.LD_FOR_END BETWEEN @Begin AND @End
			)
	) DIA_FORB
		ON DIA_FORB.BF_SSN = LNXX.BF_SSN
	LEFT JOIN
	(
		SELECT	DISTINCT
			BF_SSN
		FROM
			CDW..LNXX_BR_DFR_APV LNXX
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND 
			(
				LNXX.LD_DFR_BEG BETWEEN @Begin AND @End
				OR
				LNXX.LD_DFR_END BETWEEN @Begin AND @End
			)
	) DEF
		ON DEF.BF_SSN = LNXX.BF_SSN
	LEFT JOIN 
	(
		SELECT DISTINCT
			BF_SSN,
			LC_STA_LONXX,
			MAX(LD_DLQ_OCC) OVER(PARTITION BY BF_SSN,LC_STA_LONXX) AS LD_DLQ_OCC,
			MAX(LN_DLQ_MAX) OVER(PARTITION BY BF_SSN,LC_STA_LONXX) AS LN_DLQ_MAX,
			MAX(LF_LST_DTS_LNXX) OVER(PARTITION BY BF_SSN,LC_STA_LONXX) AS LF_LST_DTS_LNXX
		FROM
			CDW..LNXX_LON_DLQ_HST LNXX
		WHERE
			(
			LNXX.LD_DLQ_OCC BETWEEN @BEGIN AND @END 
			OR
			LNXX.LD_STA_LONXX BETWEEN @BEGIN AND @END
			)
		) LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LN_DLQ_MAX > X
	LEFT JOIN
	(
		SELECT	DISTINCT
			LNXX.BF_SSN,
			SUM(LA_RPS_ISL) AS LA_RPS_ISL
		FROM
			CDW..RSXX_BR_RPD RSXX
			INNER JOIN CDW..LNXX_LON_RPS LNXX
				ON RSXX.BF_SSN = LNXX.BF_SSN
				AND RSXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
			INNER JOIN CDW..LNXX_LON_RPS_SPF LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
				AND LNXX.LN_GRD_RPS_SEQ = X
		WHERE 
			(
				RSXX.LD_RPS_X_PAY_DU BETWEEN @BEGIN AND @END 
				OR
				RSXX.LF_LST_DTS_RSXX BETWEEN @BEGIN AND @END 
			)
		GROUP BY
			LNXX.BF_SSN
		HAVING 
			SUM(LA_RPS_ISL) = X
	) ZERO_PAYMENT
		ON ZERO_PAYMENT.BF_SSN = LNXX.BF_SSN
	LEFT JOIN
	(
		SELECT	DISTINCT
			BF_SSN
		FROM
			CDW.calc.RepaymentSchedules RS
		WHERE
			RS.CurrentGradation = X
			AND RS.LA_RPS_ISL = X
			AND 
			(
				@Begin BETWEEN DATEADD(YEAR, -X, TermStartDate) and TermStartDate
				OR 
				@Begin >= TermStartDate
				OR
				@End BETWEEN TermStartDate AND DATEADD(YEAR, X, TermStartDate)
			)
	) RS
		ON RS.BF_SSN = LNXX.BF_SSN
	LEFT JOIN 
	(
		SELECT DISTINCT
			BF_SSN
		FROM
			CDW..DWXX_DW_CLC_CLU
		WHERE
			WC_DW_LON_STA IN ('XX','XX')
	) DWXX_X
		on DWXX_X.BF_SSN = LNXX.BF_SSN
	WHERE
		LNXX.BF_SSN IS NOT NULL OR CASE WHEN DWXX.WX_OVR_DW_LON_STA = 'DELINQUENCY TRANSFER' THEN 'DEFAULTED' ELSE 'NO' END  = 'YES'
ORDER BY
	AP.SSN
			


SELECT * FROM #FINAL_POP

SELECT DISTINCT SSN, name FROM #FINAL_POP WHERE HAD_DEFERMENT IS NULL AND HAD_DISASTER_FORB IS NULL AND HAD_ZERO_IDR IS NULL AND DEATH_TPD IS NULL