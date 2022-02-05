/*UTLWG28 TERM BEGIN AND END DATE DISCREPANCIES*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWG28.LWG28R2";
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);

CREATE TABLE LO AS
SELECT SSN
	,CLID
	,AF_APL_ID
	,CUR_LDR
	,AN_SEQ
	,LO_GRD_LVL
	,CASE
		WHEN LO_GRD_LVL = '6' 	THEN 'A'
		WHEN LO_GRD_LVL = '7' 	THEN 'B'
		WHEN LO_GRD_LVL = '8' 	THEN 'C'
		WHEN LO_GRD_LVL IN (
			'9','10','11','12',
			'13','14','15')		THEN 'D'
	 END AS LO_TRANS_GRD_LVL
	,LO_TRM_BEG
	,LO_TRM_END
	,LO_XPC_GRD
	,GRD_LVL
	,TRM_BEG
	,TRM_END
	,XPC_GRD	
	,INPUT(AX_APL_FLD_CUR,MMDDYY10.) AS AP33_DT FORMAT=MMDDYY10.
	,AP33_IND
	,SYSTEM
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.BF_SSN						AS SSN
	,A.AF_CNL||A.AF_CNL_SFX			AS CLID
	,A.AF_APL_ID
	,A.AF_DOE_LDR					AS CUR_LDR
	,A.AN_SEQ
	,A.AC_SCL_ACA_GDE_LEV 			AS LO_GRD_LVL
	,A.AD_SCL_LON_TRM_BEG			AS LO_TRM_BEG
	,A.AD_SCL_LON_TRM_END			AS LO_TRM_END
/*	,A.AD_STU_GRD_XPC				AS LO_XPC_GRD*/
	,A.AD_SCL_GRD_XPC				AS LO_XPC_GRD
	,A.AC_SCL_ACA_GDE_LEV			AS GRD_LVL
	,A.AD_SCL_LON_TRM_BEG			AS TRM_BEG
	,A.AD_SCL_LON_TRM_END			AS TRM_END
/*	,A.AD_STU_GRD_XPC				AS XPC_GRD*/
	,A.AD_SCL_GRD_XPC AS XPC_GRD
	,B.AX_APL_FLD_CUR
	,B.AP33_IND
	,'LO'							AS SYSTEM
	,C.DF_SPE_ACC_ID
FROM OLWHRM1.AP03_MASTER_APL A
LEFT OUTER JOIN 
	(SELECT AF_APL_ID
		,AX_APL_FLD_CUR
		,'Y' AS AP33_IND
	 FROM OLWHRM1.AP33_LO_APL_DTL A
	 WHERE DAYS(AF_LST_DTS_AP33) = DAYS(CURRENT DATE)-1
	 AND PF_LO_SCR_FLD_HST = 'SC07'
	 ) B
	ON A.AF_APL_ID = B.AF_APL_ID
LEFT OUTER JOIN OLWHRM1.PD10_PRS_NME C
	ON A.BF_SSN = C.DF_PRS_ID

WHERE   A.IF_GTR IN ('000749')
		AND A.AF_CNL NOT LIKE '%REALLO%'
		AND A.IC_LON_PGM <> 'TILP'
FOR READ ONLY WITH UR
);

CREATE TABLE LS AS
SELECT 
	SSN
	,CLID			LENGTH = 19
	,LN_SEQ						
	,LC_STA_LON10
	,WC_DW_LON_STA
	,LA_CUR_PRI
	,CUR_LDR
	,NAME
	,AN_SEQ
	,LS_GRD_LVL
	,CASE
		WHEN LS_GRD_LVL = '6' 	THEN 'A'
		WHEN LS_GRD_LVL = '7' 	THEN 'B'
		WHEN LS_GRD_LVL = '8' 	THEN 'C'
		WHEN LS_GRD_LVL IN (
			'9','10','11','12',
			'13','14','15')		THEN 'D'
	 END AS LS_TRANS_GRD_LVL
	,LS_TRM_BEG
	,LS_TRM_END
	,LS_XPC_GRD
	,GRD_LVL
	,TRM_BEG
	,TRM_END
	,XPC_GRD
	,SYSTEM
	,DF_SPE_ACC_ID
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.BF_SSN						AS SSN
	,RTRIM(CHAR(A.LF_LON_ALT))||'0'||CHAR(A.LN_LON_ALT_SEQ) AS CLID
	,A.LN_SEQ						
	,A.LC_STA_LON10
	,B.WC_DW_LON_STA
	,A.LA_CUR_PRI
	,A.IF_DOE_LDR					AS CUR_LDR
	,C.DM_PRS_LST					AS NAME
	,D.AN_SEQ
	,A.LC_ACA_GDE_LEV				AS LS_GRD_LVL
	,A.LD_TRM_BEG					AS LS_TRM_BEG
	,A.LD_TRM_END					AS LS_TRM_END
	,CASE 
		WHEN A.IC_LON_PGM NOT IN ('PLUS','PLUSGB') 
			THEN D.LD_SCL_SPR
		WHEN A.IC_LON_PGM IN ('PLUS','PLUSGB') AND D.LD_SCL_SPR IS NULL 
			THEN E.LD_SCL_SPR
		WHEN A.IC_LON_PGM IN ('PLUS','PLUSGB') AND D.LD_SCL_SPR IS NOT NULL 
			THEN D.LD_SCL_SPR
	 END AS LS_XPC_GRD
	,A.LC_ACA_GDE_LEV				AS GRD_LVL
	,A.LD_TRM_BEG					AS TRM_BEG
	,A.LD_TRM_END					AS TRM_END
	,CASE 
		WHEN A.IC_LON_PGM NOT IN ('PLUS','PLUSGB') 
			THEN D.LD_SCL_SPR
		WHEN A.IC_LON_PGM IN ('PLUS','PLUSGB') AND D.LD_SCL_SPR IS NULL 
			THEN E.LD_SCL_SPR
		WHEN A.IC_LON_PGM IN ('PLUS','PLUSGB') AND D.LD_SCL_SPR IS NOT NULL 
			THEN D.LD_SCL_SPR
	 END AS XPC_GRD
	,C.DF_SPE_ACC_ID
	,'LS'							AS SYSTEM
FROM 	OLWHRM1.LN10_LON A
		INNER JOIN OLWHRM1.DW01_DW_CLC_CLU B ON
			A.BF_SSN = B.BF_SSN
			AND A.LN_SEQ = B.LN_SEQ
			AND A.IF_GTR IN ('000749')
			AND A.LF_LON_ALT NOT LIKE '%REALLO%'
			AND A.LC_STA_LON10 IN ('R','L')
		INNER JOIN OLWHRM1.PD10_PRS_NME C ON
			A.BF_SSN = C.DF_PRS_ID
		INNER JOIN OLWHRM1.GR10_RPT_LON_APL D ON
			A.BF_SSN = D.BF_SSN
			AND	A.LN_SEQ = D.LN_SEQ
		LEFT OUTER JOIN (
			SELECT LF_STU_SSN
				,MAX(LD_SCL_SPR) AS LD_SCL_SPR
			FROM OLWHRM1.SD10_STU_SPR
			WHERE LC_STA_STU10 = 'A'
			GROUP BY LF_STU_SSN
			) E
			ON A.LF_STU_SSN = E.LF_STU_SSN
		
		WHERE A.LA_CUR_PRI > 0
		AND A.LC_STA_LON10 = 'R'
FOR READ ONLY WITH UR
);

CREATE TABLE OL AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT
	A.DF_PRS_ID_BR					AS SSN
	,C.DM_PRS_LST					AS NAME
	,B.AF_APL_ID||B.AF_APL_ID_SFX 	AS CLID
	,B.AF_CUR_LON_OPS_LDR			AS CUR_LDR
	,A.AD_IST_TRM_BEG				AS OL_TRM_BEG
	,A.AD_IST_TRM_END				AS OL_TRM_END
	,A.AC_ACA_GDE_LEV       		AS OL_GRD_LVL
	,A.AC_ACA_GDE_LEV				AS OL_TRANS_GRD_LVL
	,D.LD_XPC_GRD					AS OL_XPC_GRD
	,A.AD_IST_TRM_BEG				AS TRM_BEG
	,A.AD_IST_TRM_END				AS TRM_END
	,A.AC_ACA_GDE_LEV	     		AS GRD_LVL
	,D.LD_XPC_GRD					AS XPC_GRD
	,'OL' 							AS SYSTEM
	,C.DF_SPE_ACC_ID
FROM 	OLWHRM1.GA01_APP A
		INNER JOIN OLWHRM1.GA10_LON_APP B ON
			A.AF_APL_ID = B.AF_APL_ID
		INNER JOIN OLWHRM1.PD01_PDM_INF C ON
			A.DF_PRS_ID_BR = C.DF_PRS_ID
/*		INNER JOIN OLWHRM1.SD02_STU_ENR D ON */
/*			A.DF_PRS_ID_STU = D.DF_PRS_ID_STU AND */
/*			D.LC_STA_SD02 = 'A'*/
		INNER JOIN (
			SELECT DISTINCT JL1B.AF_APL_ID
				,JL1B.AF_APL_ID_SFX 
				,JL1C.LD_XPC_GRD
			FROM OLWHRM1.GA01_APP JL1A
			INNER JOIN OLWHRM1.GA16_ENR_RPT_CRF JL1B
				ON JL1A.AF_APL_ID = JL1B.AF_APL_ID 
			INNER JOIN OLWHRM1.SD02_STU_ENR JL1C	
				ON JL1A.DF_PRS_ID_STU = JL1C.DF_PRS_ID_STU
				AND JL1B.LF_STU_ENR_RPT_SEQ = JL1C.LF_STU_ENR_RPT_SEQ
				AND JL1B.LF_CRT_DTS_SD02 = JL1C.LF_CRT_DTS_SD02
			WHERE JL1B.AC_STA_GA16 = 'E'
			AND JL1C.LC_STA_SD02 = 'A'
			) D
		ON B.AF_APL_ID = D.AF_APL_ID
		AND B.AF_APL_ID_SFX = D.AF_APL_ID_SFX
FOR READ ONLY WITH UR
);
DISCONNECT FROM DB2;
PROC SORT DATA=OL;
BY CLID;
RUN;

PROC SORT DATA=LS;
BY CLID AN_SEQ;
RUN;

PROC SORT DATA=LO;
BY CLID AN_SEQ;
RUN;
/*==========================================*/
/*GET LIST OF UNIQUE IDS TO USE IN REPORTING*/
/*==========================================*/
PROC SQL;
CREATE TABLE LS_LO AS
SELECT DISTINCT 
		A.*
		,B.LO_TRM_BEG
		,B.LO_TRM_END
		,B.LO_GRD_LVL
		,B.LO_TRANS_GRD_LVL
		,B.LO_XPC_GRD
		,B.AF_APL_ID
		,B.AP33_IND
FROM	LS A
		FULL OUTER JOIN LO B ON
			A.CLID = B.CLID
			AND A.AN_SEQ = B.AN_SEQ
ORDER BY A.CLID;
QUIT;
RUN;

DATA OL_LO_LS;
	MERGE OL LS_LO;
	BY CLID;
RUN;

DATA OL_LO_LS;
SET OL_LO_LS;
IF SUBSTR(OL_GRD_LVL,1,1) = '0' THEN OL_GRD_LVL = SUBSTR(OL_GRD_LVL,2,1);
ELSE IF SUBSTR(LO_GRD_LVL,1,1) = '0' THEN LO_GRD_LVL = SUBSTR(LO_GRD_LVL,2,1);
ELSE IF SUBSTR(LS_GRD_LVL,1,1) = '0' THEN LS_GRD_LVL = SUBSTR(LS_GRD_LVL,2,1);
RUN;

DATA OL_LO_LS;
SET OL_LO_LS;
IF CLID = '' THEN DELETE;
ELSE OUTPUT;
RUN;
/*endrsubmit;*/
/**/
/*RSUBMIT;*/
PROC SQL;
CREATE TABLE BEDD AS
SELECT DISTINCT CLID
	,NAME
	,LC_STA_LON10 
	,WC_DW_LON_STA 
	,LA_CUR_PRI
FROM OL_LO_LS
WHERE (OL_TRM_BEG IS NOT NULL AND LS_TRM_BEG IS NOT NULL AND OL_TRM_BEG NE LS_TRM_BEG)
OR (OL_TRM_BEG IS NOT NULL AND LO_TRM_BEG IS NOT NULL AND OL_TRM_BEG NE LO_TRM_BEG)
OR (LO_TRM_BEG IS NOT NULL AND LS_TRM_BEG IS NOT NULL AND LO_TRM_BEG NE LS_TRM_BEG)
OR (OL_TRM_END IS NOT NULL AND LS_TRM_END IS NOT NULL AND OL_TRM_END NE LS_TRM_END)
OR (OL_TRM_END IS NOT NULL AND LO_TRM_END IS NOT NULL AND OL_TRM_END NE LO_TRM_END)
OR (LO_TRM_END IS NOT NULL AND LS_TRM_END IS NOT NULL AND LO_TRM_END NE LS_TRM_END)
/*GRADE LEVEL EDITS*/
/*LO_TRANS_GRD_LVL LS_TRANS_GRD_LVL OL_TRANS_GRD_LVL*/
OR (OL_TRANS_GRD_LVL IS NOT NULL AND LS_TRANS_GRD_LVL IS NOT NULL AND OL_TRANS_GRD_LVL NE LS_TRANS_GRD_LVL)
OR (OL_TRANS_GRD_LVL IS NOT NULL AND LO_TRANS_GRD_LVL IS NOT NULL AND OL_TRANS_GRD_LVL NE LO_TRANS_GRD_LVL)
OR (LO_TRANS_GRD_LVL IS NOT NULL AND LS_TRANS_GRD_LVL IS NOT NULL AND LO_TRANS_GRD_LVL NE LS_TRANS_GRD_LVL)
/**********************************************************************************
* LEGACY
***********************************************************************************
OR (OL_GRD_LVL IS NOT NULL AND LS_GRD_LVL IS NOT NULL AND OL_GRD_LVL NE LS_GRD_LVL)
OR (OL_GRD_LVL IS NOT NULL AND LO_GRD_LVL IS NOT NULL AND OL_GRD_LVL NE LO_GRD_LVL)
OR (LO_GRD_LVL IS NOT NULL AND LS_GRD_LVL IS NOT NULL AND LO_GRD_LVL NE LS_GRD_LVL)
***********************************************************************************/

OR (OL_XPC_GRD IS NOT NULL AND LS_XPC_GRD IS NOT NULL AND OL_XPC_GRD NE LS_XPC_GRD 
	AND AP33_IND = 'Y')
OR (OL_XPC_GRD IS NOT NULL AND LO_XPC_GRD IS NOT NULL AND OL_XPC_GRD NE LO_XPC_GRD 
 	AND AP33_IND = 'Y')
OR (LO_XPC_GRD IS NOT NULL AND LS_XPC_GRD IS NOT NULL AND LO_XPC_GRD NE LS_XPC_GRD 
	AND AP33_IND = 'Y');
QUIT;

DATA BEDD;
SET BEDD;
LENGTH STA $4.;
IF LC_STA_LON10 = 'R' AND (WC_DW_LON_STA = '22' OR LA_CUR_PRI = 0) THEN STA = 'PIF ';
ELSE STA = 'OPEN';
RUN;

DATA BEDD;
SET BEDD;
IF STA = 'PIF ' THEN DELETE;
ELSE OUTPUT;
RUN;

/*==========================================*/
/*END LIST CALCULATIONS						*/
/*==========================================*/

PROC SORT DATA=LS; BY CLID; RUN;
PROC SORT DATA=LO; BY CLID; RUN;
PROC SORT DATA=OL; BY CLID; RUN;

DATA LTBEDDA (KEEP=SSN DF_SPE_ACC_ID CLID CUR_LDR LN_SEQ AF_APL_ID);
MERGE LS LO OL;
BY CLID;
RUN;

DATA LTBEDDB (KEEP=CLID SYSTEM GRD_LVL TRM_BEG TRM_END XPC_GRD);
SET LO LS OL;
RUN;

PROC SORT DATA=LTBEDDA; BY CLID; RUN;
PROC SORT DATA=LTBEDDB; BY CLID; RUN;

DATA LTBEDDU;
MERGE LTBEDDA LTBEDDB;
BY CLID;
RUN;

DATA LTBEDDU;
SET LTBEDDU;
IF SUBSTR(GRD_LVL,1,1) = '0' THEN GRD_LVL = SUBSTR(GRD_LVL,2,1);
ELSE GRD_LVL = GRD_LVL;
RUN;

DATA LTBEDDU;
MERGE LTBEDDU (IN=A) BEDD (IN=B);
BY CLID;
IF B;
RUN;
ENDRSUBMIT;

DATA LTBEDDU;
SET WORKLOCL.LTBEDDU;
RUN;

PROC SORT DATA=LTBEDDU;
BY DF_SPE_ACC_ID STA;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;

OPTIONS CENTER NUMBER PAGENO=1 ORIENTATION=LANDSCAPE ;
OPTIONS LS=132 PS=39;
TITLE	'LOAN TERM, GRADE LEVEL, GRAD DATE DISCREPANCY REPORT - OPEN LOANS';
FOOTNOTE  "JOB = UTLWG28     REPORT = ULWG28.LWG28R2";

PROC CONTENTS DATA=LTBEDDU OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 127*'-';
	PUT      //////
		@51 '**** NO DISBURSEMENTS FOUND ****';
	PUT ////////
		@57 '-- END OF REPORT --';
	PUT /////////////
		@46 "JOB = UTLWG28     REPORT = ULWG28.LWG28R2";
	END;
RETURN;
RUN;
PROC REPORT DATA=LTBEDDU NOWD HEADSKIP SPLIT='/';
COLUMN CUR_LDR DF_SPE_ACC_ID NAME AF_APL_ID LN_SEQ CLID SYSTEM TRM_BEG TRM_END GRD_LVL XPC_GRD;
DEFINE CUR_LDR / GROUP "LENDER" WIDTH=8;
DEFINE DF_SPE_ACC_ID / GROUP "ACCT #" WIDTH=10;
DEFINE NAME / GROUP "NAME" WIDTH=20;
DEFINE AF_APL_ID / GROUP "APP ID" WIDTH=9;
DEFINE LN_SEQ / GROUP "LN/SEQ" WIDTH=3;
DEFINE CLID / GROUP "UNIQUE ID" WIDTH=19;
DEFINE SYSTEM / GROUP "SYSTEM" WIDTH=6;
DEFINE TRM_BEG / ANALYSIS "TERM/BEGIN" FORMAT = MMDDYY10. WIDTH=10;
DEFINE TRM_END / ANALYSIS "TERM/END" FORMAT = MMDDYY10. WIDTH=10;
DEFINE GRD_LVL / DISPLAY "GRADE/LEVEL" WIDTH=5;
DEFINE XPC_GRD / ANALYSIS "EXPECTED/GRAD DATE" FORMAT = MMDDYY10. WIDTH=10;
RUN;
proc printto;
run;