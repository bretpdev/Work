USE UDW
GO

DECLARE @Begin DATE = '12/08/2017'
DECLARE @End DATE = DATEADD(DAY,89,@Begin)
IF OBJECT_ID('tempdb..#POP') IS NOT NULL 
	DROP TABLE #POP
--DECLARE @TEMP TABLE(EmailCampaignID INT, AccountNumber VARCHAR(10), ActualFile VARCHAR(200), EmailData VARCHAR(MAX), ArcNeeded BIT, AddedBy VARCHAR(250), AddedAt DATETIME, PD32Email VARCHAR(100), PH05Email VARCHAR(100))
--INSERT INTO @TEMP (EmailCampaignID, AccountNumber, ActualFile, EmailData, ArcNeeded, AddedBy, AddedAt, PD32Email, PH05Email)
	SELECT DISTINCT
		pd10.df_prs_id as ssn,
			RTRIM(PD10.DM_PRS_1) + ' ' 	+ RTRIM(PD10.DM_PRS_LST) as name,
			PD30.DD_VER_ADR,
			case 
				when pd30.DD_VER_ADR < '12/31/2017' then 1
				when pd30.DD_VER_ADR between '01/01/2018' and '01/31/2018' then 2
				when pd30.DD_VER_ADR between '02/01/2018' and getdate() then 3
				else 0
			end as [file]
		into #pop
	FROM
		LN10_LON LN10
		INNER JOIN PD10_PRS_NME PD10 
			ON PD10.DF_PRS_ID = LN10.BF_SSN
		INNER JOIN 
		(
			SELECT
				ADR.DF_PRS_ID,
				ADR.DX_STR_ADR_1,
				ADR.DX_STR_ADR_2,
				ADR.DM_CT,
				ADR.DC_DOM_ST,
				ADR.DF_ZIP_CDE,
				ADR.DM_FGN_CNY,
				ADR.DM_FGN_ST,
				adr.DD_VER_ADR,
				ROW_NUMBER() OVER (PARTITION BY ADR.DF_PRS_ID ORDER BY ADR.PriorityNumber) [AddressPriority] -- number in order of Address
			FROM
			(
				SELECT
					PD30.DF_PRS_ID,
					PD30.DX_STR_ADR_1,
					PD30.DX_STR_ADR_2,
					PD30.DM_CT,
					PD30.DC_DOM_ST,
					PD30.DF_ZIP_CDE,
					PD30.DM_FGN_CNY,
					PD30.DM_FGN_ST,
					CASE	  
 						WHEN DC_ADR = 'L' THEN 1 -- legal 
 						WHEN DC_ADR = 'B' THEN 2 -- billing
 						WHEN DC_ADR = 'D' THEN 3 -- disbursement
 					END AS PriorityNumber,
					pd30.DD_VER_ADR
				FROM
					PD30_PRS_ADR PD30
				WHERE
					PD30.DI_VLD_ADR = 'Y'
					AND	SUBSTRING(PD30.DF_ZIP_CDE, 1, 5) IN 
					(
	'92709',
'92710',
'92725',
'95408',
'95422',
'95423',
'95424',
'95426',
'95435',
'95443',
'95451',
'95453',
'95457',
'95458',
'95461',
'95464',
'95467',
'95485',
'95493'
)

					
			) ADR
		) PD30 
			ON PD30.DF_PRS_ID = LN10.BF_SSN
			AND PD30.AddressPriority = 1
		LEFT OUTER JOIN 
		(
			SELECT
				DW01.BF_SSN
			FROM
				LN10_LON LN10
				INNER JOIN DW01_DW_CLC_CLU DW01 
					ON DW01.BF_SSN = LN10.BF_SSN
					AND DW01.LN_SEQ = LN10.LN_SEQ 
			WHERE
				DW01.WC_DW_LON_STA IN ('16','17','20','21','07','08','09','10','11','12') --exclude all borrowers that have any of these statuses on any loan (BKY, death, claim)
		) DW01
			ON DW01.BF_SSN = LN10.BF_SSN
		
		LEFT JOIN LN16_LON_DLQ_HST LN16 
			ON LN16.BF_SSN = LN10.BF_SSN 
			AND	LN16.LN_DLQ_MAX > 0 
			AND LN16.LC_STA_LON16 = '1'	-- Active, delinquent
			AND CAST(LN16.LD_DLQ_MAX AS DATE) BETWEEN CAST(GETDATE() - 4 AS DATE) AND CAST(GETDATE() AS DATE)
		
		LEFT JOIN PH05_CNC_EML PH05 
			ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
			AND	PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
			AND PH05.DI_CNC_ELT_OPI = 'Y' --on ecorr
	    LEFT JOIN
		( -- email address 
			SELECT 
				DF_PRS_ID, 
				Email.EM [ALT_EM],
 				ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) [EmailPriority] -- number in order of Email.PriorityNumber 
 			FROM 
 			( 
 				SELECT 
 					PD32.DF_PRS_ID, 
 					PD32.DX_ADR_EML [EM], 
 					CASE	  
 						WHEN DC_ADR_EML = 'H' THEN 1 -- home 
 						WHEN DC_ADR_EML = 'A' THEN 2 -- alternate 
 						WHEN DC_ADR_EML = 'W' THEN 3 -- work 
 					END AS PriorityNumber
 				FROM 
 					PD32_PRS_ADR_EML PD32 
 				WHERE 
 					PD32.DI_VLD_ADR_EML = 'Y' -- valid email address 
 					AND PD32.DC_STA_PD32 = 'A' -- active email address record 

 			) Email 
		) PD32 
			ON PD32.DF_PRS_ID = PD30.DF_PRS_ID
			AND PD32.EmailPriority = 1
	WHERE 
		LN10.LC_STA_LON10 = 'R'	-- Released
		AND LN10.LA_CUR_PRI > 0.00
		AND DW01.BF_SSN IS NULL

SELECT SSN,NAME, DD_VER_ADR FROM #POP WHERE [FILE] = 1
SELECT SSN,NAME, DD_VER_ADR FROM #POP WHERE [FILE] in (1,2)
SELECT SSN,NAME, DD_VER_ADR FROM #POP 




----INSERT INTO [ULS].emailbatch.EmailProcessing(EmailCampaignId, AccountNumber, ActualFile, EmailData, ArcNeeded, AddedBy, AddedAt)
--SELECT
--	EmailCampaignId,
--	AccountNumber,
--	ActualFile,
--	EmailData,
--	ArcNeeded,
--	AddedBy,
--	AddedAt
--FROM
--	@TEMP
--WHERE
--	(
--		PH05Email IS NOT NULL
--		OR PD32Email IS NOT NULL
--	)
	
--SELECT DISTINCT
--	AccountNumber,
--	@Begin AS BeginDate,
--	@End AS EndDate
--FROM
--	@TEMP
--WHERE
--	EmailCampaignId = 50 --delinquent


------Claims stuff
--SELECT DISTINCT
--		PD10.DF_SPE_ACC_ID AS AccountNumber,
--		LN10.LN_SEQ
--	FROM
--		LN10_LON LN10
--		INNER JOIN DW01_DW_CLC_CLU DW01 
--			ON DW01.BF_SSN = LN10.BF_SSN 
--			AND DW01.LN_SEQ = LN10.LN_SEQ
--			AND DW01.WC_DW_LON_STA IN ('07','08','09','10','11','12') --only claims
--		LEFT JOIN 
--		(
--			SELECT
--				LN60.BF_SSN,
--				LN60.LN_SEQ
--			FROM
--				LN60_BR_FOR_APV LN60 
--				INNER JOIN FB10_BR_FOR_REQ FB10 
--					ON FB10.BF_SSN = LN60.BF_SSN 
--					AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
--			WHERE
--				LN60.LC_STA_LON60 = 'A'
--				AND CAST(LN60.LD_FOR_BEG AS DATE) = CAST('2017-08-25' AS DATE)
--				AND FB10.LC_FOR_TYP = '40' 
--				AND FB10.LC_FOR_STA = 'A'
--				AND FB10.LC_STA_FOR10 = 'A'
--				AND LN60.LC_FOR_RSP != '003'	
--		) LN60
--			ON LN60.BF_SSN = LN10.BF_SSN
--			AND LN60.LN_SEQ = LN10.LN_SEQ
--		INNER JOIN PD10_PRS_NME PD10 
--			ON PD10.DF_PRS_ID = LN10.BF_SSN
--		INNER JOIN 
--		(
--			SELECT
--				ADR.DF_PRS_ID,
--				ADR.DX_STR_ADR_1,
--				ADR.DX_STR_ADR_2,
--				ADR.DM_CT,
--				ADR.DC_DOM_ST,
--				ADR.DF_ZIP_CDE,
--				ADR.DM_FGN_CNY,
--				ADR.DM_FGN_ST,
--				ROW_NUMBER() OVER (PARTITION BY ADR.DF_PRS_ID ORDER BY ADR.PriorityNumber) [AddressPriority] -- number in order of Address
--			FROM
--			(
--				SELECT
--					PD30.DF_PRS_ID,
--					PD30.DX_STR_ADR_1,
--					PD30.DX_STR_ADR_2,
--					PD30.DM_CT,
--					PD30.DC_DOM_ST,
--					PD30.DF_ZIP_CDE,
--					PD30.DM_FGN_CNY,
--					PD30.DM_FGN_ST,
--					CASE	  
-- 						WHEN DC_ADR = 'L' THEN 1 -- legal 
-- 						WHEN DC_ADR = 'B' THEN 2 -- billing
-- 						WHEN DC_ADR = 'D' THEN 3 -- disbursement
-- 					END AS PriorityNumber
--				FROM
--					PD30_PRS_ADR PD30
--				WHERE
--					PD30.DI_VLD_ADR = 'Y'
--					AND	SUBSTRING(PD30.DF_ZIP_CDE, 1, 5) IN 
--					(
--					'92709',
--					'92710',
--					'92725',
--					'95408',
--					'95422',
--					'95423',
--					'95424',
--					'95426',
--					'95435',
--					'95443',
--					'95451',
--					'95453',
--					'95457',
--					'95458',
--					'95461',
--					'95464',
--					'95467',
--					'95485',
--					'95493'
--					) 
--			) ADR
--		) PD30 
--			ON PD30.DF_PRS_ID = LN10.BF_SSN
--			AND PD30.AddressPriority = 1
--	WHERE 
--		LN10.LC_STA_LON10 = 'R'	-- Released
--		AND LN10.LA_CUR_PRI > 0.00
--	ORDER BY 
--		AccountNumber