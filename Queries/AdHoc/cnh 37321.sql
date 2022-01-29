USE CDW
GO



SELECT DISTINCT
	LN90.BF_SSN
	,LN90.PC_FAT_TYP
	,LN90.PC_FAT_SUB_TYP
	,LN90.LD_FAT_APL
FROM
	LN90_FIN_ATY LN90
WHERE
	LN90.PC_FAT_TYP = '77'
	AND LN90.PC_FAT_SUB_TYP IN ('01','02','03')
	AND LN90.LD_FAT_APL BETWEEN '02/01/2019' AND '02/28/2019'
