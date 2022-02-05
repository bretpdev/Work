/****************************************************************
			GET BASE POPULATION
****************************************************************/
--all borrowers living in affected zip codes

DROP TABLE IF EXISTS #FED_FORB;

DECLARE @Now DATETIME = GETDATE();

SELECT
	Base.DF_SPE_ACC_ID,
	Base.BF_SSN,
	Base.LN_SEQ,
	Base.LC_STA_LON10,
	Base.LD_END_GRC_PRD,
	Base.WC_DW_LON_STA,
	Base.LA_CUR_PRI,
	Base.ZipCode,
	Base.DisasterId,
	Base.Disaster,
	Base.BeginDate,
	Base.EndDate,
	Base.MaxEndDate,
	Base.Active,
	Base.LD_DLQ_OCC,
	Base.LN_DLQ_MAX,
	Base.LA_RPS_ISL, 
	Base.EARLIEST_DISASTER, 
	Base.DISASTER_INSTANCES, 
	Base.BeginDateScriptData,
	Base.Comment
INTO 
	#FED_FORB
FROM
	(
		SELECT DISTINCT 
			PD10.DF_SPE_ACC_ID,
			LN10.BF_SSN,
			LN10.LN_SEQ,
			LN10.LC_STA_LON10,
			LN10.LD_END_GRC_PRD,
			DW01.WC_DW_LON_STA,
			LN10.LA_CUR_PRI,
			PD30_VLA.ZipCode,
			PD30_VLA.DisasterId,
			PD30_VLA.Disaster,
			PD30_VLA.BeginDate,
			PD30_VLA.EndDate,
			PD30_VLA.MaxEndDate,
			PD30_VLA.Active,
			LN16_mins.LD_DLQ_OCC,
			LN16_mins.LN_DLQ_MAX,
			RS.LA_RPS_ISL, --flagged for exclusion: current payment amount is zero dollars
			DENSE_RANK() OVER(PARTITION BY PD10.DF_SPE_ACC_ID ORDER BY PD30_VLA.DisasterId) AS EARLIEST_DISASTER, --flags earliest disaster=1,next=2,3,4,etc.
			DENSE_RANK() OVER(PARTITION BY PD10.DF_SPE_ACC_ID ORDER BY PD30_VLA.DisasterId)--first disaster
			+ DENSE_RANK() OVER (PARTITION BY PD10.DF_SPE_ACC_ID ORDER BY PD30_VLA.DisasterId DESC)--latest disaster
			- 1 AS DISASTER_INSTANCES, --counts # of disasters borrower has been in (ex. 1 disaster=1+1-1 | 2 disasters=1+2-1 | 3 disasters=1+3-1)
			CASE WHEN LN16_mins.LD_DLQ_OCC > PD30_VLA.BeginDate
				THEN LN16_mins.LD_DLQ_OCC
				ELSE PD30_VLA.BeginDate
			END AS BeginDateScriptData,
			CASE WHEN RS.LA_RPS_ISL IS NOT NULL AND LN16_mins.BF_SSN IS NOT NULL --SR 5024
				THEN 'Please review account: borrower has a 0.00 payment and delinquent. DisasterId: ' + CAST(DisasterId AS VARCHAR(10)) 
				ELSE NULL 
			END AS Comment
		FROM
			CDW..LN10_LON LN10
			INNER JOIN CDW..PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
			INNER JOIN CDW..DW01_DW_CLC_CLU DW01
				ON LN10.BF_SSN = DW01.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
			INNER JOIN 
			(--get valid legal address
				SELECT
					ADR.DF_PRS_ID,
					ADR.ZipCode,
					ADR.DisasterId,
					ADR.Disaster,
					ADR.BeginDate,
					ADR.EndDate,
					ADR.MaxEndDate,
					ADR.Active,
					ROW_NUMBER() OVER (PARTITION BY ADR.DF_PRS_ID ORDER BY ADR.PriorityNumber) AS AddressPriority -- number in order of Address
				FROM
				(--get valid address of all borrowers in affected active areas
					SELECT
						PD30.DF_PRS_ID,
						ZIP.ZipCode,
						DIS.DisasterId,
						DIS.Disaster,
						DIS.BeginDate,
						DIS.EndDate,
						DIS.MaxEndDate,
						DIS.Active,
						CASE WHEN DC_ADR = 'L' THEN 1 -- legal 
 							 WHEN DC_ADR = 'B' THEN 2 -- billing
 							 WHEN DC_ADR = 'D' THEN 3 -- disbursement
 						END AS PriorityNumber
					FROM
						CDW..PD30_PRS_ADR PD30
						INNER JOIN CLS.dasforbfed.Zips ZIP
							ON ZIP.ZipCode = SUBSTRING(PD30.DF_ZIP_CDE, 1, 5)
						INNER JOIN CLS.dasforbfed.Disasters DIS
							ON ZIP.DisasterId = DIS.DisasterId
					WHERE
						PD30.DI_VLD_ADR = 'Y'
						AND DIS.Active = 1
				) ADR
			) PD30_VLA --valid legal address
				ON PD30_VLA.DF_PRS_ID = LN10.BF_SSN
				AND PD30_VLA.AddressPriority = 1
			LEFT JOIN 
			(--must be left join since borrowers only get a record when delq hits 1 and will never see ln_dlq_max=0
				SELECT
					LN16.BF_SSN,
					CAST(MIN(LD_DLQ_OCC) AS DATE) AS LD_DLQ_OCC,
					MAX(LN_DLQ_MAX) + 1 AS LN_DLQ_MAX -- +1 to include current day
				FROM
					CDW..LN16_LON_DLQ_HST LN16
					INNER JOIN CDW..LN10_LON LN10
						ON LN10.BF_SSN = LN16.BF_SSN
						AND LN10.LN_SEQ = LN16.LN_SEQ
				WHERE 
					LN16.LC_STA_LON16 = '1'
					AND LN10.LA_CUR_PRI > 0.00
					AND LN10.LC_STA_LON10 = 'R'
				GROUP BY	
					LN16.BF_SSN
			) LN16_mins
				ON LN10.BF_SSN = LN16_mins.BF_SSN
			LEFT JOIN CDW.calc.RepaymentSchedules RS --flagged for exclusion: current payment amount is zero dollars
				ON LN10.BF_SSN = RS.BF_SSN
				AND LN10.LN_SEQ = RS.LN_SEQ
				AND RS.LA_RPS_ISL = 0.00
				AND RS.CurrentGradation = 1
			LEFT JOIN
			(--flag for exclusion: forbearances that fall inside disaster begin/end dates
				SELECT DISTINCT
					LN60.BF_SSN,
					LN60.LN_SEQ,
					LN60.LD_FOR_BEG,
					LN60.LD_FOR_END
				FROM
					CDW..FB10_BR_FOR_REQ FB10
					INNER JOIN CDW..LN60_BR_FOR_APV LN60
						ON FB10.BF_SSN = LN60.BF_SSN
						AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM	
				WHERE
					FB10.LC_FOR_TYP = '40'
					AND FB10.LC_FOR_STA = 'A'
					AND FB10.LC_STA_FOR10 = 'A'
					AND LN60.LC_STA_LON60 = 'A'
					AND LN60.LC_FOR_RSP != '003' --Exclude denial
			) ExistingForb
				ON ExistingForb.BF_SSN = LN10.BF_SSN
				AND ExistingForb.LN_SEQ = LN10.LN_SEQ
				AND CAST(ExistingForb.LD_FOR_BEG AS DATE) BETWEEN CAST(PD30_VLA.BeginDate AS DATE) AND CAST(PD30_VLA.EndDate AS DATE)
			LEFT JOIN CDW..WQ20_TSK_QUE WQ20 --flagged for exclusion: have open place holder queue task
				ON LN10.BF_SSN = WQ20.BF_SSN
				AND WQ20.WF_QUE = '40'
				AND WQ20.WF_SUB_QUE = '01'
				AND WQ20.WC_STA_WQUE20 IN ('A','H','P','U','W')
		WHERE
			LN10.LC_STA_LON10 = 'R'
			AND DW01.WC_DW_LON_STA NOT IN ('16','17','18','19','20','21') --SR 5024
			AND DW01.WX_OVR_DW_LON_STA NOT IN('CNSLD-STOP PURSUIT') --SR 5024
			AND LN10.LA_CUR_PRI > 0.00
			AND WQ20.BF_SSN IS NULL
			AND ExistingForb.BF_SSN IS NULL --excludes forbearances that fall inside disaster begin/end dates
			--SR 5024
			AND COALESCE(LN16_mins.LD_DLQ_OCC,'1900-01-01') < CAST(PD30_VLA.EndDate AS DATE) --delinquency occurs before the disaster 90 day end date
	) Base
WHERE
	Base.EARLIEST_DISASTER = 1
;

/****************************************************************
			MANUAL REVIEW POP
****************************************************************/
INSERT INTO CLS..ArcAddProcessing (ArcTypeId, AccountNumber, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, ProcessingAttempts, CreatedAt, CreatedBy)
SELECT DISTINCT
	2 AS ArcTypeId,
	FF.DF_SPE_ACC_ID AS AccountNumber,
	'DASFB' AS ARC,
	'DASFORBFED' AS ScriptId,
	@Now AS ProcessOn,
	FF.Comment,
	0 AS IsReference,
	0 AS IsEndorser,
	0 AS ProcessingAttempts,
	@Now AS CreatedAt,
	SUSER_SNAME() AS CreatedBy
FROM
	#FED_FORB FF
	LEFT JOIN CLS..ArcAddProcessing ExistingAAP
		ON ExistingAAP.AccountNumber = FF.DF_SPE_ACC_ID
		AND ExistingAAP.ARC = 'DASFB'
		AND ExistingAAP.ScriptId = 'DASFORBFED'
		--Comment having disasterId appended will not affect script as this is just comments for manual review accounts, and will make a row unique without needing to parse on date.
		AND COALESCE(ExistingAAP.Comment,'') = FF.Comment
		AND CAST(ExistingAAP.CreatedAt AS DATE) BETWEEN CAST(FF.BeginDate AS DATE) AND CAST(FF.EndDate AS DATE)--SR 5024 Riley asked to use the disaster begin and end date to exclude the email
WHERE
	FF.Comment IS NOT NULL
	AND ExistingAAP.AccountNumber IS NULL --No matching existing record
;

/****************************************************************
			90 DAY FORBEARANCE POP LOGIC
				SCRIPT DATA INSERT
SHOULD MATCH SSRS REPORT AND DELINQUENT EMAILPROCESSING GROUP
****************************************************************/
INSERT INTO CLS.dasforbfed.ProcessQueue (AccountNumber, BeginDate, EndDate, AddedAt, AddedBy, DisasterId, ForbearanceTypeId)
SELECT DISTINCT
	FF.DF_SPE_ACC_ID,
	FF.BeginDateScriptData,
	FF.EndDate,
	@Now AS AddedAt,
	SUSER_SNAME() AS AddedBy,
	FF.DisasterId,
	--FF.LD_DLQ_OCC, --SR 5024
	1 --90 day forb type
FROM
	#FED_FORB FF
	INNER JOIN CDW.calc.RepaymentSchedules RS --SR 5024
		ON FF.BF_SSN = RS.BF_SSN
		AND FF.LN_SEQ = RS.LN_SEQ
		AND RS.LA_RPS_ISL > 0.00
		AND RS.CurrentGradation = 1
	LEFT JOIN CLS.dasforbfed.ProcessQueue ExistingPQ
		ON ExistingPQ.AccountNumber = FF.DF_SPE_ACC_ID
		AND ExistingPQ.BeginDate = FF.BeginDateScriptData
		AND ExistingPQ.EndDate = FF.EndDate
		AND ExistingPQ.DisasterId = FF.DisasterId
		AND ExistingPQ.ForbearanceTypeId = 1
WHERE
	FF.Comment IS NULL
	AND FF.LD_DLQ_OCC < CAST(FF.EndDate AS DATE) --delinquency occurs before the disaster 90 day end date
	AND FF.LN_DLQ_MAX >= 5 --SR 5024
	AND ExistingPQ.AccountNumber IS NULL --No matching existing record
;

/****************************************************************
				DELINQUENT EMAIL FILE
				30 & 90 DAY POPULATIONS
****************************************************************/
INSERT INTO	CLS.emailbtcf.CampaignData (EmailCampaignId, Recipient, AccountNumber, FirstName, LastName, AddedAt, AddedBy)
SELECT DISTINCT
	59 AS EmailCampaignId, --59 for live, 127 for test
	COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) AS Recipient,
	FF.DF_SPE_ACC_ID AS AccountNumber,
	PD10.DM_PRS_1 AS FirstName,
	PD10.DM_PRS_LST AS LastName,
	GETDATE() AS AddedAt,
	SUSER_SNAME() AS AddedBy
FROM
	#FED_FORB FF
	INNER JOIN CDW..PD10_PRS_NME PD10
		ON FF.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN CDW.calc.RepaymentSchedules RS
		ON FF.BF_SSN = RS.BF_SSN
		AND FF.LN_SEQ = RS.LN_SEQ
		AND RS.LA_RPS_ISL > 0.00
		AND RS.CurrentGradation = 1
	LEFT JOIN CDW..PH05_CNC_EML PH05 
		ON PH05.DF_SPE_ID = FF.DF_SPE_ACC_ID
		AND	PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
		AND PH05.DI_CNC_ELT_OPI = 'Y' --on ecorr
	LEFT JOIN
	( -- email address
		SELECT 
			DF_PRS_ID, 
			Email.EM AS ALT_EM,
 			ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) AS EmailPriority -- number in order of Email.PriorityNumber 
 		FROM 
 		( 
 			SELECT 
 				PD32.DF_PRS_ID, 
				PD32.DX_ADR_EML AS EM, 
 				CASE WHEN DC_ADR_EML = 'H' THEN 1 -- home 
 					 WHEN DC_ADR_EML = 'A' THEN 2 -- alternate 
 					 WHEN DC_ADR_EML = 'W' THEN 3 -- work 
 				END AS PriorityNumber
 			FROM 
 				CDW..PD32_PRS_ADR_EML PD32 
 			WHERE 
 				PD32.DI_VLD_ADR_EML = 'Y' -- valid email address 
 				AND PD32.DC_STA_PD32 = 'A' -- active email address record 
 		) Email 
	) PD32 
		ON PD32.DF_PRS_ID = FF.BF_SSN
		AND PD32.EmailPriority = 1
	LEFT JOIN CLS.emailbtcf.CampaignData ExistingCD --SR 5024
		ON ExistingCD.EmailCampaignId = 59 --59 for live, 127 for test
		AND ExistingCD.Recipient = COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM)
		AND ExistingCD.AccountNumber = FF.DF_SPE_ACC_ID
		AND ExistingCD.FirstName = PD10.DM_PRS_1
		AND ExistingCD.LastName = PD10.DM_PRS_LST
WHERE
	FF.Comment IS NULL
	AND FF.LD_DLQ_OCC < CAST(FF.EndDate AS DATE)
	AND COALESCE(PH05.DX_CNC_EML_ADR,PD32.ALT_EM) IS NOT NULL
	AND COALESCE(FF.LN_DLQ_MAX,0) >= 5
	AND ExistingCD.AccountNumber IS NULL --No existing record SR 5024
;

/****************************************************************
				CURRENT EMAIL FILE
		BASE POPULATION - NON-DELINQUENT BORROWERS
****************************************************************/
INSERT INTO CLS.emailbtcf.CampaignData (EmailCampaignId, Recipient, AccountNumber, FirstName, LastName, AddedAt, AddedBy)
SELECT DISTINCT
	58 AS EmailCampaignId, --58 for live, 126 for test
	COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) AS Recipient,
	FF.DF_SPE_ACC_ID AS AccountNumber,
	PD10.DM_PRS_1 AS FirstName,
	PD10.DM_PRS_LST AS LastName,
	GETDATE() AS AddedAt,
	SUSER_SNAME() AS AddedBy
FROM
	#FED_FORB FF
	INNER JOIN CDW..DW01_DW_CLC_CLU DW01
		ON FF.BF_SSN = DW01.BF_SSN
		AND FF.LN_SEQ = DW01.LN_SEQ
		AND DW01.WC_DW_LON_STA = '03'
	INNER JOIN CDW..PD10_PRS_NME PD10
		ON FF.BF_SSN = PD10.DF_PRS_ID
	LEFT JOIN CDW..PH05_CNC_EML PH05 
		ON PH05.DF_SPE_ID = FF.DF_SPE_ACC_ID
		AND	PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
		AND PH05.DI_CNC_ELT_OPI = 'Y' --on ecorr
	LEFT JOIN
	( -- email address
		SELECT 
			DF_PRS_ID, 
			Email.EM AS ALT_EM,
 			ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) AS EmailPriority -- number in order of Email.PriorityNumber 
 		FROM 
 		( 
 			SELECT 
 				PD32.DF_PRS_ID, 
 				PD32.DX_ADR_EML AS EM, 
 				CASE WHEN DC_ADR_EML = 'H' THEN 1 -- home 
 					 WHEN DC_ADR_EML = 'A' THEN 2 -- alternate 
 					 WHEN DC_ADR_EML = 'W' THEN 3 -- work 
 				END AS PriorityNumber
 			FROM 
 				CDW..PD32_PRS_ADR_EML PD32 
 			WHERE 
 				PD32.DI_VLD_ADR_EML = 'Y' -- valid email address 
 				AND PD32.DC_STA_PD32 = 'A' -- active email address record 
 		) Email 
	) PD32 
		ON PD32.DF_PRS_ID = FF.BF_SSN
		AND PD32.EmailPriority = 1
	LEFT JOIN 
	(--flagged for exclusion: exclude borrowers who received email in last 30 days
		SELECT
			BF_SSN,
			MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
		FROM
			CDW..AY10_BR_LON_ATY
		WHERE
			PF_REQ_ACT = 'NDSNF'
			AND CAST(LD_ATY_REQ_RCV AS DATE) BETWEEN CAST(DATEADD(DAY,-30,@Now) AS DATE) AND CAST(@Now AS DATE)
		GROUP BY
			BF_SSN
	)AY10M
		ON FF.BF_SSN = AY10M.BF_SSN
	LEFT JOIN CLS.emailbtcf.CampaignData ExistingCD
		ON ExistingCD.EmailCampaignId = 58 --58 for live, 126 for test
		AND ExistingCD.Recipient = COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM)
		AND ExistingCD.AccountNumber = FF.DF_SPE_ACC_ID
		AND ExistingCD.FirstName = PD10.DM_PRS_1
		AND ExistingCD.LastName = PD10.DM_PRS_LST
		AND CAST(ExistingCD.AddedAt AS DATE) BETWEEN CAST(DATEADD(DAY,-30,@Now) AS DATE) AND CAST(@Now AS DATE)
WHERE
	AY10M.BF_SSN IS NULL --excludes
	AND	COALESCE(PH05.DX_CNC_EML_ADR,PD32.ALT_EM) IS NOT NULL
	AND COALESCE(FF.LN_DLQ_MAX,0) < 5 --non-delinquent borrowers
	AND ExistingCD.AccountNumber IS NULL --No existing record in the last 30 days
;
/****************************************************************
		DAILY SSRS REPORT BASED ON SCRIPT DATA INSERT
****************************************************************/
TRUNCATE TABLE CLS.dasforbfed.UTNWS44_SSRS;

INSERT INTO CLS.dasforbfed.UTNWS44_SSRS (Disaster, [90 forbs], Extensions, Total) --SR 5024
SELECT
	D.Disaster AS [Disaster],
	SUM(1) AS [90 forbs], --SR 5024
	0 AS Extensions,
	SUM(1) AS Total --SR 5024
FROM
	CLS.dasforbfed.Disasters D
	INNER JOIN CLS.dasforbfed.ProcessQueue PQ
		ON PQ.DisasterId = D.DisasterId
		AND PQ.AddedAt = @Now
GROUP BY
	D.Disaster
;

UPDATE 
	CLS.dasforbfed.Disasters
SET 
	Active = 0 
WHERE 
	EndDate < CAST(@Now AS DATE) 
	AND Active = 1
;
