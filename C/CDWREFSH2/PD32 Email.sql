USE CDW
GO

MERGE 
	CDW.dbo.PD32_Email PD32
USING
	(
		SELECT DISTINCT
			PD10.DF_SPE_ACC_ID,
			PD32.DC_ADR_EML,
			ISNULL(PD32.DX_ADR_EML,'') AS DX_ADR_EML,
			ISNULL(CONVERT(VARCHAR(10),PD32.DD_VER_ADR_EML,101),'') AS DD_VER_ADR_EML,
			ISNULL(PD32.DI_VLD_ADR_EML,'') AS DI_VLD_ADR_EML
		FROM
			CDW..PD10_PRS_NME PD10
			INNER JOIN CDW..PD32_PRS_ADR_EML PD32
				ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
		WHERE
			PD32.DC_STA_PD32 = 'A'
			AND LEFT(PD32.DF_PRS_ID,1) != 'P'
			AND CentralData.dbo.TRIM(PD10.DF_SPE_ACC_ID) != ''
	) NewData 
		ON NewData.DF_SPE_ACC_ID = PD32.DF_SPE_ACC_ID
		AND NewData.DC_ADR_EML = PD32.DC_ADR_EML
WHEN MATCHED THEN 
	UPDATE SET 
		PD32.DX_ADR_EML = NewData.DX_ADR_EML,
		PD32.DD_VER_ADR_EML = NewData.DD_VER_ADR_EML,
		PD32.DI_VLD_ADR_EML = NewData.DI_VLD_ADR_EML
WHEN NOT MATCHED THEN
	INSERT 
	(
		DF_SPE_ACC_ID,
		DC_ADR_EML,
		DX_ADR_EML,
		DD_VER_ADR_EML,
		DI_VLD_ADR_EML
	)
	VALUES 
	(
		NewData.DF_SPE_ACC_ID,
		NewData.DC_ADR_EML,
		NewData.DX_ADR_EML,
		NewData.DD_VER_ADR_EML,
		NewData.DI_VLD_ADR_EML
	)
WHEN NOT MATCHED BY SOURCE THEN
	DELETE
;