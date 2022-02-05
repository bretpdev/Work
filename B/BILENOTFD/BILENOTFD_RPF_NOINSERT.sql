--BILENOTFD_NOACH - •	A bill was sent out the previous day and borrower is NOT on auto pay and may or may not have a  reduced payment forbearance (RPF)

DECLARE @SCRIPT_ID VARCHAR(12) = 'BILENOTFD';
DECLARE @LetterId VARCHAR(20) = 'EBILLSTFED.html'; --Letter Id For Email Campaigns Table
DECLARE @EmailCampaignId INT = (SELECT EmailCampaignId FROM CLS.emailbtcf.EmailCampaigns WHERE letterId = @LetterId);
DECLARE @BEGINDATE DATE = CAST(DATEADD(DAY,-5,GETDATE()) AS DATE)


		SELECT DISTINCT
			BILL.Letter,
			@EmailCampaignId AS EmailCampaignId,
			COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) AS Recipient, --select the PH05 email address if it exists otherwise select the highest priority PD32 email address
			PD10.DF_SPE_ACC_ID  AS AccountNumber,
			SUBSTRING(PD10.DM_PRS_1,1,1) + RTRIM(LOWER(SUBSTRING(PD10.DM_PRS_1,2,13))) AS FirstName,
			'' AS LastName,
			--LineData
			DATENAME(MONTH, BILL.LD_BIL_DU_LON) + ',' + --Current_Month,
			'"$' + CONVERT(VARCHAR(12),CAST(BILL.AMT_DUE AS money),1) + '",' + --Monthly Payment
			'"$0.00",' + --Past Due Amount
			'"$' + CONVERT(VARCHAR(12),CAST(BILL.AMT_DUE AS money),1) + '",' + --Total_Amount_Due
			'"$' + CONVERT(VARCHAR(12),CAST(BILL.BALANCE AS money),1) + '",' + --Current_Balance
			RTRIM(CONVERT(VARCHAR(10),BILL.LD_BIL_DU_LON,101)) -- Due_Date 
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
						LN80.LA_BIL_CUR_DU,
						LN80.LA_BIL_DU_PRT,
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
						INNER JOIN ECorrFed..DocumentDetails DD
							ON DD.ADDR_ACCT_NUM = PD10.DF_SPE_ACC_ID
							AND DD.CreateDate = BL10.LD_BIL_CRT 
						INNER JOIN ECorrFed..Letters L 
							ON L.LetterId = DD.LetterId
							AND L.Letter IN ('BILSTFED')
						INNER JOIN CDW..LN10_LON LN10
							ON LN10.BF_SSN = LN80.BF_SSN
							AND LN10.LN_SEQ = LN80.LN_SEQ
					WHERE
						BL10.LC_STA_BIL10 = 'A'
						AND LN80.LC_STA_LON80 = 'A'
						AND CAST(DD.CreateDate AS DATE) BETWEEN @BEGINDATE AND CAST(GETDATE() AS DATE)
						AND LN10.LA_CUR_PRI > 0.00
						AND LN10.LC_STA_LON10 = 'R'
						AND BL10.LC_BIL_TYP = 'C'
						AND BL10.LC_IND_BIL_SNT  = 'T'

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
