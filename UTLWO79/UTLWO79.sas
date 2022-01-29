/*UTLWO79 - Autopay Review*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWO79.LWO79RZ";
FILENAME REPORT2 "&RPTLIB/ULWO79.LWO79R2";
FILENAME REPORT3 "&RPTLIB/ULWO79.LWO79R3";

DATA _NULL_;
     CALL SYMPUT('PRVDY',"'"||PUT(INTNX('DAY',TODAY(),-1,'BEGINNING'), MMDDYYD10.)||"'");
RUN;
%SYSLPUT PRVDY = &PRVDY;

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
CREATE TABLE R2 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	PD10.DF_SPE_ACC_ID		AS ACCOUNT
	,TRIM(PD10.DM_PRS_1) || ' ' || TRIM(PD10.DM_PRS_LST)	AS NAME
	,MAX(BR30.BD_EFT_STA)	AS ADDED_DATE
FROM	OLWHRM1.PD10_PRS_NME PD10
INNER JOIN OLWHRM1.BR30_BR_EFT BR30
	ON PD10.DF_PRS_ID = BR30.BF_SSN
WHERE	BR30.BC_EFT_STA = 'A'
	AND BR30.BD_EFT_STA = &PRVDY
GROUP BY
	PD10.DF_SPE_ACC_ID
	,TRIM(PD10.DM_PRS_1) || ' ' || TRIM(PD10.DM_PRS_LST)
FOR READ ONLY WITH UR
);

CREATE TABLE R3 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	PD10.DF_SPE_ACC_ID		AS ACCOUNT
	,TRIM(PD10.DM_PRS_1) || ' ' || TRIM(PD10.DM_PRS_LST)	AS NAME
	,MAX(BR30.BD_EFT_STA)	AS ADDED_DATE
FROM	OLWHRM1.PD10_PRS_NME PD10
INNER JOIN OLWHRM1.BR30_BR_EFT BR30
	ON PD10.DF_PRS_ID = BR30.BF_SSN
INNER JOIN OLWHRM1.AY10_BR_LON_ATY AY10
	ON PD10.DF_PRS_ID = AY10.BF_SSN
WHERE	BR30.BC_EFT_STA = 'A'
	AND BR30.BD_EFT_STA = &PRVDY
	AND AY10.PF_REQ_ACT = 'DTPRC'
	AND AY10.LD_ATY_REQ_RCV >= &PRVDY
GROUP BY
	PD10.DF_SPE_ACC_ID
	,TRIM(PD10.DM_PRS_1) || ' ' || TRIM(PD10.DM_PRS_LST)
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;

DATA R2; SET WORKLOCL.R2; RUN;
DATA R3; SET WORKLOCL.R3; RUN;

PROC SORT DATA=R2;
BY ACCOUNT;
RUN;
PROC SORT DATA=R3;
BY ACCOUNT;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;
TITLE	'Autopay Added to System';

PROC CONTENTS DATA=R2 OUT=EMPTYSET NOPRINT;
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
		@32 "JOB = UTLWO79     REPORT = ULWO79.LWO79R2";
	END;
RETURN;
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=R2 WIDTH=UNIFORM WIDTH=MIN;
FORMAT ADDED_DATE MMDDYY10.;
VAR ACCOUNT
	NAME
	ADDED_DATE;
LABEL	ADDED_DATE = 'ADDED DATE';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB = UTLWO79     REPORT = ULWO79.LWO79R2';
RUN;

PROC PRINTTO;
RUN;

PROC PRINTTO PRINT=REPORT3 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;
TITLE	'Due Date Changes with Auto Pay';

PROC CONTENTS DATA=R3 OUT=EMPTYSET NOPRINT;
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
		@32 "JOB = UTLWO79     REPORT = ULWO79.LWO79R2";
	END;
RETURN;
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=R3 WIDTH=UNIFORM WIDTH=MIN;
FORMAT ADDED_DATE MMDDYY10.;
VAR ACCOUNT
	NAME
	ADDED_DATE;
LABEL	ADDED_DATE = 'ADDED DATE';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB = UTLWO79     REPORT = ULWO79.LWO79R3';
RUN;

PROC PRINTTO;
RUN;
