SELECT
	LN10.BF_SSN,
	LN10.LN_SEQ,
	CASE WHEN Forb.BF_SSN IS NOT NULL THEN 'Forb'
	     WHEN Defer.BF_SSN IS NOT NULL THEN 'Defer'
		 ELSE ''
	END AS [Type],
	CASE WHEN Forb.BF_SSN IS NOT NULL THEN Forb.DateOfDenial
	     WHEN Defer.BF_SSN IS NOT NULL THEN Defer.DateOfDenial
		 ELSE ''
	END AS [DateOfDenial]
FROM
	UDW..LN10_LON LN10
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = LN10.BF_SSN
	LEFT JOIN
	(
		SELECT DISTINCT
			LN60.BF_SSN,
			LN60.LN_SEQ,
			FB10.LD_CRT_REQ_FOR,
			CAST(LN60.LF_LST_DTS_LN60 AS DATE) AS DateOfDenial
		FROM
			UDW..LN60_BR_FOR_APV LN60
			INNER JOIN UDW..FB10_BR_FOR_REQ FB10
				ON FB10.BF_SSN = LN60.BF_SSN
				AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
		WHERE
			LN60.LC_FOR_RSP = '003' --Denied
			AND CAST(LN60.LF_LST_DTS_LN60 AS DATE) >= '2020-07-01'
	) Forb
		ON Forb.BF_SSN = LN10.BF_SSN
		AND Forb.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN
	(
		SELECT DISTINCT
			LN50.BF_SSN,
			LN50.LN_SEQ,
			DF10.LD_CRT_REQ_DFR,
			CAST(LN50.LF_LST_DTS_LN50 AS DATE) AS DateOfDenial
		FROM
			UDW..LN50_BR_DFR_APV LN50
			INNER JOIN UDW..DF10_BR_DFR_REQ DF10
				ON DF10.BF_SSN = LN50.BF_SSN
				AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
		WHERE
			LN50.LC_DFR_RSP = '003' --Denied
			AND CAST(LN50.LF_LST_DTS_LN50 AS DATE) >= '2020-07-01'

	) Defer
		ON Defer.BF_SSN = LN10.BF_SSN
		AND Defer.LN_SEQ = LN10.LN_SEQ
WHERE
	Defer.BF_SSN IS NOT NULL 
	OR Forb.BF_SSN IS NOT NULL
		