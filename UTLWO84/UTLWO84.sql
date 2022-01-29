BEGIN TRY
	BEGIN TRANSACTION

		DECLARE @LetterId VARCHAR(10) = 'INTERESTNO',
				@Arc VARCHAR(5) = 'LSINB',
				@Today DATE = GETDATE(),
				@NOW DATETIME = GETDATE(),
				@ScriptId VARCHAR(7) = 'UTLWO84';

		DROP TABLE IF EXISTS #Base,#Demos,#LoanDetail,#LoanPrep,#Output;

		--base population
		SELECT
			BF_SSN,
			LN_SEQ,
			UheaaCC,
			NONUheaaCC,
			LOAN_PROGRAM,
			CURRENT_OWNER,
			CURRENT_PRINCIPAL,
			CURRENT_OUTSTANDING_INTEREST,
			PROJECTED_INTEREST_DURING_FORBEARANCE,
			PROJECTED_INTEREST_CAPITALIZATION_DATE
		INTO
			#Base
		FROM
		( --Gather loan information for letter data
			SELECT DISTINCT
				LN10.BF_SSN,
				LN10.LN_SEQ,
				CostCenter.UheaaCC,
				CostCenter.NONUheaaCC,
				ISNULL(FMT.Label,LN10.IC_LON_PGM) AS LOAN_PROGRAM,
				ISNULL(OW10.IM_OWN_SHO, LN10.LF_LON_CUR_OWN) AS CURRENT_OWNER,
				LN10.LA_CUR_PRI AS CURRENT_PRINCIPAL,
				DW01.LA_NSI_ACR AS CURRENT_OUTSTANDING_INTEREST,
				ABS(CAST(LN10.LA_CUR_PRI * (LN72.LR_ITR / 100 / CentralData.dbo.DaysInYear(YEAR(@Today))) * DATEDIFF(DAY,LN60.LD_FOR_BEG,LN60.LD_FOR_END) AS DECIMAL(10,2))) AS PROJECTED_INTEREST_DURING_FORBEARANCE,
				CAST(DATEADD(DAY,1,LN60.LD_FOR_END) AS DATE) AS PROJECTED_INTEREST_CAPITALIZATION_DATE
			FROM 
				UDW..LN10_LON LN10
				INNER JOIN UDW..LN60_BR_FOR_APV LN60
					ON LN10.BF_SSN = LN60.BF_SSN
					AND LN10.LN_SEQ = LN60.LN_SEQ
					AND LN60.LC_STA_LON60 = 'A' --active
					AND LN60.LC_FOR_RSP IN ('000','001','070') --AND LN60.LC_FOR_RSP != '003' --not denied
					AND CAST(LN60.LD_FOR_END AS DATE) > CAST(@Today AS DATE)
				INNER JOIN UDW..FB10_BR_FOR_REQ FB10
					ON LN60.BF_SSN = FB10.BF_SSN
					AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
					AND FB10.LC_FOR_STA = 'A' --approved
					AND FB10.LC_STA_FOR10 = 'A' --active
					AND CASE
							WHEN CAST(FB10.LD_FOR_INF_CER AS DATE) < CAST(LN60.LD_FOR_BEG AS DATE) 
							THEN DATEDIFF(DAY, LN60.LD_FOR_BEG, @Today) --Forb certified prior to starting, and has been going for at least 175 days
							ELSE DATEDIFF(DAY, FB10.LD_FOR_INF_CER, @Today) --Forb certified after starting, use cert date as start date
						END > 175 --Forbearance has been active for at least 175 days
				INNER JOIN UDW..DW01_DW_CLC_CLU DW01
					ON LN10.BF_SSN = DW01.BF_SSN
					AND LN10.LN_SEQ = DW01.LN_SEQ
				INNER JOIN 
				( --Current interest rate
					SELECT
						LN72.BF_SSN,
						LN72.LN_SEQ,
						LN72.LR_ITR,
						ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ ORDER BY LD_STA_LON72 DESC) AS ValidLN72
					FROM	
						UDW..LN72_INT_RTE_HST LN72
					WHERE
						LN72.LC_STA_LON72 = 'A' --active
						AND CAST(@Today AS DATE) BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
				) LN72
					ON LN72.BF_SSN = LN10.BF_SSN
					AND LN72.LN_SEQ = LN10.LN_SEQ
					AND LN72.ValidLN72 = 1
				LEFT JOIN 
				( --Most recent LSINB arc
					SELECT DISTINCT
						LN85.BF_SSN,
						LN85.LN_SEQ,
						MAX(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
					FROM
						UDW..AY10_BR_LON_ATY AY10
						INNER JOIN UDW..LN85_LON_ATY LN85
							ON AY10.BF_SSN = LN85.BF_SSN
							AND AY10.LN_ATY_SEQ = LN85.LN_ATY_SEQ
					WHERE 
						AY10.PF_REQ_ACT IN ('LSINB','FOR06')
						AND AY10.LC_STA_ACTY10 = 'A' --active
					GROUP BY 
						LN85.BF_SSN,
						LN85.LN_SEQ
				) LSINB_ARC
					ON LN10.BF_SSN = LSINB_ARC.BF_SSN
					AND LN10.LN_SEQ = LSINB_ARC.LN_SEQ
				LEFT JOIN
				( --Determine cost center for mailing
					SELECT DISTINCT
						BF_SSN,
						LN_SEQ,
						IIF(LEFT(LF_LON_CUR_OWN,6) IN ('828476','834396','834437','834493','834529','826717','830248','971357','900749','829769'), 1, 0) AS UheaaCC,
						IIF(LEFT(LF_LON_CUR_OWN,6) NOT IN ('828476','834396','834437','834493','834529','826717','830248','971357','900749','829769'), 1, 0) AS NONUheaaCC
					FROM
						UDW..LN10_LON
					WHERE
						LA_CUR_PRI > 0.00
						AND LC_STA_LON10 = 'R' --released
				) CostCenter
					ON CostCenter.BF_SSN = LN10.BF_SSN
					AND CostCenter.LN_SEQ = LN10.LN_SEQ
				LEFT JOIN UDW..FormatTranslation FMT
					ON FMT.FmtName = '$LNPROG'
					AND FMT.Start = LN10.IC_LON_PGM
				LEFT JOIN UDW..OW10_OWN OW10
					ON OW10.IF_OWN = LN10.LF_LON_CUR_OWN
				LEFT JOIN 
				(--LN10.IC_LON_PGM != 'TILP' --TEST
					SELECT
						BF_SSN
					FROM
						UDW..LN10_LON
					WHERE
						IC_LON_PGM = 'TILP'
				) TILP
					ON LN10.BF_SSN = TILP.BF_SSN
			WHERE 
				TILP.BF_SSN IS NULL
				AND LN10.LC_STA_LON10 = 'R' --released
				AND LN10.LA_CUR_PRI > 0.00
				AND (
						--LN10.LI_CON_PAY_STP_PUR != 'Y' --TEST
						LN10.LI_CON_PAY_STP_PUR IN ('N',' ')
						OR LN10.LI_CON_PAY_STP_PUR IS NULL
					)
				AND DATEDIFF(DAY,ISNULL(LSINB_ARC.LD_ATY_REQ_RCV,'1900-01-01'),@Today) >= 175 --most recent LSINB arc happened 175 days or more ago
		) Base
		;

		--demographic data
		SELECT
			BR_ID,
			ACCOUNT,
			FIRSTNAME,
			RTRIM(CONCAT(LASTNAME,' ',SUFFIX)) AS LASTNAME,
			ADDRESS1,
			ADDRESS2,
			CITY,
			[STATE],
			ZIP,
			COUNTRY,
			DI_VLD_ADR,
			ACSKEY,
			COST_CENTER_CODE,
			Email,
			OnEcorr,
			ScriptDataFlag
		INTO
			#Demos
		FROM
		(--gather demographic data
			SELECT 
				PD10.DF_PRS_ID AS BR_ID,
				PD10.DF_SPE_ACC_ID AS ACCOUNT,
				CentralData.dbo.TRIM(PD10.DM_PRS_1) AS FIRSTNAME,
				CentralData.dbo.TRIM(PD10.DM_PRS_LST) AS LASTNAME,
				CentralData.dbo.TRIM(PD10.DM_PRS_LST_SFX) AS SUFFIX,
				CentralData.dbo.TRIM(PD30.DX_STR_ADR_1) AS ADDRESS1,
				CentralData.dbo.TRIM(PD30.DX_STR_ADR_2) AS ADDRESS2,
				CentralData.dbo.TRIM(PD30.DM_CT) AS CITY,
				CASE 
					WHEN ISNULL(PD30.DC_DOM_ST,'') = '' AND ISNULL(PD30.DM_FGN_ST,'') != '' 
					THEN CentralData.dbo.TRIM(PD30.DM_FGN_ST) 
					ELSE CentralData.dbo.TRIM(PD30.DC_DOM_ST) 
				END AS [STATE],
				CentralData.dbo.TRIM(PD30.DF_ZIP_CDE) AS ZIP,
				CentralData.dbo.TRIM(PD30.DM_FGN_CNY) AS COUNTRY,
				PD30.DI_VLD_ADR AS DI_VLD_ADR,
				CentralData.dbo.CreateACSKeyline(PD10.DF_PRS_ID, 'B', 'L') AS ACSKEY,
				CASE 
					WHEN SUM(B.UheaaCC) OVER(PARTITION BY B.BF_SSN) >= SUM(B.NONUheaaCC) OVER(PARTITION BY B.BF_SSN)
					THEN 'MA2324' 
					ELSE 'MA2327' 
				END AS COST_CENTER_CODE,
				COALESCE(EMAIL.EmailAddress,'Ecorr@UHEAA.org') AS Email,
				IIF(PH05.DX_CNC_EML_ADR IS NOT NULL, 1, 0) AS OnEcorr,
				IIF(PD30.DI_VLD_ADR = 'Y', 'BV', 'BI') AS ScriptDataFlag
			FROM
				#Base B
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON PD10.DF_PRS_ID = B.BF_SSN
				INNER JOIN UDW..PD30_PRS_ADR PD30
					ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
					AND PD30.DC_ADR = 'L' --legal
				LEFT JOIN UDW.calc.EmailAddress EMAIL
					ON EMAIL.DF_PRS_ID = PD10.DF_PRS_ID
				LEFT JOIN UDW..PH05_CNC_EML PH05
					ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
					AND PH05.DI_CNC_ELT_OPI = 'Y'
					AND PH05.DI_VLD_CNC_EML_ADR = 'Y' 

			UNION ALL

			SELECT 
				COMAKER.CO_ID AS BR_ID,
				PD10Bor.DF_SPE_ACC_ID AS ACCOUNT,
				CentralData.dbo.TRIM(COMAKER.DM_PRS_1) AS FIRSTNAME,
				CentralData.dbo.TRIM(COMAKER.DM_PRS_LST) AS LASTNAME,
				CentralData.dbo.TRIM(COMAKER.DM_PRS_LST_SFX) AS SUFFIX,
				CentralData.dbo.TRIM(COMAKER.DX_STR_ADR_1) AS ADDRESS1,
				CentralData.dbo.TRIM(COMAKER.DX_STR_ADR_2) AS ADDRESS2,
				CentralData.dbo.TRIM(COMAKER.DM_CT) AS CITY,
				CASE
					WHEN ISNULL(COMAKER.DC_DOM_ST,'') = '' AND ISNULL(COMAKER.DM_FGN_ST,'') != '' 
					THEN CentralData.dbo.TRIM(COMAKER.DM_FGN_ST)
					ELSE CentralData.dbo.TRIM(COMAKER.DC_DOM_ST)
				END AS [STATE],
				CentralData.dbo.TRIM(COMAKER.DF_ZIP_CDE) AS ZIP,
				CentralData.dbo.TRIM(COMAKER.DM_FGN_CNY) AS COUNTRY,
				COMAKER.DI_VLD_ADR AS DI_VLD_ADR,
				CentralData.dbo.CreateACSKeyline(COMAKER.CO_ID, 'B', 'L') AS ACSKEY,
				CASE 
					WHEN SUM(B.UheaaCC) OVER(PARTITION BY B.BF_SSN) >= SUM(B.NONUheaaCC) OVER(PARTITION BY B.BF_SSN) 
					THEN 'MA2324' 
					ELSE 'MA2327' 
				END AS COST_CENTER_CODE,
				COALESCE(EMAIL.EmailAddress,'Ecorr@UHEAA.org') AS Email,
				IIF(PH05.DX_CNC_EML_ADR IS NOT NULL, 1, 0 ) AS OnEcorr,
				IIF(COMAKER.DI_VLD_ADR = 'Y', 'CV', 'CI') AS ScriptDataFlag
			FROM
				#Base B
				INNER JOIN 
				( --Comaker demographics
					SELECT
						CLN20.LF_EDS AS CO_ID,
						CPD10.DF_SPE_ACC_ID,
						CPD10.DM_PRS_1,
						CPD10.DM_PRS_LST,
						CPD10.DM_PRS_LST_SFX,
						CPD30.DX_STR_ADR_1,
						CPD30.DX_STR_ADR_2,
						CPD30.DM_CT,
						CPD30.DC_DOM_ST,
						CPD30.DF_ZIP_CDE,
						CPD30.DM_FGN_CNY,
						CPD30.DM_FGN_ST,
						CPD30.DI_VLD_ADR,
						CLN20.BF_SSN,
						CLN20.LN_SEQ
					FROM 
						UDW..LN20_EDS CLN20
						INNER JOIN UDW..PD10_PRS_NME CPD10
							ON CLN20.LF_EDS = CPD10.DF_PRS_ID
						INNER JOIN UDW..PD30_PRS_ADR CPD30
							ON CPD10.DF_PRS_ID = CPD30.DF_PRS_ID
					WHERE
						CLN20.LC_STA_LON20 = 'A' --active
						AND CLN20.LC_EDS_TYP = 'M' --comaker
						AND CPD30.DC_ADR = 'L' --legal
				) COMAKER
					ON COMAKER.BF_SSN = B.BF_SSN
					AND COMAKER.LN_SEQ = B.LN_SEQ
				INNER JOIN UDW..PD10_PRS_NME PD10Bor
					ON PD10Bor.DF_PRS_ID = COMAKER.BF_SSN
				LEFT JOIN UDW.calc.EmailAddress EMAIL
					ON EMAIL.DF_PRS_ID = COMAKER.CO_ID
				LEFT JOIN UDW..PH05_CNC_EML PH05
					ON PH05.DF_SPE_ID = COMAKER.DF_SPE_ACC_ID
					AND PH05.DI_CNC_ELT_OPI = 'Y'
					AND PH05.DI_VLD_CNC_EML_ADR = 'Y'

			UNION ALL

			SELECT 
				ENDORSER.EN_ID AS BR_ID,
				PD10Bor.DF_SPE_ACC_ID AS ACCOUNT,
				CentralData.dbo.TRIM(ENDORSER.DM_PRS_1) AS FIRSTNAME,
				CentralData.dbo.TRIM(ENDORSER.DM_PRS_LST) AS LASTNAME,
				CentralData.dbo.TRIM(ENDORSER.DM_PRS_LST_SFX) AS SUFFIX,
				CentralData.dbo.TRIM(ENDORSER.DX_STR_ADR_1) AS ADDRESS1,
				CentralData.dbo.TRIM(ENDORSER.DX_STR_ADR_2) AS ADDRESS2,
				CentralData.dbo.TRIM(ENDORSER.DM_CT) AS CITY,
				CASE 
					WHEN ISNULL(ENDORSER.DC_DOM_ST,'') = '' AND ISNULL(ENDORSER.DM_FGN_ST,'') != ''
					THEN CentralData.dbo.TRIM(ENDORSER.DM_FGN_ST) 
					ELSE CentralData.dbo.TRIM(ENDORSER.DC_DOM_ST) 
				END AS [STATE],
				CentralData.dbo.TRIM(ENDORSER.DF_ZIP_CDE) AS ZIP,
				CentralData.dbo.TRIM(ENDORSER.DM_FGN_CNY) AS COUNTRY,
				ENDORSER.DI_VLD_ADR AS DI_VLD_ADR,
				CentralData.dbo.CreateACSKeyline(ENDORSER.EN_ID, 'B', 'L') AS ACSKEY,
				CASE
					WHEN SUM(B.UheaaCC) OVER(PARTITION BY B.BF_SSN) >= SUM(B.NONUheaaCC) OVER(PARTITION BY B.BF_SSN) 
					THEN 'MA2324' 
					ELSE 'MA2327' 
				END AS COST_CENTER_CODE,
				COALESCE(EMAIL.EmailAddress,'Ecorr@UHEAA.org') AS Email,
				IIF(PH05.DX_CNC_EML_ADR IS NOT NULL, 1, 0) AS OnEcorr,
				IIF(ENDORSER.DI_VLD_ADR = 'Y', 'EV', 'EI') AS ScriptDataFlag
			FROM
				#Base B
				INNER JOIN 
				( --Endorser demographics
					SELECT
						ELN20.LF_EDS AS EN_ID,
						EPD10.DF_SPE_ACC_ID,
						EPD10.DM_PRS_1,
						EPD10.DM_PRS_LST,
						EPD10.DM_PRS_LST_SFX,
						EPD30.DX_STR_ADR_1,
						EPD30.DX_STR_ADR_2,
						EPD30.DM_CT,
						EPD30.DC_DOM_ST,
						EPD30.DF_ZIP_CDE,
						EPD30.DM_FGN_CNY,
						EPD30.DM_FGN_ST,
						EPD30.DI_VLD_ADR,
						ELN20.BF_SSN,
						ELN20.LN_SEQ
					FROM 
						UDW..LN20_EDS ELN20
						INNER JOIN UDW..PD10_PRS_NME EPD10
							ON ELN20.LF_EDS = EPD10.DF_PRS_ID
						INNER JOIN UDW..PD30_PRS_ADR EPD30
							ON EPD10.DF_PRS_ID = EPD30.DF_PRS_ID
					WHERE
						ELN20.LC_STA_LON20 = 'A' --active
						AND ELN20.LC_EDS_TYP = 'S' --Cosigner
						AND EPD30.DC_ADR = 'L' --legal
				) ENDORSER
					ON ENDORSER.BF_SSN = B.BF_SSN
					AND ENDORSER.LN_SEQ = B.LN_SEQ
				INNER JOIN UDW..PD10_PRS_NME PD10Bor
					ON PD10Bor.DF_PRS_ID = ENDORSER.BF_SSN
				INNER JOIN UDW..PD30_PRS_ADR PD30Bor
					ON PD30Bor.DF_PRS_ID = PD10Bor.DF_PRS_ID
					AND PD30Bor.DC_ADR = 'L'
					AND PD30Bor.DI_VLD_ADR = 'N' --Borrower address is bad. Contact Student instead
				LEFT JOIN UDW.calc.EmailAddress EMAIL
					ON EMAIL.DF_PRS_ID = ENDORSER.EN_ID
				LEFT JOIN UDW..PH05_CNC_EML PH05
					ON PH05.DF_SPE_ID = ENDORSER.DF_SPE_ACC_ID
					AND PH05.DI_CNC_ELT_OPI = 'Y'
					AND PH05.DI_VLD_CNC_EML_ADR = 'Y'
		) Demos
		;

		SELECT
			BR_ID,
			ACCOUNT,
			LN_SEQ,
			LOAN_PROGRAM,
			CURRENT_OWNER,
			CURRENT_PRINCIPAL,
			CURRENT_OUTSTANDING_INTEREST,
			PROJECTED_INTEREST_DURING_FORBEARANCE,
			PROJECTED_INTEREST_CAPITALIZATION_DATE,
			LoanCount
		INTO
			#LoanDetail
		FROM
		(
			SELECT DISTINCT
				PD10.DF_PRS_ID AS BR_ID,
				PD10.DF_SPE_ACC_ID AS ACCOUNT,
				B.LN_SEQ,
				B.LOAN_PROGRAM,
				B.CURRENT_OWNER,
				B.CURRENT_PRINCIPAL,
				B.CURRENT_OUTSTANDING_INTEREST,
				B.PROJECTED_INTEREST_DURING_FORBEARANCE,
				B.PROJECTED_INTEREST_CAPITALIZATION_DATE,
				COUNT(*) OVER(PARTITION BY B.BF_SSN) AS LoanCount
			FROM 
				#Base B
				INNER JOIN UDW..PD10_PRS_NME PD10
					ON PD10.DF_PRS_ID = B.BF_SSN	

			UNION ALL 

			SELECT DISTINCT
				COMAKER.CO_ID AS BR_ID,
				PD10Bor.DF_SPE_ACC_ID AS ACCOUNT,
				B.LN_SEQ,
				B.LOAN_PROGRAM,
				B.CURRENT_OWNER,
				B.CURRENT_PRINCIPAL,
				B.CURRENT_OUTSTANDING_INTEREST,
				B.PROJECTED_INTEREST_DURING_FORBEARANCE,
				B.PROJECTED_INTEREST_CAPITALIZATION_DATE,
				COUNT(*) OVER(PARTITION BY B.BF_SSN) AS LoanCount
			FROM
				#Base B
				INNER JOIN 
				( --Active Comakers that need to receive letter
					SELECT
						CLN20.LF_EDS AS CO_ID,
						CLN20.BF_SSN,
						CLN20.LN_SEQ
					FROM 
						UDW..LN20_EDS CLN20
						INNER JOIN UDW..PD10_PRS_NME CPD10
							ON CLN20.LF_EDS = CPD10.DF_PRS_ID
					WHERE
						CLN20.LC_STA_LON20 = 'A' --active
						AND CLN20.LC_EDS_TYP = 'M' --comaker
				) COMAKER
					ON COMAKER.BF_SSN = B.BF_SSN
					AND COMAKER.LN_SEQ = B.LN_SEQ
				INNER JOIN UDW..PD10_PRS_NME PD10Bor
					ON PD10Bor.DF_PRS_ID = COMAKER.BF_SSN

			UNION ALL

			SELECT DISTINCT
				ENDORSER.EN_ID AS BR_ID,
				PD10Bor.DF_SPE_ACC_ID AS ACCOUNT,
				B.LN_SEQ,
				B.LOAN_PROGRAM,
				B.CURRENT_OWNER,
				B.CURRENT_PRINCIPAL,
				B.CURRENT_OUTSTANDING_INTEREST,
				B.PROJECTED_INTEREST_DURING_FORBEARANCE,
				B.PROJECTED_INTEREST_CAPITALIZATION_DATE,
				COUNT(*) OVER(PARTITION BY B.BF_SSN) AS LoanCount
			FROM 
				#Base B
				INNER JOIN 
				( --Active Endorsers that need to receive letter
					SELECT
						ELN20.LF_EDS AS EN_ID,
						ELN20.BF_SSN,
						ELN20.LN_SEQ
					FROM 
						UDW..LN20_EDS ELN20
						INNER JOIN UDW..PD10_PRS_NME EPD10
							ON ELN20.LF_EDS = EPD10.DF_PRS_ID
					WHERE
						ELN20.LC_STA_LON20 = 'A' --active
						AND ELN20.LC_EDS_TYP = 'S' --Cosigner
				) ENDORSER
					ON ENDORSER.BF_SSN = B.BF_SSN
					AND ENDORSER.LN_SEQ = B.LN_SEQ
				INNER JOIN UDW..PD10_PRS_NME PD10Bor
					ON PD10Bor.DF_PRS_ID = ENDORSER.BF_SSN
				INNER JOIN UDW..PD30_PRS_ADR PD30Bor
					ON PD30Bor.DF_PRS_ID = PD10Bor.DF_PRS_ID
					AND PD30Bor.DC_ADR = 'L'
					AND PD30Bor.DI_VLD_ADR = 'N' --Borrower address is bad. Contact Student instead
		) Loans
		;

		DECLARE @MaxLoans INT = 30;
		;WITH LoanCount(loans) AS 
		(
			SELECT 1 
			UNION ALL
			SELECT loans + 1 FROM LoanCount
			WHERE loans < @MaxLoans
		)
		SELECT DISTINCT
			LD.BR_ID,
			LD.ACCOUNT,
			LD.LN_SEQ,
			LD.LOAN_PROGRAM,
			LD.CURRENT_OWNER,
			LD.CURRENT_PRINCIPAL,
			LD.CURRENT_OUTSTANDING_INTEREST,
			LD.PROJECTED_INTEREST_DURING_FORBEARANCE,
			LD.PROJECTED_INTEREST_CAPITALIZATION_DATE,
			LC.loans
		INTO
			#LoanPrep
		FROM
			LoanCount LC
			CROSS JOIN
			( --Get 30 loan records' worth of data even if they dont have 30 loans
				SELECT
					L.BR_ID,
					L.ACCOUNT,
					L.LN_SEQ,
					L.LOAN_PROGRAM,
					L.CURRENT_OWNER,
					L.CURRENT_PRINCIPAL,
					L.CURRENT_OUTSTANDING_INTEREST,
					L.PROJECTED_INTEREST_DURING_FORBEARANCE,
					L.PROJECTED_INTEREST_CAPITALIZATION_DATE
				FROM
					#LoanDetail L
				WHERE
					L.LoanCount <= @MaxLoans
			) LD
		ORDER BY
			LD.BR_ID,
			LD.LN_SEQ
		;

		SELECT DISTINCT
			D.ACCOUNT AS AccountNumber,
			D.Email AS EmailAddress,
			CASE
				WHEN D.ScriptDataFlag = 'BV' THEN 191 --191 for live 277 for test
				WHEN D.ScriptDataFlag = 'BI' THEN 192 --192 for live 278 for test
				WHEN D.ScriptDataFlag = 'CV' THEN 193 --193 for live 279 for test
				WHEN D.ScriptDataFlag = 'CI' THEN 194 --194 for live 280 for test
				WHEN D.ScriptDataFlag = 'EV' THEN 195 --195 for live 281 for test
				WHEN D.ScriptDataFlag = 'EI' THEN 196 --196 for live 282 for test
				ELSE 191 
			END AS ScriptDataId, --191 for live 277 for test
			D.ACSKEY +
			',' + D.FIRSTNAME +
			',' + D.LASTNAME +
			',' + D.ADDRESS1 +
			',' + D.ADDRESS2 +
			',' + D.CITY +
			',' + D.[STATE] + 
			',' + D.ZIP +
			',' + D.COUNTRY +
			',' + D.ACCOUNT +
			',' + D.COST_CENTER_CODE +
			',' + Detailed.LetterData AS LetterData,
			D.COST_CENTER_CODE AS CostCenter,
			CASE WHEN D.DI_VLD_ADR ='Y' THEN 0 ELSE 1 END AS InValidAddress,
			0 AS DoNotProcessEcorr,
			D.OnEcorr AS OnEcorr,
			1 AS ArcNeeded,
			1 AS ImagingNeeded,
			SUSER_SNAME() AS AddedBy,
			GETDATE() AS AddedAt
		INTO
			#Output
		FROM
			#Demos D --Demographic information
			INNER JOIN
			( --Loan detail information in comma delimited format
				SELECT DISTINCT
					OB.BR_ID,
					OB.ACCOUNT,
					LetterData = STUFF
					(
						( 
							SELECT
								',' + [LOAN_PROGRAM] + ',' + [CURRENT_OWNER] + ',' + [CURRENT_PRINCIPAL] + ',' + [CURRENT_OUTSTANDING_INTEREST] + ',' + [PROJECTED_INTEREST_DURING_FORBEARANCE] + ',' + [PROJECTED_INTEREST_CAPITALIZATION_DATE]
							FROM    
								( 
									SELECT DISTINCT
										L.Loans,
										DENSE_RANK() OVER(PARTITION BY L.BR_ID ORDER BY COALESCE(LD.LN_SEQ,-1)) AS LN, --force null loans to have a low rank (below 0) and order by that to force them to one side of the set
										ISNULL(LD.[LOAN_PROGRAM], '') AS [LOAN_PROGRAM],
										ISNULL(LD.[CURRENT_OWNER], '') AS [CURRENT_OWNER],
										ISNULL(CAST(LD.[CURRENT_PRINCIPAL] AS VARCHAR(20)),'') AS [CURRENT_PRINCIPAL],
										ISNULL(CAST(LD.[CURRENT_OUTSTANDING_INTEREST] AS VARCHAR(20)),'') AS [CURRENT_OUTSTANDING_INTEREST],
										ISNULL(CAST(LD.[PROJECTED_INTEREST_DURING_FORBEARANCE] AS VARCHAR(20)),'') AS [PROJECTED_INTEREST_DURING_FORBEARANCE],
										ISNULL(CONVERT(VARCHAR(10), LD.[PROJECTED_INTEREST_CAPITALIZATION_DATE], 101),'') AS [PROJECTED_INTEREST_CAPITALIZATION_DATE]
									FROM 
										#LoanPrep L --inner borrower data set
										LEFT JOIN
										(
											SELECT 
												*
											FROM
												#LoanPrep
										) LD --loan data
											ON LD.BR_ID = L.BR_ID
											AND LD.LN_SEQ = L.loans
									WHERE
										OB.BR_ID = L.BR_ID --outer borrower = inner borrower
								) x
							ORDER BY
								CASE 
									WHEN LN = 1 
									THEN 20000 
									ELSE LN 
								END, --partition sets 1 for rank on null rows, so force those to the end
								BR_ID
							FOR XML PATH(''), TYPE
						).value('.','VARCHAR(MAX)'),
						1,1,''
					)
				FROM
					( 
						SELECT DISTINCT
							BR_ID,
							ACCOUNT,
							LN_SEQ,
							LOAN_PROGRAM,
							CURRENT_OWNER,
							CURRENT_PRINCIPAL,
							CURRENT_OUTSTANDING_INTEREST,
							PROJECTED_INTEREST_DURING_FORBEARANCE,
							PROJECTED_INTEREST_CAPITALIZATION_DATE,
							loans
						FROM 
							#LoanPrep OB --outer borrower data set
					) OB
			) Detailed
				ON Detailed.BR_ID = D.BR_ID
				AND Detailed.ACCOUNT = D.ACCOUNT
		;

		INSERT INTO ULS.[print].PrintProcessing 
		(
			AccountNumber, 
			EmailAddress, 
			ScriptDataId, 
			LetterData, 
			CostCenter, 
			InValidAddress, 
			DoNotProcessEcorr, 
			OnEcorr, 
			ArcNeeded, 
			ImagingNeeded, 
			AddedAt, 
			AddedBy
		)
		SELECT
			O.AccountNumber, 
			O.EmailAddress, 
			O.ScriptDataId, 
			O.LetterData, 
			O.CostCenter, 
			O.InValidAddress, 
			O.DoNotProcessEcorr, 
			O.OnEcorr, 
			O.ArcNeeded,  
			O.ImagingNeeded, 
			O.AddedAt, 
			O.AddedBy
		FROM	
			#Output O
			LEFT JOIN ULS.[print].PrintProcessing PP
				ON PP.AccountNumber = O.AccountNumber
				AND PP.EmailAddress = O.EmailAddress
				AND PP.ScriptDataId = O.ScriptDataId
				AND CAST(PP.AddedAt AS DATE) BETWEEN CAST(DATEADD(DAY,-4,O.AddedAt) AS DATE) AND CAST(O.AddedAt AS DATE) --Not added to print processing in the last 4 days
		WHERE
			PP.AccountNumber IS NULL --Not already added.
		;
	COMMIT TRANSACTION
END TRY
BEGIN CATCH
	DECLARE @EM VARCHAR(4000) = @ScriptId + ' encountered an error.  Transaction not committed. Error: ' + (SELECT ERROR_MESSAGE());

	ROLLBACK TRANSACTION;

	DECLARE @ProcessLogId INT;
	DECLARE @ProcessNotificationId INT;
	DECLARE @NotificationTypeId INT = (SELECT TOP 1 NotificationTypeId FROM ProcessLogs..NotificationTypes WHERE NotificationTypeDescription = 'Error Report'); --Error report
	DECLARE @NotificationSeverityTypeId INT = (SELECT TOP 1 NotificationSeverityTypeId FROM ProcessLogs..NotificationSeverityTypes WHERE NotificationSeverityTypeDescription = 'Critical'); --Critical
		
	INSERT INTO ProcessLogs..ProcessLogs (StartedOn, EndedOn, ScriptId, Region, RunBy) VALUES(@NOW,@NOW,@ScriptId,'uheaa',SUSER_SNAME());
	SET @ProcessLogId = SCOPE_IDENTITY()

	INSERT INTO ProcessLogs..ProcessNotifications (NotificationTypeId,NotificationSeverityTypeId,ProcessLogId, ResolvedAt, ResolvedBy) VALUES(@NotificationTypeId,@NotificationSeverityTypeId,@ProcessLogId, NULL, NULL)
	SET @ProcessNotificationId = SCOPE_IDENTITY()

	INSERT INTO ULS.[log].ProcessLogMessages (ProcessNotificationId, LogMessage) VALUES(@ProcessNotificationId,@EM);

	THROW;
END CATCH;