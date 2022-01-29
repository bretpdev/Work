USE CDW
GO

SELECT
	*
FROM
	CDW..LN90_FIN_ATY
WHERE
	PC_FAT_TYP = '54'
	AND PC_FAT_SUB_TYP = '01'
	AND LC_STA_LON90 = 'A'
	AND ISNULL(LC_FAT_REV_REA,'') = ''
	AND LD_FAT_PST BETWEEN '05/11/2020' AND '05/13/2020'