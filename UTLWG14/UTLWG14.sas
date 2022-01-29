/* UTLWG14 - FISCAL YEAR GROSS GUARANTEES BY SCHOOL BY LENDER
Totals for schools with lenders sorted according by total gross 
guaranty amount.*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWG14.LWG14R2";
FILENAME REPORT3 "&RPTLIB/ULWG14.LWG14R3";
*/
FILENAME REPORT2 "T:\SAS\ULWG14.LWG14R2";
FILENAME REPORT3 "T:\SAS\ULWG14.LWG14R3";

DATA _NULL_;
BEGDATE = PUT(INTNX('YEAR.7',TODAY(),-1), YYMMDD10.);	/*RESOLVES TO 1ST OF PRIOR MONTH*/
ENDDATE = PUT(INTNX('YEAR.7',TODAY(),0) - 1 , YYMMDD10.);	/*RESOLVES TO END OF PRIOR MONTH*/
DATE = PUT(INTNX('YEAR.7',TODAY(),0) - 1, MMDDYY10.);
CALL SYMPUT('BEGIN',"'"||BEGDATE||"'");	            /*CREATES MACRO VARIABLE WITH IN FORMAT  'YYYY/MM/DD'*/
CALL SYMPUT('END',"'"||ENDDATE||"'");              	/* WILL BE USED AS REPLACEMENTS IN CODE*/
CALL SYMPUT('EFFDATE',DATE);
RUN;
%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
OPTIONS MPRINT SYMBOLGEN;

PROC SQL STIMER;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE FYGU AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT
	COUNT(A.AF_APL_ID||A.AF_APL_ID_SFX) AS LN_CT
	,SUM(A.AA_GTE_LON_AMT) AS SUM
	,B.AF_CUR_APL_OPS_LDR AS LENDER
	,D.IM_IST_FUL AS LNAME
	,B.AF_APL_OPS_SCL AS SCHOOL
	,C.IM_IST_FUL AS SNAME
	,A.AC_LON_TYP AS TYPE
FROM OLWHRM1.GA10_LON_APP A 
INNER JOIN OLWHRM1.GA01_APP B 
	ON A.AF_APL_ID = B.AF_APL_ID 
INNER JOIN 
	(SELECT DISTINCT
	IF_IST
	,IM_IST_FUL
	,IC_GEN_ST
	FROM OLWHRM1.SC01_LGS_SCL_INF) C 
	ON B.AF_APL_OPS_SCL = C.IF_IST 
INNER JOIN OLWHRM1.LR01_LGS_LDR_INF D 
	ON B.AF_CUR_APL_OPS_LDR = D.IF_IST

WHERE A.AC_PRC_STA = 'A' 
AND A.AD_PRC BETWEEN &BEGIN AND &END 
AND A.AC_LON_TYP IN ('SF','SU','PL')
GROUP BY B.AF_APL_OPS_SCL, C.IM_IST_FUL, B.AF_CUR_APL_OPS_LDR, D.IM_IST_FUL, A.AC_LON_TYP 
ORDER BY B.AF_APL_OPS_SCL, B.AF_CUR_APL_OPS_LDR, A.AC_LON_TYP 
);
DISCONNECT FROM DB2;

ENDRSUBMIT;
DATA FYGU;
SET WORKLOCL.FYGU;
RUN;

/*SCHOOL REPORTING*/
PROC TRANSPOSE DATA=FYGU OUT=SUMS1 (DROP=_NAME_ _LABEL_ RENAME=(PL=PLNO SF=SFNO SU=SUNO));
VAR LN_CT;
BY SCHOOL SNAME LENDER LNAME;
ID TYPE;
RUN;
PROC TRANSPOSE DATA=FYGU OUT=SUMS2 (DROP=_NAME_ _LABEL_ RENAME=(PL=PLAMT SF=SFAMT SU=SUAMT));
VAR SUM;
BY SCHOOL SNAME LENDER LNAME;
ID TYPE;
RUN;

DATA SUMS;
MERGE SUMS1 SUMS2;
BY SCHOOL SNAME LENDER LNAME;;
TTLNO = SUM(PLNO,SFNO,SUNO);
TTLAMT = SUM(PLAMT,SFAMT,SUAMT);
IF SFNO = . THEN SFNO = 0;
IF SUNO = . THEN SUNO = 0;
IF PLNO = . THEN PLNO = 0;
IF SFAMT = . THEN SFAMT = 0;
IF SUAMT = . THEN SUAMT = 0;
IF PLAMT = . THEN PLAMT = 0;
RUN;

PROC SORT DATA=SUMS;
BY SCHOOL DESCENDING TTLAMT;
RUN;

/*PROC PRINTTO PRINT=REPORT2;*/
/*RUN;*/
OPTIONS NONUMBER NODATE CENTER PAGENO=1 LS=132;
PROC REPORT DATA = SUMS NOWD HEADLINE HEADSKIP SPACING = 1 SPLIT = '/';
TITLE;
FOOTNOTE	'JOB = UTLWG14     REPORT = ULWG14.LWG14R2';
COLUMN	SCHOOL SNAME LENDER LNAME SFNO SFAMT SUNO SUAMT PLNO PLAMT TTLNO TTLAMT;
DEFINE SCHOOL / GROUP NOPRINT;
DEFINE SNAME / GROUP NOPRINT;
DEFINE 	LENDER / DISPLAY 'LENDER ID' WIDTH = 9;
DEFINE	LNAME / DISPLAY 'LENDER NAME' WIDTH = 35;
DEFINE	SFAMT / FORMAT = DOLLAR13. 'AMOUNT' WIDTH = 12;
DEFINE	SFNO / FORMAT = COMMA7. 'NO' WIDTH = 7;
DEFINE	SUAMT / FORMAT = DOLLAR13. 'AMOUNT' WIDTH = 11;
DEFINE	SUNO / FORMAT = COMMA7. 'NO' WIDTH = 7;
DEFINE	PLAMT / FORMAT = DOLLAR13. 'AMOUNT' WIDTH = 11;
DEFINE	PLNO / FORMAT = COMMA7. 'NO' WIDTH = 7;
DEFINE	TTLAMT / FORMAT = DOLLAR13. 'AMOUNT' WIDTH = 12;
DEFINE	TTLNO / FORMAT = COMMA7. 'NO' WIDTH = 7;

COMPUTE BEFORE _PAGE_ / CENTER;
	LINE 'UTAH HIGHER EDUCATION ASSISTANCE AUTHORITY';
	LINE "GROSS GUARANTEES FOR THE FISCAL YEAR ENDING &EFFDATE";
	LINE '(' SCHOOL $8. ')  ' SNAME $40.;
	LINE ' ';
	LINE @57  'STAFFORD' @78 'STAFFORD';
	LINE @56 'SUBSIDIZED' @76 'UNSUBSIDIZED' @99 'PLUS'  @119 'TOTAL';
	ENDCOMP;

BREAK AFTER SCHOOL / OL SUMMARIZE PAGE;
RUN;

/*PROC PRINTTO PRINT=REPORT3;*/
/*RUN;*/
OPTIONS NONUMBER NODATE CENTER PAGENO=1 LS=132;
PROC REPORT DATA = SUMS NOWD HEADLINE HEADSKIP SPACING = 1 SPLIT = '/';
TITLE;
FOOTNOTE	'JOB = UTLWG14     REPORT = ULWG14.LWG14R3';
COLUMN	LENDER LNAME SFNO SFAMT SUNO SUAMT PLNO PLAMT TTLNO TTLAMT;
DEFINE 	LENDER / GROUP 'LENDER ID' WIDTH = 9;
DEFINE	LNAME / GROUP 'LENDER NAME' WIDTH = 35;
DEFINE	SFAMT / FORMAT = DOLLAR13. 'AMOUNT' WIDTH = 12;
DEFINE	SFNO / FORMAT = COMMA7. 'NO' WIDTH = 7;
DEFINE	SUAMT / FORMAT = DOLLAR13. 'AMOUNT' WIDTH = 12;
DEFINE	SUNO / FORMAT = COMMA7. 'NO' WIDTH = 7;
DEFINE	PLAMT / FORMAT = DOLLAR13. 'AMOUNT' WIDTH = 11;
DEFINE	PLNO / FORMAT = COMMA7. 'NO' WIDTH = 6;
DEFINE	TTLAMT / FORMAT = DOLLAR13. 'AMOUNT' WIDTH = 12;
DEFINE	TTLNO / FORMAT = COMMA7. 'NO' WIDTH = 7;

COMPUTE BEFORE _PAGE_ / CENTER;
	LINE 'UTAH HIGHER EDUCATION ASSISTANCE AUTHORITY';
	LINE "GROSS GUARANTEES FOR THE FISCAL YEAR ENDING &EFFDATE";
	LINE 'TOTALS - ALL SCHOOLS';
	LINE ' ';
	LINE @57  'STAFFORD' @78 'STAFFORD';
	LINE @56 'SUBSIDIZED' @76 'UNSUBSIDIZED' @99 'PLUS'  @119 'TOTAL';
	ENDCOMP;
RBREAK AFTER / OL SUMMARIZE;
RUN;


/*TESTFILE;
DATA _NULL_;
BEGDATE = PUT(INTNX('YEAR.7',TODAY(),-1), YYMMDD10.);
ENDDATE = PUT(INTNX('YEAR.7',TODAY(),0) - 1 , YYMMDD10.);
CALL SYMPUT('BEGIN',"'"||BEGDATE||"'");
CALL SYMPUT('END',"'"||ENDDATE||"'");
RUN;
%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
OPTIONS MPRINT SYMBOLGEN;

PROC SQL STIMER;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE FYGUTST AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT B.DF_PRS_ID_BR
	,A.AF_APL_ID||A.AF_APL_ID_SFX CLUID
	,A.AA_GTE_LON_AMT
	,B.AF_CUR_APL_OPS_LDR AS LENDER
	,D.IM_IST_FUL AS LNAME
	,B.AF_APL_OPS_SCL AS SCHOOL
	,C.IM_IST_FUL AS SNAME
	,A.AC_LON_TYP AS TYPE
FROM OLWHRM1.GA10_LON_APP A 
INNER JOIN OLWHRM1.GA01_APP B 
	ON A.AF_APL_ID = B.AF_APL_ID 
INNER JOIN 
	(SELECT DISTINCT
	IF_IST
	,IM_IST_FUL
	,IC_GEN_ST
	FROM OLWHRM1.SC01_LGS_SCL_INF) C 
	ON B.AF_APL_OPS_SCL = C.IF_IST 
INNER JOIN OLWHRM1.LR01_LGS_LDR_INF D 
	ON B.AF_CUR_APL_OPS_LDR = D.IF_IST

WHERE A.AC_PRC_STA = 'A' 
AND A.AD_PRC BETWEEN &BEGIN AND &END 
AND A.AC_LON_TYP IN ('SF','SU','PL')
ORDER BY B.AF_APL_OPS_SCL, B.AF_CUR_APL_OPS_LDR, A.AC_LON_TYP 
);
DISCONNECT FROM DB2;

ENDRSUBMIT;
DATA FYGUTST;
SET WORKLOCL.FYGUTST;
RUN;

DATA YR1;
SET FYGUTST (FIRSTOBS=1 OBS=50000);
RUN;
DATA YR2;
SET FYGUTST (FIRSTOBS=50001);
RUN;
PROC EXPORT DATA= YR1
            OUTFILE= "T:\SAS\FYGUTST1.xls" 
            DBMS=EXCEL2000 REPLACE;
RUN;
PROC EXPORT DATA= YR2
            OUTFILE= "T:\SAS\FYGUTST2.xls" 
            DBMS=EXCEL2000 REPLACE;
RUN;
*/
