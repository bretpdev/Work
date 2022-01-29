DECLARE @BEGIN DATE = 'XXXX-X-X'
DECLARE @END DATE = GETDATE()

SELECT DISTINCT
	LNXX.BF_SSN,
	LNXX.LN_SEQ,
	LNXX.LD_FAT_EFF
FROM
	CDW..LNXX_FIN_ATY LNXX
WHERE
	LNXX.PC_FAT_TYP = 'XX'
	AND 
	LNXX.PC_FAT_SUB_TYP = 'XX'
	AND
	LNXX.LD_FAT_PST BETWEEN @BEGIN AND @END
