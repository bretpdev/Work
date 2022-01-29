USE CDW
GO

IF OBJECT_ID('tempdb..#Data', 'U') IS NOT NULL
BEGIN
	DROP TABLE #Data
END

IF OBJECT_ID('tempdb..#DataX', 'U') IS NOT NULL
BEGIN
	DROP TABLE #DataX
END
	SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
	 SELECT * into #Data  FROM (
	SELECT DISTINCT
		pdXX.DF_SPE_ACC_ID,
		lnXX.LN_SEQ,
		S.*
	FROM
		CDW..PDXX_PRS_NME PDXX
		INNER JOIN CDW..LNXX_LON LNXX
			ON PDXX.DF_PRS_ID = LNXX.BF_SSN
		INNER JOIN  [CDW].[dbo].[brXXxxx$] S 
			ON S.BF_SSN = PDXX.DF_PRS_ID
		INNER JOIN CDW..BRXX_BR_EFT BRXX
			ON BRXX.BF_SSN = S.BF_SSN
			AND BRXX.BN_EFT_SEQ = S.BN_EFT_SEQ
		INNER JOIN CDW..LNXX_EFT_TO_LON LNXX
			ON S.BF_SSN = LNXX.BF_SSN	
			AND S.BN_EFT_SEQ = LNXX.BN_EFT_SEQ
			and LNXX.LN_SEQ = LNXX.LN_SEQ

	
	
		) P





		
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
	inner join cdw..#Data c on c.bf_ssn = lnXX.bf_ssn and c.ln_seq = lnXX.ln_seq 
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
	   ,rd.LA_RPS_ISL
	  ,day(m.LD_RPS_X_PAY_DU) as "due date"
	 , m.LD_RPS_X_PAY_DU
	into #dataX
	from
		cdw..#Data c
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


select DF_SPE_ACC_ID, BF_SSN, ln_seq, "due date"  from #dataX where "due date" in (X,X)
