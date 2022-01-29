USE CDW

GO

SELECT
	COUNT(*) AS LOANS
	,SUM(LNS.BALANCE) AS BALANCE
FROM
	(
		SELECT DISTINCT
			LNXX.BF_SSN
			,LNXX.LN_SEQ
			,SUM(COALESCE(LNXX.LA_CUR_PRI,X) + COALESCE(DWXX.WA_TOT_BRI_OTS,X)) AS BALANCE
		FROM 
			PDXX_PRS_ADR PDXX
			INNER JOIN LNXX_LON LNXX
				ON PDXX.DF_PRS_ID = LNXX.BF_SSN
			INNER JOIN DWXX_DW_CLC_CLU DWXX
				ON LNXX.BF_SSN = DWXX.BF_SSN
				AND LNXX.LN_SEQ = DWXX.LN_SEQ
		WHERE
			PDXX.DC_DOM_ST = 'DC'
			AND LNXX.LC_STA_LONXX = 'R'
			AND LNXX.LA_CUR_PRI > X.XX
		GROUP BY
			LNXX.BF_SSN
			,LNXX.LN_SEQ
	) LNS

	