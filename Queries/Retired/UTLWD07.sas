/*UTLWD07 WORK TRACKING SHEET POSTCLAIM SERVICES*/
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWD07.LWD07R2";
FILENAME REPORT3 "&RPTLIB/ULWD07.LWD07R3";
/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;*/
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
	A.BX_CMT			AS COMMENTS
FROM OLWHRM1.AY01_BR_ATY A 
INNER JOIN OLWHRM1.AC01_ACT B
	ON A.PF_ACT = B.PF_ACT
WHERE A.BF_LST_USR_AY01 IN 
	('UT00021', 'UT00035', 'UT00072', 'UT00078', 'UT00079' 
	,'UT00123', 'UT00179', 'UT00076', 'UT00182', 'UT00184'
	,'UT00073') 
AND DAYS(A.BD_ATY_PRF) = DAYS(CURRENT DATE) - 1
);
DISCONNECT FROM DB2;
/*ENDRSUBMIT;
DATA DEMO;
SET WORKLOCL.DEMO;
RUN;*/
PROC SORT DATA = DEMO;
BY USER_ID DESCENDING ACTION_CODE;
RUN;


PROC PRINTTO PRINT=REPORT2;
RUN;
OPTIONS PAGENO=1 LS=126;
PROC PRINT SPLIT='/' DATA=DEMO N="TOTAL:  " /*WIDTH=MIN*/;
BY USER_ID;
LABEL 	USER_ID = 'User ID'
		ACTION_CODE = 'Action Code'
		ACTION_CODE_DESCRIPTION = 'Action Code Description'
		ACTIVITY_TYPE = 'Activity Type'
		CONTACT_TYPE = 'Contact Type'
		SSN = 'SNN'
		DATE = 'Date'
		COMMENTS = 'Comments';

VAR USER_ID ACTION_CODE ACTION_CODE_DESCRIPTION ACTIVITY_TYPE CONTACT_TYPE SSN DATE;
FORMAT DATE MMDDYY.;
TITLE	"Collections' Daily Work Tracking Report";
FOOTNOTE  'JOB = UTLWD07     REPORT = ULWD07.LWD07R2';
RUN;

PROC PRINTTO PRINT=REPORT3;
RUN;
OPTIONS PAGENO=1 LS=126;
PROC PRINT SPLIT='/' DATA=DEMO N="TOTAL:  " /*WIDTH=MIN*/;
BY USER_ID;
LABEL COMMENTS = 'Comments';
VAR COMMENTS;
FORMAT COMMENTS $122.;
TITLE	"Collections' Daily Work Tracking Report";
FOOTNOTE  'JOB = UTLWD07     REPORT = ULWD07.LWD07R3';
RUN;