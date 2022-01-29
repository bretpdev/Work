/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWM29.LWM29RZ";
FILENAME REPORT2 "&RPTLIB/ULWM29.LWM29R2";

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
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT	DISTINCT
		C.DF_SPE_ACC_ID,
		C.DM_PRS_LST,
		DAYS(CURRENT DATE) - DAYS(Q.LD_DLQ_OCC) AS DAYS_DELINQUENT,
		A.LA_CUR_PRI
FROM	OLWHRM1.LN16_LON_DLQ_HST Q
	INNER JOIN OLWHRM1.LN10_LON A
		ON Q.BF_SSN = A.BF_SSN
		AND Q.LN_SEQ = A.LN_SEQ
	INNER JOIN OLWHRM1.PD30_PRS_ADR B
		ON A.BF_SSN = B.DF_PRS_ID
	INNER JOIN OLWHRM1.PD10_PRS_NME C
		ON B.DF_PRS_ID = C.DF_PRS_ID
WHERE	A.IC_LON_PGM = 'TILP'
	AND A.LA_CUR_PRI > 0
	AND A.LC_STA_LON10 = 'L'
	AND B.DC_DOM_ST = 'UT'
	AND B.DC_ADR = 'L'
	AND B.DI_VLD_ADR = 'Y'
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

PROC SORT DATA = DEMO;
BY DAYS_DELINQUENT;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;
TITLE	'TILP Loans in Litigation';

PROC CONTENTS DATA=DEMO OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 95*'-';
	PUT      //////
		@35 '**** NO OBSERVATIONS FOUND ****';
	PUT ////////
		@41 '-- END OF REPORT --';
	PUT /////////////
		@32 "JOB = UTLWM29     REPORT = ULWM29.LWM29R2";
	END;
RETURN;
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=DEMO WIDTH=UNIFORM WIDTH=MIN;
VAR DF_SPE_ACC_ID
	DM_PRS_LST
	DAYS_DELINQUENT
	LA_CUR_PRI;
LABEL	DF_SPE_ACC_ID = 'ACCOUNT #'
		DM_PRS_LST = 'LAST NAME'
		DAYS_DELINQUENT = 'DAYS DELINQUENT'
		LA_CUR_PRI = 'CURRENT PRINCIPAL BALANCE';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB = UTLWM29     REPORT = ULWM29.LWM29R2';
RUN;

PROC PRINTTO;
RUN;
