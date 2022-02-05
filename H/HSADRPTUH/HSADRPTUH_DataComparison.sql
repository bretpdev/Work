/****************************** INSERT INTO DATA COMPARISON ******************************/

--TO LOAD BY FILE:

--variables to load files based on Active Duty Status Date
--NOTE: Historical SCRA files date back to February 2015. 
--Run each month separately beginning with Month=2 and Year=2015, cycling up month-by-month and year-by-year incrementally and consecutively timewise until September 2020.

DECLARE @MONTH TINYINT = 9,   --range 1-12
		@YEAR SMALLINT = 2020; --range 2015-2020

-------------------------------------------------------

--TO LOAD BY INDIVIDUAL:

--comment out any portions of the query that have @MONTH and @YEAR
--uncomment any portions of the query that have @SSN

--DECLARE @SSN VARCHAR(9) = '';

-------------------------------------------------------
BEGIN TRY
BEGIN TRANSACTION;

INSERT INTO ULS.hsadrptuh.DataComparison
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
		CASE 
			WHEN 
				BORR.ActiveDutyOnActiveDutyStatusDt = 'N' 
				AND BORR.LeftActiveDutyLE367DaysFromActiveDutyStatusDt = 'Y' 
				AND BORR.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'N' 
			THEN 0
			WHEN
				BORR.ActiveDutyOnActiveDutyStatusDt IN ('X','Y')
				OR (
						BORR.ActiveDutyOnActiveDutyStatusDt = 'N' 
						AND BORR.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'Y'
					) 
			THEN 1
			ELSE NULL --doesn't exist in DOD database
		END AS BorrActive,
		ENDR.EndrSSN,
		CASE
			WHEN 
				ENDR.ActiveDutyOnActiveDutyStatusDt = 'N' 
				AND ENDR.LeftActiveDutyLE367DaysFromActiveDutyStatusDt = 'Y' 
				AND ENDR.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt = 'N' 
			THEN 0
			WHEN
				ENDR.ActiveDutyOnActiveDutyStatusDt IN ('X','Y')
				OR (
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
			ULS.hsadrptuh.ScraHistoricalFiles DOD
			INNER JOIN UDW..LN10_LON LN10 --ULS.hsadrptuh.LN10_LON LN10 --opsdev
				ON DOD.SSN = LN10.BF_SSN
		WHERE
			DOD.ActiveDutyOnActiveDutyStatusDt != 'Z'
			AND DOD.LeftActiveDutyLE367DaysFromActiveDutyStatusDt != 'Z'
			AND DOD.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt != 'Z'
			AND LN10.LA_CUR_PRI > 0.00
			AND MONTH(CONVERT(DATE,CONVERT(VARCHAR(8),ActiveDutyStatusDate))) = @MONTH --LOAD BY FILE
			AND YEAR(CONVERT(DATE,CONVERT(VARCHAR(8),ActiveDutyStatusDate))) = @YEAR   --LOAD BY FILE
			--AND DOD.SSN = @SSN --LOAD BY INDIVIDUAL
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
			ULS.hsadrptuh.ScraHistoricalFiles DOD
			INNER JOIN UDW..LN20_EDS LN20 --ULS.hsadrptuh.LN20_EDS LN20 opsdev
				ON DOD.SSN = LN20.LF_EDS
			INNER JOIN UDW..LN10_LON LN10 --ULS.hsadrptuh.LN10_LON LN10 opsdev
				ON LN20.BF_SSN = LN10.BF_SSN
				AND LN20.LN_SEQ = LN10.LN_SEQ
		WHERE
			DOD.ActiveDutyOnActiveDutyStatusDt != 'Z'
			AND DOD.LeftActiveDutyLE367DaysFromActiveDutyStatusDt != 'Z'
			AND DOD.NotifiedOfActiveDutyRecallOnActiveDutyStatusDt != 'Z'
			AND LN10.LA_CUR_PRI > 0.00
			AND MONTH(CONVERT(DATE,CONVERT(VARCHAR(8),ActiveDutyStatusDate))) = @MONTH --LOAD BY FILE
			AND YEAR(CONVERT(DATE,CONVERT(VARCHAR(8),ActiveDutyStatusDate))) = @YEAR   --LOAD BY FILE
			--AND DOD.SSN = @SSN --LOAD BY INDIVIDUAL
	) ENDR
		ON BORR.BorrSSN = ENDR.BorrSSN
		AND BORR.Loan = ENDR.Loan
		AND BORR.ActiveDutyStatusDate = ENDR.ActiveDutyStatusDate
) NEWDATA
LEFT JOIN ULS.hsadrptuh.DataComparison EXISTING_DATA
	ON NEWDATA.ActiveRow = EXISTING_DATA.ActiveRow
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
--SELECT StatusDate,COUNT(StatusDate) FROM ULS.hsadrptuh.DataComparison GROUP BY StatusDate --TEST

COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	ROLLBACK TRANSACTION;
	PRINT '!!!  Transaction NOT committed.  Contact a member of Application Development to have this error corrected.';
	THROW;
END CATCH;