--HEP Disqualification =  loans disbursed before May 1, 2006 and became 15-30 days past due on a 4th bill. (They satisifed the bill more than 15 days after the due date)

--Please identify the following populations:
--1) Closed loans that have erroneously achieved the reduced interest rate
		--POP 1:
		--LA_CUR_PR = '0'
		--LN55.LC_LON_BBT_STA = 'R' 
		--UTLWO39 Billing logic

--2) Open loans that have erroneously achieved the reduced interest rate
		--POP 2 = 
		--LA_CUR_PR > '0'
		--LN55.LC_LON_BBT_STA = 'R' 
		--UTLWO39 Billing logic

--3) Open loans working towards the benefit that should have already been disqualified
		--POP 3 = 
		--LA_CUR_PR > '0'
		--LN55.LC_LON_BBT_STA = 'Q' 
		--UTLWO39 Billing logic

--Output for each population to include:  
--Account
--Loan
--Principal
--RateType (Fixed/Variable)
--Base Rate
--Charged Rate
--BBP (Borrower Benefit Plan)
--BBP Counter
--Disqual Date (Date they should have been disqualified)
--Qualified Date (Date they achieved the benefit)

USE UDW
GO

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DROP TABLE IF EXISTS  #POP2;



/********************************* POP 2 **********************************/
--2) Open loans that have erroneously achieved the reduced interest rate
		--POP 2 = 
		--LA_CUR_PR > '0'
		--LN55.LC_LON_BBT_STA = 'R' 
		--UTLWO39 Billing logic


--EMAIL

--TPBEM
--Email for removal of benefit.

--LETTER
--TPBML
--Letter for removal of benefit
SELECT
	'POP 2' AS POP,
	*
INTO
	#POP2
FROM
	(
		SELECT
			*,
			ROW_NUMBER() OVER(PARTITION BY BF_SSN, LN_SEQ, DLQ_INSTANCES 
								  ORDER BY BF_SSN, LN_SEQ, DLQ_INSTANCES, LD_BIL_DU) AS DLQ_COUNTER
		FROM
			(
				SELECT DISTINCT 
					PD10.DF_SPE_ACC_ID AS Account
					,LN10.BF_SSN
					,LN10.LN_SEQ 
					,LN10.LA_CUR_PRI AS Principal
					,CASE 
						WHEN LN72.LC_ITR_TYP = 'F1'
						THEN 'FIXED'
						ELSE 'VARIABLE' 
					END AS RateType
					,LN72.LR_INT_RDC_PGM_ORG AS BaseRate
					,LN72.LR_ITR AS ChargedRate
					,LN54.PM_BBS_PGM AS BBP
					,LN54.LN_BBS_STS_PCV_PAY AS CounterPCV -- Preconversion Payments
					,LN55.LN_LON_BBT_PAY_OVR AS CounterOVR -- Override Counter Payments 
					,LN55.LN_LON_BBT_PAY AS CounterRCV -- Compass-Received Payments
					,LN55.LD_LON_BBT_STA AS QualifiedDate -- (Date they achieved the benefit)

					,BL10.LD_BIL_CRT	
					,BL10.LN_SEQ_BIL_WI_DTE
					,BL10.LD_BIL_DU
					,'A' AS SCN_STA		
		
					,CAST(LN80.LD_BIL_DU_LON AS DATE) AS BILL_DUE
					,CAST(LN80.LD_BIL_STS_RIR_TOL AS DATE) AS SATISFIED
					,DATEDIFF(DAY,LN80.LD_BIL_DU_LON, LN80.LD_BIL_STS_RIR_TOL) AS DIFF
					,CASE
						WHEN DATEDIFF(DAY,LN80.LD_BIL_DU_LON, LN80.LD_BIL_STS_RIR_TOL) > 14
						THEN 1
						ELSE NULL
					END AS DLQ_INSTANCES
				FROM
					LN80_LON_BIL_CRF LN80
					INNER JOIN BL10_BR_BIL BL10
						ON BL10.BF_SSN = LN80.BF_SSN
						AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
						AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
					INNER JOIN LN10_LON LN10
 						ON LN80.BF_SSN = LN10.BF_SSN
						AND LN80.LN_SEQ = LN10.LN_SEQ
					INNER JOIN PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = LN10.BF_SSN
					INNER JOIN LN54_LON_BBS LN54 
						ON  LN54.BF_SSN = LN10.BF_SSN 
						AND LN54.LN_SEQ = LN10.LN_SEQ
						AND LN54.LC_STA_LN54 = 'A'
						AND LN54.LC_BBS_ELG = 'Y' --LOAN IS STILL ELIGIBLE FOR THE BB
					INNER JOIN LN55_LON_BBS_TIR LN55 
						ON  LN55.BF_SSN = LN54.BF_SSN 
						AND LN55.LN_SEQ = LN54.LN_SEQ
						AND LN55.PM_BBS_PGM = LN54.PM_BBS_PGM
						AND LN55.PN_BBS_PGM_SEQ = LN54.PN_BBS_PGM_SEQ
						AND LN55.LN_LON_BBS_SEQ = LN54.LN_LON_BBS_PGM_SEQ
						AND LN55.LC_STA_LN55 = 'A'
					LEFT JOIN LN72_INT_RTE_HST LN72 
						ON  LN72.BF_SSN = LN10.BF_SSN
						AND LN72.LN_SEQ = LN10.LN_SEQ
						AND LN72.LC_STA_LON72 = 'A'
						AND LN72.LD_ITR_EFF_BEG <= CONVERT(DATE,GETDATE())
						AND LN72.LD_ITR_EFF_END >= CONVERT(DATE,GETDATE())
				WHERE 
					BL10.LC_BIL_TYP = 'P'
					AND BL10.LC_STA_BIL10 = 'A'
					AND LN80.LI_BIL_DLQ_OVR_RIR <> 'Y'	
					AND LN10.LC_STA_LON10 = 'R' --RELEASED STATUS
					AND LN10.LD_LON_1_DSB < CONVERT(DATE,'20060501') --DISB BEFORE 05/01/2006 
					AND LN10.LF_LON_CUR_OWN = '828476' --UHEAA OWNER CODE (NOT A BANA, ALIGN, OR TILP LOAN) 
					AND LN10.LA_CUR_PRI > 0.00 --HAS BALANCE
					AND LN55.LC_LON_BBT_STA = 'R' --BORR IS WORKING TOWARDS RIR (R=ReductionAchieved, Q=WorkingTowardsQual, D=Disqual, N=NotElg)
			)X
		WHERE 
			DLQ_INSTANCES IS NOT NULL
	)Y
WHERE
	DLQ_COUNTER = 4
	AND QualifiedDate > LD_BIL_DU --aka exclude if disqualification date is after qualified date. Use the 4th instance of delinquent LD_BIL_DU as the disqualification date
	AND Account NOT IN ('0689328808','9487965272','2385810673','4510210705','3899130513')

;


DROP TABLE IF EXISTS #BWR;
DROP TABLE IF EXISTS #COBWR;
SELECT DISTINCT 
	Account, 
	CentralData.dbo.PascalCase(PD10.DM_PRS_1) AS [NAME],
	COALESCE(PH05.DX_CNC_EML_ADR,PD32.DX_ADR_EML) AS EMAIL,
	CASE 
		WHEN COALESCE(PH05.DX_CNC_EML_ADR,PD32.DX_ADR_EML) IS NOT NULL THEN 'EMAIL'
		WHEN PD30.DF_PRS_ID IS NOT NULL THEN 'PRINT'
		ELSE NULL
	END AS CORR_METHOD,
	PD30.DI_VLD_ADR,
	CentralData.dbo.CreateACSKeyline(POP.BF_SSN,'B','L') +
	',' + PD10.DF_SPE_ACC_ID +
	',"' + RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_LST) +'"' +
	',"' + RTRIM(PD30.DX_STR_ADR_1) + 
	'","' + RTRIM(PD30.DX_STR_ADR_2) +
	'","' + RTRIM(PD30.DM_CT) + 
	'",' + RTRIM(PD30.DC_DOM_ST) +
	',' + RTRIM(PD30.DF_ZIP_CDE) +
	',"' + RTRIM(PD30.DC_FGN_CNY) + '"' AS CommunicationLetterData
INTO #BWR
FROM 
	#POP2 POP
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_PRS_ID = POP.BF_SSN
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
					UDW..PD32_PRS_ADR_EML PD32
					INNER JOIN UDW..PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
				WHERE
					PD32.DI_VLD_ADR_EML = 'Y' -- valid email address
					AND PD32.DC_STA_PD32 = 'A' -- active email address record
			) Email
	) PD32
		ON PD32.DF_PRS_ID = POP.BF_SSN
		AND PD32.EmailPriority = 1
	LEFT JOIN UDW..PH05_CNC_EML PH05
		ON PH05.DF_SPE_ID = POP.Account
		AND PH05.DI_VLD_CNC_EML_ADR = 'Y'
	LEFT JOIN UDW..PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = POP.BF_SSN
		AND PD30.DC_ADR = 'L'
		AND PD30.DI_VLD_ADR = 'Y'

DECLARE @EMAILID INT = (SELECT 
	EC.EmailCampaignId
FROM 
	ULS.emailbatch.EmailCampaigns EC 
	INNER JOIN ULS.emailbatch.HTMLFiles H 
		ON H.HTMLFileId = EC.HTMLFileId 
where 
	h.HTMLFile = 'TPBIRRMLE.html')

INSERT INTO ULS.emailbatch.EmailProcessing(EmailCampaignId, AccountNumber, ActualFile, EmailData, ArcNeeded,  AddedBy, AddedAt)
SELECT DISTINCT
	@EMAILID,
	B.Account,
	NULL,
	(B.Account + ',' +B.[NAME] + ',' +B.EMAIL) AS EMAIL_DATA,
	0,
	'UNH 68848',
	GETDATE()
FROM
	#BWR b
WHERE
	B.CORR_METHOD = 'EMAIL'


INSERT INTO ULS..ArcAddProcessing(AccountNumber, ArcTypeId, ARC, Comment, CreatedAt, CreatedBy, ScriptId, ProcessOn, IsReference, IsEndorser)
SELECT
	B.Account,
	1, --ARC_TYPE
	'TPBEM', -- ARC
	'Email for removal of benefit', --COMMENT
	GETDATE(), --CREATEDAT,
	SUSER_NAME(), --CREATEDBY
	'UNH 68848', --SCRIPTID
	GETDATE(), --PROCESSON
	0, --ISREFERENCE
	0 --INENDORSER
FROM
	#BWR B
WHERE
	B.CORR_METHOD = 'EMAIL'

--TPBEM
--Email for removal of benefit.
DECLARE @SCRIPTDATAID INT = (
SELECT
	SD.ScriptDataId
FROM
	ULS.[print].ScriptData SD
WHERE
	SD.ScriptID = 'TPBIRRMLL'
)
INSERT INTO ULS.[print].PrintProcessing(AccountNumber, EmailAddress, ScriptDataId, SourceFile, LetterData, CostCenter, InValidAddress, DoNotProcessEcorr, OnEcorr, ArcNeeded, ImagingNeeded, AddedBy, AddedAt)
SELECT DISTINCT
	B.Account,
	B.EMAIL,
	@SCRIPTDATAID,
	NULL,
	B.CommunicationLetterData,
	'MA4119',
	0,
	1,
	0,
	0,
	0,
	'UNH 68848',
	GETDATE()
FROM
	#BWR b
WHERE
	B.CORR_METHOD = 'PRINT'



		
SELECT DISTINCT 
	Account, 
	PD10E.DF_SPE_ACC_ID,
	CentralData.dbo.PascalCase(PD10E.DM_PRS_1) AS [NAME],
	POP.LN_SEQ,
	COALESCE(PH05.DX_CNC_EML_ADR,PD32.DX_ADR_EML) AS EMAIL,
	CASE 
		WHEN COALESCE(PH05.DX_CNC_EML_ADR,PD32.DX_ADR_EML) IS NOT NULL THEN 'EMAIL'
		WHEN PD30.DF_PRS_ID IS NOT NULL THEN 'PRINT'
		ELSE NULL
	END AS CORR_METHOD,
	PD30.DI_VLD_ADR,
	CentralData.dbo.CreateACSKeyline(POP.BF_SSN,'B','L') +
	',' + PD10E.DF_SPE_ACC_ID +
	',"' + RTRIM(PD10E.DM_PRS_1) + ' ' + RTRIM(PD10E.DM_PRS_LST) +'"' +
	',"' + RTRIM(PD30.DX_STR_ADR_1) + 
	'","' + RTRIM(PD30.DX_STR_ADR_2) +
	'","' + RTRIM(PD30.DM_CT) + 
	'",' + RTRIM(PD30.DC_DOM_ST) +
	',' + RTRIM(PD30.DF_ZIP_CDE) +
	',"' + RTRIM(PD30.DC_FGN_CNY) + '"' AS CommunicationLetterData
INTO #COBWR
FROM 
	#POP2 POP
	INNER JOIN UDW..LN20_EDS LN20
		ON LN20.BF_SSN = POP.BF_SSN
		AND LN20.LN_SEQ = POP.LN_SEQ
		AND LN20.LC_EDS_TYP = 'M'
	INNER JOIN UDW..PD10_PRS_NME PD10E
		ON PD10E.DF_PRS_ID = LN20.LF_EDS
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
					UDW..PD32_PRS_ADR_EML PD32
					INNER JOIN UDW..PD10_PRS_NME PD10
						ON PD10.DF_PRS_ID = PD32.DF_PRS_ID
				WHERE
					PD32.DI_VLD_ADR_EML = 'Y' -- valid email address
					AND PD32.DC_STA_PD32 = 'A' -- active email address record
			) Email
	) PD32
		ON PD32.DF_PRS_ID = LN20.BF_SSN
		AND PD32.EmailPriority = 1
	LEFT JOIN UDW..PH05_CNC_EML PH05
		ON PH05.DF_SPE_ID = PD10E.DF_SPE_ACC_ID
		AND PH05.DI_VLD_CNC_EML_ADR = 'Y'
	LEFT JOIN UDW..PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = LN20.BF_SSN
		AND PD30.DC_ADR = 'L'
		AND PD30.DI_VLD_ADR = 'Y'

INSERT INTO ULS.emailbatch.EmailProcessing(EmailCampaignId, AccountNumber, ActualFile, EmailData, ArcNeeded,  AddedBy, AddedAt)
SELECT DISTINCT
	@EMAILID,
	B.DF_SPE_ACC_ID,
	NULL,
	(B.Account + ',' +B.[NAME] + ',' +B.EMAIL) AS EMAIL_DATA,
	0,
	'UNH 68848',
	GETDATE()
FROM
	#COBWR b
WHERE
	B.CORR_METHOD = 'EMAIL'


DECLARE @COARC TABLE(ArcAddProcessingId int NOT NULL, RecipientId VARCHAR(9) NOT NULL, AccountNumber VARCHAR(10));
INSERT INTO ULS..ArcAddProcessing(AccountNumber, ArcTypeId, ARC, Comment, CreatedAt, CreatedBy, ScriptId, ProcessOn, IsReference, IsEndorser, RecipientId)
OUTPUT Inserted.ArcAddProcessingId, Inserted.RecipientId, Inserted.AccountNumber
INTO @COARC
SELECT DISTINCT
	PD10B.DF_SPE_ACC_ID, --BORROWERS ACCT #
	0, --ARC_TYPE
	'TPBEM', -- ARC
	'Email for removal of benefit', --COMMENT
	GETDATE(), --CREATEDAT,
	SUSER_NAME(), --CREATEDBY
	'UNH 68848', --SCRIPTID
	GETDATE(), --PROCESSON
	0, --ISREFERENCE
	1,	 --ISENDORSER
	PD10.DF_PRS_ID -- RECIPIENTID
FROM
	#COBWR P
	INNER JOIN UDW..PD10_PRS_NME PD10 --cobwr
		ON PD10.DF_SPE_ACC_ID = P.DF_SPE_ACC_ID
	INNER JOIN UDW..LN20_EDS LN20
		ON LN20.LF_EDS = PD10.DF_PRS_ID
	INNER JOIN UDW..PD10_PRS_NME PD10B
		ON PD10B.DF_PRS_ID = LN20.BF_SSN


INSERT INTO ULS..ArcLoanSequenceSelection(ArcAddProcessingId, LoanSequence)
SELECT
	CA.ArcAddProcessingId,
	LN20.LN_SEQ
FROM
	@COARC CA
	INNER JOIN UDW..PD10_PRS_NME PD10B
		ON PD10B.DF_SPE_ACC_ID = CA.AccountNumber
	INNER JOIN UDW..LN20_EDS LN20
		ON LN20.LF_EDS = CA.RecipientId
		AND LN20.BF_SSN = PD10B.DF_PRS_ID