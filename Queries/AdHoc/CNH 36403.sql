SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
USE CDW

SELECT
	SUM(BorrOverXXX) AS [Borrowers Over XXX],
	SUM(LoansOverXXX) AS [Loans Over XXX],
	SUM(BalanceOfLoansOverXXX) AS [Total $ of Loans Over XXX],
	SUM(BorrRejectedOnce) AS [Borrowers Over XXX Rejected at Least Once],
	SUM(LoansRejectedOnce) AS [Loans Over XXX Rejected at Least Once],
	SUM(BalanceOfLoanRejectedOnce) AS [Total $ of Loans Over XX Rejected at Least Once]
FROM
(
	SELECT DISTINCT
		Summary.[Acct #],
		MAX(Summary.BorrOverXXX) AS BorrOverXXX,
		MAX(Summary.LoansOverXXX) AS LoansOverXXX,
		SUM(Summary.BalanceOfLoansOverXXX) AS BalanceOfLoansOverXXX,
		MAX(Summary.BorrRejectedOnce) AS BorrRejectedOnce,
		MAX(Summary.LoansRejectedOnce) AS LoansRejectedOnce,
		SUM(Summary.BalanceOfLoanRejectedOnce) AS BalanceOfLoanRejectedOnce
	FROM
	(
		SELECT DISTINCT
			DATA.[Acct #],
			SUM(CASE WHEN Data.[# Days Delinq] > XXX THEN X ELSE X END) OVER(PARTITION BY Data.[Acct #]) AS LoansOverXXX,
			SUM(CASE WHEN Data.[# Days Delinq] > XXX THEN X ELSE X END) OVER(PARTITION BY Data.[Acct #], Data.[Loan Seq]) AS BorrOverXXX,
			SUM(CASE WHEN Data.[# Days Delinq] > XXX THEN Data.Balance ELSE X END) OVER(PARTITION BY Data.[Acct #], Data.[Loan Seq]) AS BalanceOfLoansOverXXX,
			SUM(CASE WHEN Data.[# Days Delinq] > XXX AND Data.WX_OVR_DW_LON_STA LIKE '%REJECT%' THEN X ELSE X END) OVER(PARTITION BY Data.[Acct #]) AS LoansRejectedOnce,
			SUM(CASE WHEN Data.[# Days Delinq] > XXX AND Data.WX_OVR_DW_LON_STA LIKE '%REJECT%' THEN X ELSE X END) OVER(PARTITION BY Data.[Acct #], Data.[Loan Seq]) AS BorrRejectedOnce,
			SUM(CASE WHEN Data.[# Days Delinq] > XXX AND Data.WX_OVR_DW_LON_STA LIKE '%REJECT%' THEN Data.Balance ELSE X END) OVER(PARTITION BY Data.[Acct #], Data.[Loan Seq]) AS BalanceOfLoanRejectedOnce
		FROM
		(
			SELECT DISTINCT
				PDXX.DF_SPE_ACC_ID AS [Acct #]
				,LNXX.LN_SEQ AS [Loan Seq]
				,COALESCE(LNXX.LA_CUR_PRI,X.XX) + COALESCE(DWXX.WA_TOT_BRI_OTS,X.XX) AS [Balance]
				,CASE
					WHEN DWXX.WX_OVR_DW_LON_STA = 'LITIGATION'
					THEN LNXX_MAX.LN_DLQ_MAX
					ELSE ((DATEDIFF(DAY, LNXX_MAX.LD_DLQ_MAX, DWXX.WD_CLC_THU)) + LNXX_MAX.LN_DLQ_MAX)
				END AS [# Days Delinq]
				,CASE
					WHEN LNXX.LA_CUR_PRI > X.XX
					THEN '>X'
					ELSE NULL
				END AS LA_CUR_PRI
				,LNXX.LC_STA_LONXX
				,DWXX.WX_OVR_DW_LON_STA
			FROM
				LNXX_LON LNXX
				INNER JOIN 
				(
					SELECT
						BF_SSN
						,LN_SEQ
						,LD_DLQ_MAX
						,MAX(LN_DLQ_MAX)+X AS LN_DLQ_MAX
					FROM
						LNXX_LON_DLQ_HST 
					WHERE
						LN_DLQ_MAX+X > XXX
						AND LC_STA_LONXX = X
					GROUP BY
						BF_SSN
						,LN_SEQ
						,LD_DLQ_MAX
				) LNXX_MAX
					ON LNXX.BF_SSN = LNXX_MAX.BF_SSN
					AND LNXX.LN_SEQ = LNXX_MAX.LN_SEQ
				INNER JOIN PDXX_PRS_NME PDXX
					ON LNXX.BF_SSN = PDXX.DF_PRS_ID
				INNER JOIN DWXX_DW_CLC_CLU DWXX
					ON LNXX.BF_SSN = DWXX.BF_SSN
					AND LNXX.LN_SEQ = DWXX.LN_SEQ
			WHERE
				LNXX.LA_CUR_PRI > X.XX
		) Data
	) Summary
	GROUP BY
		Summary.[Acct #]
) Final