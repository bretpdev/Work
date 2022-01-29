SELECT * FROM OPENQUERY(LEGEND,
'
SELECT
	--COUNT(DISTINCT LNXX.BF_SSN) "BORROWER COUNT"
	* --UNCOMMENT TO GET DETAIL
FROM
	PKUB.LNXX_INT_RTE_HST LNXX
WHERE
	LNXX.LC_INT_RDC_PGM IN (''M'', ''S'')
	AND LNXX.LR_ITR = X
	AND LNXX.LC_STA_LONXX = ''A''
	AND 
	(
		(LNXX.LD_ITR_EFF_BEG BETWEEN ''XX/XX/XXXX'' AND ''XX/XX/XXXX'')
		OR
		(LNXX.LD_ITR_EFF_END BETWEEN ''XX/XX/XXXX'' AND ''XX/XX/XXXX'')
	)
')

SELECT * FROM OPENQUERY(LEGEND,
'
SELECT
	--COUNT(DISTINCT LNXX.BF_SSN) "BORROWER COUNT"
	* --UNCOMMENT TO GET DETAIL
FROM
	PKUB.LNXX_INT_RTE_HST LNXX
WHERE
	LNXX.LC_INT_RDC_PGM IN (''M'', ''S'')
	AND LNXX.LR_ITR = X
	AND LNXX.LC_STA_LONXX = ''A''
	AND 
	(
		(LNXX.LD_ITR_EFF_BEG BETWEEN ''XX/XX/XXXX'' AND ''XX/XX/XXXX'')
		OR
		(LNXX.LD_ITR_EFF_END BETWEEN ''XX/XX/XXXX'' AND ''XX/XX/XXXX'')
	)
')