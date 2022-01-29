CREATE PROCEDURE [bbontmdisq].[GetDisqualificationRecords]
AS

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
/********************************* POP **********************************/
--3) Open loans working towards the benefit that should have already been disqualified 
		--LA_CUR_PR > '0'
		--LN55.LC_LON_BBT_STA = 'Q' 
		--UTLWO39 Billing logic


SELECT DISTINCT
	BF_SSN AS Ssn,
	LN_SEQ AS LoanSequence,
	LD_BIL_DU AS DisqualificationDate
FROM
(
	SELECT
		*,
		ROW_NUMBER() OVER(PARTITION BY BF_SSN, LN_SEQ, DLQ_INSTANCES ORDER BY BF_SSN, LN_SEQ, DLQ_INSTANCES, LD_BIL_DU) AS DLQ_COUNTER
	FROM
	(
		SELECT DISTINCT 
			PD10.DF_SPE_ACC_ID AS Account,
			LN10.BF_SSN,
			LN10.LN_SEQ,
			LN10.LA_CUR_PRI AS Principal,
			CASE WHEN LN72.LC_ITR_TYP = 'F1'
				THEN 'FIXED'
				ELSE 'VARIABLE' 
			END AS RateType,
			LN72.LR_INT_RDC_PGM_ORG AS BaseRate,
			LN72.LR_ITR AS ChargedRate,
			LN54.PM_BBS_PGM AS BBP,
			BL10.LD_BIL_DU,
			'A' AS SCN_STA,
			CAST(LN80.LD_BIL_DU_LON AS DATE) AS BILL_DUE,
			CAST(LN80.LD_BIL_STS_RIR_TOL AS DATE) AS SATISFIED,
			DATEDIFF(DAY,LN80.LD_BIL_DU_LON, LD_BIL_STS_RIR_TOL) AS DIFF,
			1 AS DLQ_INSTANCES
		FROM
			dbo.LN80_LON_BIL_CRF LN80
			INNER JOIN
			(
				SELECT DISTINCT
					BF_SSN,
					LN_SEQ,
					LD_BIL_CRT,
					LN_SEQ_BIL_WI_DTE,
					MAX(LN_BIL_OCC_SEQ) AS LN_BIL_OCC_SEQ
				FROM
					dbo.LN80_LON_BIL_CRF
				WHERE
					DATEDIFF(DAY,LD_BIL_DU_LON, ISNULL(LD_BIL_STS_RIR_TOL,'2099-01-01')) > 14
				GROUP BY
					BF_SSN,
					LN_SEQ,
					LD_BIL_CRT,
					LN_SEQ_BIL_WI_DTE
			)MaxBillSeq
				ON MaxBillSeq.BF_SSN = LN80.BF_SSN
				AND MaxBillSeq.LN_SEQ = LN80.LN_SEQ
				AND MaxBillSeq.LD_BIL_CRT = LN80.LD_BIL_CRT
				AND MaxBillSeq.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
				AND MaxBillSeq.LN_BIL_OCC_SEQ = LN80.LN_BIL_OCC_SEQ
			INNER JOIN dbo.BL10_BR_BIL BL10
				ON BL10.BF_SSN = LN80.BF_SSN
				AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
				AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
				AND BL10.LC_BIL_TYP = 'P'
				AND BL10.LC_STA_BIL10 = 'A'
				AND BL10.LC_IND_BIL_SNT IN 
				(
 					'1' /*normal paper bill is printed and sent to borrower*/
 					,'2' /*reprint of normal bill*/
 					,'4' /*use due date minus current date > = 5 days (insufficient lead time)*/
 					,'7' /*paid ahead bill*/
 					,'G' /*when a c bill is printed (a c bill = eft bill printed and sent to borrower without an amount due. 1st time notice and letter will not be extracted)*/
 					,'A' /*ACH and/or ebill bill-not printed*/
 					,'B' /*ACH and/or ebill bill not printed < 15 days notice*/
 					,'C' /*ACH and/or ebill 1st notice not printed*/
 					,'D' /*ACH and/or ebill insufficient lead time not printed*/
 					,'E' /*monthly ACH and/or ebill bill not printed*/
 					,'F' /*ACH and/or ebill bill printed not sent<15 days notice*/
 					,'H' /*ACH and/or ebill insufficient lead time not printed*/
 					,'I' /*monthly ACH and/or ebill bill not printed*/
 					,'J' /*<15 days reprint request not printed*/
 					,'K' /*ACH and/or ebill 1st notice reprint rqst not printd*/
 					,'L' /*ACH and/or ebill insufficient lead time rpq not prntd*/
 					,'M' /*monthly ACH and/or ebill bill reprint rqst not printd*/
 					,'P' /*ACH and/or ebill reprint request not printed*/
 					,'Q' /*ACH and/or ebill paid ahead reprint reqst not printed*/
 					,'R' /*ACH and/or ebill evaluated by late fees process*/
 					,'8' /*inactive loans included in bill*/
 					,'T' /*reduced payment bill (borrower in a reduced payment forbearance)*/
 				)  --Dont look at paid ahead
			INNER JOIN dbo.LN10_LON LN10
 				ON LN80.BF_SSN = LN10.BF_SSN
				AND LN80.LN_SEQ = LN10.LN_SEQ
				AND LN10.LC_STA_LON10 = 'R' --RELEASED STATUS
				AND LN10.LA_CUR_PRI > 0.00
				AND LN10.LD_LON_1_DSB < CONVERT(DATE,'20060501') --DISB BEFORE 05/01/2006 
				AND LN10.LF_LON_CUR_OWN = '828476' --UHEAA OWNER CODE (NOT A BANA, ALIGN, OR TILP LOAN) 
			INNER JOIN dbo.PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
			INNER JOIN dbo.LN54_LON_BBS LN54 
				ON LN54.BF_SSN = LN10.BF_SSN 
				AND LN54.LN_SEQ = LN10.LN_SEQ
				AND LN54.LC_STA_LN54 = 'A'
				AND LN54.LC_BBS_ELG = 'Y' --LOAN IS STILL ELIGIBLE FOR THE BB
			INNER JOIN dbo.LN55_LON_BBS_TIR LN55 
				ON LN55.BF_SSN = LN54.BF_SSN 
				AND LN55.LN_SEQ = LN54.LN_SEQ
				AND LN55.PM_BBS_PGM = LN54.PM_BBS_PGM
				AND LN55.PN_BBS_PGM_SEQ = LN54.PN_BBS_PGM_SEQ
				AND LN55.LC_STA_LN55 = 'A'
				AND LN55.LC_LON_BBT_STA = 'Q' --BORR IS WORKING TOWARDS RIR (R=ReductionAchieved, Q=WorkingTowardsQual, D=Disqual, N=NotElg)
			LEFT JOIN dbo.LN72_INT_RTE_HST LN72 
				ON LN72.BF_SSN = LN10.BF_SSN
				AND LN72.LN_SEQ = LN10.LN_SEQ
				AND LN72.LC_STA_LON72 = 'A'
				AND LN72.LD_ITR_EFF_BEG <= CONVERT(DATE,GETDATE())
				AND LN72.LD_ITR_EFF_END >= CONVERT(DATE,GETDATE())
			LEFT JOIN
			(
				SELECT DISTINCT
					LN50.BF_SSN,
					LN50.LN_SEQ,
					LN50.LD_DFR_APL,
					LN50.LD_DFR_BEG,
					LN50.LD_DFR_END
				FROM
					LN50_BR_DFR_APV LN50
					INNER JOIN DF10_BR_DFR_REQ DF10
						ON DF10.BF_SSN = LN50.BF_SSN
						AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
						AND DF10.LC_DFR_TYP IN('15','18')
						AND DF10.LC_STA_DFR10 = 'A'
						AND DF10.LC_DFR_STA = 'A'
				WHERE
					LN50.LC_DFR_RSP != '003'
					AND LN50.LC_STA_LON50 = 'A'
					AND LN50.LD_DFR_APL >= CONVERT(DATE,'20060120')
			) DF10
				ON DF10.BF_SSN = LN10.BF_SSN
				AND DF10.LN_SEQ = LN10.LN_SEQ
				AND DF10.LD_DFR_APL > CAST(DATEADD(DAY,15,LN80.LD_BIL_DU_LON) AS DATE) 
				AND CAST(LN80.LD_BIL_DU_LON AS DATE) BETWEEN CAST(DF10.LD_DFR_BEG AS DATE) AND CAST(DF10.LD_DFR_END AS DATE)
		WHERE 
			ISNULL(LN80.LI_BIL_DLQ_OVR_RIR,'') NOT IN('Y','P','O')
			AND LN80.LC_BIL_TYP_LON = 'P' --RPS BILL
			AND DF10.BF_SSN IS NULL --No in school defer
			AND DATEDIFF(DAY,LN80.LD_BIL_DU_LON, LN80.LD_BIL_STS_RIR_TOL) > 14

		UNION ALL
		
		SELECT DISTINCT 
			PD10.DF_SPE_ACC_ID AS Account,
			LN10.BF_SSN,
			LN10.LN_SEQ,
			LN10.LA_CUR_PRI AS Principal,
			CASE WHEN LN72.LC_ITR_TYP = 'F1'
				THEN 'FIXED'
				ELSE 'VARIABLE' 
			END AS RateType,
			LN72.LR_INT_RDC_PGM_ORG AS BaseRate,
			LN72.LR_ITR AS ChargedRate,
			LN54.PM_BBS_PGM AS BBP,
			BL10.LD_BIL_DU,
			'A' AS SCN_STA,
			CAST(LN80.LD_BIL_DU_LON AS DATE) AS BILL_DUE,
			CAST(LN80.LD_BIL_STS_RIR_TOL AS DATE) AS SATISFIED,
			DATEDIFF(DAY,LN80.LD_BIL_DU_LON, LD_BIL_STS_RIR_TOL) AS DIFF,
			1 AS DLQ_INSTANCES
		FROM 
			dbo.LN80_LON_BIL_CRF LN80
			INNER JOIN
			(
				SELECT DISTINCT
					BF_SSN,
					LN_SEQ,
					LD_BIL_CRT,
					LN_SEQ_BIL_WI_DTE,
					MAX(LN_BIL_OCC_SEQ) AS LN_BIL_OCC_SEQ
				FROM
					dbo.LN80_LON_BIL_CRF
				WHERE
					DATEDIFF(DAY,LD_BIL_DU_LON, ISNULL(LD_BIL_STS_RIR_TOL,'2099-01-01')) > 14
				GROUP BY
					BF_SSN,
					LN_SEQ,
					LD_BIL_CRT,
					LN_SEQ_BIL_WI_DTE
			)MaxBillSeq
				ON MaxBillSeq.BF_SSN = LN80.BF_SSN
				AND MaxBillSeq.LN_SEQ = LN80.LN_SEQ
				AND MaxBillSeq.LD_BIL_CRT = LN80.LD_BIL_CRT
				AND MaxBillSeq.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
				AND MaxBillSeq.LN_BIL_OCC_SEQ = LN80.LN_BIL_OCC_SEQ
			INNER JOIN dbo.BL10_BR_BIL BL10
				ON BL10.BF_SSN = LN80.BF_SSN
				AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
				AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
				AND BL10.LC_BIL_TYP = 'P'
				AND BL10.LC_IND_BIL_SNT IN 
				(
 					'1' /*normal paper bill is printed and sent to borrower*/
 					,'2' /*reprint of normal bill*/
 					,'4' /*use due date minus current date > = 5 days (insufficient lead time)*/
 					,'7' /*paid ahead bill*/
 					,'G' /*when a c bill is printed (a c bill = eft bill printed and sent to borrower without an amount due. 1st time notice and letter will not be extracted)*/
 					,'A' /*ACH and/or ebill bill-not printed*/
 					,'B' /*ACH and/or ebill bill not printed < 15 days notice*/
 					,'C' /*ACH and/or ebill 1st notice not printed*/
 					,'D' /*ACH and/or ebill insufficient lead time not printed*/
 					,'E' /*monthly ACH and/or ebill bill not printed*/
 					,'F' /*ACH and/or ebill bill printed not sent<15 days notice*/
 					,'H' /*ACH and/or ebill insufficient lead time not printed*/
 					,'I' /*monthly ACH and/or ebill bill not printed*/
 					,'J' /*<15 days reprint request not printed*/
 					,'K' /*ACH and/or ebill 1st notice reprint rqst not printd*/
 					,'L' /*ACH and/or ebill insufficient lead time rpq not prntd*/
 					,'M' /*monthly ACH and/or ebill bill reprint rqst not printd*/
 					,'P' /*ACH and/or ebill reprint request not printed*/
 					,'Q' /*ACH and/or ebill paid ahead reprint reqst not printed*/
 					,'R' /*ACH and/or ebill evaluated by late fees process*/
 					,'8' /*inactive loans included in bill*/
 					,'T' /*reduced payment bill (borrower in a reduced payment forbearance)*/
 				) --Dont look at paid ahead
			INNER JOIN dbo.LN10_LON LN10 
				ON LN10.BF_SSN = LN80.BF_SSN 
				AND LN10.LN_SEQ = LN80.LN_SEQ
				AND LN10.LA_CUR_PRI > 0.00
				AND LN10.LC_STA_LON10 = 'R'
				AND LN10.LD_LON_1_DSB < CONVERT(DATE,'20060501') --DISB BEFORE 05/01/2006 
				AND LN10.LF_LON_CUR_OWN = '828476'--UHEAA OWNER CODE (NOT A BANA, ALIGN, OR TILP LOAN) 
			INNER JOIN dbo.PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
			INNER JOIN dbo.LN54_LON_BBS LN54 
				ON LN54.BF_SSN = LN10.BF_SSN 
				AND LN54.LN_SEQ = LN10.LN_SEQ
				AND LN54.LC_STA_LN54 = 'A'
				AND LN54.LC_BBS_ELG = 'Y' --LOAN IS ELIGIBLE FOR THE BB
			INNER JOIN dbo.LN55_LON_BBS_TIR LN55 
				ON LN55.BF_SSN = LN54.BF_SSN 
				AND LN55.LN_SEQ = LN54.LN_SEQ
				AND LN55.PM_BBS_PGM = LN54.PM_BBS_PGM
				AND LN55.PN_BBS_PGM_SEQ = LN54.PN_BBS_PGM_SEQ
				AND LN55.LC_STA_LN55 = 'A'
				AND LN55.LC_LON_BBT_STA = 'Q' --WORKING TOWARDS THE BB
			LEFT JOIN dbo.LN72_INT_RTE_HST LN72 
				ON LN72.BF_SSN = LN10.BF_SSN
				AND LN72.LN_SEQ = LN10.LN_SEQ
				AND LN72.LC_STA_LON72 = 'A'
				AND LN72.LD_ITR_EFF_BEG <= CONVERT(DATE,GETDATE())
				AND LN72.LD_ITR_EFF_END >= CONVERT(DATE,GETDATE())
			LEFT JOIN
			(
				SELECT DISTINCT
					LN50.BF_SSN,
					LN50.LN_SEQ,
					LN50.LD_DFR_APL,
					LN50.LD_DFR_BEG,
					LN50.LD_DFR_END
				FROM	
					dbo.LN50_BR_DFR_APV LN50
					INNER JOIN dbo.DF10_BR_DFR_REQ DF10
						ON DF10.BF_SSN = LN50.BF_SSN
						AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
						AND DF10.LC_STA_DFR10 = 'A'
						AND DF10.LC_DFR_STA = 'A'
						AND DF10.LC_DFR_TYP NOT IN ('15', '18')
				WHERE
					LN50.LC_DFR_RSP != '003'
					AND LN50.LC_STA_LON50 = 'A'
			) DF10
				ON DF10.BF_SSN = LN10.BF_SSN
				AND DF10.LN_SEQ = LN10.LN_SEQ
				AND DF10.LD_DFR_BEG <= LN80.LD_BIL_DU_LON  
				AND DF10.LD_DFR_END >= LN80.LD_BIL_DU_LON
				AND DF10.LD_DFR_APL > DATEADD(DAY,15,LN80.LD_BIL_DU_LON)  --BILL WAS AT LEAST 15 DAYS DELQ WHEN THE D/F WAS APPLIED
			LEFT JOIN dbo.LN60_BR_FOR_APV LN60
				ON LN60.BF_SSN = LN80.BF_SSN
				AND LN60.LN_SEQ = LN80.LN_SEQ
				AND LN60.LD_FOR_BEG <= LN80.LD_BIL_DU_LON   
				AND LN60.LD_FOR_END >= LN80.LD_BIL_DU_LON
				AND LN60.LD_FOR_APL > DATEADD(DAY,15,LN80.LD_BIL_DU_LON) --BILL WAS AT LEAST 15 DAYS DELQ WHEN THE D/F WAS APPLIED
				AND LN60.LC_FOR_RSP  != '003'
				AND LN60.LC_STA_LON60 = 'A'
			LEFT JOIN dbo.FB10_BR_FOR_REQ FB10
				ON FB10.BF_SSN = LN60.BF_SSN
				AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
				AND FB10.LC_STA_FOR10 = 'A'
				AND FB10.LC_FOR_STA = 'A'
		WHERE 
			LN80.LC_STA_LON80 = 'I' --INACTIVE BILL
			AND LN80.LC_BIL_TYP_LON = 'P' --RPS BILL
			AND ISNULL(LN80.LI_BIL_DLQ_OVR_RIR,'') IN ('', 'N') --BILL DOESN'T HAVE A BB OVERRIDE
			AND DATEDIFF(DAY,LN80.LD_BIL_DU_LON, ISNULL(LN80.LD_BIL_STS_RIR_TOL,'2099-01-01')) > 14-- BILL WAS NEVER SATISIFED PRIOR TO BEING ACTIVATED OR BILL WAS 15+ DAYS LATE WHEN COVERED BY PMT
			AND 
			( 
				DF10.BF_SSN IS NOT NULL 
				OR FB10.BF_SSN IS NOT NULL
			) --BILL WAS INACTIVATED BY A FORB OR A NON-SCHOOL DEFER	
	) Delq
	WHERE 
		DLQ_INSTANCES IS NOT NULL
)DelqFour
WHERE
	DLQ_COUNTER = 4
;
