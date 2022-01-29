/*UTLWA30 - Loan Provision Statistics*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWA30.LWA30RZ";
FILENAME REPORT2 "&RPTLIB/ULWA30.LWA30R2";
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
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
CREATE TABLE DAT AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT 	GA10.AF_APL_ID || GA10.AF_APL_ID_SFX AS APPID,
		COALESCE(GA10.AA_CUR_PRI, 0) AS AA_CUR_PRI,
		COALESCE(GA10.AA_OTS_ACR_INT,0) AS AA_OTS_ACR_INT,
		(COALESCE(GA10.AA_CUR_PRI, 0) + COALESCE(GA10.AA_OTS_ACR_INT,0)) AS PRINANDINT,
		(GA10.AA_GTE_LON_AMT - (COALESCE(GA10.AA_TOT_CAN,0) + COALESCE(GA10.AA_TOT_RFD,0))) AS ORIGINALPRIN
FROM	OLWHRM1.GA10_LON_APP GA10
WHERE	GA10.AD_PRC > '06/30/2006' 
		AND GA10.AC_PRC_STA = 'A'
		AND GA10.AC_GTE_TRF != 'O'
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;
DATA DAT;
	SET WORKLOCL.DAT;
RUN;

PROC SQL;
	CREATE TABLE SUMMEDDAT AS
	SELECT SUM(AA_CUR_PRI) AS AA_CUR_PRI,
			SUM(AA_OTS_ACR_INT)	AS AA_OTS_ACR_INT,
			SUM(PRINANDINT)	AS PRINANDINT,
			SUM(ORIGINALPRIN) AS ORIGINALPRIN
	FROM DAT;
QUIT;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;
PROC PRINT NOOBS SPLIT='/' DATA=SUMMEDDAT WIDTH=UNIFORM WIDTH=MIN;
FORMAT AA_CUR_PRI DOLLAR20.2
	AA_OTS_ACR_INT DOLLAR20.2
	PRINANDINT DOLLAR20.2
	ORIGINALPRIN DOLLAR20.2;
VAR AA_CUR_PRI
	AA_OTS_ACR_INT
	PRINANDINT
	ORIGINALPRIN;
LABEL	AA_CUR_PRI = 'CURRENT PRINCIPAL'
		AA_OTS_ACR_INT = 'CURRENT INTEREST'
		PRINANDINT = 'CURRENT PRIN AND INT'
		ORIGINALPRIN = 'ORIGINAL PRINCIPAL';
TITLE	'LOAN PROVISION DATA POST JUNE 30, 2006';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB = UTLWA30     REPORT = ULWA30.LWA30R2';
RUN;
PROC PRINTTO;
RUN;
