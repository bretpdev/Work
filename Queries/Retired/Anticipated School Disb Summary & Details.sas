DATA _NULL_;
CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
CALL SYMPUT('RUNTIME',PUT(TIME(), TIME.));
RUN;

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK  ;
RSUBMIT;

OPTION SYMBOLGEN;
PROC SQL STIMER;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE EDSBX AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT B.AF_DOE_LDR
	,E.IM_LDR_FUL
	,A.BF_SSN
	,A.LD_DSB
	,CHAR(A.AN_SEQ) AS APP
	,CHAR(A.LN_LON_DSB_SEQ) AS DISB
	,A.LA_DSB - COALESCE(A.LA_DSB_CAN,0) AS NET_DSB
	,CASE 
		WHEN (MONTH(A.LD_DSB)) < 10 
		THEN '0'||RTRIM(CHAR(MONTH(A.LD_DSB)))||'/'||CHAR(YEAR(A.LD_DSB))
		ELSE RTRIM(CHAR(MONTH(A.LD_DSB)))||'/'||CHAR(YEAR(A.LD_DSB))
		END AS YRMO
	,B.AF_DOE_SCL 
	,D.IM_SCL_FUL
FROM OLWHRM1.LN15_DSB A
INNER JOIN OLWHRM1.AP03_MASTER_APL B 
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.SC10_SCH_DMO D
	ON B.AF_DOE_SCL = D.IF_DOE_SCL 
INNER JOIN OLWHRM1.LR10_LDR_DMO E
	ON B.AF_DOE_LDR = E.IF_DOE_LDR 
WHERE A.LC_DSB_TYP = '1'
AND A.LC_STA_LON15 IN ('1','3')
AND (A.LA_DSB_CAN IS NULL OR A.LA_DSB_CAN <> A.LA_DSB)
AND A.LD_DSB_ROS_PRT IS NULL
AND A.LC_LDR_DSB_MDM = ' '
AND SUBSTR(B.AF_DOE_SCL,1,6) IN (
	'002004','002014','002016','002029','002015','002032',
	'015130','002023','004626','002026','002430','001029',
	'001057','002007','002010','014682','002005','002024',
	'002402','002410','002417','002423','002424','002441',
	'014721','016998','013952','016999','013919','007333',
	'014675','005483','014911','005471','015187','012413',
	'005482','002418','002419','009621','002022','002447',
	'002411') 
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA EDSB;
SET WORKLOCL.EDSBX;
RUN;

PROC SQL;
CREATE TABLE EDSB AS 
SELECT AF_DOE_SCL
	,YRMO 
	,IM_SCL_FUL 
	,BF_SSN 
	,NET_DSB
	,COUNT((X.BF_SSN||X.APP||X.DISB)) AS DSB_CT
	,SUM(X.NET_DSB) AS TOT
FROM EDSB X
GROUP BY AF_DOE_SCL,YRMO,IM_SCL_FUL,BF_SSN,NET_DSB
;
QUIT;
RUN;

DATA EDSB;
SET EDSB;
*Key Bank;
IF AF_DOE_LDR = '813760UT' THEN AF_DOE_LDR = '813760';
RUN;

PROC SORT DATA=EDSB;
BY AF_DOE_SCL YRMO BF_SSN;
RUN;

OPTIONS ORIENTATION = PORTRAIT;
OPTIONS PS=52 LS=96 CENTER NONUMBER PAGENO=1 NOBYLINE NODATE;
TITLE  'TOTAL ANTICIPATED DISBURSEMENT REPORT';
TITLE2 "&RUNDATE - &RUNTIME";
FOOTNOTE "JOB = ANTICIPATED DISB BY SCHOOL     REPORT = R2";
PROC CONTENTS DATA=EDSB OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
   PUT // 90*'-';
   PUT      ////////
       @31 '**** NO OBSERVATIONS FOUND ****';
   PUT ////////
       @37 '-- END OF REPORT --';
   PUT ///////////////
   		@26 "JOB = ANTICIPATED DISB BY SCHOOL     REPORT = R2";
   END;
RETURN;
RUN;
PROC REPORT DATA=EDSB NOWD SPACING=2 HEADSKIP SPLIT='*';
BY AF_DOE_SCL /*YRMO*/;
COLUMN AF_DOE_SCL YRMO IM_SCL_FUL BF_SSN NET_DSB DSB_CT TOT;
DEFINE AF_DOE_SCL / ORDER "SCHOOL ID" WIDTH=9; 
DEFINE YRMO / ORDER "MONTH/YEAR" WIDTH=10;
DEFINE IM_SCL_FUL / DISPLAY "SCHOOL NAME" WIDTH=40;
DEFINE BF_SSN / DISPLAY "SSN" WIDTH=9;
DEFINE NET_DSB / ANALYSIS "DISB AMNT" FORMAT=DOLLAR12.2 WIDTH=14;
DEFINE DSB_CT / ANALYSIS NOPRINT ;
DEFINE TOT / ANALYSIS NOPRINT;
BREAK AFTER YRMO / SUMMARIZE SKIP OL SUPPRESS;
COMPUTE AFTER AF_DOE_SCL;
LINE ' ';
LINE 'TOTAL ANTICIPATED DISBURSEMENTS       - ' @40 DSB_CT.SUM COMMA6.;
LINE 'TOTAL ANTICIPATED DISBURSEMENT AMOUNT - ' @40 TOT.SUM DOLLAR14.2;
ENDCOMP;
RUN;