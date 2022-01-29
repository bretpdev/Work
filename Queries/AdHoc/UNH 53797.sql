--run on UHEAASQLDB
USE UDW
GO

DECLARE @BEGINDATE DATE = '2017-10-10'
DECLARE @ENDDATE DATE =   '2018-01-07'
DECLARE @ZIPCODES1 TABLE (ZIPS VARCHAR(5))
--DECLARE @ZIPCODES2 TABLE (ZIPS VARCHAR(5))--use if more than 1000 zip codes

INSERT INTO @ZIPCODES1 (ZIPS)
VALUES
 ('95938')
,('95930')
,('95929')
,('95942')
,('95941')
,('95940')
,('95917')
,('95916')
,('95914')
,('95928')
,('95927')
,('95926')
,('95973')
,('95969')
,('95968')
,('95978')
,('95976')
,('95974')
,('95958')
,('95954')
,('95948')
,('95967')
,('95966')
,('95965')
,('92850')
,('92846')
,('92856')
,('92899')
,('92845')
,('92843')
,('90624')
,('92844')
,('90623')
,('92637')
,('92630')
,('92637')
,('92647')
,('92646')
,('95420')
,('95427')
,('95428')
,('95415')
,('95410')
,('95418')
,('95417')
,('95587')
,('95482')
,('95468')
,('95463')
,('95494')
,('95469')
,('95466')
,('95585')
,('95481')
,('95470')
,('95490')
,('95445')
,('95449')
,('95437')
,('95429')
,('95432')
,('95460')
,('95488')
,('95459')
,('95454')
,('95456')
,('92626')
,('92625')
,('92627')
,('92629')
,('92628')
,('92654')
,('92653')
,('92655')
,('92657')
,('92656')
,('92649')
,('92648')
,('92650')
,('92652')
,('94508')
,('94558')
,('94515')
,('94562')
,('94559')
,('94503')
,('94576')
,('94581')
,('94599')
,('94567')
,('94573')
,('94574')
,('95712')
,('96160')
,('95959')
,('95975')
,('96111')
,('95924')
,('96161')
,('95945')
,('95728')
,('95724')
,('96162')
,('95949')
,('95946')
,('95977')
,('95986')
,('95960')
,('92651')
,('92606')
,('92605')
,('92607')
,('92610')
,('92609')
,('90633')
,('90680')
,('92602')
,('92604')
,('92603')
,('92619')
,('92618')
,('92620')
,('92823'), ('92807'), ('90620'), ('95480')
,('92817'), ('92859'), ('92672'), ('95452')
,('92816'), ('92871'), ('92674'), ('94999')
,('92822'), ('92862'), ('92673'), ('95402')
,('92821'), ('92861'), ('92659'), ('95401')
,('92833'), ('92885'), ('92658'), ('94926')
,('92834'), ('92887'), ('92660'), ('95405')
,('92835'), ('92857'), ('92662'), ('94928')
,('92825'), ('90622'), ('92661'), ('95403')
,('92831'), ('92886'), ('92683'), ('95404')
,('92832'), ('92870'), ('92679'), ('94931')
,('92802'), ('90621'), ('92684'), ('94927')
,('92801'), ('92868'), ('92688'), ('94972')
,('92804'), ('92869'), ('92685'), ('95465')
,('92803'), ('92867'), ('92675'), ('95407')
,('92799'), ('92864'), ('90720'), ('95409')
,('92780'), ('92863'), ('92676'), ('95412')
,('92735'), ('92866'), ('92678'), ('94975')
,('92782'), ('92865'), ('92677'), ('95444')
,('92781'), ('92840'), ('90743'), ('94954')
,('92812'), ('92838'), ('90742'), ('94955')
,('92811'), ('92842'), ('90740'), ('95450')
,('92815'), ('92841'), ('95439'), ('95446')
,('92814'), ('92837'), ('95476'), ('95448')
,('92809'), ('90632'), ('95473'), ('95487')
,('92806'), ('92836'), ('95436'), ('95486')
,('92805'), ('90630'), ('95419'), ('95497')
,('92808'), ('90631'), ('95416'), ('95492')
,('92624'), ('92692'), ('95442'), ('95472')
,('92623'), ('92694'), ('95441'), ('95471')
,('92614'), ('92693'), ('95462'), ('95935')
,('92612'), ('92708'), ('94951'), ('95961')
,('92615'), ('92707'), ('95431'), ('95922')
,('92617'), ('92711'), ('94953'), ('95962')
,('92616'), ('92728'), ('95430'), ('95981')
,('92697'), ('92712'), ('94922'), ('95925')
,('92694'), ('92704'), ('95425'), ('95919')
,('92698'), ('92703'), ('95406'), ('95903')
,('92702'), ('92705'), ('95421'), ('95692')
,('92701'), ('90721'), ('95433'), ('95901')
,('92691'), ('92706'), ('94923'), ('95972')
,('92690'), ('92663'), ('94952'), ('95918')

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
				DW01.WC_DW_LON_STA IN ('07','08','09','10','11','12','16','17','20','21') --exclude all borrowers that have any of these statuses on any loan (BKY, death)
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
				AND CAST(LN60.LD_FOR_BEG AS DATE) = @BEGINDATE
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
					INNER JOIN 
					(
						SELECT * FROM @ZIPCODES1
						--UNION
						--SELECT * FROM @ZIPCODES2 --if more than 1000 zip codes
					) Z
						ON SUBSTRING(PD30.DF_ZIP_CDE, 1, 5) = Z.ZIPS
				WHERE
					PD30.DI_VLD_ADR = 'Y'
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
		And DW01.BF_SSN IS NULL
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
	 CONVERT(VARCHAR,@BEGINDATE,101) AS BeginDate,
	 CONVERT(VARCHAR,@ENDDATE,101)  AS EndDate
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
				AND CAST(LN60.LD_FOR_BEG AS DATE) = @BEGINDATE
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
					INNER JOIN 
					(
						SELECT * FROM @ZIPCODES1
						--UNION
						--SELECT * FROM @ZIPCODES2 --if more than 1000 zip codes
					) Z
						ON SUBSTRING(PD30.DF_ZIP_CDE, 1, 5) = Z.ZIPS
				WHERE
					PD30.DI_VLD_ADR = 'Y'
			) ADR
		) PD30 
			ON PD30.DF_PRS_ID = LN10.BF_SSN
			AND PD30.AddressPriority = 1
	WHERE 
		LN10.LC_STA_LON10 = 'R'	-- Released
		AND LN10.LA_CUR_PRI > 0.00
	ORDER BY 
		AccountNumber
