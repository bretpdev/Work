USE CDW
GO

SELECT
	*,
	LA_FAT_CUR_PRI + LA_FAT_NSI AS WRITE_OFF_AMOUNT
FROM
	OPENQUERY
	(
		LEGEND,
		'
			SELECT DISTINCT
				PDXX.DF_SPE_ACC_ID
				,TRIM(PDXX.DM_PRS_X) || '' '' || TRIM(PDXX.DM_PRS_LST) AS NAME
				,LNXX.LN_SEQ
				,LNXX.LD_FAT_PST
				,LNXX.LD_FAT_EFF
				,ADXX.LC_WOF_WUP_REA
				,LKXX.PX_DSC_XXL_XC
				,LNXX.LA_FAT_CUR_PRI 
				,LNXX.LA_FAT_NSI
			FROM
				PKUB.PDXX_PRS_NME PDXX
				INNER JOIN PKUB.LNXX_FIN_ATY LNXX
					ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				INNER JOIN PKUB.ADXX_PCV_ATY_ADJ ADXX
					ON PDXX.DF_PRS_ID = ADXX.BF_SSN
				INNER JOIN PKUB.LKXX_LS_CDE_LKP LKXX
					ON ADXX.LC_WOF_WUP_REA = LKXX.PX_ATR_VAL
					AND LKXX.PM_ATR = ''LC-WOF-WUP-REA''
			WHERE	
				LNXX.PC_FAT_TYP = ''XX''
				AND LNXX.PC_FAT_SUB_TYP = ''XX''
				AND LNXX.LD_FAT_PST >= ''XX/XX/XXXX''
		'
	)
ORDER BY
	DF_SPE_ACC_ID
	,LN_SEQ