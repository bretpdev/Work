--Create an email into emailbatchfed table for each of these letters whenever they generate.
USE CLS
GO
/*JAMS PART for inserting email campaign requests*/
DECLARE @TS50BGLB1 VARCHAR(10)= 'TS50BGLB1'
DECLARE @TS06BCNSWL VARCHAR(10)= 'TS06BCNSWL'
DECLARE @TS50BGLB1CampaignId INT = (SELECT EmailCampaignId FROM CLS.emailbtcf.EmailCampaigns WHERE LetterId = 'CODWCEFED.html') 
DECLARE @TS06BCNSWLCampaignId INT = (SELECT EmailCampaignId FROM CLS.emailbtcf.EmailCampaigns WHERE LetterId = 'CNSLWEFED.html')

INSERT INTO CLS..LetterToEmailProcessing(RM_APL_PGM_PRC, RT_RUN_SRT_DTS_PRC, RN_SEQ_LTR_CRT_PRC, RN_SEQ_REC_PRC, RM_DSC_LTR_PRC, EmailCampaignId, Recipient, AccountNumber, FirstName, LastName, LineData, AddedAt, AddedBy, IsCoborrowerRecord)
SELECT DISTINCT
	LT20.RM_APL_PGM_PRC,
	LT20.RT_RUN_SRT_DTS_PRC,
	LT20.RN_SEQ_LTR_CRT_PRC,
	LT20.RN_SEQ_REC_PRC,
	LT20.RM_DSC_LTR_PRC,
	@TS50BGLB1CampaignId AS EmailCampaignId,
	PH05.DX_CNC_EML_ADR AS Recipient,
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	SUBSTRING(PD10.DM_PRS_1,1,1) + RTRIM(LOWER(SUBSTRING(PD10.DM_PRS_1,2,13))) AS FirstName,
	'' AS LastName, --BU doesnt want a last name
	NULL AS LineData,
	GETDATE() AS AddedAt,
	SUSER_NAME() AS AddedBy,
	0
FROM
	CDW..PD10_PRS_NME PD10
	INNER JOIN CDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN CDW..LT20_LTR_REQ_PRC LT20
		ON LT20.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
		AND LT20.RM_DSC_LTR_PRC = @TS50BGLB1
		AND CAST(LT20.CreatedAt AS DATE) >= CAST(DATEADD(DAY,-5,GETDATE()) AS DATE)
	INNER JOIN CDW..PH05_CNC_EML PH05 
		ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
		AND PH05.DI_CNC_ELT_OPI = 'Y'
		AND PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
	LEFT JOIN CLS..LetterToEmailProcessing existingRequest
		ON existingRequest.EmailCampaignId = @TS50BGLB1CampaignId
		AND existingRequest.RM_APL_PGM_PRC = LT20.RM_APL_PGM_PRC
		AND CAST(existingRequest.RT_RUN_SRT_DTS_PRC AS DATE) = CAST(LT20.RT_RUN_SRT_DTS_PRC AS DATE)
		AND existingRequest.RN_SEQ_LTR_CRT_PRC = LT20.RN_SEQ_LTR_CRT_PRC
		AND existingRequest.RN_SEQ_REC_PRC = LT20.RN_SEQ_REC_PRC
		AND existingRequest.RM_DSC_LTR_PRC = @TS50BGLB1
		AND existingRequest.AccountNumber = PD10.DF_SPE_ACC_ID
		AND existingRequest.FirstName = SUBSTRING(PD10.DM_PRS_1,1,1) + RTRIM(LOWER(SUBSTRING(PD10.DM_PRS_1,2,13)))
		AND CAST(existingRequest.AddedAt AS DATE) BETWEEN CAST(DATEADD(DAY,-5,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE)
WHERE
	LN10.LC_STA_LON10 = 'R'
	AND LN10.LA_CUR_PRI > 0.00	
	AND existingRequest.AccountNumber IS NULL --Wasnt already added in last 5 days
ORDER BY
	PD10.DF_SPE_ACC_ID

INSERT INTO CLS..LetterToEmailProcessing(RM_APL_PGM_PRC, RT_RUN_SRT_DTS_PRC, RN_SEQ_LTR_CRT_PRC, RN_SEQ_REC_PRC, RM_DSC_LTR_PRC, EmailCampaignId, Recipient, AccountNumber, FirstName, LastName, LineData, AddedAt, AddedBy, IsCoborrowerRecord)
SELECT DISTINCT
	LT20.RM_APL_PGM_PRC,
	LT20.RT_RUN_SRT_DTS_PRC,
	LT20.RN_SEQ_LTR_CRT_PRC,
	LT20.RN_SEQ_REC_PRC,
	LT20.RM_DSC_LTR_PRC,
	@TS06BCNSWLCampaignId AS EmailCampaignId,
	PH05.DX_CNC_EML_ADR AS Recipient,
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	SUBSTRING(PD10.DM_PRS_1,1,1) + RTRIM(LOWER(SUBSTRING(PD10.DM_PRS_1,2,13))) AS FirstName,
	'' AS LastName, --BU doesnt want a last name
	NULL AS LineData,
	GETDATE() AS AddedAt,
	SUSER_NAME() AS AddedBy,
	0
FROM
	CDW..PD10_PRS_NME PD10
	INNER JOIN CDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN CDW..LT20_LTR_REQ_PRC LT20
		ON LT20.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
		AND LT20.RM_DSC_LTR_PRC = @TS06BCNSWL
		AND CAST(LT20.CreatedAt AS DATE) >= CAST(DATEADD(DAY,-5,GETDATE()) AS DATE)
	INNER JOIN CDW..PH05_CNC_EML PH05 
		ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
		AND PH05.DI_CNC_ELT_OPI = 'Y'
		AND PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
	LEFT JOIN CLS..LetterToEmailProcessing existingRequest
		ON existingRequest.EmailCampaignId = @TS06BCNSWLCampaignId
		AND existingRequest.RM_APL_PGM_PRC = LT20.RM_APL_PGM_PRC
		AND CAST(existingRequest.RT_RUN_SRT_DTS_PRC AS DATE) = CAST(LT20.RT_RUN_SRT_DTS_PRC AS DATE)
		AND existingRequest.RN_SEQ_LTR_CRT_PRC = LT20.RN_SEQ_LTR_CRT_PRC
		AND existingRequest.RN_SEQ_REC_PRC = LT20.RN_SEQ_REC_PRC
		AND existingRequest.RM_DSC_LTR_PRC = @TS06BCNSWL
		AND existingRequest.AccountNumber = PD10.DF_SPE_ACC_ID
		AND existingRequest.FirstName = SUBSTRING(PD10.DM_PRS_1,1,1) + RTRIM(LOWER(SUBSTRING(PD10.DM_PRS_1,2,13)))
		AND CAST(existingRequest.AddedAt AS DATE) BETWEEN CAST(DATEADD(DAY,-5,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE)
WHERE
	LN10.LC_STA_LON10 = 'R'
	AND LN10.LA_CUR_PRI > 0.00	
	AND existingRequest.AccountNumber IS NULL --Wasnt already added in last 5 days
ORDER BY
	PD10.DF_SPE_ACC_ID

--Coborrower
INSERT INTO CLS..LetterToEmailProcessing(RM_APL_PGM_PRC, RT_RUN_SRT_DTS_PRC, RN_SEQ_LTR_CRT_PRC, RN_SEQ_REC_PRC, RM_DSC_LTR_PRC, EmailCampaignId, Recipient, AccountNumber, FirstName, LastName, LineData, AddedAt, AddedBy, IsCoborrowerRecord)
SELECT DISTINCT
	LT20.RM_APL_PGM_PRC,
	LT20.RT_RUN_SRT_DTS_PRC,
	LT20.RN_SEQ_LTR_CRT_PRC,
	LT20.RN_SEQ_REC_PRC,
	LT20.RM_DSC_LTR_PRC,
	@TS50BGLB1CampaignId AS EmailCampaignId,
	PH05.DX_CNC_EML_ADR AS Recipient,
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	SUBSTRING(PD10.DM_PRS_1,1,1) + RTRIM(LOWER(SUBSTRING(PD10.DM_PRS_1,2,13))) AS FirstName,
	'' AS LastName, --BU doesnt want a last name
	NULL AS LineData,
	GETDATE() AS AddedAt,
	SUSER_NAME() AS AddedBy,
	1
FROM
	CDW..PD10_PRS_NME PD10
	INNER JOIN CDW..LT20_LTR_REQ_PRC_Coborrower LT20
		ON LT20.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
		AND LT20.RM_DSC_LTR_PRC = @TS50BGLB1
		AND CAST(LT20.CreatedAt AS DATE) >= CAST(DATEADD(DAY,-5,GETDATE()) AS DATE)
	INNER JOIN CDW..LN20_EDS LN20
		ON LN20.LF_EDS = PD10.DF_PRS_ID
		AND LN20.LC_EDS_TYP = 'M'
		AND LN20.LC_STA_LON20 = 'A'
	INNER JOIN CDW..LN10_LON LN10
		ON LN10.BF_SSN = LN20.BF_SSN
		AND LN10.LN_SEQ = LN20.LN_SEQ
	INNER JOIN CDW..PH05_CNC_EML PH05 
		ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
		AND PH05.DI_CNC_ELT_OPI = 'Y'
		AND PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
	LEFT JOIN CLS..LetterToEmailProcessing existingRequest
		ON existingRequest.EmailCampaignId = @TS50BGLB1CampaignId
		AND existingRequest.RM_APL_PGM_PRC = LT20.RM_APL_PGM_PRC
		AND CAST(existingRequest.RT_RUN_SRT_DTS_PRC AS DATE) = CAST(LT20.RT_RUN_SRT_DTS_PRC AS DATE)
		AND existingRequest.RN_SEQ_LTR_CRT_PRC = LT20.RN_SEQ_LTR_CRT_PRC
		AND existingRequest.RN_SEQ_REC_PRC = LT20.RN_SEQ_REC_PRC
		AND existingRequest.RM_DSC_LTR_PRC = @TS50BGLB1
		AND existingRequest.AccountNumber = PD10.DF_SPE_ACC_ID
		AND existingRequest.FirstName = SUBSTRING(PD10.DM_PRS_1,1,1) + RTRIM(LOWER(SUBSTRING(PD10.DM_PRS_1,2,13)))
		AND CAST(existingRequest.AddedAt AS DATE) BETWEEN CAST(DATEADD(DAY,-5,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE)
WHERE
	LN10.LC_STA_LON10 = 'R'
	AND LN10.LA_CUR_PRI > 0.00	
	AND existingRequest.AccountNumber IS NULL --Wasnt already added in last 5 days
ORDER BY
	PD10.DF_SPE_ACC_ID

INSERT INTO CLS..LetterToEmailProcessing(RM_APL_PGM_PRC, RT_RUN_SRT_DTS_PRC, RN_SEQ_LTR_CRT_PRC, RN_SEQ_REC_PRC, RM_DSC_LTR_PRC, EmailCampaignId, Recipient, AccountNumber, FirstName, LastName, LineData, AddedAt, AddedBy, IsCoborrowerRecord)
SELECT DISTINCT
	LT20.RM_APL_PGM_PRC,
	LT20.RT_RUN_SRT_DTS_PRC,
	LT20.RN_SEQ_LTR_CRT_PRC,
	LT20.RN_SEQ_REC_PRC,
	LT20.RM_DSC_LTR_PRC,
	@TS06BCNSWLCampaignId AS EmailCampaignId,
	PH05.DX_CNC_EML_ADR AS Recipient,
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	SUBSTRING(PD10.DM_PRS_1,1,1) + RTRIM(LOWER(SUBSTRING(PD10.DM_PRS_1,2,13))) AS FirstName,
	'' AS LastName, --BU doesnt want a last name
	NULL AS LineData,
	GETDATE() AS AddedAt,
	SUSER_NAME() AS AddedBy,
	1
FROM
	CDW..PD10_PRS_NME PD10
	INNER JOIN CDW..LT20_LTR_REQ_PRC_Coborrower LT20
		ON LT20.DF_SPE_ACC_ID = PD10.DF_SPE_ACC_ID
		AND LT20.RM_DSC_LTR_PRC = @TS06BCNSWL
		AND CAST(LT20.CreatedAt AS DATE) >= CAST(DATEADD(DAY,-5,GETDATE()) AS DATE)
	INNER JOIN CDW..LN20_EDS LN20
		ON LN20.LF_EDS = PD10.DF_PRS_ID
		AND LN20.LC_EDS_TYP = 'M'
		AND LN20.LC_STA_LON20 = 'A'
	INNER JOIN CDW..LN10_LON LN10
		ON LN10.BF_SSN = LN20.BF_SSN
		AND LN10.LN_SEQ = LN20.LN_SEQ
	INNER JOIN CDW..PH05_CNC_EML PH05 
		ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
		AND PH05.DI_CNC_ELT_OPI = 'Y'
		AND PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
	LEFT JOIN CLS..LetterToEmailProcessing existingRequest
		ON existingRequest.EmailCampaignId = @TS06BCNSWLCampaignId
		AND existingRequest.RM_APL_PGM_PRC = LT20.RM_APL_PGM_PRC
		AND CAST(existingRequest.RT_RUN_SRT_DTS_PRC AS DATE) = CAST(LT20.RT_RUN_SRT_DTS_PRC AS DATE)
		AND existingRequest.RN_SEQ_LTR_CRT_PRC = LT20.RN_SEQ_LTR_CRT_PRC
		AND existingRequest.RN_SEQ_REC_PRC = LT20.RN_SEQ_REC_PRC
		AND existingRequest.RM_DSC_LTR_PRC = @TS06BCNSWL
		AND existingRequest.AccountNumber = PD10.DF_SPE_ACC_ID
		AND existingRequest.FirstName = SUBSTRING(PD10.DM_PRS_1,1,1) + RTRIM(LOWER(SUBSTRING(PD10.DM_PRS_1,2,13)))
		AND CAST(existingRequest.AddedAt AS DATE) BETWEEN CAST(DATEADD(DAY,-5,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE)
WHERE
	LN10.LC_STA_LON10 = 'R'
	AND LN10.LA_CUR_PRI > 0.00	
	AND existingRequest.AccountNumber IS NULL --Wasnt already added in last 5 days
ORDER BY
	PD10.DF_SPE_ACC_ID

--SELECT * FROM CLS..LetterToEmailProcessing

INSERT INTO CLS.emailbtcf.CampaignData(EmailCampaignId, Recipient, AccountNumber, FirstName, LastName, AddedAt, AddedBy)
SELECT DISTINCT
	LTEP.EmailCampaignId,
	LTEP.Recipient,
	LTEP.AccountNumber,
	LTEP.FirstName,
	LTEP.LastName,
	GETDATE(),
	SUSER_NAME()
FROM 
	CLS..LetterToEmailProcessing LTEP
WHERE
	LTEP.ProcessedAt IS NULL
	AND LTEP.DeletedAt IS NULL

UPDATE
	CLS..LetterToEmailProcessing
SET
	ProcessedAt = GETDATE()
WHERE
	ProcessedAt IS NULL
	AND DeletedAt IS NULL
