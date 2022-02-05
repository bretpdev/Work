/*UTLWO74 - Loans With a Teacher Loan Forgiveness Forbearance Applied--Exclude PLUS*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWO74.LWO74RZ";
FILENAME REPORT2 "&RPTLIB/ULWO74.LWO74R2";

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
CREATE TABLE QUERY AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	PD10.DF_SPE_ACC_ID,
	LN10.LN_SEQ,
	LN15.LA_DSB,
	LN15.LD_DSB,
	LN15.LA_DSB_CAN
FROM	OLWHRM1.PD10_PRS_NME PD10
	INNER JOIN OLWHRM1.LN10_LON LN10
		ON PD10.DF_PRS_ID = LN10.BF_SSN
	INNER JOIN OLWHRM1.LN15_DSB LN15
		ON LN10.BF_SSN = LN15.BF_SSN
		AND LN10.LN_SEQ = LN15.LN_SEQ
	INNER JOIN OLWHRM1.LN90_FIN_ATY LN90
		ON LN15.BF_SSN = LN90.BF_SSN
		AND LN15.LN_SEQ = LN90.LN_SEQ
	INNER JOIN OLWHRM1.FB10_BR_FOR_REQ FB10
		ON PD10.DF_PRS_ID = FB10.BF_SSN
WHERE LN10.LA_CUR_PRI > 0
	AND LN10.LC_STA_LON10 = 'R'
	AND LN10.IC_LON_PGM NOT IN ('PLUS', 'PLUSGB')
	AND FB10.LC_FOR_TYP = '21'
	AND FB10.LC_FOR_STA = 'A'
	AND FB10.LC_STA_FOR10 = 'A'
	AND NOT (LN90.PC_FAT_TYP = '10' AND LN90.PC_FAT_SUB_TYP = '50')
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

CREATE TABLE SUMMED AS
SELECT
	DF_SPE_ACC_ID,
	LN_SEQ,
	SUM(LA_DSB) AS LA_DSB,
	SUM(LA_DSB_CAN) AS LA_DSB_CAN
FROM QUERY
GROUP BY DF_SPE_ACC_ID, LN_SEQ;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;

DATA SUMMED;
SET WORKLOCL.SUMMED;
LOAN_AMOUNT = LA_DSB - COALESCE(LA_DSB_CAN, 0);
RUN;

PROC SORT DATA=SUMMED;
BY DF_SPE_ACC_ID LN_SEQ;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;
TITLE	'TLF Loans Where No Credit has been Applied';

PROC CONTENTS DATA=SUMMED OUT=EMPTYSET NOPRINT;
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
		@32 "JOB = UTLWO74     REPORT = ULWO74.LWO74R2";
	END;
RETURN;
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=SUMMED WIDTH=UNIFORM WIDTH=MIN;
FORMAT LOAN_AMOUNT DOLLAR10.2;
VAR DF_SPE_ACC_ID
	LN_SEQ
	LOAN_AMOUNT;
LABEL	DF_SPE_ACC_ID = 'ACCOUNT #'
		LN_SEQ = 'LOAN #'
		LOAN_AMOUNT = 'LOAN AMOUNT';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB = UTLWO74     REPORT = ULWO74.LWO74R2';
RUN;

PROC PRINTTO;
RUN;
