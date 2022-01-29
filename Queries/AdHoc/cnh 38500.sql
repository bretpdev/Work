SELECT DISTINCT
	PD10.DF_PRS_ID,
	PD10.DF_SPE_ACC_ID,
	PD30.DX_STR_ADR_1,
	PD30.DX_STR_ADR_2,
	PD30.DX_STR_ADR_3,
	PD30.DM_CT,
	PD30.DC_DOM_ST,
	PD30.DF_ZIP_CDE,
	ISNULL(PD40H.DN_DOM_PHN_ARA + PD40H.DN_DOM_PHN_XCH + PD40H.DN_DOM_PHN_LCL,'N/A') AS HOME_PHN,
	ISNULL(PD40A.DN_DOM_PHN_ARA + PD40A.DN_DOM_PHN_XCH + PD40A.DN_DOM_PHN_LCL,'N/A') AS ALT_PHN,
	ISNULL(PD40W.DN_DOM_PHN_ARA + PD40W.DN_DOM_PHN_XCH + PD40W.DN_DOM_PHN_LCL,'N/A') AS WORD_PHN,
	ISNULL(PD32H.DX_ADR_EML,'N/A') AS HOME_EMAIL,
	ISNULL(PD32A.DX_ADR_EML,'N/A') AS ALT_EMAIL,
	ISNULL(PH05.DX_CNC_EML_ADR,'N/A') AS CONTACT_EMAIL
FROM
	CDW..PD10_PRS_NME PD10
	INNER JOIN CDW..PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
	LEFT JOIN CDW..PD40_PRS_PHN PD40H
		ON PD40H.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD40H.DC_PHN = 'H'
		AND PD40H.DI_PHN_VLD = 'Y'
	LEFT JOIN CDW..PD40_PRS_PHN PD40A
		ON PD40A.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD40A.DC_PHN = 'A'
		AND PD40A.DI_PHN_VLD = 'Y'
	LEFT JOIN CDW..PD40_PRS_PHN PD40W
		ON PD40W.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD40W.DC_PHN = 'W'
		AND PD40W.DI_PHN_VLD = 'Y'
	LEFT JOIN CDW..PD32_PRS_ADR_EML PD32H
		ON PD32H.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD32H.DC_ADR_EML = 'H'
		AND PD32H.DI_VLD_ADR_EML = 'Y'
		AND PD32H.DC_STA_PD32 = 'A'
	LEFT JOIN CDW..PD32_PRS_ADR_EML PD32A
		ON PD32A.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD32A.DC_ADR_EML = 'A'
		AND PD32A.DI_VLD_ADR_EML = 'Y'
	LEFT JOIN CDW..PH05_CNC_EML PH05
		ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
		AND PH05.DI_VLD_CNC_EML_ADR = 'Y'

WHERE
	
	 PD10.DF_SPE_ACC_ID IN ('7572912268',
'7572912268',
'7572912268',
'7572912268',
'7572912268',
'7572912268',
'7572912268',
'7572912268',
'7572912268',
'3271576302',
'3271576302',
'3271576302',
'3271576302',
'3271576302',
'4097537264',
'4097537264',
'4097537264',
'4097537264',
'9226763615',
'9226763615',
'9226763615',
'6342398439',
'6342398439',
'6342398439',
'6342398439',
'6342398439',
'1091473319',
'1091473319',
'4722393206',
'4722393206',
'4722393206',
'4722393206',
'4722393206',
'4722393206',
'4824387324',
'4824387324',
'4824387324',
'4824387324',
'4824387324',
'1807116945',
'1807116945',
'1807116945',
'1807116945',
'1807116945',
'3462353654',
'3462353654',
'3462353654',
'3462353654',
'3462353654',
'3462353654',
'3462353654',
'3462353654',
'7309482817',
'7309482817',
'7309482817',
'7309482817',
'7309482817',
'7309482817',
'7309482817',
'7309482817',
'7309482817',
'7309482817',
'6455625489',
'6455625489',
'6455625489',
'6455625489',
'6455625489',
'0255914675',
'0255914675',
'0255914675',
'2233080945',
'2233080945',
'2233080945',
'2233080945',
'8284372235',
'8284372235',
'8284372235',
'8284372235',
'0314949287',
'7287257131',
'5525281234',
'1237817395',
'1237817395',
'9795283816',
'9795283816',
'9795283816',
'0246813164',
'0246813164',
'0246813164',
'4491730228',
'1924534372',
'1924534372',
'1924534372',
'1924534372')