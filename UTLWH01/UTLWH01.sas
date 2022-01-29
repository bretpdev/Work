/*COLLEGE OF EASTERN UTAH DEFAULT REPORT - UTLWH01*/


/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
/*FILENAME REPORTZ "&RPTLIB/ULWH01.LWH01RZ";*/
/*FILENAME REPORT2 "&RPTLIB/ULWH01.LWH01R2";*/

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
CREATE TABLE CEUDFLT AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.BF_SSN
	,B.DF_PRS_ID_STU
	,C.AC_LON_TYP
	,A.AF_APL_ID||A.AF_APL_ID_SFX		AS CLID
	,D.LA_CLM_BAL
	,A.LD_LDR_POF
FROM	OLWHRM1.DC01_LON_CLM_INF A
		INNER JOIN OLWHRM1.GA01_APP B ON
			A.AF_APL_ID = B.AF_APL_ID
			AND LC_STA_DC10 = '03'
			AND DAYS(A.LD_LDR_POF) >= DAYS(CURRENT DATE) - 180
			AND A.LC_PCL_REA IN('DF','RD','DQ','RS','DU')
			AND B.AF_APL_OPS_SCL = '00367600'
		INNER JOIN OLWHRM1.GA10_LON_APP C ON
			A.AF_APL_ID = C.AF_APL_ID
			AND A.AF_APL_ID_SFX = C.AF_APL_ID_SFX
		LEFT OUTER JOIN OLWHRM1.DC02_BAL_INT D ON
			A.AF_APL_ID = D.AF_APL_ID
			AND A.AF_APL_ID_SFX = D.AF_APL_ID_SFX
ORDER BY BF_SSN

FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;

/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/

ENDRSUBMIT;

DATA CEUDFLT;
SET WORKLOCL.CEUDFLT;
RUN;

/*PROC PRINTTO PRINT=REPORT2;*/
/*RUN;*/
OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;

PROC PRINT NOOBS SPLIT='/' DATA=CEUDFLT WIDTH=UNIFORM WIDTH=MIN;
SUM		LA_CLM_BAL;
VAR	 	BF_SSN
		DF_PRS_ID_STU
		AC_LON_TYP
		CLID
		LA_CLM_BAL
		LD_LDR_POF;
LABEL	BF_SSN = 'BORROWER SSN'
		DF_PRS_ID_STU = 'STUDENT SSN'
		AC_LON_TYP = 'LOAN TYPE'
		CLID = 'UNIQUE ID'
		LA_CLM_BAL = 'BALANCE'
		LD_LDR_POF = 'CLAIM PAID DATE';
FORMAT  LA_CLM_BAL DOLLAR14.2
		LD_LDR_POF MMDDYY10.;
TITLE	'COLLEGE OF EASTERN UTAH DEFAULT REPORT';
FOOTNOTE  'JOB = UTLWH01     REPORT = ULWH01.LWH01R2';
RUN;
