USE CDW
GO
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
DECLARE @Begin DATE = '12/08/2017'
DECLARE @End DATE = DATEADD(DAY,120,@Begin)

IF OBJECT_ID('tempdb..#PD31') IS NOT NULL 
	DROP TABLE #PD31
IF OBJECT_ID('tempdb..#PD30') IS NOT NULL 
	DROP TABLE #PD30

SELECT
	*
INTO #PD31
FROM OPENQUERY(DUSTER,
'
SELECT
	PD10.DF_PRS_ID,
	DM_PRS_1,
	DM_PRS_LST,
	DF_ZIP_CDE_HST,
	DD_CRT_PD31
FROM
	OLWHRM1.PD31_PRS_INA PD31
	INNER JOIN OLWHRM1.PD10_PRS_NME PD10
		ON PD31.DF_PRS_ID = PD10.DF_PRS_ID
WHERE
	SUBSTR(DF_ZIP_CDE_HST,1,5) IN 
	(
		''78616'',
''78622'',
''78644'',
''78648'',
''78655'',
''78656'',
''78661'',
''77363'',
''77830'',
''77831'',
''77861'',
''77868'',
''77873'',
''77875'',
''77876'',
''77709'',
''77097'',
''77246'',
''77247'',
''77149'',
''77250'',
''77260'',
''77276'',
''77278'',
''77285'',
''77286'',
''77294'',
''77296'',
''77298'',
''77709'',
''78461'',
''78470'',
''78471'',
''78473'',
''78474'',
''78475'',
''78476'',
''78477'',
''78478''

	)
	AND SUBSTR(PD31.DF_PRS_ID,1,1) != ''P''
	AND DC_ADR_HST = ''L''
	AND DF_LST_DTS_PD31 BETWEEN ''08/25/2017'' AND CURRENT DATE
	AND DC_STA_PDEM31 = ''H''
')


SELECT 'OQ DONE'

SELECT DISTINCT
		pd10.df_prs_id as ssn,
			RTRIM(PD10.DM_PRS_1) + ' ' 	+ RTRIM(PD10.DM_PRS_LST) as name,
			PD30.DD_VER_ADR
		into #PD30
	FROM
		LN10_LON LN10
		INNER JOIN PD10_PRS_NME PD10 
			ON PD10.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN 
		(
			SELECT
				ADR.DF_PRS_ID,
				ADR.DX_STR_ADR_1,
				ADR.DX_STR_ADR_2,
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
					PD30.DF_PRS_ID,
					PD30.DX_STR_ADR_1,
					PD30.DX_STR_ADR_2,
					PD30.DM_CT,
					PD30.DC_DOM_ST,
					PD30.DF_ZIP_CDE,
					PD30.DM_FGN_CNY,
					PD30.DM_FGN_ST,
					CASE	  
 						WHEN DC_ADR = 'L' THEN 1 -- legal 
 						WHEN DC_ADR = 'B' THEN 2 -- billing
 						WHEN DC_ADR = 'D' THEN 3 -- disbursement
 					END AS PriorityNumber,
					pd30.DD_VER_ADR
				FROM
					PD30_PRS_ADR PD30
				WHERE
					PD30.DI_VLD_ADR = 'Y'
					AND	SUBSTRING(PD30.DF_ZIP_CDE, 1, 5) IN 
					(
						'78616',
'78622',
'78644',
'78648',
'78655',
'78656',
'78661',
'77363',
'77830',
'77831',
'77861',
'77868',
'77873',
'77875',
'77876',
'77709',
'77097',
'77246',
'77247',
'77149',
'77250',
'77260',
'77276',
'77278',
'77285',
'77286',
'77294',
'77296',
'77298',
'77709',
'78461',
'78470',
'78471',
'78473',
'78474',
'78475',
'78476',
'78477',
'78478'

					) 
			) ADR
		) PD30 
			ON PD30.DF_PRS_ID = LN10.BF_SSN
			AND PD30.AddressPriority = 1
		LEFT OUTER JOIN 
		(
			SELECT
				DW01.BF_SSN
			FROM
				LN10_LON LN10
				INNER JOIN DW01_DW_CLC_CLU DW01 
					ON DW01.BF_SSN = LN10.BF_SSN
					AND DW01.LN_SEQ = LN10.LN_SEQ 
			WHERE
				DW01.WC_DW_LON_STA IN ('16','17','20','21','07','08','09','10','11','12') --exclude all borrowers that have any of these statuses on any loan (BKY, death, claim)
		) DW01
			ON DW01.BF_SSN = LN10.BF_SSN
		
		LEFT JOIN LN16_LON_DLQ_HST LN16 
			ON LN16.BF_SSN = LN10.BF_SSN 
			AND	LN16.LN_DLQ_MAX > 0 
			AND LN16.LC_STA_LON16 = '1'	-- Active, delinquent
			AND CAST(LN16.LD_DLQ_MAX AS DATE) BETWEEN CAST(GETDATE() - 4 AS DATE) AND CAST(GETDATE() AS DATE)
		
		LEFT JOIN PH05_CNC_EML PH05 
			ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
			AND	PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
			AND PH05.DI_CNC_ELT_OPI = 'Y' --on ecorr
	    LEFT JOIN
		( -- email address 
			SELECT 
				DF_PRS_ID, 
				Email.EM [ALT_EM],
 				ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) [EmailPriority] -- number in order of Email.PriorityNumber 
 			FROM 
 			( 
 				SELECT 
 					PD32.DF_PRS_ID, 
 					PD32.DX_ADR_EML [EM], 
 					CASE	  
 						WHEN DC_ADR_EML = 'H' THEN 1 -- home 
 						WHEN DC_ADR_EML = 'A' THEN 2 -- alternate 
 						WHEN DC_ADR_EML = 'W' THEN 3 -- work 
 					END AS PriorityNumber
 				FROM 
 					PD32_PRS_ADR_EML PD32 
 				WHERE 
 					PD32.DI_VLD_ADR_EML = 'Y' -- valid email address 
 					AND PD32.DC_STA_PD32 = 'A' -- active email address record 

 			) Email 
		) PD32 
			ON PD32.DF_PRS_ID = PD30.DF_PRS_ID
			AND PD32.EmailPriority = 1
	WHERE 
		LN10.LC_STA_LON10 = 'R'	-- Released
		AND LN10.LA_CUR_PRI > 0.00
		AND DW01.BF_SSN IS NULL

SELECT 
	* 
FROM 
	#PD30
	
UNION ALL  

SELECT DISTINCT
	DF_PRS_ID AS SSN,
	LTRIM(RTRIM(DM_PRS_1)) + ' ' +  LTRIM(RTRIM(DM_PRS_LST)) AS NAME,
	MAX(DD_CRT_PD31) AS DD_VER_ADR
FROM
	#PD31 
	LEFT JOIN #PD30 
		ON #PD31.DF_PRS_ID = #PD30.ssn
WHERE
	#PD30.ssn IS NULL
GROUP BY
	DF_PRS_ID,
	LTRIM(RTRIM(DM_PRS_1)) + ' ' +  LTRIM(RTRIM(DM_PRS_LST))
ORDER BY
	DD_VER_ADR