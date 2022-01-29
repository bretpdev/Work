SELECT DISTINCT 
	LNXX.BF_SSN,
	LNXX.LN_SEQ,
	 --FSXX.LF_FED_AWD + RIGHT('XXX' + CAST(FSXX.LN_FED_AWD_SEQ AS VARCHAR(XX)),X) AS AWARD_ID,
	lnXX.la_cur_pri as outstanding_balance, 
	 EAXX.TransferNumber AS TransferNumber
FROM 
	CDW..LNXX_LON LNXX
	INNER JOIN CDW..FSXX_DL_LON FSXX
		ON FSXX.BF_SSN = LNXX.BF_SSN
		AND FSXX.LN_SEQ = LNXX.LN_SEQ
	left JOIN CDW..CS_Transfer_EAXX EAXX
		ON EAXX.BF_SSN = LNXX.BF_SSN
		and eaXX.TransferNumber in (X,X,X,X)
	
WHERE 
	LNXX.LC_STA_LONXX = 'R' 
	AND LNXX.LA_CUR_PRI < X
	--AND LNXX.BF_SSN = 'XXXXXXXXX'
ORDER BY
	LNXX.BF_SSN,
	LNXX.LN_SEQ