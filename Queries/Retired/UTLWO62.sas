LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
%LET FILEDIR = /sas/whse/progrevw;

/*%LET RPTLIB = T:\SAS;*/
/*%LET FILEDIR = Q:\Process Automation\TabSAS;*/
FILENAME REPORT2 "&RPTLIB/ULWO62.LWO62R2";
FILENAME REPORTZ "&RPTLIB/ULWO62.LWO62RZ";


OPTIONS SYMBOLGEN NOCENTER NODATE NONUMBER LS=132;
DATA _NULL_;		
	CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY(),-1,'BEGINNING'), MMDDYYD10.)||"'"); /*beginning of previous month*/
	CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',TODAY(),-1,'END'), MMDDYYD10.)||"'"); /*end of previous month*/
RUN;

/*%SYSLPUT BEGIN = &BEGIN;*/
/*%SYSLPUT END = &END;*/
/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*RSUBMIT;*/

%macro sqlcheck ;
  %if  &sqlxrc ne 0  %then  %do  ;
    data _null_  ;
            file reportz notitles  ;
            put @01 " ********************************************************************* "
              / @01 " ****  The SQL code above has experienced an error.               **** "
              / @01 " ****  The SAS should be reviewed.                                **** "       
              / @01 " ********************************************************************* "
              / @01 " ****  The SQL error code is  &sqlxrc  and the SQL error message  **** "
              / @01 " ****  &sqlxmsg   **** "
              / @01 " ********************************************************************* "
            ;
         run  ;
  %end  ;
%mend  ;


PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMOA AS
SELECT *
FROM CONNECTION TO DB2 (

SELECT  
	CASE 
	 WHEN A.AF_ORG_APL_OPS_LDR = '828476' and  C.AC_MPN_SRL_LON = 'S'
	 THEN A2.AF_ORG_APL_OPS_LDR
	 ELSE A.AF_ORG_APL_OPS_LDR
	 END AS REF_LNDR
	,A.DF_PRS_ID_BR AS SSN
	,A.AF_APL_ID AS APP
	,B.AA_GTE_LON_AMT AS AMT

FROM	OLWHRM1.GA01_APP A
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID 
	AND B.AC_PRC_STA = 'A'
	AND B.AD_PRC BETWEEN &BEGIN AND &END
INNER JOIN OLWHRM1.GA20_CNL_DAT C 
	ON A.AF_APL_ID = C.AF_APL_ID 
LEFT OUTER JOIN OLWHRM1.GA01_APP A2
	ON A.AF_BS_MPN_APL_ID = A2.AF_APL_ID 
WHERE A.AF_CUR_APL_OPS_LDR = '828476'

FOR READ ONLY WITH UR

);
DISCONNECT FROM DB2;

%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;
%sqlcheck;
quit;

/*ENDRSUBMIT;*/
/*DATA DEMOA; SET WORKLOCL.DEMOA; RUN;*/

/*************************************************************************************
* NOTE: THE UTLWU09.txt FILE THAT THIS DATA STEP READS NEEDS TO FOLLOW THIS CONVENTION
* VAL1 = LENDER ID
* VAL1 = LENDER NAME
* WHERE VAL1 AND VAL2 ARE CHARACTER DATA 
**************************************************************************************/
DATA REF_LEN;
INFILE "&FILEDIR/UTLWO62.txt" DLM=',' MISSOVER DSD;
FORMAT VAL2 $50.;
INPUT VAL1 $ VAL2 $ ;
RUN;


PROC SQL;
CREATE TABLE DEMO AS
SELECT A.*
	,A.REF_LNDR || ' ' || B.VAL2 AS ID_NAME
FROM DEMOA A
INNER JOIN REF_LEN B
	ON A.REF_LNDR = B.VAL1

;
QUIT;

/*GET BORROWER COUNT*/
PROC SQL;
CREATE TABLE BOR AS
SELECT DISTINCT ID_NAME, SSN
FROM DEMO
;
QUIT;

PROC SQL;
CREATE TABLE BOR2 AS
SELECT ID_NAME, COUNT(*) AS BOR_CNT
FROM BOR
GROUP BY ID_NAME
;
QUIT;

/*GET LOAN COUNT*/
PROC SQL;
CREATE TABLE LOAN AS
SELECT DISTINCT ID_NAME, APP
FROM DEMO
;
QUIT;

PROC SQL;
CREATE TABLE LOAN2 AS
SELECT ID_NAME, COUNT(*) AS LOAN_CNT
FROM LOAN
GROUP BY ID_NAME
;
QUIT;

/*GET TOTAL AMOUNT GUARANTEED*/
PROC SQL;
CREATE TABLE GUAR AS
SELECT ID_NAME, SUM(AMT) AS AMT
FROM DEMO
GROUP BY ID_NAME
;
QUIT;

DATA MASTER;
MERGE BOR2 LOAN2 GUAR;
BY ID_NAME;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;

/*For portrait reports;*/
OPTIONS ORIENTATION = PORTRAIT;
OPTIONS CENTER NODATE NUMBER PAGENO=1 PS=52 LS=96;

PROC PRINT NOOBS SPLIT='/' DATA=MASTER;
BY ID_NAME;
PAGEBY ID_NAME;
FORMAT AMT DOLLAR10.2;
VAR BOR_CNT LOAN_CNT AMT ;
LABEL	BOR_CNT = 'BORROWERS'
		LOAN_CNT = 'LOANS'
		AMT = 'GUARANTEED AMOUNT';

TITLE 'Referral Lender Statistical Report';
FOOTNOTE  'JOB = UTLWO62     REPORT = UTLWO62.LWO62R2';
RUN;


/*GRAND TOTAL*/
/***************************************************/
/***************************************************/
/***************************************************/
/***************************************************/
/*GET BORROWER COUNT*/
PROC SQL;
CREATE TABLE BOR AS
SELECT DISTINCT SSN
FROM DEMO
;
QUIT;

PROC SQL;
CREATE TABLE BOR2 AS
SELECT COUNT(*) AS BOR_CNT
FROM BOR
;
QUIT;

/*GET LOAN COUNT*/
PROC SQL;
CREATE TABLE LOAN AS
SELECT DISTINCT APP
FROM DEMO
;
QUIT;

PROC SQL;
CREATE TABLE LOAN2 AS
SELECT COUNT(*) AS LOAN_CNT
FROM LOAN
;
QUIT;

/*GET TOTAL AMOUNT GUARANTEED*/
PROC SQL;
CREATE TABLE GUAR AS
SELECT SUM(AMT) AS AMT
FROM DEMO
;
QUIT;

DATA GRAND;
MERGE BOR2 LOAN2 GUAR;
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=GRAND;
FORMAT AMT DOLLAR10.2;
VAR BOR_CNT LOAN_CNT AMT ;
LABEL	BOR_CNT = 'BORROWERS'
		LOAN_CNT = 'LOANS'
		AMT = 'GUARANTEED AMOUNT';

TITLE 'Referral Lender Statistical Report';
TITLE2 'Grand Total';
FOOTNOTE  'JOB = UTLWO62     REPORT = UTLWO62.LWO62R2';
RUN;



/**/
/*PROC EXPORT DATA=DEMO*/
/*            OUTFILE= "T:\SAS\DEMO.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/
