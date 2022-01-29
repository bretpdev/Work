SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED

DECLARE @EmailCampaignId INT = (SELECT EmailCampaignId FROM CLS.emailbtcf.EmailCampaigns WHERE LetterId = 'ALTRPY1FED.html');
DECLARE @TODAY DATE = GETDATE();
DECLARE @YEARAGO DATE = DATEADD(YEAR,-1,@TODAY);
DECLARE @BEGINMONTH DATE = DATEADD(DAY,1,EOMONTH(@TODAY,-2));--first day of previous month
DECLARE @ENDMONTH DATE = EOMONTH(@TODAY,-1)--last day of previous month

--get latest values from placeholder table
DECLARE @MAXTI INT = (SELECT MAX([Table Index])	FROM CLS..ALTREPEMA);
DECLARE @SEGMENT INT = (SELECT Segment FROM	CLS..ALTREPEMA WHERE [Table Index] = @MAXTI);
DECLARE @MAXNUMBER INT = (SELECT [Max Number] FROM CLS..ALTREPEMA WHERE [Table Index] = @MAXTI);
DECLARE @MINDELQ INT = (SELECT [Min Delq] FROM CLS..ALTREPEMA WHERE [Table Index] = @MAXTI);
DECLARE @MAXDELQ INT = (SELECT [Max Delq] FROM CLS..ALTREPEMA WHERE [Table Index] = @MAXTI);

--SELECT @MAXTI,@SEGMENT,@MAXNUMBER,@MINDELQ,@MAXDELQ

;WITH BORROWERS AS
(
	SELECT
		PD10.DF_SPE_ACC_ID,
		PD10.DM_PRS_1,
		PD10.DM_PRS_LST,
		FSA.BF_SSN,
		FSA.LN_SEQ,
		FSA.Segment
	FROM 
		CDW.FsaInvMet.Daily_LoanLevel FSA
		INNER JOIN CDW..LN10_LON LN10
			ON FSA.BF_SSN = LN10.BF_SSN
			AND FSA.LN_SEQ = LN10.LN_SEQ
		INNER JOIN CDW..PD10_PRS_NME PD10
			ON LN10.BF_SSN = PD10.DF_PRS_ID
		LEFT JOIN CDW..AY10_BR_LON_ATY AY10 --flag for exclusion: received ALTRP in prev month
			ON AY10.BF_SSN = LN10.BF_SSN
			AND AY10.PF_REQ_ACT = 'ALTRP'
			AND AY10.LD_ATY_REQ_RCV BETWEEN @BEGINMONTH AND @ENDMONTH
		LEFT JOIN CDW..DW01_DW_CLC_CLU DW01 --flag for exclusion: paid,death,disability,bankruptcy
			ON DW01.BF_SSN = LN10.BF_SSN
			AND DW01.WC_DW_LON_STA IN ('12','16','17','18','19','20','21')
		LEFT JOIN
		(--flag for exclusion: active collection suspension forbearance
			SELECT
				LN60.BF_SSN
			FROM
				CDW..FB10_BR_FOR_REQ FB10
				INNER JOIN CDW..LN60_BR_FOR_APV LN60
					ON LN60.BF_SSN = FB10.BF_SSN
					AND LN60.LF_FOR_CTL_NUM = FB10.LF_FOR_CTL_NUM
			WHERE
				FB10.LC_FOR_TYP = '28'
				AND FB10.LC_FOR_STA = 'A'
				AND FB10.LC_STA_FOR10 = 'A'
				AND LN60.LC_STA_LON60 = 'A'
				AND LN60.LC_FOR_RSP != '003'
				AND @TODAY BETWEEN LN60.LD_FOR_BEG AND LN60.LD_FOR_END
		) CSFORB
			ON CSFORB.BF_SSN = LN10.BF_SSN
	WHERE
		CSFORB.BF_SSN IS NULL --exclude active collection suspension forbearance
		AND AY10.BF_SSN IS NULL --excludes ALTRP
		AND DW01.BF_SSN IS NULL --excludes DW01 statuses
		AND LN10.LC_STA_LON10 = 'R'
		AND LN10.LA_CUR_PRI > 0.00
		AND FSA.LN_DLQ_MAX BETWEEN @MINDELQ AND @MAXDELQ --limits output to number specified by BU
		AND FSA.Segment = @SEGMENT --limits output to number specified by BU
),
ENDORSERS AS
(--endorsers of borrower pop above
	SELECT
		PD10.DF_SPE_ACC_ID
		,LN20.LF_EDS AS [BF_SSN]
		,PD10.DM_PRS_1
		,PD10.DM_PRS_MID
		,PD10.DM_PRS_LST
	FROM 
		BORROWERS BOR
		INNER JOIN CDW..LN20_EDS LN20
			ON BOR.BF_SSN = LN20.BF_SSN
		INNER JOIN CDW..PD10_PRS_NME PD10
			ON LN20.LF_EDS = PD10.DF_PRS_ID
	WHERE 
		LN20.LC_STA_LON20 = 'A'
		AND LN20.LC_EDS_TYP = 'M'
		AND LN20.LF_EDS NOT LIKE 'P%'
)
MERGE
	CLS.emailbtcf.CampaignData CD
USING
(--final output merge fields
	SELECT
		 EmailCampaignId
		,Recipient
		,AccountNumber
		,FirstName
		,LastName
		,AddedAt
		,AddedBy
	FROM
	(--prep and count output merge fields 
		SELECT DISTINCT
			@EmailCampaignId AS EmailCampaignId
			,COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM) AS Recipient
			,ALLPOP.DF_SPE_ACC_ID AS AccountNumber
			,ALLPOP.DM_PRS_1 AS FirstName
			,ALLPOP.DM_PRS_LST AS LastName
			,GETDATE() AS AddedAt
			,SUSER_SNAME() AS AddedBy
			,ROW_NUMBER() OVER(ORDER BY ALLPOP.DF_SPE_ACC_ID) AS ROWNUM
		FROM
		(--puts data sets together
			SELECT
				DF_SPE_ACC_ID,
				BF_SSN,
				DM_PRS_1,
				DM_PRS_LST
			FROM
				BORROWERS

			UNION

			SELECT
				DF_SPE_ACC_ID,
				BF_SSN,
				DM_PRS_1,
				DM_PRS_LST
			FROM
				ENDORSERS
		) ALLPOP
			LEFT JOIN CDW..PH05_CNC_EML PH05 
				ON PH05.DF_SPE_ID = ALLPOP.DF_SPE_ACC_ID
				AND	PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
				AND PH05.DI_CNC_ELT_OPI = 'Y' --on ecorr
			LEFT JOIN
			(--gets active valid email address
				SELECT 
					DF_PRS_ID, 
					Email.EM [ALT_EM],
 					ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) AS [EmailPriority] -- number in order of Email.PriorityNumber 
 				FROM 
 				(--flags valid active email addresses by type
 					SELECT 
 						PD32.DF_PRS_ID, 
						PD32.DX_ADR_EML [EM], 
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
				ON PD32.DF_PRS_ID = ALLPOP.BF_SSN
				AND PD32.EmailPriority = 1
		WHERE
			PH05.DX_CNC_EML_ADR IS NOT NULL
			OR PD32.ALT_EM IS NOT NULL
	) COUNTPOP
	WHERE
		ROWNUM <= @MAXNUMBER --limits output to number specified by BU
) NEWDATA
		ON  NEWDATA.EmailCampaignId = CD.EmailCampaignId
		AND NEWDATA.Recipient = CD.Recipient
		AND NEWDATA.AccountNumber = CD.AccountNumber
		AND NEWDATA.FirstName = CD.FirstName
		AND NEWDATA.LastName = CD.LastName
		AND CAST(NEWDATA.AddedAt AS DATE) = CAST(CD.AddedAt AS DATE) --allows multiple runs per day w/o duplicating data
		AND NEWDATA.AddedBy = CD.AddedBy
WHEN NOT MATCHED THEN
	INSERT
	(
		 EmailCampaignId
		,Recipient
		,AccountNumber
		,FirstName
		,LastName
		,AddedAt
		,AddedBy
	)
	VALUES
	(
		 NEWDATA.EmailCampaignId
		,NEWDATA.Recipient
		,NEWDATA.AccountNumber
		,NEWDATA.FirstName
		,NEWDATA.LastName
		,NEWDATA.AddedAt
		,NEWDATA.AddedBy
	)
;
