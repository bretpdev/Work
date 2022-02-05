/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWO91.LWO91RZ";
FILENAME REPORT2 "&RPTLIB/ULWO91.LWO91R2";
FILENAME REPORT3 "&RPTLIB/ULWO91.LWO91R3";
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
SELECT 	DISTINCT
		A.DF_SPE_ACC_ID
		,TRIM(A.DM_PRS_1)||' '||TRIM(A.DM_PRS_LST) AS NAME
		,B.LF_LON_ALT||'0'||CHAR(B.LN_LON_ALT_SEQ) AS CLUID
		,B.LN_SEQ
		,F.LD_DSB

		,B.IC_LON_PGM
		,B.LA_CUR_PRI
		,B.LC_STA_LON10 
		,C.WC_DW_LON_STA
		,D.LC_DFR_TYP
		,E.LC_STA_LON50

FROM OLWHRM1.DF10_BR_DFR_REQ D
INNER JOIN OLWHRM1.LN50_BR_DFR_APV E
	ON D.BF_SSN  = E.BF_SSN
	AND D.LF_DFR_CTL_NUM = E.LF_DFR_CTL_NUM
INNER JOIN OLWHRM1.LN10_LON B
	ON E.BF_SSN = B.BF_SSN
	AND E.LN_SEQ = B.LN_SEQ
INNER JOIN OLWHRM1.PD01_PDM_INF A
	ON D.BF_SSN = A.DF_PRS_ID
LEFT OUTER JOIN OLWHRM1.DW01_DW_CLC_CLU C
	ON B.BF_SSN = C.BF_SSN
	AND B.LN_SEQ = C.LN_SEQ
INNER JOIN (
	SELECT	MIN(Z.LD_DSB) AS LD_DSB
		,Z.BF_SSN
		,Z.LN_SEQ
	FROM OLWHRM1.LN15_DSB Z
	WHERE Z.LC_DSB_TYP = '2'
	GROUP BY Z.AF_APL_ID, 
		Z.BF_SSN, 
		Z.LN_SEQ
	) F
	ON B.BF_SSN = F.BF_SSN
	AND B.LN_SEQ = F.LN_SEQ
WHERE B.IC_LON_PGM IN ('PLUS','PLUSGB')
	AND D.LC_DFR_TYP = '37'
ORDER BY A.DF_SPE_ACC_ID
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
%MACRO O91_REPS(CRIT,RNO,RTITLE);
PROC SORT DATA=DEMO OUT=RDS(WHERE=(&CRIT)) NODUPKEY;
BY DF_SPE_ACC_ID NAME CLUID LN_SEQ LD_DSB;
RUN;

PROC PRINTTO PRINT=REPORT&RNO NEW;
RUN;

OPTIONS CENTER PAGENO=1 ORIENTATION=PORTRAIT ;
OPTIONS LS=96 PS=52;
TITLE	"&RTITLE";
FOOTNOTE4 	"JOB = UTLWO91     REPORT = ULWO91.LWO91R&RNO";

PROC CONTENTS DATA=RDS OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 96*'-';
	PUT      //////
		@28 '**** NO OBSERVATIONS FOUND ****';
	PUT //////
		@32 '-- END OF REPORT --';
	PUT //////////////
		@22 "JOB = UTLWO91  	 REPORT = ULWO91.LWO91R&RNO";
	END;
RETURN;
RUN;

PROC PRINT NOOBS SPLIT='/' DATA=RDS WIDTH=UNIFORM WIDTH=MIN;
FORMAT LD_DSB MMDDYY10.;
VAR	DF_SPE_ACC_ID NAME CLUID LN_SEQ LD_DSB;
LABEL DF_SPE_ACC_ID = "ACCOUNT NUMBER"
	NAME = "BORROWER NAME"
	LN_SEQ = "LOAN SEQUENCE NUMBER"
	LD_DSB = "FIRST DISBURSEMENT DATE";
RUN;

PROC PRINTTO;
RUN;
%MEND O91_REPS;

%O91_REPS(%STR(LA_CUR_PRI > 0 AND LC_STA_LON10 = 'R' AND WC_DW_LON_STA = '04' AND LC_STA_LON50 = 'A'),2,
	PLUS Loans in Exceptional Regulation Deferment);
%O91_REPS(LC_DFR_TYP EQ '37',3,All PLUS Exceptional Regulation Defr);
