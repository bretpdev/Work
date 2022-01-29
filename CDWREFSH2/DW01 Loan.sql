USE CDW
GO

MERGE 
	CDW.dbo.DW01_Loan DW01
USING
	(
		SELECT
			PD10.DF_SPE_ACC_ID,
			DW01.LN_SEQ,
			ISNULL(DW01.WC_DW_LON_STA,'') AS WC_DW_LON_STA,
			ISNULL(CASE WHEN DW01.WX_OVR_DW_LON_STA = 'LITIGATION' THEN 'DEFAULT - PRIOR REHAB'
			     WHEN ISNULL(DW01.WX_OVR_DW_LON_STA,'') != '' THEN DW01.WX_OVR_DW_LON_STA
				 ELSE ISNULL(FMT.[Label],DW01.WC_DW_LON_STA)
			END,'') AS DW_LON_STA,
			ISNULL(CONVERT(VARCHAR(10),DW01.WD_LON_RPD_SR,101),'') AS WD_LON_RPD_SR,
			ISNULL(DW01.WA_TOT_BRI_OTS,0.00) AS WA_TOT_BRI_OTS
		FROM
			CDW..PD10_PRS_NME PD10
			INNER JOIN CDW..DW01_DW_CLC_CLU DW01
				ON DW01.BF_SSN = PD10.DF_PRS_ID
			LEFT JOIN CDW..FormatTranslation FMT
				ON FMT.[Start] = DW01.WC_DW_LON_STA
				AND FMT.FmtName = '$LONSTA'
		WHERE
			CentralData.dbo.TRIM(PD10.DF_SPE_ACC_ID) != ''
	) NewData 
		ON NewData.DF_SPE_ACC_ID = DW01.DF_SPE_ACC_ID
		AND NewData.LN_SEQ = DW01.LN_SEQ
WHEN MATCHED THEN 
	UPDATE SET 
		DW01.WC_DW_LON_STA = NewData.WC_DW_LON_STA,
		DW01.DW_LON_STA = NewData.DW_LON_STA,
		DW01.WD_LON_RPD_SR = NewData.WD_LON_RPD_SR,
		DW01.WA_TOT_BRI_OTS = NewData.WA_TOT_BRI_OTS
WHEN NOT MATCHED THEN
	INSERT 
	(
		DF_SPE_ACC_ID,
		LN_SEQ,
		WC_DW_LON_STA,
		DW_LON_STA,
		WD_LON_RPD_SR,
		WA_TOT_BRI_OTS
	)
	VALUES 
	(
		NewData.DF_SPE_ACC_ID,
		NewData.LN_SEQ,
		NewData.WC_DW_LON_STA,
		NewData.DW_LON_STA,
		NewData.WD_LON_RPD_SR,
		NewData.WA_TOT_BRI_OTS
	)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE
;
