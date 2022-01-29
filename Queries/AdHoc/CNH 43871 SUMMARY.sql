SELECT distinct
	MONTH(PP.AddedAt) AS [MONTH],
	YEAR(PP.AddedAt) AS [YEAR],
	COUNT(DISTINCT PP.AccountNumber) AS BWR_COUNT
FROM 
	CLS.[billing].PrintProcessing PP
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_SPE_ACC_ID = PP.AccountNumber
WHERE 
	PP.AddedAt >= 'XX/XX/XXXX'
	AND PP.DeletedAt IS NULL
	and pp.scriptfileid not in (X,XX) 
GROUP BY
	MONTH(PP.AddedAt),
	YEAR(PP.AddedAt) 
ORDER BY
	[MONTH],
	[YEAR]