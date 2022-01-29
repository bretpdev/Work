LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/
FILENAME REPORTZ "&RPTLIB/ULWO59.LWO59RZ";
FILENAME REPORT2 "&RPTLIB/ULWO59.LWO59R2";

/*Set PRVDAY to Friday if today is Monday; otherwise set it to yesterday.*/
DATA _NULL_;
	IF WEEKDAY(DATE()) = 1 THEN CALL SYMPUT('PRVDY',"'"||PUT(INTNX('DAY',TODAY(),-3,'BEGINNING'), MMDDYYD10.)||"'");
    ELSE CALL SYMPUT('PRVDY',"'"||PUT(INTNX('DAY',TODAY(),-1,'BEGINNING'), MMDDYYD10.)||"'");
RUN;

/*%SYSLPUT PRVDY = &PRVDY;*/
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
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT
		C.DF_SPE_ACC_ID
		,B.AF_APL_ID
		,B.AF_APL_ID_SFX
		,A.AD_APL_RCV
		,B.AC_PRC_STA
		,B.AD_PRC
		,A.AF_ORG_APL_OPS_LDR
		,A.AF_APL_OPS_SCL
FROM	OLWHRM1.GA01_APP A
		INNER JOIN OLWHRM1.GA10_LON_APP B ON
			A.AF_APL_ID = B.AF_APL_ID
			AND A.AF_ORG_APL_OPS_LDR <> '828476'
			AND A.AF_CUR_APL_OPS_LDR = '828476'
			AND DAYS(A.AD_APL_RCV) >= DAYS(&PRVDY)
		INNER JOIN OLWHRM1.PD01_PDM_INF C ON
			A.DF_PRS_ID_BR = C.DF_PRS_ID
ORDER BY AD_APL_RCV

FOR READ ONLY WITH UR
);
/*DISCONNECT FROM DB2;*/

%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;
%sqlcheck;
quit;

/*ENDRSUBMIT;*/
/**/
/*DATA DEMO;*/
/*SET WORKLOCL.DEMO;*/
/*RUN;*/

PROC SORT DATA=DEMO; BY AF_ORG_APL_OPS_LDR DF_SPE_ACC_ID;RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;

PROC PRINT NOOBS SPLIT='/' DATA=DEMO WIDTH=UNIFORM WIDTH=MIN;
BY 		AF_ORG_APL_OPS_LDR;
VAR		DF_SPE_ACC_ID
		AF_APL_ID
		AF_APL_ID_SFX
		AD_APL_RCV
		AC_PRC_STA
		AD_PRC
		AF_ORG_APL_OPS_LDR
		AF_APL_OPS_SCL;
LABEL	DF_SPE_ACC_ID = 'ACCOUNT NUMBER'
		AF_APL_ID = 'UNIQUE ID'
		AF_APL_ID_SFX = 'UNIQUE ID SUFFIX'
		AD_APL_RCV = 'RECEIVED DATE'
		AC_PRC_STA = 'LOAN STATUS'
		AD_PRC = 'PROCESSING DATE'
		AF_ORG_APL_OPS_LDR = 'ORIGINAL LENDER'
		AF_APL_OPS_SCL = 'SCHOOL CODE';
TITLE	'Referral Lender Loans Received Previous Day';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB = UTLWO59     REPORT = ULWO59.LWO59R2';
RUN;

PROC PRINTTO;
RUN;
