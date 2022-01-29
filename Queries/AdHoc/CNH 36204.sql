SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED
USE CDW
GO

SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID AS [Acct #]
	,LNXX.LN_SEQ AS [Loan Seq]
	,CASE
		WHEN DWXX.WX_OVR_DW_LON_STA = 'LITIGATION'
		THEN LNXX_MAX.LN_DLQ_MAX
		ELSE ((DATEDIFF(DAY, LNXX_MAX.LD_DLQ_MAX, DWXX.WD_CLC_THU)) + LNXX_MAX.LN_DLQ_MAX)
	END AS [# Days Delinq]
	--,CASE
	--	WHEN LNXX.LA_CUR_PRI > X.XX
	--	THEN '>X'
	--	ELSE NULL
	--END AS LA_CUR_PRI
	--,LNXX.LC_STA_LONXX
	--,DWXX.WX_OVR_DW_LON_STA
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
ORDER BY
	PDXX.DF_SPE_ACC_ID
	,LNXX.LN_SEQ