/*UTLWG18 BORROWERS WITH PAST SEPARATION AND ANTICIPATED DISBURSEMENTS*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWG18.LWG18R2";

LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE BPSEPAD AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT H.DF_SPE_ACC_ID
		,E.AN_SEQ
		,A.LN_SEQ
		,RTRIM(CHAR(A.LF_LON_ALT))||'0'||CHAR(A.LN_LON_ALT_SEQ) AS CLUID
		,C.LD_SCL_SPR
		,E.IC_LON_PGM
		,D.LD_DSB
		,D.LN_LON_DSB_SEQ
		,CASE
			 WHEN D.LA_DSB_CAN IS NULL 
			 THEN D.LA_DSB
			 WHEN D.LA_DSB_CAN IS NOT NULL 
			 THEN D.LA_DSB - D.LA_DSB_CAN 
		 END AS DISAMNT
		,E.AF_DOE_SCL 
		,F.IM_SCL_FUL 
		,E.AF_DOE_LDR	

FROM OLWHRM1.LN10_LON A
INNER JOIN OLWHRM1.LN13_LON_STU_OSD B
	ON A.BF_SSN = B.BF_SSN 
	AND A.LN_SEQ = B.LN_SEQ
INNER JOIN OLWHRM1.SD10_STU_SPR C
	ON B.LF_STU_SSN = C.LF_STU_SSN
	AND B.LN_STU_SPR_SEQ = C.LN_STU_SPR_SEQ
INNER JOIN OLWHRM1.LN15_DSB D
	ON A.BF_SSN = D.BF_SSN 
	AND A.LN_SEQ = D.LN_SEQ
INNER JOIN OLWHRM1.AP03_MASTER_APL E
	ON D.AF_APL_ID = E.AF_APL_ID
	AND C.LF_STU_SSN = E.LF_STU_SSN
INNER JOIN OLWHRM1.SC10_SCH_DMO F
	ON E.AF_DOE_SCL = F.IF_DOE_SCL 
INNER JOIN OLWHRM1.DW01_DW_CLC_CLU G	
	ON A.BF_SSN = G.BF_SSN
	AND A.LN_SEQ = G.LN_SEQ
INNER JOIN OLWHRM1.PD01_PDM_INF H
	ON A.BF_SSN = H.DF_PRS_ID

WHERE A.LA_CUR_PRI > 0
AND B.LC_STA_LON13 = 'A'
AND C.LC_STA_STU10 = 'A'
AND D.LC_DSB_TYP = '1'
AND	D.LC_STA_LON15 IN ('1','3')
AND E.AC_STA_ORG_PRC = '51'  
AND	(A.LC_STA_LON10 <> 'D'
	 OR 
	 G.WC_DW_LON_STA = '22')
AND	(DAYS(C.LD_SCL_SPR) < DAYS(CURRENT DATE)
	 OR
	 C.LD_SCL_SPR < D.LD_DSB)
AND (D.LA_DSB_CAN IS NULL
	 OR
	 D.LA_DSB_CAN <> D.LA_DSB)
);
DISCONNECT FROM DB2;
ENDRSUBMIT;

DATA BPSEPAD;
SET WORKLOCL.BPSEPAD;
RUN;

PROC SORT DATA = BPSEPAD;
BY AF_DOE_SCL IM_SCL_FUL DF_SPE_ACC_ID;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 PAGENO=1;
PROC PRINT NOOBS SPLIT='/' DATA=BPSEPAD WIDTH=MIN ;
FORMAT LD_SCL_SPR MMDDYY8. LD_DSB MMDDYY8.;
BY AF_DOE_SCL IM_SCL_FUL;
PAGEBY AF_DOE_SCL;
VAR DF_SPE_ACC_ID
	AN_SEQ
	LN_SEQ
	CLUID
	LD_SCL_SPR
	IC_LON_PGM
	LD_DSB
	LN_LON_DSB_SEQ
	DISAMNT
	AF_DOE_LDR;
LABEL	DF_SPE_ACC_ID = 'ACCOUNT NUMBER'
		AN_SEQ = 'APP SEQ'
		LN_SEQ = 'LOAN SEQ'
		CLUID = 'UNIQUE ID'
		LD_SCL_SPR = 'SEP DATE '
		IC_LON_PGM = ' LOAN TYPE'
		LD_DSB = 'DISB DATE'
		LN_LON_DSB_SEQ = 'DISB #'
		DISAMNT = 'DISB AMOUNT'
		AF_DOE_SCL = 'SCHOOL CODE'
		AF_DOE_LDR = 'LENDER ID'
		IM_SCL_FUL = 'SCHOOL NAME';
TITLE	'BORROWERS WITH PAST ENROLLMENT & ANTICIPATED DISBURSEMENTS';
FOOTNOTE  'JOB = UTLWG18  	 REPORT = ULWG18.LWG18R2';
RUN;

PROC PRINTTO;
RUN;