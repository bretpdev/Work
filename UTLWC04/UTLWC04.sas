/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWC04.LWC04R2";
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

DATA CLMS(DROP=BX_CMT star1-star21 I );
SET DLGSUTWH.AY01_BR_ATY(KEEP=DF_PRS_ID PF_ACT BX_CMT BF_CRT_DTS_AY01) ;
WHERE PF_ACT IN ('RCLMR','RCLRC') AND DATEPART(BF_CRT_DTS_AY01) > TODAY() - 90;
FORMAT CLAIMREC MMDDYY10.;
FORMAT CLID $19. ;
/*THE UNIQUE LOAN ID'S ARE LISTED IN THE COMMENT TEXT*/
	CLAIMREC = MDY(SUBSTR(BX_CMT,63,2),SUBSTR(BX_CMT,65,2),SUBSTR(BX_CMT,67,4));
		array star{21} star1-star21 (76 96 116 151 171 191 226 
		246 266 301 321 341 376 396 
		416 451 471 491 526 546 566);
		DO I=1 TO 21;
			CLID = SUBSTR(BX_CMT,star{I},19);
			IF CLID ^= '' THEN OUTPUT;
		END;
RUN;

PROC SORT DATA=CLMS; BY CLID BF_CRT_DTS_AY01 PF_ACT; RUN;

DATA CLMS;
SET CLMS;
BY CLID;
IF LAST.CLID AND PF_ACT = 'RCLMR';
RUN;

PROC SQL;
CREATE TABLE CLMSPD AS
SELECT DISTINCT B.DF_SPE_ACC_ID
	,B.DF_PRS_ID
	,B.DM_PRS_1
	,B.DM_PRS_LST
	,C.IF_OPS_LDR
	,C.LF_CUR_LON_SER_AGY
	,C.LC_PCL_REA
	,A.CLAIMREC
	,C.LD_STA_UPD_DC10
	,C.AF_APL_ID||C.AF_APL_ID_SFX AS CLID
	,C.LC_STA_DC10
	,C.LA_CLM_PRI
	,D.AA_OTS_ACR_INT
FROM CLMS A
INNER JOIN DLGSUTWH.PD01_PDM_INF B
	ON A.DF_PRS_ID = B.DF_PRS_ID
INNER JOIN DLGSUTWH.DC01_LON_CLM_INF C
	ON C.AF_APL_ID||C.AF_APL_ID_SFX = A.CLID
INNER JOIN (SELECT D.AF_APL_ID
				,D.AF_APL_ID_SFX
				,MAX(D.LD_STA_UPD_DC10) AS LD_STA_UPD_DC10		 
		 FROM	DLGSUTWH.DC01_LON_CLM_INF D
		 GROUP BY AF_APL_ID, AF_APL_ID_SFX) E
	ON C.AF_APL_ID = E.AF_APL_ID
	AND C.AF_APL_ID_SFX = E.AF_APL_ID_SFX
	AND C.LD_STA_UPD_DC10 = E.LD_STA_UPD_DC10
LEFT OUTER JOIN DLGSUTWH.GA10_LON_APP D
	ON C.AF_APL_ID = D.AF_APL_ID
	AND C.AF_APL_ID_SFX = D.AF_APL_ID_SFX
WHERE C.LC_STA_DC10 IN ('01')
	AND (C.BD_CLM_PKG_RTN IS NULL
	     OR C.BD_CLM_PKG_RTN < C.BD_CLM_PKG_RCV
		 OR C.BD_CLM_PKG_RS IS NOT NULL)
	AND C.LD_STA_UPD_DC10 LE DATEPART(A.BF_CRT_DTS_AY01) ;

/*SELECT THE CLAIMS ADDED THROUGH THE CAM SYSTEM*/
INSERT INTO CLMSPD
SELECT DISTINCT B.DF_SPE_ACC_ID
	,B.DF_PRS_ID
	,B.DM_PRS_1
	,B.DM_PRS_LST
	,C.IF_OPS_LDR
	,C.LF_CUR_LON_SER_AGY
	,C.LC_PCL_REA
	,DATEPART(C.BF_CRT_DTS_DC01) AS CLAIMREC FORMAT= MMDDYY10.
	,C.LD_STA_UPD_DC10
	,C.AF_APL_ID||C.AF_APL_ID_SFX AS CLID
	,C.LC_STA_DC10
	,C.LA_CLM_PRI
	,D.AA_OTS_ACR_INT
FROM DLGSUTWH.PD01_PDM_INF B
INNER JOIN DLGSUTWH.DC01_LON_CLM_INF C
	ON B.DF_PRS_ID = C.BF_SSN
INNER JOIN (SELECT D.AF_APL_ID
				,D.AF_APL_ID_SFX
				,MAX(D.LD_STA_UPD_DC10) AS LD_STA_UPD_DC10		 
		 FROM	DLGSUTWH.DC01_LON_CLM_INF D
		 GROUP BY AF_APL_ID, AF_APL_ID_SFX) E
	ON C.AF_APL_ID = E.AF_APL_ID
	AND C.AF_APL_ID_SFX = E.AF_APL_ID_SFX
	AND C.LD_STA_UPD_DC10 = E.LD_STA_UPD_DC10
LEFT OUTER JOIN DLGSUTWH.GA10_LON_APP D
	ON C.AF_APL_ID = D.AF_APL_ID
	AND C.AF_APL_ID_SFX = D.AF_APL_ID_SFX
WHERE C.LC_STA_DC10 IN ('01')
	AND C.BF_USR_CRT_DC01 = 'LCXGX'
	AND C.BF_CRT_DTS_DC01 > TODAY() - 90
	AND C.BC_CLM_PKG_RTN_REA = ''
;
QUIT;
ENDRSUBMIT;
DATA CLMSPD;SET WORKLOCL.CLMSPD;RUN;

PROC SORT DATA=CLMSPD; 
BY CLAIMREC LC_PCL_REA DF_SPE_ACC_ID IF_OPS_LDR LF_CUR_LON_SER_AGY CLID;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;
TITLE	'CLAIMS IN PROCESS';
FOOTNOTE  'JOB = UTLWC04     REPORT = ULWC04.LWC04R2';
OPTIONS CENTER PAGENO=1 ORIENTATION=LANDSCAPE;
OPTIONS LS=128 PS=39;
PROC PRINT NOOBS SPLIT='/' DATA=CLMSPD WIDTH=UNIFORM WIDTH=MIN 
	N='CLAIMS IN PROCESS FOR RECEIVED DATE = ' 'TOTAL CLAIMS IN PROCESS = ';
FORMAT LA_CLM_PRI AA_OTS_ACR_INT DOLLAR14.2;
BY CLAIMREC;
SUMBY CLAIMREC;
VAR LC_PCL_REA DF_SPE_ACC_ID DM_PRS_LST DM_PRS_1 IF_OPS_LDR LF_CUR_LON_SER_AGY CLID LA_CLM_PRI AA_OTS_ACR_INT;
LABEL	 CLAIMREC = 'CLAIM RECEIVED DATE '
		DF_SPE_ACC_ID = 'ACCT'
		DM_PRS_1 = 'FIRST NAME'
		DM_PRS_LST = 'LAST NAME'
		IF_OPS_LDR = 'HOLDER'
		LF_CUR_LON_SER_AGY = 'SERVICER'
		LC_PCL_REA = 'CLAIM TYPE'
		CLID = 'UNIQUE ID'
		LA_CLM_PRI = 'CLAIM PRINCIPAL'
		AA_OTS_ACR_INT = 'CLAIM INTEREST';
RUN;
PROC PRINTTO;
RUN;