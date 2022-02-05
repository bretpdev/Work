/*********************************************
	TIMESHEET HOURS AGGREGATOR

	This script will read your current quarter's timesheet and list a daily aggregate of 
	the hours entered on your timesheet as well as the time recorded in all NeedHelp tickets.
	Just run as-is; no need to change any parts of this script.
**********************************************/
ODS _ALL_ CLOSE;
LIBNAME CSYS ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\CSYS.dsn; update_lock_typ=nolock; bl_keepnulls=no");
%LET WINDOWSUSERNAME = &SYSUSERID;

*get timesheet and folder name;
PROC SQL;
	CREATE TABLE TIMENAMES AS
		SELECT 
			 SqlUserId
			,WindowsUserName
			,FirstName
			,LastName
			,CASE
				WHEN WindowsUserName IN 
				(
					'ccole',
					'cmccomb',
					'jgutierrez',
					'rbigelow'
				)
				THEN FirstName
				WHEN WindowsUserName = 'dphillips'
				THEN 'Deb'
				WHEN WindowsUserName = 'ewalker'
				THEN CATX(' ', 'Daniel', FirstName, LastName)
				WHEN WindowsUserName = 'jlwright'
				THEN 'Josh'
				WHEN WindowsUserName = 'nburnham'
				THEN CATX(' ', 'Nick', LastName)
				WHEN WindowsUserName = 'sostler'
				THEN CATX(' ', 'Steve', LastName)
				ELSE CATX(' ', FirstName, LastName)
			END AS TIMESHEETNAME
			,CASE
				WHEN WindowsUserName IN
				(
					'jnolasco',
					'jkramer',
					'jkieschnick',
					'jdavis',
					'mjones',
					'nnguyen',
					'pmalone',
					'tjohnson'
				)
				THEN CATX(' ', FirstName, LastName)
				WHEN WindowsUserName = 'dphillips'
				THEN 'Deb'
				WHEN WindowsUserName = 'jlwright'
				THEN 'Josh'
				WHEN WindowsUserName = 'nburnham'
				THEN 'Nick'
				WHEN WindowsUserName = 'sostler'
				THEN 'Steve'
				WHEN WindowsUserName = 'ssibulo'
				THEN 'Jake Sibulo'
				ELSE FirstName
			END AS FOLDERNAME
		FROM
			CSYS.SYSA_DAT_Users
		WHERE
			Status = 'Active'
			AND BusinessUnit IN 
			(
				 30 /*--application development*/
				,34 /*--strategic projects*/
				,35 /*--systems support*/
				,49 /*--operations*/
			)
			AND SqlUserId NOT IN
			(
				 83 /*--batch scripts*/
				,1119 /*--Brenda Cox*/
			)
			AND WindowsUserName = "&WINDOWSUSERNAME"
;QUIT;
DATA _NULL_;
	SET TIMENAMES;
	CALL SYMPUT('FOLDERNAME', CATT(FOLDERNAME));
	CALL SYMPUT('TIMESHEETNAME', CATT(TIMESHEETNAME));
RUN;
%PUT &FOLDERNAME;
%PUT &TIMESHEETNAME;

*calculate current quarter;
DATA CurrentQuarter;
	CURR_DATE = TODAY();
	CURR_MONTH = MONTH(CURR_DATE);
	CURR_YEAR = YEAR(CURR_DATE);
	FY_YEAR = YEAR(CURR_DATE) + 1;

	* creates &QUARTER variable: ;
	IF CURR_MONTH IN (7,8,9) THEN CURR_QUARTER = ' Q1 ';
	IF CURR_MONTH IN (10,11,12) THEN CURR_QUARTER = ' Q2 ';
	IF CURR_MONTH IN (1,2,3) THEN CURR_QUARTER = ' Q3 ';
	IF CURR_MONTH IN (4,5,6) THEN CURR_QUARTER = ' Q4 ';
	
	IF CURR_MONTH IN (7,8,9) THEN QUARTER = CAT('FY ', FY_YEAR, CURR_QUARTER, 'Time Tracking -');
	IF CURR_MONTH IN (10,11,12) THEN QUARTER = CAT('FY ', FY_YEAR, CURR_QUARTER, 'Time Tracking -');
	IF CURR_MONTH IN (1,2,3) THEN QUARTER = CAT('FY ', CURR_YEAR, CURR_QUARTER, 'Time Tracking -');
	IF CURR_MONTH IN (4,5,6) THEN QUARTER = CAT('FY ', CURR_YEAR, CURR_QUARTER, 'Time Tracking -');

	CALL SYMPUT('QUARTER', CATT(QUARTER)); 

	* creates &BEGIN_QUARTER variable: ;
	IF CURR_MONTH IN (7,8,9) THEN BEGIN_QUARTER = MDY(7,1,CURR_YEAR);
	IF CURR_MONTH IN (10,11,12) THEN BEGIN_QUARTER = MDY(10,1,CURR_YEAR);
	IF CURR_MONTH IN (1,2,3) THEN BEGIN_QUARTER = MDY(1,1,CURR_YEAR);
	IF CURR_MONTH IN (4,5,6) THEN BEGIN_QUARTER = MDY(4,1,CURR_YEAR);
	CALL SYMPUT('BEGIN_QUARTER', PUT(BEGIN_QUARTER,DATE9.)); 
	FORMAT CURR_DATE BEGIN_QUARTER DATE9.;
RUN;

*get timesheet data from current spreadsheet;
%LET FILEPATH = Q:\Support Services\Time tracking;
LIBNAME TYMSHEET "&FILEPATH\&FOLDERNAME\&QUARTER &TIMESHEETNAME..xlsx";
DATA TYMSHEET;
	SET TYMSHEET.'Sheet1$'N (KEEP = DATE HOURS) ;
	WHERE DATE ^= '01JAN2013'D AND DATE ^= .;
	LENGTH WindowsUserName $50.;
	WindowsUserName = "&WindowsUserName";
RUN;
LIBNAME TYMSHEET CLEAR;

*get NeedHelp hours;
LIBNAME RPT ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\Reporting.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME NHU ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\NeedHelpUheaa.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME NHC ODBC %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\NeedHelpCornerStone.dsn; update_lock_typ=nolock; bl_keepnulls=no");
PROC SQL;
	CREATE TABLE NeedHelpTimeTracking AS
		SELECT
			TT.SqlUserID,
			TT.TicketID,
			NH.Subject,
			INPUT(SUBSTR(PUT(TT.StartTime, DATETIME22.3),1,9), DATE9.) AS TaskDate FORMAT = DATE9.,
			(TT.EndTime - TT.StartTime)/3600 AS Hours,
			'Need Help UHEAA Ticket' AS TaskType,
			CU.WindowsUserName
		FROM
			RPT.TimeTracking TT
			INNER JOIN CSYS.SYSA_DAT_Users CU
				ON TT.SqlUserId = CU.SqlUserID
			INNER JOIN NHU.DAT_Ticket NH
				ON TT.TicketID = NH.Ticket
				AND TT.Region = 'uheaa'
		WHERE					
			TT.EndTime IS NOT NULL
			AND TT.StartTime >= "&BEGIN_QUARTER"D
			AND NH.TicketCode NOT IN ('FAC','FAR','FTRANS')
			AND CU.WindowsUserName = "&WINDOWSUSERNAME"

		UNION

		SELECT
			TT.SqlUserID,
			TT.TicketID,
			NH.Subject,
			INPUT(SUBSTR(PUT(TT.StartTime, DATETIME22.3),1,9), DATE9.) AS TaskDate FORMAT = DATE9.,
			(TT.EndTime - TT.StartTime)/3600 AS Hours,
			'Need Help CornerStone Ticket' AS TaskType,
			CU.WindowsUserName
		FROM
			RPT.TimeTracking TT
			INNER JOIN CSYS.SYSA_DAT_Users CU
				ON TT.SqlUserId = CU.SqlUserID
			INNER JOIN NHC.DAT_Ticket NH
				ON TT.TicketID = NH.Ticket
				AND TT.Region ^= 'uheaa'
		WHERE					
			TT.EndTime IS NOT NULL
			AND TT.StartTime >= "&BEGIN_QUARTER"D
			AND NH.TicketCode NOT IN ('FAC','FAR','FTRANS')
			AND CU.WindowsUserName = "&WINDOWSUSERNAME"
;QUIT;
LIBNAME RPT CLEAR;
LIBNAME NHU CLEAR;
LIBNAME NHC CLEAR;
LIBNAME CSYS CLEAR;

ODS HTML;
TITLE 'Daily Aggregated Hours - ' "&TIMESHEETNAME" ;
PROC SQL NUMBER;
	SELECT
		 COALESCE(SH.DATE,NH.DATE) AS Date FORMAT = WEEKDATE17.
		,SH.Spreadsheet_Hrs
		,NH.NeedHelp_Hrs
		,SUM(SH.Spreadsheet_Hrs,NH.NeedHelp_Hrs) AS Total_Hrs
		,CASE
			WHEN SUM(SH.Spreadsheet_Hrs,NH.NeedHelp_Hrs)= 7.5
			THEN .
			WHEN 7.5-(SUM(SH.Spreadsheet_Hrs,NH.NeedHelp_Hrs)) < 0.0000009
			THEN .
			ELSE 7.5-(SUM(SH.Spreadsheet_Hrs,NH.NeedHelp_Hrs)) 
		END AS 'Hrs_to_7.5'n 
	FROM
		(	
			SELECT
				 WindowsUserName
				,DATE
				,SUM(HOURS) AS Spreadsheet_Hrs
			FROM
				TYMSHEET
			GROUP BY
				 DATE
				,WindowsUserName
		) SH
		LEFT JOIN
		(
			SELECT
				 WindowsUserName
				,TaskDate AS DATE
				,SUM(HOURS) AS NeedHelp_Hrs
			FROM
				NeedHelpTimeTracking
			GROUP BY
				 TaskDate
				,WindowsUserName
		) NH
			ON SH.DATE = NH.DATE
			AND SH.WindowsUserName = NH.WindowsUserName
	ORDER BY
		Date DESC
;QUIT;
TITLE;
