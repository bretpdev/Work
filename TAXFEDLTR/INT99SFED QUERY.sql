DECLARE @LETTER_ID INT = (SELECT LetterId FROM CLS.[print].Letters WHERE Letter = 'INT99SFED')
DECLARE @SCRIPT_DATA_ID INT = (SELECT ScriptDataId FROM CLS.[print].ScriptData WHERE LetterId = @LETTER_ID )
INSERT INTO CLS.[print].PrintProcessing(AccountNumber, EmailAddress, ScriptDataId, SourceFile, LetterData, CostCenter, DoNotProcessEcorr, OnEcorr, ArcNeeded, ImagingNeeded, AddedAt, AddedBy)
SELECT DISTINCT
	PD10.DF_SPE_ACC_ID,
	COALESCE(PH05.DX_CNC_EML_ADR,	PD32.DX_ADR_EML, 'ECORR@MYCORNERSTONELOAN.ORG') AS EMAILADDRESS,
	@SCRIPT_DATA_ID,
	'', --SOURCE FILE
	CentralData.dbo.CreateACSKeyline(AY10.BF_SSN,'B','L') +
	',' + PD10.DF_SPE_ACC_ID +
	',"' + RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) +'"' +
	',"' + RTRIM(PD30.DX_STR_ADR_1) + 
	'","' + RTRIM(PD30.DX_STR_ADR_2) +
	'","' + RTRIM(PD30.DM_CT) + 
	'",' + RTRIM(PD30.DC_DOM_ST) +
	',' + RTRIM(PD30.DF_ZIP_CDE) +
	',' + RTRIM(PD30.DM_FGN_ST) +
	',"' + RTRIM(PD30.DC_FGN_CNY) + '"' +
	',"' + MR65.CancelledPrincipalBalance + '"' + 
	',"' + MR65.InterestIncludedInCancellation + '"' + 
	',2020' AS LETTER_DATA,
	'MA4481',
	0,
	CASE WHEN PH05.DI_CNC_ELT_OPI = 'Y'  THEN 1 ELSE 0 END AS ON_ECORR, 
	0, --ARC NEEDED
	0, --IMAGING NEEDED
	getdate() as addedat,
	SUSER_SNAME() as addedBy

from 
	CDW..AY10_BR_LON_ATY AY10
	INNER JOIN CDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = AY10.BF_SSN		
	INNER JOIN 
	(
		SELECT
			MR65.BF_SSN,
			REPLACE(FORMAT(SUM(ISNULL(MR65.WA_PRI_INC_CRD,0)), 'C'), ',', '') AS CancelledPrincipalBalance,
			REPLACE(FORMAT(SUM(ISNULL(MR65.WA_INT_INC_CRD,0)), 'C'), ',', '') AS InterestIncludedInCancellation
		FROM
			CDW..MR65_MSC_TAX_RPT MR65
			INNER JOIN
			(
				SELECT
					MR65.BF_SSN,
					MR65.LN_SEQ,
					MAX(WF_CRT_DTS_MR65) AS WF_CRT_DTS_MR65
				FROM			
					CDW..MR65_MSC_TAX_RPT MR65
							
				WHERE
					MR65.LF_TAX_YR = '2020'
					AND MR65.WC_STA_MR65 = 'A'
				GROUP BY
					MR65.BF_SSN,
					MR65.LN_SEQ
			) MR65_AGG
				ON MR65_AGG.BF_SSN = MR65.BF_SSN
				AND MR65_AGG.LN_SEQ = MR65.LN_SEQ
				AND MR65_AGG.WF_CRT_DTS_MR65 = MR65.WF_CRT_DTS_MR65
		WHERE
			MR65.WC_STA_MR65 = 'A'
		GROUP BY 
			MR65.BF_SSN					
	) MR65
		ON MR65.BF_SSN = AY10.BF_SSN

	LEFT JOIN
	(
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
	) PD32
		ON PD32.DF_PRS_ID = AY10.BF_SSN
		AND PD32.EmailPriority = 1
	LEFT JOIN CDW..PH05_CNC_EML PH05
		ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
		AND PH05.DI_VLD_CNC_EML_ADR = 'Y'
	LEFT JOIN CDW..PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = AY10.BF_SSN
		AND PD30.DC_ADR = 'L'
		AND PD30.DI_VLD_ADR = 'Y'
	LEFT JOIN CLS.[print].PrintProcessing PP
		ON PP.AccountNumber = PD10.DF_SPE_ACC_ID
		AND PP.ScriptDataId = @SCRIPT_DATA_ID
		AND PP.AddedAt > AY10.LD_ATY_REQ_RCV
WHERE
	AY10.PF_REQ_ACT = 'TSCSM'
	AND ISNULL(AY10.PF_RSP_ACT,'') IN ('PRNTD','')
	AND AY10.LC_STA_ACTY10 = 'A'
	AND 
	(
		PH05.DI_CNC_ELT_OPI = 'Y' --ON ECRR
		OR PD30.DF_PRS_ID IS NOT NULL --HAS A VALID ADDRESS
	)
	AND PP.AccountNumber IS NULL --NOT ALREADY ADDED

--CO BORROWER PROCESSING
INSERT INTO CLS.[print].PrintProcessingCoBorrower(AccountNumber, EmailAddress, ScriptDataId, SourceFile, LetterData, CostCenter, DoNotProcessEcorr, OnEcorr, ArcNeeded, ImagingNeeded, AddedAt, AddedBy, BorrowerSsn)
SELECT DISTINCT
	PD10E.DF_SPE_ACC_ID,
	COALESCE(PH05.DX_CNC_EML_ADR,	PD32.DX_ADR_EML, 'ECORR@MYCORNERSTONELOAN.ORG') AS EMAILADDRESS,
	@SCRIPT_DATA_ID,
	'', --SOURCE FILE
	CentralData.dbo.CreateACSKeyline(AY10.BF_SSN,'B','L') +
	',' + PD10.DF_SPE_ACC_ID +
	',"' + RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) +'"' +
	',"' + RTRIM(PD30.DX_STR_ADR_1) + 
	'","' + RTRIM(PD30.DX_STR_ADR_2) +
	'","' + RTRIM(PD30.DM_CT) + 
	'",' + RTRIM(PD30.DC_DOM_ST) +
	',' + RTRIM(PD30.DF_ZIP_CDE) +
	',' + RTRIM(PD30.DM_FGN_ST) +
	',"' + RTRIM(PD30.DC_FGN_CNY) + '"' +
	',"' + MR65.CancelledPrincipalBalance + '"' + 
	',"' + MR65.InterestIncludedInCancellation + '"' +
	',2020' AS LETTER_DATA,
	'MA4481',
	0,
	CASE WHEN PH05.DI_CNC_ELT_OPI = 'Y'  THEN 1 ELSE 0 END AS ON_ECORR, 
	0, --ARC NEEDED
	0, --IMAGING NEEDED
	getdate() as addedat,
	SUSER_SNAME() as addedBy,
	PD10.DF_PRS_ID

from 
	CDW..AY10_BR_LON_ATY AY10
	INNER JOIN CDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = AY10.BF_SSN	
	INNER JOIN CDW..LN20_EDS LN20
		ON LN20.BF_SSN = AY10.BF_SSN
		AND LN20.LC_EDS_TYP = 'M'
	INNER JOIN CDW..PD10_PRS_NME PD10E
		ON PD10E.DF_PRS_ID = LN20.LF_EDS	
	INNER JOIN 
	(
		SELECT
			MR65.BF_SSN,
			REPLACE(FORMAT(SUM(ISNULL(MR65.WA_PRI_INC_CRD,0)), 'C'), ',', '') AS CancelledPrincipalBalance,
			REPLACE(FORMAT(SUM(ISNULL(MR65.WA_INT_INC_CRD,0)), 'C'), ',', '') AS InterestIncludedInCancellation
		FROM
			CDW..MR65_MSC_TAX_RPT MR65
			INNER JOIN CDW..LN20_EDS LN20
				ON LN20.BF_SSN = MR65.BF_SSN
				AND LN20.LN_SEQ = MR65.LN_SEQ
				AND LN20.LC_EDS_TYP = 'M'
			INNER JOIN
			(
				SELECT
					MR65.BF_SSN,
					MR65.LN_SEQ,
					MAX(WF_CRT_DTS_MR65) AS WF_CRT_DTS_MR65
				FROM			
					CDW..MR65_MSC_TAX_RPT MR65
							
				WHERE
					MR65.LF_TAX_YR = '2020'
					AND MR65.WC_STA_MR65 = 'A'
				GROUP BY
					MR65.BF_SSN,
					MR65.LN_SEQ
			) MR65_AGG
				ON MR65_AGG.BF_SSN = MR65.BF_SSN
				AND MR65_AGG.LN_SEQ = MR65.LN_SEQ
				AND MR65_AGG.WF_CRT_DTS_MR65 = MR65.WF_CRT_DTS_MR65
		WHERE
			MR65.WC_STA_MR65 = 'A'
		GROUP BY 
			MR65.BF_SSN					
	) MR65
		ON MR65.BF_SSN = AY10.BF_SSN

	LEFT JOIN
	(
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
	) PD32
		ON PD32.DF_PRS_ID = PD10E.DF_PRS_ID
		AND PD32.EmailPriority = 1
	LEFT JOIN CDW..PH05_CNC_EML PH05
		ON PH05.DF_SPE_ID = PD10E.DF_SPE_ACC_ID
		AND PH05.DI_VLD_CNC_EML_ADR = 'Y'
	LEFT JOIN CDW..PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = PD10E.DF_PRS_ID
		AND PD30.DC_ADR = 'L'
		AND PD30.DI_VLD_ADR = 'Y'
	LEFT JOIN CLS.[print].PrintProcessingCoBorrower PP
		ON PP.AccountNumber = PD10E.DF_SPE_ACC_ID
		and pp.BorrowerSsn = PD10.DF_PRS_ID
		AND PP.ScriptDataId = @SCRIPT_DATA_ID
		AND PP.AddedAt > AY10.LD_ATY_REQ_RCV
WHERE
	AY10.PF_REQ_ACT = 'TSCSM'
	AND ISNULL(AY10.PF_RSP_ACT,'') IN ('PRNTD','')
	AND AY10.LC_STA_ACTY10 = 'A'
	AND 
	(
		PH05.DI_CNC_ELT_OPI = 'Y' --ON ECRR
		OR PD30.DF_PRS_ID IS NOT NULL --HAS A VALID ADDRESS
	)
	AND PP.AccountNumber IS NULL --NOT ALREADY ADDED