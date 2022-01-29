USE CDW
GO

SELECT 
	LC_WOF_WUP_REA,
	LNXX.*
FROM
	CDW..LNXX_FIN_ATY LNXX
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = LNXX.BF_SSN
		AND LNXX.LN_SEQ = LNXX.LN_SEQ
WHERE
	LNXX.PC_FAT_TYP = 'XX'
	AND LNXX.PC_FAT_SUB_TYP = 'XX'
	AND LNXX.LD_FAT_PST >= 'XX/XX/XXXX'
	AND LNXX.LC_WOF_WUP_REA = 'X'
	AND 
	(LNXX.LC_STA_LONXX != 'A'
	OR ISNULL(LNXX.LC_FAT_REV_REA,'') != ''
	)

