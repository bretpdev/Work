SELECT DISTINCT
    PP.PrintProcessingId
    ,PP.AccountNumber
	,CAST(RMXX.LD_RMT_PAY_EFF AS DATE) AS LD_RMT_PAY_EFF
	,RMXX.LA_BR_RMT
	,BLXX.LD_BIL_CRT
	,RMXX.PC_FAT_TYP
	,RMXX.PC_FAT_SUB_TYP
	,RMXX.LC_RMT_BCH_SRC_IPT
	,OnEcorr
FROM 
    CLS.billing.PrintProcessing pp
    INNER JOIN CLS.billing.LineData LD
        ON LD.PrintProcessingId = PP.PrintProcessingId
    INNER JOIN CDW..PDXX_PRS_NME PDXX
        ON PDXX.DF_SPE_ACC_ID = PP.AccountNumber
	INNER JOIN CDW..BLXX_BR_BIL BLXX
		ON PDXX.DF_PRS_ID = BLXX.BF_SSN
	INNER JOIN CDW..RMXX_BR_RMT RMXX
		ON BLXX.BF_SSN = RMXX.BF_SSN
		AND BLXX.LD_BIL_CRT = RMXX.LD_BIL_CRT
		AND BLXX.LN_SEQ_BIL_WI_DTE = RMXX.LN_SEQ_BIL_WI_DTE
WHERE 
    CAST(AddedAt AS DATE) between 'XX/XX/XXXX' and 'XX/XX/XXXX' --WE HAVE SOME BILLS ADDED ON THE XND AND XRD BUT THE BILL CREATE DATE IS THE XND
    AND CLS.dbo.SplitAndRemoveQuotes(LD.LineData, ',', XX, X) = 'XX/XX/XXXX' --grab the bill create date from the file 
    AND OnEcorr = X
    AND DeletedAt IS NULL
	AND RMXX.PC_FAT_TYP = XX
	AND RMXX.PC_FAT_SUB_TYP = XX
	AND RMXX.LD_RMT_PAY_EFF BETWEEN CAST('XX/XX/XXXX' AS DATE) AND CAST('XX/XX/XXXX' AS DATE)
	AND RMXX.LC_RMT_BCH_SRC_IPT = 'S'
