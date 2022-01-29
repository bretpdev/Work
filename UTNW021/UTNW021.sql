/**************************  Populate BorrowerCountComparison  **************************/

DECLARE @Sent INT = (SELECT COUNT(DISTINCT BF_SSN) FROM CLS.scra.ActiveDutyValidation) --total sent to DOD
DECLARE @Received INT = (SELECT COUNT(DISTINCT SSN) FROM CLS.scra._DODReturnFile) --total returned from DOD
DECLARE @NoReturnDataReceived INT = 
(--borrowers sent but have no return data
	SELECT
		COUNT(DISTINCT ADV.BF_SSN)
	FROM
		CLS.scra.ActiveDutyValidation ADV
		LEFT JOIN CLS.scra._DODReturnFile DOD
			ON ADV.BF_SSN = DOD.SSN
	WHERE
		DOD.SSN IS NULL
)

DECLARE @ExtraBorrowersReceived INT = 
(--borrowers returned from DOD but not in original sent file
	SELECT
		COUNT(DISTINCT DOD.SSN)
	FROM
		CLS.scra._DODReturnFile DOD
		LEFT JOIN CLS.scra.ActiveDutyValidation ADV
			ON DOD.SSN = ADV.BF_SSN
	WHERE
		ADV.BF_SSN IS NULL
) 
;

INSERT INTO CLS.scra.BorrowerCountComparison
SELECT
	@Sent,
	@Received,
	@NoReturnDataReceived,
	@ExtraBorrowersReceived,
	GETDATE() AS ComparisonDate,
	NULL AS InactivatedAt
;

/**************************  Populate SentMissingFromReturn  **************************/

INSERT INTO CLS.scra.SentMissingFromReturn
(
	BF_SSN,
	Issue,
	AddedAt,
	AddedBy
)
SELECT DISTINCT
	NEWDATA.BF_SSN,
	NEWDATA.Issue,
	NEWDATA.AddedAt,
	NEWDATA.AddedBy
FROM
(	--CS get missing DOD return R file names
	SELECT
		BF_SSN,
		Issue,
		CAST(GETDATE() AS DATE) AS AddedAt,
		'UTNW021' AS AddedBy
	FROM
	(	--get list of missing return files
		SELECT DISTINCT 
			'' AS BF_SSN,
			'Missing R file: ' + ADV.[FILENAME] AS Issue
		FROM 
			CLS.scra.ActiveDutyValidation ADV --extract filename from table data sent to DOD
			LEFT JOIN
			(--extract filename from files we get back from DOD
				SELECT DISTINCT
					UPPER(LEFT(RIGHT(SourceFile,13),9)) AS SourceFile --extract filename from field
				FROM
					CLS.scra._DODReturnFile
			) DOD
				ON ADV.[FILENAME] = DOD.SourceFile
		WHERE
			DOD.SourceFile IS NULL

		UNION ALL

		--get borrowers who were sent to DOD but have no return info 
		SELECT DISTINCT
			ADV.BF_SSN,
			'No Return Data Received' AS Issue
		FROM
			CLS.scra.ActiveDutyValidation ADV
			LEFT JOIN CLS.scra._DODReturnFile DOD
				ON ADV.BF_SSN = DOD.SSN
		WHERE
			DOD.SSN IS NULL

		UNION ALL

		--get borrowers who came from DOD but not sent by us
		SELECT DISTINCT
			DOD.SSN AS BF_SSN,
			'Extra Borrowers Received' AS Issue
		FROM
			CLS.scra._DODReturnFile DOD
			LEFT JOIN CLS.scra.ActiveDutyValidation ADV
				ON DOD.SSN = ADV.BF_SSN
		WHERE
			ADV.BF_SSN IS NULL
	) U
) NEWDATA
	LEFT JOIN CLS.scra.SentMissingFromReturn EXISTING_DATA
		ON NEWDATA.BF_SSN = EXISTING_DATA.BF_SSN
		AND NEWDATA.AddedAt = EXISTING_DATA.AddedAt
WHERE
	EXISTING_DATA.BF_SSN IS NULL
;

/**************************  LN90X   ***************************/
--exclude claim paid & deconverted to DMCS/other servicer. 
--used in exclusionary left joins in "update scra.DataComparison.ActiveRow field" and "insert new records into scra.DataComparison"

DROP TABLE IF EXISTS ##LN90X_SCRAFED;

CREATE TABLE ##LN90X_SCRAFED (BF_SSN CHAR(9), LN_SEQ SMALLINT);

INSERT INTO ##LN90X_SCRAFED (BF_SSN, LN_SEQ)
SELECT DISTINCT
	LN90.BF_SSN,
	LN90.LN_SEQ
FROM
	CDW..LN90_FIN_ATY LN90
	INNER JOIN CLS.scra._DODReturnFile DOD
		ON LN90.BF_SSN = DOD.SSN
WHERE
	(
		(--10:PAYMENT/30:GUARANTOR - claim paid more than 45 days ago
			LN90.PC_FAT_TYP = '10' 
			AND LN90.PC_FAT_SUB_TYP = '30'
		) 
		OR LN90.PC_FAT_TYP = '04' --deconverted to DMCS or other servicer more than 45 days ago
	)
	AND LN90.LC_STA_LON90 = 'A'
	AND ISNULL(LN90.LC_FAT_REV_REA, ' ') = ' '
;

/**************************  update scra.DataComparison.ActiveRow field  **************************/

DECLARE @DOD_BORR TABLE (BorrSSN VARCHAR(9), Loan SMALLINT);

INSERT INTO @DOD_BORR (BorrSSN, Loan)
SELECT DISTINCT
	DOD.SSN AS BorrSSN,
	LN10.LN_SEQ AS Loan
FROM 
	CLS.scra._DODReturnFile DOD
	INNER JOIN CDW..LN10_LON LN10
		ON DOD.SSN = LN10.BF_SSN
WHERE
	DOD.ActiveDutyOnActiveDutyStatusDt <> 'Z'
	AND DOD.LeftActiveDutyLE367DaysFromActiveDutyStatusDt <> 'Z'
	AND DOD.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt <> 'Z'
	AND ISNULL(LN10.LD_PIF_RPT,GETDATE()) > DATEADD(DAY,-45,CONVERT(DATE,GETDATE()))--exclude paid in full more than 45 days ago
;

UPDATE 
	DC
SET 
	ActiveRow = 0 --deactivates borrower records in historical table if there's a new record for that borrower
FROM 
	CLS.scra.DataComparison DC
	INNER JOIN @DOD_BORR DB
		ON DC.BorrSSN = DB.BorrSSN
		AND DC.Loan = DB.Loan
WHERE
	DC.ActiveRow = 1
;

/**************************  deactivates borrowers over 45 days & paid in full  **************************/

UPDATE 
	DC
SET 
	ActiveRow = 0 --deactivation
FROM
	CLS.scra.DataComparison DC
	INNER JOIN CDW..LN10_LON LN10
		ON DC.BorrSSN = LN10.BF_SSN
WHERE
	LN10.LC_STA_LON10 = 'R'
	AND DC.ActiveRow = 1
	AND ISNULL(DC.LoanBalance,0) <= 0.00
	AND ISNULL(LN10.LD_PIF_RPT,GETDATE()) < DATEADD(DAY,-45,CONVERT(DATE,GETDATE())) --deactivates borrowers over 45 days & paid in full
;

/**************************  insert new records into scra.DataComparison **************************/

INSERT INTO CLS.scra.DataComparison
(
	ActiveRow,
	Loan,
	LoanBalance,
	StatusDate,
	BorrSSN,
	BorrActive,
	EndrSSN,
	EndrActive,
	BeginBrwr,
	EIDB,
	BeginEndr,
	EIDE,
	EndBrwr,
	EndEndr,
	BorrIsReservist,
	EndrIsReservist,
	CreatedAt,
	ServiceComponent,
	EIDServiceComponent,
	EndorserServiceComponent,
	EndorserEIDServiceComponent,
	ActiveBeginBrwr,
	ActiveEndBrwr,
	ActiveBeginEndr,
	ActiveEndEndr
)
SELECT
	NEWDATA.ActiveRow,
	NEWDATA.Loan,
	NEWDATA.LoanBalance,
	NEWDATA.StatusDate,
	NEWDATA.BorrSSN,
	NEWDATA.BorrActive,
	NEWDATA.EndrSSN,
	NEWDATA.EndrActive,
	NEWDATA.BeginBrwr,
	NEWDATA.EIDB,
	NEWDATA.BeginEndr,
	NEWDATA.EIDE,
	NEWDATA.EndBrwr,
	NEWDATA.EndEndr,
	NEWDATA.BorrIsReservist,
	NEWDATA.EndrIsReservist,
	NEWDATA.CreatedAt,
	NEWDATA.ServiceComponent,
	NEWDATA.EIDServiceComponent,
	NEWDATA.EndorserServiceComponent,
	NEWDATA.EndorserEIDServiceComponent,
	NEWDATA.ActiveBeginBrwr,
	NEWDATA.ActiveEndBrwr,
	NEWDATA.ActiveBeginEndr,
	NEWDATA.ActiveEndEndr
FROM
(--inserted rows
	SELECT DISTINCT
		1 AS ActiveRow, --new record indicator
		BORR.Loan,
		BORR.LoanBalance,
		BORR.ActiveDutyStatusDate AS StatusDate,
		BORR.BorrSSN,
		CASE WHEN 
				BORR.ActiveDutyOnActiveDutyStatusDt = 'N' 
				AND BORR.LeftActiveDutyLE367DaysFromActiveDutyStatusDt = 'Y' 
				AND BORR.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'N' 
			THEN 0
			 WHEN
				BORR.ActiveDutyOnActiveDutyStatusDt IN ('X','Y')
				OR
				(
					BORR.ActiveDutyOnActiveDutyStatusDt = 'N' 
					AND BORR.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y'
				) 
			THEN 1
			ELSE NULL --doesn't exist in DOD database
		END AS BorrActive,
		ENDR.EndrSSN,
		CASE WHEN 
				ENDR.ActiveDutyOnActiveDutyStatusDt = 'N' 
				AND ENDR.LeftActiveDutyLE367DaysFromActiveDutyStatusDt = 'Y' 
				AND ENDR.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'N' 
			THEN 0
			WHEN
				ENDR.ActiveDutyOnActiveDutyStatusDt IN ('X','Y')
				OR
				(
					ENDR.ActiveDutyOnActiveDutyStatusDt = 'N'
					AND ENDR.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y'
				) 
			THEN 1
			ELSE NULL --doesn't exist in DOD database
		END AS EndrActive,
		IIF(BORR.ActiveDutyOnActiveDutyStatusDt = 'N' AND BORR.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y', BORR.EIDBeginDate, BORR.ActiveDutyBeginDate) AS BeginBrwr,
		IIF(BORR.ActiveDutyOnActiveDutyStatusDt = 'N' AND BORR.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y', 1, 0) AS EIDB,
		IIF(ENDR.ActiveDutyOnActiveDutyStatusDt = 'N' AND ENDR.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y', ENDR.EIDBeginDate, ENDR.ActiveDutyBeginDate) AS BeginEndr,
		IIF(ENDR.ActiveDutyOnActiveDutyStatusDt = 'N' AND ENDR.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y', 1, 0) AS EIDE,
		CASE WHEN BORR.ActiveDutyOnActiveDutyStatusDt = 'N' AND BORR.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y' THEN CONVERT(DATE,'20991231')
			 WHEN BORR.ActiveDutyOnActiveDutyStatusDt IN ('X','Y') AND BORR.ActiveDutyEndDate IS NULL THEN CONVERT(DATE,'20991231')
			 ELSE BORR.ActiveDutyEndDate
		END AS EndBrwr,
		CASE WHEN ENDR.ActiveDutyOnActiveDutyStatusDt = 'N' AND ENDR.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y' THEN CONVERT(DATE,'20991231')
			 WHEN ENDR.ActiveDutyOnActiveDutyStatusDt IN ('X','Y') AND ENDR.ActiveDutyEndDate IS NULL THEN CONVERT(DATE,'20991231')
			 ELSE ENDR.ActiveDutyEndDate
		END AS EndEndr,
		IIF(BORR.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y', 1, 0) AS BorrIsReservist,
		IIF(ENDR.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y', 1, 0) AS EndrIsReservist,
		GETDATE() AS CreatedAt,
		BORR.ServiceComponent AS ServiceComponent,
		BORR.EIDServiceComponent AS EIDServiceComponent,
		ENDR.ServiceComponent AS EndorserServiceComponent,
		ENDR.EIDServiceComponent AS EndorserEIDServiceComponent,
		BORR.ActiveDutyBeginDate AS ActiveBeginBrwr,
		CASE WHEN BORR.ActiveDutyBeginDate IS NOT NULL THEN ISNULL(BORR.ActiveDutyEndDate,'2099-12-31') ELSE NULL END AS ActiveEndBrwr,
		ENDR.ActiveDutyBeginDate AS ActiveBeginEndr,
		CASE WHEN ENDR.ActiveDutyBeginDate IS NOT NULL THEN ISNULL(ENDR.ActiveDutyEndDate,'2099-12-31') ELSE NULL END AS ActiveEndEndr
	FROM
	(--gets borrower population from DOD file, converts dates, and nulls out 0's
		SELECT DISTINCT
			DOD.SSN AS BorrSSN,
			LN10.LN_SEQ AS Loan,
			LN10.LA_CUR_PRI AS LoanBalance,
			CONVERT(DATE,CONVERT(CHAR(8),(DOD.ActiveDutyStatusDate))) AS ActiveDutyStatusDate,
			DOD.ActiveDutyOnActiveDutyStatusDt,
			DOD.LeftActiveDutyLE367DaysFromActiveDutyStatusDt,
			DOD.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt,
			CONVERT(DATE,CONVERT(CHAR(8),(NULLIF(DOD.ActiveDutyEndDate,0)))) AS ActiveDutyEndDate,
			CONVERT(DATE,CONVERT(CHAR(8),(NULLIF(DOD.ActiveDutyBeginDate,0)))) AS ActiveDutyBeginDate,
			CONVERT(DATE,CONVERT(CHAR(8),(NULLIF(DOD.EIDBeginDate,0)))) AS EIDBeginDate,
			CONVERT(DATE,CONVERT(CHAR(8),(NULLIF(DOD.EIDEndDate,0)))) AS EIDEndDate,
			DOD.ServiceComponent AS ServiceComponent,
			DOD.EIDServiceComponent AS EIDServiceComponent
		FROM 
			CLS.scra._DODReturnFile DOD
			INNER JOIN CDW..LN10_LON LN10
				ON DOD.SSN = LN10.BF_SSN
			LEFT JOIN ##LN90X_SCRAFED LN90X
				ON LN10.BF_SSN = LN90X.BF_SSN
				AND LN10.LN_SEQ = LN90X.LN_SEQ
		WHERE
			LN10.LC_STA_LON10 = 'R'
			AND	DOD.ActiveDutyOnActiveDutyStatusDt <> 'Z'
			AND DOD.LeftActiveDutyLE367DaysFromActiveDutyStatusDt <> 'Z'
			AND DOD.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt <> 'Z'
			AND ISNULL(LN10.LD_PIF_RPT,GETDATE()) > DATEADD(DAY,-45,CONVERT(DATE,GETDATE())) --exclude paid in full more than 45 days ago
			AND LN90X.BF_SSN IS NULL--exclude claim paid & deconverted to DMCS/other servicer
	) BORR
	LEFT JOIN 
	(--gets endorser population from DOD file, converts dates, and nulls out 0's
		SELECT DISTINCT
			DOD.SSN AS EndrSSN,
			LN20.BF_SSN AS BorrSSN,
			LN20.LN_SEQ AS Loan,
			LN10.LA_CUR_PRI AS LoanBalance,
			CONVERT(DATE,CONVERT(CHAR(8),(DOD.ActiveDutyStatusDate))) AS ActiveDutyStatusDate,
			DOD.ActiveDutyOnActiveDutyStatusDt,
			DOD.LeftActiveDutyLE367DaysFromActiveDutyStatusDt,
			DOD.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt,
			CONVERT(DATE,CONVERT(CHAR(8),(NULLIF(DOD.ActiveDutyEndDate,0)))) AS ActiveDutyEndDate,
			CONVERT(DATE,CONVERT(CHAR(8),(NULLIF(DOD.ActiveDutyBeginDate,0)))) AS ActiveDutyBeginDate,
			CONVERT(DATE,CONVERT(CHAR(8),(NULLIF(DOD.EIDBeginDate,0)))) AS EIDBeginDate,
			CONVERT(DATE,CONVERT(CHAR(8),(NULLIF(DOD.EIDEndDate,0)))) AS EIDEndDate,
			DOD.ServiceComponent AS ServiceComponent,
			DOD.EIDServiceComponent AS EIDServiceComponent
		FROM
			CLS.scra._DODReturnFile DOD
			INNER JOIN CDW..LN20_EDS LN20
				ON DOD.SSN = LN20.LF_EDS
			INNER JOIN CDW..LN10_LON LN10
				ON LN20.BF_SSN = LN10.BF_SSN
				AND LN20.LN_SEQ = LN10.LN_SEQ
			LEFT JOIN ##LN90X_SCRAFED LN90X
				ON LN10.BF_SSN = LN90X.BF_SSN
				AND LN10.LN_SEQ = LN90X.LN_SEQ
		WHERE
			LN10.LC_STA_LON10 = 'R'
			AND DOD.ActiveDutyOnActiveDutyStatusDt <> 'Z'
			AND DOD.LeftActiveDutyLE367DaysFromActiveDutyStatusDt <> 'Z'
			AND DOD.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt <> 'Z'
			AND LN20.LC_STA_LON20 = 'A'
			AND ISNULL(LN10.LD_PIF_RPT,GETDATE()) > DATEADD(DAY,-45,CONVERT(DATE,GETDATE())) --exclude paid in full more than 45 days ago
			AND LN90X.BF_SSN IS NULL--exclude claim paid & deconverted to DMCS/other servicer
	) ENDR
		ON BORR.BorrSSN = ENDR.BorrSSN
		AND BORR.Loan = ENDR.Loan
) NEWDATA
LEFT JOIN CLS.scra.DataComparison EXISTING_DATA
	ON  NEWDATA.ActiveRow = EXISTING_DATA.ActiveRow
	AND NEWDATA.Loan = EXISTING_DATA.Loan
	AND ISNULL(NEWDATA.LoanBalance,0) = ISNULL(EXISTING_DATA.LoanBalance,0)
	AND NEWDATA.StatusDate = EXISTING_DATA.StatusDate
	AND NEWDATA.BorrSSN	= EXISTING_DATA.BorrSSN
	AND ISNULL(NEWDATA.BorrActive,'') = ISNULL(EXISTING_DATA.BorrActive,'')
	AND ISNULL(NEWDATA.EndrSSN,'') = ISNULL(EXISTING_DATA.EndrSSN,'')
	AND ISNULL(NEWDATA.EndrActive,'') = ISNULL(EXISTING_DATA.EndrActive,'')
	AND ISNULL(NEWDATA.BeginBrwr,'') = ISNULL(EXISTING_DATA.BeginBrwr,'')
	AND ISNULL(NEWDATA.EIDB,'')	= ISNULL(EXISTING_DATA.EIDB,'')
	AND ISNULL(NEWDATA.BeginEndr,'') = ISNULL(EXISTING_DATA.BeginEndr,'')
	AND ISNULL(NEWDATA.EIDE,'')	= ISNULL(EXISTING_DATA.EIDE,'')
	AND ISNULL(NEWDATA.EndBrwr,'') = ISNULL(EXISTING_DATA.EndBrwr,'')
	AND ISNULL(NEWDATA.EndEndr,'') = ISNULL(EXISTING_DATA.EndEndr,'')
	AND ISNULL(NEWDATA.BorrIsReservist,'')= ISNULL(EXISTING_DATA.BorrIsReservist,'')
	AND ISNULL(NEWDATA.EndrIsReservist,'')= ISNULL(EXISTING_DATA.EndrIsReservist,'')
	AND CAST(NEWDATA.CreatedAt AS DATE) = CAST(EXISTING_DATA.CreatedAt AS DATE)
	AND NEWDATA.ServiceComponent = EXISTING_DATA.ServiceComponent
	AND NEWDATA.EIDServiceComponent = EXISTING_DATA.EIDServiceComponent
	AND NEWDATA.EndorserServiceComponent = EXISTING_DATA.EndorserServiceComponent
	AND NEWDATA.EndorserEIDServiceComponent = EXISTING_DATA.EndorserEIDServiceComponent
	AND ISNULL(NEWDATA.ActiveBeginBrwr,'') = ISNULL(EXISTING_DATA.ActiveBeginBrwr,'')
	AND ISNULL(NEWDATA.ActiveEndBrwr,'') = ISNULL(EXISTING_DATA.ActiveEndBrwr,'')
	AND ISNULL(NEWDATA.ActiveBeginEndr,'') = ISNULL(EXISTING_DATA.ActiveBeginEndr,'')
	AND ISNULL(NEWDATA.ActiveEndEndr,'') = ISNULL(EXISTING_DATA.ActiveEndEndr,'')
WHERE
	EXISTING_DATA.BorrSSN IS NULL
;

/**************************  inserts into scra.ActiveDutyReporting  **************************/
INSERT INTO CLS.scra.ActiveDutyReporting
(
	BorrSSN,
	EndrSSN,
	IsEndorser,
	TXCXBegin,
	TXCXEnd,
	TXCXType,
	ServiceComponent,
	TXCXUpdated,
	CreatedAt,
	CreatedBy,
	DeletedAt,
	DeletedBy,
	ErroredAt
)
SELECT
	NEWDATA.BorrSSN,
	NEWDATA.EndrSSN,
	NEWDATA.IsEndorser,
	NEWDATA.TXCXBegin,
	NEWDATA.TXCXEnd,
	NEWDATA.TXCXType,
	NEWDATA.ServiceComponent,
	NEWDATA.TXCXUpdated,
	NEWDATA.CreatedAt,
	NEWDATA.CreatedBy,
	NEWDATA.DeletedAt,
	NEWDATA.DeletedBy,
	NEWDATA.ErroredAt
FROM
(
	SELECT DISTINCT
		DC.BorrSSN,
		CASE WHEN DC.EndrActive IS NOT NULL THEN DC.EndrSSN ELSE NULL END AS EndrSSN,
		CASE WHEN DC.EndrSSN IS NOT NULL AND DC.EndrActive IS NOT NULL THEN 1 ELSE 0 END AS IsEndorser,
		CASE WHEN DC.EndrSSN IS NOT NULL AND DC.EndrActive IS NOT NULL THEN DC.ActiveBeginEndr ELSE DC.ActiveBeginBrwr END AS TXCXBegin,
		CASE WHEN DC.EndrSSN IS NOT NULL AND DC.EndrActive IS NOT NULL THEN ISNULL(DC.ActiveEndEndr,'2099-12-31') ELSE ISNULL(DC.ActiveEndBrwr,'2099-12-31') END AS TXCXEnd,
		'SCRA' AS TXCXType,
		CASE  WHEN DC.EndrSSN IS NOT NULL AND DC.EndrActive IS NOT NULL THEN DC.EndorserServiceComponent
				WHEN DC.EndrSSN IS NULL THEN DC.ServiceComponent
				ELSE DC.ServiceComponent
		END AS ServiceComponent,
		NULL AS TXCXUpdated,
		GETDATE() AS CreatedAt,
		SUSER_SNAME() AS CreatedBy,
		NULL AS DeletedAt,
		NULL AS DeletedBy,
		NULL AS ErroredAt
	FROM
		CLS.scra.DataComparison DC
	WHERE
		DC.ActiveRow = 1
		AND
		(
			(
				DC.EndrActive IS NOT NULL
				AND DC.ActiveBeginEndr IS NOT NULL
				AND DC.ActiveEndEndr IS NOT NULL
			)
			OR
			(
				DC.BorrActive IS NOT NULL
				AND DC.ActiveBeginBrwr IS NOT NULL
				AND DC.ActiveEndBrwr IS NOT NULL
			)
		)
		AND NOT 
		(
			( --reservist endorsers need to be ignored for ENDORSER reporting
				DC.EndrActive IS NOT NULL
				AND DC.EndrIsReservist = 1
			)
			OR 
			( --exclude reservist only borrowers that dont have endorsers
				DC.EndrSSN IS NULL
				AND DC.BorrIsReservist = 1
			)
		)

	UNION

	SELECT DISTINCT
		DC.BorrSSN,
		NULL AS EndrSSN,
		0 AS IsEndorser,
		DC.ActiveBeginBrwr AS TXCXBegin,
		DC.ActiveEndBrwr AS TXCXEnd,
		'SCRA' AS TXCXType,
		DC.ServiceComponent AS ServiceComponent,
		NULL AS TXCXUpdated,
		GETDATE() AS CreatedAt,
		SUSER_SNAME() AS CreatedBy,
		NULL AS DeletedAt,
		NULL AS DeletedBy,
		NULL AS ErroredAt
	FROM
		CLS.scra.DataComparison DC
		LEFT JOIN
		(
			SELECT DISTINCT
				BorrSSN
			FROM
				CLS.scra.DataComparison
			WHERE
				EndrSSN IS NULL
				AND ActiveRow = 1
				AND
				(	
					BorrActive IS NOT NULL
					OR EndrActive IS NOT NULL
				)
		) BorrowerOnly
			ON BorrowerOnly.BorrSSN = DC.BorrSSN
	WHERE
		DC.ActiveRow = 1
		AND
		(	
			DC.BorrActive IS NOT NULL
			OR DC.EndrActive IS NOT NULL
		)
		AND BorrowerOnly.BorrSSN IS NULL --Doesnt have any records not tied to an endorser
		AND DC.ActiveBeginBrwr IS NOT NULL
		AND DC.ActiveEndBrwr IS NOT NULL
		AND DC.BorrIsReservist != 1 --Exclude borrowers that are reservist
) NEWDATA
	LEFT JOIN CLS.scra.ActiveDutyReporting EXISTING_DATA
		ON NEWDATA.BorrSSN = EXISTING_DATA.BorrSSN
		AND ISNULL(NEWDATA.EndrSSN,'') = ISNULL(EXISTING_DATA.EndrSSN,'')
		AND NEWDATA.IsEndorser = EXISTING_DATA.IsEndorser
		AND NEWDATA.TXCXBegin = EXISTING_DATA.TXCXBegin
		AND NEWDATA.TXCXEnd = EXISTING_DATA.TXCXEnd
		AND NEWDATA.TXCXType = EXISTING_DATA.TXCXType
		AND NEWDATA.ServiceComponent = EXISTING_DATA.ServiceComponent
		AND CAST(NEWDATA.CreatedAt AS DATE) = CAST(EXISTING_DATA.CreatedAt AS DATE)
WHERE
	EXISTING_DATA.BorrSSN IS NULL
;

/**************************  inserts into scra.ScriptProcessing  **************************/

;WITH MIN_DATES AS --DATA CHUNK 1 used in multiple places - borrower level
(--for TXCXBegin field - borrower level
	SELECT DISTINCT
		BRWRS.BorrSSN,
		BRWRS.Loan,
		BRWRS.EndrSSN,
		MIN_BRWR.BeginBrwr,
		MIN_ENDR.BeginEndr,
		MIN_BRWR.EIDB,
		MIN_ENDR.EIDE
	FROM
	(
		SELECT DISTINCT
			BorrSSN,
			Loan,
			ISNULL(EndrSSN,'') AS EndrSSN
		FROM
			CLS.scra.DataComparison
		WHERE
			ActiveRow = 1
			AND
			(
				BorrActive IS NOT NULL
				OR EndrActive IS NOT NULL
			)
	) BRWRS
	LEFT JOIN
	(--gets earliest borrower date
		SELECT DISTINCT
			MinRow.BorrSSN,
			MinRow.Loan,
			ISNULL(MinRow.EndrSSN,'') AS EndrSSN,
			MinRow.EIDB,
			ISNULL(MinActive.BeginBrwr,MIN(MinRow.BeginBrwr)) AS BeginBrwr
		FROM
			CLS.scra.DataComparison MinRow
			LEFT JOIN
			(
				SELECT DISTINCT
					BorrSSN,
					Loan,
					ISNULL(EndrSSN,'') AS EndrSSN,
					EIDB,
					MIN(BeginBrwr) AS BeginBrwr
				FROM
					CLS.scra.DataComparison 
				WHERE
					ActiveRow = 1
					AND LoanBalance > 0.00
					AND BorrActive IS NOT NULL
				GROUP BY
					BorrSSN,
					Loan,
					ISNULL(EndrSSN,''),
					EIDB
			) MinActive
				ON MinRow.BorrSSN = MinActive.BorrSSN
				AND MinRow.Loan = MinActive.Loan
				AND ISNULL(MinRow.EndrSSN,'') = MinActive.EndrSSN
				AND MinRow.EIDB = MinActive.EIDB
		WHERE
			MinRow.ActiveRow = 1
			AND MinRow.BorrActive IS NOT NULL
		GROUP BY
			MinRow.BorrSSN,
			MinRow.Loan,
			ISNULL(MinRow.EndrSSN,''),
			MinRow.EIDB,
			MinActive.BeginBrwr
	) MIN_BRWR
		ON MIN_BRWR.BorrSSN = BRWRS.BorrSSN
		AND MIN_BRWR.Loan = BRWRS.Loan
		AND MIN_BRWR.EndrSSN = BRWRS.EndrSSN
	LEFT JOIN --puts borrowers and any endorsers on the same line	
	(--gets earliest endorser date
		SELECT DISTINCT
			MinRow.BorrSSN,
			MinRow.Loan,
			ISNULL(MinRow.EndrSSN,'') AS EndrSSN,
			MinRow.EIDE,
			ISNULL(MinActive.BeginEndr,MIN(MinRow.BeginEndr)) AS BeginEndr
		FROM
			CLS.scra.DataComparison MinRow
			LEFT JOIN
			(
				SELECT DISTINCT
					BorrSSN,
					Loan,
					ISNULL(EndrSSN,'') AS EndrSSN,
					EIDE,
					MIN(BeginEndr) AS BeginEndr
				FROM
					CLS.scra.DataComparison 
				WHERE
					ActiveRow = 1
					AND LoanBalance > 0.00
					AND EndrActive IS NOT NULL
				GROUP BY
					BorrSSN,
					Loan,
					ISNULL(EndrSSN,''),
					EIDE
			) MinActive
				ON MinRow.BorrSSN = MinActive.BorrSSN
				AND MinRow.Loan = MinActive.Loan
				AND ISNULL(MinRow.EndrSSN,'') = MinActive.EndrSSN
				AND MinRow.EIDE = MinActive.EIDE
		WHERE
			MinRow.ActiveRow = 1
			AND MinRow.EndrActive IS NOT NULL
		GROUP BY
			MinRow.BorrSSN,
			MinRow.Loan,
			ISNULL(MinRow.EndrSSN,''),
			MinRow.EIDE,
			MinActive.BeginEndr
	) MIN_ENDR
		ON MIN_ENDR.BorrSSN = BRWRS.BorrSSN
		AND MIN_ENDR.Loan = BRWRS.Loan
		AND MIN_ENDR.EndrSSN = BRWRS.EndrSSN
) --MIN_DATES
,MAX_DATES AS --DATA CHUNK 2 used in multiple places - borrower level
(--for TXCXEnd field - borrower level
	SELECT DISTINCT
		BRWRS.BorrSSN,
		BRWRS.Loan,
		BRWRS.EndrSSN,
		MAX_BRWR.EndBrwr,
		MAX_ENDR.EndEndr,
		MAX_BRWR.EIDB,
		MAX_ENDR.EIDE
	FROM
	(
		SELECT DISTINCT
			BorrSSN,
			Loan,
			ISNULL(EndrSSN,'') AS EndrSSN
		FROM
			CLS.scra.DataComparison
		WHERE
			ActiveRow = 1
			AND
			(
				BorrActive IS NOT NULL
				OR EndrActive IS NOT NULL
			)
	) BRWRS
	LEFT JOIN
	(--gets earliest borrower date
		SELECT DISTINCT
			MAXRow.BorrSSN,
			MAXRow.Loan,
			ISNULL(MAXRow.EndrSSN,'') AS EndrSSN,
			MAXRow.EIDB,
			ISNULL(MAXActive.EndBrwr,MAX(MAXRow.EndBrwr)) AS EndBrwr
		FROM
			CLS.scra.DataComparison MAXRow
			LEFT JOIN
			(
				SELECT DISTINCT
					BorrSSN,
					Loan,
					ISNULL(EndrSSN,'') AS EndrSSN,
					EIDB,
					MAX(EndBrwr) AS EndBrwr
				FROM
					CLS.scra.DataComparison 
				WHERE
					ActiveRow = 1
					AND LoanBalance > 0.00
					AND BorrActive IS NOT NULL
				GROUP BY
					BorrSSN,
					Loan,
					ISNULL(EndrSSN,''),
					EIDB
			) MAXActive
				ON MAXRow.BorrSSN = MAXActive.BorrSSN
				AND MAXRow.Loan = MAXActive.Loan
				AND ISNULL(MAXRow.EndrSSN,'') = MAXActive.EndrSSN
				AND MAXRow.EIDB = MAXActive.EIDB
		WHERE
			MAXRow.ActiveRow = 1
			AND MAXRow.BorrActive IS NOT NULL
		GROUP BY
			MAXRow.BorrSSN,
			MAXRow.Loan,
			ISNULL(MAXRow.EndrSSN,''),
			MAXRow.EIDB,
			MAXActive.EndBrwr
	) MAX_BRWR
		ON MAX_BRWR.BorrSSN = BRWRS.BorrSSN
		AND MAX_BRWR.Loan = BRWRS.Loan
		AND MAX_BRWR.EndrSSN = BRWRS.EndrSSN
	LEFT JOIN --puts borrowers and any endorsers on the same line	
	(--gets earliest endorser date
		SELECT DISTINCT
			MAXRow.BorrSSN,
			MAXRow.Loan,
			ISNULL(MAXRow.EndrSSN,'') AS EndrSSN,
			MAXRow.EIDE,
			ISNULL(MAXActive.EndEndr,MAX(MAXRow.EndEndr)) AS EndEndr
		FROM
			CLS.scra.DataComparison MAXRow
			LEFT JOIN
			(
				SELECT DISTINCT
					BorrSSN,
					Loan,
					ISNULL(EndrSSN,'') AS EndrSSN,
					EIDE,
					MAX(EndEndr) AS EndEndr
				FROM
					CLS.scra.DataComparison 
				WHERE
					ActiveRow = 1
					AND LoanBalance > 0.00
					AND EndrActive IS NOT NULL
				GROUP BY
					BorrSSN,
					Loan,
					ISNULL(EndrSSN,''),
					EIDE
			) MAXActive
				ON MAXRow.BorrSSN = MAXActive.BorrSSN
				AND MAXRow.Loan = MAXActive.Loan
				AND ISNULL(MAXRow.EndrSSN,'') = MAXActive.EndrSSN
				AND MAXRow.EIDE = MAXActive.EIDE
		WHERE
			MAXRow.ActiveRow = 1
			AND MAXRow.EndrActive IS NOT NULL
		GROUP BY
			MAXRow.BorrSSN,
			MAXRow.Loan,
			ISNULL(MAXRow.EndrSSN,''),
			MAXRow.EIDE,
			MAXActive.EndEndr
	) MAX_ENDR
		ON MAX_ENDR.BorrSSN = BRWRS.BorrSSN
		AND MAX_ENDR.Loan = BRWRS.Loan
		AND MAX_ENDR.EndrSSN = BRWRS.EndrSSN
),
ASSIGN_DATES AS
(
	SELECT DISTINCT
		DC.BorrSSN,
		DC.DataComparisonId,
		DC.StatusDate,
		LN72.LN_SEQ AS Loan,
		SUM(DC.LoanBalance) OVER(PARTITION BY DC.BorrSSN) AS sum_LoanBalance,
		LN72.LD_ITR_EFF_BEG AS LN72Begin,
		LN72.LD_ITR_EFF_END AS LN72End,
		LN72.LR_INT_RDC_PGM_ORG AS LN72RegRate,
		IIF(LN72.LC_INT_RDC_PGM = 'M', 1, 0) AS LN72SCRA,
		LN10.LD_LON_EFF_ADD AS LN10LnAdd,
		LN10.LD_LON_1_DSB AS LN10Disb,
		LN10.LA_CUR_PRI AS LN10CurPri,
		LN10.LC_STA_LON10 AS LN10Sta,
		RTRIM(LN10.LC_SST_LON10) AS LN10Sub,
		DW01.WC_DW_LON_STA AS DW01Sta,
		CASE WHEN DC.BeginEndr IS NULL THEN DC.BeginBrwr
			 WHEN DC.EndrActive = 1 AND ISNULL(DC.BorrActive,0) = 0 AND DC.EndrSSN = LN20.LF_EDS THEN DC.BeginEndr
			 WHEN DC.BorrActive = 1 AND ISNULL(DC.EndrActive,0) = 0 THEN DC.BeginBrwr
			 WHEN DC.BorrActive = 0 AND DC.EndrActive IS NULL THEN DC.BeginBrwr
			 WHEN DC.BorrActive IS NULL AND DC.EndrActive = 0 AND DC.EndrSSN = LN20.LF_EDS THEN DC.BeginEndr
			 WHEN DC.BorrActive = 0 AND DC.EndrActive = 0 AND DC.EndrSSN = LN20.LF_EDS THEN 
				CASE WHEN ISNULL(DC.BeginBrwr,'9999-12-31') <= ISNULL(DC.BeginEndr,'9999-12-31') THEN DC.BeginBrwr
					 WHEN ISNULL(DC.BeginBrwr,'9999-12-31') > ISNULL(DC.BeginEndr,'9999-12-31') THEN DC.BeginEndr
					 ELSE NULL
				END
			 WHEN DC.EndrActive = 1 AND DC.BorrActive = 1 AND DC.EndrSSN = LN20.LF_EDS THEN
				CASE WHEN ISNULL(DC.BeginBrwr,'9999-12-31') <= ISNULL(DC.BeginEndr,'9999-12-31') THEN DC.BeginBrwr
					 WHEN ISNULL(DC.BeginBrwr,'9999-12-31') > ISNULL(DC.BeginEndr,'9999-12-31') THEN DC.BeginEndr
					 ELSE NULL
				END
			 ELSE NULL
		END AS DODBegin, --loan level
		CASE WHEN DC.EndEndr IS NULL THEN DC.EndBrwr
			 WHEN DC.EndrActive = 1 AND ISNULL(DC.BorrActive,0) = 0 AND DC.EndrSSN = LN20.LF_EDS THEN DC.EndEndr
			 WHEN DC.BorrActive = 1 AND ISNULL(DC.EndrActive,0) = 0 THEN DC.EndBrwr
			 WHEN DC.BorrActive = 0 AND DC.EndrActive IS NULL THEN DC.EndBrwr
			 WHEN DC.BorrActive IS NULL AND DC.EndrActive = 0 AND DC.EndrSSN = LN20.LF_EDS THEN DC.EndEndr
			 WHEN DC.BorrActive = 0 AND DC.EndrActive = 0 AND DC.EndrSSN = LN20.LF_EDS THEN 
				CASE WHEN ISNULL(DC.EndBrwr,'1900-01-01') < ISNULL(DC.EndEndr,'1900-01-01') THEN DC.EndEndr
					 WHEN ISNULL(DC.EndBrwr,'1900-01-01') >= ISNULL(DC.EndEndr,'1900-01-01') THEN DC.EndBrwr
					 ELSE NULL
				END
			 WHEN DC.EndrActive = 1 AND DC.BorrActive = 1 AND DC.EndrSSN = LN20.LF_EDS THEN 
				CASE WHEN ISNULL(DC.EndBrwr,'1900-01-01') < ISNULL(DC.EndEndr,'1900-01-01') THEN DC.EndEndr
					 WHEN ISNULL(DC.EndBrwr,'1900-01-01') >= ISNULL(DC.EndEndr,'1900-01-01') THEN DC.EndBrwr
					 ELSE NULL
				END
			 ELSE NULL
		END AS DODEnd, --loan level
		NULL AS TXCXBegin, --borrower level
		NULL AS TXCXEnd
	FROM
		CLS.scra.DataComparison DC
		INNER JOIN CDW..LN10_LON LN10
			ON DC.BorrSSN = LN10.BF_SSN
			AND DC.Loan = LN10.LN_SEQ
		INNER JOIN
		(	--Adding Active Interest Rate
			SELECT
				LN72.BF_SSN, 
				LN72.LN_SEQ,
				LN72.LD_ITR_EFF_BEG,
				LN72.LD_ITR_EFF_END,
				LN72.LC_INT_RDC_PGM,
				LN72.LR_INT_RDC_PGM_ORG,
				ROW_NUMBER() OVER (PARTITION BY LN72.BF_SSN, LN72.LN_SEQ ORDER BY LD_STA_LON72 DESC) AS SEQ
			FROM
				CDW..LN72_INT_RTE_HST LN72
				INNER JOIN CDW..PD10_PRS_NME PD10 
					ON PD10.DF_PRS_ID = LN72.BF_SSN
				INNER JOIN CLS.scra.DataComparison DC
					ON DC.BorrSSN = PD10.DF_PRS_ID
					AND DC.StatusDate BETWEEN LN72.LD_ITR_EFF_BEG AND LN72.LD_ITR_EFF_END
					AND DC.ActiveRow = 1
					AND 
					(
						DC.BorrActive IS NOT NULL
						OR DC.EndrActive IS NOT NULL
					)
			WHERE
				LN72.LC_STA_LON72 = 'A'
		) LN72 
			ON LN10.BF_SSN = LN72.BF_SSN
			AND LN10.LN_SEQ = LN72.LN_SEQ
			AND LN72.SEQ = 1
		INNER JOIN CDW..DW01_DW_CLC_CLU DW01
			ON LN10.BF_SSN = DW01.BF_SSN
			AND LN10.LN_SEQ = DW01.LN_SEQ
		LEFT JOIN CDW..LN20_EDS LN20
			ON LN10.BF_SSN = LN20.BF_SSN
			AND LN10.LN_SEQ = LN20.LN_SEQ
			AND LN20.LC_STA_LON20 = 'A'
		LEFT JOIN MIN_DATES MN --DATA CHUNK 1 borrower level
			ON DC.BorrSSN = MN.BorrSSN
			AND DC.Loan = MN.Loan
			AND ISNULL(DC.EndrSSN,'') = MN.EndrSSN
		LEFT JOIN MAX_DATES MX --DATA CHUNK 2 borrower level
			ON DC.BorrSSN = MX.BorrSSN
			AND DC.Loan = MX.Loan
			AND ISNULL(DC.EndrSSN,'') = MX.EndrSSN
	WHERE
		DC.ActiveRow = 1
		AND 
		(
			DC.BorrActive IS NOT NULL
			OR DC.EndrActive IS NOT NULL
		)
)
INSERT INTO CLS.scra.ScriptProcessing
(
	BorrSSN,
	DataComparisonId,
	Loan,
	LN72Begin,
	LN72End,
	LN72RegRate,
	LN72SCRA,
	LN10LnAdd,
	LN10Disb,
	LN10CurPri,
	LN10Sta,
	LN10Sub,
	DW01Sta,
	DODBegin,
	DODEnd,
	TXCXBegin,
	TXCXEnd,
	TXCXType,
	BenefitSourceId,
	ScriptAction,
	TS06Indicator,
	TS06Updated,
	TXCXIndicator,
	TXCXUpdated,
	TS0NIndicator,
	TS0NUpdated,
	AAPUpdated,
	CreatedAt,
	CreatedBy
)
SELECT
	NEWDATA.BorrSSN,
	NEWDATA.DataComparisonId,
	NEWDATA.Loan,
	NEWDATA.LN72Begin,
	NEWDATA.LN72End,
	NEWDATA.LN72RegRate,
	NEWDATA.LN72SCRA,
	NEWDATA.LN10LnAdd,
	NEWDATA.LN10Disb,
	NEWDATA.LN10CurPri,
	NEWDATA.LN10Sta,
	NEWDATA.LN10Sub,
	NEWDATA.DW01Sta,
	NEWDATA.DODBegin,
	NEWDATA.DODEnd,
	NEWDATA.TXCXBegin,
	NEWDATA.TXCXEnd,
	NEWDATA.TXCXType,
	NEWDATA.BenefitSourceId,
	NEWDATA.ScriptAction,
	NULL AS TS06Indicator,
	NULL AS TS06Updated,
	NULL AS TXCXIndicator,
	NULL AS TXCXUpdated,
	NULL AS TS0NIndicator,
	NULL AS TS0NUpdated,
	NULL AS AAPUpdated,
	NEWDATA.CreatedAt,
	NEWDATA.CreatedBy
FROM
(
	SELECT DISTINCT
		ASSIGN_DATES.BorrSSN,
		ASSIGN_DATES.DataComparisonId,
		ASSIGN_DATES.StatusDate,
		ASSIGN_DATES.Loan,
		ASSIGN_DATES.LN72Begin,
		ASSIGN_DATES.LN72End,
		ASSIGN_DATES.LN72RegRate,
		ASSIGN_DATES.LN72SCRA,
		ASSIGN_DATES.LN10LnAdd,
		ASSIGN_DATES.LN10Disb,
		ASSIGN_DATES.LN10CurPri,
		ASSIGN_DATES.LN10Sta,
		ASSIGN_DATES.LN10Sub,
		ASSIGN_DATES.DW01Sta,
		ASSIGN_DATES.DODBegin, --loan level
		ASSIGN_DATES.DODEnd, --loan level
		ASSIGN_DATES.TXCXBegin, --borrower level
		ASSIGN_DATES.TXCXEnd, --borrower level
		NULL AS TXCXType,
		CASE WHEN ASSIGN_DATES.DODBegin = MN.BeginBrwr AND ASSIGN_DATES.DODEnd = MX.EndBrwr THEN 1
			 WHEN ASSIGN_DATES.DODBegin = MN.BeginEndr AND ASSIGN_DATES.DODEnd = MX.EndEndr THEN 2
			 ELSE 3
		END AS BenefitSourceId,
		CASE WHEN 
				ASSIGN_DATES.DW01Sta = 12
				OR 
				(
					ASSIGN_DATES.LN10Sta = 'D' 
					AND ASSIGN_DATES.LN10Sub IN (1,2,3,4)
				)
				OR ISNULL(ASSIGN_DATES.LN10Disb,'') > ISNULL(ASSIGN_DATES.DODBegin,'')
				OR 
				(
					ASSIGN_DATES.LN72Begin = ASSIGN_DATES.DODBegin
					AND ASSIGN_DATES.LN72End = ASSIGN_DATES.DODEnd
					AND
					(
						ASSIGN_DATES.LN72SCRA = 1 --"M" value from above CTE
						OR ASSIGN_DATES.LN72RegRate <= 6.00
					)
				)
				OR 
				(
					ISNULL(ASSIGN_DATES.LN72End,'') > ISNULL(ASSIGN_DATES.DODEnd,'')
					AND ASSIGN_DATES.LN72SCRA = 1 --"M" value from above CTE
					AND ASSIGN_DATES.LN10CurPri <= 0.00
				)
				OR 
				(
					ASSIGN_DATES.LN72SCRA <> 1 --"M" value from above CTE
					AND ASSIGN_DATES.LN72RegRate <= 6.00
				)
				OR ISNULL(ASSIGN_DATES.DODEnd,'') < ISNULL(ASSIGN_DATES.LN10Disb,'')
				OR 
				(
					ASSIGN_DATES.LN72RegRate = 6.00
					AND ASSIGN_DATES.LN72SCRA = 1
					AND ISNULL(ASSIGN_DATES.DODEnd,'') <> ISNULL(ASSIGN_DATES.LN72End,'')
					AND ISNULL(ASSIGN_DATES.LN72End,'') >= DATEADD(DAY,31,ASSIGN_DATES.StatusDate)
					AND ASSIGN_DATES.DODEnd = CONVERT(DATE,'20991231')
					AND ASSIGN_DATES.LN72End <> CONVERT(DATE,'99990101')
				)
			THEN 'B'
			 WHEN 
				(
					ISNULL(ASSIGN_DATES.LN72End,'') > ISNULL(ASSIGN_DATES.DODEnd,'')
					AND ASSIGN_DATES.DODEnd <> CONVERT(DATE,'20991231')
					AND ASSIGN_DATES.LN72End <> CONVERT(DATE,'99990101')
					AND ASSIGN_DATES.LN72SCRA = 1 --"M" value from above CTE
					AND ASSIGN_DATES.LN10CurPri > 0.00
				)
				OR 
				(
					ASSIGN_DATES.LN72RegRate < 6.00
					AND ASSIGN_DATES.LN72SCRA = 1 --"M" value from above CTE
				)
			THEN 'E'
			ELSE 'U'
		END AS ScriptAction,
		GETDATE() AS CreatedAt,
		SYSTEM_USER AS CreatedBy
	FROM
		ASSIGN_DATES
		LEFT JOIN MIN_DATES MN --DATA CHUNK 1 borrower level
			ON ASSIGN_DATES.BorrSSN = MN.BorrSSN
			AND ASSIGN_DATES.Loan = MN.Loan
		LEFT JOIN MAX_DATES MX --DATA CHUNK 2 borrower level
			ON ASSIGN_DATES.BorrSSN = MX.BorrSSN
			AND ASSIGN_DATES.Loan = MX.Loan
) NEWDATA
	LEFT JOIN CLS.scra.ScriptProcessing EXISTING_DATA
		ON NEWDATA.BorrSSN = EXISTING_DATA.BorrSSN
		AND NEWDATA.DataComparisonId = EXISTING_DATA.DataComparisonId
		AND NEWDATA.Loan = EXISTING_DATA.Loan
		AND NEWDATA.LN72Begin = EXISTING_DATA.LN72Begin
		AND NEWDATA.LN72End = EXISTING_DATA.LN72End
		AND NEWDATA.LN72RegRate = EXISTING_DATA.LN72RegRate
		AND NEWDATA.LN72SCRA = EXISTING_DATA.LN72SCRA
		AND NEWDATA.LN10LnAdd = EXISTING_DATA.LN10LnAdd
		AND NEWDATA.LN10Disb = EXISTING_DATA.LN10Disb
		AND NEWDATA.LN10CurPri = EXISTING_DATA.LN10CurPri
		AND NEWDATA.LN10Sta = EXISTING_DATA.LN10Sta
		AND COALESCE(NEWDATA.LN10Sub,'') = COALESCE(EXISTING_DATA.LN10Sub,'')--nullable
		AND NEWDATA.DW01Sta = EXISTING_DATA.DW01Sta
		AND NEWDATA.DODBegin = EXISTING_DATA.DODBegin
		AND NEWDATA.DODEnd = EXISTING_DATA.DODEnd
		AND NEWDATA.BenefitSourceId = EXISTING_DATA.BenefitSourceId
		AND NEWDATA.ScriptAction = EXISTING_DATA.ScriptAction
		AND CAST(NEWDATA.CreatedAt AS DATE) = CAST(EXISTING_DATA.CreatedAt AS DATE)
WHERE
	EXISTING_DATA.BorrSSN IS NULL
;

/**************************  ArcAdd processing  **************************/

INSERT INTO CLS.dbo.ArcAddProcessing
(
	ArcTypeId,
	AccountNumber,
	ARC,
	ScriptId,
	ProcessOn,
	Comment,
	IsReference,
	IsEndorser,
	ProcessingAttempts,
	CreatedAt,
	CreatedBy
)
SELECT
	NEWDATA.ArcTypeId,
	NEWDATA.AccountNumber,
	NEWDATA.ARC,
	NEWDATA.ScriptId,
	NEWDATA.ProcessOn,
	NEWDATA.Comment,
	NEWDATA.IsReference,
	NEWDATA.IsEndorser,
	NEWDATA.ProcessingAttempts,
	NEWDATA.CreatedAt,
	NEWDATA.CreatedBy
FROM
(
	SELECT DISTINCT
		2 AS ArcTypeId,
		PD10.DF_SPE_ACC_ID AS AccountNumber,
		'ASCRA' AS ARC,
		'UTNWS81' AS ScriptId,
		GETDATE() AS ProcessOn,
		CASE WHEN DC.BorrActive = 1 AND ISNULL(DC.EndrActive,0) = 0
			THEN 'Active Military Borrower. Borrower''s Active Duty Begin Date: ' + CAST(DC.BeginBrwr AS VARCHAR(10)) + ', Borrower''s Active Duty End Date: ' + CAST(DC.EndBrwr AS VARCHAR(10))
			 WHEN ISNULL(DC.BorrActive,0) = 0 AND DC.EndrActive = 1
			THEN 'Endorser''s Active Duty Begin Date: ' + CAST(DC.BeginEndr AS VARCHAR(10)) + ', Endorser''s Active Duty End Date: ' + CAST(DC.EndEndr AS VARCHAR(10))
			 WHEN DC.BorrActive = 1 AND DC.EndrActive = 1
			THEN 'Active Military Borrower. Borrower''s Active Duty Begin Date: ' + CAST(DC.BeginBrwr AS VARCHAR(10)) 
				+ ', Borrower''s Active Duty End Date: ' + CAST(DC.EndBrwr AS VARCHAR(10)) 
				+ '. Endorser''s Active Duty Begin Date: ' + CAST(DC.BeginEndr AS VARCHAR(10)) 
				+ ', Endorser''s Active Duty End Date: ' + CAST(DC.EndEndr AS VARCHAR(10))
			ELSE 'ERROR'
		END AS Comment,
		0 AS IsReference,
		0 AS IsEndorser,
		0 AS ProcessingAttempts,
		GETDATE() AS CreatedAt,
		'SCRAFED' AS CreatedBy,
		SUM(DC.LoanBalance) OVER(PARTITION BY DC.BorrSSN) AS sum_LoanBalance
	FROM
		CLS.scra.DataComparison DC
		INNER JOIN CDW..PD10_PRS_NME PD10
			ON DC.BorrSSN = PD10.DF_PRS_ID
	WHERE
		DC.ActiveRow = 1
		AND
		(
			DC.BorrActive = 1
			OR DC.EndrActive = 1
		)
) NEWDATA
LEFT JOIN CLS.dbo.ArcAddProcessing EXISTING_DATA
	ON NEWDATA.ArcTypeId = EXISTING_DATA.ArcTypeId
	AND NEWDATA.AccountNumber = EXISTING_DATA.AccountNumber
	AND NEWDATA.ARC  = EXISTING_DATA.ARC
	AND NEWDATA.ScriptId = EXISTING_DATA.ScriptId
	AND CAST(NEWDATA.ProcessOn AS DATE) = CAST(EXISTING_DATA.ProcessOn AS DATE)
	AND ISNULL(NEWDATA.Comment,'') = ISNULL(EXISTING_DATA.Comment,'')--nullable
	AND NEWDATA.IsReference = EXISTING_DATA.IsReference
	AND NEWDATA.IsEndorser = EXISTING_DATA.IsEndorser
	AND CAST(NEWDATA.CreatedAt AS DATE) = CAST(EXISTING_DATA.CreatedAt AS DATE)
WHERE
	EXISTING_DATA.AccountNumber IS NULL
	AND NEWDATA.sum_LoanBalance > 0.00
;

/**************************  input groups for email & print processing  **************************/

DROP TABLE IF EXISTS ##SCRA_INPUT_FED;

CREATE TABLE ##SCRA_INPUT_FED (BorrSSN CHAR(9));

WITH max_SP AS
(--get most recent record
	SELECT 
		BorrSSN,
		MAX(CreatedAt) AS max_CreatedAt
	FROM 
		CLS.scra.ScriptProcessing
	GROUP BY
		BorrSSN
)
INSERT INTO ##SCRA_INPUT_FED (BorrSSN)
SELECT DISTINCT
	BOTHGROUPS.BorrSSN
FROM
	(
		SELECT DISTINCT
			SP.*
		FROM
			CLS.scra.ScriptProcessing SP
			LEFT JOIN
			(--exclude this borrower pop
				SELECT DISTINCT
					AY10.BF_SSN,
					SP.Loan
				FROM
					CDW..AY10_BR_LON_ATY AY10
					INNER JOIN CLS.scra.ScriptProcessing SP
						ON AY10.BF_SSN = SP.BorrSSN
				WHERE
					AY10.PF_REQ_ACT = 'SCRAN'
					AND AY10.LC_STA_ACTY10 = 'A'
					AND	
					(
						ISNULL(AY10.LD_ATY_REQ_RCV,'') > ISNULL(SP.TXCXBegin,'') --group 1: Exclude if the borrower has received a letter since the Active Duty Begin Date
						OR ISNULL(AY10.LD_ATY_REQ_RCV,'') > ISNULL(SP.TXCXEnd,'') --group 2: Exclude if borrower has received a letter since the Active Duty End date
					)
			) EXCLUDE
				ON SP.BorrSSN = EXCLUDE.BF_SSN
				AND SP.Loan = EXCLUDE.Loan
		WHERE
			EXCLUDE.BF_SSN IS NULL
			AND SP.LN10Sta = 'R'
			AND SP.LN10CurPri > 0.00
			AND 
			(
				SP.LN72RegRate < 6.00
				OR 
				(
					SP.LN72RegRate = 6.00
					AND SP.LN72SCRA = 0
				)
				OR SP.LN10Disb > SP.DODBegin
			)
			AND 
			(
				(--group 1: Active Duty & DOD Begin in past 45 days
					ISNULL(SP.TXCXBegin,'') >= DATEADD(DAY,-45,CONVERT(DATE,GETDATE()))
					AND ISNULL(SP.TXCXBegin,'') <= CONVERT(DATE,GETDATE())
				)
				OR
				(--group 2: exiting Active Duty in past 45 days
					ISNULL(SP.TXCXEnd,'') >= DATEADD(DAY,-45,CONVERT(DATE,GETDATE()))
					AND ISNULL(SP.TXCXEnd,'') <= CONVERT(DATE,GETDATE())
				)
			)
	) BOTHGROUPS
	LEFT JOIN --to exclude if borrower has loans that are both approved/eligible and denied/ineligible for SCRA benefits
	(
		(
			SELECT
				SP.BorrSSN
			FROM
				CLS.scra.ScriptProcessing SP
				INNER JOIN max_SP
					ON SP.BorrSSN = max_SP.BorrSSN
					AND SP.CreatedAt = max_SP.max_CreatedAt
			WHERE 
				SP.LN72RegRate > 6.000--eligible/approved
				OR 
				(
					SP.LN72RegRate = 6.000
					AND SP.LN72SCRA = 1 --On Military Special Rate Indicator
				)

			INTERSECT --gets borrowers that have both eligible and ineligible loans based on RegRate since SCRA lowers rates to 6%
		
			SELECT 
				SP.BorrSSN
			FROM
				CLS.scra.ScriptProcessing SP
				INNER JOIN max_SP
					ON SP.BorrSSN = max_SP.BorrSSN
					AND SP.CreatedAt = max_SP.max_CreatedAt
			WHERE 
				SP.LN72RegRate <= 6.000--ineligible/denied
		)

		UNION ALL
			
		(
			SELECT 
				SP.BorrSSN
			FROM 
				CLS.scra.ScriptProcessing SP
				INNER JOIN max_SP
					ON SP.BorrSSN = max_SP.BorrSSN
					AND SP.CreatedAt = max_SP.max_CreatedAt
			WHERE 
				SP.LN10Disb > SP.DODBegin --denied (loan disbursed after active duty begin date)
		
			INTERSECT --gets borrowers that have both approved and denied loans based on when loan was disbursed

			SELECT 
				SP.BorrSSN
			FROM 
				CLS.scra.ScriptProcessing SP
				INNER JOIN max_SP
					ON SP.BorrSSN = max_SP.BorrSSN
					AND SP.CreatedAt = max_SP.max_CreatedAt
			WHERE 
				SP.LN10Disb <= SP.DODBegin --approved (loan disbursed before active duty begin date)
		)
	) YESANDNO
		ON BOTHGROUPS.BorrSSN = YESANDNO.BorrSSN
WHERE
	YESANDNO.BorrSSN IS NULL --excludes borrowers that are both approved/eligible and denied/ineligible for SCRA benefits
;

--select * from ##SCRA_INPUT_FED

/**************************  email processing  **************************/

DECLARE @EmailCcampaignId SMALLINT = (SELECT EmailCampaignId FROM CLS.emailbtcf.EmailCampaigns WHERE SasFile = 'SCRA_FED*');

INSERT INTO	CLS.emailbtcf.CampaignData
(
	EmailCampaignId,
	Recipient,
	AccountNumber,
	FirstName,
	LastName,
	AddedAt,
	AddedBy
)
SELECT
	NEWDATA.EmailCampaignId,
	NEWDATA.Recipient,
	NEWDATA.AccountNumber,
	NEWDATA.FirstName,
	NEWDATA.LastName,
	NEWDATA.AddedAt,
	NEWDATA.AddedBy
FROM
(
	SELECT DISTINCT
		@EmailCcampaignId AS EmailCampaignId,
		COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM,'sshelp@utahsbr.edu') AS Recipient,
		PD10.DF_SPE_ACC_ID AS AccountNumber,
		PD10.DM_PRS_1 AS FirstName,
		PD10.DM_PRS_LST AS LastName,
		GETDATE() AS AddedAt,
		SUSER_SNAME() AS AddedBy
	FROM
		##SCRA_INPUT_FED SI
		INNER JOIN CDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = SI.BorrSSN
		LEFT JOIN CDW..PH05_CNC_EML PH05 
			ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
			AND PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
		LEFT JOIN
		( -- email address
			SELECT 
				DF_PRS_ID, 
				Email.EM AS [ALT_EM],
				ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) AS [EmailPriority] -- number in order of Email.PriorityNumber 
			FROM 
			( 
				SELECT 
					PD32.DF_PRS_ID,
					PD32.DX_ADR_EML AS [EM],
					CASE WHEN DC_ADR_EML = 'H' THEN 1 -- home
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
) NEWDATA
LEFT JOIN CLS.emailbtcf.CampaignData EXISTING_DATA
	ON NEWDATA.EmailCampaignId = EXISTING_DATA.EmailCampaignId
	AND NEWDATA.Recipient = EXISTING_DATA.Recipient
	AND NEWDATA.AccountNumber = EXISTING_DATA.AccountNumber
	AND NEWDATA.FirstName = EXISTING_DATA.FirstName
	AND NEWDATA.LastName = EXISTING_DATA.LastName
	AND CAST(NEWDATA.AddedAt AS DATE) = CAST(EXISTING_DATA.AddedAt AS DATE)
	AND NEWDATA.AddedBy = EXISTING_DATA.AddedBy
WHERE
	EXISTING_DATA.AccountNumber IS NULL
;

/**************************  print processing  **************************/

DECLARE @ScriptDataId INT = (SELECT ScriptDataId FROM CLS.[print].ScriptData WHERE ScriptId = 'UTNW021');

INSERT INTO CLS.[print].PrintProcessing
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
	AddedAt
)
SELECT
	NEWDATA.AccountNumber,
	NEWDATA.EmailAddress,
	NEWDATA.ScriptDataId,
	NEWDATA.SourceFile,
	NEWDATA.LetterData,
	NEWDATA.CostCenter,
	NEWDATA.DoNotProcessEcorr,
	NEWDATA.OnEcorr,
	NEWDATA.ArcNeeded,
	NEWDATA.ImagingNeeded,
	NEWDATA.AddedBy,
	NEWDATA.AddedAt
FROM
(
	SELECT DISTINCT
		PD10.DF_SPE_ACC_ID AS AccountNumber,
		COALESCE(PH05.DX_CNC_EML_ADR, PD32.ALT_EM,'Ecorr@MyCornerStoneLoan.org') AS EmailAddress,
		@ScriptDataId AS ScriptDataId,
		'UTNW021' AS SourceFile,
		'"' + COALESCE(LTRIM(RTRIM(PD10.DF_SPE_ACC_ID)),'')
			+ '","' + COALESCE(LTRIM(RTRIM(PD10.DM_PRS_1)),'')
			+ '","' + COALESCE(LTRIM(RTRIM(PD10.DM_PRS_LST)),'')
			+ '","' + COALESCE(LTRIM(RTRIM(PD30.DX_STR_ADR_1)),'')
			+ '","' + COALESCE(LTRIM(RTRIM(PD30.DX_STR_ADR_2)),'')
			+ '","' + COALESCE(LTRIM(RTRIM(PD30.DM_CT)),'')
			+ '","' + COALESCE(LTRIM(RTRIM(PD30.DC_DOM_ST)),'')
			+ '","' + COALESCE(LTRIM(RTRIM(PD30.DF_ZIP_CDE)),'')
			+ '","' + COALESCE(LTRIM(RTRIM(PD30.DM_FGN_CNY)),'')
			+ '","' + COALESCE(LTRIM(RTRIM(PD30.DM_FGN_ST)),'')
			+ '","' + COALESCE(LTRIM(RTRIM(CentralData.dbo.CreateACSKeyline(SI.BorrSSN, 'B', PD30.DC_ADR))),'')
			+ '","' + 'MA4481"' AS LetterData,
		'MA4481' AS CostCenter,
		SD.DoNotProcessEcorr,
		IIF(PH05.DI_CNC_ELT_OPI = 'Y', 1, 0) AS OnEcorr, --Contact person's e-letter opt in indicator
		IIF(SDM.ArcId IS NULL, 0, 1) AS ArcNeeded,
		IIF(SD.DocIdId IS NULL, 0, 1) AS ImagingNeeded,
		SYSTEM_USER AS AddedBy,
		GETDATE() AS AddedAt
	FROM
		##SCRA_INPUT_FED SI
		INNER JOIN CDW..PD10_PRS_NME PD10
			ON PD10.DF_PRS_ID = SI.BorrSSN
		INNER JOIN CDW..PD30_PRS_ADR PD30
			ON PD10.DF_PRS_ID = PD30.DF_PRS_ID
			AND PD30.DI_VLD_ADR = 'Y'
		INNER JOIN CLS.[print].ScriptData SD
			ON SD.ScriptId = 'UTNW021'
			AND SD.ScriptDataId = @ScriptDataId
		INNER JOIN CLS.[print].ArcScriptDataMapping SDM
			ON SDM.ScriptDataId = SD.ScriptDataId
		LEFT JOIN CDW..PH05_CNC_EML PH05 
			ON PH05.DF_SPE_ID = PD10.DF_SPE_ACC_ID
			AND PH05.DI_VLD_CNC_EML_ADR = 'Y' -- valid email
		LEFT JOIN
		( -- email address
			SELECT 
				DF_PRS_ID, 
				Email.EM AS [ALT_EM],
				ROW_NUMBER() OVER (PARTITION BY Email.DF_PRS_ID ORDER BY Email.PriorityNumber) AS [EmailPriority] -- number in order of Email.PriorityNumber 
			FROM 
			( 
				SELECT 
					PD32.DF_PRS_ID,
					PD32.DX_ADR_EML AS [EM],
					CASE WHEN DC_ADR_EML = 'H' THEN 1 -- home
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
) NEWDATA
LEFT JOIN CLS.[print].PrintProcessing EXISTING_DATA
	ON NEWDATA.AccountNumber = EXISTING_DATA.AccountNumber
	AND NEWDATA.EmailAddress = EXISTING_DATA.EmailAddress
	AND NEWDATA.ScriptDataId = EXISTING_DATA.ScriptDataId
	AND NEWDATA.SourceFile = EXISTING_DATA.SourceFile
	AND NEWDATA.LetterData = EXISTING_DATA.LetterData
	AND NEWDATA.CostCenter = EXISTING_DATA.CostCenter
	AND NEWDATA.DoNotProcessEcorr = EXISTING_DATA.DoNotProcessEcorr
	AND NEWDATA.OnEcorr = EXISTING_DATA.OnEcorr
	AND NEWDATA.ArcNeeded = EXISTING_DATA.ArcNeeded
	AND NEWDATA.ImagingNeeded = EXISTING_DATA.ImagingNeeded
	AND NEWDATA.AddedBy = EXISTING_DATA.AddedBy
	AND CAST(NEWDATA.AddedAt AS DATE) = CAST(EXISTING_DATA.AddedAt AS DATE)
WHERE
	EXISTING_DATA.AccountNumber IS NULL
;
