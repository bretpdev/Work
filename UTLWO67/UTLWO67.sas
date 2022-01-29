/*UTLWO67 - Accounts With A Borrower Benefit Override on Consolidation Loans*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWO67.LWO67RZ";
FILENAME REPORT2 "&RPTLIB/ULWO67.LWO67R2";

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
	C.DM_PRS_1,
	C.DM_PRS_LST,
	C.DF_SPE_ACC_ID,
	A.LD_ATY_REQ_RCV
FROM	OLWHRM1.AY10_BR_LON_ATY A
	INNER JOIN OLWHRM1.LN10_LON B
		ON A.BF_SSN = B.BF_SSN
	INNER JOIN OLWHRM1.PD10_PRS_NME C
		ON B.BF_SSN = C.DF_PRS_ID
WHERE	A.PF_REQ_ACT = 'CNBBO'
	AND DAYS(CURRENT_DATE) - DAYS(A.LD_ATY_REQ_RCV) <= 7
	AND B.LA_CUR_PRI > 0
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
BY LD_ATY_REQ_RCV;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;
TITLE	'Accounts With a Borrower Benefit Override on Consolidation Loans';

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
		@32 "JOB = UTLWO67     REPORT = ULWO67.LWO67R2";
	END;
RETURN;
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=QUERY WIDTH=UNIFORM WIDTH=MIN;
FORMAT LD_ATY_REQ_RCV MMDDYY10.;
VAR DM_PRS_1
	DM_PRS_LST
	DF_SPE_ACC_ID
	LD_ATY_REQ_RCV;
LABEL	DM_PRS_1 = 'FIRST NAME'
		DM_PRS_LST = 'LAST NAME'
		DF_SPE_ACC_ID = 'ACCOUNT #'
		LD_ATY_REQ_RCV = 'ACTIVITY DATE';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB = UTLWO67     REPORT = ULWO67.LWO67R2';
RUN;

PROC PRINTTO;
RUN;
