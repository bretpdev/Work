SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT * FROM
(
SELECT  [DF_SPE_ACC_ID]
      ,c.[LN_SEQ]
      ,[WC_DW_LON_STA]
      ,[LC_IND_BIL_SNT]
      ,C.[LD_BIL_CRT]
      ,ISnull(LNXX.LN_DLQ_MAX,X) AS LN_DLQ_MAX
      ,c.[BF_SSN]
      ,[BN_EFT_SEQ]
      ,[BF_EFT_ABA]
      ,[BF_EFT_ACC]
      ,[BC_EFT_TYP_ACC]
      ,[BC_EFT_STA]
      ,[BD_EFT_STA]
      ,[BF_LST_DTS_BRXX]
      ,[BD_EFT_PNO_SNT]
      ,[BA_EFT_ADD_WDR]
      ,[BN_EFT_NSF_CTR]
      ,[BN_EFT_DAY_DUE]
      ,[BA_EFT_LST_WDR]
      ,[BA_EFT_TOL]
      ,[BC_EFT_DNL_REA]
      ,[DF_PRS_ID]
      ,[BC_EFT_PAY_OPT]
      ,[BC_SRC_DIR_DBT_APL]
      ,[BC_DDT_PAY_PRS_TYP]
      ,[BF_DDT_PAY_EDS]
      ,[LA_CUR_PRI]
      ,[LR_ITR]
      ,[LA_RPS_ISL]
      ,[due date]
      ,[LD_RPS_X_PAY_DU]
	  ,  CASE WHEN BILL.LD_NXT_PAY_DUE_AHD IS NOT NULL AND MONTH(LD_NXT_PAY_DUE_AHD) != X THEN 'Y' ELSE 'N' END AS PAID_AHEAD
	  ,DAY(PMT.LD_FAT_EFF) AS PAY_EFF_DTE
	,PMT.PMT_AMT
	,CASE WHEN PMT.PMT_AMT >= c.LA_RPS_ISL THEN 'Y' ELSE 'N' END AS PMY_SAT
	,ISNULL(LNXX.LN_DLQ_MAX,X) AS DAYS_PAST_DUE_REAL
	,PMT_BEFORE.PMT_AMT AS BEFORE_DUE_DATE_PMT
	,DAY(PMT_BEFORE.LD_FAT_EFF) AS BEFORE_PMT_DATE
	--,ISnull(LNXX.LN_DLQ_MAX,X) AS CUR_LN_DEL_MAX
  FROM [CDW].[dbo].CNH_XXXXX_PAYMENT_runX C
 -- 		inner JOIN
	--	(
	--		SELECT
	--	CASE 
	--		WHEN LD_BIL_DU_LON > 'XX/XX/XXXX' THEN 'Y' 
	--		WHEN LD_BIL_DU_LON > 'XX/XX/XXXX' and lnXX.ld_bil_crt < 'XX/XX/XXXX' then 'Y'
	--		WHEN JAN_PH.LA_BIL_DU_PRT = X THEN 'Y'
	--		ELSE 'N' END AS PAID_AHEAD,
	--	LNXX.BF_SSN,
	--	LNXX.LN_SEQ
	--FROM CDW..CNH_XXXXX_PAYMENT_runX C
	--INNER JOIN
	--(
	--		SELECT DISTINCT
	--			LNXX.BF_SSN,
	--			LNXX.LN_SEQ,
	--			MAX(LNXX.LD_BIL_DU_LON) AS LD_BIL_DU_LON,
	--			MAX(LNXX.LD_BIL_CRT) AS LD_BIL_CRT,
	--			MAX(LN_SEQ_BIL_WI_DTE) AS LN_SEQ_BIL_WI_DTE,
	--			MAX(LN_BIL_OCC_SEQ) AS LN_BIL_OCC_SEQ
	--		FROM
	--			CDW..LNXX_LON_BIL_CRF LNXX
	--			INNER JOIN CDW..CNH_XXXXX_PAYMENT_runX C
	--				ON C.BF_SSN = LNXX.BF_SSN
	--				AND C.LN_SEQ = LNXX.LN_SEQ
	--		WHERE
	--			LNXX.LC_STA_LONXX = 'A'
	--		GROUP BY 
	--			LNXX.BF_SSN,
	--			LNXX.LN_SEQ
	--	) LNXX
	--		ON LNXX.BF_SSN = C.BF_SSN
	--		AND LNXX.LN_SEQ = C.LN_SEQ
	--	LEFT JOIN
	--	(
	--	SELECT
	--		LNXX.BF_SSN,
	--		lnXX.ln_seq,
	--		lnXX.LA_BIL_DU_PRT
	--		FROM
	--			CDW..LNXX_LON_BIL_CRF LNXX
	--			INNER JOIN
	--			(
	--				SELECT DISTINCT
	--					LNXX.BF_SSN,
	--					LNXX.LN_SEQ,
	--					MAX(LNXX.LD_BIL_DU_LON) AS LD_BIL_DU_LON,
	--					MAX(LNXX.LD_BIL_CRT) AS LD_BIL_CRT,
	--					MAX(LN_SEQ_BIL_WI_DTE) AS LN_SEQ_BIL_WI_DTE,
	--					MAX(LN_BIL_OCC_SEQ) AS LN_BIL_OCC_SEQ
	--				FROM
	--					CDW..LNXX_LON_BIL_CRF LNXX
	--					INNER JOIN CDW..CNH_XXXXX_PAYMENT_runX C
	--						ON C.BF_SSN = LNXX.BF_SSN
	--						AND C.LN_SEQ = LNXX.LN_SEQ
	--				WHERE
	--					LNXX.LC_STA_LONXX = 'A'
	--					and MONTH(LNXX.LD_BIL_DU_LON) = X
	--					AND YEAR(LNXX.LD_BIL_DU_LON) = XXXX
	--				GROUP BY 
	--					LNXX.BF_SSN,
	--					LNXX.LN_SEQ
	--			)MLNXX
	--				ON MLNXX.BF_SSN = LNXX.BF_SSN
	--				AND MLNXX.LN_SEQ = LNXX.LN_SEQ
	--				AND MLNXX.LD_BIL_DU_LON = LNXX.LD_BIL_DU_LON
	--				AND MLNXX.LD_BIL_CRT = LNXX.LD_BIL_CRT
	--				AND MLNXX.LN_SEQ_BIL_WI_DTE = LNXX.LN_SEQ_BIL_WI_DTE
	--				AND MLNXX.LN_BIL_OCC_SEQ = LNXX.LN_BIL_OCC_SEQ

	--	) JAN_PH
	--		ON JAN_PH.BF_SSN = C.BF_SSN
	--		AND JAN_PH.LN_SEQ = C.LN_SEQ
	--	) BILL
	--		 ON BILL.BF_SSN = C.BF_SSN
	--		 AND BILL.LN_SEQ = C.LN_SEQ
	LEFT JOIN
	(
		SELECT distinct
			BLXX.BF_SSN,
			LNXX.LN_SEQ,
			MAX(BLXX.LD_BIL_CRT) AS LD_BIL_CRT,
			MAX(BLXX.LN_SEQ_BIL_WI_DTE) AS LN_SEQ_BIL_WI_DTE,
			MAX(LD_NXT_PAY_DUE_AHD) AS LD_NXT_PAY_DUE_AHD
		FROM
			CDW..BLXX_BR_BIL BLXX
			INNER JOIN CDW..LNXX_LON_BIL_CRF LNXX
				ON LNXX.BF_SSN = BLXX.BF_SSN
				AND LNXX.LD_BIL_CRT = BLXX.LD_BIL_CRT
			INNER JOIN CDW..CNH_XXXXX_PAYMENT_runX C
				ON C.BF_SSN = LNXX.BF_SSN
				AND C.LN_SEQ = LNXX.LN_SEQ
		WHERE
			BLXX.LC_STA_BILXX = 'A'
			AND LNXX.LC_STA_LONXX = 'A'
			AND MONTH(BLXX.LD_BIL_DU) = X
			AND YEAR(BLXX.LD_BIL_DU) = XXXX
		GROUP BY
			BLXX.BF_SSN,
			LNXX.LN_SEQ
	) BILL
		 ON BILL.BF_SSN = C.BF_SSN
		 AND BILL.LN_SEQ = C.LN_SEQ
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
			lnXX.bf_ssn,
			LNXX.LN_SEQ,
			lnXX.LD_FAT_EFF,
			ABS(ISNULL(LA_FAT_NSI,X)) + ABS(ISNULL(LA_FAT_CUR_PRI,X)) AS PMT_AMT
			--XX.*
		FROM
			CDW..CNH_XXXXX_PAYMENT_runX C
			INNER JOIN CDW..LNXX_FIN_ATY LNXX
				ON LNXX.BF_SSN = C.BF_SSN
				AND LNXX.LN_SEQ = C.LN_SEQ
			INNER JOIN CDW..LNXX_LON_PAY_FAT LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LN_FAT_SEQ  = LNXX.LN_FAT_SEQ 
			INNER JOIN CDW..RMXX_RMT_BCH RMXX
				ON RMXX.LD_RMT_BCH_INI = LNXX.LD_RMT_BCH_INI
				AND RMXX.LC_RMT_BCH_SRC_IPT = LNXX.LC_RMT_BCH_SRC_IPT
				AND RMXX.LN_RMT_BCH_SEQ = LNXX.LN_RMT_BCH_SEQ
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND COALESCE(LNXX.LC_FAT_REV_REA, '') = ''
			AND LNXX.PC_FAT_TYP = 'XX'
			AND RMXX.LC_RMT_BCH_SRC_IPT != 'E'
			and lnXX.LD_FAT_EFF >= cast('XX/' + cast(c.[due date] as varchar(X)) + '/XXXX' as date)

		) P
		group by
			p.bf_ssn,
			p.LN_SEQ
	) PMT
		ON PMT.BF_SSN = C.BF_SSN
		AND PMT.LN_SEQ = C.LN_SEQ
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
			lnXX.bf_ssn,
			LNXX.LN_SEQ,
			lnXX.LD_FAT_EFF,
			ABS(ISNULL(LA_FAT_NSI,X)) + ABS(ISNULL(LA_FAT_CUR_PRI,X)) AS PMT_AMT
			--XX.*
		FROM
			CDW..CNH_XXXXX_PAYMENT_runX C
			INNER JOIN CDW..LNXX_FIN_ATY LNXX
				ON LNXX.BF_SSN = C.BF_SSN
				AND LNXX.LN_SEQ = C.LN_SEQ
			INNER JOIN CDW..LNXX_LON_PAY_FAT LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LN_FAT_SEQ  = LNXX.LN_FAT_SEQ 
			INNER JOIN CDW..RMXX_RMT_BCH RMXX
				ON RMXX.LD_RMT_BCH_INI = LNXX.LD_RMT_BCH_INI
				AND RMXX.LC_RMT_BCH_SRC_IPT = LNXX.LC_RMT_BCH_SRC_IPT
				AND RMXX.LN_RMT_BCH_SEQ = LNXX.LN_RMT_BCH_SEQ
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND COALESCE(LNXX.LC_FAT_REV_REA, '') = ''
			AND LNXX.PC_FAT_TYP = 'XX'
			AND RMXX.LC_RMT_BCH_SRC_IPT != 'E'
			and lnXX.LD_FAT_EFF between  cast('XX/' + cast((c.[due date] + X) as varchar(X)) + '/XXXX' as date) AND cast('XX/' + cast(c.[due date] as varchar(X)) + '/XXXX' as date)

		) P
		group by
			p.bf_ssn,
			p.LN_SEQ
	) PMT_BEFORE
		ON PMT_BEFORE.BF_SSN = C.BF_SSN
		AND PMT_BEFORE.LN_SEQ = C.LN_SEQ
	LEFT JOIN CDW..LNXX_LON_DLQ_HST LNXX
		ON LNXX.BF_SSN = C.BF_SSN
		AND LNXX.LN_SEQ = C.LN_SEQ
		AND LNXX.LC_STA_LONXX = 'X'
) POP
where POP.LN_DLQ_MAX = X and pop.PMT_AMT is null AND pop.PAID_AHEAD = 'N' AND POP.BEFORE_DUE_DATE_PMT IS  NULL AND POP.LA_RPS_ISL > X
ORDER BY 
POP.BF_SSN,
POP.LN_SEQ

		
