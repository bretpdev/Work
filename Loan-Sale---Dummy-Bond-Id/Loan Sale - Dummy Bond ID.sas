/*UTLWO69 - Loan Sale - Dummy Bond Id*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = C:\WINDOWS\TEMP;
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
SELECT
	A.BF_SSN,
	A.LN_SEQ,
	A.LA_CUR_PRI,
	A.LA_NSI_OTS,
	A.LD_NSI_ACR_THU
FROM	OLWHRM1.LN10_LON A
	INNER JOIN OLWHRM1.LN35_LON_OWN B
		ON A.BF_SSN = B.BF_SSN
		AND A.LN_SEQ = B.LN_SEQ
WHERE	B.IF_BND_ISS = 'SUSPENSE'
	AND A.LD_NSI_ACR_THU = '09/19/2007'
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
BY BF_SSN LN_SEQ;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;
TITLE	'LOAN SALE - DUMMY BOND ID';

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
FORMAT LD_NSI_ACR_THU MMDDYY10.;
VAR BF_SSN
	LN_SEQ
	LA_CUR_PRI
	LA_NSI_OTS
	LD_NSI_ACR_THU;
LABEL	BF_SSN = 'SSN'
		LN_SEQ = 'LOAN SEQ #'
		LA_CUR_PRI = 'CURRENT PRINCIPAL BALANCE'
		LA_NSI_OTS = 'OUTSTANDING ACCRUED INTEREST'
		LD_NSI_ACR_THU = 'INTEREST ACCRUED THROUGH';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB = UTLWO69     REPORT = ULWO69.LWO69R2';
RUN;

PROC PRINTTO;
RUN;
