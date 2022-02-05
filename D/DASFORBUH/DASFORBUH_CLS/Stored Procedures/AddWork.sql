CREATE PROCEDURE [dasforbuh].[AddWork]
AS

DROP TABLE IF EXISTS #UHEAA_FORB_UH;

DECLARE @Now DATETIME = GETDATE()

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
	Base.DelinquencyOverride,
	Base.Active,
	Base.LD_DLQ_OCC,
	Base.LN_DLQ_MAX,
	Base.LA_RPS_ISL, 
	Base.EARLIEST_DISASTER, 
	Base.DISASTER_INSTANCES, 
	Base.BeginDateScriptData,
	Base.Comment,
	CASE WHEN Claim.BF_SSN IS NOT NULL THEN 1 ELSE 0 END AS HasClaim
INTO
	#UHEAA_FORB_UH
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
			PD30_VLA.DelinquencyOverride,
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
			CASE WHEN RS.LA_RPS_ISL IS NOT NULL AND LN16_mins.BF_SSN IS NOT NULL THEN 'Please review account: borrower has a 0.00 payment and delinquent. DisasterId: ' + CAST(DisasterId AS VARCHAR(10)) 
			WHEN LN10_TILP.HasTilp = 1 AND LN16_mins.BF_SSN IS NOT NULL THEN 'Please review account: borrower has a TILP loan and delinquent. DisasterId: ' + CAST(DisasterId AS VARCHAR(10)) 	
				ELSE NULL 
			END AS Comment
		FROM
			UDW..LN10_LON LN10
			INNER JOIN UDW..PD10_PRS_NME PD10
				ON PD10.DF_PRS_ID = LN10.BF_SSN
			INNER JOIN UDW..DW01_DW_CLC_CLU DW01
				ON LN10.BF_SSN = DW01.BF_SSN
				AND LN10.LN_SEQ = DW01.LN_SEQ
			INNER JOIN
			(
				SELECT
					LN10.BF_SSN,
					MAX(CASE WHEN LN10.IC_LON_PGM = 'TILP' THEN 1 ELSE 0 END) AS HasTilp
				FROM	
					UDW..LN10_LON LN10
				WHERE
					LN10.LC_STA_LON10 = 'R'
					AND LN10.LA_CUR_PRI > 0.00
				GROUP BY
					BF_SSN
			) LN10_TILP
				ON LN10.BF_SSN = LN10_TILP.BF_SSN
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
					ADR.DelinquencyOverride,
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
						DIS.DelinquencyOverride,
						DIS.Active,
						CASE WHEN DC_ADR = 'L' THEN 1 -- legal 
 							 WHEN DC_ADR = 'B' THEN 2 -- billing
 							 WHEN DC_ADR = 'D' THEN 3 -- disbursement
 						END AS PriorityNumber
					FROM
						UDW..PD30_PRS_ADR PD30
						INNER JOIN ULS.dasforbuh.Zips ZIP
							ON ZIP.ZipCode = SUBSTRING(PD30.DF_ZIP_CDE, 1, 5)
						INNER JOIN ULS.dasforbuh.Disasters DIS
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
					UDW..LN16_LON_DLQ_HST LN16
					INNER JOIN UDW..LN10_LON LN10
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
			LEFT JOIN UDW.calc.RepaymentSchedules RS --flagged for exclusion: current payment amount is zero dollars
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
					UDW..FB10_BR_FOR_REQ FB10
					INNER JOIN UDW..LN60_BR_FOR_APV LN60
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
			LEFT JOIN UDW..WQ20_TSK_QUE WQ20 --flagged for exclusion: have open place holder queue task
				ON LN10.BF_SSN = WQ20.BF_SSN
				AND WQ20.WF_QUE = '40'
				AND WQ20.WF_SUB_QUE = '01'
				AND WQ20.WC_STA_WQUE20 IN ('A','H','P','U','W')
		WHERE
			LN10.LC_STA_LON10 = 'R'
			AND DW01.WC_DW_LON_STA NOT IN ('16','17','18','19','20','21')
			AND DW01.WX_OVR_DW_LON_STA NOT IN ('CNSLD-STOP PURSUIT')
			AND LN10.LA_CUR_PRI > 0.00
			AND WQ20.BF_SSN IS NULL
			AND ExistingForb.BF_SSN IS NULL --excludes forbearances that fall inside disaster begin/end dates
			AND 
			(
				(
					LN16_mins.LD_DLQ_OCC < CAST(PD30_VLA.EndDate AS DATE) --delinquency occurs before the disaster 90 day end date
					AND LN16_mins.LN_DLQ_MAX >= 5 --delinquent borrowers
				)
				OR 
				(
					LN16_mins.BF_SSN IS NULL
					OR 
					(
						LN16_mins.LN_DLQ_MAX < 5
						AND LN16_mins.LD_DLQ_OCC < CAST(PD30_VLA.EndDate AS DATE) --delinquency occurs before the disaster 90 day end date
					)
				)
			)
	) Base
	LEFT JOIN 
	(
		SELECT DISTINCT
			Claim.BF_SSN
		FROM	
			UDW..LN10_LON LN10
			INNER JOIN UDW..DW01_DW_CLC_CLU Claim 
				ON Claim.BF_SSN = LN10.BF_SSN 
				AND Claim.LN_SEQ = LN10.LN_SEQ
				AND Claim.WC_DW_LON_STA IN ('07','08','09','10','11','12') --only claims
		WHERE
			LN10.LA_CUR_PRI > 0.00
			AND LN10.LC_STA_LON10 = 'R'
	) Claim
		ON Claim.BF_SSN = Base.BF_SSN 
WHERE
	Base.EARLIEST_DISASTER = 1
;

/****************************************************************
			MANUAL REVIEW POP
****************************************************************/
INSERT INTO ULS..ArcAddProcessing (ArcTypeId, AccountNumber, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, ProcessingAttempts, CreatedAt, CreatedBy)
SELECT DISTINCT
	2 AS ArcTypeId,
	UF.DF_SPE_ACC_ID AS AccountNumber,
	'DASFB' AS ARC,
	'DASFORBUH' AS ScriptId,
	@Now AS ProcessOn,
	UF.Comment,
	0 AS IsReference,
	0 AS IsEndorser,
	0 AS ProcessingAttempts,
	@Now AS CreatedAt,
	SUSER_SNAME() AS CreatedBy
FROM
	#UHEAA_FORB_UH UF
	LEFT JOIN ULS..ArcAddProcessing ExistingAAP
		ON ExistingAAP.AccountNumber = UF.DF_SPE_ACC_ID
		AND ExistingAAP.ARC = 'DASFB'
		AND ExistingAAP.ScriptId = 'DASFORBUH'
		--Comment having disasterId appended will not affect script as this is just comments for manual review accounts, and will make a row unique without needing to parse on date.
		AND COALESCE(ExistingAAP.Comment,'') = UF.Comment
		AND CAST(ExistingAAP.CreatedAt AS DATE) BETWEEN UF.BeginDate AND UF.EndDate--Riley asked to use the disaster begin and end date to exclude the email
WHERE
	UF.Comment IS NOT NULL
	AND UF.HasClaim = 0
	AND ExistingAAP.AccountNumber IS NULL --No matching existing record
;

/****************************************************************
			MANUAL REVIEW CLAIMS POP
****************************************************************/
INSERT INTO ULS..ArcAddProcessing (ArcTypeId, AccountNumber, ARC, ScriptId, ProcessOn, Comment, IsReference, IsEndorser, ProcessingAttempts, CreatedAt, CreatedBy)
SELECT DISTINCT
	2 AS ArcTypeId,
	UF.DF_SPE_ACC_ID AS AccountNumber,
	'DASFB' AS ARC,
	'DASFORBUH' AS ScriptId,
	@Now AS ProcessOn,
	'Manual Review: Borrower has a loan in a claim status. DisasterId: ' + CAST(UF.DisasterId AS VARCHAR(10)) AS Comment,
	0 AS IsReference,
	0 AS IsEndorser,
	0 AS ProcessingAttempts,
	@Now AS CreatedAt,
	SUSER_SNAME() AS CreatedBy
FROM
	#UHEAA_FORB_UH UF
	INNER JOIN UDW..DW01_DW_CLC_CLU DW01
		ON DW01.BF_SSN = UF.BF_SSN
		AND DW01.LN_SEQ = UF.LN_SEQ
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = DW01.BF_SSN
		AND LN10.LN_SEQ = DW01.LN_SEQ
	LEFT JOIN ULS..ArcAddProcessing ExistingAAP
		ON ExistingAAP.AccountNumber = UF.DF_SPE_ACC_ID
		AND ExistingAAP.ARC = 'DASFB'
		AND ExistingAAP.ScriptId = 'DASFORBUH'
		--Comment having disasterId appended will not affect script as this is just comments for manual review accounts, and will make a row unique without needing to parse on date.
		AND COALESCE(ExistingAAP.Comment,'') = UF.Comment
		AND CAST(ExistingAAP.CreatedAt AS DATE) BETWEEN UF.BeginDate AND UF.EndDate--Riley asked to use the disaster begin and end date to exclude the email
WHERE
	UF.HasClaim = 1
	AND ExistingAAP.AccountNumber IS NULL --No matching existing record
	AND DW01.WC_DW_LON_STA != '22' -- borrower has at least 1 loan that isnt paid
	AND 
	(
		(
			DW01.WC_DW_LON_STA = '12' 
			AND CAST(LN10.LD_PIF_RPT AS DATE) BETWEEN CAST(UF.BeginDate AS DATE) AND CAST(UF.EndDate AS DATE)
		)
		OR DW01.WC_DW_LON_STA != '12'
	)
;
/****************************************************************
			90 DAY FORBEARANCE POP LOGIC
				SCRIPT DATA INSERT
SHOULD MATCH SSRS REPORT AND DELINQUENT EMAILPROCESSING GROUP
****************************************************************/
INSERT INTO ULS.dasforbuh.ProcessQueue (AccountNumber, BeginDate, EndDate, AddedAt, AddedBy, DisasterId, LD_DLQ_OCC, ForbearanceTypeId)
SELECT DISTINCT
	UF.DF_SPE_ACC_ID,
	UF.BeginDateScriptData,
	UF.EndDate,
	@Now AS AddedAt,
	SUSER_SNAME() AS AddedBy,
	UF.DisasterId,
	UF.LD_DLQ_OCC,
	1 --90 day forb type
FROM
	#UHEAA_FORB_UH UF
	INNER JOIN
	(
		SELECT
			UF.DF_SPE_ACC_ID,
			MAX(CAST(UF.HasClaim AS INT)) AS AccountHasClaim
		FROM	
			#UHEAA_FORB_UH UF
		GROUP BY 
			UF.DF_SPE_ACC_ID
	) AccountClaim
		ON UF.DF_SPE_ACC_ID = AccountClaim.DF_SPE_ACC_ID
	INNER JOIN UDW.calc.RepaymentSchedules RS
		ON UF.BF_SSN = RS.BF_SSN
		AND UF.LN_SEQ = RS.LN_SEQ
		AND RS.LA_RPS_ISL > 0.00
		AND RS.CurrentGradation = 1
	LEFT JOIN ULS.dasforbuh.ProcessQueue ExistingPQ
		ON ExistingPQ.AccountNumber = UF.DF_SPE_ACC_ID
		AND ExistingPQ.BeginDate = UF.BeginDateScriptData
		AND ExistingPQ.EndDate = UF.EndDate
		AND ExistingPQ.DisasterId = UF.DisasterId
		AND ExistingPQ.ForbearanceTypeId = 1
		AND ExistingPQ.DeletedOn IS NULL
WHERE
	UF.Comment IS NULL
	AND AccountClaim.AccountHasClaim = 0
	AND UF.LD_DLQ_OCC < CAST(UF.EndDate AS DATE) --delinquency occurs before the disaster 90 day end date
	AND UF.LN_DLQ_MAX >= 5
	AND ExistingPQ.AccountNumber IS NULL --No matching existing record
;

/****************************************************************
				DELINQUENT EMAIL FILE
				90 DAY POPULATION
****************************************************************/
INSERT INTO ULS.emailbatch.EmailProcessing (EmailCampaignId, AccountNumber, ActualFile, EmailData, ArcNeeded, AddedBy, AddedAt)
SELECT DISTINCT
	50 AS EmailCampaignId, --delinquent
	UF.DF_SPE_ACC_ID AS AccountNumber,
	'' AS ActualFile,
	PD10.DF_SPE_ACC_ID + ',' + LTRIM(RTRIM(PD10.DM_PRS_1)) + ' ' + LTRIM(RTRIM(PD10.DM_PRS_LST)) + ',' + COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) AS EmailData,
	1 AS ArcNeeded,
	SUSER_SNAME() AS AddedBy,
	@Now AS AddedAt
FROM
	#UHEAA_FORB_UH UF
	INNER JOIN
	(
		SELECT
			UF.DF_SPE_ACC_ID,
			MAX(CAST(UF.HasClaim AS INT)) AS AccountHasClaim
		FROM	
			#UHEAA_FORB_UH UF
		GROUP BY 
			UF.DF_SPE_ACC_ID
	) AccountClaim
		ON UF.DF_SPE_ACC_ID = AccountClaim.DF_SPE_ACC_ID
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON UF.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN UDW.calc.RepaymentSchedules RS
		ON UF.BF_SSN = RS.BF_SSN
		AND UF.LN_SEQ = RS.LN_SEQ
		AND RS.LA_RPS_ISL > 0.00
		AND RS.CurrentGradation = 1
	LEFT JOIN UDW..PH05_CNC_EML PH05 
		ON PH05.DF_SPE_ID = UF.DF_SPE_ACC_ID
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
 				UDW..PD32_PRS_ADR_EML PD32 
 			WHERE 
 				PD32.DI_VLD_ADR_EML = 'Y' -- valid email address 
 				AND PD32.DC_STA_PD32 = 'A' -- active email address record 
 		) Email 
	) PD32 
		ON PD32.DF_PRS_ID = UF.BF_SSN
		AND PD32.EmailPriority = 1
	LEFT JOIN ULS.emailbatch.EmailProcessing ExistingEP
		ON ExistingEP.EmailCampaignId = 50
		AND ExistingEP.AccountNumber = UF.DF_SPE_ACC_ID
		AND ExistingEP.EmailData = (PD10.DF_SPE_ACC_ID + ',' + LTRIM(RTRIM(PD10.DM_PRS_1)) + ' ' + LTRIM(RTRIM(PD10.DM_PRS_LST)) + ',' + COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM))
		AND CAST(ExistingEP.AddedAt AS DATE) BETWEEN UF.BeginDate AND UF.EndDate--Riley asked to use the disaster begin and end date to exclude the email
WHERE
	UF.Comment IS NULL
	AND AccountClaim.AccountHasClaim = 0
	AND UF.LD_DLQ_OCC <= CAST(UF.EndDate AS DATE)
	AND COALESCE(PH05.DX_CNC_EML_ADR,PD32.ALT_EM) IS NOT NULL
	AND ExistingEP.AccountNumber IS NULL --No existing record
	AND COALESCE(UF.LN_DLQ_MAX,0) >= 5 --delinquent borrowers
;

/****************************************************************
				CURRENT EMAIL FILE
		BASE POPULATION - NON-DELINQUENT BORROWERS
****************************************************************/
INSERT INTO ULS.emailbatch.EmailProcessing (EmailCampaignId, AccountNumber, ActualFile, EmailData, ArcNeeded, AddedBy, AddedAt)
SELECT DISTINCT
	49 AS EmailCampaignId, --not delinquent
	UF.DF_SPE_ACC_ID AS AccountNumber,
	'' AS ActualFile,
	PD10.DF_SPE_ACC_ID + ',' + LTRIM(RTRIM(PD10.DM_PRS_1)) + ' ' + LTRIM(RTRIM(PD10.DM_PRS_LST)) + ',' + COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) AS EmailData,
	1 AS ArcNeeded,
	SUSER_SNAME() AS AddedBy,
	@Now AS AddedAt
FROM
	#UHEAA_FORB_UH UF
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON UF.BF_SSN = PD10.DF_PRS_ID
	INNER JOIN UDW..DW01_DW_CLC_CLU DW01
		ON UF.BF_SSN = DW01.BF_SSN
		AND UF.LN_SEQ = DW01.LN_SEQ
		AND DW01.WC_DW_LON_STA = '03'
	LEFT JOIN UDW..PH05_CNC_EML PH05 
		ON PH05.DF_SPE_ID = UF.DF_SPE_ACC_ID
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
 				UDW..PD32_PRS_ADR_EML PD32 
 			WHERE 
 				PD32.DI_VLD_ADR_EML = 'Y' -- valid email address 
 				AND PD32.DC_STA_PD32 = 'A' -- active email address record 
 		) Email 
	) PD32 
		ON PD32.DF_PRS_ID = UF.BF_SSN
		AND PD32.EmailPriority = 1
	LEFT JOIN 
	(--flagged for exclusion: exclude borrowers who received email in last 30 days
		SELECT
			BF_SSN,
			MAX(LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
		FROM
			UDW..AY10_BR_LON_ATY
		WHERE
			PF_REQ_ACT = 'NDSNU'
			AND LD_ATY_REQ_RCV BETWEEN CAST(DATEADD(DAY,-30,@Now) AS DATE) AND CAST(@Now AS DATE)
		GROUP BY
			BF_SSN
	)AY10M
		ON UF.BF_SSN = AY10M.BF_SSN
	LEFT JOIN ULS.emailbatch.EmailProcessing ExistingEP
		ON ExistingEP.EmailCampaignId = 49
		AND ExistingEP.AccountNumber = UF.DF_SPE_ACC_ID
		AND ExistingEP.EmailData = (PD10.DF_SPE_ACC_ID + ',' + LTRIM(RTRIM(PD10.DM_PRS_1)) + ' ' + LTRIM(RTRIM(PD10.DM_PRS_LST)) + ',' + COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM))
		AND CAST(ExistingEP.AddedAt AS DATE) BETWEEN CAST(DATEADD(DAY,-30,@Now) AS DATE) AND CAST(@Now AS DATE)
WHERE
	AY10M.BF_SSN IS NULL --excludes
	AND	COALESCE(PH05.DX_CNC_EML_ADR,PD32.ALT_EM) IS NOT NULL
	AND COALESCE(UF.LN_DLQ_MAX,0) < 5 --non-delinquent borrowers
	AND ExistingEP.AccountNumber IS NULl --No existing record in the last 30 days
;

/****************************************************************
	DAILY SSRS REPORT BASED ON SCRIPT DATA INSERT
****************************************************************/
TRUNCATE TABLE ULS.dasforbuh.UTNWS44_SSRS;

INSERT INTO ULS.dasforbuh.UTNWS44_SSRS(Disaster, [90 forbs], Extensions, Total)
SELECT
	D.Disaster,
	SUM(1) AS [90 forbs],
	0 AS Extensions,
	SUM(1) AS Total
FROM
(
	SELECT DISTINCT
		UF.DF_SPE_ACC_ID,
		UF.BeginDateScriptData,
		UF.EndDate,
		@Now AS AddedAt,
		SUSER_SNAME() AS AddedBy,
		UF.DisasterId,
		UF.LD_DLQ_OCC,
		1 AS ForbType --90 day forb type
	FROM
		#UHEAA_FORB_UH UF
		INNER JOIN
		(
			SELECT
				UF.DF_SPE_ACC_ID,
				MAX(CAST(UF.HasClaim AS INT)) AS AccountHasClaim
			FROM	
				#UHEAA_FORB_UH UF
			GROUP BY 
				UF.DF_SPE_ACC_ID
		) AccountClaim
			ON UF.DF_SPE_ACC_ID = AccountClaim.DF_SPE_ACC_ID
		INNER JOIN UDW.calc.RepaymentSchedules RS
			ON UF.BF_SSN = RS.BF_SSN
			AND UF.LN_SEQ = RS.LN_SEQ
			AND RS.LA_RPS_ISL > 0.00
			AND RS.CurrentGradation = 1
	WHERE
		UF.Comment IS NULL
		AND AccountClaim.AccountHasClaim = 0
		AND UF.LD_DLQ_OCC < CAST(UF.EndDate AS DATE) --delinquency occurs before the disaster 90 day end date
		AND UF.LN_DLQ_MAX >= 5
) POP
INNER JOIN ULS.dasforbuh.Disasters D
	ON POP.DisasterId = D.DisasterId
GROUP BY
	D.Disaster
;

UPDATE 
	ULS.dasforbuh.Disasters 
SET 
	Active = 0 
WHERE 
	EndDate < CAST(@Now AS DATE) 
	AND Active = 1
;