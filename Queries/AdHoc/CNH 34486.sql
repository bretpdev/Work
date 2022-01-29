USE CDW
GO

SELECT
	*
FROM
	CDW..LNXX_FIN_ATY LNXX
WHERE
	LNXX.PC_FAT_TYP = 'XX'
	AND LNXX.PC_FAT_SUB_TYP IN ('XX','XX','XX')