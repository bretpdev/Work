/*UTLWO03 - Identify loans that have exceeded the maximum 10 year repayment period.*/
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = /sas/whse/progrevw   ;*/
%LET RPTLIB = T:\SAS ;
FILENAME REPORT2 "&RPTLIB/ULWO03.LWO03R2";
DATA _NULL_;
	CALL SYMPUT('NTCDT',"'"||PUT(INTNX('YEAR',TODAY(),-10,'S'), MMDDYYD10.)||"'");
	CALL SYMPUT('CNSDT',"'"||PUT(INTNX('YEAR',TODAY(),-30,'S'), MMDDYYD10.)||"'");
RUN;
%SYSLPUT NTCDT = &NTCDT;
%SYSLPUT CNSDT = &CNSDT;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK  ;
RSUBMIT;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE DEMO AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT E.DF_SPE_ACC_ID 	AS ACCNUM
	,E.DM_PRS_LST					AS NAME
	,B.AF_APL_ID||B.AF_APL_ID_SFX 	AS CLUID
	,B.AF_CUR_LON_OPS_LDR 			AS HOLDER
	,B.AF_CUR_LON_SER_AGY 			AS SERVICER
	,B.AC_LON_TYP
	,B.AD_PRC
	,B.AA_GTE_LON_AMT
	,E.AC_LON_STA_TYP 				AS LONSTA
	,E.AC_STA_GA14
	,E.AD_LON_STA
	,D.AD_NDS_CLC_ENT_RPD 			AS REPAYDT
	,F.AD_DSB_ADJ					AS DISB1DT
	,G.LC_STA_DC10 
	,G.LC_AUX_STA 
	,G.LD_RHB 
FROM OLWHRM1.GA01_APP A 
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.GA14_LON_STA E
	ON E.AF_APL_ID = B.AF_APL_ID
	AND E.AF_APL_ID_SFX = B.AF_APL_ID_SFX
INNER JOIN OLWHRM1.GA15_NDS_ID D
	ON E.AF_APL_ID = D.AF_APL_ID
	AND E.AF_APL_ID_SFX = D.AF_APL_ID_SFX
LEFT OUTER JOIN OLWHRM1.PD01_PDM_INF E
	ON A.DF_PRS_ID_BR = E.DF_PRS_ID
LEFT OUTER JOIN OLWHRM1.GA11_LON_DSB_ATY F
	ON E.AF_APL_ID = F.AF_APL_ID
	AND E.AF_APL_ID_SFX = F.AF_APL_ID_SFX
	AND F.AN_DSB_SEQ = 1		
	AND F.AC_DSB_ADJ = 'A' 		
	AND F.AC_DSB_ADJ_STA = 'A'
LEFT OUTER JOIN (
	SELECT X.AF_APL_ID
		,X.AF_APL_ID_SFX
		,X.LC_STA_DC10 
		,X.LC_AUX_STA 
		,X.LD_RHB 
	FROM OLWHRM1.DC01_LON_CLM_INF X
	INNER JOIN (
		SELECT AF_APL_ID
			,AF_APL_ID_SFX
			,MAX(LD_RHB) AS LD_RHB
		FROM OLWHRM1.DC01_LON_CLM_INF
		GROUP BY AF_APL_ID
			,AF_APL_ID_SFX
		) Y
		ON X.AF_APL_ID = Y.AF_APL_ID
		AND X.AF_APL_ID_SFX = Y.AF_APL_ID_SFX
		AND X.LD_RHB = Y.LD_RHB
	) G
	ON B.AF_APL_ID = G.AF_APL_ID
	AND B.AF_APL_ID_SFX = G.AF_APL_ID_SFX
WHERE E.AC_STA_GA14 = 'A'
AND D.AC_STA_GA15 = 'A'
AND (
		(
			B.AC_LON_TYP IN ('SF','SU','PL','SL','GB') AND 
			E.AC_LON_STA_TYP IN ('IA','ID','IG','IM','DA','FB','RP') AND 
			D.AD_NDS_CLC_ENT_RPD < &NTCDT
		)
	OR
		(
			B.AC_LON_TYP = 'CL' AND 
			E.AC_LON_STA_TYP IN ('DA','FB','RP') AND 
			D.AD_NDS_CLC_ENT_RPD < &CNSDT
		)
	)
);

CREATE TABLE GA14 AS 
SELECT DISTINCT B.CLUID
		,B.AC_LON_STA_TYP
		,B.AD_LON_STA FORMAT=MMDDYY10.
FROM DEMO A
INNER JOIN CONNECTION TO DB2 (
	SELECT AF_APL_ID||AF_APL_ID_SFX AS CLUID
		,AC_LON_STA_TYP
		,AD_LON_STA 
	FROM OLWHRM1.GA14_LON_STA
	WHERE AC_STA_GA14 IN ('A','H')
	) B
	ON A.CLUID = B.CLUID
;
DISCONNECT FROM DB2;
ENDRSUBMIT ;

DATA DEMO;SET WORKLOCL.DEMO;RUN;
DATA GA14;SET WORKLOCL.GA14;RUN;

DATA DSCN (KEEP=CLUID LD_RHB);
SET DEMO;
WHERE LC_STA_DC10 = '04' AND LC_AUX_STA = '10' AND LD_RHB ^= . ;
RUN;

PROC SORT DATA=DSCN; BY CLUID ;RUN;
PROC SORT DATA=GA14; BY CLUID ;RUN;
DATA GA14;
MERGE GA14 (IN=A) DSCN (IN=B);
BY CLUID;
IF A;
RUN;

DATA GA14;
SET GA14;
IF AC_LON_STA_TYP IN ('DA','FB') AND AD_LON_STA < LD_RHB THEN DELETE;
ELSE OUTPUT;
RUN;

PROC SORT DATA=GA14 ; BY CLUID DESCENDING AD_LON_STA;RUN;

DATA GA14;
SET GA14;
FORMAT END_DT BEG_DT MMDDYY10.;
BY CLUID;
RETAIN BEG_DT;
IF FIRST.CLUID THEN DO;
	END_DT = TODAY();
	BEG_DT = AD_LON_STA;
END;
	ELSE DO;
	END_DT = BEG_DT;
	BEG_DT = AD_LON_STA;
END;
RUN;

DATA GA14;
SET GA14;
NET_DAYS = END_DT - BEG_DT;
RUN;

DATA GA14 (KEEP=CLUID TOT_DAYS);
SET GA14;
BY CLUID;
RETAIN TOT_DAYS;
WHERE AC_LON_STA_TYP IN ('FB','DA');
IF FIRST.CLUID THEN DO;
	TOT_DAYS = NET_DAYS;
	END;
ELSE DO;
	TOT_DAYS+NET_DAYS;
	END;
IF LAST.CLUID THEN OUTPUT;
RUN;
/****************************************************************************
* DATE CALCULATIONS
*****************************************************************************/
DATA _NULL_;
YEARS_AGO_9 = INTNX('YEAR',TODAY(),-9,'S');
YEARS_AGO_29 = INTNX('YEAR',TODAY(),-29,'S');
	CALL SYMPUT('YRS_9_MTH_3',INTNX('MONTH',YEARS_AGO_9,-3,'S'));
	CALL SYMPUT('YRS_29_MTH_3',INTNX('MONTH',YEARS_AGO_29,-3,'S'));
	CALL SYMPUT('YRS_10',INTNX('YEAR',TODAY(),-10,'S'));
	CALL SYMPUT('YRS_30',INTNX('YEAR',TODAY(),-30,'S'));
	CALL SYMPUT('NUM_9_3',TODAY() - INTNX('MONTH',YEARS_AGO_9,-3,'S'));
	CALL SYMPUT('NUM_29_3',TODAY() - INTNX('MONTH',YEARS_AGO_29,-3,'S'));
	CALL SYMPUT('NUM_10',TODAY() - INTNX('YEAR',TODAY(),-10,'S'));
	CALL SYMPUT('NUM_30',TODAY() - INTNX('YEAR',TODAY(),-30,'S'));
RUN;
/*%PUT &YRS_9_MTH_3;*/
/*%PUT &YRS_29_MTH_3;*/
/*%PUT &NUM_9_3;*/
/*%PUT &NUM_29_3;*/
/*%PUT &YRS_10;*/
/*%PUT &YRS_30;*/
/*%PUT &NUM_10;*/
/*%PUT &NUM_30;*/
/****************************************************************************
* PUT EVERYTHING TOGETHER AND PROCESS DATA
*****************************************************************************/
PROC SORT DATA=DEMO; BY CLUID ;RUN;
PROC SORT DATA=GA14; BY CLUID ;RUN;
DATA DEMO;
MERGE DEMO (IN=A) GA14 (IN=B);
BY CLUID;
IF A;
RUN;

DATA DEMO;
SET DEMO;
IF AC_LON_TYP IN ('SF','SU','PL','SL','GB') THEN DO;
	IF LC_STA_DC10 = '04' AND LC_AUX_STA = '10' AND LD_RHB > &YRS_9_MTH_3 
		THEN DELETE;
	ELSE IF LC_STA_DC10 = '04' AND LC_AUX_STA = '10' AND LD_RHB < &YRS_9_MTH_3 THEN DO;
		IF TODAY() - REPAYDT > &NUM_9_3 + COALESCE(TOT_DAYS,0) THEN OUTPUT;
	END;
	ELSE DO;
		IF TODAY() - REPAYDT > &NUM_10 + COALESCE(TOT_DAYS,0) THEN OUTPUT;
	END;
END;
ELSE IF AC_LON_TYP IN ('CL') THEN DO;
	IF LC_STA_DC10 = '04' AND LC_AUX_STA = '10' AND LD_RHB > &YRS_29_MTH_3 
		THEN DELETE;
	ELSE IF LC_STA_DC10 = '04' AND LC_AUX_STA = '10' AND LD_RHB < &YRS_29_MTH_3 THEN DO;
		IF TODAY() - REPAYDT > &NUM_29_3 + COALESCE(TOT_DAYS,0) THEN OUTPUT;
	END;
	ELSE DO;
		IF TODAY() - REPAYDT > &NUM_30 + COALESCE(TOT_DAYS,0) THEN OUTPUT;
	END;
END;
RUN;

DATA DEMO; 
SET DEMO; 
SORTBY = SERVICER||HOLDER;
RUN;

PROC SORT DATA = DEMO;
BY SORTBY ACCNUM AD_PRC;
RUN;

PROC PRINTTO PRINT=REPORT2 NEW;
RUN;

OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 CENTER NODATE NUMBER LABEL PAGENO=1;
PROC REPORT DATA=DEMO NOWD HEADLINE HEADSKIP SPLIT='/';
TITLE 'LOANS IN REPAYMENT MORE THAN 10 YEARS';
TITLE2 '(30 YEARS FOR CONSOL)';
FOOTNOTE  'JOB = UTLWO03     REPORT = ULWO03.LWO03R2';
COLUMN SORTBY SERVICER HOLDER NAME ACCNUM AC_LON_TYP CLUID AA_GTE_LON_AMT  
	LONSTA REPAYDT N;
DEFINE SORTBY / GROUP NOPRINT;
DEFINE SERVICER / GROUP NOPRINT;
DEFINE HOLDER / 'LENDER';
DEFINE NAME / DISPLAY WIDTH=20;
DEFINE ACCNUM / 'ACCT #' WIDTH=10;
DEFINE CLUID / 'UNIQUE ID';
DEFINE AC_LON_TYP / 'LOAN TYPE' WIDTH=4;
DEFINE AA_GTE_LON_AMT / noprint ;
DEFINE LONSTA / 'UPDATED/STATUS' WIDTH=7;
DEFINE REPAYDT / 'EFFECTIVE/DATE' FORMAT=MMDDYY8. WIDTH=9;
DEFINE N / NOPRINT;
BREAK AFTER SORTBY / PAGE;
COMPUTE BEFORE _PAGE_;
	LINE @1 'SERVICER:  ' SERVICER $26.;
ENDCOMP;
COMPUTE AFTER SORTBY;
	SUMAMT = AA_GTE_LON_AMT.SUM;
	LINE ' ';
	LINE @1 'NUMBER OF LOANS FOR THIS LENDER:  ' @40 N COMMA19.;
	LINE @1 'TOTAL AMOUNT APPROVED FOR THIS LENDER:  ' @40 SUMAMT DOLLAR19.2;
	LINE ' ';
ENDCOMP;
COMPUTE AFTER;
	SUMAMT = AA_GTE_LON_AMT.SUM;
	LINE @1'---------------------------------------------------------- ';
	LINE ' ';
	LINE @1 'TOTAL NUMBER OF LOANS:  ' @40 N COMMA19.;
	LINE @1 'TOTAL AMOUNT APPROVED:  ' @40 SUMAMT DOLLAR19.2;
	LINE ' ';
	LINE @1'---------------------------------------------------------- ';
	LINE ' ';
ENDCOMP;
RUN;

PROC PRINTTO;
RUN;