SELECT *
FROM
	CDW..LNXX_FIN_ATY
WHERE
	PC_FAT_TYP = 'XX'
	AND PC_FAT_SUB_TYP = 'XX'
	AND LD_FAT_EFF BETWEEN 'XXXX-XX-XX' AND 'XXXX-XX-XX'