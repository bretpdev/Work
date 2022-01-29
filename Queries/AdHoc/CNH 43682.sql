USE CDW
GO


SELECT DISTINCT
	LNXX.BF_SSN,
	MAX(LD_FAT_EFF) AS TRANSFER_DATE
FROM
	CDW..LNXX_FIN_ATY LNXX
	INNER JOIN CDW..CNHXXXXX CNH
		ON CNH.SSN = LNXX.BF_SSN
WHERE
	LNXX.LC_STA_LONXX = 'A'
	AND COALESCE(LNXX.LC_FAT_REV_REA,'') = ''
	AND LNXX.PC_FAT_TYP = 'XX'
	AND LNXX.PC_FAT_SUB_TYP = 'XX'
	AND LNXX.LD_FAT_EFF > 'XX/XX/XXXX'
GROUP BY
	BF_SSN
