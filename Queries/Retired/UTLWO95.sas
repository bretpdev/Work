/*UTLWO95 PLUS Credit Check Approval/Denial Monthly Totals*/
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/

FILENAME REPORT2 "&RPTLIB/ULWO95.LWO95R2";   
FILENAME REPORTZ "&RPTLIB/ULWO95.LWO95RZ";

options symbolgen;
/*SET DATE RANGE FOR REPORTING TO PREVIOUS MONTH*/
DATA _NULL_;
     CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY()+3,-1,'beginning'), MMDDYYD10.)||"'");
     CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',TODAY()+3,-1,'end'), MMDDYYD10.)||"'");
     CALL SYMPUT('EFFDATE',TRIM(LEFT(UPCASE(
		PUT(INTNX('MONTH',TODAY()+3,-1), MONNAME9.)||' '||
		PUT(INTNX('MONTH',TODAY()+3,-1), YEAR4.)))));
RUN;

/*%SYSLPUT BEGIN = &BEGIN;*/
/*%SYSLPUT END = &END;*/
/**/
/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*RSUBMIT;*/
%MACRO SQLCHECK (SQLRPT= );
%IF &SQLXRC NE 0 %THEN %DO;
	DATA _NULL_;
    FILE REPORTZ NOTITLES;
    PUT @01 " ********************************************************************* "
      / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
      / @01 " ****  THE SAS LOG IN &SQLRPT SHOULD BE REVIEWED.          **** "       
      / @01 " ********************************************************************* "
      / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
      / @01 " ****  &SQLXMSG   **** "
      / @01 " ********************************************************************* ";
	RUN;
%END;
%MEND;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO2 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	DISTINCT
		A.DF_PRS_ID_BR
		,B.AF_APL_ID || B.AF_APL_ID_SFX AS CLUID
		,A.AD_CRD_CHK_PRF 
		,B.AC_LON_TYP
		,A.AC_CRD_CHK_PRF
		,A.AX_LDR_PS_APV_IAA
		,A.AX_BR_REQ_IAA
		,CASE
			WHEN A.AC_CRD_CHK_PRF = 'Y' AND A.AX_LDR_PS_APV_IAA = '' THEN 0
			WHEN A.AC_CRD_CHK_PRF = 'Y' AND A.AX_LDR_PS_APV_IAA = '000000' THEN 0
			WHEN A.AC_CRD_CHK_PRF = 'Y' THEN CAST(A.AX_LDR_PS_APV_IAA AS BIGINT)
		END AS AMOUNT_APPROVED
		,CASE
			WHEN A.AC_CRD_CHK_PRF = 'D' AND A.AX_BR_REQ_IAA = '' THEN 0
			WHEN A.AC_CRD_CHK_PRF = 'D' AND A.AX_BR_REQ_IAA = '000000' THEN 0
			WHEN A.AC_CRD_CHK_PRF = 'D' THEN CAST(A.AX_BR_REQ_IAA AS BIGINT)
		END AS AMOUNT_DENIED

	FROM	OLWHRM1.GA01_APP A 
			INNER JOIN OLWHRM1.GA10_LON_APP B
			ON A.AF_APL_ID = B.AF_APL_ID

	WHERE 	A.AD_CRD_CHK_PRF BETWEEN &BEGIN AND &END
			AND B.AC_LON_TYP IN ('GB','PL')
			AND A.AC_CRD_CHK_PRF IN ('Y','D')

);
DISCONNECT FROM DB2;
%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;
%SQLCHECK (SQLRPT=ULWAB1.LWAB1RZ);
QUIT;
/*ENDRSUBMIT;*/
/*DATA DEMO2; SET WORKLOCL.DEMO2; RUN;*/

PROC SQL;
CREATE TABLE CNTAPPROVED AS
SELECT COUNT(DF_PRS_ID_BR) AS APPROVED
FROM DEMO2
WHERE AC_CRD_CHK_PRF = 'Y';

PROC SQL;
CREATE TABLE CNTDENIED AS
SELECT COUNT(DF_PRS_ID_BR) AS DENIED
FROM DEMO2
WHERE AC_CRD_CHK_PRF = 'D';

DATA _NULL_;
SET CNTAPPROVED;
CALL SYMPUT ('APPROVED',APPROVED);
RUN;

%PUT &APPROVED;

DATA _NULL_;
SET CNTDENIED;
CALL SYMPUT ('DENIED',DENIED);
RUN;

%PUT &DENIED;

PROC SQL;
CREATE TABLE DEMO3 AS
SELECT	SUM(AMOUNT_APPROVED) AS AMOUNT_APPROVED
		,&APPROVED AS NUMBER_APPROVED
		,SUM(AMOUNT_DENIED) AS AMOUNT_DENIED
		,&DENIED AS NUMBER_DENIED
		,(&APPROVED/(&APPROVED+&DENIED))*100 AS PERCENT_APPROVED
		,(&DENIED/(&APPROVED+&DENIED))*100 AS PERCENT_DENIED
FROM	DEMO2;

QUIT;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS PAGENO=1;
PROC PRINT DATA = DEMO3 NOOBS SPLIT='/' WIDTH=MIN;
VAR		AMOUNT_APPROVED	NUMBER_APPROVED
		AMOUNT_DENIED NUMBER_DENIED
		PERCENT_APPROVED
		PERCENT_DENIED;
FORMAT 	AMOUNT_APPROVED DOLLAR13.2
		NUMBER_APPROVED COMMA13.
		AMOUNT_DENIED DOLLAR13.2
		NUMBER_DENIED COMMA13.
		PERCENT_APPROVED 6.2
		PERCENT_DENIED 6.2;
LABEL	AMOUNT_APPROVED = 'AMOUNT APPROVED'
		NUMBER_APPROVED = 'NUMBER APPROVED'
		AMOUNT_DENIED = 'AMOUNT DENIED'
		NUMBER_DENIED = 'NUMBER DENIED'
		PERCENT_APPROVED = 'PERCENT APPROVED'
		PERCENT_DENIED = 'PERCENT DENIED';
TITLE1	'PLUS Credit Check Results Monthly';
TITLE2	"Summary Report for the Month of &EFFDATE";
FOOTNOTE	'JOB = UTLWO95     REPORT = ULWO95.LWO95R2';
RUN;

PROC PRINTTO;
RUN;

/*PROC EXPORT DATA=DEMO2*/
/*            OUTFILE= "T:\SAS\UTLWO95_Detail.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/

