/*UTLWD23 Activity History Entries*/

LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWD23.LWD23R2";
FILENAME REPORT3 "&RPTLIB/ULWD23.LWD23R3";
FILENAME REPORT4 "&RPTLIB/ULWD23.LWD23R4";
FILENAME REPORT5 "&RPTLIB/ULWD23.LWD23R5";

/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*RSUBMIT;*/
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT
	A.BF_LST_USR_AY01	AS USER_ID,
	A.PF_ACT			AS ACTION_CODE,
	RTRIM(B.PX_ACT_DSC)	AS ACTION_CODE_DESCRIPTION,  
	A.BC_ATY_TYP		AS ACTIVITY_TYPE,
	A.BC_ATY_CNC_TYP	AS CONTACT_TYPE, 
	A.DF_PRS_ID			AS SSN,
	A.BD_ATY_PRF		AS DATE,
	CASE 
		WHEN HOUR(A.BT_ATY_PRF) < 19 THEN 'DAY  '
		ELSE 'NIGHT'
	END					AS TIME,
	A.BT_ATY_PRF,
	A.BX_CMT			AS COMMENTS
		
FROM OLWHRM1.AY01_BR_ATY A 
 	INNER JOIN OLWHRM1.AC01_ACT B
ON A.PF_ACT = B.PF_ACT
WHERE A.BF_LST_USR_AY01 IN ('UT00013','UT00123','UT00072','UT00021'
							,'UT00035', 'UT00072', 'UT00078', 'UT00079' 
							,'UT00123', 'UT00179', 'UT00076', 'UT00182', 'UT00184'
							,'UT00073') 
AND DAYS(A.BD_ATY_PRF) = DAYS(CURRENT DATE) - 1
);
DISCONNECT FROM DB2;
/*ENDRSUBMIT;*/
/**/
/*DATA DEMO;*/
/*SET WORKLOCL.DEMO;*/
/*RUN;*/

PROC SORT DATA = DEMO;
BY USER_ID DESCENDING ACTION_CODE;
RUN;

PROC SQL;
CREATE TABLE DEMOSUM AS
SELECT 
	USER_ID
	,ACTION_CODE
	,ACTION_CODE_DESCRIPTION
	,COUNT(ACTION_CODE) AS TTL_BY_CD
FROM DEMO
GROUP BY USER_ID, ACTION_CODE, ACTION_CODE_DESCRIPTION;

PROC SORT DATA=DEMOSUM;
BY USER_ID ACTION_CODE;
RUN;

PROC SQL;
CREATE TABLE BYTIME AS
SELECT DISTINCT
	(SELECT COUNT(ACTION_CODE)
	 FROM DEMO
	 WHERE TIME = 'DAY' ) AS DAY
   ,(SELECT COUNT(ACTION_CODE)
	 FROM DEMO
	 WHERE TIME = 'NIGHT' ) AS NIGHT
FROM DEMO;


DATA _NULL_;
CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
RUN;

OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS = 39 LS = 128 SYMBOLGEN NODATE CENTER;
OPTIONS PAGENO=1;

TITLE	'Postclaim Services';

PROC PRINTTO PRINT=REPORT2;
RUN;

PROC PRINT SPLIT='/' DATA=DEMO N="TOTAL:  ";
BY USER_ID;
LABEL USER_ID = 'User ID'
ACTION_CODE = 'Action Code'
ACTION_CODE_DESCRIPTION = 'Action Code Description'
ACTIVITY_TYPE = 'Activity Type'
CONTACT_TYPE = 'Contact Type'
SSN = 'SNN'
DATE = 'Date'
COMMENTS = 'Comments';
VAR USER_ID ACTION_CODE ACTION_CODE_DESCRIPTION ACTIVITY_TYPE CONTACT_TYPE SSN DATE;
FORMAT DATE MMDDYY.;
TITLE2	'Activity History Entries - OneLINK';
TITLE3 	"For &RUNDATE";
FOOTNOTE  'JOB = UTLWD23     REPORT = ULWD23.LWD23R2';
RUN;

PROC PRINTTO PRINT=REPORT3;
RUN;
OPTIONS PAGENO=1;
PROC PRINT SPLIT='/' DATA=DEMO N="TOTAL:  ";
BY USER_ID;
LABEL COMMENTS = 'Comments';
VAR COMMENTS;
FORMAT COMMENTS $122.;
TITLE2	'Comments';
TITLE3 	"For &RUNDATE";
FOOTNOTE  'JOB = UTLWD23     REPORT = ULWD23.LWD23R3';
RUN;

PROC PRINTTO PRINT=REPORT4;
RUN;
OPTIONS ORIENTATION = PORTRAIT;
OPTIONS PS = 52 LS = 96;
OPTIONS PAGENO=1;
PROC PRINT NOOBS SPLIT='/' DATA=DEMOSUM;
BY 		USER_ID;
SUM 	TTL_BY_CD;
LABEL 	USER_ID = 'User ID'
		ACTION_CODE = 'Action Code'
		ACTION_CODE_DESCRIPTION = 'Action Code Description'
		TTL_BY_CD = 'Total Used';
VAR 	ACTION_CODE ACTION_CODE_DESCRIPTION TTL_BY_CD;
TITLE2	'Action Code Usage Summary';
TITLE3 	"For &RUNDATE";
FOOTNOTE  'JOB = UTLWD23     REPORT = ULWD23.LWD23R4';
RUN;

PROC PRINTTO PRINT=REPORT5;
RUN;
OPTIONS PAGENO=1;

PROC PRINT NOOBS SPLIT='/' DATA=BYTIME;
VAR 	DAY NIGHT;
TITLE2	'Action Code Usage by Time';
TITLE3 	"For &RUNDATE";
FOOTNOTE  'JOB = UTLWD23     REPORT = ULWD23.LWD23R5';
RUN;