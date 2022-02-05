--BILENOTFD_ACH - •	A bill was sent out the previous day and borrower IS on auto pay BUT DOES NOT have a reduced payment forbearance (RPF)

DECLARE @SCRIPT_ID VARCHAR(12) = 'BILENOTFD';
DECLARE @LetterId VARCHAR(20) = 'EACHBILFED.html'; --Letter Id For Email Campaigns Table for non RPF borrowers
DECLARE @EmailCampaignId INT = (SELECT EmailCampaignId FROM CLS.emailbtcf.EmailCampaigns WHERE letterId = @LetterId);  --email campaign ID for non RPF borrowers
DECLARE @BEGINDATE DATE = CAST(DATEADD(DAY,-5,GETDATE()) AS DATE)


		SELECT DISTINCT
			BILL.Letter,
			@EmailCampaignId AS EmailCampaignId, --select the correct campaign ID depending on the borrower's RPF status
			COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) AS Recipient, --select the PH05 email address if it exists otherwise select the highest priority PD32 email address
			PD10.DF_SPE_ACC_ID  AS AccountNumber,
			PD10.DF_PRS_ID,
			SUBSTRING(PD10.DM_PRS_1,1,1) + RTRIM(LOWER(SUBSTRING(PD10.DM_PRS_1,2,13))) AS FirstName,
			'' AS LastName,
			--LineData
			DATENAME(MONTH,BILL.LD_BIL_DU_LON) + ',' + --Current_Month,
			'"$' + CONVERT(VARCHAR(12),CAST(BILL.AMT_DUE AS money),1) + '",' + --Monthly Payment
			'"$' + CONVERT(VARCHAR(12),CAST(MAX(COALESCE(BILL.BA_EFT_ADD_WDR,0.00)) OVER (PARTITION BY BILL.BF_SSN, BILL.LD_BIL_CRT) AS money),1) + '",' + --Additional ACH amount
			'"$' + CONVERT(VARCHAR(12),CAST(SUM(COALESCE(BILL.LA_BIL_PAS_DU,0.00)) OVER (PARTITION BY BILL.BF_SSN, BILL.LD_BIL_CRT) AS money),1) + '",' + --Past Due Amount
			'"$' + COALESCE(CAST(BILL.TOTAL_AMT_DUE AS VARCHAR(100)),'0.00') + '",' + --Total_Amount_Due
			'"$' + CONVERT(VARCHAR(12),CAST(BILL.BALANCE AS money),1) + '",' + --Current_Balance
			RTRIM(CONVERT(VARCHAR(10),	BILL.LD_BIL_DU_LON	,101)) + ',' + -- ACH draw date
			RTRIM(CONVERT(VARCHAR(10),BILL.LD_BIL_CRT,101)) -- Date of Billing 
			AS LineData,
			GETDATE() AS AddedAt,
			SUSER_SNAME() AS AddedBy,
			0 AS IsCoborrowerRecord,
			NULL AS ProcessedAt,
			NULL AS DeletedAt,
			NULL AS DeletedBy
		FROM
			CDW..PD10_PRS_NME PD10
			INNER JOIN 
			(
				SELECT
				 P.*,
				 SUM(ISNULL(P.LA_BIL_CUR_DU,0)) OVER (PARTITION BY P.BF_SSN, P.LD_BIL_CRT) AS AMT_DUE,
				 SUM(P.LA_BIL_DU_PRT + ISNULL(LA_BIL_PAS_DU,0) ) OVER (PARTITION BY P.BF_SSN, P.LD_BIL_CRT) AS TOTAL_AMT_DUE,
				 SUM(P.LA_CUR_PRI) OVER (PARTITION BY P.BF_SSN, P.LD_BIL_CRT) AS BALANCE
				FROM
				(
					SELECT DISTINCT
						LN80.BF_SSN,
						LN80.LN_SEQ,
						LN80.LD_BIL_CRT,
						BL10.LC_STA_BIL10,
						LN80.LC_STA_LON80,
						LN80.LA_BIL_PAS_DU,
						BR30.BN_EFT_DAY_DUE, --2-digit draw day
						BR30.BA_EFT_ADD_WDR,  --Additional ACH amount
						LN80.LA_BIL_CUR_DU,
						LN80.LA_BIL_DU_PRT ,
						LN10.LA_CUR_PRI,
						LN80.LD_BIL_DU_LON,
						BL10.LC_IND_BIL_SNT,
						L.Letter   
					FROM
						CDW..BL10_BR_BIL BL10
						INNER JOIN CDW..PD10_PRS_NME PD10
							ON PD10.DF_PRS_ID = BL10.BF_SSN
						INNER JOIN CDW..LN80_LON_BIL_CRF LN80
							ON BL10.BF_SSN = LN80.BF_SSN
							AND BL10.LD_BIL_CRT = LN80.LD_BIL_CRT
							AND BL10.LN_SEQ_BIL_WI_DTE = LN80.LN_SEQ_BIL_WI_DTE
							AND BL10.BN_EFT_SEQ IS NOT NULL
						INNER JOIN CDW..LN10_LON LN10
							ON LN10.BF_SSN = LN80.BF_SSN
							AND LN10.LN_SEQ = LN80.LN_SEQ
						INNER JOIN ECorrFed..DocumentDetails DD
							ON DD.ADDR_ACCT_NUM = PD10.DF_SPE_ACC_ID
							AND DD.CreateDate = BL10.LD_BIL_CRT 
						INNER JOIN ECorrFed..Letters L 
							ON L.LetterId = DD.LetterId
							AND L.Letter IN ('BILSTFED')
						LEFT JOIN
						( --BR30
							SELECT
								BR30.BF_SSN,
								LN83.LN_SEQ,
								BR30.BN_EFT_DAY_DUE, --2-digit draw day
								BR30.BA_EFT_ADD_WDR  --Additional ACH amount
							FROM
								CDW..BR30_BR_EFT BR30
								INNER JOIN CDW..LN83_EFT_TO_LON LN83
									ON LN83.BF_SSN = BR30.BF_SSN 
									AND LN83.BN_EFT_SEQ = BR30.BN_EFT_SEQ
							WHERE
								BR30.BC_EFT_STA = 'A'
								AND LN83.LD_EFT_EFF_END IS NULL 
						) BR30
							ON BR30.BF_SSN = LN80.BF_SSN
							AND BR30.LN_SEQ = LN80.LN_SEQ
					WHERE
						BL10.LC_STA_BIL10 = 'A'
						AND 
							(
								BL10.LC_IND_BIL_SNT IN ('G', 'I','H')
								OR
								(BL10.LC_IND_BIL_SNT IN ('1','2','4','7','F','R') AND BR30.BF_SSN IS NOT NULL)
							)
						AND LN80.LC_STA_LON80 = 'A'
						AND BL10.LC_BIL_TYP = 'P' 
						AND LN10.LA_CUR_PRI > 0.00
						AND LN10.LC_STA_LON10 = 'R'
						
						AND CAST(DD.CreateDate AS DATE) BETWEEN @BEGINDATE  AND CAST(GETDATE() AS DATE)
				) P
			) BILL
				ON BILL.BF_SSN = PD10.DF_PRS_ID
			INNER JOIN CDW..LN10_LON LN10
				ON LN10.BF_SSN = BILL.BF_SSN
				AND LN10.LN_SEQ = BILL.LN_SEQ
		 -- valid PD32 email address
			LEFT JOIN
			(
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
			LEFT JOIN CDW..PH05_CNC_EML PH05
				ON PD10.DF_SPE_ACC_ID = PH05.DF_SPE_ID
				AND PH05.DI_VLD_CNC_EML_ADR = 'Y'
		 -- active type 31 forbearance indicating a RPF borrower

		WHERE
			LN10.LA_CUR_PRI > 0 --•	has a balance
			AND LN10.LC_STA_LON10 = 'R' --•	loans have been released
			AND COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) IS NOT NULL  --• has a valid email address 
			AND BILL.TOTAL_AMT_DUE IS NOT NULL
	