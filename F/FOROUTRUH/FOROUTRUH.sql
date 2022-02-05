SET XACT_ABORT ON --Rollback if there is an error on the transaction
GO
BEGIN TRANSACTION
DECLARE @ERROR INT = 0;
DECLARE @Arc CHAR(5) = 'DELTR';
DECLARE @ScriptId VARCHAR(10) = 'FOROUTRUH';
DECLARE @Today DATE = GETDATE();
DECLARE @Now DATETIME = GETDATE();
DECLARE @Comment VARCHAR(50) = 'High delinquency letter sent to borrower.';
DECLARE @CommentCo VARCHAR(50) = 'High delinquency letter sent to coborrower.'
DECLARE @ArcTracking TABLE(ArcAddProcessingId INT, AccountNumber VARCHAR(10));
DECLARE @ArcTrackingCo TABLE(ArcAddProcessingId INT, AccountNumber VARCHAR(10), CoAccountNumber VARCHAR(10));
DECLARE @ScriptDataId INT = (SELECT ScriptDataId FROM ULS.[print].ScriptData WHERE ScriptID = 'FOROUTRUH');
DECLARE @InitialPop TABLE(AccountNumber VARCHAR(10), BF_SSN VARCHAR(9), LN_SEQ INT, AscKey VARCHAR(20), FullName VARCHAR(80), Address1 VARCHAR(30), Address2 VARCHAR(30), City VARCHAR(20), [State] VARCHAR(2), Zip VARCHAR(17), Country VARCHAR(25));
DECLARE @InitialPopCo TABLE(CoAccountNumber VARCHAR(10), AccountNumber VARCHAR(10), BF_SSN VARCHAR(9), LN_SEQ INT, AscKey VARCHAR(20), FullName VARCHAR(80), Address1 VARCHAR(30), Address2 VARCHAR(30), City VARCHAR(20), [State] VARCHAR(2), Zip VARCHAR(17), Country VARCHAR(25));

INSERT INTO 
	@InitialPop
SELECT
	PD10.DF_SPE_ACC_ID,
	LN10.BF_SSN,
	LN10.LN_SEQ,
	CentralData.dbo.CreateACSKeyline(LN10.BF_SSN, 'B', 'L') AS AscKey,
	RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST) AS FullName,
	RTRIM(PD30.DX_STR_ADR_1) AS Address1,
	RTRIM(PD30.DX_STR_ADR_2) AS Address2,
	RTRIM(PD30.DM_CT) AS City,
	PD30.DC_DOM_ST AS [State],
	RTRIM(PD30.DF_ZIP_CDE) AS Zip,
	RTRIM(PD30.DM_FGN_CNY) AS Country
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..PD30_PRS_ADR PD30
		ON PD30.DF_PRS_ID = PD10.DF_PRS_ID
		AND PD30.DC_ADR = 'L'
		AND PD30.DI_VLD_ADR = 'Y'
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
		AND LN10.LA_CUR_PRI > 0.00
		AND LN10.LC_STA_LON10 = 'R'
		AND LN10.IC_LON_PGM != 'TILP'
	INNER JOIN UDW..DW01_DW_CLC_CLU DW01
		ON DW01.BF_SSN = LN10.BF_SSN
		AND DW01.LN_SEQ = LN10.LN_SEQ
		AND DW01.WC_DW_LON_STA NOT IN ('06','12','16','17','18','19','20','21') -- Not Cure, Claim, Death, Disability, or Bky
		AND DW01.WX_OVR_DW_LON_STA NOT LIKE '%CNSLD-STOP PURSUIT%'
	INNER JOIN UDW..LN16_LON_DLQ_HST LN16
		ON LN16.BF_SSN = LN10.BF_SSN
		AND LN16.LN_SEQ = LN10.LN_SEQ
		AND LN16.LC_STA_LON16 = '1' -- Active Delq
		AND LN16.LN_DLQ_MAX + 1 >= 210
		AND CAST(LN16.LD_DLQ_MAX AS DATE) BETWEEN CAST(DATEADD(DAY,-5,@Now) AS DATE) AND @Today
	INNER JOIN
	(
		SELECT
			LN10.BF_SSN,
			SUM(LN10.LA_CUR_PRI) + SUM(DW01.WA_TOT_BRI_OTS) AS Balance
		FROM
			UDW..LN10_LON LN10
			INNER JOIN UDW..DW01_DW_CLC_CLU DW01
				ON DW01.BF_SSN = LN10.BF_SSN
				AND DW01.LN_SEQ = LN10.LN_SEQ
				AND DW01.WC_DW_LON_STA NOT IN ('06','12','16','17','18','19','20','21') -- Not Cure, Claim, Death, Disability, or Bky
				AND DW01.WX_OVR_DW_LON_STA NOT LIKE '%CNSLD-STOP PURSUIT%'
		WHERE
			LN10.LC_STA_LON10 = 'R'
			AND LN10.IC_LON_PGM != 'TILP'
			AND LN10.LA_CUR_PRI > 0.00
		GROUP BY
			LN10.BF_SSN
	) Balance
		ON Balance.BF_SSN = LN10.BF_SSN
		AND Balance.Balance >= 25.00
	LEFT JOIN ULS.[print].PrintProcessing ExistingData
		ON ExistingData.AccountNumber = PD10.DF_SPE_ACC_ID
		AND ExistingData.EmailAddress = 'Ecorr@UHEAA.org'
		AND ExistingData.ScriptDataId = @ScriptDataId
		AND ExistingData.LetterData = 
			CONCAT(--data to be inserted into letter
					CentralData.dbo.CreateACSKeyline(LN10.BF_SSN, 'B', 'L')
					,',',PD10.DF_SPE_ACC_ID
					,',',RTRIM(PD10.DM_PRS_1) + ' ' + RTRIM(PD10.DM_PRS_MID) + ' ' + RTRIM(PD10.DM_PRS_LST)
					,',',RTRIM(PD30.DX_STR_ADR_1)
					,',',RTRIM(PD30.DX_STR_ADR_2)
					,',',RTRIM(PD30.DM_CT)
					,',',PD30.DC_DOM_ST
					,',',RTRIM(PD30.DF_ZIP_CDE)
					,',',RTRIM(PD30.DM_FGN_CNY)
				)
		AND ExistingData.CostCenter = 'MA2324'
		AND CONVERT(DATE,ExistingData.AddedAt) >= CONVERT(DATE,DATEADD(DAY,-5,@Now)) --Dont re-add if it has been added in the last week
WHERE
	ExistingData.AccountNumber IS NULL

INSERT INTO 
	@InitialPopCo
SELECT
	ENDR.DF_SPE_ACC_ID,
	PD10.DF_SPE_ACC_ID,
	LN10.BF_SSN,
	LN10.LN_SEQ,
	CentralData.dbo.CreateACSKeyline(LN10.BF_SSN, 'B', 'L') AS AscKey,
	RTRIM(ENDR.DM_PRS_1) + ' ' + RTRIM(ENDR.DM_PRS_MID) + ' ' + RTRIM(ENDR.DM_PRS_LST) AS FullName,
	RTRIM(AddrENDR.DX_STR_ADR_1) AS Address1,
	RTRIM(AddrENDR.DX_STR_ADR_2) AS Address2,
	RTRIM(AddrENDR.DM_CT) AS City,
	AddrENDR.DC_DOM_ST AS [State],
	RTRIM(AddrENDR.DF_ZIP_CDE) AS Zip,
	RTRIM(AddrENDR.DM_FGN_CNY) AS Country
FROM
	UDW..PD10_PRS_NME PD10
	INNER JOIN UDW..LN10_LON LN10
		ON LN10.BF_SSN = PD10.DF_PRS_ID
		AND LN10.LA_CUR_PRI > 0.00
		AND LN10.LC_STA_LON10 = 'R'
		AND LN10.IC_LON_PGM != 'TILP'
	INNER JOIN UDW..DW01_DW_CLC_CLU DW01
		ON DW01.BF_SSN = LN10.BF_SSN
		AND DW01.LN_SEQ = LN10.LN_SEQ
		AND DW01.WC_DW_LON_STA NOT IN ('06','12','16','17','18','19','20','21') -- Not Cure, Claim, Death, Disability, or Bky
		AND DW01.WX_OVR_DW_LON_STA NOT LIKE '%CNSLD-STOP PURSUIT%'
	INNER JOIN UDW..LN16_LON_DLQ_HST LN16
		ON LN16.BF_SSN = LN10.BF_SSN
		AND LN16.LN_SEQ = LN10.LN_SEQ
		AND LN16.LC_STA_LON16 = '1' -- Active Delq
		AND LN16.LN_DLQ_MAX + 1 >= 210
		AND CAST(LN16.LD_DLQ_MAX AS DATE) BETWEEN CAST(DATEADD(DAY,-5,@Now) AS DATE) AND @Today
	INNER JOIN UDW..LN20_EDS LN20
		ON LN20.BF_SSN = LN10.BF_SSN
		AND LN20.LN_SEQ = LN10.LN_SEQ
		AND LN20.LC_EDS_TYP = 'M'
		AND LN20.LC_STA_LON20 = 'A'
	INNER JOIN UDW..PD10_PRS_NME ENDR
		ON ENDR.DF_PRS_ID = LN20.LF_EDS
	INNER JOIN UDW..PD30_PRS_ADR AddrENDR
		ON AddrENDR.DF_PRS_ID = ENDR.DF_PRS_ID
		AND AddrENDR.DC_ADR = 'L'
		AND AddrENDR.DI_VLD_ADR = 'Y'
	INNER JOIN
	(
		SELECT
			LN10.BF_SSN,
			SUM(LN10.LA_CUR_PRI) + SUM(DW01.WA_TOT_BRI_OTS) AS Balance
		FROM
			UDW..LN10_LON LN10
			INNER JOIN UDW..DW01_DW_CLC_CLU DW01
				ON DW01.BF_SSN = LN10.BF_SSN
				AND DW01.LN_SEQ = LN10.LN_SEQ
				AND DW01.WC_DW_LON_STA NOT IN ('06','12','16','17','18','19','20','21') -- Not Cure, Claim, Death, Disability, or Bky
				AND DW01.WX_OVR_DW_LON_STA NOT LIKE '%CNSLD-STOP PURSUIT%'
		WHERE
			LN10.LC_STA_LON10 = 'R'
			AND LN10.IC_LON_PGM != 'TILP'
			AND LN10.LA_CUR_PRI > 0.00
		GROUP BY
			LN10.BF_SSN
	) Balance
		ON Balance.BF_SSN = LN10.BF_SSN
		AND Balance.Balance >= 25.00
	LEFT JOIN ULS.[print].PrintProcessingCoBorrower ExistingData
		ON ExistingData.AccountNumber = ENDR.DF_SPE_ACC_ID
		AND ExistingData.EmailAddress = 'Ecorr@UHEAA.org'
		AND ExistingData.ScriptDataId = @ScriptDataId
		AND ExistingData.LetterData = 
			CONCAT(--data to be inserted into letter
					CentralData.dbo.CreateACSKeyline(LN10.BF_SSN, 'B', 'L')
					,',',PD10.DF_SPE_ACC_ID
					,',',RTRIM(ENDR.DM_PRS_1) + ' ' + RTRIM(ENDR.DM_PRS_MID) + ' ' + RTRIM(ENDR.DM_PRS_LST)
					,',',RTRIM(AddrENDR.DX_STR_ADR_1)
					,',',RTRIM(AddrENDR.DX_STR_ADR_2)
					,',',RTRIM(AddrENDR.DM_CT)
					,',',AddrENDR.DC_DOM_ST
					,',',RTRIM(AddrENDR.DF_ZIP_CDE)
					,',',RTRIM(AddrENDR.DM_FGN_CNY)
				)
		AND ExistingData.CostCenter = 'MA2324'
		AND CONVERT(DATE,ExistingData.AddedAt) >= CONVERT(DATE,DATEADD(DAY,-5,@Now)) --Dont re-add if it has been added in the last week
WHERE
	ExistingData.AccountNumber IS NULL

--Borrower Arcs
INSERT INTO ULS..ArcAddProcessing
(
	ArcTypeId,
	ArcResponseCodeId,
	AccountNumber,
	RecipientId,
	ARC,
	ScriptId,
	ProcessOn,
	Comment,
	IsReference,
	IsEndorser,
	CreatedAt,
	CreatedBy
)
OUTPUT 
	INSERTED.ArcAddProcessingId,
	INSERTED.AccountNumber
INTO 
	@ArcTracking
	(
		ArcAddProcessingId,
		AccountNumber
	)
SELECT DISTINCT
	0 AS ArcTypeId, --By loan
	NULL AS ArcResponseCodeId,
	AccountNumber AS AccountNumber,
	NULL AS RecipientId,
	@Arc AS ARC,
	@ScriptId AS ScriptId,
	@Now AS ProcessOn,
	@Comment AS Comment,
	0 AS IsReference,
	0 AS IsEndorser,
	@Now AS CreatedAt,
	SUSER_SNAME() AS CreatedBy
FROM
	@InitialPop

--Coborrower Arcs
INSERT INTO ULS..ArcAddProcessing
(
	ArcTypeId,
	ArcResponseCodeId,
	AccountNumber,
	RecipientId,
	ARC,
	ScriptId,
	ProcessOn,
	Comment,
	IsReference,
	IsEndorser,
	CreatedAt,
	CreatedBy
)
OUTPUT 
	INSERTED.ArcAddProcessingId,
	INSERTED.AccountNumber
INTO 
	@ArcTrackingCo
	(
		ArcAddProcessingId,
		AccountNumber
	)
SELECT DISTINCT
	0 AS ArcTypeId, --By loan
	NULL AS ArcResponseCodeId,
	AccountNumber AS AccountNumber,
	NULL AS RecipientId,
	@Arc AS ARC,
	@ScriptId AS ScriptId,
	@Now AS ProcessOn,
	@CommentCo AS Comment,
	0 AS IsReference,
	1 AS IsEndorser,
	@Now AS CreatedAt,
	SUSER_SNAME() AS CreatedBy
FROM
	@InitialPopCo

--Borrower Arc loan selection
INSERT INTO ULS..ArcLoanSequenceSelection
(
	ArcAddProcessingId,
	LoanSequence
)
SELECT
	A.ArcAddProcessingId,
	IP.LN_SEQ
FROM
	@ArcTracking A
	INNER JOIN @InitialPop IP
		ON IP.AccountNumber = A.AccountNumber

--Co Borrower Arc loan selection
INSERT INTO ULS..ArcLoanSequenceSelection
(
	ArcAddProcessingId,
	LoanSequence
)
SELECT
	A.ArcAddProcessingId,
	IP.LN_SEQ
FROM
	@ArcTrackingCo A
	INNER JOIN @InitialPopCo IP
		ON IP.AccountNumber = A.AccountNumber

--Borrower Letters
INSERT INTO ULS.[print].PrintProcessing
(
	AccountNumber,
	EmailAddress,
	ScriptDataId,
	SourceFile,
	LetterData,
	CostCenter,
	InValidAddress,
	DoNotProcessEcorr,
	OnEcorr,
	ArcNeeded,
	ImagingNeeded,
	AddedBy,
	AddedAt
)
SELECT DISTINCT
	IP.AccountNumber,
	'Ecorr@UHEAA.org' AS EmailAddress,
	@ScriptDataId AS ScriptDataId,
	NULL AS SourceFile,
	CONCAT
	(--data to be inserted into letter
		IP.AscKey
		,',',IP.AccountNumber
		,',',IP.FullName
		,',',IP.Address1
		,',',IP.Address2
		,',',IP.City
		,',',IP.State
		,',',IP.Zip
		,',',IP.Country
	) AS LetterData,
	'MA2324' AS CostCenter,
	0 AS InValidAddress,
	1 AS DoNotProcessEcorr,
	0 AS OnEcorr,
	0 AS ArcNeeded, --By loan.  Dropping arc outside of pp
	0 AS ImagingNeeded,
	@ScriptId AS AddedBy,
	@Today AS AddedAt
FROM
	@InitialPop IP

--Coborrower letters
INSERT INTO ULS.[print].PrintProcessingCoBorrower
(
	AccountNumber, 
	EmailAddress, 
	ScriptDataId, 
	SourceFile, 
	LetterData, 
	CostCenter, 
	DoNotProcessEcorr, 
	OnEcorr, 
	ArcNeeded, 
	ImagingNeeded, 
	AddedBy, 
	AddedAt, 
	BorrowerSsn
)
SELECT DISTINCT
	IP.CoAccountNumber,
	'Ecorr@UHEAA.org' AS EmailAddress,
	@ScriptDataId AS ScriptDataId,
	NULL AS SourceFile,
	CONCAT
	(--data to be inserted into letter
		IP.AscKey
		,',',IP.AccountNumber
		,',',IP.FullName
		,',',IP.Address1
		,',',IP.Address2
		,',',IP.City
		,',',IP.[State]
		,',',IP.Zip
		,',',IP.Country
	) AS LetterData,
	'MA2324' AS CostCenter,
	1 AS DoNotProcessEcorr,
	0 AS OnEcorr,
	0 AS ArcNeeded, --By loan.  Dropping arc outside of pp
	0 AS ImagingNeeded,
	@ScriptId AS AddedBy,
	@Today AS AddedAt,
	PD10.DF_PRS_ID AS BorrowerSsn
FROM
	@InitialPopCo IP
	INNER JOIN UDW..PD10_PRS_NME PD10
		ON PD10.DF_SPE_ACC_ID = IP.AccountNumber

COMMIT TRANSACTION;
