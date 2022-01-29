SELECT * FROM OPENQUERY(LEGEND,
'
SELECT 
	--DFXX.LC_DFR_TYP,
	--LNXX.*
	COUNT(DISTINCT DFXX.BF_SSN) "BORROWER COUNT"
FROM
	PKUB.DFXX_BR_DFR_REQ DFXX
	INNER JOIN PKUB.LNXX_BR_DFR_APV LNXX
		ON LNXX.BF_SSN = DFXX.BF_SSN
		AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
WHERE
	DFXX.LC_DFR_TYP IN (''XX'',''XX'')
	AND LNXX.LC_STA_LONXX = ''A''
	AND DFXX.LC_DFR_STA = ''A''
	AND DFXX.LC_STA_DFRXX = ''A''
	AND
	(
		(LNXX.LD_DFR_BEG BETWEEN ''XX/XX/XXXX'' AND ''XX/XX/XXXX'')
		OR
		(LNXX.LD_DFR_END BETWEEN ''XX/XX/XXXX'' AND ''XX/XX/XXXX'')
	)

')

SELECT * FROM OPENQUERY(LEGEND,
'
SELECT 
	--DFXX.LC_DFR_TYP,
	--LNXX.*
	COUNT(DISTINCT DFXX.BF_SSN) "BORROWER COUNT"
FROM
	PKUB.DFXX_BR_DFR_REQ DFXX
	INNER JOIN PKUB.LNXX_BR_DFR_APV LNXX
		ON LNXX.BF_SSN = DFXX.BF_SSN
		AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
WHERE
	DFXX.LC_DFR_TYP IN (''XX'',''XX'')
	AND LNXX.LC_STA_LONXX = ''A''
	AND DFXX.LC_DFR_STA = ''A''
	AND DFXX.LC_STA_DFRXX = ''A''
	AND
	(
		(LNXX.LD_DFR_BEG BETWEEN ''XX/XX/XXXX'' AND ''XX/XX/XXXX'')
		OR
		(LNXX.LD_DFR_END BETWEEN ''XX/XX/XXXX'' AND ''XX/XX/XXXX'')
	)

')