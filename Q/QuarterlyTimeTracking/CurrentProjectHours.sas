/*********************************************
	CURRENT PROJECT HOURS REPORT

	This script will read your current quarter's timesheet and list an aggregate 
	by project of the hours entered on your past and current timesheets.
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
					'avillont',
					'aisom',
					'jnolasco',
					'jkramer',
					'jkieschnick',
					'jdavis',
					'jhyde',
					'kmurphy',
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

*get timesheet data from past quarters;
LIBNAME SQL_NOC ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\CSYS.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= DBO;
DATA NOCHOUSE;
	SET	SQL_NOC.COST_DAT_TimeTracking
		(DROP = GenericMeetings
				BatchScripts
				FsaCr
				BillingScript
				ConversionActivities
				CostCenter
				CostCenterId
		);
	WHERE 
		Agent = "&WINDOWSUSERNAME"
		AND TaskDate > '01JUL2018'D;
RUN;
LIBNAME SQL_NOC CLEAR;

*get timesheet data from current spreadsheet;
%LET FILEPATH = Q:\Support Services\Time tracking;
LIBNAME TYMSHEET "&FILEPATH\&TIMESHEETNAME\&QUARTER &TIMESHEETNAME..xlsx";
DATA TYMSHEET;
	SET TYMSHEET.'Sheet1$'N;
	WHERE DATE ^= '01JAN2013'D AND DATE ^= .;
RUN;
LIBNAME TYMSHEET CLEAR;

*get estimates from Jarom's app;
LIBNAME APPDEV ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\AppDev.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= DBO;
DATA APPDEV_ESTIMATES;
	SET	APPDEV.ESTIMATES;
	WHERE Employee = "&WINDOWSUSERNAME"	;
RUN;
LIBNAME APPDEV CLEAR;

*calculate SASR hours;
PROC SQL;
	CREATE TABLE TIMETRAX AS
	SELECT
		 E.RequestType
		,E.RequestNumber
		,E.EstimatedHours
		,E.TestingFixes
		,E.AdditionalHrs
		,(E.EstimatedHours + COALESCE(E.TestingFixes,0) + COALESCE(E.AdditionalHrs,0)) AS TOTAL_ESTIMATED_HRS
		,COALESCE(T_SAS.T_SAS_WORKED_HRS,0) + COALESCE(N_SAS.N_SAS_WORKED_HRS,0) AS SAS_WORKED_HRS
		,COALESCE(T_SR.T_SR_WORKED_HRS,0) + COALESCE(N_SR.N_SR_WORKED_HRS,0) AS SR_WORKED_HRS
		,COALESCE(T_LTS.T_LTS_WORKED_HRS,0) + COALESCE(N_LTS.N_LTS_WORKED_HRS,0) AS LTS_WORKED_HRS
		,COALESCE(T_PMD.T_PMD_WORKED_HRS,0) + COALESCE(N_PMD.N_PMD_WORKED_HRS,0) AS PMD_WORKED_HRS
		,COALESCE(T_PROJECT.T_PJ_WORKED_HRS,0) + COALESCE(N_PROJECT.N_PJ_WORKED_HRS,0) AS PJ_WORKED_HRS
	FROM
		APPDEV_ESTIMATES E
		INNER JOIN
		(
			SELECT
				 RequestType
				,RequestNumber
				,MAX(CreatedAt) AS CreatedAt
			FROM
				APPDEV_ESTIMATES 
			GROUP BY 
				 RequestType
				,RequestNumber
		) MAX_E
			ON E.RequestType = MAX_E.RequestType
			AND E.RequestNumber = MAX_E.RequestNumber
			AND E.CreatedAt = MAX_E.CreatedAt
		LEFT JOIN 
		(
			SELECT
				SASR__
				,SUM(HOURS) AS T_SAS_WORKED_HRS
			FROM
				TYMSHEET
			GROUP BY
				SASR__
		) T_SAS
			ON INPUT(E.RequestNumber,BEST12.) = T_SAS.SASR__
		LEFT JOIN
		(
			SELECT
				SR__
				,SUM(HOURS) AS T_SR_WORKED_HRS
			FROM
				TYMSHEET
			GROUP BY
				SR__
		) T_SR
			ON INPUT(E.RequestNumber,BEST12.) = T_SR.SR__
		LEFT JOIN
		(
			SELECT
				LTS__
				,SUM(HOURS) AS T_LTS_WORKED_HRS
			FROM
				TYMSHEET
			GROUP BY
				LTS__
		) T_LTS
			ON INPUT(E.RequestNumber,BEST12.) = T_LTS.LTS__
		LEFT JOIN
		(
			SELECT
				PMD__
				,SUM(HOURS) AS T_PMD_WORKED_HRS
			FROM
				TYMSHEET
			GROUP BY
				PMD__
		) T_PMD
			ON INPUT(E.RequestNumber,BEST12.) = T_PMD.PMD__
		LEFT JOIN
		(
			SELECT
				PROJECT__
				,SUM(HOURS) AS T_PJ_WORKED_HRS
			FROM
				TYMSHEET
			GROUP BY
				PROJECT__
		) T_PROJECT
			ON INPUT(E.RequestNumber,BEST12.) = T_PROJECT.PROJECT__
		LEFT JOIN 
		(
			SELECT
				SASR
				,SUM(HOURS) AS N_SAS_WORKED_HRS
			FROM
				NOCHOUSE
			GROUP BY
				SASR
		) N_SAS
			ON INPUT(E.RequestNumber,BEST12.) = N_SAS.SASR
		LEFT JOIN
		(
			SELECT
				SR
				,SUM(HOURS) AS N_SR_WORKED_HRS
			FROM
				NOCHOUSE
			GROUP BY
				SR
		) N_SR
			ON INPUT(E.RequestNumber,BEST12.) = N_SR.SR
		LEFT JOIN
		(
			SELECT
				LTS
				,SUM(HOURS) AS N_LTS_WORKED_HRS
			FROM
				NOCHOUSE
			GROUP BY
				LTS
		) N_LTS
			ON INPUT(E.RequestNumber,BEST12.) = N_LTS.LTS
		LEFT JOIN
		(
			SELECT
				PMD
				,SUM(HOURS) AS N_PMD_WORKED_HRS
			FROM
				NOCHOUSE
			GROUP BY
				PMD
		) N_PMD
			ON INPUT(E.RequestNumber,BEST12.) = N_PMD.PMD
		LEFT JOIN
		(
			SELECT
				PROJECT
				,SUM(HOURS) AS N_PJ_WORKED_HRS
			FROM
				NOCHOUSE
			GROUP BY
				PROJECT
		) N_PROJECT
			ON INPUT(E.RequestNumber,BEST12.) = N_PROJECT.PROJECT
	ORDER BY
		E.RequestNumber DESC
	;
QUIT;

*final output;
ODS HTML FILE="Current Project Hours - &TIMESHEETNAME..html"(TITLE="&TIMESHEETNAME") PATH="T:\";
TITLE 'Project Aggregated Hours - ' "&TIMESHEETNAME" ;
PROC REPORT DATA=TIMETRAX NOWD;
   COMPUTE RequestType;
      COUNT+1;
      IF MOD(COUNT,2) THEN DO;
         CALL DEFINE(_ROW_, "STYLE", "STYLE=[BACKGROUND=VERY LIGHT GRAY]");
      END;
   ENDCOMP;
RUN;
ODS HTML CLOSE;
