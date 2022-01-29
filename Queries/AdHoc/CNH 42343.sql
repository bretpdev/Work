/****** Script for SelectTopNRows command from SSMS  ******/



SELECT 
	PDXX.DF_SPE_ACC_ID AS ACCOUNT_NUMBER,
	M_MEND.LN_SEQ,
	LNXX.IC_LON_PGM,
	LKXX.PX_DSC_MDM AS CURRENT_STATUS,
	LNXX.LA_CUR_PRI AS LOAN_BALANCE,
	M_MEND.WA_TOT_BRI_OTS AS MAY_EOM_OUTSTANDING_INTEREST,
	A_MEND.WA_TOT_BRI_OTS  AS APRIL_EOM_OUTSTANDING_INTEREST
FROM 
	[AUDITCDW].[DBO].[DWXX_DW_CLC_CLU_MAYXXXX] M_MEND
	INNER JOIN CDW..DWXX_DW_CLC_CLU DWXX
		ON DWXX.BF_SSN = M_MEND.BF_SSN
		AND DWXX.LN_SEQ = M_MEND.LN_SEQ
	INNER JOIN CDW..LKXX_LS_CDE_LKP LKXX
		ON LKXX.PM_ATR = 'WC-DW-LON-STA'
		AND LKXX.PX_ATR_VAL = DWXX.WC_DW_LON_STA
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = DWXX.BF_SSN
		AND LNXX.LN_SEQ = DWXX.LN_SEQ 
	INNER JOIN CDW..PDXX_PRS_NME PDXX
		ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	INNER JOIN 
	 (
		SELECT 
			BF_SSN,
			LN_SEQ,
			WA_TOT_BRI_OTS
		FROM 
			[AUDITCDW].[DBO].[DWXX_DW_CLC_CLU_APRXXXX]
  ) A_MEND ON A_MEND.BF_SSN = M_MEND.BF_SSN
  AND A_MEND.LN_SEQ = M_MEND.LN_SEQ
WHERE
	M_MEND.WA_TOT_BRI_OTS > A_MEND.WA_TOT_BRI_OTS
