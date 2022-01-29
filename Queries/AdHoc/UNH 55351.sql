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
				when pd30.DD_VER_ADR < '01/31/2018' then 1
				when pd30.DD_VER_ADR between '01/31/2018' and getdate() then 2
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
	'90009',
'90030',
'90050',
'90051',
'90053',
'90054',
'90055',
'90070',
'90072',
'90073',
'90075',
'90076',
'90078',
'90079',
'90080',
'90081',
'90082',
'90083',
'90086',
'90087',
'90091',
'90093',
'90099',
'90101',
'90102',
'90103',
'90202',
'90209',
'90213',
'90223',
'90224',
'90231',
'90233',
'90239',
'90251',
'90261',
'90264',
'90267',
'90294',
'90295',
'90306',
'90307',
'90308',
'90309',
'90310',
'90311',
'90312',
'90313',
'90397',
'90398',
'90407',
'90408',
'90409',
'90410',
'90411',
'91008',
'90506',
'90507',
'90509',
'90510',
'90608',
'90609',
'90610',
'90612',
'90637',
'90651',
'90652',
'90659',
'90661',
'90662',
'90671',
'90707',
'90711',
'90714',
'90734',
'90748',
'90749',
'90801',
'90809',
'90822',
'90831',
'90832',
'90833',
'90834',
'90835',
'90842',
'90844',
'90845',
'90846',
'90847',
'90848',
'90853',
'90888',
'90899',
'91003',
'91009',
'91012',
'91021',
'91025',
'91031',
'91041',
'91046',
'91066',
'91077',
'91102',
'91109',
'91110',
'91114',
'91115',
'91116',
'91117',
'91118',
'91121',
'91124',
'91125',
'91126',
'91129',
'91131',
'91184',
'91185',
'91188',
'91191',
'91199',
'91209',
'91221',
'91222',
'91224',
'91225',
'91226',
'91305',
'91308',
'91309',
'91313',
'91319',
'91322',
'91327',
'91328',
'91329',
'91333',
'91334',
'91337',
'91341',
'91353',
'91358',
'91359',
'92058',
'91353',
'91358',
'91359',
'91363',
'91371',
'91372',
'91376',
'91380',
'91382',
'91383',
'91385',
'91386',
'91388',
'91392',
'91393',
'91394',
'91395',
'91396',
'91399',
'91404',
'91407',
'91408',
'91409',
'91410',
'91412',
'91413',
'91416',
'91426',
'91470',
'91482',
'91496',
'91497',
'91499',
'91503',
'91507',
'91508',
'91510',
'91521',
'91522',
'91523',
'91526',
'91603',
'92397',
'91608',
'91609',
'91610',
'91611',
'91612',
'91615',
'91616',
'91617',
'91618',
'91714',
'91715',
'91716',
'91734',
'91749',
'91756',
'93225',
'93243',
'93252',
'91769',
'91771',
'91788',
'91795',
'91797',
'91799',
'91802',
'91804',
'91841',
'91896',
'91899',
'91903',
'91933',
'91943',
'91944',
'91946',
'91947',
'91951',
'91976',
'91979',
'91987',
'91990',
'92013',
'92022',
'92023',
'92030',
'92038',
'92039',
'92046',
'92055',
'92072',
'92074',
'92085',
'92088',
'92090',
'92133',
'92137',
'92142',
'92143',
'92147',
'92149',
'92150',
'92152',
'92153',
'92160',
'92161',
'92162',
'92163',
'92164',
'92165',
'92166',
'92167',
'92168',
'92169',
'92170',
'92172',
'92174',
'92176',
'92177',
'92184',
'92187',
'92190',
'92191',
'92192',
'92193',
'92194',
'92195',
'92196',
'92197',
'92199',
'93005',
'93007',
'93014',
'93016',
'93020',
'93024',
'93032',
'93034',
'93042',
'93043',
'93044',
'93061',
'93062',
'93093',
'93094',
'93102',
'93107',
'93118',
'93120',
'93121',
'93130',
'93140',
'93150',
'93199',
'93438',
'93457',
'93464',
'93539',
'93584',
'93586',
'93599'
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