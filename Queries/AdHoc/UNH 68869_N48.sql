USE UDW
GO

DROP TABLE IF EXISTS #FINAL;

WITH BILLS AS
(
SELECT DISTINCT
	LN80.BF_SSN,
	LN80.LN_SEQ,
	LN80.LD_BIL_CRT,
	0 AS BILL_SAT,
	LN80.LD_BIL_DU_LON,
	ISNULL(LN16.DELQ_COUNT,0) AS DELQ_COUNT,
	LN10.LD_LON_1_DSB,
	CASE WHEN 
		ISNULL(LEAD(LN80.LD_BIL_DU_LON, 1, 0) OVER (PARTITION BY LN80.BF_SSN, LN80.LN_SEQ ORDER BY LN80.LD_BIL_DU_LON), '01/01/2099') = '1900-01-01 00:00:00.000' THEN '01/01/2099' 
		ELSE ISNULL(LEAD(LN80.LD_BIL_DU_LON, 1, 0) OVER (PARTITION BY LN80.BF_SSN, LN80.LN_SEQ ORDER BY LN80.LD_BIL_DU_LON), '01/01/2099')
	END AS NEXT_DUE,
	LN10.LA_CUR_PRI,
	isnull(ln80.LI_BIL_DLQ_OVR_RIR,'') as LI_BIL_DLQ_OVR_RIR
	--LN54.* 
FROM
	UDW..LN10_LON LN10
	INNER JOIN dbo.LN54_LON_BBS LN54 
		ON LN54.BF_SSN = LN10.BF_SSN 
		AND LN54.LN_SEQ = LN10.LN_SEQ
		AND LN54.LC_STA_LN54 = 'A'
		AND LN54.LC_BBS_ELG != 'Y' --LOAN IS STILL ELIGIBLE FOR THE BB
	INNER JOIN dbo.LN55_LON_BBS_TIR LN55 
		ON LN55.BF_SSN = LN54.BF_SSN 
		AND LN55.LN_SEQ = LN54.LN_SEQ
		AND LN55.PM_BBS_PGM = LN54.PM_BBS_PGM
		AND LN55.PN_BBS_PGM_SEQ = LN54.PN_BBS_PGM_SEQ
		AND LN55.LN_LON_BBS_SEQ = LN54.LN_LON_BBS_PGM_SEQ
		AND LN55.LC_STA_LN55 = 'A'
		AND LN55.LC_LON_BBT_STA = 'D' --BORR IS WORKING TOWARDS RIR (R=ReductionAchieved, Q=WorkingTowardsQual, D=Disqual, N=NotElg)
	INNER JOIN UDW..BL10_BR_BIL BL10
		ON BL10.BF_SSN = LN54.BF_SSN
	INNER JOIN UDW..LN80_LON_BIL_CRF LN80
		ON LN80.BF_SSN = BL10.BF_SSN
		AND LN80.LD_BIL_CRT = BL10.LD_BIL_CRT
		AND LN80.LN_SEQ_BIL_WI_DTE = BL10.LN_SEQ_BIL_WI_DTE
		AND LN80.LN_SEQ = LN10.LN_SEQ
	INNER JOIN
	(
		SELECT DISTINCT
			FB10.BF_SSN,
			LN60.LN_SEQ
		FROM	
			UDW..FB10_BR_FOR_REQ FB10
			INNER JOIN UDW..LN60_BR_FOR_APV LN60
				ON LN60.BF_SSN = FB10.BF_SSN
				AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
		WHERE
			FB10.LC_FOR_STA = 'A'
			AND LN60.LC_STA_LON60 = 'A'
			AND FB10.LC_STA_FOR10 = 'A'
			AND LN60.LC_FOR_RSP != '003'
			AND FB10.LC_FOR_TYP IN ('01','04','07','10','13','18','20','25','26','39','40','41','44')

		UNION ALL
		
		SELECT DISTINCT
			DF10.BF_SSN,
			LN50.LN_SEQ
		FROM	
			UDW..DF10_BR_DFR_REQ DF10
			INNER JOIN UDW..LN50_BR_DFR_APV LN50
				ON LN50.BF_SSN = DF10.BF_SSN
				AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
		WHERE
			DF10.LC_DFR_STA = 'A'
			AND LN50.LC_STA_LON50 = 'A'
			AND DF10.LC_STA_DFR10 = 'A'
			AND LN50.LC_DFR_RSP != '003'
	)FORB
		ON FORB.BF_SSN = LN10.BF_SSN
		AND FORB.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT
		P.BF_SSN,
		P.LN_SEQ,
		COUNT(*) AS DELQ_COUNT
	FROM
	(
			SELECT DISTINCT
				LN16.BF_SSN,
				LN16.LN_SEQ,
				LN16.LD_DLQ_OCC,
				LN16.LN_DLQ_MAX,
				LN16.LD_DLQ_MAX,
				FORB.LD_FOR_APL,
				FORB.LD_FOR_BEG,
				FORB.LD_FOR_END
				--COUNT(*) DELQ_COUNT
			FROM
				 UDW..LN16_LON_DLQ_HST LN16
				LEFT JOIN 
				(
					SELECT DISTINCT
						FB10.BF_SSN,
						LN60.LN_SEQ,
						LN60.LD_FOR_BEG,
						LN60.LD_FOR_END,
						LN60.LD_FOR_APL
					FROM	
						UDW..FB10_BR_FOR_REQ FB10
						INNER JOIN UDW..LN60_BR_FOR_APV LN60
							ON LN60.BF_SSN = FB10.BF_SSN
							AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
					WHERE
						FB10.LC_FOR_STA = 'A'
						AND LN60.LC_STA_LON60 = 'A'
						AND FB10.LC_STA_FOR10 = 'A'
						AND LN60.LC_FOR_RSP != '003'
						AND FB10.LC_FOR_TYP IN ('01','04','07','10','13','18','20','25','26','39','40','41','44')
					UNION ALL

					SELECT DISTINCT
						DF10.BF_SSN,
						LN50.LN_SEQ,
						LN50.LD_DFR_BEG,
						LN50.LD_DFR_END,
						LN50.LD_DFR_APL
					FROM	
						UDW..DF10_BR_DFR_REQ DF10
						INNER JOIN UDW..LN50_BR_DFR_APV LN50
							ON LN50.BF_SSN = DF10.BF_SSN
							AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
					WHERE
						DF10.LC_DFR_STA = 'A'
						AND LN50.LC_STA_LON50 = 'A'
						AND DF10.LC_STA_DFR10 = 'A'
						AND LN50.LC_DFR_RSP != '003'
				)FORB
					ON FORB.BF_SSN = LN16.BF_SSN
					AND FORB.LN_SEQ = LN16.LN_SEQ
					AND --FORB COVERS DELQ PERIOD
					(
						FORB.LD_FOR_BEG BETWEEN LN16.LD_DLQ_OCC AND LN16.LD_DLQ_MAX
						OR
						FORB.LD_FOR_END BETWEEN LN16.LD_DLQ_OCC AND LN16.LD_DLQ_MAX
						OR
						LN16.LD_DLQ_OCC BETWEEN FORB.LD_FOR_BEG AND FORB.LD_FOR_END
					)
			WHERE
				LN16.LN_DLQ_MAX + 1 >= 30
	
) P
GROUP BY
	P.BF_SSN,
	P.LN_SEQ
	
	 ) LN16
		ON LN16.BF_SSN = LN10.BF_SSN
		AND LN16.LN_SEQ = LN10.LN_SEQ
--242576685.01		
WHERE
	LN10.LA_CUR_PRI > 0
	AND LN10.LC_STA_LON10 = 'R' 
	AND LN54.PM_BBS_PGM = 'N48'
	AND LN54.LC_STA_LN54 = 'A'
	--534929526.08
	--AND LN54.BF_SSN = '217139759'
	--AND LN54.LN_SEQ = 9
	AND BL10.LC_BIL_TYP = 'P'
	AND BL10.LC_STA_BIL10 = 'A'
	AND LN80.LD_BIL_DU_LON <= CAST(GETDATE() AS DATE)
	AND LN10.LF_LON_CUR_OWN = '828476' --UHEAA OWNER CODE (NOT A BANA, ALIGN, OR TILP LOAN) 

)
SELECT DISTINCT 
	B.BF_SSN,
	B.LN_SEQ,
	B.LD_BIL_DU_LON,
	B.LD_LON_1_DSB,
	B.NEXT_DUE,
	B.DELQ_COUNT,
	B.LI_BIL_DLQ_OVR_RIR,
	B.LD_BIL_CRT,
	B.BILL_SAT,
	LN16.LD_DLQ_OCC,
	LN16.LD_DLQ_MAX,
	LN16.LN_DLQ_MAX,
	FORB.LD_FOR_APL,
	FORB.LD_FOR_BEG,
	FORB.LD_FOR_END,
	B.LA_CUR_PRI,
	CASE 
		WHEN LD_LON_1_DSB >= '05/01/2006'  THEN 30
		WHEN LD_LON_1_DSB < '05/01/2006' THEN 30
	END AS PGM_DAYS,
	SUM(CASE 
		WHEN LI_BIL_DLQ_OVR_RIR != 'Y' AND LD_FOR_BEG IS NULL AND LN_DLQ_MAX >= CASE 
		WHEN LD_LON_1_DSB >= '05/01/2006'  THEN 30
		WHEN LD_LON_1_DSB < '05/01/2006' THEN 30
	END  THEN 1
		ELSE 0
	END) OVER (PARTITION BY B.BF_SSN, B.LN_SEQ) AS NEW_DELQ_COUNT
INTO #FINAL
FROM 
	BILLS B
	LEFT JOIN UDW..LN16_LON_DLQ_HST LN16
		ON LN16.BF_SSN = B.BF_SSN
		AND LN16.LN_SEQ = B.LN_SEQ
		AND LN16.LN_DLQ_MAX + 1 >= 30
		AND LN16.LD_DLQ_OCC < B.NEXT_DUE
		AND 
		(
			B.LD_BIL_DU_LON BETWEEN LN16.LD_DLQ_OCC AND LN16.LD_DLQ_MAX
			OR
			B.NEXT_DUE BETWEEN LN16.LD_DLQ_OCC AND LN16.LD_DLQ_MAX
		)
		LEFT JOIN --WE ALREADY KNOW THEY HAVE AT LEAST ONE SO WE ARE MAKING IT A LEFT JOIN TO SEE ALL RECORDS STILL
		(
			SELECT DISTINCT
				FB10.BF_SSN,
				LN60.LN_SEQ,
				LN60.LD_FOR_BEG,
				LN60.LD_FOR_END,
				LN60.LD_FOR_APL
			FROM	
				UDW..FB10_BR_FOR_REQ FB10
				INNER JOIN UDW..LN60_BR_FOR_APV LN60
					ON LN60.BF_SSN = FB10.BF_SSN
					AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
			WHERE
				FB10.LC_FOR_STA = 'A'
				AND LN60.LC_STA_LON60 = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
				AND LN60.LC_FOR_RSP != '003'
				AND FB10.LC_FOR_TYP IN ('01','04','07','10','13','18','20','25','26','39','40','41','44')
			UNION ALL

			SELECT DISTINCT
				DF10.BF_SSN,
				LN50.LN_SEQ,
				LN50.LD_DFR_BEG,
				LN50.LD_DFR_END,
				LN50.LD_DFR_APL
			FROM	
				UDW..DF10_BR_DFR_REQ DF10
				INNER JOIN UDW..LN50_BR_DFR_APV LN50
					ON LN50.BF_SSN = DF10.BF_SSN
					AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
			WHERE
				DF10.LC_DFR_STA = 'A'
				AND LN50.LC_STA_LON50 = 'A'
				AND DF10.LC_STA_DFR10 = 'A'
				AND LN50.LC_DFR_RSP != '003'
		)FORB
			ON FORB.BF_SSN = B.BF_SSN
			AND FORB.LN_SEQ = B.LN_SEQ
			AND --FORB COVERS DELQ PERIOD
			(
				FORB.LD_FOR_BEG BETWEEN LN16.LD_DLQ_OCC AND LN16.LD_DLQ_MAX
				OR
				FORB.LD_FOR_END BETWEEN LN16.LD_DLQ_OCC AND LN16.LD_DLQ_MAX
				OR
				LN16.LD_DLQ_OCC BETWEEN FORB.LD_FOR_BEG AND FORB.LD_FOR_END
			)
			--2009-09-01	2010-08-31
WHERE
	LN_DLQ_MAX IS NOT NULL

UPDATE
	F
SET
	BILL_SAT = 1
FROM #FINAL F
INNER JOIN
(
--DETAIL
SELECT * FROM
(
SELECT DISTINCT
	*,
	CASE 
		WHEN isnull(LI_BIL_DLQ_OVR_RIR,'') = 'Y' THEN 0
		--WHEN PGM_DAYS = 30 AND NEW_DELQ_COUNT >= 3 THEN 1
		WHEN datediff(day, ld_dlq_occ, ld_for_beg) >= 30 THEN 1
		WHEN LN_DLQ_MAX >= 30 AND LD_FOR_BEG IS NULL THEN 1
		ELSE 0
	END d
	-- SUM(CASE 
	--	WHEN LI_BIL_DLQ_OVR_RIR = 'Y' THEN 0
	--	WHEN PGM_DAYS = 15 AND NEW_DELQ_COUNT >= 3 THEN 1
	--	WHEN datediff(day, ld_dlq_occ, ld_for_beg) >= 30 THEN 1
	--	ELSE 0
	--END) over (partition by bf_ssn, ln_seq)  AS SHOULD_REINSTATE 
FROM 
	#FINAL
)P
)P
	ON P.BF_SSN = F.BF_SSN
	AND P.LN_SEQ = F.LN_SEQ
	AND P.LD_BIL_CRT = F.LD_BIL_CRT
WHERE
	P.d = 0

DROP TABLE IF EXISTS #FINAL_DATA;
SELECT *, SUM(CASE WHEN M_BILL_SAT = 1 THEN 0 ELSE 1 END) Over (partition by bf_ssn, ln_seq) AS SHOULD_REINSTATE INTO #FINAL_DATA   FROM 
(
SELECT MAX(P.BILL_SAT) OVER (PARTITION BY P.BF_SSN, P.LN_SEQ, P.LD_BIL_CRT) AS M_BILL_SAT, *  FROM
(
SELECT DISTINCT
	*,
	CASE 
		WHEN isnull(LI_BIL_DLQ_OVR_RIR,'') = 'y' THEN 0
		--WHEN PGM_DAYS = 15 AND NEW_DELQ_COUNT >= 3 THEN 1
		WHEN datediff(day, ld_dlq_occ, ld_for_beg) >= 30 THEN 1
		WHEN LN_DLQ_MAX >= 30 AND LD_FOR_BEG IS NULL THEN 1
		ELSE 0
	END d
FROM 
	#FINAL
)P
) FP

--WHERE P.SHOULD_REINSTATE = 0


SELECT DISTINCT * FROM #FINAL_DATA
SELECT DISTINCT BF_SSN, LN_SEQ, LA_CUR_PRI FROM #FINAL_DATA
WHERE SHOULD_REINSTATE = 0