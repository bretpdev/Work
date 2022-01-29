SELECT
	LNXX.BF_SSN,
	LNXX.LN_SEQ,
	'FORBEARANCE XX' AS [TYPE],
	LNXX.LD_FOR_BEG,
	LNXX.LD_FOR_END
FROM
	CDW..FBXX_BR_FOR_REQ FBXX
	INNER JOIN CDW..LNXX_BR_FOR_APV LNXX
		ON LNXX.BF_SSN = FBXX.BF_SSN
		AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
		AND LNXX.LA_CUR_PRI > X
		AND LNXX.LC_STA_LONXX = 'R'
WHERE
	LNXX.LC_STA_LONXX = 'A'
	AND FBXX.LC_STA_FORXX = 'A'
	AND FBXX.LC_FOR_STA = 'A' --denied records cant have this active
	AND LNXX.LC_FOR_RSP != 'XXX'
	AND FBXX.LC_FOR_TYP = 'XX'
	AND LNXX.LD_FOR_BEG >= 'XX/XX/XXXX'

UNION ALL

SELECT
	LNXX.BF_SSN,
	LNXX.LN_SEQ,
	'DEFERMENT XX' AS [TYPE],
	LNXX.LD_DFR_BEG,
	LNXX.LD_DFR_END
FROM
	CDW..DFXX_BR_DFR_REQ DFXX
	INNER JOIN CDW..LNXX_BR_DFR_APV LNXX
		ON LNXX.BF_SSN = DFXX.BF_SSN
		AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
		AND LNXX.LA_CUR_PRI > X
		AND LNXX.LC_STA_LONXX = 'R'
WHERE
	LNXX.LC_STA_LONXX = 'A'
	AND DFXX.LC_STA_DFRXX = 'A'
	AND DFXX.LC_DFR_STA = 'A' --denied records cant have this active
	AND LNXX.LC_DFR_RSP != 'XXX'
	AND DFXX.LC_DFR_TYP = 'XX'
	AND	LNXX.LD_DFR_BEG >= 'XX/XX/XXXX'