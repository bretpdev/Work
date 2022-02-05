/********************************************
	BORROWERS NEW COD DISBURESMENT - FED
*********************************************/

SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

IF OBJECT_ID('tempdb..#NEWCODFED') IS NOT NULL 
	DROP TABLE #NEWCODFED
GO

DECLARE @SackerID VARCHAR(9) = 'NEWCODFED';
DECLARE @LetterID VARCHAR(15) = 'NWCODEMFED.html';

DECLARE @TODAY DATE = GETDATE();
DECLARE @PREVIOUSDAYCALC DATE = 
(--if run on Monday then do 3 days ago, else do yesterday
	SELECT
		CASE
			WHEN DATENAME(WEEKDAY,@TODAY) = 'Monday'
			THEN DATEADD(DAY,-3,@TODAY)
			ELSE DATEADD(DAY,-1,@TODAY)
		END
);

DECLARE @YESTERDAY DATE = DATEADD(DAY,-1,@TODAY);

select @TODAY,@PREVIOUSDAYCALC,@YESTERDAY --TEST

DECLARE @EmailCampaignId INT = (SELECT EmailCampaignId FROM CLS.emailbtcf.EmailCampaigns WHERE LetterId = @LetterID);
select @EmailCampaignId --TEST

--base population:
SELECT DISTINCT
	BASEPOP.Recipient,
	BASEPOP.AccountNumber,
	BASEPOP.FirstName,
	BASEPOP.FirstName + ',"' + BASEPOP.School + '",$' + CAST(BASEPOP.AmountOfDisbursement AS VARCHAR(10)) AS LineData
INTO
	#NEWCODFED
FROM
	(
		SELECT
			COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM,'') AS Recipient,
			PD10.DF_SPE_ACC_ID AS AccountNumber,
			CASE WHEN PATINDEX('% [A-Za-z]%',LTRIM(RTRIM(DM_PRS_1))) + 1 != 1
				THEN
					LTRIM(RTRIM(SUBSTRING(DM_PRS_1,1,1))) 
					+ LOWER(SUBSTRING(DM_PRS_1,2,PATINDEX('% [A-Za-z]%',SUBSTRING(LTRIM(RTRIM(DM_PRS_1)),2,LEN(LTRIM(RTRIM(DM_PRS_1)))-1)))) --First first name lower cased
					+ LTRIM(RTRIM(SUBSTRING(DM_PRS_1,PATINDEX('% [A-Za-z]%',SUBSTRING(LTRIM(RTRIM(DM_PRS_1)),2,LEN(LTRIM(RTRIM(DM_PRS_1)))-1))+2,1))) --Start of 2nd Name
					+ LOWER(SUBSTRING(DM_PRS_1,PATINDEX('% [A-Za-z]%',SUBSTRING(LTRIM(RTRIM(DM_PRS_1)),2,LEN(LTRIM(RTRIM(DM_PRS_1)))-1))+3,LEN(LTRIM(RTRIM(DM_PRS_1)))-PATINDEX('% [A-Za-z]%',SUBSTRING(LTRIM(RTRIM(DM_PRS_1)),2,LEN(LTRIM(RTRIM(DM_PRS_1)))-1))+2)) 
				ELSE
					LTRIM(RTRIM(SUBSTRING(DM_PRS_1,1,1) + LOWER(SUBSTRING(DM_PRS_1,2,13))))
			END AS FirstName, 
			LTRIM(RTRIM(SC10.IM_SCL_FUL)) AS School,
			SUM(LN90.DSB) OVER(PARTITION BY LN90.BF_SSN, LN10.LF_DOE_SCL_ORG) AS AmountOfDisbursement
		FROM
			CDW..PD10_PRS_NME PD10
			INNER JOIN CDW..LN10_LON LN10
				ON LN10.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN
			(
				SELECT
					LN90.BF_SSN,
					LN90.LN_SEQ,
					SUM(COALESCE(LN90.LA_FAT_CUR_PRI,0.00)) AS DSB
				FROM
					CDW..LN90_FIN_ATY LN90
					INNER JOIN CDW..LN10_LON LN10
						ON LN10.BF_SSN = LN90.BF_SSN
						AND LN10.LN_SEQ = LN90.LN_SEQ
						AND LN10.LD_LON_ACL_ADD = LN90.LD_STA_LON90
				WHERE
					LN90.LC_STA_LON90 = 'A' --active
					AND COALESCE(LN90.LC_FAT_REV_REA,'') = '' --no reverse
					AND LN90.PC_FAT_TYP = '01' --disbursement
				GROUP BY
					LN90.BF_SSN,
					LN90.LN_SEQ
			) LN90
				ON LN10.BF_SSN = LN90.BF_SSN
				AND LN10.LN_SEQ = LN90.LN_SEQ
			INNER JOIN CDW..SC10_SCH_DMO SC10
				ON LN10.LF_DOE_SCL_ORG = SC10.IF_DOE_SCL
			LEFT JOIN CDW..PH05_CNC_EML PH05 
				ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
				AND	PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
				AND PH05.DI_CNC_ELT_OPI = 'Y' --Opted in to Ecorr
			LEFT JOIN
			( -- email address
				SELECT 
					DF_PRS_ID, 
					Email.EM AS [ALT_EM],
 					ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) AS [EmailPriority] -- number in order of Email.PriorityNumber 
 				FROM 
 				( 
 					SELECT 
 						PD32.DF_PRS_ID, 
 						PD32.DX_ADR_EML AS [EM], 
 						CASE	  
 							WHEN DC_ADR_EML = 'H' THEN 1 -- home 
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
				ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
				AND PD32.EmailPriority = 1 --sends only to highest priority email
				
		WHERE
			LN10.LD_LON_ACL_ADD BETWEEN @PREVIOUSDAYCALC AND @YESTERDAY
			AND COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) IS NOT NULL --Has a valid email
			--AND PD10.DF_SPE_ACC_ID = '' --For testing
	) BASEPOP
;

--1st insert: put new data into table to assign ID
INSERT INTO CLS.emailbtcf.LineData
(
	LineData
)
SELECT DISTINCT
	NCF.LineData
FROM
	#NEWCODFED NCF
	LEFT JOIN CLS.emailbtcf.LineData EXISTING_DATA
		ON EXISTING_DATA.LineData = NCF.LineData
WHERE
	EXISTING_DATA.LineData IS NULL --wasn't already added
;
/*LineData table will re-use existing lineData rows in the event that the borrower has the same lineData 
text on a subsequent run (table doesnt have a date added) or duplication would occur*/

--2nd insert: put new daily data into table w/linedata ID reference
INSERT INTO CLS.emailbtcf.CampaignData
(
	EmailCampaignId,
	Recipient,
	AccountNumber,
	FirstName,
	LastName,
	AddedAt,
	AddedBy,
	LineDataId
)
SELECT DISTINCT
	@EmailCampaignId AS EmailCampaignId,
	NCF.Recipient,
	NCF.AccountNumber,
	NCF.FirstName,
	'' AS LastName,
	GETDATE() AS AddedAt,
	@SackerID AS AddedBy,
	LD.LineDataId
FROM
	#NEWCODFED NCF
	LEFT JOIN CLS.emailbtcf.LineData LD
		ON NCF.LineData = LD.LineData
	LEFT JOIN CLS.emailbtcf.CampaignData EXISTING_DATA --flag to exclude existing data added today
		ON EXISTING_DATA.EmailCampaignId = @EmailCampaignId
		AND EXISTING_DATA.Recipient = NCF.Recipient
		AND EXISTING_DATA.AccountNumber = NCF.AccountNumber
		AND EXISTING_DATA.FirstName = NCF.FirstName
		AND CAST(EXISTING_DATA.AddedAt AS DATE) = @TODAY
		AND EXISTING_DATA.LineDataId = LD.LineDataId
WHERE
	EXISTING_DATA.AccountNumber IS NULL --wasn't already added today
;

