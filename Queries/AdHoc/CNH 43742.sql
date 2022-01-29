SELECT DISTINCT
	FSXX.BF_SSN,
	FSXX.LN_SEQ,
	(FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(XX)), X)) AS AwardId,
	LNXX.IC_LON_PGM
FROM
	CDW..LNXX_DSB LNXX
	INNER JOIN CDW..CS_TransferX TX
		ON TX.BF_SSN = LNXX.BF_SSN
	INNER JOIN CDW..FSXX_DL_LON FSXX
		ON FSXX.BF_SSN = LNXX.BF_SSN
		AND FSXX.LN_SEQ = LNXX.LN_SEQ
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = FSXX.BF_SSN
		AND LNXX.LN_SEQ = FSXX.LN_SEQ
WHERE
	LNXX.LC_DSB_TYP = 'X'
	AND LNXX.LD_DSB >= CAST(GETDATE() AS DATE)