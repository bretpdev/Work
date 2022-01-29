/*UTLWO66 - QC Report for RIR Program*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWO66.LWO66RZ";
FILENAME REPORT2 "&RPTLIB/ULWO66.LWO66R2";

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
SELECT DISTINCT
	C.DF_PRS_ID,		/*Not used in production, but sometimes requested specially. Does no harm to leave it in.*/
	C.DF_SPE_ACC_ID,
	B.LN_SEQ,
	B.IC_LON_PGM,
	B.LD_LON_1_DSB
	/*,A.LC_BBS_ELG		/*This attribute is here to check the query results and is not needed in production.*/
FROM	OLWHRM1.LN10_LON B
	INNER JOIN OLWHRM1.PD10_PRS_NME C
		ON B.BF_SSN = C.DF_PRS_ID
	LEFT OUTER JOIN OLWHRM1.LN54_LON_BBS A
		ON A.BF_SSN = B.BF_SSN
		AND A.LN_SEQ = B.LN_SEQ
WHERE	(A.LC_BBS_ELG IS NULL OR A.LC_BBS_ELG = '')
	AND B.LA_CUR_PRI > 0 
	AND B.LF_LON_CUR_OWN = '828476'
	AND B.IC_LON_PGM <> 'SLS'
	AND DAYS(B.LD_LON_1_DSB) >= DAYS('01/01/1993')
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
PROC SORT DATA=DEMO;
BY DF_SPE_ACC_ID;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;

PROC PRINT NOOBS SPLIT='/' DATA=DEMO WIDTH=UNIFORM WIDTH=MIN;
FORMAT LD_LON_1_DSB MMDDYY10.;
VAR /*DF_PRS_ID		/*Include this only when Business Systems makes a special request.*/
	DF_SPE_ACC_ID
	LN_SEQ
	IC_LON_PGM
	LD_LON_1_DSB;
LABEL	/*DF_PRS_ID = 'SSN'		/*Again, only include this for special requests.*/
		DF_SPE_ACC_ID = 'ACCOUNT ID'
		LN_SEQ = 'LOAN SEQUENCE #'
		IC_LON_PGM = 'LOAN PROGRAM'
		LD_LON_1_DSB = 'FIRST DISBURSEMENT';
TITLE	'QC Report for RIR Program';
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	"Please take appropriate precautions to safeguard this information.";
FOOTNOTE3	;
FOOTNOTE4 	'JOB = UTLWO66     REPORT = ULWO66.LWO66R2';
RUN;

PROC PRINTTO;
RUN;

