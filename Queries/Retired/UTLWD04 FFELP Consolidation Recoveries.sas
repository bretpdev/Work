/*UTLWD04 - FEDERAL DIRECT CONSOLIDATION RECOVERIES*/

/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
%LET RPTLIB = %SYSGET(reportdir);
FILENAME REPORT2 "&RPTLIB/ULWD04.LWD04R2";*/

libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DCONS AS
SELECT SSN
	/*,CLUID*/
	,COALESCE(SUM(LA_TRX_EP),0) AS LA_TRX_EP
	,COALESCE(SUM(LA_TRX_GP),0) AS LA_TRX_GP
	,COALESCE(SUM(LA_TRX_BR),0) AS LA_TRX_BR
	,COALESCE(SUM(LA_TRX_ACDC),0) AS LA_TRX_ACDC
	,COALESCE(SUM(LA_TRX_CP),0) AS LA_TRX_CP
FROM CONNECTION TO DB2 (
	SELECT INTEGER(A.BF_SSN) as SSN
		,A.AF_APL_ID||A.AF_APL_ID_SFX	as CLUID
		,CASE WHEN C.LC_TRX_TYP = 'EP'
			THEN LA_TRX
		END AS LA_TRX_EP
		,CASE WHEN C.LC_TRX_TYP = 'GP'
			THEN LA_TRX
		END AS LA_TRX_GP
		,CASE WHEN C.LC_TRX_TYP = 'BR'
			THEN LA_TRX
		END AS LA_TRX_BR
		,CASE WHEN C.LC_TRX_TYP IN('DC','AC')
			THEN LA_TRX
		END AS LA_TRX_ACDC
		,CASE WHEN C.LC_TRX_TYP = 'CP'
			THEN LA_TRX
		END AS LA_TRX_CP

	FROM  OLWHRM1.DC01_LON_CLM_INF A 
	INNER JOIN 
		(SELECT DF_PRS_ID
			,MIN(BD_ATY_PRF) AS BD_ATY_PRF
		FROM OLWHRM1.AY01_BR_ATY
		WHERE PF_ACT = 'DCONS'
		AND BX_CMT LIKE '%FFELP%'
		GROUP BY DF_PRS_ID)B
		ON A.BF_SSN = B.DF_PRS_ID
	LEFT OUTER JOIN OLWHRM1.DC11_LON_FAT C
		ON A.AF_APL_ID = C.AF_APL_ID
		AND A.AF_APL_ID_SFX = C.AF_APL_ID_SFX
		AND A.LF_CRT_DTS_DC10 = C.LF_CRT_DTS_DC10

	WHERE A.LC_STA_DC10 IN ('03','04')
	AND C.LC_REV_IND_TYP = ' '
	AND C.LC_TRX_TYP IN ('EP','GP','BR','DC','CP','AC')
	AND C.LD_TRX_EFF >= B.BD_ATY_PRF
	)
GROUP BY SSN/*, CLUID*/;
DISCONNECT FROM DB2;
endrsubmit  ;

DATA DCONS; 
SET WORKLOCL.DCONS; 
RUN;

DATA DCONS; 
SET DCONS; 
TOTAL = SUM(LA_TRX_EP, LA_TRX_GP, LA_TRX_BR, LA_TRX_ACDC, LA_TRX_CP);
RUN;

PROC SORT DATA=DCONS; BY SSN; RUN;

DATA _NULL_;
CALL SYMPUT('RUNDATE',PUT(INTNX('DAY',TODAY(),0,'beginning'), MMDDYY10.));
RUN;

/*PROC PRINTTO PRINT=REPORT2;
RUN;*/
OPTIONS CENTER NODATE NUMBER PAGENO=1 LS=126 MISSING=0;
PROC REPORT DATA=DCONS NOWD SPACING=1 HEADSKIP SPLIT='~';
TITLE 'FFELP CONSOLIDATION RECOVERIES';
TITLE2 "RUN DATE:  &RUNDATE";
FOOTNOTE  'JOB = UTLWD04     REPORT = ULWD04.LWD04R2';
COLUMN SSN LA_TRX_EP LA_TRX_GP LA_TRX_BR LA_TRX_ACDC LA_TRX_CP TOTAL N;
DEFINE SSN / DISPLAY ORDER LEFT FORMAT=SSN11. "BORROWER~SSN";
DEFINE LA_TRX_EP / ANALYSIS FORMAT=COMMA12.2 "EP~PAYMENTS";
DEFINE LA_TRX_GP / ANALYSIS FORMAT=COMMA12.2 "GP~PAYMENTS";
DEFINE LA_TRX_BR / ANALYSIS FORMAT=COMMA12.2 "BR~PAYMENTS";
DEFINE LA_TRX_ACDC / ANALYSIS FORMAT=COMMA12.2 "AC/DC~PAYMENTS";
DEFINE LA_TRX_CP / ANALYSIS FORMAT=COMMA12.2 "CP~PAYMENTS";
DEFINE TOTAL / ANALYSIS FORMAT=DOLLAR14.2 "TOTAL~PAYMENTS";
DEFINE N / NOPRINT;
RBREAK AFTER / SUMMARIZE SKIP DOL SUPPRESS;

COMPUTE AFTER;
LINE ' ';
LINE ' ';
LINE 'BORROWER COUNT: ' N COMMA6.;
ENDCOMP;
RUN;

/*TESTFILE
libname  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=work  ;
RSUBMIT;
OPTIONS NOCENTER NODATE NONUMBER LS=132;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DCONS_TEST AS
SELECT *
FROM CONNECTION TO DB2 (
	SELECT INTEGER(A.BF_SSN) as SSN
		,A.AF_APL_ID||A.AF_APL_ID_SFX	as CLUID
		,CASE WHEN C.LC_TRX_TYP = 'EP'
			THEN LA_TRX
		END AS LA_TRX_EP
		,CASE WHEN C.LC_TRX_TYP = 'GP'
			THEN LA_TRX
		END AS LA_TRX_GP
		,CASE WHEN C.LC_TRX_TYP = 'BR'
			THEN LA_TRX
		END AS LA_TRX_BR
		,CASE WHEN C.LC_TRX_TYP IN('DC','AC')
			THEN LA_TRX
		END AS LA_TRX_ACDC
		,CASE WHEN C.LC_TRX_TYP = 'CP'
			THEN LA_TRX
		END AS LA_TRX_CP

	FROM  OLWHRM1.DC01_LON_CLM_INF A 
	INNER JOIN 
		(SELECT DF_PRS_ID
			,MIN(BD_ATY_PRF) AS BD_ATY_PRF
		FROM OLWHRM1.AY01_BR_ATY
		WHERE PF_ACT = 'DCONS'
		AND BX_CMT LIKE '%FFELP%'
		GROUP BY DF_PRS_ID)B
		ON A.BF_SSN = B.DF_PRS_ID
	LEFT OUTER JOIN OLWHRM1.DC11_LON_FAT C
		ON A.AF_APL_ID = C.AF_APL_ID
		AND A.AF_APL_ID_SFX = C.AF_APL_ID_SFX
		AND A.LF_CRT_DTS_DC10 = C.LF_CRT_DTS_DC10

	WHERE A.LC_STA_DC10 IN ('03','04')
	AND C.LC_REV_IND_TYP = ' '
	AND C.LC_TRX_TYP IN ('EP','GP','BR','DC','CP','AC')
	AND C.LD_TRX_EFF >= B.BD_ATY_PRF
	);
DISCONNECT FROM DB2;
endrsubmit  ;

DATA DCONS_TEST; 
SET WORKLOCL.DCONS_TEST; 
RUN;

PROC SORT DATA=DCONS_TEST; BY SSN; RUN;

PROC EXPORT DATA= DCONS_TEST
            OUTFILE= "T:\SAS\FELCONS_TEST.xls" 
            DBMS=EXCEL2000 REPLACE;
RUN;
*/