/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWM27.LWM27RZ";
FILENAME REPORT2 "&RPTLIB/ULWM27.LWM27R2";

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
/*CONNECT TO DB2 (DATABASE=DLGSUTST);*/
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	DISTINCT
		C.DF_SPE_ACC_ID
		,C.DM_PRS_1
		,C.DM_PRS_LST
/*		,B.LD_DLQ_OCC*/
FROM	OLWHRM1.LN10_LON A
		INNER JOIN OLWHRM1.LN16_LON_DLQ_HST B
			ON A.BF_SSN = B.BF_SSN
			AND A.LN_SEQ = B.LN_SEQ
			AND A.IC_LON_PGM = 'TILP'
			AND A.LA_CUR_PRI > 0
			AND DAYS(CURRENT DATE) - 90 >= DAYS(B.LD_DLQ_OCC)
			AND B.LC_STA_LON16 = '1'
			AND B.LC_DLQ_TYP = 'P'
			AND A.LC_STA_LON10 <> 'L'
		INNER JOIN OLWHRM1.PD10_PRS_NME C
			ON A.BF_SSN = C.DF_PRS_ID
ORDER BY DF_SPE_ACC_ID

FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;

DATA DEMO;
SET WORKLOCL.DEMO;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;

PROC PRINT NOOBS SPLIT='/' DATA=DEMO WIDTH=UNIFORM WIDTH=MIN;
VAR 	DF_SPE_ACC_ID
		DM_PRS_1
		DM_PRS_LST;
LABEL	DF_SPE_ACC_ID = 'Account Number'
		DM_PRS_1 = 'First Name'
		DM_PRS_LST = 'Last Name';
TITLE	'TILP Loans to Set to Litigation';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB = UTLWM27     REPORT = ULWM27.LWM27R2';
RUN;

PROC PRINTTO;
RUN;
