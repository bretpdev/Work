CREATE PROCEDURE [bbreininsc].[LoadU36Records]
AS
BEGIN

TRUNCATE TABLE ULS.bbreininsc._CoveredStrikes;
TRUNCATE TABLE ULS.bbreininsc._LoanStrikeCount;

DECLARE @Today DATE = GETDATE();
--Use this list as NON strikes
INSERT INTO ULS.bbreininsc._CoveredStrikes
SELECT DISTINCT
	LN80.BF_SSN,
	LN80.LN_SEQ,
	LN80.LD_BIL_CRT,
	LN80.LN_SEQ_BIL_WI_DTE,
	LN80.LN_BIL_OCC_SEQ,
	LN80.LD_BIL_DU_LON,
	LN80.LI_BIL_DLQ_OVR_RIR,
	CoveragePeriod.StartDate,
	CoveragePeriod.EndDate
FROM
	UDW..LN10_LON LN10
	INNER JOIN UDW..LN54_LON_BBS LN54 
		ON LN54.BF_SSN = LN10.BF_SSN 
		AND LN54.LN_SEQ = LN10.LN_SEQ
		AND LN54.LC_STA_LN54 = 'A'
		AND LN54.LC_BBS_ELG = 'N' --LOAN IS STILL ELIGIBLE FOR THE BB
	INNER JOIN UDW..LN55_LON_BBS_TIR LN55 
		ON LN55.BF_SSN = LN54.BF_SSN 
		AND LN55.LN_SEQ = LN54.LN_SEQ
		AND LN55.PM_BBS_PGM = LN54.PM_BBS_PGM
		AND LN55.PN_BBS_PGM_SEQ = LN54.PN_BBS_PGM_SEQ
		--AND LN55.LN_LON_BBS_SEQ = LN54.LN_LON_BBS_PGM_SEQ
		AND LN55.LC_STA_LN55 = 'A'
		AND LN55.LC_LON_BBT_STA = 'D' --BORR IS WORKING TOWARDS RIR (R=ReductionAchieved, Q=WorkingTowardsQual, D=Disqual, N=NotElg)
	INNER JOIN
	(
		SELECT
			LN80.BF_SSN,
			LN80.LN_SEQ,
			LN80.LN_SEQ_BIL_WI_DTE,
			LN80.LD_BIL_DU_LON,
			MAX(LN80.LN_BIL_OCC_SEQ) AS LN_BIL_OCC_SEQ,
			LN80.LD_BIL_CRT
		FROM
			UDW..LN80_LON_BIL_CRF LN80
			INNER JOIN
			(
				SELECT
					BF_SSN,
					LN_SEQ,
					LN_SEQ_BIL_WI_DTE,
					LD_BIL_DU_LON,
					MAX(LD_BIL_CRT) AS LD_BIL_CRT
				FROM
					UDW..LN80_LON_BIL_CRF
				WHERE
					CAST(LD_BIL_DU_LON AS DATE) <= CAST(GETDATE() AS DATE)
					AND LC_BIL_TYP_LON = 'P'
				GROUP BY
					BF_SSN,
					LN_SEQ,
					LN_SEQ_BIL_WI_DTE,
					LD_BIL_DU_LON
			) MaxBillCRT
				ON MaxBillCRT.BF_SSN = LN80.BF_SSN
				AND MaxBillCRT.LN_SEQ = LN80.LN_SEQ
				AND MaxBillCRT.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
				AND MaxBillCRT.LD_BIL_DU_LON = LN80.LD_BIL_DU_LON
				AND MaxBillCRT.LD_BIL_CRT = LN80.LD_BIL_CRT
		GROUP BY
			LN80.BF_SSN,
			LN80.LN_SEQ,
			LN80.LN_SEQ_BIL_WI_DTE,
			LN80.LD_BIL_DU_LON,
			LN80.LD_BIL_CRT
	) MaxLN80
		ON MaxLN80.BF_SSN = LN10.BF_SSN
		AND MaxLN80.LN_SEQ = LN10.LN_SEQ
	INNER JOIN UDW..LN80_LON_BIL_CRF LN80
		ON LN80.BF_SSN = MaxLN80.BF_SSN
		AND LN80.LN_SEQ = MaxLN80.LN_SEQ
		AND LN80.LN_SEQ_BIL_WI_DTE = MaxLN80.LN_SEQ_BIL_WI_DTE
		AND LN80.LD_BIL_DU_LON = MaxLN80.LD_BIL_DU_LON
		AND LN80.LD_BIL_CRT = MaxLN80.LD_BIL_CRT
		AND LN80.LN_BIL_OCC_SEQ = MaxLN80.LN_BIL_OCC_SEQ
	INNER JOIN UDW..BL10_BR_BIL BL10
		ON BL10.BF_SSN = LN80.BF_SSN
		AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
		AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
	INNER JOIN
	(
		SELECT DISTINCT
			FB10.BF_SSN,
			LN60.LN_SEQ,
			CAST(LN60.LD_FOR_BEG AS DATE) AS StartDate,
			CAST(LN60.LD_FOR_END AS DATE) AS EndDate
		FROM	
			UDW..FB10_BR_FOR_REQ FB10
			INNER JOIN UDW..LN60_BR_FOR_APV LN60
				ON LN60.BF_SSN = FB10.BF_SSN
				AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
			INNER JOIN UDW..LN10_LON LN10
				ON LN10.BF_SSN = LN60.BF_SSN
				AND LN10.LN_SEQ = LN60.LN_SEQ
		WHERE
			FB10.LC_FOR_STA = 'A'
			AND LN60.LC_STA_LON60 = 'A'
			AND FB10.LC_STA_FOR10 = 'A'
			AND LN60.LC_FOR_RSP != '003'
			AND FB10.LC_FOR_TYP IN ('01','04','07','10','13','18','20','25','26','39','40','41','44')
			AND ISNULL(FB10.LD_FOR_INF_CER,LN10.LD_LON_ACL_ADD) >= '2006-01-20'

		UNION ALL
		
		SELECT DISTINCT
			DF10.BF_SSN,
			LN50.LN_SEQ,
			CAST(LN50.LD_DFR_BEG AS DATE) AS StartDate,
			CAST(LN50.LD_DFR_END AS DATE) AS EndDate
		FROM	
			UDW..DF10_BR_DFR_REQ DF10
			INNER JOIN UDW..LN50_BR_DFR_APV LN50
				ON LN50.BF_SSN = DF10.BF_SSN
				AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			INNER JOIN UDW..LN10_LON LN10
				ON LN10.BF_SSN = LN50.BF_SSN
				AND LN10.LN_SEQ = LN50.LN_SEQ
		WHERE
			DF10.LC_DFR_STA = 'A'
			AND LN50.LC_STA_LON50 = 'A'
			AND DF10.LC_STA_DFR10 = 'A'
			AND LN50.LC_DFR_RSP != '003'
			AND ISNULL(DF10.LD_DFR_INF_CER,LN10.LD_LON_ACL_ADD) >= '2006-01-20'
	) CoveragePeriod
		ON CoveragePeriod.BF_SSN = LN80.BF_SSN
		AND CoveragePeriod.LN_SEQ = LN80.LN_SEQ
		AND LN80.LD_BIL_DU_LON BETWEEN CoveragePeriod.StartDate AND CoveragePeriod.EndDate
WHERE
	LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 = 'R' 
	AND LN54.PM_BBS_PGM = 'U36'
	AND LN54.LC_STA_LN54 = 'A'
	AND LN10.LF_LON_CUR_OWN = '828476' --UHEAA OWNER CODE (NOT A BANA, ALIGN, OR TILP LOAN) 
	AND BL10.LC_BIL_TYP = 'P'
	AND BL10.LC_STA_BIL10 = 'A'
	AND BL10.LC_IND_BIL_SNT IN 
	(
		'1', /*normal paper bill is printed and sent to borrower*/
		'2', /*reprint of normal bill*/
		'4', /*use due date minus current date > = 5 days (insufficient lead time)*/
		'6', /*bad address*/
		'7', /*paid ahead bill*/
		'G', /*when a c bill is printed (a c bill = eft bill printed and sent to borrower without an amount due.  1st time notice and letter will not be extracted)*/
		'A', /*ACH and/or ebill bill-not printed*/
		'B', /*ACH and/or ebill bill not printed < 15 days notice*/
		'C', /*ACH and/or ebill 1st notice not printed*/
		'D', /*ACH and/or ebill insufficient lead time not printed*/
		'E', /*monthly ACH and/or ebill bill not printed*/
		'F', /*ACH and/or ebill bill printed not sent<15 days notice*/
		'H', /*ACH and/or ebill insufficient lead time not printed*/
		'I', /*monthly ACH and/or ebill bill not printed*/
		'J', /*<15 days reprint request not printed*/
		'K', /*ACH and/or ebill 1st notice reprint rqst not printd*/
		'L', /*ACH and/or ebill insufficient lead time rpq not prntd*/
		'M', /*monthly ACH and/or ebill bill reprint rqst not printd*/
		'P', /*ACH and/or ebill reprint request not printed*/
		'Q', /*ACH and/or ebill paid ahead reprint reqst not printed*/
		'R', /*ACH and/or ebill evaluated by late fees process*/
		'8' /*inactive loans included in bill*/
	)
	AND CAST(LN80.LD_BIL_DU_LON AS DATE) <= CAST(GETDATE() AS DATE)
	AND LN10.LF_LON_CUR_OWN = '828476' --UHEAA OWNER CODE (NOT A BANA, ALIGN, OR TILP LOAN) 
	AND ISNULL(LN80.LI_BIL_DLQ_OVR_RIR,'') NOT IN ('Y','O','P')

--Determine if any OTHER bills are 15 or more days delinquent AND ACTIVE (diff between due and sat dates).
INSERT INTO ULS.bbreininsc._LoanStrikeCount
SELECT DISTINCT
	LN80.BF_SSN,
	LN80.LN_SEQ,
	NULL,
	NULL,
	--Fatal strikes start at 15 days
	SUM(CASE WHEN DATEDIFF(DAY,LN80.LD_BIL_DU_LON,ISNULL(LN80.LD_BIL_STS_RIR_TOL,GETDATE())) > 15 THEN 1 ELSE 0 END) OVER(PARTITION BY LN80.BF_SSN, LN80.LN_SEQ) AS FatalStrike
FROM
	UDW..LN10_LON LN10
	INNER JOIN UDW..LN54_LON_BBS LN54 
		ON LN54.BF_SSN = LN10.BF_SSN 
		AND LN54.LN_SEQ = LN10.LN_SEQ
		AND LN54.LC_STA_LN54 = 'A'
		AND LN54.LC_BBS_ELG = 'N' --LOAN IS STILL ELIGIBLE FOR THE BB
	INNER JOIN UDW..LN55_LON_BBS_TIR LN55 
		ON LN55.BF_SSN = LN54.BF_SSN 
		AND LN55.LN_SEQ = LN54.LN_SEQ
		AND LN55.PM_BBS_PGM = LN54.PM_BBS_PGM
		AND LN55.PN_BBS_PGM_SEQ = LN54.PN_BBS_PGM_SEQ
		--AND LN55.LN_LON_BBS_SEQ = LN54.LN_LON_BBS_PGM_SEQ
		AND LN55.LC_STA_LN55 = 'A'
		AND LN55.LC_LON_BBT_STA = 'D' --BORR IS WORKING TOWARDS RIR (R=ReductionAchieved, Q=WorkingTowardsQual, D=Disqual, N=NotElg)
	INNER JOIN
	(
		SELECT
			LN80.BF_SSN,
			LN80.LN_SEQ,
			LN80.LN_SEQ_BIL_WI_DTE,
			LN80.LD_BIL_DU_LON,
			LN80.LN_BIL_OCC_SEQ,
			LN80.LD_BIL_CRT,
			ROW_NUMBER() OVER(PARTITION BY LN80.BF_SSN, LN80.LN_SEQ ORDER BY LN80.LD_BIL_CRT, LN80.LN_BIL_OCC_SEQ DESC) AS BillNumber
		FROM
			UDW..LN80_LON_BIL_CRF LN80
			INNER JOIN
			(
				SELECT
					LN80.BF_SSN,
					LN80.LN_SEQ,
					LN80.LN_SEQ_BIL_WI_DTE,
					LN80.LD_BIL_DU_LON,
					MAX(LN80.LN_BIL_OCC_SEQ) AS LN_BIL_OCC_SEQ,
					LN80.LD_BIL_CRT
				FROM
					UDW..LN80_LON_BIL_CRF LN80
					INNER JOIN
					(
						SELECT
							LN80.BF_SSN,
							LN80.LN_SEQ,
							LN_SEQ_BIL_WI_DTE,
							LD_BIL_DU_LON,
							MAX(LD_BIL_CRT) AS LD_BIL_CRT
						FROM
							UDW..LN80_LON_BIL_CRF LN80
							INNER JOIN UDW..LN54_LON_BBS LN54 
								ON LN54.BF_SSN = LN80.BF_SSN 
								AND LN54.LN_SEQ = LN80.LN_SEQ
								AND LN54.LC_STA_LN54 = 'A'
								AND LN54.LC_BBS_ELG = 'N' --LOAN IS STILL ELIGIBLE FOR THE BB
							INNER JOIN UDW..LN55_LON_BBS_TIR LN55 
								ON LN55.BF_SSN = LN54.BF_SSN 
								AND LN55.LN_SEQ = LN54.LN_SEQ
								AND LN55.PM_BBS_PGM = LN54.PM_BBS_PGM
								AND LN55.PN_BBS_PGM_SEQ = LN54.PN_BBS_PGM_SEQ
								--AND LN55.LN_LON_BBS_SEQ = LN54.LN_LON_BBS_PGM_SEQ
								AND LN55.LC_STA_LN55 = 'A'
								AND LN55.LC_LON_BBT_STA = 'D' 
						WHERE
							CAST(LD_BIL_DU_LON AS DATE) <= CAST(GETDATE() AS DATE)
							AND LC_BIL_TYP_LON = 'P'
							AND LC_STA_LON80 = 'A'
						GROUP BY
							LN80.BF_SSN,
							LN80.LN_SEQ,
							LN_SEQ_BIL_WI_DTE,
							LD_BIL_DU_LON
					) MaxBillCRT
						ON MaxBillCRT.BF_SSN = LN80.BF_SSN
						AND MaxBillCRT.LN_SEQ = LN80.LN_SEQ
						AND MaxBillCRT.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
						AND MaxBillCRT.LD_BIL_DU_LON = LN80.LD_BIL_DU_LON
						AND MaxBillCRT.LD_BIL_CRT = LN80.LD_BIL_CRT
				GROUP BY
					LN80.BF_SSN,
					LN80.LN_SEQ,
					LN80.LN_SEQ_BIL_WI_DTE,
					LN80.LD_BIL_DU_LON,
					LN80.LD_BIL_CRT
			) MaxLN80
				ON MaxLN80.BF_SSN = LN80.BF_SSN
				AND MaxLN80.LN_SEQ = LN80.LN_SEQ
				AND MaxLN80.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
				AND MaxLN80.LN_BIL_OCC_SEQ = LN80.LN_BIL_OCC_SEQ
				AND MaxLN80.LD_BIL_CRT = LN80.LD_BIL_CRT
				AND MaxLN80.LD_BIL_DU_LON = LN80.LD_BIL_DU_LON
	) First36
		ON First36.BF_SSN = LN10.BF_SSN
		AND First36.LN_SEQ = LN10.LN_SEQ
		AND First36.BillNumber <= 36 --first 36 bills are the only exclusionary ones
	INNER JOIN UDW..LN80_LON_BIL_CRF LN80
		ON LN80.BF_SSN = First36.BF_SSN
		AND LN80.LN_SEQ = First36.LN_SEQ
		AND LN80.LN_SEQ_BIL_WI_DTE = First36.LN_SEQ_BIL_WI_DTE
		AND LN80.LD_BIL_DU_LON = First36.LD_BIL_DU_LON
		AND LN80.LD_BIL_CRT = First36.LD_BIL_CRT
		AND LN80.LN_BIL_OCC_SEQ = First36.LN_BIL_OCC_SEQ
	INNER JOIN UDW..BL10_BR_BIL BL10
		ON BL10.BF_SSN = LN80.BF_SSN
		AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
		AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
	LEFT JOIN ULS.bbreininsc._CoveredStrikes CS
		ON CS.BF_SSN = LN80.BF_SSN
		AND CS.LN_SEQ = LN80.LN_SEQ
		AND CS.LD_BIL_CRT = LN80.LD_BIL_CRT
		AND CS.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
		AND CS.LN_BIL_OCC_SEQ = LN80.LN_BIL_OCC_SEQ
WHERE
	CS.BF_SSN IS NULL --Not one of the covered bills
	AND DATEDIFF(DAY,LN80.LD_BIL_DU_LON,ISNULL(LN80.LD_BIL_STS_RIR_TOL,GETDATE())) >= 15 --Overdue by at least 15 days
	AND LN80.LC_STA_LON80 = 'A'
	AND LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 = 'R' 
	AND LN54.PM_BBS_PGM = 'U36'
	AND LN54.LC_STA_LN54 = 'A'
	AND LN10.LF_LON_CUR_OWN = '828476' --UHEAA OWNER CODE (NOT A BANA, ALIGN, OR TILP LOAN) 
	AND BL10.LC_BIL_TYP = 'P'
	AND BL10.LC_STA_BIL10 = 'A'
	AND BL10.LC_IND_BIL_SNT IN 
	(
		'1', /*normal paper bill is printed and sent to borrower*/
		'2', /*reprint of normal bill*/
		'4', /*use due date minus current date > = 5 days (insufficient lead time)*/
		'6', /*bad address*/
		'7', /*paid ahead bill*/
		'G', /*when a c bill is printed (a c bill = eft bill printed and sent to borrower without an amount due.  1st time notice and letter will not be extracted)*/
		'A', /*ACH and/or ebill bill-not printed*/
		'B', /*ACH and/or ebill bill not printed < 15 days notice*/
		'C', /*ACH and/or ebill 1st notice not printed*/
		'D', /*ACH and/or ebill insufficient lead time not printed*/
		'E', /*monthly ACH and/or ebill bill not printed*/
		'F', /*ACH and/or ebill bill printed not sent<15 days notice*/
		'H', /*ACH and/or ebill insufficient lead time not printed*/
		'I', /*monthly ACH and/or ebill bill not printed*/
		'J', /*<15 days reprint request not printed*/
		'K', /*ACH and/or ebill 1st notice reprint rqst not printd*/
		'L', /*ACH and/or ebill insufficient lead time rpq not prntd*/
		'M', /*monthly ACH and/or ebill bill reprint rqst not printd*/
		'P', /*ACH and/or ebill reprint request not printed*/
		'Q', /*ACH and/or ebill paid ahead reprint reqst not printed*/
		'R', /*ACH and/or ebill evaluated by late fees process*/
		'8' /*inactive loans included in bill*/
	)
	AND CAST(LN80.LD_BIL_DU_LON AS DATE) <= CAST(GETDATE() AS DATE)
	AND LN10.LF_LON_CUR_OWN = '828476' --UHEAA OWNER CODE (NOT A BANA, ALIGN, OR TILP LOAN) 
	AND ISNULL(LN80.LI_BIL_DLQ_OVR_RIR,'') NOT IN ('Y','O','P')

UNION ALL --Allow for Non forgiving forbs to also count as fatal strikes if delq > 30

SELECT DISTINCT
	LN10.BF_SSN,
	LN10.LN_SEQ,
	NULL,
	NULL,
	FatalInactive.FatalStrikes AS FatalStrike	
FROM
	UDW..LN10_LON LN10
	INNER JOIN UDW..LN54_LON_BBS LN54 
		ON LN54.BF_SSN = LN10.BF_SSN 
		AND LN54.LN_SEQ = LN10.LN_SEQ
		AND LN54.LC_STA_LN54 = 'A'
		AND LN54.LC_BBS_ELG = 'N' --LOAN IS STILL ELIGIBLE FOR THE BB
	INNER JOIN UDW..LN55_LON_BBS_TIR LN55 
		ON LN55.BF_SSN = LN54.BF_SSN 
		AND LN55.LN_SEQ = LN54.LN_SEQ
		AND LN55.PM_BBS_PGM = LN54.PM_BBS_PGM
		AND LN55.PN_BBS_PGM_SEQ = LN54.PN_BBS_PGM_SEQ
		--AND LN55.LN_LON_BBS_SEQ = LN54.LN_LON_BBS_PGM_SEQ
		AND LN55.LC_STA_LN55 = 'A'
		AND LN55.LC_LON_BBT_STA = 'D' --BORR IS WORKING TOWARDS RIR (R=ReductionAchieved, Q=WorkingTowardsQual, D=Disqual, N=NotElg)
	INNER JOIN --Grabbing inactive bills that were inactivated by a NON forgiving forb to count as part of fatal strikes
	(
		SELECT DISTINCT
			InactiveBills.BF_SSN,
			InactiveBills.LN_SEQ,
			InactiveBills.LD_BIL_CRT,
			InactiveBills.LN_SEQ_BIL_WI_DTE,
			InactiveBills.LN_BIL_OCC_SEQ,
			InactiveBills.LI_BIL_DLQ_OVR_RIR,
			SUM(
				CASE WHEN InactiveBillStrikes.AppliedDate < InactiveBills.LD_BIL_DU_LON THEN 0 --Applied Forb prior to bill due date. Treat as non delq
					 ELSE 
						CASE WHEN InactiveBillStrikes.AppliedDate >= InactiveBills.LD_BIL_DU_LON THEN 
								CASE WHEN DATEDIFF(DAY,InactiveBills.LD_BIL_DU_LON, InactiveBillStrikes.AppliedDate) > 15 THEN 1 --Fatal strike applied date
									 ELSE 0
								END
							 ELSE 
								CASE WHEN DATEDIFF(DAY,InactiveBills.LD_BIL_DU_LON, ISNULL(InactiveBills.LD_BIL_STS_RIR_TOL,GETDATE())) > 15 THEN 1 --Fatal strike satisfy date
								     ELSE 0
								END
						END
				END) OVER(PARTITION BY InactiveBills.BF_SSN, InactiveBills.LN_SEQ, InactiveBills.LD_BIL_DU_LON) AS FatalStrikes
		FROM
		(
			SELECT DISTINCT
				LN80.BF_SSN,
				LN80.LN_SEQ,
				LN80.LN_SEQ_BIL_WI_DTE,
				LN80.LD_BIL_DU_LON,
				LN80.LN_BIL_OCC_SEQ,
				LN80.LD_BIL_CRT,
				LN80.LD_BIL_STS_RIR_TOL,
				LN80.LI_BIL_DLQ_OVR_RIR
			FROM
			(
				SELECT
					LN80.BF_SSN,
					LN80.LN_SEQ,
					LN80.LN_SEQ_BIL_WI_DTE,
					LN80.LD_BIL_DU_LON,
					MAX(LN80.LN_BIL_OCC_SEQ) AS LN_BIL_OCC_SEQ,
					LN80.LD_BIL_CRT
				FROM
					UDW..LN80_LON_BIL_CRF LN80
					INNER JOIN
					(
						SELECT
							LN80.BF_SSN,
							LN80.LN_SEQ,
							LN_SEQ_BIL_WI_DTE,
							LD_BIL_DU_LON,
							MAX(LD_BIL_CRT) AS LD_BIL_CRT
						FROM
							UDW..LN80_LON_BIL_CRF LN80
							INNER JOIN UDW..LN54_LON_BBS LN54 
								ON LN54.BF_SSN = LN80.BF_SSN 
								AND LN54.LN_SEQ = LN80.LN_SEQ
								AND LN54.LC_STA_LN54 = 'A'
								AND LN54.LC_BBS_ELG = 'N' --LOAN IS STILL ELIGIBLE FOR THE BB
							INNER JOIN UDW..LN55_LON_BBS_TIR LN55 
								ON LN55.BF_SSN = LN54.BF_SSN 
								AND LN55.LN_SEQ = LN54.LN_SEQ
								AND LN55.PM_BBS_PGM = LN54.PM_BBS_PGM
								AND LN55.PN_BBS_PGM_SEQ = LN54.PN_BBS_PGM_SEQ
								--AND LN55.LN_LON_BBS_SEQ = LN54.LN_LON_BBS_PGM_SEQ
								AND LN55.LC_STA_LN55 = 'A'
								AND LN55.LC_LON_BBT_STA = 'D' 
						WHERE
							CAST(LD_BIL_DU_LON AS DATE) <= CAST(GETDATE() AS DATE)
							AND LC_BIL_TYP_LON = 'P'
							AND LC_STA_LON80 = 'I' 
						GROUP BY
							LN80.BF_SSN,
							LN80.LN_SEQ,
							LN_SEQ_BIL_WI_DTE,
							LD_BIL_DU_LON
					) MaxBillCRT
						ON MaxBillCRT.BF_SSN = LN80.BF_SSN
						AND MaxBillCRT.LN_SEQ = LN80.LN_SEQ
						AND MaxBillCRT.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
						AND MaxBillCRT.LD_BIL_DU_LON = LN80.LD_BIL_DU_LON
						AND MaxBillCRT.LD_BIL_CRT = LN80.LD_BIL_CRT
				GROUP BY
					LN80.BF_SSN,
					LN80.LN_SEQ,
					LN80.LN_SEQ_BIL_WI_DTE,
					LN80.LD_BIL_DU_LON,
					LN80.LD_BIL_CRT
			) MaxLN80
			INNER JOIN UDW..LN80_LON_BIL_CRF LN80
				ON LN80.BF_SSN = MaxLN80.BF_SSN
				AND LN80.LN_SEQ = MaxLN80.LN_SEQ
				AND LN80.LN_SEQ_BIL_WI_DTE = MaxLN80.LN_SEQ_BIL_WI_DTE
				AND LN80.LD_BIL_DU_LON = MaxLN80.LD_BIL_DU_LON
				AND LN80.LD_BIL_CRT = MaxLN80.LD_BIL_CRT
				AND LN80.LN_BIL_OCC_SEQ = MaxLN80.LN_BIL_OCC_SEQ
		) InactiveBills
		INNER JOIN
		(
			SELECT DISTINCT
				FB10.BF_SSN,
				LN60.LN_SEQ,
				CAST(LN60.LD_FOR_BEG AS DATE) AS StartDate,
				CAST(LN60.LD_FOR_END AS DATE) AS EndDate,
				CAST(LN60.LD_FOR_APL AS DATE) AS AppliedDate
			FROM	
				UDW..FB10_BR_FOR_REQ FB10
				INNER JOIN UDW..LN60_BR_FOR_APV LN60
					ON LN60.BF_SSN = FB10.BF_SSN
					AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
				INNER JOIN UDW..LN10_LON LN10
					ON LN10.BF_SSN = LN60.BF_SSN
					AND LN10.LN_SEQ = LN60.LN_SEQ
			WHERE
				FB10.LC_FOR_STA = 'A'
				AND LN60.LC_STA_LON60 = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
				AND LN60.LC_FOR_RSP != '003'
				AND FB10.LC_FOR_TYP NOT IN ('01','04','07','10','13','18','20','25','26','39','40','41','44') --all forbs not in this list are "non-forgiving"
				AND ISNULL(FB10.LD_FOR_INF_CER,LN10.LD_LON_ACL_ADD) >= '2006-01-20'
		) InactiveBillStrikes
			ON InactiveBillStrikes.BF_SSN = InactiveBills.BF_SSN
			AND InactiveBillStrikes.LN_SEQ = InactiveBills.LN_SEQ
			AND InactiveBills.LD_BIL_DU_LON BETWEEN InactiveBillStrikes.StartDate AND InactiveBillStrikes.EndDate
	) FatalInactive
		ON FatalInactive.BF_SSN = LN10.BF_SSN
		AND FatalInactive.LN_SEQ = LN10.LN_SEQ
	INNER JOIN UDW..BL10_BR_BIL BL10
		ON BL10.BF_SSN = FatalInactive.BF_SSN
		AND BL10.LN_SEQ_BIL_WI_DTE = FatalInactive.LN_SEQ_BIL_WI_DTE
		AND BL10.LD_BIL_CRT = FatalInactive.LD_BIL_CRT
	LEFT JOIN ULS.bbreininsc._CoveredStrikes CS
		ON CS.BF_SSN = FatalInactive.BF_SSN
		AND CS.LN_SEQ = FatalInactive.LN_SEQ
		AND CS.LD_BIL_CRT = FatalInactive.LD_BIL_CRT
		AND CS.LN_SEQ_BIL_WI_DTE = FatalInactive.LN_SEQ_BIL_WI_DTE
		AND CS.LN_BIL_OCC_SEQ = FatalInactive.LN_BIL_OCC_SEQ
WHERE
	LN10.LA_CUR_PRI > 0.00
	AND LN10.LC_STA_LON10 = 'R' 
	AND LN54.PM_BBS_PGM = 'U36'
	AND LN54.LC_STA_LN54 = 'A'
	AND LN10.LF_LON_CUR_OWN = '828476' --UHEAA OWNER CODE (NOT A BANA, ALIGN, OR TILP LOAN) 
	AND ISNULL(FatalInactive.LI_BIL_DLQ_OVR_RIR,'') NOT IN ('Y','O','P')
	AND FatalInactive.FatalStrikes > 0
	AND BL10.LC_BIL_TYP = 'P'
	--AND BL10.LC_STA_BIL10 = 'I'
	AND BL10.LC_IND_BIL_SNT IN 
	(
		'1', /*normal paper bill is printed and sent to borrower*/
		'2', /*reprint of normal bill*/
		'4', /*use due date minus current date > = 5 days (insufficient lead time)*/
		'6', /*bad address*/
		'7', /*paid ahead bill*/
		'G', /*when a c bill is printed (a c bill = eft bill printed and sent to borrower without an amount due.  1st time notice and letter will not be extracted)*/
		'A', /*ACH and/or ebill bill-not printed*/
		'B', /*ACH and/or ebill bill not printed < 15 days notice*/
		'C', /*ACH and/or ebill 1st notice not printed*/
		'D', /*ACH and/or ebill insufficient lead time not printed*/
		'E', /*monthly ACH and/or ebill bill not printed*/
		'F', /*ACH and/or ebill bill printed not sent<15 days notice*/
		'H', /*ACH and/or ebill insufficient lead time not printed*/
		'I', /*monthly ACH and/or ebill bill not printed*/
		'J', /*<15 days reprint request not printed*/
		'K', /*ACH and/or ebill 1st notice reprint rqst not printd*/
		'L', /*ACH and/or ebill insufficient lead time rpq not prntd*/
		'M', /*monthly ACH and/or ebill bill reprint rqst not printd*/
		'P', /*ACH and/or ebill reprint request not printed*/
		'Q', /*ACH and/or ebill paid ahead reprint reqst not printed*/
		'R', /*ACH and/or ebill evaluated by late fees process*/
		'8' /*inactive loans included in bill*/
	)

--If loan has 4 or more strikes, dont insert anything into reinstate table, otherwise insert the defer/forb covered bills
INSERT INTO ULS.bbreininsc.ReinstatementProcessing(BF_SSN, LN_SEQ, LD_BIL_DU, RecordType, CreatedAt, CreatedBy, DeletedAt, DeletedBy)
SELECT DISTINCT
	CS.BF_SSN,
	CS.LN_SEQ,
	CAST(CS.LD_BIL_DU_LON AS DATE),
	'U36',
	@Today,
	'BBREININSC U36 load',
	NULL,
	NULL
FROM
	ULS.bbreininsc._CoveredStrikes CS
	LEFT JOIN
	(
		SELECT
			LSC.BF_SSN,
			LSC.LN_SEQ,
			MAX(LSC.FatalStrike) AS FatalStrike
		FROM
			ULS.bbreininsc._LoanStrikeCount LSC
		GROUP BY
			LSC.BF_SSN,
			LSC.LN_SEQ
	) LSC
		ON LSC.BF_SSN = CS.BF_SSN
		AND LSC.LN_SEQ = CS.LN_SEQ
	LEFT JOIN ULS.bbreininsc.ReinstatementProcessing RP
		ON RP.BF_SSN = CS.BF_SSN
		AND RP.LN_SEQ = CS.LN_SEQ
		AND RP.LD_BIL_DU = CAST(CS.LD_BIL_DU_LON AS DATE)
		AND RP.RecordType = 'U36'
		AND CAST(RP.CreatedAt AS DATE) = @Today
WHERE
	RP.BF_SSN IS NULL --Dont readd
	AND 
	(
		LSC.BF_SSN IS NULL --No additional strikes
		OR LSC.FatalStrike = 0 --Forces any borrower with a delq bill greater than 31 days to auto fail reinstatement
	)

TRUNCATE TABLE ULS.bbreininsc._CoveredStrikes;
TRUNCATE TABLE ULS.bbreininsc._LoanStrikeCount;
END