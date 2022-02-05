/*UTLWD31 - QC AWG Withdrawal*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWD31.LWD31RZ";
FILENAME REPORT2 "&RPTLIB/ULWD31.LWD31R2";

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
	A.DF_SPE_ACC_ID,
	B.LA_TRX,
	B.LD_TRX_EFF,
	C.BF_LST_USR_AY01
FROM	OLWHRM1.PD01_PDM_INF A
	INNER JOIN OLWHRM1.DC11_LON_FAT B
		ON A.DF_PRS_ID = B.BF_SSN
	INNER JOIN OLWHRM1.AY01_BR_ATY C
		ON A.DF_PRS_ID = C.DF_PRS_ID
	INNER JOIN OLWHRM1.LA10_LEG_ACT D
		ON A.DF_PRS_ID = D.DF_PRS_ID_BR
WHERE	C.PF_ACT = '1030N'
	AND DAYS(D.BD_WDR) = DAYS(CURRENT DATE) - 1
	AND D.BC_WDR_REA = '5'
	AND B.LC_TRX_TYP = 'GP'
	AND (A.BI_EMP_INF_OVR = 'Y'
		OR DAYS(CURRENT DATE) - DAYS(B.LD_TRX_EFF) <= 45)
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
BY LA_TRX;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;
TITLE	'QC AWG Withdrawal';

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
		@32 "JOB = UTLWD31     REPORT = ULWD31.LWD31R2";
	END;
RETURN;
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=QUERY WIDTH=UNIFORM WIDTH=MIN;
FORMAT LD_TRX_EFF MMDDYY10.;
VAR DF_SPE_ACC_ID
	LA_TRX
	LD_TRX_EFF
	BF_LST_USR_AY01;
LABEL	DF_SPE_ACC_ID = 'ACCOUNT NUMBER'
		LA_TRX = 'LAST TRANSACTION AMOUNT'
		LD_TRX_EFF = 'DATE OF LAST PAYMENT'
		BF_LST_USR_AY01 = 'USER WHO WITHDREW';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB = UTLWD31     REPORT = ULWD31.LWD31R2';
RUN;

PROC PRINTTO;
RUN;
