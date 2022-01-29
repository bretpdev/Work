USE CDW
GO

SELECT
	CNH.Award_id,
	LNXX.LD_LON_X_DSB AS [XST DSB DATE],
	LNXX.IC_LON_PGM AS [LOAN PROGRAM]
FROM
	CDW..FSXX_DL_LON FSXX
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = FSXX.BF_SSN
		AND LNXX.LN_SEQ = FSXX.LN_SEQ
	INNER JOIN CDW..[CNH XXXXX] CNH
		ON CNH.Award_id = FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(LN_FED_AWD_SEQ AS VARCHAR(X)), X)

