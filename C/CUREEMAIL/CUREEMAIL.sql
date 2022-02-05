USE UDW
GO

DECLARE @TODAY DATE = GETDATE();
DECLARE @TODAY_FORMATTED VARCHAR(8) = FORMAT(@TODAY, 'yyyyMMdd', 'en-US');
DECLARE @14DAYSAGO DATE = DATEADD(DAY,-14,GETDATE());

DECLARE @EMAILCAMPAIGN_ID1 VARCHAR(3) = (SELECT EmailCampaignId FROM [ULS].[emailbatch].[EmailCampaigns] WHERE SourceFile = 'CureEmail.1*');
DECLARE @EMAILCAMPAIGN_ID2 VARCHAR(3) = (SELECT EmailCampaignId FROM [ULS].[emailbatch].[EmailCampaigns] WHERE SourceFile = 'CureEmail.2*');
DECLARE @EMAILCAMPAIGN_ID3 VARCHAR(3) = (SELECT EmailCampaignId FROM [ULS].[emailbatch].[EmailCampaigns] WHERE SourceFile = 'CureEmail.3*');
DECLARE @EMAILCAMPAIGN_ID4 VARCHAR(3) = (SELECT EmailCampaignId FROM [ULS].[emailbatch].[EmailCampaigns] WHERE SourceFile = 'CureEmail.4*');
DECLARE @EMAILCAMPAIGN_ID5 VARCHAR(3) = (SELECT EmailCampaignId FROM [ULS].[emailbatch].[EmailCampaigns] WHERE SourceFile = 'CureEmail.5*');

DECLARE @EMAILCAMPAIGN_FILE1 VARCHAR(12) = (SELECT SourceFile FROM [ULS].[emailbatch].[EmailCampaigns] WHERE SourceFile = 'CureEmail.1*');
DECLARE @EMAILCAMPAIGN_FILE2 VARCHAR(12) = (SELECT SourceFile FROM [ULS].[emailbatch].[EmailCampaigns] WHERE SourceFile = 'CureEmail.2*');
DECLARE @EMAILCAMPAIGN_FILE3 VARCHAR(12) = (SELECT SourceFile FROM [ULS].[emailbatch].[EmailCampaigns] WHERE SourceFile = 'CureEmail.3*');
DECLARE @EMAILCAMPAIGN_FILE4 VARCHAR(12) = (SELECT SourceFile FROM [ULS].[emailbatch].[EmailCampaigns] WHERE SourceFile = 'CureEmail.4*');
DECLARE @EMAILCAMPAIGN_FILE5 VARCHAR(12) = (SELECT SourceFile FROM [ULS].[emailbatch].[EmailCampaigns] WHERE SourceFile = 'CureEmail.5*');

;WITH BASE_POP AS
(
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID
		,(PD10.DF_SPE_ACC_ID + ', ' + LTRIM(RTRIM(PD10.DM_PRS_1)) + ' ' + LTRIM(RTRIM(PD10.DM_PRS_LST)) + ', ' + LTRIM(RTRIM(PD32.DX_ADR_EML))) AS EmailData
		,LN10.BF_SSN	
	FROM
		LN10_LON LN10
		INNER JOIN DW01_DW_CLC_CLU DW01 --calculated/derived columns table
			ON LN10.BF_SSN = DW01.BF_SSN
			AND LN10.LN_SEQ = DW01.LN_SEQ
		INNER JOIN PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		INNER JOIN LN33_LON_CU_INF LN33
			ON LN10.BF_SSN = LN33.BF_SSN
			AND LN10.LN_SEQ = LN33.LN_SEQ
		INNER JOIN 
		(--get max ln_cu_seq if more than 1 on a loan
			SELECT
				BF_SSN
				,LN_SEQ
				,MAX(LN_CU_SEQ) AS MAX_LN_CU_SEQ
			FROM
				LN33_LON_CU_INF
			GROUP BY
				BF_SSN
				,LN_SEQ
		) LN33_MAX
			ON LN33.BF_SSN = LN33_MAX.BF_SSN
			AND LN33.LN_SEQ = LN33_MAX.LN_SEQ
			AND LN33.LN_CU_SEQ = LN33_MAX.MAX_LN_CU_SEQ
		INNER JOIN 
		( -- email address
			SELECT
				*,
				ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) [EmailPriority] -- number in order of Email.PriorityNumber
			FROM
			(
				SELECT
					PD32.DF_PRS_ID,
					PD32.DX_ADR_EML,
					CASE 
						WHEN DC_ADR_EML = 'H' THEN 1 -- home
						WHEN DC_ADR_EML = 'A' THEN 2 -- alternate
						WHEN DC_ADR_EML = 'W' THEN 3 -- work
					END [PriorityNumber]
				FROM
					PD32_PRS_ADR_EML PD32
				WHERE
					PD32.DI_VLD_ADR_EML = 'Y' -- valid email address
					AND PD32.DC_STA_PD32 = 'A' -- active email address record
			) Email
		) PD32 
			ON PD32.DF_PRS_ID = PD10.DF_PRS_ID
			--AND PD32.EmailPriority = 1 --highest priority email only --commented out because want to sent to all addresses
	WHERE
		LN10.LA_CUR_PRI > 0.00
		AND LN10.LC_STA_LON10 = 'R'
		AND DW01.WC_DW_LON_STA = '06'
		AND LN33.LD_CU_3_YR_RUL > @TODAY --within 3 years of cure status
)
,AY10_MAX AS
(--get most recent cure status
	SELECT DISTINCT
		AY10.BF_SSN
		,AY10.PF_REQ_ACT
		,AY10M.MAX_LD_ATY_REQ_RCV
	FROM
		AY10_BR_LON_ATY AY10
		INNER JOIN
		(
			SELECT
				BF_SSN
				,MAX(LD_ATY_REQ_RCV) AS MAX_LD_ATY_REQ_RCV
			FROM
				AY10_BR_LON_ATY
			WHERE
				LC_STA_ACTY10 = 'A'
				AND PF_REQ_ACT IN ('CURE1','CURE2','CURE3','CURE4','CURE5')
			GROUP BY
				BF_SSN
		) AY10M
			ON AY10.BF_SSN = AY10M.BF_SSN
			AND AY10.LD_ATY_REQ_RCV = AY10M.MAX_LD_ATY_REQ_RCV
)
,RECEIVEDEMAILS AS
(
	--R1: the FIFTH email is the last email the borrower has received and it’s been at least two weeks since the borrower received that email
	SELECT 
		 BP.BF_SSN
		,@EMAILCAMPAIGN_ID1 AS EmailCampaignId
		,BP.DF_SPE_ACC_ID
		,BP.EmailData
		,@EMAILCAMPAIGN_FILE1 AS Report
	FROM
		BASE_POP BP
		INNER JOIN AY10_MAX
			ON AY10_MAX.BF_SSN = BP.BF_SSN
	WHERE
		AY10_MAX.PF_REQ_ACT = 'CURE5'
		AND AY10_MAX.MAX_LD_ATY_REQ_RCV <= @14DAYSAGO

	UNION

	--R2: The FIRST email is the last email the borrower has received and it’s been at least two weeks since the borrower received that email
	SELECT 
		 BP.BF_SSN
		,@EMAILCAMPAIGN_ID2 AS EmailCampaignId
		,BP.DF_SPE_ACC_ID
		,BP.EmailData
		,@EMAILCAMPAIGN_FILE2 AS Report
	FROM
		BASE_POP BP
		INNER JOIN AY10_MAX
			ON AY10_MAX.BF_SSN = BP.BF_SSN
	WHERE
		AY10_MAX.PF_REQ_ACT = 'CURE1'
		AND AY10_MAX.MAX_LD_ATY_REQ_RCV <= @14DAYSAGO

	UNION

	--R3: The SECOND email is the last email the borrower has received and it’s been at least two weeks since the borrower received that email
	SELECT 
		 BP.BF_SSN
		,@EMAILCAMPAIGN_ID3 AS EmailCampaignId
		,BP.DF_SPE_ACC_ID
		,BP.EmailData
		,@EMAILCAMPAIGN_FILE3 AS Report
	FROM
		BASE_POP BP
		INNER JOIN AY10_MAX
			ON AY10_MAX.BF_SSN = BP.BF_SSN
	WHERE
		AY10_MAX.PF_REQ_ACT = 'CURE2'
		AND AY10_MAX.MAX_LD_ATY_REQ_RCV <= @14DAYSAGO

	UNION

	--R4: The THIRD email is the last email the borrower has received and it’s been at least two weeks since the borrower received that email
	SELECT 
		 BP.BF_SSN
		,@EMAILCAMPAIGN_ID4 AS EmailCampaignId
		,BP.DF_SPE_ACC_ID
		,BP.EmailData
		,@EMAILCAMPAIGN_FILE4 AS Report
	FROM
		BASE_POP BP
		INNER JOIN AY10_MAX
			ON AY10_MAX.BF_SSN = BP.BF_SSN
	WHERE
		AY10_MAX.PF_REQ_ACT = 'CURE3'
		AND AY10_MAX.MAX_LD_ATY_REQ_RCV <= @14DAYSAGO

	UNION

	--R5: The FOURTH email is the last email the borrower has received and it’s been at least two weeks since the borrower received that email
	SELECT 
		 BP.BF_SSN
		,@EMAILCAMPAIGN_ID5 AS EmailCampaignId
		,BP.DF_SPE_ACC_ID
		,BP.EmailData
		,@EMAILCAMPAIGN_FILE5 AS Report
	FROM
		BASE_POP BP
		INNER JOIN AY10_MAX
			ON AY10_MAX.BF_SSN = BP.BF_SSN
	WHERE
		AY10_MAX.PF_REQ_ACT = 'CURE4'
		AND AY10_MAX.MAX_LD_ATY_REQ_RCV <= @14DAYSAGO
)
MERGE
	ULS.emailbatch.EmailProcessing EP
USING
	(
		SELECT DISTINCT
			 ALLPOP.EmailCampaignId
			,ALLPOP.DF_SPE_ACC_ID AS AccountNumber
			,Report + '.' + @TODAY_FORMATTED AS ActualFile
			,ALLPOP.EmailData
			,NULL AS EmailSentAt
			,1 AS ArcNeeded
			,NULL AS ArcAddProcessingId
			,0 AS ProcessingAttempts
			,SYSTEM_USER AS AddedBy
			,GETDATE() AS AddedAt
			,NULL AS DeletedBy
			,NULL AS DeletedAt
		FROM
			(
				--R1: borrower has never received a cure email before (Borrower doesn’t have an active cure arc)
				SELECT
					 BP.BF_SSN
					,@EMAILCAMPAIGN_ID1 AS EmailCampaignId
					,BP.DF_SPE_ACC_ID
					,BP.EmailData		
					,@EMAILCAMPAIGN_FILE1 AS Report
				FROM
					BASE_POP BP
					LEFT JOIN 
					(--flag for exclusion: active cure arcs
						SELECT
							BF_SSN
						FROM
							AY10_BR_LON_ATY
						WHERE
							PF_REQ_ACT IN ('CURE1','CURE2','CURE3','CURE4','CURE5')
							AND LC_STA_ACTY10 = 'A'
					) AY10_CURE
						ON BP.BF_SSN = AY10_CURE.BF_SSN
					LEFT JOIN RECEIVEDEMAILS RE
						ON BP.BF_SSN = RE.BF_SSN
				WHERE
					AY10_CURE.BF_SSN IS NULL --exclude active cure arcs
					AND RE.BF_SSN IS NULL --remove those who have received email

				UNION

				SELECT
					 BF_SSN
					,EmailCampaignId
					,DF_SPE_ACC_ID
					,EmailData
					,Report
				FROM
					RECEIVEDEMAILS
			)ALLPOP
	) NEWDATA
		ON NEWDATA.EmailCampaignId = EP.EmailCampaignId
		AND NEWDATA.AccountNumber = EP.AccountNumber
		AND NEWDATA.ActualFile = EP.ActualFile
		AND NEWDATA.EmailData = EP.EmailData
		AND NEWDATA.ArcNeeded = EP.ArcNeeded
		AND NEWDATA.ProcessingAttempts = EP.ProcessingAttempts
WHEN NOT MATCHED THEN
	INSERT
	(
		 EmailCampaignId
		,AccountNumber
		,ActualFile
		,EmailData
		,EmailSentAt
		,ArcNeeded
		,ArcAddProcessingId
		,ProcessingAttempts
		,AddedBy
		,AddedAt
		,DeletedBy
		,DeletedAt
	)
	VALUES
	(
		 NEWDATA.EmailCampaignId
		,NEWDATA.AccountNumber
		,NEWDATA.ActualFile
		,NEWDATA.EmailData
		,NEWDATA.EmailSentAt
		,NEWDATA.ArcNeeded
		,NEWDATA.ArcAddProcessingId
		,NEWDATA.ProcessingAttempts
		,NEWDATA.AddedBy
		,NEWDATA.AddedAt
		,NEWDATA.DeletedBy
		,NEWDATA.DeletedAt
	)
;

--FOR TESTING
--SELECT * FROM ULS.emailbatch.EmailProcessing WHERE ActualFile LIKE '%CureEmail%';