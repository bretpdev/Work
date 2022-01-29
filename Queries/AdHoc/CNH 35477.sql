USE CDW
GO

/*************************************************/
/*CHANGE THIS NUMBER TO THE NUMBER OF EMAILS YOU WANT TO SEND*/
/************************************************/
DECLARE @NUMBEROFEMAILS INT = XXXXX
DECLARE @EMAILID INT = (SELECT EmailCampaignId FROM CLS.emailbtcf.EmailCampaigns WHERE LetterId = 'CHTNEMLFED.html') 

INSERT INTO CLS.emailbtcf.CampaignData (EmailCampaignId, Recipient, AccountNumber, FirstName, LastName, AddedAt, AddedBy)
SELECT TOP (@NUMBEROFEMAILS)
@EMAILID,
POP.DX_ADR_EML,
POP.DF_SPE_ACC_ID,
POP.DM_PRS_X,
POP.DM_PRS_LST,
GETDATE(),
'CNH XXXXX'

FROM 
(
SELECT DISTINCT
	PDXX.DF_SPE_ACC_ID,
	COALESCE(PHXX.DX_CNC_EML_ADR, PDXX.DX_ADR_EML) AS DX_ADR_EML,
	RTRIM(PDXX.DM_PRS_X) AS DM_PRS_X,
	RTRIM(PDXX.DM_PRS_LST) AS DM_PRS_LST
FROM
	CDW..PDXX_PRS_NME PDXX
	INNER JOIN CDW..LNXX_LON LNXX
		ON LNXX.BF_SSN = PDXX.DF_PRS_ID
	LEFT JOIN CLS.emailbtcf.CampaignData CD
		ON PDXX.DF_SPE_ACC_ID = CD.AccountNumber
		AND CD.EmailCampaignId = @EMAILID
	LEFT JOIN CDW..PHXX_CNC_EML PHXX
		ON PHXX.DF_SPE_ID = PDXX.DF_SPE_ACC_ID
		AND PHXX.DI_VLD_CNC_EML_ADR = 'Y'
		AND PHXX.DI_CNC_ELT_OPI = 'Y'
	LEFT JOIN
	(
		SELECT
			EMAIL.*,
			ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) AS EmailPriority -- number in order of Email.PriorityNumber
		FROM
		(
			SELECT
				PDXX.DF_PRS_ID,
				PDXX.DX_ADR_EML,
				CASE 
					WHEN DC_ADR_EML = 'H' THEN X -- home
					WHEN DC_ADR_EML = 'A' THEN X -- alternate
					WHEN DC_ADR_EML = 'W' THEN X -- work
				END AS PriorityNumber
			FROM
				CDW..PDXX_PRS_ADR_EML PDXX
				INNER JOIN CDW..LNXX_LON LNXX
					ON LNXX.BF_SSN = PDXX.DF_PRS_ID
			WHERE
				PDXX.DI_VLD_ADR_EML = 'Y' -- valid email address
				AND PDXX.DC_STA_PDXX = 'A'-- active email address record
		) Email
	) PDXX 
		ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
		AND PDXX.EmailPriority = X --highest priority email only

WHERE
	LNXX.LC_STA_LONXX = 'R'
	AND LNXX.LA_CUR_PRI > X.XX
	AND 
	(
		PHXX.DF_SPE_ID IS NOT NULL
		OR PDXX.DF_PRS_ID IS NOT NULL
	)
	AND CD.AccountNumber IS NULL
) POP