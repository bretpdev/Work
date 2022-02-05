/*LOAN MANAGEMENT SPECIAL CAMPAIGN STATISTICS*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*FILENAME REPORT2 "&RPTLIB/ULWM10.LWM10R2";*/

options symbolgen;
DATA _NULL_;		
	B = TODAY() - 9;
	E = TODAY() - 2;
    CALL SYMPUT('BEGIN',"'"||put(B,MMDDYY10.)||"'");
	CALL SYMPUT('END',"'"||put(E,MMDDYY10.)||"'");
/*	CALL SYMPUT('BEGINT',put(B,MMDDYY10.));*/
/*	CALL SYMPUT('ENDT',put(E,MMDDYY10.));*/
RUN;
%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;
/*%SYSLPUT BEGINT = &BEGINT;*/
/*%SYSLPUT ENDT = &ENDT;*/

FILENAME REPORT2 "T:\SAS\ULWM10.LWM10R2";
options symbolgen;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;

PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (

SELECT DISTINCT AY10.BF_SSN AS SSN
	,DAYS(AY10.LD_ATY_REQ_RCV) AS LD_ATY_REQ_RCV
	,AY10.LD_ATY_REQ_RCV AS TXT_LD_ATY_REQ_RCV
	,AY10.PF_REQ_ACT AS CODE
	,AY10.PF_RSP_ACT AS RSPCODE
	,DAYS(AY10.LD_ATY_RSP) AS LD_ATY_RSP
	,AY10.LD_ATY_RSP AS TXT_LD_ATY_RSP
	,DAYS(CURRENT DATE) AS CDATE
	,&BEGIN AS BDATE
	,&END AS EDATE
	,DAYS(&BEGIN) AS BDATEN
	,DAYS(&END) AS EDATEN

	,DC01.LC_STA_DC10
	,DC01.LC_PCL_REA
	,DAYS(DC01.LD_PCL_RCV) AS LD_PCL_RCV
	,LN10.LC_STA_LON10
	,LN10.LA_CUR_PRI
	,DW01.WA_TOT_BRI_OTS
	,DW01.WC_DW_LON_STA
	,DAYS(LN16.LD_DLQ_OCC) AS LD_DLQ_OCC
	,LN16.LC_STA_LON16
	
FROM OLWHRM1.AY10_BR_LON_ATY AY10
LEFT OUTER JOIN OLWHRM1.LN85_LON_ATY LN85
ON LN85.BF_SSN = AY10.BF_SSN
AND LN85.LN_ATY_SEQ = AY10.LN_ATY_SEQ 
LEFT OUTER JOIN OLWHRM1.LN10_LON LN10
ON LN85.BF_SSN = LN10.BF_SSN
AND LN85.LN_SEQ = LN10.LN_SEQ
LEFT OUTER JOIN OLWHRM1.DW01_DW_CLC_CLU DW01
ON DW01.BF_SSN = LN10.BF_SSN
AND DW01.LN_SEQ = LN10.LN_SEQ
LEFT OUTER JOIN OLWHRM1.DC01_LON_CLM_INF DC01
ON DC01.BF_SSN = LN10.BF_SSN
AND DC01.AF_APL_ID = LN10.LF_LON_ALT 
AND INTEGER(DC01.AF_APL_ID_SFX) = LN10.LN_LON_ALT_SEQ


LEFT OUTER JOIN OLWHRM1.LN16_LON_DLQ_HST LN16
ON LN16.BF_SSN = DW01.BF_SSN
AND LN16.LN_SEQ = DW01.LN_SEQ

WHERE (DAYS(AY10.LD_ATY_REQ_RCV) BETWEEN DAYS(&BEGIN) AND DAYS(&END)
	OR DAYS(AY10.LD_ATY_RSP) BETWEEN DAYS(&BEGIN) AND DAYS(&END))
AND AY10.PF_REQ_ACT IN ('XC10R','XWXPR','XCNWD')
AND LN10.LI_CON_PAY_STP_PUR <> 'Y'
);

DISCONNECT FROM DB2;
ENDRSUBMIT;
DATA DEMO; SET WORKLOCL.DEMO; RUN;

PROC SORT DATA=DEMO;
BY SSN CODE;
RUN;

/*STEP 1*/
PROC SQL;
CREATE TABLE STEP1 AS
SELECT DISTINCT SSN, CODE
FROM DEMO
WHERE CODE IN ('XC10R','XWXPR','XCNWD')
AND LD_ATY_REQ_RCV > BDATEN 
AND LD_ATY_REQ_RCV < EDATEN 
;
QUIT;

PROC SQL;
CREATE TABLE STEP1B AS
SELECT CODE, COUNT(*) AS S1
FROM STEP1
GROUP BY CODE;
QUIT;

/*STEP 2*/
PROC SQL;
CREATE TABLE STEP2 AS
SELECT DISTINCT SSN, CODE
FROM DEMO
WHERE RSPCODE IN ('CNTCT')
AND LD_ATY_RSP > BDATEN 
AND LD_ATY_RSP < EDATEN
;
QUIT;

PROC SQL;
CREATE TABLE STEP2B AS
SELECT DISTINCT CODE, COUNT(*) AS S2
FROM STEP2 
GROUP BY CODE;
QUIT;

/*STEP 3*/
PROC SQL;
CREATE TABLE STEP3 AS
SELECT DISTINCT SSN, CODE
FROM DEMO
WHERE RSPCODE IN ('NOCTC','INVPH')
AND LD_ATY_RSP > BDATEN 
AND LD_ATY_RSP < EDATEN
;
QUIT;

PROC SQL;
CREATE TABLE STEP3B AS
SELECT DISTINCT CODE, COUNT(*) AS S3
FROM STEP3
GROUP BY CODE;
QUIT;

/*STEP 4 */
PROC SQL;
CREATE TABLE STEP4 AS
SELECT DISTINCT SSN, CODE, LD_ATY_REQ_RCV, LD_ATY_RSP, RSPCODE
FROM DEMO
WHERE RSPCODE IN ('NOCTC','INVPH')
AND LD_ATY_RSP > BDATEN
AND LD_ATY_RSP < EDATEN
;
QUIT;

PROC SQL;
CREATE TABLE STEP4B AS
SELECT CODE, COUNT(*) AS S4
FROM STEP4
GROUP BY CODE;
QUIT;

/*STEP 5*/
PROC SQL;
CREATE TABLE STEP5 AS
SELECT DISTINCT SSN, CODE
FROM DEMO
WHERE LC_STA_DC10 = '01'
AND LC_PCL_REA NOT IN ('BC','BH','BO','DI','DE','CS','FC','IN')
AND LD_ATY_REQ_RCV > BDATEN 
AND LD_ATY_REQ_RCV < EDATEN
;
QUIT;

PROC SQL;
CREATE TABLE STEP51 AS
SELECT DISTINCT CODE, COUNT(*) AS S51
FROM STEP5
GROUP BY CODE;
QUIT;

PROC SQL;
CREATE TABLE STEP52 AS
SELECT DISTINCT SSN, CODE
FROM DEMO D
WHERE EXISTS (SELECT * 
				FROM STEP5 S
				WHERE S.SSN = D.SSN
			  )
AND D.LD_PCL_RCV > D.LD_ATY_REQ_RCV
AND LD_ATY_REQ_RCV > BDATEN 
AND LD_ATY_REQ_RCV < EDATEN
;
QUIT;

PROC SQL;
CREATE TABLE STEP52B AS
SELECT DISTINCT CODE, COUNT(*) AS S52
FROM STEP52
GROUP BY CODE;
QUIT;

PROC SQL;
CREATE TABLE STEP53 AS
SELECT DISTINCT CODE, COUNT(*) AS S53
FROM STEP1 D
WHERE NOT EXISTS (SELECT * 
				FROM STEP5 S
				WHERE S.SSN = D.SSN
			  	)
GROUP BY CODE;
QUIT;

/*STEP 6  BORROWERS WITH IN-HOUSE LOANS*/
/*PROC SQL;*/
/*CREATE TABLE STEP6 AS*/
/*SELECT DISTINCT SSN, CODE*/
/*FROM DEMO*/
/*WHERE LC_STA_LON10 = 'R'*/
/*AND LA_CUR_PRI > 0;*/
/*QUIT;*/
/**/
/*PROC SQL;*/
/*CREATE TABLE STEP6B AS*/
/*SELECT DISTINCT CODE, COUNT(*) AS S6*/
/*FROM STEP6*/
/*GROUP BY CODE;*/
/*QUIT;*/

/*STEP 7*/
PROC SQL;
CREATE TABLE STEP7B AS
SELECT *
FROM DEMO
WHERE RSPCODE IN ('CNTCT')
AND LD_ATY_RSP > BDATEN 
AND LD_ATY_RSP < EDATEN
;
QUIT;

PROC SQL;
CREATE TABLE STEP7 AS
SELECT DISTINCT SSN, CODE
FROM STEP7B
WHERE LC_STA_LON10 = 'R'
AND WC_DW_LON_STA NOT IN ('01','02','04','05','12','17','19','22','23','88','98')
AND CDATE - LD_DLQ_OCC >= 30
AND LC_STA_LON16 = '1'
AND WA_TOT_BRI_OTS + LA_CUR_PRI > 0
;
QUIT;

PROC SQL;
CREATE TABLE STEP7B AS
SELECT DISTINCT CODE, COUNT(*) AS S7
FROM STEP7
GROUP BY CODE
;
QUIT;

/*STEP4B*/
DATA CONC;
MERGE STEP1B STEP2B STEP3B STEP4B STEP51 STEP52B STEP53 STEP7B;/*STEP6B */
BY CODE;
RUN;

DATA CONCLUSION;
SET CONC;
FORMAT CODE $19.;
IF S1 = . THEN S1 = 0; 
IF S2 = . THEN S2 = 0;
IF S3 = . THEN S3 = 0;
IF S4 = . THEN S4 = 0;
IF S51 = . THEN S51 = 0;
IF S52 = . THEN S52 = 0;
IF S53 = . THEN S53 = 0;
/*IF S6 = . THEN S6 = 0;*/
IF S7 = . THEN S7 = 0;
IF CODE = 'XC10R' THEN CODE1 = 'EXIT CALLS';
IF CODE = 'XWXPR' THEN CODE1 = 'DEF/FOR ';
IF CODE = 'XCNWD' THEN CODE1 = 'DISCLOSURE';
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;

/*For landscape reports:*/
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127;

/*For portrait reports;*/
/*OPTIONS ORIENTATION = PORTRAIT;*/
/*OPTIONS PS=52 LS=96;*/

PROC REPORT DATA = CONCLUSION NOWD HEADLINE HEADSKIP SPLIT = '/' SPACING=2;
TITLE 'LOAN MANAGEMENT COMPASS SPECIAL CAMPAIGN STATISTICS - WEEKLY';
/*TITLE2 &BEGINT' TO '&ENDT;*/
FOOTNOTE  'JOB = UTLWM10     REPORT = UTLWM10.LWM10R2';
COLUMN CODE1 s1 s2 s3 S4 s51 s52 s53 s7 ;/*s6*/
DEFINE CODE1 / 'SPECIAL/CAMPAIGN' WIDTH = 19; 
DEFINE S1 / 'BRWRS.' WIDTH = 6;
DEFINE S2 / 'SUCCESSFULL/CONTACTS' WIDTH = 11;
DEFINE S3 / 'BORROWER/UNSUCCESSFULL/CONTACTS' WIDTH = 13;
DEFINE S4 / 'TOTAL/UNSUCCESSFUL/ATTEMPTS/MADE' WIDTH = 13;
DEFINE S51 / 'BRWRS./ACTIVE/PRECLAIM' WIDTH = 8;
DEFINE S52 / 'BRWRS./NEW/PRECLAIM' WIDTH = 8;
DEFINE S53 / 'BRWRS./NOT/IN/PRECLAIM' WIDTH = 8;
/*DEFINE S6 / 'SPECIAL/CAMP./BORROWERS/WITH/IN-HOUSE LOANS' WIDTH = 13;*/
DEFINE S7 / 'CONTACTED/BORROWERS/30 + DAYS/DELINQUENT/STATUS' WIDTH = 13;
RBREAK AFTER / SUMMARIZE SKIP DOL SUPPRESS;
RUN;

/**/
/*PROC EXPORT DATA=STEP1*/
/*            OUTFILE= "T:\SAS\STEP1.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/
/*PROC EXPORT DATA=STEP2*/
/*            OUTFILE= "T:\SAS\STEP2.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/
/*PROC EXPORT DATA=STEP3*/
/*            OUTFILE= "T:\SAS\STEP3.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/
/*PROC EXPORT DATA=STEP4*/
/*            OUTFILE= "T:\SAS\STEP4.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/
/*PROC EXPORT DATA=STEP5*/
/*            OUTFILE= "T:\SAS\STEP5.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/
/*PROC EXPORT DATA=STEP52*/
/*            OUTFILE= "T:\SAS\STEP52.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/
/*PROC EXPORT DATA=STEP53*/
/*            OUTFILE= "T:\SAS\STEP53.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/
/**/
/*PROC EXPORT DATA=STEP7*/
/*            OUTFILE= "T:\SAS\STEP7.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/