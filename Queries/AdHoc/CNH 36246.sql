--based on CNH XXXXX	
USE CDW	
GO	
	
DECLARE @BEGIN DATE = 'XXXX-XX-XX';	
DECLARE @END DATE = 'XXXX-XX-XX';	
	
SELECT DISTINCT	
	BF_SSN,
	LN_SEQ,
	LD_FAT_EFF
FROM	
	LNXX_FIN_ATY
WHERE	
	LD_FAT_PST BETWEEN @BEGIN AND @END
	AND PC_FAT_TYP = 'XX'
	AND PC_FAT_SUB_TYP = 'XX'
;	
