/*UCCU TOTAL ESTIMATED DISBURSEMENTS*/

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK  ;
RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132;
PROC SQL STIMER;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE UCCU AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT RTRIM(X.YR)||'/'||RTRIM(X.MO) AS YRMO
,RTRIM(X.MONAME) AS MONAME
,X.AF_DOE_SCL
,X.IM_SCL_FUL
,COUNT( (X.BF_SSN||X.APP||X.DISB)) AS LN_CT
,SUM(X.NET_DSB) AS SUM
FROM
	(SELECT A.BF_SSN
		,CHAR(A.AN_SEQ) AS APP
		,CHAR(A.LN_LON_DSB_SEQ) AS DISB
		,A.LA_DSB - COALESCE(A.LA_DSB_CAN,0) AS NET_DSB
		,CHAR(YEAR(A.LD_DSB)) AS YR
		,CASE 
			WHEN (MONTH(A.LD_DSB)) < 10 
			THEN '0'||CHAR(MONTH(A.LD_DSB))
			ELSE CHAR(MONTH(A.LD_DSB))
			END AS MO
		,MONTHNAME(A.LD_DSB) AS MONAME
		,C.AF_DOE_SCL
		,D.IM_SCL_FUL
	FROM OLWHRM1.LN15_DSB A
	INNER JOIN OLWHRM1.AP20_APL_LON B
		ON B.BF_SSN = A.BF_SSN 
		AND B.AN_SEQ = A.AN_SEQ
		AND B.IC_LON_PGM =A.IC_LON_PGM
	INNER JOIN OLWHRM1.AP10_APL C
		ON C.BF_SSN = A.BF_SSN
		AND C.AN_SEQ = A.AN_SEQ
	INNER JOIN OLWHRM1.SC10_SCH_DMO D
		ON C.AF_DOE_SCL = D.IF_DOE_SCL
	WHERE B.AF_DOE_LDR = '829123'
	AND A.LC_DSB_TYP = '1'
	AND A.LC_STA_LON15 IN ('1','3')
	AND (A.LA_DSB_CAN IS NULL OR A.LA_DSB_CAN <> A.LA_DSB)
	AND A.LD_DSB_ROS_PRT IS NULL
	AND A.LC_LDR_DSB_MDM = ' '
	AND A.LD_DSB BETWEEN '08/01/2003' AND '05/31/2004'
	)X
GROUP BY X.YR, X.MO, X.MONAME, X.IM_SCL_FUL, X.AF_DOE_SCL
ORDER BY X.YR, X.MO, X.MONAME, X.IM_SCL_FUL, X.AF_DOE_SCL

);
DISCONNECT FROM DB2;
ENDRSUBMIT  ;

DATA UCCU;
SET WORKLOCL.UCCU;
RUN;

DATA _NULL_;
CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
RUN;

/*PROC PRINTTO PRINT=REPORT2;
RUN;*/
OPTIONS CENTER NODATE NUMBER PAGENO=1 LS=90;
PROC PRINT DATA=UCCU NOOBS SPLIT='*' WIDTH=UNIFORM WIDTH=MIN;  
VAR AF_DOE_SCL IM_SCL_FUL LN_CT SUM;  
LABEL YRMO='YEAR/MONTH' AF_DOE_SCL='SCHOOL ID' IM_SCL_FUL='SCHOOL NAME' LN_CT='DISBURSEMENTS' 
SUM='TOTAL DISBURSEMENT AMOUNT';
FORMAT LN_CT COMMA7. SUM DOLLAR14.2;

BY YRMO;  
SUMBY YRMO;  
SUM LN_CT SUM;  

TITLE1 "TOTAL ESTIMATED ANTICIPATED DISBURSEMENT REPORT";
TITLE2 "UTAH COMMUNITY CREDIT UNION";
TITLE3 "&RUNDATE";
FOOTNOTE  'JOB = ON DEMAND     REPORT = UCCU TOTAL ESTIMATED DISBS';
RUN;


/*TESTFILE

RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE UCCUTST AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.BF_SSN
	,A.AN_SEQ AS APP
	,A.LN_LON_DSB_SEQ AS DISB
	,A.LA_DSB - COALESCE(A.LA_DSB_CAN,0) AS NET_DSB
	,A.LD_DSB AS DISBDATE
	,C.AF_DOE_SCL
	,D.IM_SCL_FUL
FROM OLWHRM1.LN15_DSB A
INNER JOIN OLWHRM1.AP20_APL_LON B
	ON B.BF_SSN = A.BF_SSN 
	AND B.AN_SEQ = A.AN_SEQ
	AND B.IC_LON_PGM =A.IC_LON_PGM
INNER JOIN OLWHRM1.AP10_APL C
	ON C.BF_SSN = A.BF_SSN
	AND C.AN_SEQ = A.AN_SEQ
INNER JOIN OLWHRM1.SC10_SCH_DMO D
	ON C.AF_DOE_SCL = D.IF_DOE_SCL
WHERE B.AF_DOE_LDR = '829123'
AND A.LC_DSB_TYP = '1'
AND A.LC_STA_LON15 IN ('1','3')
AND (A.LA_DSB_CAN IS NULL OR A.LA_DSB_CAN <> A.LA_DSB)
AND A.LD_DSB_ROS_PRT IS NULL
AND A.LC_LDR_DSB_MDM = ' '
AND A.LD_DSB BETWEEN '08/01/2003' AND '05/31/2004'
);
DISCONNECT FROM DB2;
endrsubmit  ;

DATA UCCUTST; 
SET WORKLOCL.UCCUTST; 
RUN;

PROC EXPORT DATA= UCCUTST
            OUTFILE= "C:\WINDOWS\TEMP\UCCUTST.xls" 
            DBMS=EXCEL2000 REPLACE;
RUN;
*/