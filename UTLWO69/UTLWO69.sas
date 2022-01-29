/*UTLWO69 - Teacher Loan Forgiveness Applied to PLUS Loans*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWO69.LWO69RZ";
FILENAME REPORT2 "&RPTLIB/ULWO69.LWO69R2";

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
	A.DF_SPE_ACC_ID,
	A.DM_PRS_1,
	A.DM_PRS_LST,
	B.LN_SEQ,
	C.LD_CRT_REQ_FOR
FROM	OLWHRM1.PD10_PRS_NME A
	INNER JOIN OLWHRM1.LN10_LON B
		ON A.DF_PRS_ID = B.BF_SSN
	INNER JOIN OLWHRM1.FB10_BR_FOR_REQ C
		ON B.BF_SSN = C.BF_SSN
WHERE	B.IC_LON_PGM IN ('PLUS', 'PLUSGB')
	AND C.LC_FOR_TYP = '21'
	AND C.LC_FOR_STA = 'A'
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;
DATA QUERY;
SET WORKLOCL.QUERY;
RUN;
PROC SORT DATA=QUERY;
BY DF_SPE_ACC_ID LD_CRT_REQ_FOR;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;
TITLE	'PLUS Loans with Teacher Loan Forgiveness Forbearance';

PROC CONTENTS DATA=QUERY OUT=EMPTYSET NOPRINT;
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
		@32 "JOB = UTLWO69     REPORT = ULWO69.LWO69R2";
	END;
RETURN;
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=QUERY WIDTH=UNIFORM WIDTH=MIN;
FORMAT LD_CRT_REQ_FOR MMDDYY10.;
VAR DF_SPE_ACC_ID
	DM_PRS_1
	DM_PRS_LST
	LN_SEQ
	LD_CRT_REQ_FOR;
LABEL	DF_SPE_ACC_ID = 'ACCOUNT #'
		DM_PRS_1 = 'FIRST NAME'
		DM_PRS_LST = 'LAST NAME'
		LN_SEQ = 'LOAN SEQUENCE'
		LD_CRT_REQ_FOR = 'FORB APPLIED DATE';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB = UTLWO69     REPORT = ULWO69.LWO69R2';
RUN;

PROC PRINTTO;
RUN;
