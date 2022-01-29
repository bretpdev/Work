SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_RPS_SEQ, 
	LNXX.LR_INT_RPD_DIS,
	LNXX.LC_TYP_SCH_DIS,
	lnXX.LD_CRT_LONXX,
	RSXX.LD_RPS_X_PAY_DU
FROM
	CDW..LNXX_LON_RPS LNXX
	INNER JOIN CDW..RSXX_BR_RPD RSXX
		ON RSXX.BF_SSN = LNXX.BF_SSN
		AND RSXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
WHERE
	LC_STA_LONXX = 'A' 
	AND LNXX.LR_INT_RPD_DIS = X