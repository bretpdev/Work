SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID,
	LNXX.LN_SEQ
FROM
	CDW..LNXX_INT_RTE_HST LNXX
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	LEFT JOIN 
	(
		SELECT DISTINCT
			BF_SSN,
			LN_SEQ,
			MIN(LD_ITR_EFF_BEG) AS LD_ITR_EFF_BEG
		FROM
			CDW..LNXX_INT_RTE_HST
		GROUP BY
			BF_SSN,
			LN_SEQ
	) MinLNXX
		ON MinLNXX.BF_SSN = LNXX.BF_SSN
		AND MinLNXX.LN_SEQ = LNXX.LN_SEQ
		AND MinLNXX.LD_ITR_EFF_BEG = LNXX.LD_ITR_EFF_BEG
WHERE
	LNXX.LR_ITR = X.XX
ORDER BY
	PDXX.DF_SPE_ACC_ID