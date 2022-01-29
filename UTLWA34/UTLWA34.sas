LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/ULWA34.LWA34RZ";
FILENAME REPORT2 "&RPTLIB/ULWA34.LWA34R2";
FILENAME REPORT3 "&RPTLIB/ULWA34.LWA34R3";
FILENAME REPORT4 "&RPTLIB/ULWA34.LWA34R4";
FILENAME REPORT5 "&RPTLIB/ULWA34.LWA34R5";
FILENAME REPORT6 "&RPTLIB/ULWA34.LWA34R6";
FILENAME REPORT7 "&RPTLIB/ULWA34.LWA34R7";
FILENAME REPORT8 "&RPTLIB/ULWA34.LWA34R8";
FILENAME REPORT9 "&RPTLIB/ULWA34.LWA34R9";
FILENAME REPORT10 "&RPTLIB/ULWA34.LWA34R10";
FILENAME REPORT11 "&RPTLIB/ULWA34.LWA34R11";
FILENAME REPORT12 "&RPTLIB/ULWA34.LWA34R12";
FILENAME REPORT13 "&RPTLIB/ULWA34.LWA34R13";
FILENAME REPORT14 "&RPTLIB/ULWA34.LWA34R14";
FILENAME REPORT15 "&RPTLIB/ULWA34.LWA34R15";
FILENAME REPORT16 "&RPTLIB/ULWA34.LWA34R16";
FILENAME REPORT17 "&RPTLIB/ULWA34.LWA34R17";
FILENAME REPORT18 "&RPTLIB/ULWA34.LWA34R18";
FILENAME REPORT19 "&RPTLIB/ULWA34.LWA34R19";
FILENAME REPORT20 "&RPTLIB/ULWA34.LWA34R20";
FILENAME REPORT21 "&RPTLIB/ULWA34.LWA34R21";
FILENAME REPORT22 "&RPTLIB/ULWA34.LWA34R22";
FILENAME REPORT23 "&RPTLIB/ULWA34.LWA34R23";
FILENAME REPORT24 "&RPTLIB/ULWA34.LWA34R24";
FILENAME REPORT25 "&RPTLIB/ULWA34.LWA34R25";
FILENAME REPORT26 "&RPTLIB/ULWA34.LWA34R26";
FILENAME REPORT27 "&RPTLIB/ULWA34.LWA34R27";
FILENAME REPORT28 "&RPTLIB/ULWA34.LWA34R28";
FILENAME REPORT29 "&RPTLIB/ULWA34.LWA34R29";
FILENAME REPORT30 "&RPTLIB/ULWA34.LWA34R30";
FILENAME REPORT31 "&RPTLIB/ULWA34.LWA34R31";
DATA _NULL_;
	CALL SYMPUT('BEGIN',"'"||PUT(INTNX('MONTH',TODAY(),-1,'BEGINNING'), MMDDYYS10.)||"'");
	CALL SYMPUT('END',"'"||PUT(INTNX('MONTH',TODAY(),-1,'END'), MMDDYYS10.)||"'");
RUN;
%SYSLPUT BEGIN = &BEGIN;
%SYSLPUT END = &END;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
%MACRO SQLCHECK (SQLRPT= );
%IF &SQLXRC NE 0 %THEN %DO;
	DATA _NULL_;
    FILE REPORTZ NOTITLES;
    PUT @01 " ********************************************************************* "
      / @01 " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
      / @01 " ****  THE SAS LOG IN &SQLRPT SHOULD BE REVIEWED.          **** "       
      / @01 " ********************************************************************* "
      / @01 " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
      / @01 " ****  &SQLXMSG   **** "
      / @01 " ********************************************************************* ";
	RUN;
%END;
%MEND;
PROC SQL;
CONNECT TO DB2 (DATABASE=DLGSUTWH);
CREATE TABLE FDFINI AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.AF_CUR_APL_OPS_LDR
	,A.DF_PRS_ID_BR
	,B.AF_APL_ID||B.AF_APL_ID_SFX AS CLUID
	,CASE 
		WHEN B.AC_LON_TYP IN ('SF','SU') THEN 'STAFFORD'
		WHEN B.AC_LON_TYP IN ('PL','GB') THEN 'PLUS'
	 END AS AC_LON_TYP
	,RTRIM(F.DM_PRS_1)||','||RTRIM(DM_PRS_MID)||','||DM_PRS_LST AS NAME
	,C.AC_DSB_ADJ 
	,C.AD_DSB_ADJ 
	,C.AA_DSB_ADJ
	,C.AD_LST_USR_UPD 
	,C.AN_DSB_SEQ
	,C.AA_GTE_FEE_ADJ
	,C.AF_CRT_DTS_GA11
	,E.MIN_DSB_DT
	,CASE
		WHEN C.AC_DSB_ADJ = 'A'	AND C.AD_DSB_ADJ BETWEEN &BEGIN AND &END THEN 'A'
		WHEN C.AC_DSB_ADJ IN ('C','S','U')
			AND
			(
				C.AD_DSB_ADJ BETWEEN &BEGIN AND &END
				AND C.AD_LST_USR_UPD BETWEEN &BEGIN AND &END
			OR 
				C.AD_LST_USR_UPD BETWEEN &BEGIN AND &END
				AND C.AD_DSB_ADJ < &BEGIN 
			) THEN 'B'
		ELSE 'C'
	 END AS ROW_TYP

FROM OLWHRM1.GA01_APP A
INNER JOIN OLWHRM1.GA10_LON_APP B
	ON A.AF_APL_ID = B.AF_APL_ID
INNER JOIN OLWHRM1.GA11_LON_DSB_ATY C
	ON B.AF_APL_ID = C.AF_APL_ID
	AND B.AF_APL_ID_SFX = C.AF_APL_ID_SFX
INNER JOIN (
	SELECT AF_APL_ID
		,AF_APL_ID_SFX
	FROM OLWHRM1.GA11_LON_DSB_ATY
	WHERE AC_DSB_ADJ = 'A'
		AND AN_DSB_SEQ = 1
		AND AD_DSB_ADJ >= '07/02/2009'
		AND AC_DSB_ADJ_STA = 'A'
	) D
	ON C.AF_APL_ID = D.AF_APL_ID
	AND C.AF_APL_ID_SFX = D.AF_APL_ID_SFX
LEFT OUTER JOIN (
	SELECT AF_APL_ID
		,AF_APL_ID_SFX
		,AN_DSB_SEQ
		,MIN(AD_DSB_ADJ) AS MIN_DSB_DT
	FROM OLWHRM1.GA11_LON_DSB_ATY
	WHERE AC_DSB_ADJ_STA = 'A'
		AND AC_DSB_ADJ = 'A'
	GROUP BY AF_APL_ID
		,AF_APL_ID_SFX
		,AN_DSB_SEQ
	) E
	ON C.AF_APL_ID = E.AF_APL_ID
	AND C.AF_APL_ID_SFX = E.AF_APL_ID_SFX
	AND C.AN_DSB_SEQ = E.AN_DSB_SEQ
INNER JOIN OLWHRM1.PD01_PDM_INF F
	ON A.DF_PRS_ID_BR = F.DF_PRS_ID
INNER JOIN (
	SELECT AF_APL_ID
		,AF_APL_ID_SFX
		,AN_DSB_SEQ
	FROM OLWHRM1.GA11_LON_DSB_ATY
	WHERE AC_DSB_ADJ = 'A'
		AND AC_DSB_ADJ_STA = 'A'
	) G
	ON C.AF_APL_ID = G.AF_APL_ID
	AND C.AF_APL_ID_SFX = G.AF_APL_ID_SFX
	AND C.AN_DSB_SEQ = G.AN_DSB_SEQ

WHERE C.AC_DSB_ADJ_STA = 'A'
	AND 
	(
		(
			C.AC_DSB_ADJ = 'A'
			AND C.AD_DSB_ADJ BETWEEN &BEGIN AND &END
		)
	OR 
		(
			C.AC_DSB_ADJ IN ('C','S','U')
			AND
			(
				C.AD_DSB_ADJ BETWEEN &BEGIN AND &END
				AND C.AD_LST_USR_UPD BETWEEN &BEGIN AND &END
			OR 
				C.AD_LST_USR_UPD BETWEEN &BEGIN AND &END
				AND C.AD_DSB_ADJ < &BEGIN 
			)
		)
	OR
		(
			C.AC_DSB_ADJ = 'P'
			AND C.AD_DSB_ADJ <= &END
		)
	)
FOR READ ONLY WITH UR
);

CREATE TABLE LR01 AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT IF_IST
	,IM_IST_FUL
FROM OLWHRM1.LR01_LGS_LDR_INF
FOR READ ONLY WITH UR
);

DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWA34.LWA34RZ);*/
/*QUIT;*/
ENDRSUBMIT;
DATA FDFINI ;
	SET WORKLOCL.FDFINI;
RUN; 
DATA LR01 ;
	SET WORKLOCL.LR01;
RUN; 

PROC SORT DATA=FDFINI OUT=DEMO(DROP = AC_DSB_ADJ AD_DSB_ADJ AF_CRT_DTS_GA11 AA_DSB_ADJ
	AD_LST_USR_UPD AA_GTE_FEE_ADJ) NODUPKEY;
	BY CLUID AN_DSB_SEQ;
RUN;

PROC SORT DATA=FDFINI OUT=DISB (KEEP = CLUID AC_DSB_ADJ AD_DSB_ADJ AA_DSB_ADJ 
	AD_LST_USR_UPD AN_DSB_SEQ AA_GTE_FEE_ADJ AF_CRT_DTS_GA11 MIN_DSB_DT ROW_TYP)NODUPKEY;
	BY CLUID AN_DSB_SEQ AF_CRT_DTS_GA11;
RUN;

DATA DISB (DROP= TEMP_BEGIN TEMP_END FEE_ADJ_PERCENET);
	SET DISB;
	TEMP_BEGIN = INPUT(COMPRESS(&BEGIN,"'"),MMDDYY10.);
	TEMP_END = INPUT(COMPRESS(&END,"'"),MMDDYY10.);
	FEE_ADJ_PERCENET = .01;
	IF AC_DSB_ADJ = 'A' THEN DO;
		DTYP = 'ACT';
		FEE_ADJ = 0;
	END;
	ELSE DO;
		DTYP = 'CAN';
		AA_DSB_ADJ = AA_DSB_ADJ * -1;
		FEE_ADJ = ROUND(AA_DSB_ADJ * FEE_ADJ_PERCENET,.01);
	END;
RUN;

PROC SQL;
	CREATE TABLE PTYP AS
	SELECT A.*
	FROM DISB A
	INNER JOIN DISB B
		ON A.CLUID = B.CLUID
		AND A.AN_DSB_SEQ = B.AN_DSB_SEQ
	WHERE A.ROW_TYP = 'C'
		AND B.ROW_TYP = 'A'
		AND A.AD_DSB_ADJ <= B.AD_DSB_ADJ
	;
QUIT;

PROC SQL;
CREATE TABLE FDF AS
SELECT DISTINCT A.AF_CUR_APL_OPS_LDR
	,A.AC_LON_TYP
	,A.NAME
	,A.DF_PRS_ID_BR
	,A.CLUID
	,A.AN_DSB_SEQ 
	,COALESCE(B.DISB_DT,A.MIN_DSB_DT) AS DISB_DT
	,COALESCE(B.DISB_AMT,0) + COALESCE(D.PAMT,0) AS DISB_AMT
	,COALESCE(B.FEE_AMT,0) AS FEE_AMT
	,C.CNCL_DT 
	,COALESCE(C.CNCL_AMT,0) AS CNCL_AMT
	,COALESCE(C.FEE_ADJ,0) AS FEE_ADJ
FROM DEMO A
LEFT OUTER JOIN (
	SELECT CLUID
		,AN_DSB_SEQ 
		,MAX(AD_DSB_ADJ) AS DISB_DT
		,SUM(AA_DSB_ADJ) AS DISB_AMT
		,SUM(AA_GTE_FEE_ADJ) AS FEE_AMT
	FROM DISB
	WHERE DTYP = 'ACT'
	GROUP BY CLUID
		,AN_DSB_SEQ 
	) B
	ON A.CLUID = B.CLUID
	AND A.AN_DSB_SEQ = B.AN_DSB_SEQ
LEFT OUTER JOIN (
	SELECT CLUID
		,AN_DSB_SEQ 
		,MAX(AD_DSB_ADJ) AS CNCL_DT
		,SUM(AA_DSB_ADJ) AS CNCL_AMT
		,SUM(FEE_ADJ) 	 AS FEE_ADJ
	FROM DISB
	WHERE DTYP = 'CAN'
		AND ROW_TYP = 'B'
	GROUP BY CLUID
		,AN_DSB_SEQ 
	) C
	ON A.CLUID = C.CLUID
	AND A.AN_DSB_SEQ = C.AN_DSB_SEQ
LEFT OUTER JOIN (
	SELECT CLUID
		,AN_DSB_SEQ 
		,SUM(AA_DSB_ADJ) AS PAMT
	FROM PTYP
	WHERE DTYP = 'CAN'
		AND ROW_TYP = 'C'
	GROUP BY CLUID
		,AN_DSB_SEQ 
	) D
	ON A.CLUID = D.CLUID
	AND A.AN_DSB_SEQ = D.AN_DSB_SEQ
INNER JOIN (
	SELECT CLUID
		,AN_DSB_SEQ 
	FROM DISB
	WHERE ROW_TYP IN ('A','B')
	) E
	ON A.CLUID = E.CLUID
	AND A.AN_DSB_SEQ = E.AN_DSB_SEQ

;
QUIT;	

DATA FDF (DROP=FEE_ADJ_PERCENET);
	SET FDF;
	FEE_ADJ_PERCENET = .01;
	IF FEE_AMT = 0 THEN 
		FEE_AMT = ROUND(DISB_AMT * FEE_ADJ_PERCENET,.01);
	ELSE 
		FEE_AMT = FEE_AMT;	 
RUN;

DATA _NULL_;
	CALL SYMPUT('BEG_REP_TITLE',COMPRESS(&BEGIN,"'"));
	CALL SYMPUT('END_REP_TITLE',COMPRESS(&END,"'"));
RUN;

%PUT &BEG_REP_TITLE;
%PUT &END_REP_TITLE;

%MACRO A34_DETAIL_REP(RNO,LDR_ID);
PROC SQL NOPRINT;
CREATE TABLE DET_REP AS
	SELECT DISTINCT A.IM_IST_FUL
		,B.*
	FROM LR01 A
	LEFT OUTER JOIN FDF B
		ON A.IF_IST = B.AF_CUR_APL_OPS_LDR
	WHERE A.IF_IST = "&LDR_ID"
	ORDER BY B.DISB_DT,B.DF_PRS_ID_BR;
QUIT;
DATA _NULL_;
	SET DET_REP;
	CALL SYMPUT('LDR_NM',TRIM(CATX(' - ',IM_IST_FUL,AF_CUR_APL_OPS_LDR)));
RUN;
%PUT &LDR_NM;
PROC PRINTTO PRINT=REPORT&RNO NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=132 CENTER DATE PAGENO=1;
TITLE "DEFAULT FEE DETAIL";
TITLE2	"&LDR_NM";
FOOTNOTE   "JOB = UTLWA34  	 REPORT = ULWA34.LWA34R&RNO";
PROC CONTENTS DATA=DET_REP OUT=EMPTYSET NOPRINT;
DATA _NULL_;
SET EMPTYSET;
FILE PRINT;
IF  NOBS=0 AND _N_ =1 THEN DO;
	PUT // 131*'-';
	PUT      //////
		@51 '**** NO OBSERVATIONS FOUND ****';
	PUT //////
		@57 '-- END OF REPORT --';
	PUT //////////////
		@46 "JOB = UTLWA34  	 REPORT = ULWA34.LWA34R&RNO";
	END;
RETURN;
RUN;
PROC REPORT NOWD DATA=DET_REP SPLIT='~' HEADSKIP HEADLINE SPACING=1;
COLUMN DISB_DT  NAME DF_PRS_ID_BR CLUID AC_LON_TYP AN_DSB_SEQ CNCL_DT  
	DISB_AMT FEE_AMT CNCL_AMT FEE_ADJ;
DEFINE DISB_DT / ORDER WIDTH=10 'DISB~DATE' FORMAT=MMDDYY10.;
DEFINE NAME / DISPLAY WIDTH=20 'BORROWER~NAME';
DEFINE DF_PRS_ID_BR / DISPLAY WIDTH=9 'SSN';
DEFINE CLUID / DISPLAY WIDTH=19 'UNIQUE~ID';
DEFINE AC_LON_TYP / DISPLAY WIDTH=8 'LOAN~TYPE';
DEFINE AN_DSB_SEQ /	DISPLAY WIDTH=4 'DISB #';
DEFINE CNCL_DT / DISPLAY WIDTH=10 'CANCEL/~REFUND~DATE' FORMAT=MMDDYY10.; 
DEFINE DISB_AMT / ANALYSIS WIDTH=10 'DISB~AMOUNT' FORMAT=COMMA12.2 WIDTH=12;
DEFINE FEE_AMT / ANALYSIS WIDTH=10 'FEE~AMOUNT' FORMAT=COMMA9.2 WIDTH=9 ;
DEFINE CNCL_AMT / ANALYSIS WIDTH=10 'CANCEL/~REFUND~AMOUNT' FORMAT=COMMA9.2 WIDTH=9;
DEFINE FEE_ADJ / ANALYSIS WIDTH=10 'FEE~ADJUSTMENT' FORMAT=COMMA10.2 WIDTH=10;
BREAK AFTER DISB_DT / SUMMARIZE SUPPRESS SKIP OL;
RUN;
PROC PRINTTO;
RUN;
%MEND A34_DETAIL_REP;
%MACRO A34_SMMRY_REP(RNO,LDR_ID);
PROC SQL NOPRINT;
CREATE TABLE SUM_REP AS
	SELECT A.IM_IST_FUL
		,B.AF_CUR_APL_OPS_LDR
		,SUM(COALESCE(B.FEE_AMT,0)) AS TOT_FEE_AMT
		,SUM(COALESCE(B.FEE_ADJ,0)) AS TOT_FEE_ADJ
		,SUM(COALESCE(B.FEE_AMT,0) + COALESCE(B.FEE_ADJ,0)) AS TOT
	FROM LR01 A
	LEFT OUTER JOIN FDF B
		ON A.IF_IST = B.AF_CUR_APL_OPS_LDR
	WHERE A.IF_IST = "&LDR_ID"
	GROUP BY  A.IM_IST_FUL
		,B.AF_CUR_APL_OPS_LDR
		,B.AC_LON_TYP;
QUIT;
DATA _NULL_;
	SET SUM_REP;
	CALL SYMPUT('LDR_NM',TRIM(CATX(' - ',IM_IST_FUL,AF_CUR_APL_OPS_LDR)));
RUN;
%PUT &LDR_NM;
PROC PRINTTO PRINT=REPORT&RNO NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=132 CENTER DATE PAGENO=1;
TITLE "DEFAULT FEE SUMMARY REPORT";
TITLE2	"&LDR_NM";
TITLE3 "TOTAL FEES DUE FROM &BEG_REP_TITLE TO &END_REP_TITLE";
FOOTNOTE   "JOB = UTLWA34  	 REPORT = ULWA34.LWA34R&RNO";
PROC REPORT NOWD DATA=SUM_REP SPLIT='~' HEADSKIP HEADLINE SPACING=4;
COLUMN TOT_FEE_AMT TOT_FEE_ADJ TOT;
DEFINE TOT_FEE_AMT / ANALYSIS WIDTH=30 'TOTAL FEE AMOUNT' FORMAT=COMMA18.2 WIDTH=20 RIGHT;
DEFINE TOT_FEE_ADJ / ANALYSIS WIDTH=30 'TOTAL FEE ADJUSTMENT' FORMAT=COMMA18.2 WIDTH=20 RIGHT;
DEFINE TOT / ANALYSIS WIDTH=30 'TOTAL FEES DUE ' FORMAT=COMMA18.2 WIDTH=20 RIGHT;
RUN;
PROC PRINTTO;
RUN;
%MEND A34_SMMRY_REP;
%A34_DETAIL_REP(2,811698);
%A34_DETAIL_REP(3,817440);
%A34_DETAIL_REP(4,817545);
%A34_DETAIL_REP(5,817546);
%A34_DETAIL_REP(6,817575);
%A34_DETAIL_REP(7,820200);
%A34_DETAIL_REP(8,822373);
%A34_DETAIL_REP(9,828476);
%A34_DETAIL_REP(10,829123);
%A34_DETAIL_REP(11,829158);
%A34_DETAIL_REP(12,830132);
%A34_DETAIL_REP(13,830146);
%A34_DETAIL_REP(14,830791);
%A34_DETAIL_REP(15,833577);
%A34_DETAIL_REP(16,833828);
%A34_SMMRY_REP(17,811698);
%A34_SMMRY_REP(18,817440);
%A34_SMMRY_REP(19,817545);
%A34_SMMRY_REP(20,817546);
%A34_SMMRY_REP(21,817575);
%A34_SMMRY_REP(22,820200);
%A34_SMMRY_REP(23,822373);
%A34_SMMRY_REP(24,828476);
%A34_SMMRY_REP(25,829123);
%A34_SMMRY_REP(26,829158);
%A34_SMMRY_REP(27,830132);
%A34_SMMRY_REP(28,830146);
%A34_SMMRY_REP(29,830791);
%A34_SMMRY_REP(30,833577);
%A34_SMMRY_REP(31,833828);