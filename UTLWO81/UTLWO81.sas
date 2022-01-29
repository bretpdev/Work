/*UTLWO81 - Consols Loaded to COMPASS but Not Split*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWO81.LWO81RZ";
FILENAME REPORT2 "&RPTLIB/ULWO81.LWO81R2";

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;

DATA _NULL_;		
	CALL SYMPUT('REPURCHASEDATELINE',"'02/01/2007'"); 
RUN; 

%SYSLPUT REPURCHASEDATELINE = &REPURCHASEDATELINE; 

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
CREATE TABLE RESULTS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	PD.DF_SPE_ACC_ID AS ACCTNUM,
	PD.DM_PRS_1 AS FN,
	PD.DM_PRS_LST AS LN,
	CLOANS.LN_SEQ AS LNSEQ,
	FIN.LD_FAT_EFF AS REPURCHASEDT
FROM OLWHRM1.LN10_LON CLOANS
INNER JOIN OLWHRM1.PD10_PRS_NME PD
	ON CLOANS.BF_SSN = PD.DF_PRS_ID
INNER JOIN (
				SELECT *
				FROM OLWHRM1.LN90_FIN_ATY
				WHERE PC_FAT_TYP = '02' 
				AND PC_FAT_SUB_TYP IN ('90','91')
				AND LD_FAT_EFF > &REPURCHASEDATELINE
			) FIN ON CLOANS.BF_SSN = FIN.BF_SSN AND CLOANS.LN_SEQ = FIN.LN_SEQ
INNER JOIN (
				SELECT AF_APL_ID
				FROM OLWHRM1.GA10_LON_APP
				WHERE AC_LON_TYP = 'CL'
			) OLOANS ON OLOANS.AF_APL_ID = CLOANS.LF_LON_ALT
INNER JOIN (
				SELECT AF_APL_ID
				FROM OLWHRM1.GA17_CON_LON_DTL
				WHERE AC_CON_UND_LON_TYP IN ('SF','CS','D1','D5')
			) SUBUNDLY ON SUBUNDLY.AF_APL_ID = CLOANS.LF_LON_ALT
INNER JOIN (
				SELECT AF_APL_ID
				FROM OLWHRM1.GA17_CON_LON_DTL
				WHERE AC_CON_UND_LON_TYP IN ('SU','PL','SL','CU','GB','D2','D3','D4','D6','D7','PK','FI','HP','NS','HL')
			) UNSUBUNDLY ON UNSUBUNDLY.AF_APL_ID = CLOANS.LF_LON_ALT
WHERE CLOANS.IC_LON_PGM IN ('SUBCNS','UNCNS', 'SUBSPC', 'UNSPC')
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;

DATA RESULTS;
SET WORKLOCL.RESULTS;
RUN;

PROC SORT DATA=RESULTS;
BY ACCTNUM;
RUN;

/*Keep only accounts that have a single consol loan (indicated by being the only occurrance of that account number).*/
DATA RESULTS;
SET RESULTS;
BY ACCTNUM;
IF FIRST.ACCTNUM = 1 AND LAST.ACCTNUM = 1;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;

OPTIONS ORIENTATION = PORTRAIT;
OPTIONS CENTER NODATE NUMBER PAGENO=1 PS=52 LS=96;

PROC CONTENTS DATA=RESULTS OUT=EMPTYSET NOPRINT;

/*PORTRAIT*/
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF NOBS=0 AND _N_ =1 THEN DO;
PUT // 96*'-';
PUT ////////
@35 '**** NO RECORDS FOUND ****';
PUT ////////
@38 '-- END OF REPORT --';
PUT ////////////////
@27 "JOB = CONSOLS LOADED TO COMPASS BUT NOT SPLIT.R2";
END;
RETURN;
TITLE "CONSOLS LOADED TO COMPASS BUT NOT SPLIT";
RUN;


PROC PRINT NOOBS SPLIT='/' DATA=RESULTS WIDTH=UNIFORM WIDTH=MIN;
VAR ACCTNUM
	FN
	LN
	LNSEQ
	REPURCHASEDT;
LABEL	ACCTNUM = 'ACCT #'
		FN = 'FIRST NAME'
		LN = 'LAST NAME'
		REPURCHASEDT = 'REPURCHASE DATE';
TITLE	'CONSOLS LOADED TO COMPASS BUT NOT SPLIT';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB = CONSOLS LOADED TO COMPASS BUT NOT SPLIT.R2';
RUN;

PROC PRINTTO;
RUN;
