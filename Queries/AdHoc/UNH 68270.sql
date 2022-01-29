--HEP Disqualification =  loans disbursed before May 1, 2006 and became 15-30 days past due on a 4th bill. (They satisifed the bill more than 15 days after the due date)

--Please identify the following populations:
--1) Closed loans that have erroneously achieved the reduced interest rate
		--POP 1:
		--LA_CUR_PR = '0'
		--LN55.LC_LON_BBT_STA = 'R' 
		--UTLWO39 Billing logic

--2) Open loans that have erroneously achieved the reduced interest rate
		--POP 2 = 
		--LA_CUR_PR > '0'
		--LN55.LC_LON_BBT_STA = 'R' 
		--UTLWO39 Billing logic

--3) Open loans working towards the benefit that should have already been disqualified
		--POP 3 = 
		--LA_CUR_PR > '0'
		--LN55.LC_LON_BBT_STA = 'Q' 
		--UTLWO39 Billing logic

--Output for each population to include:  
--Account
--Loan
--Principal
--RateType (Fixed/Variable)
--Base Rate
--Charged Rate
--BBP (Borrower Benefit Plan)
--BBP Counter
--Disqual Date (Date they should have been disqualified)
--Qualified Date (Date they achieved the benefit)

USE UDW
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DROP TABLE IF EXISTS #POP1, #POP2, #POP3, #RoughEstimates, #Part1, #POP3Estimates, #Part2, #POP2Estimates, #Part3;

/********************************* POP 1 **********************************/
--1) Closed loans that have erroneously achieved the reduced interest rate
		--POP 1:
		--LA_CUR_PR = '0'
		--LN55.LC_LON_BBT_STA = 'R' 
		--UTLWO39 Billing logic

SELECT
	'POP 1' AS POP,
	*
INTO
	#POP1
FROM
	(
		SELECT
			*,
			ROW_NUMBER() OVER(PARTITION BY BF_SSN, LN_SEQ, DLQ_INSTANCES 
							      ORDER BY BF_SSN, LN_SEQ, DLQ_INSTANCES, LD_BIL_DU) AS DLQ_COUNTER
		FROM
			(
				SELECT DISTINCT 
					PD10.DF_SPE_ACC_ID AS Account
					,LN10.BF_SSN
					,LN10.LN_SEQ 
					,LN10.LA_CUR_PRI AS Principal
					,CASE 
						WHEN LN72.LC_ITR_TYP = 'F1'
						THEN 'FIXED'
						ELSE 'VARIABLE' 
					END AS RateType
					,LN72.LR_INT_RDC_PGM_ORG AS BaseRate
					,LN72.LR_ITR AS ChargedRate
					,LN54.PM_BBS_PGM AS BBP
					--BBP Counters:
					,LN54.LN_BBS_STS_PCV_PAY AS CounterPCV -- Preconversion Payments
					,LN55.LN_LON_BBT_PAY_OVR AS CounterOVR -- Override Counter Payments 
					,LN55.LN_LON_BBT_PAY AS CounterRCV -- Compass-Received Payments
					,LN55.LD_LON_BBT_STA AS QualifiedDate -- (Date they achieved the benefit)

					,BL10.LD_BIL_CRT	
					,BL10.LN_SEQ_BIL_WI_DTE
					,BL10.LD_BIL_DU
					,'A' AS SCN_STA		
		
					,CAST(LN80.LD_BIL_DU_LON AS DATE) AS BILL_DUE
					,CAST(LN80.LD_BIL_STS_RIR_TOL AS DATE) AS SATISFIED
					,DATEDIFF(DAY,LN80.LD_BIL_DU_LON, LD_BIL_STS_RIR_TOL) AS DIFF
					,CASE
						WHEN DATEDIFF(DAY,LN80.LD_BIL_DU_LON, LN80.LD_BIL_STS_RIR_TOL) > 14
						THEN 1
						ELSE NULL
					END AS DLQ_INSTANCES
				FROM
					LN80_LON_BIL_CRF LN80
					INNER JOIN BL10_BR_BIL BL10
						ON BL10.BF_SSN = LN80.BF_SSN
						AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
						AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
					INNER JOIN LN10_LON LN10
 						ON LN80.BF_SSN = LN10.BF_SSN
						AND LN80.LN_SEQ = LN10.LN_SEQ
					INNER JOIN PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = LN10.BF_SSN
					INNER JOIN LN54_LON_BBS LN54 
						ON  LN54.BF_SSN = LN10.BF_SSN 
						AND LN54.LN_SEQ = LN10.LN_SEQ
						AND LN54.LC_STA_LN54 = 'A'
						AND LN54.LC_BBS_ELG = 'Y' --LOAN IS STILL ELIGIBLE FOR THE BB
					INNER JOIN LN55_LON_BBS_TIR LN55 
						ON  LN55.BF_SSN = LN54.BF_SSN 
						AND LN55.LN_SEQ = LN54.LN_SEQ
						AND LN55.PM_BBS_PGM = LN54.PM_BBS_PGM
						AND LN55.PN_BBS_PGM_SEQ = LN54.PN_BBS_PGM_SEQ
						AND LN55.LN_LON_BBS_SEQ = LN54.LN_LON_BBS_PGM_SEQ
						AND LN55.LC_STA_LN55 = 'A'
					LEFT JOIN LN72_INT_RTE_HST LN72 
						ON  LN72.BF_SSN = LN10.BF_SSN
						AND LN72.LN_SEQ = LN10.LN_SEQ
						AND LN72.LC_STA_LON72 = 'A'
						AND LN72.LD_ITR_EFF_BEG <= CONVERT(DATE,GETDATE())
						AND LN72.LD_ITR_EFF_END >= CONVERT(DATE,GETDATE())
				WHERE 
					BL10.LC_BIL_TYP = 'P'
					AND BL10.LC_STA_BIL10 = 'A'
					AND LN80.LI_BIL_DLQ_OVR_RIR <> 'Y'	
					AND LN10.LC_STA_LON10 = 'R' --RELEASED STATUS
					AND LN10.LD_LON_1_DSB < CONVERT(DATE,'20060501') --DISB BEFORE 05/01/2006 
					AND LN10.LF_LON_CUR_OWN = '828476' --UHEAA OWNER CODE (NOT A BANA, ALIGN, OR TILP LOAN) 
					AND LN10.LA_CUR_PRI = 0.00 --NO BALANCE
					AND LN55.LC_LON_BBT_STA = 'R' --BORR IS WORKING TOWARDS RIR (R=ReductionAchieved, Q=WorkingTowardsQual, D=Disqual, N=NotElg)
			)X
		WHERE 
			DLQ_INSTANCES IS NOT NULL
	)Y
WHERE
	DLQ_COUNTER = 4
	AND QualifiedDate > LD_BIL_DU --aka exclude if disqualification date is after qualified date. Use the 4th instance of delinquent LD_BIL_DU as the disqualification date
;

SELECT * FROM #POP1

/********************************* POP 2 **********************************/
--2) Open loans that have erroneously achieved the reduced interest rate
		--POP 2 = 
		--LA_CUR_PR > '0'
		--LN55.LC_LON_BBT_STA = 'R' 
		--UTLWO39 Billing logic

SELECT
	'POP 2' AS POP,
	*
INTO
	#POP2
FROM
	(
		SELECT
			*,
			ROW_NUMBER() OVER(PARTITION BY BF_SSN, LN_SEQ, DLQ_INSTANCES 
								  ORDER BY BF_SSN, LN_SEQ, DLQ_INSTANCES, LD_BIL_DU) AS DLQ_COUNTER
		FROM
			(
				SELECT DISTINCT 
					PD10.DF_SPE_ACC_ID AS Account
					,LN10.BF_SSN
					,LN10.LN_SEQ 
					,LN10.LA_CUR_PRI AS Principal
					,CASE 
						WHEN LN72.LC_ITR_TYP = 'F1'
						THEN 'FIXED'
						ELSE 'VARIABLE' 
					END AS RateType
					,LN72.LR_INT_RDC_PGM_ORG AS BaseRate
					,LN72.LR_ITR AS ChargedRate
					,LN54.PM_BBS_PGM AS BBP
					,LN54.LN_BBS_STS_PCV_PAY AS CounterPCV -- Preconversion Payments
					,LN55.LN_LON_BBT_PAY_OVR AS CounterOVR -- Override Counter Payments 
					,LN55.LN_LON_BBT_PAY AS CounterRCV -- Compass-Received Payments
					,LN55.LD_LON_BBT_STA AS QualifiedDate -- (Date they achieved the benefit)

					,BL10.LD_BIL_CRT	
					,BL10.LN_SEQ_BIL_WI_DTE
					,BL10.LD_BIL_DU
					,'A' AS SCN_STA		
		
					,CAST(LN80.LD_BIL_DU_LON AS DATE) AS BILL_DUE
					,CAST(LN80.LD_BIL_STS_RIR_TOL AS DATE) AS SATISFIED
					,DATEDIFF(DAY,LN80.LD_BIL_DU_LON, LN80.LD_BIL_STS_RIR_TOL) AS DIFF
					,CASE
						WHEN DATEDIFF(DAY,LN80.LD_BIL_DU_LON, LN80.LD_BIL_STS_RIR_TOL) > 14
						THEN 1
						ELSE NULL
					END AS DLQ_INSTANCES
				FROM
					LN80_LON_BIL_CRF LN80
					INNER JOIN BL10_BR_BIL BL10
						ON BL10.BF_SSN = LN80.BF_SSN
						AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
						AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
					INNER JOIN LN10_LON LN10
 						ON LN80.BF_SSN = LN10.BF_SSN
						AND LN80.LN_SEQ = LN10.LN_SEQ
					INNER JOIN PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = LN10.BF_SSN
					INNER JOIN LN54_LON_BBS LN54 
						ON  LN54.BF_SSN = LN10.BF_SSN 
						AND LN54.LN_SEQ = LN10.LN_SEQ
						AND LN54.LC_STA_LN54 = 'A'
						AND LN54.LC_BBS_ELG = 'Y' --LOAN IS STILL ELIGIBLE FOR THE BB
					INNER JOIN LN55_LON_BBS_TIR LN55 
						ON  LN55.BF_SSN = LN54.BF_SSN 
						AND LN55.LN_SEQ = LN54.LN_SEQ
						AND LN55.PM_BBS_PGM = LN54.PM_BBS_PGM
						AND LN55.PN_BBS_PGM_SEQ = LN54.PN_BBS_PGM_SEQ
						AND LN55.LN_LON_BBS_SEQ = LN54.LN_LON_BBS_PGM_SEQ
						AND LN55.LC_STA_LN55 = 'A'
					LEFT JOIN LN72_INT_RTE_HST LN72 
						ON  LN72.BF_SSN = LN10.BF_SSN
						AND LN72.LN_SEQ = LN10.LN_SEQ
						AND LN72.LC_STA_LON72 = 'A'
						AND LN72.LD_ITR_EFF_BEG <= CONVERT(DATE,GETDATE())
						AND LN72.LD_ITR_EFF_END >= CONVERT(DATE,GETDATE())
				WHERE 
					BL10.LC_BIL_TYP = 'P'
					AND BL10.LC_STA_BIL10 = 'A'
					AND LN80.LI_BIL_DLQ_OVR_RIR <> 'Y'	
					AND LN10.LC_STA_LON10 = 'R' --RELEASED STATUS
					AND LN10.LD_LON_1_DSB < CONVERT(DATE,'20060501') --DISB BEFORE 05/01/2006 
					AND LN10.LF_LON_CUR_OWN = '828476' --UHEAA OWNER CODE (NOT A BANA, ALIGN, OR TILP LOAN) 
					AND LN10.LA_CUR_PRI > 0.00 --HAS BALANCE
					AND LN55.LC_LON_BBT_STA = 'R' --BORR IS WORKING TOWARDS RIR (R=ReductionAchieved, Q=WorkingTowardsQual, D=Disqual, N=NotElg)
			)X
		WHERE 
			DLQ_INSTANCES IS NOT NULL
	)Y
WHERE
	DLQ_COUNTER = 4
	AND QualifiedDate > LD_BIL_DU --aka exclude if disqualification date is after qualified date. Use the 4th instance of delinquent LD_BIL_DU as the disqualification date
;

SELECT * FROM #POP2

/********************************* POP 3 **********************************/
--3) Open loans working towards the benefit that should have already been disqualified
		--POP 3 = 
		--LA_CUR_PR > '0'
		--LN55.LC_LON_BBT_STA = 'Q' 
		--UTLWO39 Billing logic

SELECT
	'POP 3' AS POP,
	*
INTO
	#POP3
FROM
	(
		SELECT
			*,
			ROW_NUMBER() OVER(PARTITION BY BF_SSN, LN_SEQ, DLQ_INSTANCES 
								  ORDER BY BF_SSN, LN_SEQ, DLQ_INSTANCES, LD_BIL_DU) AS DLQ_COUNTER
		FROM
			(
				SELECT DISTINCT 
					PD10.DF_SPE_ACC_ID AS Account
					,LN10.BF_SSN
					,LN10.LN_SEQ 
					,LN10.LA_CUR_PRI AS Principal
					,CASE 
						WHEN LN72.LC_ITR_TYP = 'F1'
						THEN 'FIXED'
						ELSE 'VARIABLE' 
					END AS RateType
					,LN72.LR_INT_RDC_PGM_ORG AS BaseRate
					,LN72.LR_ITR AS ChargedRate
					,LN54.PM_BBS_PGM AS BBP
					,LN54.LN_BBS_STS_PCV_PAY AS CounterPCV -- Preconversion Payments
					,LN55.LN_LON_BBT_PAY_OVR AS CounterOVR -- Override Counter Payments 
					,LN55.LN_LON_BBT_PAY AS CounterRCV -- Compass-Received Payments
					,LN55.LD_LON_BBT_STA AS QualifiedDate -- (Date they achieved the benefit)

					,BL10.LD_BIL_CRT	
					,BL10.LN_SEQ_BIL_WI_DTE
					,BL10.LD_BIL_DU
					,'A' AS SCN_STA		
		
					,CAST(LN80.LD_BIL_DU_LON AS DATE) AS BILL_DUE
					,CAST(LN80.LD_BIL_STS_RIR_TOL AS DATE) AS SATISFIED
					,DATEDIFF(DAY,LN80.LD_BIL_DU_LON, LD_BIL_STS_RIR_TOL) AS DIFF
					,CASE
						WHEN DATEDIFF(DAY,LN80.LD_BIL_DU_LON, LN80.LD_BIL_STS_RIR_TOL) > 14
						THEN 1
						ELSE NULL
					END AS DLQ_INSTANCES
				FROM
					LN80_LON_BIL_CRF LN80
					INNER JOIN BL10_BR_BIL BL10
						ON BL10.BF_SSN = LN80.BF_SSN
						AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
						AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
					INNER JOIN LN10_LON LN10
 						ON LN80.BF_SSN = LN10.BF_SSN
						AND LN80.LN_SEQ = LN10.LN_SEQ
					INNER JOIN PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = LN10.BF_SSN
					INNER JOIN LN54_LON_BBS LN54 
						ON  LN54.BF_SSN = LN10.BF_SSN 
						AND LN54.LN_SEQ = LN10.LN_SEQ
						AND LN54.LC_STA_LN54 = 'A'
						AND LN54.LC_BBS_ELG = 'Y' --LOAN IS STILL ELIGIBLE FOR THE BB
					INNER JOIN LN55_LON_BBS_TIR LN55 
						ON  LN55.BF_SSN = LN54.BF_SSN 
						AND LN55.LN_SEQ = LN54.LN_SEQ
						AND LN55.PM_BBS_PGM = LN54.PM_BBS_PGM
						AND LN55.PN_BBS_PGM_SEQ = LN54.PN_BBS_PGM_SEQ
						AND LN55.LN_LON_BBS_SEQ = LN54.LN_LON_BBS_PGM_SEQ
						AND LN55.LC_STA_LN55 = 'A'
					LEFT JOIN LN72_INT_RTE_HST LN72 
						ON  LN72.BF_SSN = LN10.BF_SSN
						AND LN72.LN_SEQ = LN10.LN_SEQ
						AND LN72.LC_STA_LON72 = 'A'
						AND LN72.LD_ITR_EFF_BEG <= CONVERT(DATE,GETDATE())
						AND LN72.LD_ITR_EFF_END >= CONVERT(DATE,GETDATE())
				WHERE 
					BL10.LC_BIL_TYP = 'P'
					AND BL10.LC_STA_BIL10 = 'A'
					AND LN80.LI_BIL_DLQ_OVR_RIR <> 'Y'	
					AND LN10.LC_STA_LON10 = 'R' --RELEASED STATUS
					AND LN10.LD_LON_1_DSB < CONVERT(DATE,'20060501') --DISB BEFORE 05/01/2006 
					AND LN10.LF_LON_CUR_OWN = '828476' --UHEAA OWNER CODE (NOT A BANA, ALIGN, OR TILP LOAN) 
					AND LN10.LA_CUR_PRI > 0.00 --HAS BALANCE
					AND LN55.LC_LON_BBT_STA = 'Q' --BORR IS WORKING TOWARDS RIR (R=ReductionAchieved, Q=WorkingTowardsQual, D=Disqual, N=NotElg)
			)X
		WHERE 
			DLQ_INSTANCES IS NOT NULL
	)Y
WHERE
	DLQ_COUNTER = 4
;

SELECT * FROM #POP3

--"BILL_I" inactive
SELECT -- DISTINCT 
	BL10.BF_SSN
	,LN10.LN_SEQ
	,BL10.LD_BIL_CRT	
	,BL10.LN_SEQ_BIL_WI_DTE
	,BL10.LD_BIL_DU
	,'I' AS SCN_STA
FROM
	LN80_LON_BIL_CRF LN80
	INNER JOIN BL10_BR_BIL BL10
		ON BL10.BF_SSN = LN80.BF_SSN
		AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
		AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
	INNER JOIN LN10_LON LN10
 		ON LN80.BF_SSN = LN10.BF_SSN
		AND LN80.LN_SEQ = LN10.LN_SEQ
	INNER JOIN LN50_BR_DFR_APV LN50
		ON LN10.BF_SSN = LN50.BF_SSN
		AND LN10.LN_SEQ = LN50.LN_SEQ
	INNER JOIN DF10_BR_DFR_REQ DF10
		ON DF10.BF_SSN = LN50.BF_SSN
		AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
WHERE 
	BL10.LC_BIL_TYP = 'P'
	AND BL10.LC_STA_BIL10 = 'I'
	AND LN50.LD_DFR_APL > CONVERT(DATE,DATEADD(DAY,15,LN80.LD_BIL_DU_LON))
	AND LN80.LI_BIL_DLQ_OVR_RIR <> 'Y'
	AND LN50.LC_STA_LON50 = 'A'
	AND DF10.LC_DFR_TYP NOT IN ('15','18')
	AND LN50.LD_DFR_APL >= CONVERT(DATE,'20060120')

;

--ROUGH ESTIMATES:

SELECT
	*,
	CASE WHEN ([Original Interest Rate] - Discount) < 0.00 THEN 0.000000000001 ELSE ([Original Interest Rate] - Discount) END AS TimelyPaymentsBenefitedRate --reg rate - IC_LON_PGM discount
INTO
	#RoughEstimates
FROM
	(
		SELECT DISTINCT
			A.*,
			CASE WHEN LN10.IC_LON_PGM IN ('STFFRD','UNSTFD','SLS','PLUS ','PLUSGB')
					THEN 2.000
				 WHEN LN10.IC_LON_PGM IN ('CNSLDN','SUBCNS','SUBSPC','UNCNS','UNSPC')
					THEN 1.000
					ELSE NULL
			END AS Discount,
			CASE WHEN LN10.IC_LON_PGM IN ('CNSLDN','UNCNS','SUBCNS','UNSPC','SUBSPC') 
					THEN LN10.LR_WIR_CON_LON
				 WHEN LP06.IC_LON_PGM IS NOT NULL 
					THEN LP06.PR_ITR_MIN
					ELSE NULL
			END AS [Original Interest Rate] --aka regulatory rate
		FROM
			(
				SELECT
					POPS.Pop,
					LN90.BF_SSN,
					LN90.LN_SEQ,
					POPS.QualifiedDate,
					SUM(LN90.LA_FAT_CUR_PRI) AS sum_LA_FAT_CUR_PRI
				FROM
					UDW..LN90_FIN_ATY LN90
					INNER JOIN
					(
						SELECT * FROM #POP1 UNION ALL
						SELECT * FROM #POP2 UNION ALL
						SELECT * FROM #POP3
					) POPS
						ON POPS.BF_SSN = LN90.BF_SSN
						AND POPS.LN_SEQ = LN90.LN_SEQ
				WHERE
					CAST(LN90.LD_FAT_EFF AS DATE) <= POPS.QualifiedDate
				GROUP BY
					POPS.Pop,
					LN90.BF_SSN,
					LN90.LN_SEQ,
					POPS.QualifiedDate
			)A
			INNER JOIN LN10_LON LN10
				ON A.BF_SSN = LN10.BF_SSN
				AND A.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN LN35_LON_OWN LN35
				ON LN35.BF_SSN = LN10.BF_SSN
				AND LN35.LN_SEQ = LN10.LN_SEQ
				AND LN35.LD_OWN_EFF_END IS NULL -- Active owner
			LEFT JOIN LN72_INT_RTE_HST LN72 
				ON  LN72.BF_SSN = LN10.BF_SSN
				AND LN72.LN_SEQ = LN10.LN_SEQ
				AND LN72.LC_STA_LON72 = 'A'
				AND LN72.LD_ITR_EFF_BEG <= CONVERT(DATE,GETDATE())
				AND LN72.LD_ITR_EFF_END >= CONVERT(DATE,GETDATE())
			LEFT JOIN LP06_ITR_AND_TYP LP06
				   ON LN10.IC_LON_PGM = LP06.IC_LON_PGM
				   AND LN10.LF_RGL_CAT_LP06 = LP06.PF_RGL_CAT
				   AND LN10.IF_GTR = LP06.IF_GTR
				   AND LN10.LF_LON_CUR_OWN = LP06.IF_OWN
				   AND LN35.IF_BND_ISS = LP06.IF_BND_ISS
				   AND A.QualifiedDate BETWEEN LP06.PD_EFF_SR_LPD06 AND LP06.PD_EFF_END_LPD06 --use target month instead of today's date
				   AND LN72.LC_ITR_TYP = LP06.PC_ITR_TYP
				   AND LP06.PC_STA_LPD06 = 'A'
	)B
WHERE
	B.[Original Interest Rate] IS NOT NULL
ORDER BY
	B.Pop,
	B.BF_SSN,
	B.LN_SEQ
;

SELECT * FROM #RoughEstimates

SELECT
	Pop,
	BF_SSN,
	LN_SEQ,
	QualifiedDate,
	sum_LA_FAT_CUR_PRI,
	Discount,
	[Original Interest Rate],
	TimelyPaymentsBenefitedRate,
	CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) AS NumeratorOrig,
	CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
			THEN 0.00
		ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
	END AS DenominatorOrig,
	CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) / CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
		 ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
	END AS AmountDue,
	CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) / CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
		 ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
	END * 72 - sum_LA_FAT_CUR_PRI AS InterestPaidReg,

	CEILING(
		LOG10((-1*CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) 
		/ CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
				ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
		END) --PMT *-1 to make negative
	
		/ (-1*CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) 
		/ CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
				ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
		END + ((TimelyPaymentsBenefitedRate/1200) * sum_LA_FAT_CUR_PRI))) --PMT + (Rate/k) * Pv
		/ LOG10(1 + (TimelyPaymentsBenefitedRate/1200))
	) AS Terms,
	
	(CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) / CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
		 ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
	END --Amount Due
	* CEILING(
		LOG10((-1*CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) 
		/ CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
				ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
		END) --PMT *-1 to make negative
	
		/ (-1*CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) 
		/ CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
				ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
		END + ((TimelyPaymentsBenefitedRate/1200) * sum_LA_FAT_CUR_PRI))) --PMT + (Rate/k) * Pv
		/ LOG10(1 + (TimelyPaymentsBenefitedRate/1200))
	)) --Terms 
	- sum_LA_FAT_CUR_PRI AS InterestPaidTimely,
	CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) / CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
		 ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
	END * 72 --Interest Reg
	- (CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) / CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
		 ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
	END --Amount Due
	* CEILING(
		LOG10((-1*CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) 
		/ CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
				ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
		END) --PMT *-1 to make negative
	
		/ (-1*CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) 
		/ CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
				ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
		END + ((TimelyPaymentsBenefitedRate/1200) * sum_LA_FAT_CUR_PRI))) --PMT + (Rate/k) * Pv
		/ LOG10(1 + (TimelyPaymentsBenefitedRate/1200))
	)) --Terms --InterestTimely
	AS InterestSaved
INTO
	#Part1
FROM
	#RoughEstimates
WHERE
	sum_LA_FAT_CUR_PRI > 0

SELECT * FROM #Part1
--POP3 extra stuff
SELECT
	*,
	CASE WHEN ([Original Interest Rate] - Discount) < 0.00 THEN 0.000000000001 ELSE ([Original Interest Rate] - Discount) END AS TimelyPaymentsBenefitedRate --reg rate - IC_LON_PGM discount
INTO
	#POP3Estimates
FROM
	(
		SELECT DISTINCT
			A.*,
			CASE
				WHEN LN10.IC_LON_PGM IN ('STFFRD','UNSTFD','SLS','PLUS ','PLUSGB')
					THEN 2.000
				WHEN LN10.IC_LON_PGM IN ('CNSLDN','SUBCNS','SUBSPC','UNCNS','UNSPC')
					THEN 1.000
					ELSE NULL
			END AS Discount,
			LN72.LR_INT_RDC_PGM_ORG AS [Original Interest Rate] --aka regulatory rate
		FROM
			(
				SELECT
					POPS.Pop,
					POPS.BF_SSN,
					POPS.LN_SEQ,
					CAST(DATEADD(MONTH, 48 - POPS.CounterRCV, GETDATE()) AS DATE) AS QualifiedDate,
					POPS.Principal - ((48 - POPS.CounterRCV) * CASE WHEN LoanCount.Loans > 1 THEN ISNULL(RS.LA_RPS_ISL,25.00) ELSE ISNULL(RS.LA_RPS_ISL,50.00) END) AS sum_LA_FAT_CUR_PRI --1 loan with no repay sched -> 50$ per month, 2 or more loans with no sched -> 25$ per loan per month
				FROM
					#POP3 POPS
					LEFT JOIN
					(
						SELECT
							BF_SSN,
							COUNT(LN_SEQ) AS Loans
						FROM
							#POP3
						GROUP BY
							BF_SSN
					) LoanCount
						ON LoanCount.BF_SSN = POPS.BF_SSN
					LEFT JOIN UDW.calc.RepaymentSchedules RS
						ON RS.BF_SSN = POPS.BF_SSN
						AND RS.LN_SEQ = POPS.LN_SEQ
						AND RS.CurrentGradation = 1
			)A
			INNER JOIN LN10_LON LN10
				ON A.BF_SSN = LN10.BF_SSN
				AND A.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN LN35_LON_OWN LN35
				ON LN35.BF_SSN = LN10.BF_SSN
				AND LN35.LN_SEQ = LN10.LN_SEQ
				AND LN35.LD_OWN_EFF_END IS NULL -- Active owner
			LEFT JOIN LN72_INT_RTE_HST LN72 
				ON  LN72.BF_SSN = LN10.BF_SSN
				AND LN72.LN_SEQ = LN10.LN_SEQ
				AND LN72.LC_STA_LON72 = 'A'
				AND LN72.LD_ITR_EFF_BEG <= CONVERT(DATE,GETDATE())
				AND LN72.LD_ITR_EFF_END >= CONVERT(DATE,GETDATE())
	)B
WHERE
	B.[Original Interest Rate] IS NOT NULL
ORDER BY
	B.Pop,
	B.BF_SSN,
	B.LN_SEQ

SELECT * FROM #POP3Estimates

SELECT
	Pop,
	BF_SSN,
	LN_SEQ,
	QualifiedDate,
	sum_LA_FAT_CUR_PRI,
	Discount,
	[Original Interest Rate],
	TimelyPaymentsBenefitedRate,
	CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) AS NumeratorOrig,
	CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
			THEN 0.00
		ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
	END AS DenominatorOrig,
	CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) / CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
		 ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
	END AS AmountDue,
	CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) / CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
		 ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
	END * 72 - sum_LA_FAT_CUR_PRI AS InterestPaidReg,

	CEILING(
		LOG10((-1*CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) 
		/ CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
				ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
		END) --PMT *-1 to make negative
	
		/ (-1*CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) 
		/ CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
				ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
		END + ((TimelyPaymentsBenefitedRate/1200) * sum_LA_FAT_CUR_PRI))) --PMT + (Rate/k) * Pv
		/ LOG10(1 + (TimelyPaymentsBenefitedRate/1200))
	) AS Terms,
	
	(CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) / CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
		 ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
	END --Amount Due
	* CEILING(
		LOG10((-1*CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) 
		/ CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
				ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
		END) --PMT *-1 to make negative
	
		/ (-1*CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) 
		/ CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
				ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
		END + ((TimelyPaymentsBenefitedRate/1200) * sum_LA_FAT_CUR_PRI))) --PMT + (Rate/k) * Pv
		/ LOG10(1 + (TimelyPaymentsBenefitedRate/1200))
	)) --Terms 
	- sum_LA_FAT_CUR_PRI AS InterestPaidTimely,
	CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) / CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
		 ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
	END * 72 --Interest Reg
	- (CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) / CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
		 ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
	END --Amount Due
	* CEILING(
		LOG10((-1*CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) 
		/ CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
				ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
		END) --PMT *-1 to make negative
	
		/ (-1*CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) 
		/ CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
				ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
		END + ((TimelyPaymentsBenefitedRate/1200) * sum_LA_FAT_CUR_PRI))) --PMT + (Rate/k) * Pv
		/ LOG10(1 + (TimelyPaymentsBenefitedRate/1200))
	)) --Terms --InterestTimely
	AS InterestSaved
INTO
	#Part2
FROM
	#POP3Estimates
WHERE
	sum_LA_FAT_CUR_PRI > 0

SELECT * FROM #Part2

SELECT
	*,
	CASE WHEN ([Original Interest Rate] - Discount) < 0.00 THEN 0.000000000001 ELSE ([Original Interest Rate] - Discount) END AS TimelyPaymentsBenefitedRate --reg rate - IC_LON_PGM discount
INTO
	#POP2Estimates
FROM
	(
		SELECT DISTINCT
			A.*,
			CASE
				WHEN LN10.IC_LON_PGM IN ('STFFRD','UNSTFD','SLS','PLUS ','PLUSGB')
					THEN 2.000
				WHEN LN10.IC_LON_PGM IN ('CNSLDN','SUBCNS','SUBSPC','UNCNS','UNSPC')
					THEN 1.000
					ELSE NULL
			END AS Discount,
			LN72.LR_INT_RDC_PGM_ORG AS [Original Interest Rate] --aka regulatory rate
		FROM
			(
				SELECT
					POPS.Pop,
					POPS.BF_SSN,
					POPS.LN_SEQ,
					GETDATE() AS QualifiedDate,
					CASE WHEN LN10.LA_CUR_PRI >= POPS.Principal THEN POPS.Principal ELSE LN10.LA_CUR_PRI END AS sum_LA_FAT_CUR_PRI 
				FROM
					#POP2 POPS
					INNER JOIN UDW..LN10_LON LN10
						ON LN10.BF_SSN = POPS.BF_SSN
						AND LN10.LN_SEQ = POPS.LN_SEQ
			)A
			INNER JOIN LN10_LON LN10
				ON A.BF_SSN = LN10.BF_SSN
				AND A.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN LN35_LON_OWN LN35
				ON LN35.BF_SSN = LN10.BF_SSN
				AND LN35.LN_SEQ = LN10.LN_SEQ
				AND LN35.LD_OWN_EFF_END IS NULL -- Active owner
			LEFT JOIN LN72_INT_RTE_HST LN72 
				ON  LN72.BF_SSN = LN10.BF_SSN
				AND LN72.LN_SEQ = LN10.LN_SEQ
				AND LN72.LC_STA_LON72 = 'A'
				AND LN72.LD_ITR_EFF_BEG <= CONVERT(DATE,GETDATE())
				AND LN72.LD_ITR_EFF_END >= CONVERT(DATE,GETDATE())
	)B
WHERE
	B.[Original Interest Rate] IS NOT NULL
ORDER BY
	B.Pop,
	B.BF_SSN,
	B.LN_SEQ

SELECT * FROM #POP2Estimates

SELECT
	Pop,
	BF_SSN,
	LN_SEQ,
	QualifiedDate,
	sum_LA_FAT_CUR_PRI,
	Discount,
	[Original Interest Rate],
	TimelyPaymentsBenefitedRate,
	CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) AS NumeratorOrig,
	CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
			THEN 0.00
		ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
	END AS DenominatorOrig,
	CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) / CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
		 ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
	END AS AmountDue,
	CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) / CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
		 ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
	END * 72 - sum_LA_FAT_CUR_PRI AS InterestPaidReg,

	CEILING(
		LOG10((-1*CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) 
		/ CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
				ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
		END) --PMT *-1 to make negative
	
		/ (-1*CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) 
		/ CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
				ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
		END + ((TimelyPaymentsBenefitedRate/1200) * sum_LA_FAT_CUR_PRI))) --PMT + (Rate/k) * Pv
		/ LOG10(1 + (TimelyPaymentsBenefitedRate/1200))
	) AS Terms,
	
	(CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) / CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
		 ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
	END --Amount Due
	* CEILING(
		LOG10((-1*CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) 
		/ CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
				ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
		END) --PMT *-1 to make negative
	
		/ (-1*CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) 
		/ CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
				ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
		END + ((TimelyPaymentsBenefitedRate/1200) * sum_LA_FAT_CUR_PRI))) --PMT + (Rate/k) * Pv
		/ LOG10(1 + (TimelyPaymentsBenefitedRate/1200))
	)) --Terms 
	- sum_LA_FAT_CUR_PRI AS InterestPaidTimely,
	CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) / CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
		 ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
	END * 72 --Interest Reg
	- (CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) / CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
		 ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
	END --Amount Due
	* CEILING(
		LOG10((-1*CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) 
		/ CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
				ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
		END) --PMT *-1 to make negative
	
		/ (-1*CONVERT(FLOAT, (sum_LA_FAT_CUR_PRI * ([Original Interest Rate] / 1200))) 
		/ CASE WHEN CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) = 0
				THEN 0.00
				ELSE CONVERT(FLOAT, (1 - (1 / POWER((1 + ([Original Interest Rate] / 1200)), 72)))) 
		END + ((TimelyPaymentsBenefitedRate/1200) * sum_LA_FAT_CUR_PRI))) --PMT + (Rate/k) * Pv
		/ LOG10(1 + (TimelyPaymentsBenefitedRate/1200))
	)) --Terms --InterestTimely
	AS InterestSaved
INTO
	#Part3
FROM
	#POP2Estimates
WHERE
	sum_LA_FAT_CUR_PRI > 0

SELECT * FROM #Part3

SELECT 
	P1.BF_SSN,
	P1.LN_SEQ,
	P1.QualifiedDate, 
	P1.sum_LA_FAT_CUR_PRI AS PrinAtQualDate,
	P1.InterestSaved AS InterestSavedFromQualDateToPayoff,
	P3.QualifiedDate AS Today,
	P3.sum_LA_FAT_CUR_PRI AS PrinAtCurrentDate,
	P3.InterestSaved AS InterestSavedFromTodayToPayoff,
	CASE WHEN (P1.InterestSaved - P3.InterestSaved) < 0.00 THEN 0.00 ELSE (P1.InterestSaved - P3.InterestSaved) END AS InterestSavedFromAchievementToToday
FROM 
	#Part3 P3 
	INNER JOIN #Part1 P1
		ON P1.BF_SSN = P3.BF_SSN
		AND P1.LN_SEQ = P3.LN_SEQ
		AND P1.POP = 'POP 2'
ORDER BY
	P1.InterestSaved - P3.InterestSaved
