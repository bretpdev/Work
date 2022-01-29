USE ULS
GO

DECLARE @Emails TABLE(DF_PRS_ID CHAR(9), DX_ADR_EML VARCHAR(254), DF_LST_DTS_PD32 DATETIME, DM_PRS_1 VARCHAR(13), DM_PRS_LST VARCHAR(23), EmailPriority INT)
INSERT INTO @Emails(DF_PRS_ID, DX_ADR_EML, DF_LST_DTS_PD32, DM_PRS_1, DM_PRS_LST, EmailPriority)
SELECT
	EMAIL.DF_PRS_ID,
	EMAIL.DX_ADR_EML,
	EMAIL.DF_LST_DTS_PD32,
	EMAIL.DM_PRS_1,
	EMAIL.DM_PRS_LST,
	ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber, Email.DF_LST_DTS_PD32 DESC) AS EmailPriority -- number in order of Email.PriorityNumber
	FROM
	(
		SELECT
			PD32.DF_PRS_ID,
			PD32.DX_ADR_EML,
			CASE 
				WHEN DC_ADR_EML = 'H' THEN 1 -- home
				WHEN DC_ADR_EML = 'A' THEN 2 -- alternate
				WHEN DC_ADR_EML = 'W' THEN 3 -- work
			END AS PriorityNumber,
			PD32.DF_LST_DTS_PD32,
			PD10.DM_PRS_1,
			PD10.DM_PRS_LST
		FROM
			UDW..PD32_PRS_ADR_EML PD32
			INNER JOIN UDW..PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
		WHERE
			PD32.DI_VLD_ADR_EML = 'Y' -- valid email address
			AND PD32.DC_STA_PD32 = 'A' -- active email address record
			AND PD32.DF_PRS_ID NOT LIKE 'P%'
	) Email

DECLARE @EmailCampaignIdB INT = (SELECT EC.EmailCampaignId FROM ULS.emailbatch.EmailCampaigns EC INNER JOIN ULS.emailbatch.HTMLFiles H ON H.HTMLFileId = EC.HTMLFileId INNER JOIN ULS.emailbatch.Comments C ON C.CommentId = EC.CommentId WHERE H.HTMLFile = '80DLDUDTUH.html' AND C.Comment = '80 day delinquency email sent to borrower')
DECLARE @EmailCampaignIdE INT = (SELECT EC.EmailCampaignId FROM ULS.emailbatch.EmailCampaigns EC INNER JOIN ULS.emailbatch.HTMLFiles H ON H.HTMLFileId = EC.HTMLFileId INNER JOIN ULS.emailbatch.Comments C ON C.CommentId = EC.CommentId WHERE H.HTMLFile = '80DLDUDTUH.html' AND C.Comment = '80 day delinquency email sent to endorser')

INSERT INTO ULS.emailbatch.EmailProcessing(EmailCampaignId, AccountNumber, ActualFile, EmailData, ArcNeeded, ProcessingAttempts, AddedAt, AddedBy)
--Borrowers
SELECT DISTINCT 
	@EmailCampaignIdB AS EmailCampaignId,
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	NULL AS ActualFile,
	CAST(PD10.DF_SPE_ACC_ID AS VARCHAR(10)) + ',' 
		+ COALESCE(LTRIM(RTRIM(PD10.DM_PRS_1)),'') + ','
		+ PD32.DX_ADR_EML AS EmailData,
	CASE WHEN EA.Arc IS NOT NULL THEN 1 ELSE 0 END AS ArcNeeded,
	0 AS ProcessingAttempts,
	GETDATE() AS AddedAt,
	SUSER_NAME() AS AddedBy
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN UDW..LN16_LON_DLQ_HST LN16 
		ON LN16.BF_SSN = LN10.BF_SSN
		AND LN16.LN_SEQ = LN10.LN_SEQ
		AND LN16.LC_STA_LON16 = '1'
		AND
		(
			LN16.LN_DLQ_MAX + 1 = 80
			OR
			(
				LN16.LN_DLQ_MAX + 1 IN(80,81,82,83)
				AND DATENAME(WEEKDAY,GETDATE()) = 'Monday'
			)
		)
	INNER JOIN UDW..DW01_DW_CLC_CLU DW01
		ON DW01.BF_SSN = LN10.BF_SSN
		AND DW01.LN_SEQ = LN10.LN_SEQ
		AND DW01.WC_DW_LON_STA NOT IN('16','17','18','19','20','21')
	INNER JOIN @Emails PD32
		ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD32.EmailPriority = 1
	LEFT JOIN
	(
		SELECT DISTINCT
			LN50.BF_SSN,
			LN50.LN_SEQ
		FROM
			UDW..LN50_BR_DFR_APV LN50
			INNER JOIN UDW..DF10_BR_DFR_REQ DF10
				ON DF10.BF_SSN = LN50.BF_SSN
				AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
		WHERE
			LN50.LC_STA_LON50 = 'A'
			AND DF10.LC_DFR_STA = 'A'
			AND DF10.LC_STA_DFR10 = 'A'
			AND LN50.LC_DFR_RSP != '003'
			AND CAST(GETDATE() AS DATE) BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END

		UNION ALL

		SELECT DISTINCT
			LN60.BF_SSN,
			LN60.LN_SEQ
		FROM
			UDW..LN60_BR_FOR_APV LN60
			INNER JOIN UDW..FB10_BR_FOR_REQ FB10
				ON FB10.BF_SSN = LN60.BF_SSN
				AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
		WHERE
			LN60.LC_STA_LON60 = 'A'
			AND FB10.LC_FOR_STA = 'A'
			AND FB10.LC_STA_FOR10 = 'A'
			AND LN60.LC_FOR_RSP != '003'
			AND CAST(GETDATE() AS DATE) BETWEEN LN60.LD_FOR_BEG AND LN60.LD_FOR_END
	)	DeferForb
		ON DeferForb.BF_SSN = LN10.BF_SSN
		AND DeferForb.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN ULS.emailbatch.EmailProcessing existingRequest
		ON existingRequest.EmailCampaignId = @EmailCampaignIdB
		AND existingRequest.AccountNumber = PD10.DF_SPE_ACC_ID
		AND existingRequest.EmailData = CAST(PD10.DF_SPE_ACC_ID AS VARCHAR(10)) + ',' 
										+ COALESCE(LTRIM(RTRIM(PD10.DM_PRS_1)),'') + ','
										+ PD32.DX_ADR_EML
		AND CAST(AddedAt AS DATE) = CAST(GETDATE() AS DATE)
	LEFT JOIN ULS.emailbatch.EmailCampaigns EC
		ON EC.EmailCampaignId = @EmailCampaignIdB
		AND EC.DeletedAt IS NULL
	LEFT JOIN ULS.emailbatch.Arcs EA
		ON EA.ArcId = EC.ArcId
WHERE
	LN10.LC_STA_LON10 = 'R'
	AND LN10.LA_CUR_PRI > 0.00
	AND DeferForb.BF_SSN IS NULL
	AND existingRequest.AccountNumber IS NULL --Wasnt already added today.
	AND PD10.DF_PRS_ID NOT LIKE 'P%'
	
UNION ALL

--Endorsers
SELECT DISTINCT 
	@EmailCampaignIdE AS EmailCampaignId,
	PD10B.DF_SPE_ACC_ID AS AccountNumber,
	NULL AS ActualFile,
	CAST(PD10.DF_SPE_ACC_ID AS VARCHAR(10)) + ',' 
		+ COALESCE(LTRIM(RTRIM(PD10.DM_PRS_1)),'') + ','
		+ PD32.DX_ADR_EML AS EmailData,
	CASE WHEN EA.Arc IS NOT NULL THEN 1 ELSE 0 END AS ArcNeeded,
	0 AS ProcessingAttempts,
	GETDATE() AS AddedAt,
	SUSER_NAME() AS AddedBy
FROM
	UDW..LN10_LON LN10
	INNER JOIN UDW..DW01_DW_CLC_CLU DW01
		ON DW01.BF_SSN = LN10.BF_SSN
		AND DW01.LN_SEQ = LN10.LN_SEQ
		AND DW01.WC_DW_LON_STA NOT IN('16','17','18','19','20','21')
	INNER JOIN UDW..LN16_LON_DLQ_HST LN16 
		ON LN16.BF_SSN = LN10.BF_SSN
		AND LN16.LN_SEQ = LN10.LN_SEQ
		AND LN16.LC_STA_LON16 = '1'
		AND
		(
			LN16.LN_DLQ_MAX + 1 = 80
			OR
			(
				LN16.LN_DLQ_MAX + 1 IN(80,81,82,83)
				AND DATENAME(WEEKDAY,GETDATE()) = 'Monday'
			)
		)
	INNER JOIN UDW..LN20_EDS LN20
		ON LN20.BF_SSN = LN10.BF_SSN
		AND LN20.LN_SEQ = LN10.LN_SEQ
		AND LN20.LC_STA_LON20 = 'A'
		AND LN20.LC_EDS_TYP = 'M'
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = LN20.LF_EDS
	INNER JOIN UDW..PD10_PRS_NME PD10B
		ON PD10B.DF_PRS_ID = LN10.BF_SSN
	INNER JOIN @Emails PD32
		ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD32.EmailPriority = 1
	LEFT JOIN
	(
		SELECT DISTINCT
			LN50.BF_SSN,
			LN50.LN_SEQ
		FROM
			UDW..LN50_BR_DFR_APV LN50
			INNER JOIN UDW..DF10_BR_DFR_REQ DF10
				ON DF10.BF_SSN = LN50.BF_SSN
				AND DF10.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
		WHERE
			LN50.LC_STA_LON50 = 'A'
			AND DF10.LC_DFR_STA = 'A'
			AND DF10.LC_STA_DFR10 = 'A'
			AND LN50.LC_DFR_RSP != '003'
			AND CAST(GETDATE() AS DATE) BETWEEN LN50.LD_DFR_BEG AND LN50.LD_DFR_END

		UNION ALL

		SELECT DISTINCT
			LN60.BF_SSN,
			LN60.LN_SEQ
		FROM
			UDW..LN60_BR_FOR_APV LN60
			INNER JOIN UDW..FB10_BR_FOR_REQ FB10
				ON FB10.BF_SSN = LN60.BF_SSN
				AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
		WHERE
			LN60.LC_STA_LON60 = 'A'
			AND FB10.LC_FOR_STA = 'A'
			AND FB10.LC_STA_FOR10 = 'A'
			AND LN60.LC_FOR_RSP != '003'
			AND CAST(GETDATE() AS DATE) BETWEEN LN60.LD_FOR_BEG AND LN60.LD_FOR_END
	)	DeferForb
		ON DeferForb.BF_SSN = LN10.BF_SSN
		AND DeferForb.LN_SEQ = LN10.LN_SEQ
	LEFT JOIN ULS.emailbatch.EmailProcessing existingRequest
		ON existingRequest.EmailCampaignId = @EmailCampaignIdE
		AND existingRequest.AccountNumber = PD10B.DF_SPE_ACC_ID
		AND existingRequest.EmailData = CAST(PD10.DF_SPE_ACC_ID AS VARCHAR(10)) + ',' 
										+ COALESCE(LTRIM(RTRIM(PD10.DM_PRS_1)),'') + ','
										+ PD32.DX_ADR_EML
		AND CAST(AddedAt AS DATE) = CAST(GETDATE() AS DATE)
	LEFT JOIN ULS.emailbatch.EmailCampaigns EC
		ON EC.EmailCampaignId = @EmailCampaignIdE
		AND EC.DeletedAt IS NULL
	LEFT JOIN ULS.emailbatch.Arcs EA
		ON EA.ArcId = EC.ArcId
WHERE
	LN10.LC_STA_LON10 = 'R'
	AND LN10.LA_CUR_PRI > 0.00	
	AND DeferForb.BF_SSN IS NULL
	AND existingRequest.AccountNumber IS NULL --Wasnt already added today.
	AND PD10.DF_PRS_ID NOT LIKE 'P%'
ORDER BY
	PD10.DF_SPE_ACC_ID