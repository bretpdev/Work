/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWO94.LWO94RZ";
FILENAME REPORT2 "&RPTLIB/ULWO94.LWO94R2";

/*SET DATE RANGE FOR REPORTING TO PREVIOUS MONTH*/
DATA _NULL_;
     CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY()+3,-1,'beginning'), MMDDYYD10.)||"'");
     CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',TODAY()+3,-1,'end'), MMDDYYD10.)||"'");
     CALL SYMPUT('EFFDATE',TRIM(LEFT(UPCASE(
		PUT(INTNX('MONTH',TODAY()+3,-1), MONNAME9.)||' '||
		PUT(INTNX('MONTH',TODAY()+3,-1), YEAR4.)))));
RUN;

%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;

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
SELECT	DISTINCT
		A.DF_SPE_ACC_ID
		,C.LA_FAT_NSI
		,C.LA_FAT_CUR_PRI
		,C.LA_FAT_NSI + LA_FAT_CUR_PRI AS LA_FAT_INT_PEN
		,C.LD_FAT_APL
		,C.LN_SEQ

FROM	OLWHRM1.PD10_PRS_NME A
		INNER JOIN OLWHRM1.DW01_DW_CLC_CLU B
			ON A.DF_PRS_ID = B.BF_SSN
			AND B.WC_DW_LON_STA = '12'
		INNER JOIN OLWHRM1.LN90_FIN_ATY C
			ON A.DF_PRS_ID = C.BF_SSN
			AND B.LN_SEQ = C.LN_SEQ

WHERE	C.PC_FAT_TYP = '50'
		AND C.PC_FAT_SUB_TYP = '02'
		AND C.LC_STA_LON90 = 'A'
		AND C.LC_FAT_REV_REA = ''
		AND C.LD_FAT_APL >= &BEGIN
		AND C.LD_FAT_APL <= &END
		AND C.LA_FAT_NSI + C.LA_FAT_CUR_PRI > 49.99

ORDER BY LD_FAT_APL, DF_SPE_ACC_ID
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

PROC SQL;
CREATE TABLE BRWCNT AS
SELECT COUNT(DISTINCT DF_SPE_ACC_ID) AS BRWCNT
FROM DEMO;


DATA _NULL_;
SET BRWCNT;
CALL SYMPUT ('BORROWERS',"'"||PUT(BRWCNT, COMMA10.)||"'");
RUN;

%PUT &BORROWERS;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS ORIENTATION = PORTRAIT;
OPTIONS CENTER DATE NUMBER PAGENO=1 LS=96 PS=52;

PROC REPORT DATA=DEMO NOWD HEADSKIP SPLIT='/' SPACING=3;
TITLE "MONTHLY INTEREST PENALTIES APPLIED";
FOOTNOTE  'JOB = UTLWO94     REPORT = UTLWO94.LWO94R2';

COLUMN	DF_SPE_ACC_ID
		LA_FAT_NSI
		LA_FAT_CUR_PRI
		LA_FAT_INT_PEN
		LD_FAT_APL;

DEFINE 	DF_SPE_ACC_ID / DISPLAY "ACCOUNT NUMBER" FORMAT=$10. WIDTH=11;
DEFINE 	LA_FAT_NSI / ANALYSIS SUM "INTEREST PENALTY INTEREST" FORMAT=DOLLAR15.2 WIDTH=15;
DEFINE 	LA_FAT_CUR_PRI / ANALYSIS SUM "INTEREST PENALTY CURRENT PRINCIPAL" FORMAT=DOLLAR15.2 WIDTH=15;
DEFINE	LA_FAT_INT_PEN / ANALYSIS SUM "TOTAL AMOUNT OF INTEREST PENALTY" FORMAT=DOLLAR15.2 WIDTH=15;
DEFINE 	LD_FAT_APL / DISPLAY "PENALTY DATE" FORMAT=MMDDYY10. WIDTH=11;

RBREAK AFTER / SUMMARIZE SKIP OL;

COMPUTE AFTER;
LINE ' ';
LINE @9 'TOTAL BORROWERS:' @28 &BORROWERS;
ENDCOMP;
RUN;

PROC PRINTTO;
RUN;

