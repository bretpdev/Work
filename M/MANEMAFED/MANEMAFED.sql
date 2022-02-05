USE CLS
GO

DECLARE @MostRecent INT = (SELECT MAX(ManagerEmailParameterId) FROM CLS..ManagerEmailParameters)
DECLARE @LowerLimit INT = (SELECT DelinquencyLowerLimit FROM CLS..ManagerEmailParameters WHERE ManagerEmailParameterId = @MostRecent)
DECLARE @UpperLimit INT = (SELECT DelinquencyUpperLimit FROM CLS..ManagerEmailParameters WHERE ManagerEmailParameterId = @MostRecent)
DECLARE @MaxNumber INT = (SELECT MaxEmails FROM CLS..ManagerEmailParameters WHERE ManagerEmailParameterId = @MostRecent)

/*JAMS PART for inserting email campaign requests*/
DECLARE @EmailCampaignId INT = (SELECT EmailCampaignId FROM CLS.emailbtcf.EmailCampaigns WHERE LetterId = 'MANEMLFED.html')

DECLARE @Emails TABLE(DF_PRS_ID CHAR(9), DX_ADR_EML VARCHAR(254), DF_LST_DTS_PD32 DATETIME, DM_PRS_1 VARCHAR(13), DM_PRS_LST VARCHAR(23), EmailPriority INT)
INSERT INTO @Emails(DF_PRS_ID, DX_ADR_EML, DF_LST_DTS_PD32, DM_PRS_1, DM_PRS_LST, EmailPriority)
SELECT
	EMAIL.DF_PRS_ID,
	EMAIL.DX_ADR_EML,
	EMAIL.DF_LST_DTS_PD32,
	EMAIL.DM_PRS_1,
	EMAIL.DM_PRS_LST,
	ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.DF_LST_DTS_PD32 DESC, Email.PriorityNumber) AS EmailPriority -- number in order of Email.PriorityNumber
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
			CDW..PD32_PRS_ADR_EML PD32
			INNER JOIN CDW..PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
		WHERE
			PD32.DI_VLD_ADR_EML = 'Y' -- valid email address
			AND PD32.DC_STA_PD32 = 'A' -- active email address record
	) Email

INSERT INTO CLS.emailbtcf.CampaignData(EmailCampaignId, Recipient, AccountNumber, FirstName, LastName, AddedAt, AddedBy)
SELECT DISTINCT TOP (@MaxNumber)
	@EmailCampaignId AS EmailCampaignId,
	COALESCE(PD32B.DX_ADR_EML,PD32E.DX_ADR_EML) AS Recipient,
	PD10.DF_SPE_ACC_ID AS AccountNumber,
	COALESCE(PD32B.DM_PRS_1,PD32E.DM_PRS_1) AS FirstName,
	'' AS LastName, --BU doesnt want a last name
	GETDATE() AS AddedAt,
	SUSER_NAME() AS AddedBy
FROM
	CDW..PD10_PRS_NME PD10
	INNER JOIN CDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN CDW..LN16_LON_DLQ_HST LN16 
		ON LN16.BF_SSN = LN10.BF_SSN
		AND LN16.LN_SEQ = LN10.LN_SEQ
		AND LN16.LC_STA_LON16 = '1'
		AND LN16.LN_DLQ_MAX +1 BETWEEN @LowerLimit AND @UpperLimit
	LEFT JOIN CDW..LN20_EDS LN20
		ON LN20.BF_SSN = LN10.BF_SSN
		AND LN20.LN_SEQ = LN10.LN_SEQ
		AND LN20.LC_STA_LON20 = 'A'
	LEFT JOIN @Emails PD32B --Needs joined x2 (one for borrower one for endorser)
		ON PD32B.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD32B.EmailPriority = 1
	LEFT JOIN @Emails PD32E --Needs joined x2 (one for borrower one for endorser)
		ON PD32E.DF_PRS_ID = LN20.LF_EDS
		AND PD32E.EmailPriority = 1
	LEFT JOIN CDW..DW01_DW_CLC_CLU DW01 --Needs to exclude on an account level
		ON DW01.BF_SSN = LN10.BF_SSN
		AND DW01.WC_DW_LON_STA IN('16','17','18','19','20','21') 
	LEFT JOIN 
	(
		SELECT DISTINCT
			LN60.BF_SSN
		FROM
			CDW..LN60_BR_FOR_APV LN60
			INNER JOIN CDW..FB10_BR_FOR_REQ FB10
				ON FB10.BF_SSN = LN60.BF_SSN
				AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
				AND FB10.LC_FOR_TYP = '28'
				AND FB10.LC_FOR_STA = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
		WHERE
			LN60.LC_STA_LON60 = 'A'
			AND LN60.LC_FOR_RSP != '003'
			AND CAST(GETDATE() AS DATE) BETWEEN LN60.LD_FOR_BEG AND LN60.LD_FOR_END
	) CollectionForb
		ON CollectionForb.BF_SSN = LN10.BF_SSN --Account level
	LEFT JOIN
	(
		SELECT
			AY10.BF_SSN
		FROM
			CDW..AY10_BR_LON_ATY AY10
		WHERE
			AY10.PF_REQ_ACT = 'EMMGR' /*TODO: change this arc to the arc tied to the email campaign*/
			AND CAST(AY10.LD_ATY_REQ_RCV AS DATE) BETWEEN CAST(DATEADD(DAY,-15,GETDATE()) AS DATE) AND CAST(GETDATE() AS DATE)
	) AY10
		ON AY10.BF_SSN = LN10.BF_SSN
	LEFT JOIN CLS.emailbtcf.CampaignData existingRequest
		ON existingRequest.EmailCampaignId = @EmailCampaignId
		AND existingRequest.AccountNumber = PD10.DF_SPE_ACC_ID
		AND existingRequest.FirstName = COALESCE(PD32B.DM_PRS_1,PD32E.DM_PRS_1)
		--AND existingRequest.LastName = COALESCE(PD32B.DM_PRS_LST,PD32E.DM_PRS_LST)
		AND CAST(AddedAt AS DATE) = CAST(GETDATE() AS DATE)
WHERE
	LN10.LC_STA_LON10 = 'R'
	AND LN10.LA_CUR_PRI > 0.00	
	AND DW01.BF_SSN IS NULL
	AND CollectionForb.BF_SSN IS NULL
	AND AY10.BF_SSN IS NULL
	AND 
	(
		PD32B.DF_PRS_ID IS NOT NULL
		OR PD32E.DF_PRS_ID IS NOT NULL
	)
	AND existingRequest.AccountNumber IS NULL --Wasnt already added today.
ORDER BY
	PD10.DF_SPE_ACC_ID