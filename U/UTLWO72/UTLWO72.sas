/*UTLWO72 - Consolidation Borrowers on active ACH Disqualified from Timely Payment Benefit*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWO72.LWO72RZ";
FILENAME REPORT2 "&RPTLIB/ULWO72.LWO72R2";

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
	A.DF_SPE_ACC_ID		AS ACCT_ID,
	B.LN_SEQ,
	B.LD_EFT_EFF_BEG	AS ACH_DATE,
	C.LD_RDC_EFF_END	AS DISQUAL_DATE
FROM	OLWHRM1.PD10_PRS_NME A
	INNER JOIN OLWHRM1.LN83_EFT_TO_LON B
		ON A.DF_PRS_ID = B.BF_SSN
	INNER JOIN OLWHRM1.LN84_LON_RTE_RDC C
		ON B.BF_SSN = C.BF_SSN
		AND B.LN_SEQ = C.LN_SEQ
	INNER JOIN OLWHRM1.LN10_LON D
		ON C.BF_SSN = D.BF_SSN
		AND C.LN_SEQ = D.LN_SEQ
	INNER JOIN OLWHRM1.BR30_BR_EFT E
		ON A.DF_PRS_ID = E.BF_SSN
WHERE	D.LC_STA_LON10 = 'R'
	AND D.LA_CUR_PRI > 0
	AND D.IC_LON_PGM IN ('SUBCNS', 'UNCNS', 'SUBSPC', 'UNSPC', 'CNSLDN')
	AND E.BC_EFT_STA = 'A'
	AND C.LD_RDC_EFF_END IS NOT NULL
	AND C.LC_STA_LON84 = 'A'
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
BY ACCT_ID LN_SEQ;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;
TITLE	'Consol Borr with Active ACH and Disqualified from Timely Payment';

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
		@32 "JOB = UTLWO72     REPORT = ULWO72.LWO72R2";
	END;
RETURN;
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=QUERY WIDTH=UNIFORM WIDTH=MIN;
VAR ACCT_ID
	LN_SEQ
	ACH_DATE
	DISQUAL_DATE;
LABEL	ACCT_ID = 'ACCOUNT #'
		LN_SEQ = 'LOAN SEQ #'
		ACH_DATE = 'DATE ACH APPROVED'
		DISQUAL_DATE = 'DATE OF TIMELY PAYMENT DISQUALIFICATION';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB = UTLWO72     REPORT = ULWO72.LWO72R2';
RUN;

PROC PRINTTO;
RUN;
