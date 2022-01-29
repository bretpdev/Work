USE UDW

GO 

SELECT
	COUNT(*) AS LOANS
	,SUM(LNS.BALANCE) AS BALANCE
FROM
	(
		SELECT DISTINCT
			LN10.BF_SSN
			,LN10.LN_SEQ
			,SUM(COALESCE(LN10.LA_CUR_PRI,0) + COALESCE(DW01.WA_TOT_BRI_OTS,0)) AS BALANCE
		FROM 
			PD30_PRS_ADR PD30
			INNER JOIN LN10_LON LN10
				ON PD30.DF_PRS_ID = LN10.BF_SSN
			INNER JOIN DW01_DW_CLC_CLU DW01
				ON LN10.BF_SSN = DW01.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
		WHERE
			PD30.DC_DOM_ST = 'DC'
			AND LN10.LC_STA_LON10 = 'R'
			AND LN10.LA_CUR_PRI > 0.00
		GROUP BY
			LN10.BF_SSN
			,LN10.LN_SEQ
	) LNS

	