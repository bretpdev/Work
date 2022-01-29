USE CDW
GO
SELECT
	*
FROM
	CDW..LNXX_INT_RTE_HST LNXX
WHERE
	LNXX.LC_INT_RDC_PGM = 'M'
	AND LR_ITR = X
	AND 
	(
		LD_ITR_EFF_BEG BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
		OR
		LD_ITR_EFF_END BETWEEN 'XX/XX/XXXX' AND 'XX/XX/XXXX'
	)