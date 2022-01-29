use udw
go

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT * FROM
(
SELECT  [DF_SPE_ACC_ID]
      ,c.[LN_SEQ]
      ,[WC_DW_LON_STA]
      --,c.[LC_IND_BIL_SNT]
      --,C.[LD_BIL_CRT]
      ,ISnull(LN16.LN_DLQ_MAX,0) AS LN_DLQ_MAX
      ,c.[BF_SSN]
   

      ,[LA_CUR_PRI]
      ,[LR_ITR]
      ,[LA_RPS_ISL]
      ,[due date]
      ,[LD_RPS_1_PAY_DU]
	  ,  CASE WHEN BILL.LD_NXT_PAY_DUE_AHD IS NOT NULL AND MONTH(bill.LD_NXT_PAY_DUE_AHD) != 1 THEN 'Y' ELSE 'N' END AS JAN_PAID_AHEAD
	  , CASE WHEN BILLf.LD_NXT_PAY_DUE_AHD IS NOT NULL AND MONTH(billf.LD_NXT_PAY_DUE_AHD) != 2 THEN 'Y' ELSE 'N' END AS FEB_PAID_AHEAD
	  ,DAY(PMTJ.LD_FAT_EFF) AS JAN_PAY_EFF_DTE
	,PMTJ.PMT_AMT AS JAN_PMT_AMT
	,CASE WHEN PMTJ.PMT_AMT >= c.LA_RPS_ISL THEN 'Y' ELSE 'N' END AS JAN_PMY_SAT


	 ,DAY(PMTF.LD_FAT_EFF) AS FEB_PAY_EFF_DTE
	,PMTF.PMT_AMT AS FEB_PMT_AMT
	,CASE WHEN PMTF.PMT_AMT >= c.LA_RPS_ISL THEN 'Y' ELSE 'N' END AS FEB_PMY_SAT

	,ISNULL(LN16.LN_DLQ_MAX,0) AS DAYS_PAST_DUE_REAL
	,PMT_BEFORE.PMT_AMT AS BEFORE_DUE_DATE_PMT
	,DAY(PMT_BEFORE.LD_FAT_EFF) AS BEFORE_PMT_DATE
	--,ISnull(LN16.LN_DLQ_MAX,0) AS CUR_LN_DEL_MAX
  FROM 
	[UDW].[dbo].UNH_60369_PAYMENT_run2 C
	
	
 -- 		inner JOIN
	--	(
	--		SELECT
	--	CASE 
	--		WHEN LD_BIL_DU_LON > '03/01/2019' THEN 'Y' 
	--		WHEN LD_BIL_DU_LON > '02/21/2019' and ln80.ld_bil_crt < '02/01/2019' then 'Y'
	--		WHEN JAN_PH.LA_BIL_DU_PRT = 0 THEN 'Y'
	--		ELSE 'N' END AS PAID_AHEAD,
	--	LN80.BF_SSN,
	--	LN80.LN_SEQ
	--FROM UDW..CNH_36886_PAYMENT_run2 C
	--INNER JOIN
	--(
	--		SELECT DISTINCT
	--			LN80.BF_SSN,
	--			LN80.LN_SEQ,
	--			MAX(LN80.LD_BIL_DU_LON) AS LD_BIL_DU_LON,
	--			MAX(LN80.LD_BIL_CRT) AS LD_BIL_CRT,
	--			MAX(LN_SEQ_BIL_WI_DTE) AS LN_SEQ_BIL_WI_DTE,
	--			MAX(LN_BIL_OCC_SEQ) AS LN_BIL_OCC_SEQ
	--		FROM
	--			UDW..LN80_LON_BIL_CRF LN80
	--			INNER JOIN UDW..CNH_36886_PAYMENT_run2 C
	--				ON C.BF_SSN = LN80.BF_SSN
	--				AND C.LN_SEQ = LN80.LN_SEQ
	--		WHERE
	--			LN80.LC_STA_LON80 = 'A'
	--		GROUP BY 
	--			LN80.BF_SSN,
	--			LN80.LN_SEQ
	--	) LN80
	--		ON LN80.BF_SSN = C.BF_SSN
	--		AND LN80.LN_SEQ = C.LN_SEQ
	--	LEFT JOIN
	--	(
	--	SELECT
	--		LN80.BF_SSN,
	--		ln80.ln_seq,
	--		ln80.LA_BIL_DU_PRT
	--		FROM
	--			UDW..LN80_LON_BIL_CRF LN80
	--			INNER JOIN
	--			(
	--				SELECT DISTINCT
	--					LN80.BF_SSN,
	--					LN80.LN_SEQ,
	--					MAX(LN80.LD_BIL_DU_LON) AS LD_BIL_DU_LON,
	--					MAX(LN80.LD_BIL_CRT) AS LD_BIL_CRT,
	--					MAX(LN_SEQ_BIL_WI_DTE) AS LN_SEQ_BIL_WI_DTE,
	--					MAX(LN_BIL_OCC_SEQ) AS LN_BIL_OCC_SEQ
	--				FROM
	--					UDW..LN80_LON_BIL_CRF LN80
	--					INNER JOIN UDW..CNH_36886_PAYMENT_run2 C
	--						ON C.BF_SSN = LN80.BF_SSN
	--						AND C.LN_SEQ = LN80.LN_SEQ
	--				WHERE
	--					LN80.LC_STA_LON80 = 'A'
	--					and MONTH(LN80.LD_BIL_DU_LON) = 1
	--					AND YEAR(LN80.LD_BIL_DU_LON) = 2019
	--				GROUP BY 
	--					LN80.BF_SSN,
	--					LN80.LN_SEQ
	--			)MLN80
	--				ON MLN80.BF_SSN = LN80.BF_SSN
	--				AND MLN80.LN_SEQ = LN80.LN_SEQ
	--				AND MLN80.LD_BIL_DU_LON = LN80.LD_BIL_DU_LON
	--				AND MLN80.LD_BIL_CRT = LN80.LD_BIL_CRT
	--				AND MLN80.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
	--				AND MLN80.LN_BIL_OCC_SEQ = LN80.LN_BIL_OCC_SEQ

	--	) JAN_PH
	--		ON JAN_PH.BF_SSN = C.BF_SSN
	--		AND JAN_PH.LN_SEQ = C.LN_SEQ
	--	) BILL
	--		 ON BILL.BF_SSN = C.BF_SSN
	--		 AND BILL.LN_SEQ = C.LN_SEQ
	LEFT JOIN
	(
		SELECT distinct
			BL10.BF_SSN,
			LN80.LN_SEQ,
			MAX(BL10.LD_BIL_CRT) AS LD_BIL_CRT,
			MAX(BL10.LN_SEQ_BIL_WI_DTE) AS LN_SEQ_BIL_WI_DTE,
			MAX(LD_NXT_PAY_DUE_AHD) AS LD_NXT_PAY_DUE_AHD
		FROM
			UDW..BL10_BR_BIL BL10
			INNER JOIN UDW..LN80_LON_BIL_CRF LN80
				ON LN80.BF_SSN = BL10.BF_SSN
				AND LN80.LD_BIL_CRT = BL10.LD_BIL_CRT
			INNER JOIN UDW..UNH_60369_PAYMENT_run2 C
				ON C.BF_SSN = LN80.BF_SSN
				AND C.LN_SEQ = LN80.LN_SEQ
		WHERE
			BL10.LC_STA_BIL10 = 'A'
			AND LN80.LC_STA_LON80 = 'A'
			AND MONTH(BL10.LD_BIL_DU) = 1
			AND YEAR(BL10.LD_BIL_DU) = 2019
		GROUP BY
			BL10.BF_SSN,
			LN80.LN_SEQ
	) BILL
		 ON BILL.BF_SSN = C.BF_SSN
		 AND BILL.LN_SEQ = C.LN_SEQ
		LEFT JOIN
	(
		SELECT distinct
			BL10.BF_SSN,
			LN80.LN_SEQ,
			MAX(BL10.LD_BIL_CRT) AS LD_BIL_CRT,
			MAX(BL10.LN_SEQ_BIL_WI_DTE) AS LN_SEQ_BIL_WI_DTE,
			MAX(LD_NXT_PAY_DUE_AHD) AS LD_NXT_PAY_DUE_AHD
		FROM
			UDW..BL10_BR_BIL BL10
			INNER JOIN UDW..LN80_LON_BIL_CRF LN80
				ON LN80.BF_SSN = BL10.BF_SSN
				AND LN80.LD_BIL_CRT = BL10.LD_BIL_CRT
			INNER JOIN UDW..UNH_60369_PAYMENT_run2 C
				ON C.BF_SSN = LN80.BF_SSN
				AND C.LN_SEQ = LN80.LN_SEQ
		WHERE
			BL10.LC_STA_BIL10 = 'A'
			AND LN80.LC_STA_LON80 = 'A'
			AND MONTH(BL10.LD_BIL_DU) = 2
			AND YEAR(BL10.LD_BIL_DU) = 2019
		GROUP BY
			BL10.BF_SSN,
			LN80.LN_SEQ
	) BILLF
		 ON BILLF.BF_SSN = C.BF_SSN
		 AND BILLF.LN_SEQ = C.LN_SEQ
	LEFT JOIN
	(
		SELECT	
			p.bf_ssn,
			p.LN_SEQ,
			max(LD_FAT_EFF) as LD_FAT_EFF,
			SUM(PMT_AMT) AS PMT_AMT
		FROM
		(
		SELECT DISTINCT
			ln90.bf_ssn,
			LN90.LN_SEQ,
			ln90.LD_FAT_EFF,
			ABS(ISNULL(LA_FAT_NSI,0)) + ABS(ISNULL(LA_FAT_CUR_PRI,0)) AS PMT_AMT
			--10.*
		FROM
			UDW..UNH_60369_PAYMENT_run2 C
			INNER JOIN UDW..LN90_FIN_ATY LN90
				ON LN90.BF_SSN = C.BF_SSN
				AND LN90.LN_SEQ = C.LN_SEQ
			INNER JOIN UDW..LN94_LON_PAY_FAT LN94
				ON LN90.BF_SSN = LN94.BF_SSN
				AND LN90.LN_SEQ = LN94.LN_SEQ
				AND LN90.LN_FAT_SEQ  = LN94.LN_FAT_SEQ 
			INNER JOIN UDW..RM10_RMT_BCH RM10
				ON RM10.LD_RMT_BCH_INI = LN94.LD_RMT_BCH_INI
				AND RM10.LC_RMT_BCH_SRC_IPT = LN94.LC_RMT_BCH_SRC_IPT
				AND RM10.LN_RMT_BCH_SEQ = LN94.LN_RMT_BCH_SEQ
		WHERE
			LN90.LC_STA_LON90 = 'A'
			AND COALESCE(LN90.LC_FAT_REV_REA, '') = ''
			AND LN90.PC_FAT_TYP = '10'
			AND RM10.LC_RMT_BCH_SRC_IPT != 'E'
			and ln90.LD_FAT_EFF >= cast('01/' + cast(c.[due date] as varchar(2)) + '/2019' as date)

		) P

		group by
			p.bf_ssn,
			p.LN_SEQ
	) PMTJ
		ON PMTJ.BF_SSN = C.BF_SSN
		AND PMTJ.LN_SEQ = C.LN_SEQ
LEFT JOIN
	(
		SELECT	
			p.bf_ssn,
			p.LN_SEQ,
			max(LD_FAT_EFF) as LD_FAT_EFF,
			SUM(PMT_AMT) AS PMT_AMT
		FROM
		(
		SELECT DISTINCT
			ln90.bf_ssn,
			LN90.LN_SEQ,
			ln90.LD_FAT_EFF,
			ABS(ISNULL(LA_FAT_NSI,0)) + ABS(ISNULL(LA_FAT_CUR_PRI,0)) AS PMT_AMT
			--10.*
		FROM
			UDW..UNH_60369_PAYMENT_run2 C
			INNER JOIN UDW..LN90_FIN_ATY LN90
				ON LN90.BF_SSN = C.BF_SSN
				AND LN90.LN_SEQ = C.LN_SEQ
			INNER JOIN UDW..LN94_LON_PAY_FAT LN94
				ON LN90.BF_SSN = LN94.BF_SSN
				AND LN90.LN_SEQ = LN94.LN_SEQ
				AND LN90.LN_FAT_SEQ  = LN94.LN_FAT_SEQ 
			INNER JOIN UDW..RM10_RMT_BCH RM10
				ON RM10.LD_RMT_BCH_INI = LN94.LD_RMT_BCH_INI
				AND RM10.LC_RMT_BCH_SRC_IPT = LN94.LC_RMT_BCH_SRC_IPT
				AND RM10.LN_RMT_BCH_SEQ = LN94.LN_RMT_BCH_SEQ
		WHERE
			LN90.LC_STA_LON90 = 'A'
			AND COALESCE(LN90.LC_FAT_REV_REA, '') = ''
			AND LN90.PC_FAT_TYP = '10'
			AND RM10.LC_RMT_BCH_SRC_IPT != 'E'
			and ln90.LD_FAT_EFF >= cast('02/' + cast(c.[due date] as varchar(2)) + '/2019' as date)
			and c.[due date] <= 14

		) P

		group by
			p.bf_ssn,
			p.LN_SEQ
	) PMTF
		ON PMTF.BF_SSN = C.BF_SSN
		AND PMTF.LN_SEQ = C.LN_SEQ
		LEFT JOIN
	(
		SELECT	
			p.bf_ssn,
			p.LN_SEQ,
			max(LD_FAT_EFF) as LD_FAT_EFF,
			SUM(PMT_AMT) AS PMT_AMT
		FROM
		(
		SELECT DISTINCT
			ln90.bf_ssn,
			LN90.LN_SEQ,
			ln90.LD_FAT_EFF,
			ABS(ISNULL(LA_FAT_NSI,0)) + ABS(ISNULL(LA_FAT_CUR_PRI,0)) AS PMT_AMT
			--10.*
		FROM
			UDW..UNH_60369_PAYMENT_run2 C
			INNER JOIN UDW..LN90_FIN_ATY LN90
				ON LN90.BF_SSN = C.BF_SSN
				AND LN90.LN_SEQ = C.LN_SEQ
			INNER JOIN UDW..LN94_LON_PAY_FAT LN94
				ON LN90.BF_SSN = LN94.BF_SSN
				AND LN90.LN_SEQ = LN94.LN_SEQ
				AND LN90.LN_FAT_SEQ  = LN94.LN_FAT_SEQ 
			INNER JOIN UDW..RM10_RMT_BCH RM10
				ON RM10.LD_RMT_BCH_INI = LN94.LD_RMT_BCH_INI
				AND RM10.LC_RMT_BCH_SRC_IPT = LN94.LC_RMT_BCH_SRC_IPT
				AND RM10.LN_RMT_BCH_SEQ = LN94.LN_RMT_BCH_SEQ
		WHERE
			LN90.LC_STA_LON90 = 'A'
			AND COALESCE(LN90.LC_FAT_REV_REA, '') = ''
			AND LN90.PC_FAT_TYP = '10'
			AND RM10.LC_RMT_BCH_SRC_IPT != 'E'
			and ln90.LD_FAT_EFF between  cast('12/' + cast((c.[due date] + 1) as varchar(2)) + '/2018' as date) AND cast('01/' + cast(c.[due date] as varchar(2)) + '/2019' as date)

		) P
		group by
			p.bf_ssn,
			p.LN_SEQ
	) PMT_BEFORE
		ON PMT_BEFORE.BF_SSN = C.BF_SSN
		AND PMT_BEFORE.LN_SEQ = C.LN_SEQ
	LEFT JOIN UDW..LN16_LON_DLQ_HST LN16
		ON LN16.BF_SSN = C.BF_SSN
		AND LN16.LN_SEQ = C.LN_SEQ
		AND LN16.LC_STA_LON16 = '1'
) POP
where POP.LN_DLQ_MAX = 0 and (pop.JAN_PMT_AMT is  null or pop.FEB_PMT_AMT is  null)  AND (pop.JAN_PAID_AHEAD = 'Y' or pop.FEB_PAID_AHEAD = 'Y')  --AND POP.BEFORE_DUE_DATE_PMT IS not  NULL --AND POP.LA_RPS_ISL = 0
ORDER BY 
POP.BF_SSN,
POP.LN_SEQ

		
