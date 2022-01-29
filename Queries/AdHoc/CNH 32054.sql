USE CDW
GO

SELECT
	LNXX.BF_SSN
FROM
	PDXX_PRS_NME PDXX
	INNER JOIN PDXX_PRS_ADR PDXX ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
	INNER JOIN LNXX_LON LNXX ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN BRXX_BR_EFT BRXX ON BRXX.BF_SSN = PDXX.DF_PRS_ID
	INNER JOIN LNXX_EFT_TO_LON LNXX ON LNXX.BF_SSN = PDXX.DF_PRS_ID AND LNXX.LN_SEQ = LNXX.LN_SEQ
WHERE
	PDXX.DI_VLD_ADR = 'N'
	AND
	LNXX.LC_STA_LNXX = 'A'
	AND
	BRXX.BC_EFT_STA = 'A'
	AND
	LNXX.LC_STA_LONXX = 'R'
	AND
	LNXX.LA_CUR_PRI > X.XX