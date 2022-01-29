/*UTLWG01 - DAILY GUARANTY REPORTING*/
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
/*%LET RPTLIB = T:\SAS;*/
FILENAME REPORT2 "&RPTLIB/ULWG01.LWG01R2";
FILENAME REPORT4 "&RPTLIB/ULWG01.LWG01R4";
FILENAME REPORT6 "&RPTLIB/ULWG01.LWG01R6";
/*LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;*/
/*RSUBMIT;*/
OPTIONS SYMBOLGEN;
LIBNAME OLRPLD V8 '/sas/whse/olrp_lookup_directory';	*CYPRUS LIVE;
*READ LAST RUN DATE FOR THIS JOB FROM CYPRUS DATASET;
DATA _NULL_;
SET OLRPLD.UTLWG02_RUNDT;
CALL SYMPUT('PVS_DT_DATE',"'"||PUT(DATEPART(PVS_RUNDT),MMDDYY10.)||"'");
CALL SYMPUT('PVS_DT_TIME',"'"||PUT(TIMEPART(PVS_RUNDT),TOD.)||"'");
RUN;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE GUARS AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT 
	B.AF_APL_ID||B.AF_APL_ID_SFX							AS CLUID,
	C.DF_PRS_ID_BR											AS SSN,
	C.DF_PRS_ID_STU											AS STUSSN,
	RTRIM(D.DM_PRS_LST)||', '||RTRIM(D.DM_PRS_1)||' '||RTRIM(D.DM_PRS_MID)
															AS NAME,
	C.AF_CUR_APL_OPS_LDR									AS LENDER, /*ORIGINAL LENDER*/
	C.AF_APL_OPS_SCL										AS SCHOOL,
	B.AA_GTE_LON_AMT										AS AMOUNT,
	B.AF_LST_DTS_GA10										AS TIME,
	B.AD_PRC												AS PROC,
	B.AF_APL_ID												AS ID,
	C.AF_BS_MPN_APL_ID										AS BASEID,
	C.AC_ELS_LON											AS ELSIND
	,B.AD_BR_NTF_PRC_RSL
	,B.AI_GTE_RSC
	,B.AF_LST_DTS_GA10
	/**** DISBURSEMENT INFO ****/
	,D1.AD_DSB_ADJ			AS AD_DSB_ADJ1
	,D.DF_SPE_ACC_ID
FROM OLWHRM1.GA10_LON_APP B 
INNER JOIN OLWHRM1.GA01_APP C
	ON B.AF_APL_ID = C.AF_APL_ID
INNER JOIN OLWHRM1.PD01_PDM_INF D
	ON C.DF_PRS_ID_BR = D.DF_PRS_ID
/***** DISBURSEMENT TABLES ******/
LEFT OUTER JOIN OLWHRM1.GA11_LON_DSB_ATY D1
	ON D1.AF_APL_ID = B.AF_APL_ID
	AND D1.AF_APL_ID_SFX = B.AF_APL_ID_SFX
	AND AC_DSB_ADJ = 'E' /*ESTIMATED DISBS ONLY*/
	AND AC_DSB_ADJ_STA = 'A' /*ACTIVE ROW*/
	AND AN_DSB_SEQ = 1

WHERE B.AC_PRC_STA = 'A'

AND days(B.AD_PRC) >= days(&PVS_DT_DATE) - 1
AND	(DAYS(B.AF_LST_DTS_GA10) > DAYS(&PVS_DT_DATE) - 1	
	OR
	(DAYS(B.AF_LST_DTS_GA10) = DAYS(&PVS_DT_DATE) - 1
		AND HOUR(B.AF_LST_DTS_GA10) > HOUR(&PVS_DT_TIME)-24)
	)
AND DAYS(B.AD_BR_NTF_PRC_RSL) = DAYS(CURRENT DATE)

/*EXCEPTS RESCREENS AND BWR NOTIFICATION REPRINTS*/
AND B.AI_GTE_RSC = ' '
AND NOT EXISTS
	(SELECT *
	FROM OLWHRM1.GA13_LON_PRC_RSL X
	WHERE X.AF_APL_ID = B.AF_APL_ID
	AND X.AF_APL_ID_SFX = B.AF_APL_ID_SFX
	AND X.AD_BR_NTF_PRC_HST <> B.AD_BR_NTF_PRC_RSL
	AND X.AC_PRC_STA_HST = 'A')

);
DISCONNECT FROM DB2;
QUIT;

/*ENDRSUBMIT;*/
/*DATA GUARS;*/
/*SET WORKLOCL.GUARS;*/
/*RUN;*/

DATA GUARS;
LENGTH NAME $ 30;
SET GUARS;
LENGTH MPN_TYPE $ 6;
SSNPRN = INPUT(SSN,9.);
IF (BASEID = ' ' OR BASEID EQ ID) AND ELSIND NE 'E' THEN MPN_TYPE = 'NEW';
ELSE IF (BASEID NE ' ' AND BASEID NE ID) THEN MPN_TYPE = 'SERIAL';
ELSE IF (BASEID = ' ' OR BASEID EQ ID) AND ELSIND EQ 'E' THEN MPN_TYPE = 'E-SIGN';
ELSE MPN_TYPE = 'ERROR';		/*ERROR DETECTION VALUE*/
RUN;

PROC SORT DATA=GUARS;
BY MPN_TYPE NAME;
RUN;

DATA _NULL_;
     EFFDT = PUT(TODAY()-1, MMDDYY10.);  *YESTERDAY'S DATE FOR PRINTED REPORT;
     CALL SYMPUT('EFFDATE',EFFDT);
	 RUNDATE = PUT(DATE(),MMDDYY10.);	*TODAY'S DATE FOR COMMA-DEL FILE;
	 CALL SYMPUT('RUNDT',RUNDATE);
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE CENTER NODATE;
OPTIONS PS = 39 LS = 128;
OPTIONS PAGENO=1;
PROC PRINT
	N = 'Number of this MPN type guaranteed = ' 'Total number of loans guaranteed for the day = '
	NOOBS SPLIT='/'	DATA=GUARS WIDTH = MIN WIDTH = UNIFORM;
WHERE LENDER NE '817455';	/*EVERYTHING BUT ZIONS*/
BY MPN_TYPE;
PAGEBY MPN_TYPE;
VAR DF_SPE_ACC_ID
	CLUID
	NAME
	SCHOOL
	AMOUNT
	TIME
	PROC;
SUM AMOUNT;
FORMAT	AMOUNT DOLLAR13. SSNPRN SSN11. PROC MMDDYY10.;
LABEL CLUID='COMMONLINE ID' MPN_TYPE='MPN TYPE' NAME='NAME' DF_SPE_ACC_ID='ACCT #';
TITLE "LPP GUARANTEES PROCESSED FOR &EFFDATE";
FOOTNOTE	'JOB = UTLWG01     REPORT = ULWG01.LWG01R2';
RUN;

PROC PRINTTO PRINT=REPORT4 NEW;
RUN;
OPTIONS PAGENO=1;
PROC PRINT
	N = 'Number of this MPN type guaranteed = ' 'Total number of loans guaranteed for the day = '
	NOOBS SPLIT='/'	DATA=GUARS WIDTH = MIN WIDTH = UNIFORM;
WHERE LENDER = '817455';	/*ONLY ZIONS*/
BY MPN_TYPE;
PAGEBY MPN_TYPE;
VAR DF_SPE_ACC_ID
	CLUID
	NAME
	SCHOOL
	AMOUNT
	TIME
	PROC;
SUM AMOUNT;
FORMAT	AMOUNT DOLLAR13. SSNPRN SSN11. PROC MMDDYY10.;
LABEL CLUID='COMMONLINE ID' MPN_TYPE='MPN TYPE' NAME='NAME' DF_SPE_ACC_ID='ACCT #';
TITLE "ZIONS GUARANTEES PROCESSED FOR &EFFDATE";
FOOTNOTE	'JOB = UTLWG01     REPORT = ULWG01.LWG01R4';
RUN;

/*SUMMARY AND DISCLOSURE RECONCILIATION*/

DATA _NULL_;
CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
CALL SYMPUT('FYEND',PUT(INTNX('YEAR.7',TODAY(),0,'E'), MMDDYY10.));
RUN;
/*IDENTIFY GUARANTEES WITH 1ST DISB AFTER FY END*/
DATA GUARS;
SET GUARS;
IF AD_DSB_ADJ1 <= INTNX('YEAR.7',TODAY(),0,'E')
	THEN DO;
	DSC_PRN = 1;
	DSC_NOPRIN = 0;
	END;
ELSE DO;
	DSC_PRN = 0;
	DSC_NOPRIN = 1;
	END;
RUN;	

PROC PRINTTO PRINT=REPORT6 NEW;
RUN;
OPTIONS PAGENO=1;
PROC REPORT DATA=GUARS NOWD SPACING=1 /*HEADSKIP*/ HEADLINE SPLIT='*';
TITLE 'DAILY GUARANTY TOTALS';
TITLE2 'AND DISCLOSURE RECONCILIATION';
TITLE3 "DATE: &RUNDATE";
FOOTNOTE  'JOB = UTLWG01     REPORT = ULWG01.LWG01R6';
COLUMN MPN_TYPE N DSC_PRN DSC_NOPRIN;
DEFINE MPN_TYPE / GROUP "MPN TYPE" WIDTH=10;
DEFINE N / FORMAT=COMMA8. "LOANS";
DEFINE DSC_PRN / ANALYSIS "SHOULD DISCLOSE TODAY" WIDTH=10;
DEFINE DSC_NOPRIN / ANALYSIS "DISCLOSE AFTER &FYEND" WIDTH=10;

COMPUTE AFTER;
MPN_TYPE = "TOTAL:";
ENDCOMP;

RBREAK AFTER / SUMMARIZE SKIP DOL DUL SUPPRESS;
RUN;

PROC PRINTTO;
RUN;

/*TESTFILE
PROC EXPORT DATA= GUARS
            OUTFILE= "T:\SAS\GUARS.xls" 
            DBMS=EXCEL2000 REPLACE;
RUN;
*/