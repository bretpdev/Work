USE CDW
GO

DECLARE @Begin DATE = 'XX/XX/XXXX'
DECLARE @End DATE = DATEADD(DAY,XX,@Begin)
IF OBJECT_ID('tempdb..#POP') IS NOT NULL 
	DROP TABLE #POP
--DECLARE @TEMP TABLE(EmailCampaignID INT, AccountNumber VARCHAR(XX), ActualFile VARCHAR(XXX), EmailData VARCHAR(MAX), ArcNeeded BIT, AddedBy VARCHAR(XXX), AddedAt DATETIME, PDXXEmail VARCHAR(XXX), PHXXEmail VARCHAR(XXX))
--INSERT INTO @TEMP (EmailCampaignID, AccountNumber, ActualFile, EmailData, ArcNeeded, AddedBy, AddedAt, PDXXEmail, PHXXEmail)
	SELECT DISTINCT
		pdXX.df_prs_id as ssn,
			RTRIM(PDXX.DM_PRS_X) + ' ' 	+ RTRIM(PDXX.DM_PRS_LST) as name,
			PDXX.DD_VER_ADR,
			case 
				when pdXX.DD_VER_ADR < 'XX/XX/XXXX' then X
				when pdXX.DD_VER_ADR between 'XX/XX/XXXX' and 'XX/XX/XXXX' then X
				when pdXX.DD_VER_ADR between 'XX/XX/XXXX' and 'XX/XX/XXXX' then X
				when pdXX.DD_VER_ADR between 'XX/XX/XXXX' and 'XX/XX/XXXX' then X
				when pdXX.DD_VER_ADR between 'XX/XX/XXXX' and 'XX/XX/XXXX' then X
				when pdXX.DD_VER_ADR between 'XX/XX/XXXX' and 'XX/XX/XXXX' then X
				when pdXX.DD_VER_ADR between 'XX/XX/XXXX' and getdate() then X
				else X
			end as [file]
		into #pop
	FROM
		LNXX_LON LNXX
		INNER JOIN PDXX_PRS_NME PDXX 
			ON PDXX.DF_PRS_ID = LNXX.BF_SSN
		INNER JOIN 
		(
			SELECT
				ADR.DF_PRS_ID,
				ADR.DX_STR_ADR_X,
				ADR.DX_STR_ADR_X,
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
					PDXX.DF_PRS_ID,
					PDXX.DX_STR_ADR_X,
					PDXX.DX_STR_ADR_X,
					PDXX.DM_CT,
					PDXX.DC_DOM_ST,
					PDXX.DF_ZIP_CDE,
					PDXX.DM_FGN_CNY,
					PDXX.DM_FGN_ST,
					CASE	  
 						WHEN DC_ADR = 'L' THEN X -- legal 
 						WHEN DC_ADR = 'B' THEN X -- billing
 						WHEN DC_ADR = 'D' THEN X -- disbursement
 					END AS PriorityNumber,
					pdXX.DD_VER_ADR
				FROM
					PDXX_PRS_ADR PDXX
				WHERE
					PDXX.DI_VLD_ADR = 'Y'
					AND	SUBSTRING(PDXX.DF_ZIP_CDE, X, X) IN 
					(
						'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX',
'XXXXX'


					) 
			) ADR
		) PDXX 
			ON PDXX.DF_PRS_ID = LNXX.BF_SSN
			AND PDXX.AddressPriority = X
		LEFT OUTER JOIN 
		(
			SELECT
				DWXX.BF_SSN
			FROM
				LNXX_LON LNXX
				INNER JOIN DWXX_DW_CLC_CLU DWXX 
					ON DWXX.BF_SSN = LNXX.BF_SSN
					AND DWXX.LN_SEQ = LNXX.LN_SEQ 
			WHERE
				DWXX.WC_DW_LON_STA IN ('XX','XX','XX','XX','XX','XX','XX','XX','XX','XX') --exclude all borrowers that have any of these statuses on any loan (BKY, death, claim)
		) DWXX
			ON DWXX.BF_SSN = LNXX.BF_SSN
		
		LEFT JOIN LNXX_LON_DLQ_HST LNXX 
			ON LNXX.BF_SSN = LNXX.BF_SSN 
			AND	LNXX.LN_DLQ_MAX > X 
			AND LNXX.LC_STA_LONXX = 'X'	-- Active, delinquent
			AND CAST(LNXX.LD_DLQ_MAX AS DATE) BETWEEN CAST(GETDATE() - X AS DATE) AND CAST(GETDATE() AS DATE)
		
		LEFT JOIN PHXX_CNC_EML PHXX 
			ON PHXX.DF_SPE_ID = PDXX.DF_SPE_ACC_ID
			AND	PHXX.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
			AND PHXX.DI_CNC_ELT_OPI = 'Y' --on ecorr
	    LEFT JOIN
		( -- email address 
			SELECT 
				DF_PRS_ID, 
				Email.EM [ALT_EM],
 				ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) [EmailPriority] -- number in order of Email.PriorityNumber 
 			FROM 
 			( 
 				SELECT 
 					PDXX.DF_PRS_ID, 
 					PDXX.DX_ADR_EML [EM], 
 					CASE	  
 						WHEN DC_ADR_EML = 'H' THEN X -- home 
 						WHEN DC_ADR_EML = 'A' THEN X -- alternate 
 						WHEN DC_ADR_EML = 'W' THEN X -- work 
 					END AS PriorityNumber
 				FROM 
 					PDXX_PRS_ADR_EML PDXX 
 				WHERE 
 					PDXX.DI_VLD_ADR_EML = 'Y' -- valid email address 
 					AND PDXX.DC_STA_PDXX = 'A' -- active email address record 

 			) Email 
		) PDXX 
			ON PDXX.DF_PRS_ID = PDXX.DF_PRS_ID
			AND PDXX.EmailPriority = X
	WHERE 
		LNXX.LC_STA_LONXX = 'R'	-- Released
		AND LNXX.LA_CUR_PRI > X.XX
		AND DWXX.BF_SSN IS NULL

SELECT SSN,NAME, DD_VER_ADR FROM #POP WHERE [FILE] = X
SELECT SSN,NAME, DD_VER_ADR FROM #POP WHERE [FILE] in (X,X)
SELECT SSN,NAME, DD_VER_ADR FROM #POP WHERE [FILE] in (X,X,X)
SELECT SSN,NAME, DD_VER_ADR FROM #POP WHERE [FILE] in (X,X,X,X)
SELECT SSN,NAME, DD_VER_ADR FROM #POP WHERE [FILE] in (X,X,X,X,X)
SELECT SSN,NAME, DD_VER_ADR FROM #POP WHERE [FILE] in (X,X,X,X,X,X)
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
--		PHXXEmail IS NOT NULL
--		OR PDXXEmail IS NOT NULL
--	)
	
--SELECT DISTINCT
--	AccountNumber,
--	@Begin AS BeginDate,
--	@End AS EndDate
--FROM
--	@TEMP
--WHERE
--	EmailCampaignId = XX --delinquent


------Claims stuff
--SELECT DISTINCT
--		PDXX.DF_SPE_ACC_ID AS AccountNumber,
--		LNXX.LN_SEQ
--	FROM
--		LNXX_LON LNXX
--		INNER JOIN DWXX_DW_CLC_CLU DWXX 
--			ON DWXX.BF_SSN = LNXX.BF_SSN 
--			AND DWXX.LN_SEQ = LNXX.LN_SEQ
--			AND DWXX.WC_DW_LON_STA IN ('XX','XX','XX','XX','XX','XX') --only claims
--		LEFT JOIN 
--		(
--			SELECT
--				LNXX.BF_SSN,
--				LNXX.LN_SEQ
--			FROM
--				LNXX_BR_FOR_APV LNXX 
--				INNER JOIN FBXX_BR_FOR_REQ FBXX 
--					ON FBXX.BF_SSN = LNXX.BF_SSN 
--					AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM
--			WHERE
--				LNXX.LC_STA_LONXX = 'A'
--				AND CAST(LNXX.LD_FOR_BEG AS DATE) = CAST('XXXX-XX-XX' AS DATE)
--				AND FBXX.LC_FOR_TYP = 'XX' 
--				AND FBXX.LC_FOR_STA = 'A'
--				AND FBXX.LC_STA_FORXX = 'A'
--				AND LNXX.LC_FOR_RSP != 'XXX'	
--		) LNXX
--			ON LNXX.BF_SSN = LNXX.BF_SSN
--			AND LNXX.LN_SEQ = LNXX.LN_SEQ
--		INNER JOIN PDXX_PRS_NME PDXX 
--			ON PDXX.DF_PRS_ID = LNXX.BF_SSN
--		INNER JOIN 
--		(
--			SELECT
--				ADR.DF_PRS_ID,
--				ADR.DX_STR_ADR_X,
--				ADR.DX_STR_ADR_X,
--				ADR.DM_CT,
--				ADR.DC_DOM_ST,
--				ADR.DF_ZIP_CDE,
--				ADR.DM_FGN_CNY,
--				ADR.DM_FGN_ST,
--				ROW_NUMBER() OVER (PARTITION BY ADR.DF_PRS_ID ORDER BY ADR.PriorityNumber) [AddressPriority] -- number in order of Address
--			FROM
--			(
--				SELECT
--					PDXX.DF_PRS_ID,
--					PDXX.DX_STR_ADR_X,
--					PDXX.DX_STR_ADR_X,
--					PDXX.DM_CT,
--					PDXX.DC_DOM_ST,
--					PDXX.DF_ZIP_CDE,
--					PDXX.DM_FGN_CNY,
--					PDXX.DM_FGN_ST,
--					CASE	  
-- 						WHEN DC_ADR = 'L' THEN X -- legal 
-- 						WHEN DC_ADR = 'B' THEN X -- billing
-- 						WHEN DC_ADR = 'D' THEN X -- disbursement
-- 					END AS PriorityNumber
--				FROM
--					PDXX_PRS_ADR PDXX
--				WHERE
--					PDXX.DI_VLD_ADR = 'Y'
--					AND	SUBSTRING(PDXX.DF_ZIP_CDE, X, X) IN 
--					(
--					'XXXXX',
--					'XXXXX',
--					'XXXXX',
--					'XXXXX',
--					'XXXXX',
--					'XXXXX',
--					'XXXXX',
--					'XXXXX',
--					'XXXXX',
--					'XXXXX',
--					'XXXXX',
--					'XXXXX',
--					'XXXXX',
--					'XXXXX',
--					'XXXXX',
--					'XXXXX',
--					'XXXXX',
--					'XXXXX',
--					'XXXXX'
--					) 
--			) ADR
--		) PDXX 
--			ON PDXX.DF_PRS_ID = LNXX.BF_SSN
--			AND PDXX.AddressPriority = X
--	WHERE 
--		LNXX.LC_STA_LONXX = 'R'	-- Released
--		AND LNXX.LA_CUR_PRI > X.XX
--	ORDER BY 
--		AccountNumber