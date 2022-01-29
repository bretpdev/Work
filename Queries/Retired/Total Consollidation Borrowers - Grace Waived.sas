*------------------------------------------------*
|  TOTAL CONSOLIDATION BORROWERS - GRACE WAIVED  |
*------------------------------------------------*;

/*==========MARCO VARIALBE DEFINITION==========*/
%LET REPORT2 = T:\SAS\Consol Borrowers with Waived Grace.txt;
%LET CDATE = '04/01/2005';
DATA _NULL_;
CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
CALL SYMPUT('RUNTIME',PUT(TIME(), TIME.));
RUN;
/*============MACRO DEFINITIONS=================*/
%MACRO GET_COUNT(BYVARS,FVAR,NVAR);
PROC SORT DATA=XEMO2;
	BY &BYVARS;
RUN;
DATA XEMO2;
	SET XEMO2;
	BY &BYVARS;
	IF FIRST.&FVAR THEN &NVAR=1;
	ELSE &NVAR = 0;
RUN;
%MEND GET_COUNT;
/*=============================================*/
%SYSLPUT CDATE = &CDATE;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE XEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT 
	A.DF_LCO_PRS_SSN_BR		AS SSN
	,A.AN_LCO_APL_SEQ 		AS APP_SEQ
	,B.LN_LCO_UND_LN_SEQ 	AS UND_SEQ 
	,A.AD_LCO_APL_DSB 		AS DISB_DT
	,CASE
		WHEN MONTH(AD_LCO_APL_DSB) < 10 
		THEN RTRIM('0')||RTRIM(CHAR(MONTH(AD_LCO_APL_DSB)))||'/'||CHAR(YEAR(AD_LCO_APL_DSB))
		ELSE RTRIM(CHAR(MONTH(AD_LCO_APL_DSB)))||'/'||CHAR(YEAR(AD_LCO_APL_DSB))
	 END AS DISB_MOYR
	,COALESCE(B.LA_UND_LN_ACL_PAY,0) AS DISB_AMT
	,MONTH(AD_LCO_APL_DSB) AS MONTH
	,YEAR(AD_LCO_APL_DSB) AS YEAR

FROM OLWHRM1.AP1A_LCO_APL A
INNER JOIN OLWHRM1.LC10_UND_LN_INF B
	ON A.DF_LCO_PRS_SSN_BR = B.DF_LCO_PRS_SSN_BR
	AND A.AN_LCO_APL_SEQ = B.AN_LCO_APL_SEQ
INNER JOIN OLWHRM1.AY10_BR_LON_ATY C
	ON A.DF_LCO_PRS_SSN_BR = C.BF_SSN
INNER JOIN OLWHRM1.LN85_LON_ATY D
	ON C.LN_ATY_SEQ = D.LN_ATY_SEQ
	AND C.BF_SSN = D.BF_SSN
INNER JOIN OLWHRM1.LN10_LON E
	ON D.BF_SSN = E.BF_SSN
	AND D.LN_SEQ = E.LN_SEQ
	
WHERE A.AD_LCO_APL_DSB >= &CDATE
AND C.PF_REQ_ACT = 'WVGRC'
AND C.LC_STA_ACTY10 = 'A'
AND COALESCE(B.LA_UND_LN_ACL_PAY,0) <> 0
FOR READ ONLY WITH UR
);

CREATE TABLE FILTER AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT B.BF_SSN 
FROM OLWHRM1.LC10_UND_LN_INF A
INNER JOIN OLWHRM1.LN10_LON B
	ON A.DF_LCO_PRS_SSN_BR = B.BF_SSN 
	AND SUBSTR(A.LF_UND_LN_ACC_NUM,1,4) = 'L0UT'	
WHERE EXISTS (
	SELECT *
	FROM OLWHRM1.LN10_LON X
	INNER JOIN OLWHRM1.SD10_STU_SPR Y
		ON X.LF_STU_SSN = Y.LF_STU_SSN
	WHERE X.BF_SSN = B.BF_SSN
	AND X.LN_SEQ = INTEGER(SUBSTR(A.LF_UND_LN_ACC_NUM,8,2))
	AND Y.LD_SCL_SPR = X.LD_END_GRC_PRD
	AND Y.LC_STA_STU10 = 'A'
	)
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA XEMO;SET WORKLOCL.XEMO;RUN;
DATA FILTER;SET WORKLOCL.FILTER;RUN;

PROC SQL;
CREATE TABLE XEMO2 AS 
SELECT A.SSN
	,A.DISB_MOYR
	,A.MONTH
	,A.YEAR
	,SUM(A.DISB_AMT) AS DISB_AMT
FROM XEMO A
INNER JOIN FILTER B
	ON A.SSN = B.BF_SSN
GROUP BY A.SSN
	,A.DISB_MOYR
	,A.MONTH
	,A.YEAR
;
QUIT;
%GET_COUNT(DESCENDING DISB_MOYR SSN,SSN,SSN_BY_MONTH);

PROC SORT DATA=XEMO2;BY YEAR MONTH;RUN;

PROC PRINTTO PRINT="&REPORT2" NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 PAGENO=1 NODATE CENTER;
TITLE 'TOTAL CONSOLIDATION LOANS - BORROWER WAIVED GRACE';
TITLE2 "&RUNDATE - &RUNTIME";
FOOTNOTE 'JOB = TOTAL CONSOLIDATION BORROWERS - GRACE WAIVED  	 REPORT = R2';
PROC REPORT DATA=XEMO2 NOWD SPLIT='#' HEADSKIP SPACING=10;
COLUMN YEAR MONTH DISB_MOYR SSN_BY_MONTH DISB_AMT BOR_MONTHLY DISB_MONTHLY;
DEFINE MONTH / GROUP NOPRINT;
DEFINE YEAR / GROUP NOPRINT;
DEFINE DISB_MOYR / GROUP "DISBURSEMENT#MONTH/YEAR" WIDTH=12;
DEFINE SSN_BY_MONTH / ANALYSIS NOPRINT; 
DEFINE DISB_AMT / ANALYSIS NOPRINT;
DEFINE BOR_MONTHLY / COMPUTED "TOTAL BORROWERS#PER MONTH" WIDTH=15 FORMAT=COMMA6. ;
DEFINE DISB_MONTHLY / COMPUTED "TOTAL DISB AMOUNT#PER MONTH" WIDTH=22 FORMAT=DOLLAR20.2;
COMPUTE BOR_MONTHLY;
	BOR_MONTHLY = SSN_BY_MONTH.SUM;
ENDCOMP;
COMPUTE DISB_MONTHLY;
	DISB_MONTHLY = DISB_AMT.SUM;
ENDCOMP;
COMPUTE AFTER;
	LINE ' ';
	LINE 80*'-';
	LINE ' ';
	LINE @43 'TOTAL NUMBER OF BORROWERS: ' SSN_BY_MONTH.SUM COMMA7.;
	LINE @43 'TOTAL AMOUNT DISBURSED: ' DISB_AMT.SUM DOLLAR18.2;
	LINE ' ';
ENDCOMP;
RUN;
PROC PRINTTO;
RUN;
*------------------------------------------------*
| 			  CREATE DETAIL FILES  			     |
*------------------------------------------------*;
/*PROC SQL;*/
/*CREATE TABLE DETAIL AS */
/*SELECT DISTINCT A.**/
/*FROM XEMO A*/
/*INNER JOIN FILTER B*/
/*	ON A.SSN = B.BF_SSN*/
/*;*/
/*QUIT;*/
/*DATA DETAIL_NODUPS;*/
/*SET DETAIL;*/
/*RUN;*/
/*PROC SORT DATA=DETAIL_NODUPS NODUPKEY;BY SSN;RUN;*/
/*PROC EXPORT DATA=DETAIL*/
/*            OUTFILE= "T:\SAS\DETAIL.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/
/*PROC EXPORT DATA=DETAIL_NODUPS*/
/*            OUTFILE= "T:\SAS\DETAIL_NODUPS.xls" */
/*            DBMS=EXCEL2000 REPLACE;*/
/*RUN;*/