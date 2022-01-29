/*PNOTE RECONCILIATION - NON-CLAIM
This job creates a _very_ large text file named 
"C:\WINDOWS\TEMP\PNOTE RECONCILE - NONCLAIM.TXT"*/

libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;

RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132;
PROC SQL STIMER;* OUTOBS=100;
CONNECT TO DB2 (DATABASE=DLGSUTWH);

CREATE TABLE PNMAIN AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT integer(A.DF_PRS_ID_BR) as SSN
	,B.AF_APL_ID||B.AF_APL_ID_SFX AS CLUID
	,RTRIM(D.DM_PRS_1) AS DM_PRS_1
	,RTRIM(D.DM_PRS_LST) AS DM_PRS_LST
	,B.AC_LON_TYP
	,A.AX_BR_REQ_IAA
	,B.AA_GTE_LON_AMT
	,A.AD_IST_TRM_BEG
	,A.AD_IST_TRM_END
	,COALESCE(A.AD_STU_SIG,A.AD_BR_SIG) AS STU_SIG
	,A.AF_APL_OPS_SCL
	,A.AF_CUR_APL_OPS_LDR
	,C.AC_LON_STA_TYP
	,CASE
		WHEN B.AC_LON_TYP = 'CL'
			THEN 'C'
		WHEN A.AF_BS_MPN_APL_ID IS NULL AND AC_ELS_LON <> 'E'
			THEN 'B'
		WHEN A.AF_BS_MPN_APL_ID IS NULL AND AC_ELS_LON = 'E'
			THEN 'E'
		WHEN A.AF_BS_MPN_APL_ID <> ' '
			THEN 'S'
	END AS MPN_TYP

	,A.AF_BS_MPN_APL_ID
	,A.AC_ELS_LON

FROM  OLWHRM1.GA01_APP A 
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.GA14_LON_STA C
	ON B.AF_APL_ID = C.AF_APL_ID
	AND B.AF_APL_ID_SFX = C.AF_APL_ID_SFX
INNER JOIN OLWHRM1.PD01_PDM_INF D
	on A.DF_PRS_ID_BR = D.DF_PRS_ID
WHERE B.AC_PRC_STA = 'A'
AND C.AC_STA_GA14 = 'A'
AND B.AC_LON_TYP IN ('SF','PL','SL','CL','SU')
AND B.AA_GTE_LON_AMT > 0
AND C.AC_LON_STA_TYP NOT IN ('CA','CP','DN','PC','PF','PM','PN','RF')
ORDER BY A.DF_PRS_ID_BR, B.AD_PRC
);

CREATE TABLE PNBASE AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT integer(A.DF_PRS_ID_BR) as SSN
	,B.AF_APL_ID||B.AF_APL_ID_SFX AS CLUID
	,RTRIM(D.DM_PRS_1) AS DM_PRS_1
	,RTRIM(D.DM_PRS_LST) AS DM_PRS_LST
	,B.AC_LON_TYP
	,A.AX_BR_REQ_IAA
	,B.AA_GTE_LON_AMT
	,A.AD_IST_TRM_BEG
	,A.AD_IST_TRM_END
	,COALESCE(A.AD_STU_SIG,A.AD_BR_SIG) AS STU_SIG
	,A.AF_APL_OPS_SCL
	,A.AF_CUR_APL_OPS_LDR
	,C.AC_LON_STA_TYP
	,'B' AS MPN_TYP

	,A.AF_BS_MPN_APL_ID
	,A.AC_ELS_LON
	,A.AF_APL_ID

FROM  OLWHRM1.GA01_APP A 
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.GA14_LON_STA C
	ON B.AF_APL_ID = C.AF_APL_ID
	AND B.AF_APL_ID_SFX = C.AF_APL_ID_SFX
INNER JOIN OLWHRM1.PD01_PDM_INF D
	on A.DF_PRS_ID_BR = D.DF_PRS_ID
WHERE B.AC_PRC_STA = 'A'
AND C.AC_STA_GA14 = 'A'
AND A.AF_BS_MPN_APL_ID IS NULL
);

DISCONNECT FROM DB2;

PROC SQL;
CREATE TABLE PNALL AS
SELECT DISTINCT B.*
FROM PNMAIN A
INNER JOIN PNBASE B
	ON A.AF_BS_MPN_APL_ID = B.AF_APL_ID
;
QUIT;

DATA PNALL;
SET PNMAIN PNALL;
PNOTE = '______';
RUN;

PROC SORT DATA=PNALL NODUPKEY;
BY SSN CLUID;
RUN;

endrsubmit  ;

DATA PNALL; SET WORKLOCL.PNALL /*(OBS=1000)*/; RUN;

PROC PRINTTO PRINT="C:\WINDOWS\TEMP\PNOTE RECONCILE - NONCLAIM.TXT" NEW;
RUN;

OPTIONS CENTER NODATE NUMBER PAGENO=1 LS=122;
PROC REPORT DATA=PNALL NOWD SPACING=1 HEADSKIP SPLIT='/';
TITLE "PROMISORRY NOTE RECONCILIATION REPORT - NON-CLAIM STATUS";
FOOTNOTE  'JOB = ON DEMAND     REPORT = PNOTE RECONCILIATION - NON-CLAIM';
COLUMN SSN
DM_PRS_1
DM_PRS_LST
AC_LON_TYP
AX_BR_REQ_IAA
AA_GTE_LON_AMT
AD_IST_TRM_BEG
AD_IST_TRM_END
STU_SIG
AF_APL_OPS_SCL
AF_CUR_APL_OPS_LDR
AC_LON_STA_TYP
MPN_TYP
PNOTE;
DEFINE SSN / DISPLAY GROUP "SSN" FORMAT=SSN11. LEFT;
DEFINE DM_PRS_1 / DISPLAY GROUP "FIRST NAME" WIDTH=10;
DEFINE DM_PRS_LST / DISPLAY GROUP "LAST NAME" WIDTH=15;
DEFINE AC_LON_TYP / DISPLAY "LOAN/TYPE" WIDTH=4;
DEFINE AX_BR_REQ_IAA / DISPLAY "REQ/LN AMT" ;
DEFINE AA_GTE_LON_AMT / DISPLAY "GUAR AMT" FORMAT=COMMA10.2;
DEFINE AD_IST_TRM_BEG / DISPLAY "LOAN/PD BEGIN" FORMAT=MMDDYY8.;
DEFINE AD_IST_TRM_END / DISPLAY "LOAN/PD END" FORMAT=MMDDYY8.;
DEFINE STU_SIG / DISPLAY "STD SIGN" FORMAT=MMDDYY8.;
DEFINE AF_APL_OPS_SCL / DISPLAY "SCHOOL";
DEFINE AF_CUR_APL_OPS_LDR / DISPLAY "ORIG/LNDR" WIDTH=6;
DEFINE AC_LON_STA_TYP / DISPLAY "STA" WIDTH=3;
DEFINE MPN_TYP / DISPLAY "APP/TYP" WIDTH=3;
DEFINE PNOTE / DISPLAY "P-NOTE?" WIDTH=7;

COMPUTE AFTER SSN;
LINE ' ';
ENDCOMP;
RUN;

PROC PRINTTO;
RUN;