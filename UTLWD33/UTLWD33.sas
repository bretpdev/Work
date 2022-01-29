/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWD33.LWD33R2";
FILENAME REPORTZ "&RPTLIB/ULWD33.LWD33RZ";
OPTIONS SYMBOLGEN NOCENTER NODATE NONUMBER LS=132;
DATA _NULL_;
	CALL SYMPUT('BEGIN',"'"||PUT(INTNX('DAY',TODAY(),-1,'BEGINNING'), MMDDYYD10.)||"'");
	CALL SYMPUT('END',"'"||PUT(INTNX('DAY',TODAY(),-1,'END'), MMDDYYD10.)||"'");
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
SELECT DISTINCT PD01.DF_SPE_ACC_ID
/*	,SUM(DC01.LA_CLM_PRI */
/*		+DC01.LA_CLM_INT*/
/*		-DC01.LA_PRI_COL*/
/*		+DC01.LA_INT_ACR*/
/*		+COALESCE(DC02.LA_CLM_INT_ACR,0)*/
/*		-DC01.LA_INT_COL)					*/
/*		 + SUM (DC01.LA_LEG_CST_ACR*/
/*		-DC01.LA_LEG_CST_COL*/
/*		+DC01.LA_OTH_CHR_ACR*/
/*		-DC01.LA_OTH_CHR_COL*/
/*		+DC01.LA_COL_CST_ACR*/
/*		-DC01.LA_COL_CST_COL)*/
/*		+SUM(COALESCE(DC02.LA_CLM_PRJ_COL_CST,0)) AS TOT_PAYOFF*/
	,COALESCE(DC11.TOT_PAYOFF,0) AS TOT_PAYOFF
	,AY01.PF_ACT	
	,AY01.BD_ATY_PRF
FROM OLWHRM1.AY01_BR_ATY AY01
INNER JOIN OLWHRM1.DC01_LON_CLM_INF DC01
	ON AY01.DF_PRS_ID = DC01.BF_SSN
INNER JOIN OLWHRM1.PD01_PDM_INF PD01
	ON AY01.DF_PRS_ID = PD01.DF_PRS_ID
LEFT OUTER JOIN (
	SELECT BF_SSN
		,SUM(LA_TRX) AS TOT_PAYOFF
	FROM OLWHRM1.DC11_LON_FAT
	WHERE LC_TRX_TYP = 'RH'
		AND LD_TRX_EFF BETWEEN &BEGIN AND &END
	GROUP BY BF_SSN
	) DC11
	ON DC01.BF_SSN = DC11.BF_SSN
WHERE AY01.PF_ACT IN ('DLHB4','DLMN2')
	AND AY01.BD_ATY_PRF > CURRENT DATE - 6 MONTHS
	AND DC01.LD_AUX_STA_UPD > CURRENT DATE - 1 MONTHS
	AND DC01.LC_AUX_STA = '10'
ORDER BY PD01.DF_SPE_ACC_ID,
	AY01.PF_ACT,
	AY01.BD_ATY_PRF
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
/*%put  sqlxrc= >>> &sqlxrc <<< ||| sqlxmsg= >>> &sqlxmsg >>> ;  ** includes error messages to SAS log  ;*/
/*%sqlcheck;*/
/*quit;*/
ENDRSUBMIT;

DATA DEMO; SET WORKLOCL.DEMO; RUN;

DATA DEMO;
SET DEMO;
BY DF_SPE_ACC_ID;
IF FIRST.DF_SPE_ACC_ID THEN BR_CNT = 1;
ELSE BR_CNT = 0;	
RUN;

DATA DEMO;
SET DEMO;
FORMAT REV 10.2;
REV = TOT_PAYOFF * .32;
RUN;

PROC SORT DATA=DEMO;
BY REV;
RUN;

PROC SQL;
CREATE TABLE TOTALS AS
SELECT SUM(BR_CNT) AS BR_CNT
	,SUM(REV) AS TOT_REV
FROM DEMO
;
QUIT;

DATA TOTALS;
SET TOTALS;
FORMAT AVG_COLL 10.2;
AVG_COLL = TOT_REV / BR_CNT;
RUN;


PROC PRINTTO PRINT=REPORT2 NEW;
RUN;

/*For portrait reports;*/
OPTIONS ORIENTATION = PORTRAIT;
OPTIONS CENTER NODATE NUMBER PAGENO=1 PS=52 LS=96;
PROC CONTENTS DATA=DEMO OUT=EMPTYSET NOPRINT;
/*PORTRAIT*/
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 96*'-';
	PUT      ////////
		@35 '**** NO RECORDS FOUND ****';
	PUT ////////
		@38 '-- END OF REPORT --';
	PUT ////////////////
		@27 "JOB = UTLWD33     REPORT = ULWD33.LWD33R2";
	END;
RETURN;
TITLE "Increase Collection Revenue - Track  Rehab Results";
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=DEMO;
FORMAT TOT_PAYOFF DOLLAR17.2 REV DOLLAR17.2 BD_ATY_PRF MMDDYY10.;
VAR DF_SPE_ACC_ID TOT_PAYOFF REV PF_ACT BD_ATY_PRF;
LABEL	DF_SPE_ACC_ID = 'ACCT #'
		TOT_PAYOFF = 'BALANCE'
		REV = 'UHEAA REVENUE'
		PF_ACT = 'ACTION CODE'
		BD_ATY_PRF = 'ACCTION DATE'
		;

TITLE 'Increase Collection Revenue - Track  Rehab Results';
FOOTNOTE  'JOB = UTLWD33     REPORT = UTLWD33.LWD33R2';
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=TOTALS;
FORMAT TOT_REV DOLLAR17.2 AVG_COLL DOLLAR17.2 ;
VAR TOT_REV BR_CNT AVG_COLL;
LABEL	TOT_REV = 'TOTAL REVENUE'
		BR_CNT = 'TOTAL BORROWERS'
		AVG_COLL = 'AVERAGE COLLECTED'
		;

TITLE 'Increase Collection Revenue - Track  Rehab Results';
TITLE2 'TOTALS';
FOOTNOTE  'JOB = UTLWD33     REPORT = UTLWD33.LWD33R2';
RUN;

PROC PRINTTO;
RUN;