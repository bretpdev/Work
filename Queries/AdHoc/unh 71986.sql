
select DISTINCT
	EP.* 
from 
	ULS.emailbatch.EmailProcessing EP
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_SPE_ACC_ID = EP.AccountNumber
	INNER JOIN UDW..PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD30.DI_VLD_ADR = 'Y'
		AND PD30.DC_ADR = 'L'
	INNER JOIN ULS.dasforbuh.Zips Z
		ON Z.ZipCode = PD30.DF_ZIP_CDE
	INNER JOIN ULS.dasforbuh.Disasters D
		ON D.DisasterId = Z.DisasterId
		AND D.Active != 1
		and d.DisasterId in (
		
		63,
64,
65,
66,
67)
where 
	EmailCampaignId = 49 
	and EP.AddedAt > '07/01/2021'
