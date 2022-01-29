SELECT DISTINCT
	T2.BF_SSN
FROM
	CDW..CS_Transfer3 T2
	INNER JOIN CDW..AY10_BR_LON_ATY AY10
		ON AY10.BF_SSN = T2.BF_SSN
	LEFT JOIN
	(
		SELECT DISTINCT 
			FB10.BF_SSN
		FROM
			CDW..FB10_BR_FOR_REQ FB10
			INNER JOIN CDW..LN60_BR_FOR_APV LN60
				ON LN60.BF_SSN = FB10.BF_SSN
				AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
			INNER JOIN CDW..LN10_LON LN10
				ON LN10.BF_SSN = LN60.BF_SSN
				AND LN10.LN_SEQ = LN60.LN_SEQ
				AND LN10.LA_CUR_PRI > 0
				AND LN10.LC_STA_LON10 = 'R'
		WHERE
			LN60.LC_STA_LON60 = 'A'
			AND FB10.LC_STA_FOR10 = 'A'
			AND FB10.LC_FOR_STA = 'A' --denied records cant have this active
			AND LN60.LC_FOR_RSP != '003'
			AND FB10.LC_FOR_TYP = '41'
			AND CAST(GETDATE() AS DATE) BETWEEN CAST(LN60.LD_FOR_BEG AS DATE) AND CAST(LN60.LD_FOR_END AS DATE)
	)FORB
		ON FORB.BF_SSN = T2.BF_SSN
WHERE
	T2.DIDTRANSFER = 1
	AND AY10.PF_REQ_ACT = 'CVDOO'
	AND AY10.LC_STA_ACTY10 = 'A'
	AND FORB.BF_SSN IS NULL