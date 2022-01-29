SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

SELECT DISTINCT
	PD10.DF_SPE_ACC_ID
FROM
	UDW..PD30_PRS_ADR PD30
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
WHERE
	PD30.DC_DOM_ST = 'IL'

SELECT DISTINCT
	PD01.DF_SPE_ACC_ID
FROM
	ODW..PD03_PRS_ADR_PHN PD03
	INNER JOIN ODW..PD01_PDM_INF PD01
		ON PD03.DF_PRS_ID = PD01.DF_PRS_ID
WHERE
	PD03.DC_DOM_ST = 'IL'
