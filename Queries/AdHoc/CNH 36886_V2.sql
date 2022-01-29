use cdw
go

IF OBJECT_ID('tempdb..#RepaymentData', 'U') IS NOT NULL
BEGIN
	DROP TABLE #RepaymentData
END
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	ROW_NUMBER() OVER (PARTITION BY RSXX.BF_SSN, LNXX.LN_SEQ, RSXX.LN_RPS_SEQ, LNXX.LN_GRD_RPS_SEQ ORDER BY RSXX.LN_RPS_SEQ DESC) [REP_SEQ],
	LNXX.BF_SSN,
	LNXX.LN_SEQ,
	RSXX.LN_RPS_SEQ,
	LNXX.LN_GRD_RPS_SEQ,
	LNXX.IC_LON_PGM,
	LNXX.LD_LON_X_DSB,
	LNXX.LA_LON_AMT_GTR,
	LNXX.LA_CUR_PRI,
	LNXX.LC_TYP_SCH_DIS,
	LNXX.LA_TOT_RPD_DIS,
	LNXX.LA_ANT_CAP,
	RSXX.LD_RPS_X_PAY_DU,
	CAST(ISNULL(LNXX.LN_RPS_TRM, X) AS INT) [LN_RPS_TRM],
	LNXX.LA_RPS_ISL,
	CAST(RSXX.LD_RPS_X_PAY_DU AS DATE) [TermStartDate],
	CAST(X AS INT) [TermsToDate]
INTO
	#RepaymentData
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..LNXX_LON LNXX ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN CDW..RSXX_BR_RPD RSXX ON RSXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN CDW..LNXX_LON_RPS LNXX ON LNXX.BF_SSN = LNXX.BF_SSN AND LNXX.LN_SEQ = LNXX.LN_SEQ AND LNXX.LN_RPS_SEQ = RSXX.LN_RPS_SEQ
	INNER JOIN CDW..LNXX_LON_RPS_SPF LNXX ON LNXX.BF_SSN = LNXX.BF_SSN AND LNXX.LN_SEQ = LNXX.LN_SEQ AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ AND RSXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ	
	inner join cdw..CNH_XXXXX$ c on c.bf_ssn = lnXX.bf_ssn and c.ln_seq = lnXX.ln_seq 
WHERE
	LNXX.LC_STA_LONXX = 'R'

ORDER BY
	LNXX.BF_SSN,
	LNXX.LN_SEQ


;WITH SUMS
AS
(
	SELECT
		BF_SSN,
		LN_SEQ, 
		LN_GRD_RPS_SEQ,
		LN_RPS_TRM, 
		LN_RPS_SEQ,
		TermsToDate = RD.LN_RPS_TRM -- start with first term
	FROM
		#RepaymentData RD
	WHERE
		LN_GRD_RPS_SEQ = X -- start with sequence X
		AND
		REP_SEQ = X -- newest repayment schedule
	
	UNION ALL
	
	SELECT
		RD.BF_SSN,
		RD.LN_SEQ, 
		RD.LN_GRD_RPS_SEQ,
		RD.LN_RPS_TRM, 
		rd.LN_RPS_SEQ,
		S.TermsToDate + RD.LN_RPS_TRM -- add terms for current record to running total
	FROM
		SUMS S
		INNER JOIN #RepaymentData RD 
			ON RD.LN_GRD_RPS_SEQ = S.LN_GRD_RPS_SEQ + X 
			AND RD.BF_SSN = S.BF_SSN 
			AND RD.LN_SEQ = S.LN_SEQ 
			AND RD.REP_SEQ = X -- newest repayment schedule
			and rd.LN_RPS_SEQ = s.LN_RPS_SEQ
)
UPDATE
	RD
SET
	RD.TermsToDate = S.TermsToDate,
	RD.TermStartDate = DATEADD(MONTH, S.TermsToDate - RD.LN_RPS_TRM, RD.TermStartDate)
FROM 
	SUMS S
	INNER JOIN #RepaymentData RD ON RD.LN_GRD_RPS_SEQ = S.LN_GRD_RPS_SEQ AND RD.BF_SSN = S.BF_SSN AND RD.LN_SEQ = S.LN_SEQ AND RD.REP_SEQ = X and rd.LN_RPS_SEQ = s.LN_RPS_SEQ




	select distinct
		[DF_SPE_ACC_ID]
      ,c.[LN_SEQ]
      ,[WC_DW_LON_STA]
      ,[LC_IND_BIL_SNT]
      ,[LD_BIL_CRT]
      ,[LN_DLQ_MAX]
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
	
	 , LNXXEOM.LA_CUR_PRI
	  ,LNXX.LR_ITR,
	   rd.LA_RPS_ISL,
	  day(m.LD_RPS_X_PAY_DU) as "due date",
	  m.LD_RPS_X_PAY_DU
	--,  BILL.PAID_AHEAD
	--,PMT.PMT_AMT
	--,CASE WHEN PMT.PMT_AMT >= rd.LA_RPS_ISL THEN 'Y' ELSE 'N' END AS PMY_SAT
	into cdw.dbo.CNH_XXXXX_PAYMENT_runX
	from
		cdw..CNH_XXXXX$ c
		inner join AuditCDW..LNXX_LON_decXXXX LNXXEOM
			ON LNXXEOM.BF_SSN = C.BF_SSN
			AND  LNXXEOM.LN_SEQ = C.LN_SEQ
		inner join #RepaymentData RD
			 on c.bf_ssn = rd.bf_ssn and c.ln_seq = rd.ln_seq 
		inner join
		(
			select
				RD.bf_ssn,
				rd.ln_seq,
				rd.ld_rps_X_pay_du,
				rd.termStartDate,
				MAX(rd.LN_RPS_SEQ) As LN_RPS_SEQ
			from #RepaymentData rd
			INNER JOIN
			(
				select	
					RD.bf_ssn,
					rd.ln_seq,
					rd.LN_RPS_SEQ,
					cast(max(termStartDate) as date) as termStartDate
				from
					#RepaymentData rd
					inner join
					(
						SELECT
							RD.bf_ssn,
							rd.ln_seq,
							max(rd.LN_RPS_SEQ) as LN_RPS_SEQ
							--cast(max(ld_rps_X_pay_du) as date) as ld_rps_X_pay_du
						FROM
							#RepaymentData RD
						WHERE
								ld_rps_X_pay_du < 'XX/XX/XXXX'
						group by
							RD.bf_ssn,
							rd.ln_seq
					)m
						on m.BF_SSN = rd.BF_SSN
						and m.LN_SEQ = rd.LN_SEQ
						and m.LN_RPS_SEQ  = rd.LN_RPS_SEQ
					where 
					termStartDate < 'XX/XX/XXXX'
				group by
					RD.bf_ssn,
					rd.ln_seq,
					rd.LN_RPS_SEQ
			) m
				on m.BF_SSN = rd.BF_SSN
				and m.LN_SEQ = rd.LN_SEQ
				and m.LN_RPS_SEQ  = rd.LN_RPS_SEQ
				and m.termStartDate  = rd.TermStartDate
		group by
			RD.bf_ssn,
				rd.ln_seq,
				rd.ld_rps_X_pay_du,
				rd.termStartDate
		)m
			on m.BF_SSN = rd.BF_SSN
			and m.LN_SEQ = rd.LN_SEQ
			--and m.ld_rps_X_pay_du  = rd.LD_RPS_X_PAY_DU
			and m.termStartDate  = rd.TermStartDate
			and m.LN_RPS_SEQ = rd.LN_RPS_SEQ
		INNER JOIN
		(
		SELECT
				LNXX.BF_SSN,
				LNXX.LN_SEQ,
				LNXX.LR_ITR,
				ROW_NUMBER() OVER (PARTITION BY LNXX.BF_SSN, LNXX.LN_SEQ ORDER BY LD_STA_LONXX DESC) AS SEQ
			FROM	
				CDW..LNXX_INT_RTE_HST LNXX
				INNER JOIN CDW..PDXX_PRS_NME PDXX ON PDXX.DF_PRS_ID = LNXX.BF_SSN
			WHERE
				LNXX.LC_STA_LONXX = 'A'
				AND
				GETDATE() BETWEEN LNXX.LD_ITR_EFF_BEG AND LNXX.LD_ITR_EFF_END

		) LNXX 
			ON C.BF_SSN = LNXX.BF_SSN	
			AND C.LN_SEQ = LNXX.LN_SEQ 
			AND LNXX.SEQ = X
--		INNER JOIN
--		(
--			SELECT
--				CASE WHEN LD_BIL_DU_LON > 'XX/XX/XXXX' THEN 'Y' ELSE 'N' END AS PAID_AHEAD,
--				LNXX.BF_SSN,
--				LNXX.LN_SEQ
--			FROM CDW..CNH_XXXXX$ C
--			INNER JOIN
--			(
--					SELECT DISTINCT
--						LNXX.BF_SSN,
--						LNXX.LN_SEQ,
--						MAX(LNXX.LD_BIL_DU_LON) AS LD_BIL_DU_LON,
--						MAX(LNXX.LD_BIL_CRT) AS LD_BIL_CRT,
--						MAX(LN_SEQ_BIL_WI_DTE) AS LN_SEQ_BIL_WI_DTE,
--						MAX(LN_BIL_OCC_SEQ) AS LN_BIL_OCC_SEQ
--					FROM
--						CDW..LNXX_LON_BIL_CRF LNXX
--						INNER JOIN CDW..CNH_XXXXX$ C
--							ON C.BF_SSN = LNXX.BF_SSN
--							AND C.LN_SEQ = LNXX.LN_SEQ
--					WHERE
--						LNXX.LC_STA_LONXX = 'A'
--					GROUP BY 
--						LNXX.BF_SSN,
--						LNXX.LN_SEQ
--				) LNXX
--					ON LNXX.BF_SSN = C.BF_SSN
--					AND LNXX.LN_SEQ = C.LN_SEQ
--		) BILL
--			 ON BILL.BF_SSN = C.BF_SSN
--			 AND BILL.LN_SEQ = C.LN_SEQ
--	LEFT JOIN
--(
--SELECT DISTINCT
--	lnXX.bf_ssn,
--	LNXX.LN_SEQ,
--	SUM(ABS(ISNULL(LA_FAT_NSI,X)) + ABS(ISNULL(LA_FAT_CUR_PRI,X))) AS PMT_AMT
--	--rmXX.*
--FROM
--	CDW..CNH_XXXXX$ C
--	INNER JOIN CDW..LNXX_FIN_ATY LNXX
--		ON LNXX.BF_SSN = C.BF_SSN
--		AND LNXX.LN_SEQ = C.LN_SEQ
--	INNER JOIN CDW..LNXX_LON_PAY_FAT LNXX
--		ON LNXX.BF_SSN = LNXX.BF_SSN
--		AND LNXX.LN_SEQ = LNXX.LN_SEQ
--		AND LNXX.LN_FAT_SEQ  = LNXX.LN_FAT_SEQ 
--	INNER JOIN CDW..RMXX_RMT_BCH RMXX
--		ON RMXX.LD_RMT_BCH_INI = LNXX.LD_RMT_BCH_INI
--		AND RMXX.LC_RMT_BCH_SRC_IPT = LNXX.LC_RMT_BCH_SRC_IPT
--		AND RMXX.LN_RMT_BCH_SEQ = LNXX.LN_RMT_BCH_SEQ
--WHERE
--	LNXX.LC_STA_LONXX = 'A'
--	AND COALESCE(LNXX.LC_FAT_REV_REA, '') = ''
--	AND LNXX.PC_FAT_TYP = 'XX'
--	AND RMXX.LC_RMT_BCH_SRC_IPT != 'E'
--	 and lnXX.LD_FAT_EFF >= 'XX/XX/XXXX'
--GROUP BY
--lnXX.bf_ssn,
--	LNXX.LN_SEQ
--) PMT
--	ON PMT.BF_SSN = C.BF_SSN
--	AND PMT.LN_SEQ = C.LN_SEQ

