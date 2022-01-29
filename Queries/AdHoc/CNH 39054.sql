USE CDW
GO

SELECT
	PDXX.DF_PRS_ID,
	LTRIM(RTRIM(PDXX.DM_PRS_X)) AS DM_PRS_X,
	LTRIM(RTRIM(PDXX.DM_PRS_LST)) AS DM_PRS_LST,
	FSXX.LN_SEQ,
	(FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(X)),X)) AS AWARD_ID,
	PDXX.DD_BKR_FIL,
	PDXX.DC_BKR_TYP,
	PDXX.DF_COU_DKT,
	PDXX.DM_BKR_CT,
	PDXX.DD_BKR_COR_X_RCV,
	PDXX.IF_IST,
	DC_BKR_STA
FROM 
	CDW..PDXX_PRS_BKR PDXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
	INNER JOIN CDW..FSXX_DL_LON FSXX
		ON FSXX.BF_SSN = PDXX.DF_PRS_ID
WHERE
	PDXX.DC_BKR_ST = 'FL'
	AND PDXX.DD_BKR_POO IS NULL
	AND PDXX.DD_BKR_COR_X_RCV >= 'XX/XX/XXXX'
	AND PDXX.IF_IST = 'IXXXXXXX'
	AND PDXX.DC_BKR_STA = 'XX'
	