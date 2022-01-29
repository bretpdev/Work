USE CDW
GO

DECLARE @StartDate DATE = 'XXXX-XX-XX'
DECLARE @EndDate DATE = 'XXXX-XX-XX'
	
SELECT DISTINCT	
	LNXX.BF_SSN,
	LNXX.LN_SEQ,
	CASE 
		WHEN LNXX.PC_FAT_SUB_TYP = 'XX' THEN 'PSLF'
		WHEN LNXX.PC_FAT_SUB_TYP = 'XX' THEN 'TPD'
		WHEN LNXX.PC_FAT_SUB_TYP = 'XX' THEN 'DMCS'
		ELSE ''
	END AS TRANSFER_TYPE
FROM	
	CDW..LNXX_FIN_ATY LNXX
WHERE	
	LNXX.PC_FAT_TYP = 'XX'
	AND 
	LNXX.PC_FAT_SUB_TYP IN ('XX','XX','XX')
	AND
	LNXX.LD_FAT_PST BETWEEN @StartDate AND @EndDate
