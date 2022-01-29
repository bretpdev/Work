/*QSASE02 ZIONS TOTAL BORROWERS/LOANS/GUARANTEED AMOUNT FOR NEVADA SCHOOLS*/
/*THIS JOB USES BOTH THE COMPASS AND ZIONS DATA WAREHOUSES*/
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE ZTBLG_ZR AS /*ZIONS WAREHOUSE*/
SELECT *
FROM CONNECTION TO DB2 (
SELECT A.BF_SSN AS SSN
	,A.AN_SEQ AS APP_NUM
	,A.AF_DOE_SCL AS SCL_ID
	,C.IM_SCL_FUL AS SCL_NM
	,B.AA_LON_AMT_GTR AS GAMT
FROM OLWHRM1.AP10_APL_ZB A
INNER JOIN OLWHRM1.AP20_APL_LON_ZB B
	ON A.AN_SEQ = B.AN_SEQ
	AND A.BF_SSN = B.BF_SSN
INNER JOIN OLWHRM1.SC10_SCH_DMO_ZB C
	ON A.AF_DOE_SCL = C.IF_DOE_SCL
INNER JOIN (SELECT DISTINCT IF_DOE_SCL
			,IC_SCL_DOM_ST 
 			FROM OLWHRM1.SC25_SCH_DPT_ZB
			) E
	ON C.IF_DOE_SCL = E.IF_DOE_SCL
WHERE E.IC_SCL_DOM_ST = 'NV'
AND B.AF_DOE_LDR = '817455'
AND B.AD_LON_GTR BETWEEN '01/01/2003' AND '03/31/2004'
);

CREATE TABLE ZTBLG_CO AS 	/*COMPASS WAREHOUSE*/
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT AP03.BF_SSN AS SSN 
	,AP03.AN_SEQ AS APP_NUM
	,AP03.AF_DOE_SCL AS SCL_ID
	,SC10.IM_SCL_FUL AS SCL_NM
	,AP03.AA_GTR_LON AS GAMT
FROM OLWHRM1.AP03_MASTER_APL AP03
INNER JOIN OLWHRM1.SC10_SCH_DMO SC10
	ON AP03.AF_DOE_SCL = SC10.IF_DOE_SCL
INNER JOIN (SELECT DISTINCT IF_DOE_SCL
			,IC_SCL_DOM_ST 
 			FROM OLWHRM1.SC25_SCH_DPT
			) SC25
	ON SC10.IF_DOE_SCL = SC25.IF_DOE_SCL
WHERE AP03.AF_DOE_LDR = '817455'
AND SC25.IC_SCL_DOM_ST = 'NV'
AND AP03.AD_GTR_ORG BETWEEN '01/01/2003' AND '03/31/2004'
);
DISCONNECT FROM DB2;
ENDRSUBMIT;
DATA ZTBLG_ZR;SET WORKLOCL.ZTBLG_ZR;RUN;
DATA ZTBLG_CO;SET WORKLOCL.ZTBLG_CO;RUN;
PROC SORT DATA=ZTBLG_ZR ;
BY SSN;
RUN;
PROC SORT DATA=ZTBLG_CO ;
BY SSN;
RUN;

DATA ZUSE;
SET ZTBLG_ZR ZTBLG_CO;
RUN;

PROC SQL;
CREATE TABLE ZBANK AS 
SELECT SCL_ID
	,SCL_NM
	,COUNT(DISTINCT SSN) AS BCOUNT
	,COUNT(APP_NUM) AS LCOUNT
	,SUM(GAMT) AS TOT
FROM ZUSE
GROUP BY SCL_ID,SCL_NM;
QUIT;
RUN;

%MACRO FDATE(FMT);
   %GLOBAL FDATE;
   DATA _NULL_;
      CALL SYMPUT("FDATE",LEFT(PUT("&SYSDATE"D,&FMT)));
   RUN;
%MEND FDATE;
%FDATE(WORDDATE.);

OPTIONS PAGENO=1 LS=132 CENTER NODATE;
PROC PRINT NOOBS SPLIT='-' DATA=ZBANK;
FORMAT TOT DOLLAR10.2;
VAR SCL_ID
	SCL_NM
	BCOUNT
	LCOUNT
	TOT;
LABEL SCL_ID = 'SCHOOL CODE'
	SCL_NM = 'SCHOOL NAME'
	BCOUNT = 'TOTAL NUMBER OF BORROWERS'
	LCOUNT = 'TOTAL NUMBER OF LOANS'
	TOT = 'TOTAL AMOUNT GUARANTEED';
TITLE 'ZIONS TOTAL BORROWERS/LOANS/GUARANTEED AMOUNT FOR NEVADA SCHOOLS';
TITLE2 "RUN DATE: &FDATE";
FOOTNOTE  'JOB = QSASE02 	 REPORT = QSASE02.R2';
RUN;

/*DETAIL FILE*/
/*DATA _NULL_;*/
/*SET  WORK.ZUSE;*/
/*FILE 'C:\WINDOWS\TEMP\ZIONS' DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*PUT SSN $ ;*/
/*RUN;*/