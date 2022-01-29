use UDW
go

drop table UDW.dbo.UNH_60369_PAYMENT_run2

IF OBJECT_ID('tempdb..#RepaymentData', 'U') IS NOT NULL
BEGIN
	DROP TABLE #RepaymentData
END
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
SELECT
	ROW_NUMBER() OVER (PARTITION BY RS10.BF_SSN, LN10.LN_SEQ, RS10.LN_RPS_SEQ, LN66.LN_GRD_RPS_SEQ ORDER BY RS10.LN_RPS_SEQ DESC) [REP_SEQ],
	LN10.BF_SSN,
	LN10.LN_SEQ,
	RS10.LN_RPS_SEQ,
	LN66.LN_GRD_RPS_SEQ,
	LN10.IC_LON_PGM,
	LN10.LD_LON_1_DSB,
	LN10.LA_LON_AMT_GTR,
	LN10.LA_CUR_PRI,
	LN65.LC_TYP_SCH_DIS,
	LN65.LA_TOT_RPD_DIS,
	LN65.LA_ANT_CAP,
	RS10.LD_RPS_1_PAY_DU,
	CAST(ISNULL(LN66.LN_RPS_TRM, 0) AS INT) [LN_RPS_TRM],
	LN66.LA_RPS_ISL,
	CAST(RS10.LD_RPS_1_PAY_DU AS DATE) [TermStartDate],
	CAST(0 AS INT) [TermsToDate]
INTO
	#RepaymentData
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..LN10_LON LN10 ON LN10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN UDW..RS10_BR_RPD RS10 ON RS10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN UDW..LN65_LON_RPS LN65 ON LN65.BF_SSN = LN10.BF_SSN AND LN65.LN_SEQ = LN10.LN_SEQ AND LN65.LN_RPS_SEQ = RS10.LN_RPS_SEQ
	INNER JOIN UDW..LN66_LON_RPS_SPF LN66 ON LN66.BF_SSN = LN65.BF_SSN AND LN66.LN_SEQ = LN65.LN_SEQ AND LN66.LN_RPS_SEQ = LN65.LN_RPS_SEQ AND RS10.LN_RPS_SEQ = LN65.LN_RPS_SEQ	
	inner join UDW..[UNH_60369 base pop] c on c.bf_ssn = ln10.bf_ssn and c.ln_seq = ln10.ln_seq 
WHERE
	LN10.LC_STA_LON10 = 'R'

ORDER BY
	LN10.BF_SSN,
	LN10.LN_SEQ


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
		LN_GRD_RPS_SEQ = 1 -- start with sequence 1
		AND
		REP_SEQ = 1 -- newest repayment schedule
	
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
			ON RD.LN_GRD_RPS_SEQ = S.LN_GRD_RPS_SEQ + 1 
			AND RD.BF_SSN = S.BF_SSN 
			AND RD.LN_SEQ = S.LN_SEQ 
			AND RD.REP_SEQ = 1 -- newest repayment schedule
			and rd.LN_RPS_SEQ = s.LN_RPS_SEQ
)
UPDATE
	RD
SET
	RD.TermsToDate = S.TermsToDate,
	RD.TermStartDate = DATEADD(MONTH, S.TermsToDate - RD.LN_RPS_TRM, RD.TermStartDate)
FROM 
	SUMS S
	INNER JOIN #RepaymentData RD ON RD.LN_GRD_RPS_SEQ = S.LN_GRD_RPS_SEQ AND RD.BF_SSN = S.BF_SSN AND RD.LN_SEQ = S.LN_SEQ AND RD.REP_SEQ = 1 and rd.LN_RPS_SEQ = s.LN_RPS_SEQ




	select distinct
		[DF_SPE_ACC_ID]
      ,c.[LN_SEQ]
	  ,[WC_DW_LON_STA]
      ,[LC_IND_BIL_SNT]
      ,[LD_BIL_CRT]
      ,c.[BF_SSN]
      --,[BN_EFT_SEQ]
      --,[BF_EFT_ABA]
      --,[BF_EFT_ACC]
      --,[BC_EFT_TYP_ACC]
      --,[BC_EFT_STA]
      --,[BD_EFT_STA]
      --,[BF_LST_DTS_BR30]
      --,[BD_EFT_PNO_SNT]
      --,[BA_EFT_ADD_WDR]
      --,[BN_EFT_NSF_CTR]
      --,[BN_EFT_DAY_DUE]
      --,[BA_EFT_LST_WDR]
      --,[BA_EFT_TOL]
      --,[BC_EFT_DNL_REA]
      --,[DF_PRS_ID]
      --,[BC_EFT_PAY_OPT]
      --,[BC_SRC_DIR_DBT_APL]
      --,[BC_DDT_PAY_PRS_TYP]
      --,[BF_DDT_PAY_EDS]
	
	 , LN10EOM.LA_CUR_PRI
	  ,LN72.LR_ITR,
	   rd.LA_RPS_ISL,
	  day(m.LD_RPS_1_PAY_DU) as "due date",
	  m.LD_RPS_1_PAY_DU
	--,  BILL.PAID_AHEAD
	--,PMT.PMT_AMT
	--,CASE WHEN PMT.PMT_AMT >= rd.LA_RPS_ISL THEN 'Y' ELSE 'N' END AS PMY_SAT
	into UDW.dbo.UNH_60369_PAYMENT_run2
	from
		UDW..[UNH_60369 base pop] c
		inner join udw..PD10_PRS_NME pd10
			on pd10.DF_PRS_ID = c.BF_SSN
		inner join udw..DW01_DW_CLC_CLU dw01
			on dw01.BF_SSN = c.BF_SSN
			and dw01.LN_SEQ = c.ln_seq
		inner join AuditUDW..LN10_LON_dec2018 LN10EOM
			ON LN10EOM.BF_SSN = C.BF_SSN
			AND  LN10EOM.LN_SEQ = C.LN_SEQ
		inner join #RepaymentData RD
			 on c.bf_ssn = rd.bf_ssn and c.ln_seq = rd.ln_seq 
		inner join
		(
			select
				RD.bf_ssn,
				rd.ln_seq,
				rd.ld_rps_1_pay_du,
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
							--cast(max(ld_rps_1_pay_du) as date) as ld_rps_1_pay_du
						FROM
							#RepaymentData RD
						WHERE
								ld_rps_1_pay_du < '02/14/2019'
						group by
							RD.bf_ssn,
							rd.ln_seq
					)m
						on m.BF_SSN = rd.BF_SSN
						and m.LN_SEQ = rd.LN_SEQ
						and m.LN_RPS_SEQ  = rd.LN_RPS_SEQ
					where 
					termStartDate < '02/14/2019'
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
				rd.ld_rps_1_pay_du,
				rd.termStartDate
		)m
			on m.BF_SSN = rd.BF_SSN
			and m.LN_SEQ = rd.LN_SEQ
			--and m.ld_rps_1_pay_du  = rd.LD_RPS_1_PAY_DU
			and m.termStartDate  = rd.TermStartDate
			and m.LN_RPS_SEQ = rd.LN_RPS_SEQ
		INNER JOIN
		(
		SELECT
				LN72.BF_SSN,
				LN72.LN_SEQ,
				LN72.LR_ITR,
				ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ ORDER BY LD_STA_LON72 DESC) AS SEQ
			FROM	
				UDW..LN72_INT_RTE_HST LN72
				INNER JOIN UDW..PD10_PRS_NME PD10 ON PD10.DF_PRS_ID = LN72.BF_SSN
			WHERE
				LN72.LC_STA_LON72 = 'A'
				AND
				GETDATE() BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END

		) LN72 
			ON C.BF_SSN = LN72.BF_SSN	
			AND C.LN_SEQ = LN72.LN_SEQ 
			AND LN72.SEQ = 1
--		INNER JOIN
--		(
--			SELECT
--				CASE WHEN LD_BIL_DU_LON > '03/01/2019' THEN 'Y' ELSE 'N' END AS PAID_AHEAD,
--				LN80.BF_SSN,
--				LN80.LN_SEQ
--			FROM UDW..CNH_36886$ C
--			INNER JOIN
--			(
--					SELECT DISTINCT
--						LN80.BF_SSN,
--						LN80.LN_SEQ,
--						MAX(LN80.LD_BIL_DU_LON) AS LD_BIL_DU_LON,
--						MAX(LN80.LD_BIL_CRT) AS LD_BIL_CRT,
--						MAX(LN_SEQ_BIL_WI_DTE) AS LN_SEQ_BIL_WI_DTE,
--						MAX(LN_BIL_OCC_SEQ) AS LN_BIL_OCC_SEQ
--					FROM
--						UDW..LN80_LON_BIL_CRF LN80
--						INNER JOIN UDW..CNH_36886$ C
--							ON C.BF_SSN = LN80.BF_SSN
--							AND C.LN_SEQ = LN80.LN_SEQ
--					WHERE
--						LN80.LC_STA_LON80 = 'A'
--					GROUP BY 
--						LN80.BF_SSN,
--						LN80.LN_SEQ
--				) LN80
--					ON LN80.BF_SSN = C.BF_SSN
--					AND LN80.LN_SEQ = C.LN_SEQ
--		) BILL
--			 ON BILL.BF_SSN = C.BF_SSN
--			 AND BILL.LN_SEQ = C.LN_SEQ
--	LEFT JOIN
--(
--SELECT DISTINCT
--	ln90.bf_ssn,
--	LN90.LN_SEQ,
--	SUM(ABS(ISNULL(LA_FAT_NSI,0)) + ABS(ISNULL(LA_FAT_CUR_PRI,0))) AS PMT_AMT
--	--rm10.*
--FROM
--	UDW..CNH_36886$ C
--	INNER JOIN UDW..LN90_FIN_ATY LN90
--		ON LN90.BF_SSN = C.BF_SSN
--		AND LN90.LN_SEQ = C.LN_SEQ
--	INNER JOIN UDW..LN94_LON_PAY_FAT LN94
--		ON LN90.BF_SSN = LN94.BF_SSN
--		AND LN90.LN_SEQ = LN94.LN_SEQ
--		AND LN90.LN_FAT_SEQ  = LN94.LN_FAT_SEQ 
--	INNER JOIN UDW..RM10_RMT_BCH RM10
--		ON RM10.LD_RMT_BCH_INI = LN94.LD_RMT_BCH_INI
--		AND RM10.LC_RMT_BCH_SRC_IPT = LN94.LC_RMT_BCH_SRC_IPT
--		AND RM10.LN_RMT_BCH_SEQ = LN94.LN_RMT_BCH_SEQ
--WHERE
--	LN90.LC_STA_LON90 = 'A'
--	AND COALESCE(LN90.LC_FAT_REV_REA, '') = ''
--	AND LN90.PC_FAT_TYP = '10'
--	AND RM10.LC_RMT_BCH_SRC_IPT != 'E'
--	 and ln90.LD_FAT_EFF >= '01/01/2019'
--GROUP BY
--ln90.bf_ssn,
--	LN90.LN_SEQ
--) PMT
--	ON PMT.BF_SSN = C.BF_SSN
--	AND PMT.LN_SEQ = C.LN_SEQ

