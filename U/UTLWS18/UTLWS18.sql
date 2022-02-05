--UTLWS18.sql
DECLARE @NOW DATETIME = GETDATE();
DECLARE @TODAY DATE = CONVERT(DATE,@NOW);
DECLARE @ArcTypeId TINYINT = 2;
DECLARE @ARC VARCHAR(5) = 'CSV30';
DECLARE @ScriptId VARCHAR(7) = 'UTLWS18';
DECLARE @Comment VARCHAR(255) = 'Borrower has not had a phone attempt in 30 days';
DECLARE @IsReference TINYINT = 0;
DECLARE @IsEndorser TINYINT = 0;
DECLARE @ProcessingAttempts TINYINT = 0;

INSERT INTO ULS..ArcAddProcessing
(
	ArcTypeId
	,AccountNumber
	,ARC
	,ScriptId
	,ProcessOn
	,Comment
	,IsReference
	,IsEndorser
	,RegardsTo
	,RegardsCode
	,ProcessingAttempts
	,CreatedAt
	,CreatedBy
)
SELECT DISTINCT
	@ArcTypeId 
	,PD10.DF_SPE_ACC_ID AS AccountNumber
	,@ARC
	,@ScriptId
	,@NOW AS ProcessOn
	,@Comment
	,@IsReference
	,@IsEndorser
	,'' AS RegardsTo
	,'' AS RegardsCode
	,@ProcessingAttempts
	,@NOW AS CreatedAt
	,SYSTEM_USER AS CreatedBy
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..PD42_PRS_PHN PD42
		ON PD10.DF_PRS_ID = PD42.DF_PRS_ID
	INNER JOIN UDW..LN10_LON LN10
		ON PD10.DF_PRS_ID = LN10.BF_SSN
	INNER JOIN UDW..LN16_LON_DLQ_HST LN16
		ON LN10.BF_SSN = LN16.BF_SSN
		AND LN10.LN_SEQ = LN16.LN_SEQ
	INNER JOIN UDW..DW01_DW_CLC_CLU DW01
		ON LN10.BF_SSN = DW01.BF_SSN
		AND LN10.LN_SEQ = DW01.LN_SEQ
	LEFT JOIN
	(--flag for exclusion: borr hasn't had delinquency due diligence phone call in past 30 days
		SELECT
			AY10.BF_SSN
		FROM
			UDW..AY10_BR_LON_ATY AY10
			INNER JOIN UDW..AC10_ACT_REQ AC10
				ON AY10.PF_REQ_ACT = AC10.PF_REQ_ACT
		WHERE
			AY10.LD_ATY_RSP > DATEADD(DAY,-30,@TODAY)
			AND AY10.PF_RSP_ACT IN ('COMPL','CNTCT','NOCTC','INVPH','NOPHN')
			AND AY10.LC_STA_ACTY10 = 'A'
			AND AC10.PC_DD_ACT_STA = 'D' --delinquent
			AND AC10.PC_CCI_CLM_COL_ATY IN ('ZZ','Z1') --due diligence; borrower call
			AND AC10.PC_STA_ACT10 = 'A'
	) AY10AC10
		ON LN10.BF_SSN = AY10AC10.BF_SSN
	LEFT JOIN
	(--flag for exclusion: borr doesn't already have open 3C queue task
		SELECT
			BF_SSN
		FROM
			UDW..WQ20_TSK_QUE
		WHERE
			WF_QUE = '3C'
			AND WC_STA_WQUE20 NOT IN ('C','X')
	) Task3CQ
		ON LN10.BF_SSN = Task3CQ.BF_SSN
	LEFT JOIN ULS..ArcAddProcessing EXISTING_DATA
		ON EXISTING_DATA.AccountNumber = PD10.DF_SPE_ACC_ID
		AND EXISTING_DATA.CreatedAt >= @TODAY  --wasn't already added today
		AND EXISTING_DATA.CreatedAt < DATEADD(DAY,1,@TODAY) --wasn't already added today
		AND EXISTING_DATA.ArcTypeId = @ArcTypeId
		AND EXISTING_DATA.ARC = @ARC
		AND EXISTING_DATA.ScriptId = @ScriptId
		AND EXISTING_DATA.Comment = @Comment
WHERE
	AY10AC10.BF_SSN IS NULL --removes borr who haven't had delinquency due diligence phone call in past 30 days
	AND Task3CQ.BF_SSN IS NULL --removes borr who don't already have open 3C queue task
	AND EXISTING_DATA.AccountNumber IS NULL --wasn't already added today
	AND LN10.LC_STA_LON10 = 'R'
	AND LN10.LA_CUR_PRI > 0.00
	AND LN10.LI_CON_PAY_STP_PUR != 'Y' --stop pursuit
	AND DW01.WC_DW_LON_STA NOT IN ('06','16','17','18','19','20','21','08','09','10','11','12')--06:cure, 16-21:Alleged/Verified DDB, 8-12:claims
	AND LN16.LN_DLQ_MAX >= 30
	AND LN16.LN_DLQ_MAX < 360
	AND LN16.LC_STA_LON16 = '1'
	AND PD42.DI_PHN_VLD = 'Y'
;

--FOR TESTING
--select * from ULS..ArcAddProcessing where ScriptId = 'UTLWS18'
