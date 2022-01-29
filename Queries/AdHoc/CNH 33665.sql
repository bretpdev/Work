USE CDW
GO

DECLARE @StartDate DATE = ''
DECLARE @EndDate DATE = ''
SELECT 
	LNXX.*,
	LNXX.IC_LON_PGM
FROM
	CDW..LNXX_FIN_ATY LNXX
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
WHERE
	LNXX.LD_FAT_APL BETWEEN @StartDate AND @EndDate