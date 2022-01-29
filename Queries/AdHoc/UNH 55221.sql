USE UDW
GO

DECLARE @Begin DATE = '2018-01-02'
DECLARE @End DATE = DATEADD(DAY,89,@Begin)
DECLARE @ZIPS TABLE (ZIPS VARCHAR(5));
INSERT INTO @ZIPS (ZIPS) VALUES
 ('90895')
,('91001')
,('91006')
,('91007')
,('91011')
,('91010')
,('91016')
,('91020')
,('91017')
,('93510')
,('91023')
,('91024')
,('91030')
,('91040')
,('91043')
,('91042')
,('91101')
,('91103')
,('91105')
,('93534')
,('91104')
,('93532')
,('91107')
,('93536')
,('91106')
,('93535')
,('91108')
,('93543')
,('93544')
,('91123')
,('93551')
,('93550')
,('93553')
,('93552')
,('91182')
,('93563')
,('93590')
,('91189')
,('91202')
,('91201')
,('93591')
,('91204')
,('91203')
,('91206')
,('91205')
,('91208')
,('91207')
,('91210')
,('91214')
,('91302')
,('91301')
,('91304')
,('91303')
,('91306')
,('91307')
,('91310')
,('91311')
,('91316')
,('91321')
,('91325')
,('91324')
,('91326')
,('91331')
,('91330')
,('91335')
,('91340')
,('91343')
,('91342')
,('91345')
,('91344')
,('91350')
,('91346')
,('91352')
,('91351')
,('91354')
,('91356')
,('91355')
,('91357')
,('91361')
,('91364')
,('91367')
,('91365')
,('91381')
,('91384')
,('91387')
,('91390')
,('91402')
,('91401')
,('91403')
,('91406')
,('91405')
,('91411')
,('91423')
,('91436')
,('91495')
,('91501')
,('91502')
,('91505')
,('91504')
,('91506')
,('91602')
,('91601')
,('91604')
,('91606')
,('91605')
,('91607')
,('91614')
,('91706')
,('91702')
,('91711')
,('91722')
,('91724')
,('91723')
,('91732')
,('91731')
,('91733')
,('91735')
,('91740')
,('91741')
,('91745')
,('91744')
,('91747')
,('91746')
,('91748')
,('91750')
,('91755')
,('91754')
,('91759')
,('91765')
,('91767')
,('91766')
,('91768')
,('91770')
,('91773')
,('91772')
,('91776')
,('91775')
,('91780')
,('91778')
,('91790')
,('91789')
,('91792')
,('91791')
,('91793')
,('91801')
,('91803')
,('91008')
,('92397')
,('90002')
,('90001')
,('90004')
,('90003')
,('90006')
,('90005')
,('90008')
,('90007')
,('90010')
,('90012')
,('90011')
,('90014')
,('90013')
,('90016')
,('90015')
,('90018')
,('90017')
,('90020')
,('90019')
,('90022')
,('90021')
,('90024')
,('90023')
,('90026')
,('90025')
,('90028')
,('90027')
,('90029')
,('90032')
,('90031')
,('90034')
,('90033')
,('90036')
,('90035')
,('90038')
,('90037')
,('90040')
,('90039')
,('90042')
,('90041')
,('90044')
,('90043')
,('90046')
,('90045')
,('90048')
,('90047')
,('90049')
,('90052')
,('90056')
,('90058')
,('90057')
,('90060')
,('90059')
,('90062')
,('90061')
,('90064')
,('90063')
,('90066')
,('90065')
,('90068')
,('90067')
,('90069')
,('90071')
,('90074')
,('90077')
,('90084')
,('90089')
,('90088')
,('90095')
,('90094')
,('90096')
,('90201')
,('90189')
,('90211')
,('90210')
,('90212')
,('90221')
,('90220')
,('90222')
,('90230')
,('90232')
,('90241')
,('90240')
,('90245')
,('90242')
,('90248')
,('90247')
,('90250')
,('90249')
,('90254')
,('90260')
,('90255')
,('90262')
,('90263')
,('90266')
,('90265')
,('90270')
,('90274')
,('90272')
,('90277')
,('90275')
,('90280')
,('90278')
,('90291')
,('90290')
,('90293')
,('90292')
,('90301')
,('90296')
,('90303')
,('90302')
,('90305')
,('90304')
,('90402')
,('90401')
,('90404')
,('90403')
,('90406')
,('93243')
,('90405')
,('90501')
,('90503')
,('90502')
,('90505')
,('90504')
,('90508')
,('90601')
,('90603')
,('90602')
,('90605')
,('90604')
,('90607')
,('90606')
,('90639')
,('90638')
,('90650')
,('90640')
,('90660')
,('90670')
,('90702')
,('90701')
,('90704')
,('90703')
,('90706')
,('90710')
,('90713')
,('90712')
,('90715')
,('90717')
,('90716')
,('90731')
,('90723')
,('90733')
,('90732')
,('90745')
,('90744')
,('90747')
,('90746')
,('90755')
,('90803')
,('90802')
,('90805')
,('90804')
,('90807')
,('90806')
,('90808')
,('90813')
,('90810')
,('90815')
,('90814')
,('90840')
,('91902')
,('91901')
,('91905')
,('91908')
,('91906')
,('91910')
,('91909')
,('91912')
,('91911')
,('91914')
,('91913')
,('91916')
,('91915')
,('91921')
,('91917')
,('91932')
,('91931')
,('91934')
,('91941')
,('91935')
,('91942')
,('91945')
,('91950')
,('91948')
,('91962')
,('91963')
,('91978')
,('91977')
,('91980')
,('92004')
,('92003')
,('92008')
,('92007')
,('92010')
,('92009')
,('92011')
,('92018')
,('92014')
,('92020')
,('92019')
,('92021')
,('92024')
,('92026')
,('92025')
,('92028')
,('92027')
,('92029')
,('92036')
,('92033')
,('92037')
,('92040')
,('92049')
,('92052')
,('92051')
,('92054')
,('92057')
,('92056')
,('92059')
,('92058')
,('92061')
,('92060')
,('92065')
,('92064')
,('92067')
,('92066')
,('92069')
,('92068')
,('92071')
,('92070')
,('92078')
,('92075')
,('92081')
,('92079')
,('92083')
,('92082')
,('92084')
,('92086')
,('92091')
,('92093')
,('92092')
,('92101')
,('92096')
,('92103')
,('92102')
,('92105')
,('92104')
,('92107')
,('92106')
,('92109')
,('92108')
,('92111')
,('92110')
,('92113')
,('92112')
,('92115')
,('92114')
,('92117')
,('92116')
,('92119')
,('92118')
,('92121')
,('92120')
,('92123')
,('92122')
,('92126')
,('92124')
,('92128')
,('92127')
,('92130')
,('92129')
,('92132')
,('92131')
,('92134')
,('92136')
,('92135')
,('92138')
,('92140')
,('92139')
,('92145')
,('92155')
,('92154')
,('92159')
,('92158')
,('92171')
,('92173')
,('92175')
,('92179')
,('92178')
,('92182')
,('92186')
,('92198')
,('93455')
,('93454')
,('93456')
,('93460')
,('93458')
,('93463')
,('93067')
,('93254')
,('93101')
,('93105')
,('93103')
,('93427')
,('93106')
,('93109')
,('93429')
,('93108')
,('93111')
,('93110')
,('93117')
,('93434')
,('93116')
,('93436')
,('93437')
,('93441')
,('93440')
,('93190')
,('93160')
,('91362')
,('93010')
,('93009')
,('93012')
,('93011')
,('93013')
,('93015')
,('91377')
,('93021')
,('93023')
,('93022')
,('93030')
,('93225')
,('93031')
,('93033')
,('93036')
,('93035')
,('93041')
,('93040')
,('93060')
,('93064')
,('93063')
,('91320')
,('93066')
,('93065')
,('93252')
,('93099')
,('93001')
,('93003')
,('93002')
,('93004')
,('91360')
,('93006')

--SELECT ZIPS FROM @ZIPS

DECLARE @TEMP TABLE(EmailCampaignID INT, AccountNumber VARCHAR(10), ActualFile VARCHAR(200), EmailData VARCHAR(MAX), ArcNeeded BIT, AddedBy VARCHAR(250), AddedAt DATETIME, PD32Email VARCHAR(100), PH05Email VARCHAR(100))
INSERT INTO @TEMP (EmailCampaignID, AccountNumber, ActualFile, EmailData, ArcNeeded, AddedBy, AddedAt, PD32Email, PH05Email)
	SELECT DISTINCT
		CASE WHEN LN16.BF_SSN IS NOT NULL THEN 50 /*Delinquent*/ ELSE 49 /*Not Delinquent*/ END AS EmailCampaignId,
		PD10.DF_SPE_ACC_ID AS AccountNumber,
		'' AS ActualFile,
		PD10.DF_SPE_ACC_ID + ',' 
			+ RTRIM(PD10.DM_PRS_1) + ' ' 
			+ RTRIM(PD10.DM_PRS_LST) + ',' 
			+ COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) AS EmailData,
		1 AS ArcNeeded, 
		SUSER_SNAME() AS AddedBy, 
		GETDATE() AS AddedAt,
		PD32.ALT_EM,
		PH05.DX_CNC_EML_ADR
	FROM
		LN10_LON LN10
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
		LEFT JOIN 
		(
			SELECT
				LN60.BF_SSN
			FROM
				LN60_BR_FOR_APV LN60 
				INNER JOIN FB10_BR_FOR_REQ FB10 
					ON FB10.BF_SSN = LN60.BF_SSN 
					AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
			WHERE
				LN60.LC_STA_LON60 = 'A'
				AND CAST(LN60.LD_FOR_BEG AS DATE) BETWEEN CAST(@Begin AS DATE) AND CAST(@End AS DATE)
				AND FB10.LC_FOR_TYP = '40' 
				AND FB10.LC_FOR_STA = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
				AND LN60.LC_FOR_RSP != '003'	
		) LN60
			ON LN60.BF_SSN = LN10.BF_SSN
		LEFT JOIN LN16_LON_DLQ_HST LN16 
			ON LN16.BF_SSN = LN10.BF_SSN 
			AND	LN16.LN_DLQ_MAX > 0 
			AND LN16.LC_STA_LON16 = '1'	-- Active, delinquent
			AND CAST(LN16.LD_DLQ_MAX AS DATE) BETWEEN CAST(GETDATE() - 4 AS DATE) AND CAST(GETDATE() AS DATE)
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
 					END AS PriorityNumber
				FROM
					PD30_PRS_ADR PD30
				WHERE
					PD30.DI_VLD_ADR = 'Y'
					AND	SUBSTRING(PD30.DF_ZIP_CDE, 1, 5) IN 
					(
						SELECT ZIPS FROM @ZIPS
					) 
			) ADR
		) PD30 
			ON PD30.DF_PRS_ID = LN10.BF_SSN
			AND PD30.AddressPriority = 1
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
	ORDER BY 
		AccountNumber

--INSERT INTO [ULS].emailbatch.EmailProcessing(EmailCampaignId, AccountNumber, ActualFile, EmailData, ArcNeeded, AddedBy, AddedAt)
SELECT
	EmailCampaignId,
	AccountNumber,
	ActualFile,
	EmailData,
	ArcNeeded,
	AddedBy,
	AddedAt
FROM
	@TEMP
WHERE
	(
		PH05Email IS NOT NULL
		OR PD32Email IS NOT NULL
	)
	
SELECT DISTINCT
	AccountNumber,
	@Begin AS BeginDate,
	@End AS EndDate
FROM
	@TEMP
WHERE
	EmailCampaignId = 50 --delinquent


--Claims stuff
SELECT DISTINCT
		PD10.DF_SPE_ACC_ID AS AccountNumber,
		LN10.LN_SEQ
	FROM
		LN10_LON LN10
		INNER JOIN DW01_DW_CLC_CLU DW01 
			ON DW01.BF_SSN = LN10.BF_SSN 
			AND DW01.LN_SEQ = LN10.LN_SEQ
			AND DW01.WC_DW_LON_STA IN ('07','08','09','10','11','12') --only claims
		LEFT JOIN 
		(
			SELECT
				LN60.BF_SSN,
				LN60.LN_SEQ
			FROM
				LN60_BR_FOR_APV LN60 
				INNER JOIN FB10_BR_FOR_REQ FB10 
					ON FB10.BF_SSN = LN60.BF_SSN 
					AND FB10.LF_FOR_CTL_NUM = LN60.LF_FOR_CTL_NUM
			WHERE
				LN60.LC_STA_LON60 = 'A'
				AND CAST(LN60.LD_FOR_BEG AS DATE) = CAST('2017-08-25' AS DATE)
				AND FB10.LC_FOR_TYP = '40' 
				AND FB10.LC_FOR_STA = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
				AND LN60.LC_FOR_RSP != '003'	
		) LN60
			ON LN60.BF_SSN = LN10.BF_SSN
			AND LN60.LN_SEQ = LN10.LN_SEQ
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
 					END AS PriorityNumber
				FROM
					PD30_PRS_ADR PD30
				WHERE
					PD30.DI_VLD_ADR = 'Y'
					AND	SUBSTRING(PD30.DF_ZIP_CDE, 1, 5) IN 
					(
						SELECT ZIPS FROM @ZIPS
					) 
			) ADR
		) PD30 
			ON PD30.DF_PRS_ID = LN10.BF_SSN
			AND PD30.AddressPriority = 1
	WHERE 
		LN10.LC_STA_LON10 = 'R'	-- Released
		AND LN10.LA_CUR_PRI > 0.00
	ORDER BY 
		AccountNumber