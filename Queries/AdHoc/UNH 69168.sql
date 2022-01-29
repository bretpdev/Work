USE UDW
GO

--SELECT * FROM tempdb.INFORMATION_SCHEMA.TABLES WHERE  TABLE_NAME LIKE '#LP10%'
IF NOT EXISTS (SELECT * FROM tempdb.INFORMATION_SCHEMA.TABLES WHERE  TABLE_NAME LIKE '#LP10%') 
SELECT * INTO #LP10 FROM OPENQUERY(DUSTER,'SELECT * FROM OLWHRM1.LP10_RPY_PAR')



SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT DISTINCT 
	PD10.DF_SPE_ACC_ID AS [ACCOUNT NUMBER],
	LN10.LN_SEQ AS [Loan Number],
	LTRIM(RTRIM(PD10S.DM_PRS_LST)) + ', ' + LTRIM(RTRIM(PD10S.DM_PRS_1)) AS [Student Name],
	PD30S.DX_STR_ADR_1 AS [Student Street Address 1],
	PD30S.DX_STR_ADR_2 AS [Student Street Address 2],
	PD30S.DM_CT AS [Student City Name],
	PD30S.DC_DOM_ST AS [Student State Code],
	PD10.DM_PRS_1 AS [Parent First Name],
	PD10.DM_PRS_LST AS [Parent Last Name],
	CASE WHEN PD30BL.DX_STR_ADR_1 IS NOT NULL THEN PD30BL.DX_STR_ADR_1 ELSE PD30B.DX_STR_ADR_1 END AS [Billing Street 1],
	CASE WHEN PD30BL.DX_STR_ADR_2 IS NOT NULL THEN PD30BL.DX_STR_ADR_2 ELSE PD30B.DX_STR_ADR_2 END AS [Billing Street 2],
	CASE WHEN PD30BL.DM_CT IS NOT NULL THEN PD30BL.DM_CT ELSE PD30B.DM_CT END AS [Billing City],
	CASE WHEN PD30BL.DC_DOM_ST IS NOT NULL THEN PD30BL.DC_DOM_ST ELSE PD30B.DC_DOM_ST END AS [Billing State],
	LN10.LF_LON_CUR_OWN AS [Loan Holder’s Name],
	GR10.WD_BR_SIG_MPN AS [Note Date],
	LN10.LD_LON_1_DSB AS [Initial Disbursement Date],
	LN10.LD_LON_EFF_ADD AS [Date Servicing Acquired],
	'NO' AS [Was loan in default when servicing acquired],

	 CASE WHEN 
		BAL.BALANCE IS NOT NULL THEN BAL.BALANCE
		ELSE LN15.BALANCE
	 END AS [Original Servicing Principal Balance],  --for bana borrower we will need to sum the payments on when they transfered on.  For non bana I think we could use dsb info

	LN10.LA_CUR_PRI AS [Current Principal Balance],
	LP10.PN_RPD_TRM_MAX AS [Original Loan Term], 
	'FFELP' AS [Loan Program],
	CASE 
		WHEN LN10.IC_LON_PGM IN ('CNSLDN','PLUS','PLUSGB','SLS','UNCNS','UNSPC','UNSTFD') THEN 'Un-subsidized'
		ELSE 'Subsidized'
	END AS [Federal Aid Status (Subsidized, Un-subsidized, n/a)],
	CASE WHEN LN72.LC_ITR_TYP IN ('F1','F2') THEN 'Fixed' ELSE 'Variable' END AS [Rate Type (Variable/Fixed)],

	CASE 
		WHEN 
			LN10.IC_LON_PGM IN ('CNSLDN','UNCNS','SUBCNS','UNSPC','SUBSPC') THEN LN10.LR_WIR_CON_LON
		WHEN 
			LP06.IC_LON_PGM IS NOT NULL THEN LP06.PR_ITR_MIN
			ELSE NULL
		END AS [Initial Interest Rate on Note],  --todo get statitory rate for when the first loan was disb

	LN72.LR_ITR AS [Current Interest Rate],
	RS.LA_RPS_ISL AS [Current Periodic Payment],
	CASE WHEN RS.LA_RPS_ISL IS NOT NULL THEN 'Monthly' END AS [Payment Frequency (monthly, quarterly, etc.)],
	LP.LAST_PAYMENT AS [Date Last Payment Made],
	LK10.PX_DSC_MDM AS [Account Status],

	cp.LAST_PAYMENT AS [Date of Default], --date cliam paid
	ISNULL(LN16.DELQ_COUNT,0) AS [Number of Payments Made 30 or More Days Late], 

	isnull(LK10RS.PX_DSC_MDM, rs.LC_TYP_SCH_DIS) AS [Current Repayment Plan (i.e. Level, Graduated, PAYE, IBR, ICR, etc.)],
	CASE WHEN DW01.WC_DW_LON_STA = '04' THEN 'Y' ELSE 'N' END AS [In Deferment],
	CASE WHEN DW01.WC_DW_LON_STA = '05' THEN 'Y' ELSE 'N' END AS [In Forbearance],

	ISNULL(FORBS.ActiveForbCount,0) AS [Number of qualified hardship forbearances received],  --todo ocunt number of active forbearances

	'NO' AS [Has loan been assigned to collections, or a collections servicer?],

	CASE WHEN WRT.BF_SSN IS NOT NULL THEN 'YES' ELSE 'NO' END AS [Has loan been forgiven, discharged, or cancelled?],

	IIF(SCRA.BorrSSN IS NOT NULL, 'YES', 'NO') AS [Active duty military borrower?],
	SCRA.BeginBrwr AS [Date military borrower entered active duty status] --use the table you created for the other scra process or if it is not there anymore ask jessica what arc's we used to use

	--LN10.BF_SSN,
	--ln10.LF_STU_SSN,
	--ln10.IC_LON_PGM
	--246805
	--243736
FROM
	AuditUDW..LN10_LON_Sep2020 LN10
	INNER JOIN AuditUDW..DW01_DW_CLC_CLU_Sep2020 DW01
		ON DW01.BF_SSN = LN10.BF_SSN
		AND DW01.LN_SEQ = LN10.LN_SEQ
	INNER JOIN UDW..LK10_LS_CDE_LKP LK10
		ON LK10.PM_ATR = 'WC-DW-LON-STA'
		AND LK10.PX_ATR_VAL = DW01.WC_DW_LON_STA
	INNER JOIN
	(
		SELECT
			DF_PRS_ID
		FROM
			UDW..PD30_PRS_ADR
		WHERE
			DC_DOM_ST = 'WA'
			AND DI_VLD_ADR = 'Y'
		UNION

		SELECT
			DF_PRS_ID
		FROM
			UDW..PD31_PRS_INA PD31
		WHERE
			PD31.DC_DOM_ST_HST = 'WA'
			AND PD31.DI_VLD_ADR_HST = 'Y'
			AND PD31.DD_CRT_PD31 > '04/23/2019'
	) PD30WA
		ON PD30WA.DF_PRS_ID = LN10.BF_SSN
	left JOIN UDW..GR10_RPT_LON_APL GR10
		ON GR10.BF_SSN = LN10.BF_SSN
		AND GR10.LN_SEQ = LN10.LN_SEQ
	left JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = LN10.BF_SSN
	left JOIN UDW..PD10_PRS_NME PD10S
		ON PD10S.DF_PRS_ID = LN10.LF_STU_SSN
	left JOIN UDW..PD30_PRS_ADR PD30S
		ON PD30S.DF_PRS_ID = LN10.LF_STU_SSN
		AND PD30S.DC_ADR = 'L'
	left JOIN UDW..PD30_PRS_ADR PD30B
		ON PD30B.DF_PRS_ID = LN10.BF_SSN
		AND PD30B.DC_ADR = 'L' 
	left JOIN UDW..PD30_PRS_ADR PD30BL
		ON PD30BL.DF_PRS_ID = LN10.BF_SSN
		AND PD30BL.DC_ADR = 'B' 
	LEFT JOIN 
	(
		SELECT 
			LN72.BF_SSN, 
			LN72.LN_SEQ,
			LN72.LR_ITR,
			LN72.LC_ITR_TYP,
			ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ, LN72.LD_ITR_EFF_BEG, LN72.LD_ITR_EFF_END ORDER BY LD_STA_LON72 DESC) AS SEQ
		FROM
			UDW..LN72_INT_RTE_HST LN72
		WHERE
			LN72.LC_STA_LON72 = 'A'
			AND	CAST(GETDATE() AS DATE) BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END

	) LN72 
		ON LN10.BF_SSN = LN72.BF_SSN
		AND LN10.LN_SEQ = LN72.LN_SEQ
		AND LN72.SEQ = 1
	LEFT JOIN UDW.calc.RepaymentSchedules RS
		ON RS.BF_SSN = LN10.BF_SSN
		AND RS.LN_SEQ = LN10.LN_SEQ
		AND RS.CurrentGradation = 1
	LEFT JOIN
	(
		SELECT
			BF_SSN,
			LN_SEQ,
			MAX(LD_FAT_EFF) AS LAST_PAYMENT
		FROM
			UDW..LN90_FIN_ATY LN90
		WHERE
			LN90.PC_FAT_TYP = '10'
			AND LN90.PC_FAT_SUB_TYP = '10'
			AND LN90.LC_STA_LON90 = 'A'
			AND 
			(
				LN90.LC_FAT_REV_REA = '' 
				OR LN90.LC_FAT_REV_REA IS NULL
			)
		GROUP BY
			BF_SSN,
			LN_SEQ	
	) LP
		ON LP.BF_SSN = LN10.BF_SSN
		AND LP.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN UDW..LK10_LS_CDE_LKP LK10RS
		ON LK10RS.PM_ATR = 'LC-TYP-SCH-DIS'
		AND LK10RS.PX_ATR_VAL = RS.LC_TYP_SCH_DIS
	LEFT JOIN
	(--find active duty military borrowers
		SELECT
			BorrSSN,
			BeginBrwr
		FROM 
			ULS.scra.DataComparison 
		WHERE
			ActiveRow = 1 
			AND (
					BorrActive = 1 
					OR EndrActive = 1
				)
	) SCRA
		ON SCRA.BorrSSN = LN10.BF_SSN
	LEFT JOIN
	(--active forbearances
		SELECT DISTINCT
			LN60.BF_SSN,
			LN60.LN_SEQ,
			COUNT(*) AS ActiveForbCount
		FROM	
			LN60_BR_FOR_APV LN60
			INNER JOIN FB10_BR_FOR_REQ FB10
				ON FB10.BF_SSN = LN60.BF_SSN
				AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
		WHERE	
			LN60.LC_STA_LON60 = 'A'
			AND LN60.LC_FOR_RSP != '003' --not denied
			AND FB10.LC_STA_FOR10 = 'A'
			AND FB10.LC_FOR_STA = 'A' --approved
			AND LN60.LD_FOR_BEG BETWEEN '04/23/2019' AND '09/30/2020'
		GROUP BY
			LN60.BF_SSN,
			LN60.LN_SEQ
	) FORBS
		ON FORBS.BF_SSN = LN10.BF_SSN
		AND FORBS.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT * FROM #LP10 WHERE PC_STA_LPD10 = 'A'
		AND GETDATE() BETWEEN PD_EFF_SR_LPD10 AND PD_EFF_END_LPD10
	) LP10
		ON LP10.IC_LON_PGM = LN10.IC_LON_PGM
		AND LP10.PF_RGL_CAT = LN10.LF_RGL_CAT_LP10
	LEFT JOIN 
	(
		SELECT	
			BF_SSN,
			LN_SEQ,
			COUNT(*) AS DELQ_COUNT
		FROM
			UDW..LN16_LON_DLQ_HST
		WHERE
			LN_DLQ_MAX >= 30
			AND LD_DLQ_OCC BETWEEN '04/23/2019' AND '09/30/2020'
		GROUP BY
			BF_SSN,
				LN_SEQ
	) LN16
		on LN16.BF_SSN = LN10.BF_SSN
		AND LN16.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT
			BF_SSN,
			LN_SEQ,
			sum(la_fat_cur_pri) AS BALANCE
		FROM
			UDW..LN90_FIN_ATY LN90
		WHERE
			LN90.PC_FAT_TYP = '02'
			AND LN90.PC_FAT_SUB_TYP = '91'
			AND LN90.LC_STA_LON90 = 'A'
			AND 
			(
				LN90.LC_FAT_REV_REA = '' 
				OR LN90.LC_FAT_REV_REA IS NULL
			)
		GROUP BY
			BF_SSN,
			LN_SEQ	
	) BAL
		ON BAL.BF_SSN = LN10.BF_SSN
		AND BAL.LN_SEQ = LN10.LN_SEQ	
	LEFT JOIN
	(
		SELECT 
			BF_SSN,
			LN_SEQ,
			SUM((LA_DSB - isnull(LA_DSB_CAN,0))) AS BALANCE
		FROM
			UDW..LN15_DSB
		WHERE
			LC_STA_LON15 = '1'
		GROUP BY
			BF_SSN,
			LN_SEQ
	) LN15
		ON LN15.BF_SSN = LN10.BF_SSN
		AND LN15.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT
			BF_SSN,
			LN_SEQ,
			MAX(LD_FAT_EFF) AS LAST_PAYMENT
		FROM
			UDW..LN90_FIN_ATY LN90
		WHERE
			LN90.PC_FAT_TYP = '10'
			AND LN90.PC_FAT_SUB_TYP = '30'
			AND LN90.LC_STA_LON90 = 'A'
			AND 
			(
				LN90.LC_FAT_REV_REA = '' 
				OR LN90.LC_FAT_REV_REA IS NULL
			)
		GROUP BY
			BF_SSN,
			LN_SEQ	
	) CP
		ON CP.BF_SSN = LN10.BF_SSN
		AND CP.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT
			BF_SSN,
			LN_SEQ,
			MAX(LD_FAT_EFF) AS LAST_PAYMENT
		FROM
			UDW..LN90_FIN_ATY LN90
		WHERE
			LN90.PC_FAT_TYP = '50'
			--AND LN90.PC_FAT_SUB_TYP = '30'
			AND LN90.LC_STA_LON90 = 'A'
			AND 
			(
				LN90.LC_FAT_REV_REA = '' 
				OR LN90.LC_FAT_REV_REA IS NULL
			)
		GROUP BY
			BF_SSN,
			LN_SEQ	
	) WRT
		ON WRT.BF_SSN = LN10.BF_SSN
		AND CP.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN LN35_LON_OWN LN35
		ON LN35.BF_SSN = LN10.BF_SSN
		AND LN35.LN_SEQ = LN10.LN_SEQ
		AND LN35.LD_OWN_EFF_END IS NULL -- Active owner
	LEFT JOIN LP06_ITR_AND_TYP LP06
		ON LN10.IC_LON_PGM = LP06.IC_LON_PGM
		AND LN10.LF_RGL_CAT_LP06 = LP06.PF_RGL_CAT
		AND LN10.IF_GTR = LP06.IF_GTR
		AND LN10.LF_LON_CUR_OWN = LP06.IF_OWN
		AND LN35.IF_BND_ISS = LP06.IF_BND_ISS
		AND LN10.LD_LON_1_DSB BETWEEN LP06.PD_EFF_SR_LPD06 AND LP06.PD_EFF_END_LPD06 --use target month instead of today's date
		AND LN72.LC_ITR_TYP = LP06.PC_ITR_TYP
		AND LP06.PC_STA_LPD06 = 'A'
WHERE
	LN10.IC_LON_PGM != 'TILP'
	AND LN10.LF_LST_DTS_LN10 >= '04/23/2019'

