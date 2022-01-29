USE CDW
GO

SELECT DISTINCT
	AYXX.BF_SSN
FROM
	CDW..AYXX_BR_LON_ATY AYXX
	INNER JOIN 
	(
		SELECT DISTINCT 
			LNXX.BF_SSN
		FROM
			CDW..LNXX_FIN_ATY LNXX
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND COALESCE(LNXX.LC_FAT_REV_REA,'') = ''
			AND LNXX.PC_FAT_TYP = 'XX'
			AND LNXX.PC_FAT_SUB_TYP = 'XX'
			AND LNXX.LD_FAT_EFF > ='XX/XX/XXXX'
		
	) Trans
		ON Trans.BF_SSN = AYXX.BF_SSN
	LEFT JOIN
	(
		SELECT DISTINCT 
			FBXX.BF_SSN,
			LNXX.LD_FOR_BEG,
			LNXX.LD_FOR_END
		FROM
			CDW..FBXX_BR_FOR_REQ FBXX
			INNER JOIN CDW..LNXX_BR_FOR_APV LNXX
				ON LNXX.BF_SSN = FBXX.BF_SSN
				AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
			INNER JOIN CDW..LNXX_LON LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LA_CUR_PRI > X
				AND LNXX.LC_STA_LONXX = 'R'
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND FBXX.LC_STA_FORXX = 'A'
			AND FBXX.LC_FOR_STA = 'A' --denied records cant have this active
			AND LNXX.LC_FOR_RSP != 'XXX'
			AND FBXX.LC_FOR_TYP = 'XX'
			AND CAST(GETDATE() AS DATE) BETWEEN CAST(LNXX.LD_FOR_BEG AS DATE) AND CAST(LNXX.LD_FOR_END AS DATE)
	) FORB
		ON FORB.BF_SSN = AYXX.BF_SSN
WHERE
	PF_REQ_ACT = 'CVDOO'
	AND FORB.BF_SSN IS NULL