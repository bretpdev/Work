USE CDW
GO
--QUERY X
SELECT
	LNXX.BF_SSN,
	LNXX.LN_SEQ,
	LNXX.LD_DFR_BEG,
	LNXX.LD_DFR_END,
	DATEDIFF(MONTH,LNXX.LD_DFR_BEG,LNXX.LD_DFR_END) AS DEF_LENGTH
FROM
	CDW..DFXX_BR_DFR_REQ DFXX
	INNER JOIN CDW..LNXX_BR_DFR_APV LNXX
		ON LNXX.BF_SSN=DFXX.BF_SSN
		AND LNXX.LF_DFR_CTL_NUM=DFXX.LF_DFR_CTL_NUM
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN=LNXX.BF_SSN
		AND LNXX.LN_SEQ=LNXX.LN_SEQ
WHERE
	DFXX.LC_DFR_TYP = 'XX'
	AND LNXX.LA_CUR_PRI > X.XX
	AND LNXX.LC_STA_LONXX = 'R'
	AND LNXX.LC_STA_LONXX = 'A'
	AND DFXX.LC_DFR_STA = 'A'
	AND LNXX.LD_LON_X_DSB < 'X/X/XXXX'
	AND LNXX.IC_LON_PGM IN ('DLPLUS','DLPLGB')
	AND LNXX.LC_DFR_RSP != 'XXX'

DROP TABLE IF EXISTS #FINAL;

WITH DEF AS 
(
SELECT DISTINCT
	LNXX.BF_SSN,
	pdXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ,
	LNXX.LD_DFR_BEG,
	LNXX.LD_DFR_END,
	DFXX.LC_DFR_TYP,
	DATEDIFF(MONTH,LNXX.LD_DFR_BEG,LNXX.LD_DFR_END) AS DEF_LENGTH
FROM
	CDW..DFXX_BR_DFR_REQ DFXX
	INNER JOIN CDW..LNXX_BR_DFR_APV LNXX
		ON LNXX.BF_SSN=DFXX.BF_SSN
		AND LNXX.LF_DFR_CTL_NUM=DFXX.LF_DFR_CTL_NUM
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN=LNXX.BF_SSN
		AND LNXX.LN_SEQ=LNXX.LN_SEQ
	INNER JOIN CDW..DFXX_BR_DFR_REQ DFXXX
		ON DFXXX.BF_SSN = LNXX.BF_SSN
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	LEFT JOIN CDW..WQXX_TSK_QUE WQXX
		ON WQXX.BF_SSN = LNXX.BF_SSN
		AND WQXX.WF_QUE = 'DR'
		AND WQXX.WC_STA_WQUEXX NOT IN ('X','C')
WHERE
	LNXX.LA_CUR_PRI > X.XX
	AND LNXX.LC_STA_LONXX = 'R'
	AND LNXX.LC_STA_LONXX = 'A'
	AND DFXX.LC_DFR_STA = 'A'
	AND LNXX.LC_DFR_RSP != 'XXX'
	AND WQXX.BF_SSN IS NULL--exclude open DR queues

)
SELECT DISTINCT
	*,
	LEAD(LC_DFR_TYP, X) OVER(PARTITION BY BF_SSN, LN_SEQ ORDER BY LD_DFR_BEG) AS NT,
	lag(LC_DFR_TYP, X) OVER(PARTITION BY BF_SSN, LN_SEQ ORDER BY LD_DFR_BEG) AS LT,
	CASE WHEN LC_DFR_TYP = 'XX' AND LEAD(LC_DFR_TYP, X) OVER(PARTITION BY BF_SSN, LN_SEQ ORDER BY LD_DFR_BEG) = 'XX' THEN X END AS BTB,
	CASE WHEN LC_DFR_TYP = 'XX' AND LAG(LC_DFR_TYP, X) OVER(PARTITION BY BF_SSN, LN_SEQ ORDER BY LD_DFR_BEG) = 'XX' THEN X END AS BTBX
INTO #FINAL
FROM 
	DEF

SELECT
	DF_SPE_ACC_ID,
	--BF_SSN,
	LN_SEQ,
	SUM(DEF_LENGTH) AS DEF_LENGTH
	--*
FROM
	#FINAL
WHERE
	(BTB = X OR BTBX = X)
GROUP BY
	DF_SPE_ACC_ID,
	--BF_SSN,
	LN_SEQ
HAVING 
	SUM(DEF_LENGTH) > X
ORDER BY DF_SPE_ACC_ID, LN_SEQ

;