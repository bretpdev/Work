--run on UHEAASQLDB
USE CDW
GO
-- 
--based on CNH XXXXX
-- 
DECLARE @BEGINDATE DATE = 'XXXX-XX-XX'
DECLARE @ENDDATE DATE =   'XXXX-XX-XX'
DECLARE @ZIPCODESX TABLE (ZIPS VARCHAR(X))
--DECLARE @ZIPCODESX TABLE (ZIPS VARCHAR(X))--use if more than XXXX zip codes

INSERT INTO @ZIPCODESX (ZIPS)
VALUES
 ('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')
,('XXXXX'), ('XXXXX'), ('XXXXX'), ('XXXXX')

--SCRIPT FILE:
DECLARE @TEMPX TABLE(EmailCampaignID INT, AccountNumber VARCHAR(XX), ActualFile VARCHAR(XXX), FirstName VARCHAR(XX), LastName VARCHAR(XX), /*EmailData VARCHAR(MAX),*/ ArcNeeded BIT, AddedBy VARCHAR(XXX), AddedAt DATETIME)
INSERT INTO @TEMPX (EmailCampaignID, AccountNumber, ActualFile, FirstName, LastName, /*EmailData,*/ ArcNeeded, AddedBy, AddedAt)
	SELECT DISTINCT
		CASE WHEN LNXX.BF_SSN IS NOT NULL THEN XX /*Delinquent*/ ELSE XX /*Not Delinquent*/ END AS EmailCampaignId,
		PDXX.DF_SPE_ACC_ID AS AccountNumber,
		NULL AS ActualFile,
		RTRIM(PDXX.DM_PRS_X) AS FirstName,
		RTRIM(PDXX.DM_PRS_LST) AS LastName,
		--COALESCE(PHXX.DX_CNC_EML_ADR, PDXX.ALT_EM) AS EmailAddress,
		X AS ArcNeeded, --TODO: Might need to get this via query?
		SUSER_SNAME() AS AddedBy, 
		GETDATE() AS AddedAt
	FROM
		LNXX_LON LNXX
		INNER JOIN DWXX_DW_CLC_CLU DWXX 
			ON DWXX.BF_SSN = LNXX.BF_SSN 
			AND DWXX.WC_DW_LON_STA NOT IN ('XX','XX','XX','XX','XX') -- In repayment TODO: (Double check this)
		LEFT JOIN 
		(
			SELECT
				LNXX.BF_SSN
			FROM
				LNXX_BR_FOR_APV LNXX 
				INNER JOIN FBXX_BR_FOR_REQ FBXX 
					ON FBXX.BF_SSN = LNXX.BF_SSN 
					AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM
			WHERE
				LNXX.LC_STA_LONXX = 'A'
				AND CAST(LNXX.LD_FOR_BEG AS DATE) = @BEGINDATE
				AND FBXX.LC_FOR_TYP = 'XX' 
				AND FBXX.LC_FOR_STA = 'A'
				AND FBXX.LC_STA_FORXX = 'A'
				AND LNXX.LC_FOR_RSP != 'XXX'	
		) LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
		LEFT JOIN LNXX_LON_DLQ_HST LNXX 
			ON LNXX.BF_SSN = LNXX.BF_SSN 
			AND	LNXX.LN_DLQ_MAX > X 
			AND LNXX.LC_STA_LONXX = 'X'	-- Active, delinquent
			AND CAST(LNXX.LD_DLQ_MAX AS DATE) BETWEEN CAST(GETDATE() - X AS DATE) AND CAST(GETDATE() AS DATE)
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
 					END AS PriorityNumber
				FROM
					PDXX_PRS_ADR PDXX
					INNER JOIN 
					(
						SELECT * FROM @ZIPCODESX
						--UNION
						--SELECT * FROM @ZIPCODESX
					) Z
						ON SUBSTRING(PDXX.DF_ZIP_CDE, X, X) = Z.ZIPS
				WHERE
					PDXX.DI_VLD_ADR = 'Y'
			) ADR
		) PDXX 
			ON PDXX.DF_PRS_ID = LNXX.BF_SSN
			AND PDXX.AddressPriority = X
	WHERE 
		LNXX.LC_STA_LONXX = 'R'	-- Released
		AND LNXX.LA_CUR_PRI > X.XX
	ORDER BY 
		AccountNumber


SELECT DISTINCT
	--'ScriptFile' AS ScriptFile
	AccountNumber
	,CONVERT(VARCHAR,@BEGINDATE,XXX) AS BeginDate
	,CONVERT(VARCHAR,@ENDDATE,XXX)  AS EndDate
FROM
	@TEMPX
WHERE
	EmailCampaignID = XX --delinquent





--DELINQUENT & CURRENT EMAIL FILE:
DECLARE @TEMPX TABLE(EmailCampaignID INT, AccountNumber VARCHAR(XX), ActualFile VARCHAR(XXX), FirstName VARCHAR(XX), LastName VARCHAR(XX), EmailData VARCHAR(MAX), ArcNeeded BIT, AddedBy VARCHAR(XXX), AddedAt DATETIME)
INSERT INTO @TEMPX (EmailCampaignID, AccountNumber, ActualFile, FirstName, LastName, EmailData, ArcNeeded, AddedBy, AddedAt)
	SELECT DISTINCT
		CASE WHEN LNXX.BF_SSN IS NOT NULL THEN XX /*Delinquent*/ ELSE XX /*Not Delinquent*/ END AS EmailCampaignId,
		PDXX.DF_SPE_ACC_ID AS AccountNumber,
		NULL AS ActualFile,
		RTRIM(PDXX.DM_PRS_X) AS FirstName,
		RTRIM(PDXX.DM_PRS_LST) AS LastName,
		COALESCE(PHXX.DX_CNC_EML_ADR, PDXX.ALT_EM) AS EmailAddress,
		X AS ArcNeeded, --TODO: Might need to get this via query?
		SUSER_SNAME() AS AddedBy, 
		GETDATE() AS AddedAt
	FROM
		LNXX_LON LNXX
		INNER JOIN DWXX_DW_CLC_CLU DWXX 
			ON DWXX.BF_SSN = LNXX.BF_SSN 
			AND DWXX.WC_DW_LON_STA NOT IN ('XX','XX','XX','XX','XX') -- In repayment TODO: (Double check this)
		LEFT JOIN 
		(
			SELECT
				LNXX.BF_SSN
			FROM
				LNXX_BR_FOR_APV LNXX 
				INNER JOIN FBXX_BR_FOR_REQ FBXX 
					ON FBXX.BF_SSN = LNXX.BF_SSN 
					AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM
			WHERE
				LNXX.LC_STA_LONXX = 'A'
				AND CAST(LNXX.LD_FOR_BEG AS DATE) = @BEGINDATE
				AND FBXX.LC_FOR_TYP = 'XX' 
				AND FBXX.LC_FOR_STA = 'A'
				AND FBXX.LC_STA_FORXX = 'A'
				AND LNXX.LC_FOR_RSP != 'XXX'	
		) LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
		LEFT JOIN LNXX_LON_DLQ_HST LNXX 
			ON LNXX.BF_SSN = LNXX.BF_SSN 
			AND	LNXX.LN_DLQ_MAX > X 
			AND LNXX.LC_STA_LONXX = 'X'	-- Active, delinquent
			AND CAST(LNXX.LD_DLQ_MAX AS DATE) BETWEEN CAST(GETDATE() - X AS DATE) AND CAST(GETDATE() AS DATE)
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
 					END AS PriorityNumber
				FROM
					PDXX_PRS_ADR PDXX
					INNER JOIN 
					(
						SELECT * FROM @ZIPCODESX
						--UNION
						--SELECT * FROM @ZIPCODESX --use if more than XXXX zip codes
					) Z
						ON SUBSTRING(PDXX.DF_ZIP_CDE, X, X) = Z.ZIPS
				WHERE
					PDXX.DI_VLD_ADR = 'Y'
			) ADR
		) PDXX 
			ON PDXX.DF_PRS_ID = LNXX.BF_SSN
			AND PDXX.AddressPriority = X
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
		AND 
		(
			PHXX.DF_SPE_ID IS NOT NULL -- has valid PHXX
			OR PDXX.DF_PRS_ID IS NOT NULL --has valid PDXX
		)
	ORDER BY 
		AccountNumber


SELECT
	--'Delinquent Email File' AS [Delinquent Email File],
	AccountNumber AS [Account Number],
	FirstName AS [First Name],
	LastName AS [Last Name],
	EmailData AS [Email Address]
FROM
	@TEMPX
WHERE
	EmailCampaignID = XX --delinquent


SELECT
--	'Current Email File' AS [Current Email File],
	AccountNumber AS [Account Number],
	FirstName AS [First Name],
	LastName AS [Last Name],
	EmailData AS [Email Address]
FROM
	@TEMPX
WHERE
	EmailCampaignID = XX --Not delinquent
