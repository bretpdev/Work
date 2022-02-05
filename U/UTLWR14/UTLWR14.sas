*--------------------------------------------*
| UTLWR14 - UHEAA Origination Fee Adjustment |
*--------------------------------------------*;
/*LIBNAME DLGSUTWH DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;*/
/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORT2 "&RPTLIB/ULWR14.LWR14R2";
FILENAME REPORT3 "&RPTLIB/ULWR14.LWR14R3";
FILENAME REPORT4 "&RPTLIB/ULWR14.LWR14R4";
FILENAME REPORT5 "&RPTLIB/ULWR14.LWR14R5";
FILENAME REPORT6 "&RPTLIB/ULWR14.LWR14R6";
FILENAME REPORT7 "&RPTLIB/ULWR14.LWR14R7";
FILENAME REPORT8 "&RPTLIB/ULWR14.LWR14R8";
FILENAME REPORT9 "&RPTLIB/ULWR14.LWR14R9";
FILENAME REPORT10 "&RPTLIB/ULWR14.LWR14R10";
FILENAME REPORT11 "&RPTLIB/ULWR14.LWR14R11";
FILENAME REPORT12 "&RPTLIB/ULWR14.LWR14R12";
FILENAME REPORT13 "&RPTLIB/ULWR14.LWR14R13";
FILENAME REPORT14 "&RPTLIB/ULWR14.LWR14R14";
FILENAME REPORT15 "&RPTLIB/ULWR14.LWR14R15";
FILENAME REPORT16 "&RPTLIB/ULWR14.LWR14R16";
FILENAME REPORT17 "&RPTLIB/ULWR14.LWR14R17";
FILENAME REPORT18 "&RPTLIB/ULWR14.LWR14R18";
FILENAME REPORT19 "&RPTLIB/ULWR14.LWR14R19";
FILENAME REPORT20 "&RPTLIB/ULWR14.LWR14R20";
FILENAME REPORT21 "&RPTLIB/ULWR14.LWR14R21";
FILENAME REPORT22 "&RPTLIB/ULWR14.LWR14R22";
FILENAME REPORT23 "&RPTLIB/ULWR14.LWR14R23";
FILENAME REPORT24 "&RPTLIB/ULWR14.LWR14R24";
FILENAME REPORT25 "&RPTLIB/ULWR14.LWR14R25";
FILENAME REPORT26 "&RPTLIB/ULWR14.LWR14R26";
FILENAME REPORTZ "&RPTLIB/ULWR14.LWR14RZ";
/*For local runs, the macro days_ago_1 should be set to the applied date.*/
/*The variable on line 132 should be set to the following business day*/

/*For Monday runs, we need to look back to Friday for the*/
/*applied date.  Otherwise, use the previous day.  This */
/*job is to run Monday through Thursday nights, and again*/
/*Monday morning.*/
DATA _NULL_;
IF WEEKDAY(today()) = 2 THEN DO;
	CALL SYMPUT('DAYS_AGO_1',"'"||PUT(INTNX('DAY',TODAY(),-3,'BEGINNING'), MMDDYYD10.)||"'");
	CALL SYMPUT('TITLE_DATE',PUT(INTNX('DAY',TODAY(),-3,'BEGINNING'), MMDDYYS10.));
END;
ELSE DO;
	CALL SYMPUT('TITLE_DATE',PUT(INTNX('DAY',TODAY(),-1,'BEGINNING'), MMDDYYS10.));
	CALL SYMPUT('DAYS_AGO_1',"'"||PUT(INTNX('DAY',TODAY(),-1,'BEGINNING'), MMDDYYD10.)||"'");
END;
RUN;
%SYSLPUT DAYS_AGO_1 = &DAYS_AGO_1;
LIBNAME  WORKLOCL  REMOTE  SERVER=CYPRUS  SLIBREF=WORK;
RSUBMIT;
LIBNAME SAS_TAB V8 '/sas/whse/progrevw';
PROC SQL NOPRINT;
	SELECT "'"||TRIM(LENDER_ID)||"'"
		INTO :UHEAA_LIST SEPARATED BY ","
	FROM SAS_TAB.LDR_AFF
	WHERE AFFILIATION = 'UHEAA';
QUIT;
PROC SQL NOPRINT;
	SELECT "'"||TRIM(LENDER_ID)||"'"
		INTO :CUSTODIAN_LIST SEPARATED BY ","
	FROM SAS_TAB.LDR_AFF
	WHERE AFFILIATION = 'CUSTODIAN';
QUIT;
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
CREATE TABLE UOFA AS
SELECT *
FROM CONNECTION TO DB2 (
SELECT DISTINCT A.BF_SSN
	,A.LN_SEQ
	,B.IC_LON_PGM
	,B.LF_LON_CUR_OWN
	,A.LD_FAT_EFF	
	,A.LD_FAT_APL
	,COALESCE(A.LA_FAT_CUR_PRI,0) 	AS LA_FAT_CUR_PRI
	,COALESCE(A.LA_FAT_NSI,0) 		AS LA_FAT_NSI
	,COALESCE(A.LA_FAT_ILG_PRI,0) 	AS LA_FAT_ILG_PRI
	,COALESCE(A.LA_FAT_LTE_FEE,0) 	AS LA_FAT_LTE_FEE
	,A.PC_FAT_TYP||A.PC_FAT_SUB_TYP AS TRX_TYP	
	,A.LC_CSH_ADV
	,A.LN_FAT_SEQ
	,A.IF_BND_ISS
	,A.IF_CUS_ACC
	,B.LF_LON_ALT || '0' || CHAR(B.LN_LON_ALT_SEQ) AS CLUID
	,CASE
		WHEN COALESCE(A.LA_FAT_ILG_PRI,0) = 0 THEN A.LA_FAT_CUR_PRI
		ELSE A.LA_FAT_ILG_PRI
	 END AS CAL_VAL
	,CASE
		WHEN B.LF_LON_CUR_OWN IN ('828476','834396','834437') THEN 'UHEAA'
		ELSE ''
	 END AS JACC
	,B.IF_TIR_PCE 
	,B.LF_RGL_CAT_LP20
	,C.IF_ABA_CUS
	,C.IF_ACC_FIN_TRF_CUS 
	,C.IA_FIN_TRF_ACL
	,D.DF_SPE_ACC_ID
	,A.LC_FAT_REV_REA
		,C.IA_CUR_CUS_BAL
FROM OLWHRM1.MR90_DLY_APL_FAT A
INNER JOIN OLWHRM1.PD10_PRS_NME D
	ON A.BF_SSN = D.DF_PRS_ID
INNER JOIN OLWHRM1.LN10_LON B
	ON A.BF_SSN = B.BF_SSN
	AND A.LN_SEQ = B.LN_SEQ
LEFT OUTER JOIN (
	SELECT CU10.IF_CUS_ACC
		,CU10.IF_ABA_CUS
		,CU10.IF_ACC_FIN_TRF_CUS 
		,RC40.IA_FIN_TRF_ACL
		,RC40.IA_CUR_CUS_BAL
	FROM OLWHRM1.CU10_CUS_ACC CU10
	LEFT OUTER JOIN OLWHRM1.RC40_CUS_FIN_TRF RC40
		ON CU10.IF_CUS_ACC = RC40.IF_CUS_ACC
		AND DAYS(RC40.ID_FIN_TRF_SCH) = DAYS(CURRENT DATE) 
	) C
	ON A.IF_CUS_ACC = C.IF_CUS_ACC
WHERE A.LD_FAT_APL = &DAYS_AGO_1
	AND B.LF_LON_CUR_OWN NOT IN ('971357')
FOR READ ONLY WITH UR
);
CREATE TABLE LNDRS AS
	SELECT *
	FROM CONNECTION TO DB2 (
		SELECT IF_DOE_LDR
			,IM_LDR_SHO
		FROM OLWHRM1.LR10_LDR_DMO 
	FOR READ ONLY WITH UR
);
CREATE TABLE CU AS
	SELECT *
	FROM CONNECTION TO DB2 (
		SELECT 'UHEAA' AS JACC
			,IF_ABA_CUS
			,IF_ACC_FIN_TRF_CUS
		FROM OLWHRM1.CU10_CUS_ACC
		WHERE IF_CUS_ACC = 'UHEA'
	FOR READ ONLY WITH UR
);
CREATE TABLE LN15 AS
	SELECT DISTINCT B.*
	FROM UOFA A 
	INNER JOIN CONNECTION TO DB2 (
		SELECT DISTINCT A1.BF_SSN
			,A1.LN_SEQ
			,A1.LN_FAT_SEQ
			,B1.LD_DSB
			,B1.LD_ECA_DSB_FUD
			,B1.LD_ECA_LON_SCH_CRT
		FROM OLWHRM1.LN93_DSB_FIN_TRX A1
		INNER JOIN OLWHRM1.LN15_DSB B1
			ON A1.BF_SSN = B1.BF_SSN
			AND A1.LN_BR_DSB_SEQ = B1.LN_BR_DSB_SEQ
		FOR READ ONLY WITH UR
		) B
		ON A.BF_SSN = B.BF_SSN
		AND A.LN_SEQ = B.LN_SEQ;
DISCONNECT FROM DB2;
/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>>  ;  * INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
/*%SQLCHECK (SQLRPT=ULWR14.LWR14RZ);*/
/*QUIT;*/
/*CALCULATE FUNDING AMOUNT,ORIGINATION FEES,AND OTHER VARIABLES*/
DATA UOFA;
	SET UOFA;
	LENGTH MSSN $ 11.;
	MSSN = CATX('-','XXX','XX',SUBSTR(BF_SSN,6,4));
	FUND_AMT = ROUND(COALESCE(CAL_VAL,0)*.99,.01) ;
	PRIN_AMT = LA_FAT_CUR_PRI + LA_FAT_ILG_PRI ;
	TOT_AMT = IA_CUR_CUS_BAL;
	IF LF_LON_CUR_OWN IN ('828476','834396','834437') THEN
		ORIG_FEE = 0;
	ELSE 
		ORIG_FEE = ROUND(COALESCE(CAL_VAL,0) - ROUND(COALESCE(CAL_VAL,0)*.99,.01) ,.01);
RUN;
/*ADD LENDER DATA TO DATA SET*/
PROC SQL;
CREATE TABLE UOFA_DET AS 
SELECT DISTINCT 
	B.IF_DOE_LDR,
	B.IM_LDR_SHO,
	A.*,
	CASE
		WHEN BF_SSN = '' THEN 'N'
		ELSE 'Y'
	END AS DIND
FROM UOFA A
RIGHT OUTER JOIN LNDRS B
	ON A.LF_LON_CUR_OWN = B.IF_DOE_LDR
	AND A.TRX_TYP IN ('1040','1041','1045')
	AND A.IF_TIR_PCE ^= ''
	AND A.LF_RGL_CAT_LP20 = '2008020'
	AND A.LF_LON_CUR_OWN NOT IN (&UHEAA_LIST)
	;
QUIT;
%SYSRPUT CUSTODIAN_LIST = &CUSTODIAN_LIST;
PROC SQL;
CREATE TABLE CUST AS 
SELECT DISTINCT 
	B.IF_DOE_LDR,
	B.IM_LDR_SHO,
	A.*,
	CASE 
		WHEN A.TRX_TYP IN ('1041','1040','1045','1044','1401','1601',
					'1448','1686','1486','1648','1048','0685','0686')
			THEN C.TRX_DSB 
		ELSE .
	END AS TRX_DSB,
	CASE 
		WHEN A.TRX_TYP IN ('1041','1040','1045','1044','1401','1601',
					'1448','1686','1486','1648','1048','0685','0686')
			 THEN C.TRX_PDATE
		ELSE D.LN_PDATE
	END AS PDATE,
	CASE 
		WHEN A.TRX_TYP IN ('1041','1040','1045','1044','1401','1601',
					'1448','1686','1486','1648','1048','0685','0686')
			THEN C.TRX_SCHD_DATE
		ELSE D.LN_SCHD_DATE		
	END AS SCHD_DATE
FROM UOFA A
INNER JOIN LNDRS B
	ON A.LF_LON_CUR_OWN = B.IF_DOE_LDR
LEFT OUTER JOIN (
	SELECT BF_SSN
		,LN_SEQ
		,LN_FAT_SEQ
		,LD_DSB	AS TRX_DSB
		,LD_ECA_DSB_FUD	AS TRX_PDATE 
		,LD_ECA_LON_SCH_CRT	AS TRX_SCHD_DATE
	FROM LN15
	) C
	ON A.BF_SSN = C.BF_SSN
	AND A.LN_SEQ = C.LN_SEQ
	AND A.LN_FAT_SEQ = C.LN_FAT_SEQ
LEFT OUTER JOIN (
	SELECT BF_SSN
		,LN_SEQ 
		,MIN(LD_ECA_DSB_FUD) AS LN_PDATE FORMAT=MMDDYY10.
		,MIN(LD_ECA_LON_SCH_CRT) AS LN_SCHD_DATE FORMAT=MMDDYY10.
	FROM LN15
	GROUP BY BF_SSN
		,LN_SEQ
	) D
	ON A.BF_SSN = D.BF_SSN
	AND A.LN_SEQ = D.LN_SEQ

WHERE 
A.LF_LON_CUR_OWN IN ('834493','83449301')
AND 
(
	(
		A.LC_CSH_ADV = 'C'
	)
OR 
	(
		A.LC_CSH_ADV = 'A'
		AND A.TRX_TYP IN ('1401','1601','0685','0686','1048','1486','1686','1448','1648','1044')
	)
)
ORDER BY TRX_TYP
;

CREATE TABLE ACH AS 
SELECT DISTINCT
	B.IF_DOE_LDR,
	B.IM_LDR_SHO,
	A.*
FROM UOFA A
INNER JOIN LNDRS B
	ON A.LF_LON_CUR_OWN = B.IF_DOE_LDR
WHERE A.LC_CSH_ADV = 'C' 
		OR
	(IF_DOE_LDR in ('834493','83449301') 
	AND  A.TRX_TYP IN ('1401','1601','0686','0685','1048','1044','1486','1686','1448','1648'))
ORDER BY B.IF_DOE_LDR
;

CREATE TABLE ACHB AS
SELECT IF_DOE_LDR
	,SUM(ORIG_FEE) * -1 AS ORIG_FEE
FROM UOFA_DET
GROUP BY IF_DOE_LDR;

CREATE TABLE ACHC AS
SELECT IF_DOE_LDR
	,IM_LDR_SHO
	,IF_CUS_ACC
	,IF_BND_ISS
	,IF_ABA_CUS
	,IF_ACC_FIN_TRF_CUS
	,SUM(LA_FAT_CUR_PRI) * -1 AS SEL_TRX_CODES
FROM ACH
WHERE TRX_TYP IN ('1401','1601','0686','0685','1048','1044','1486','1686','1448','1648')
GROUP BY IF_DOE_LDR
	,IF_CUS_ACC
	,IF_BND_ISS	;

CREATE TABLE ACHD AS
SELECT IF_DOE_LDR
	,IM_LDR_SHO
	,IF_CUS_ACC
	,IF_BND_ISS
	,IF_ABA_CUS
	,IF_ACC_FIN_TRF_CUS
	,SUM(PRIN_AMT) * -1 AS PRIN_AMT
	,SUM(LA_FAT_NSI) * -1 AS LA_FAT_NSI
	,SUM(LA_FAT_LTE_FEE) * -1 AS LA_FAT_LTE_FEE
FROM ACH
WHERE LC_CSH_ADV = 'C'
GROUP BY IF_DOE_LDR
	,IF_CUS_ACC
	,IF_BND_ISS;
QUIT;
PROC SORT DATA=ACH;
	BY IF_DOE_LDR IM_LDR_SHO IF_CUS_ACC IF_BND_ISS IF_ABA_CUS IF_ACC_FIN_TRF_CUS;
RUN;
PROC SORT DATA=ACHC;
	BY IF_DOE_LDR IM_LDR_SHO IF_CUS_ACC IF_BND_ISS IF_ABA_CUS IF_ACC_FIN_TRF_CUS;
RUN;
PROC SORT DATA=ACHD;
	BY IF_DOE_LDR IM_LDR_SHO IF_CUS_ACC IF_BND_ISS IF_ABA_CUS IF_ACC_FIN_TRF_CUS;
RUN;
DATA ACHC;
	MERGE ACH ACHC ACHD;
	BY IF_DOE_LDR IM_LDR_SHO IF_CUS_ACC IF_BND_ISS IF_ABA_CUS IF_ACC_FIN_TRF_CUS;
RUN;
PROC SORT DATA=ACHB;
	BY IF_DOE_LDR;
RUN;
DATA ACHFR (KEEP=IF_DOE_LDR IM_LDR_SHO IF_CUS_ACC IF_BND_ISS 
		PRIN_AMT LA_FAT_NSI LA_FAT_LTE_FEE TOT_AMT ORIG_FEE REIM_AMT);
	MERGE ACHC ACHB;
	BY IF_DOE_LDR;
	TOT_AMT = TOT_AMT * -1;
	IF REIM_AMT < 0 THEN REIM_AMT = 0;
	IF ORIG_FEE = . THEN ORIG_FEE = 0;
	IF SEL_TRX_CODES = . THEN SEL_TRX_CODES = 0;
	IF IA_FIN_TRF_ACL = . THEN IA_FIN_TRF_ACL = 0;
	REIM_AMT = IA_FIN_TRF_ACL - ORIG_FEE + SEL_TRX_CODES;
RUN; 
PROC SORT DATA=ACHFR NODUPKEY;
	BY IF_DOE_LDR IM_LDR_SHO IF_CUS_ACC IF_BND_ISS ;
	WHERE IM_LDR_SHO ^= '';
RUN;

PROC SQL;
CREATE TABLE LRIF AS 
	SELECT DISTINCT 
		A.IF_DOE_LDR
		,A.IM_LDR_SHO
		,CASE 
			WHEN A.IF_DOE_LDR IN ('828476','834396','82847601') THEN B.IF_ABA_CUS
			ELSE A.IF_ABA_CUS
		 END AS IF_ABA_CUS
		,CASE 
			WHEN A.IF_DOE_LDR IN ('828476','834396','82847601') THEN B.IF_ACC_FIN_TRF_CUS
			ELSE A.IF_ACC_FIN_TRF_CUS
		 END AS IF_ACC_FIN_TRF_CUS
		,COALESCE(C.REIM_AMT,0) AS REIM_AMT
	FROM ACH A
	LEFT OUTER JOIN CU B
		ON A.JACC = B.JACC
	LEFT OUTER JOIN ACHFR C
		ON A.IF_DOE_LDR = C.IF_DOE_LDR
	WHERE A.IF_DOE_LDR NOT IN ('834437','834493','83449301')
;
QUIT;
PROC SORT DATA=ACHFR; 
	BY IM_LDR_SHO IF_DOE_LDR IF_BND_ISS;
RUN; 
PROC SORT DATA=LRIF; 
	BY IM_LDR_SHO IF_DOE_LDR;
RUN; 
DATA LRIF (DROP=T_REIM_AMT);
	SET LRIF;
	BY IM_LDR_SHO IF_DOE_LDR;
	RETAIN REIM_AMT T_REIM_AMT;
	IF FIRST.IF_DOE_LDR THEN DO;
		T_REIM_AMT = REIM_AMT;
	END;
	ELSE DO;
		T_REIM_AMT =  T_REIM_AMT + REIM_AMT;
	END;
	IF LAST.IF_DOE_LDR THEN DO;
		REIM_AMT =  T_REIM_AMT ;
		OUTPUT;
	END;
RUN;
ENDRSUBMIT;
DATA UOFA_DET;SET WORKLOCL.UOFA_DET;RUN;
DATA ACHFR;SET WORKLOCL.ACHFR;RUN;
DATA LRIF;SET WORKLOCL.LRIF;RUN;
DATA CUST;SET WORKLOCL.CUST;RUN;

/************************************************
* CREATE REPORTS
*************************************************
* LENDER REPORT MACRO REPORTS 2-18 & 21
*************************************************/
OPTIONS NOSYMBOLGEN;
%MACRO PRINT_R14(LNDR_ID,RNO);
/*CREATE REPORTING TABLE*/
DATA REPDS;
	SET UOFA_DET;
	WHERE IF_DOE_LDR =  "&LNDR_ID";
RUN;
PROC SORT DATA=REPDS;
	BY BF_SSN TRX_TYP LD_FAT_EFF;
RUN;
/*CREATE MACRO VARIABLES FOR TITLES AND NO OBS REPORT*/
PROC SQL NOPRINT ;
SELECT IM_LDR_SHO
	,MAX(DIND) 
	INTO:LDR_NAME,:HAS_DAT_IND
FROM REPDS
GROUP BY IM_LDR_SHO
;
QUIT;
%PUT &HAS_DAT_IND;
%LET LDR_NAME = %TRIM(%BQUOTE(&LDR_NAME));
/*TITLES, FOOTNOTES AND OPTIONS*/
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 PAGENO=1 DATE CENTER;
TITLE 'UHEAA ORIGINATION FEE ADJUSTMENT';
TITLE2 "&LDR_NAME - &LNDR_ID";
TITLE3	"&TITLE_DATE";
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	'Please take appropriate precautions to safeguard this information.';
FOOTNOTE3	;
FOOTNOTE4   "JOB = UTLWR14  	 REPORT = ULWR14.LWR14R&RNO";
/*CREATE THE FILES*/
PROC PRINTTO PRINT=REPORT&RNO NEW;
RUN;
%IF &HAS_DAT_IND = N %THEN %DO;
	DATA _NULL_;
		FILE PRINT;
		DO;
			PUT // 127*'-';
			PUT      ////
				@51 '**** NO OBSERVATIONS FOUND ****';
			PUT ////
				@55 '-- END OF REPORT --';
			PUT ////////////
				@46 "JOB = UTLWR14  	 REPORT = ULWR14.LWR14R&RNO";
		END;
		RETURN;
	RUN;
%END;
%ELSE %DO;
	PROC PRINT NOOBS SPLIT='/' DATA=REPDS WIDTH=UNIFORM WIDTH=MIN LABEL;
	FORMAT LD_FAT_EFF MMDDYY10. LA_FAT_CUR_PRI LA_FAT_ILG_PRI FUND_AMT ORIG_FEE DOLLAR14.2;
	VAR TRX_TYP 
		IC_LON_PGM 
		LD_FAT_EFF 
		MSSN 
		DF_SPE_ACC_ID
		LN_SEQ 
		LA_FAT_CUR_PRI 
		LA_FAT_ILG_PRI 
		FUND_AMT 
		ORIG_FEE;
	LABEL TRX_TYP = 'TRANS/TYPE' 
		IC_LON_PGM = 'LOAN/TYPE' 
		LD_FAT_EFF ='EFFECTIVE/DATE' 
		MSSN = 'SSN'
		DF_SPE_ACC_ID = 'ACCOUNT/NUMBER'
		LN_SEQ = 'LOAN/SEQ' 
		LA_FAT_CUR_PRI = 'PRINCIPAL/AMOUNT' 
		LA_FAT_ILG_PRI = 'INELIGIBLE/PRINCIPAL/AMOUNT'
		FUND_AMT = 'FUNDING/AMOUNT' 
		ORIG_FEE = 'UHEAA/ORIGINATION/FEE';
	SUM LA_FAT_CUR_PRI 
		LA_FAT_ILG_PRI 
		FUND_AMT 
		ORIG_FEE;
	RUN;
%END;
PROC PRINTTO;
RUN;
%MEND PRINT_R14;
%PRINT_R14(811698,2);	/*US Bank*/
%PRINT_R14(813760UT,3);	/*Key Bank - UT*/
%PRINT_R14(813894,4);	/*Wells Fargo EFS*/
%PRINT_R14(817440,5);	/*Family First CU*/
%PRINT_R14(817545,6);	/*Granite CU*/
%PRINT_R14(817546,7);	/*Mountain America CU*/
%PRINT_R14(817575,8);	/*Alliance CU*/
%PRINT_R14(820200,9);	/*Tooele Federal CU*/
%PRINT_R14(822373,10);	/*America First CU*/
%PRINT_R14(829123,11);	/*Utah Community CU*/
%PRINT_R14(829158,12);	/*Weber State CU*/
%PRINT_R14(830132,13);	/*University of Utah CU*/
%PRINT_R14(830146,14);	/*USU Charter CU*/
%PRINT_R14(830791,15);	/*Jordan CU*/
%PRINT_R14(833577,16);	/*American United Family of CU FCU (VAMCU)*/
%PRINT_R14(833828,17);	/*Deseret First CU*/
*%PRINT_R14(834265,18);	/*Intermountain CU*/
%PRINT_R14(819628,21);	/*Chase*/
OPTIONS SYMBOLGEN;
/************************************************
* SUMMARY REPORT - R19
*************************************************/
PROC SORT DATA=UOFA_DET;
	BY IM_LDR_SHO;
RUN;
PROC PRINTTO PRINT=REPORT19 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 PAGENO=1 NODATE CENTER;
TITLE "UHEAA ORIGINATION FEE ADJUSTMENT SUMMARY";
TITLE2	"&TITLE_DATE";
FOOTNOTE   "JOB = UTLWR14  	 REPORT = ULWR14.LWR14R19";
PROC REPORT DATA=UOFA_DET HEADSKIP HEADLINE SPLIT='/' NOWD;
	WHERE DIND = 'Y';
	COLUMN DIND IM_LDR_SHO IF_DOE_LDR LA_FAT_CUR_PRI LA_FAT_ILG_PRI FUND_AMT ORIG_FEE;
	DEFINE DIND / GROUP NOPRINT;
	DEFINE IM_LDR_SHO / GROUP 'LENDER/NAME' LEFT;
	DEFINE IF_DOE_LDR / GROUP 'LENDER/CODE' LEFT;
	DEFINE LA_FAT_CUR_PRI / ANALYSIS WIDTH= 16 'TOTAL/PRINCIPAL/AMOUNT' FORMAT=DOLLAR14.2 RIGHT;
	DEFINE LA_FAT_ILG_PRI / ANALYSIS WIDTH= 16 'TOTAL/INELIGIBLE/PRINCIPAL/AMOUNT' FORMAT=DOLLAR14.2 RIGHT;
	DEFINE FUND_AMT / ANALYSIS WIDTH= 16 'TOTAL/FUNDING/AMOUNT' FORMAT=DOLLAR14.2 RIGHT;
	DEFINE ORIG_FEE / ANALYSIS WIDTH= 16 'TOTAL/UHEAA/ORIGINATION/FEE/AMOUNT' FORMAT=DOLLAR14.2 RIGHT;
	BREAK AFTER DIND / SUMMARIZE SUPPRESS OL SKIP;
RUN;
PROC PRINTTO;
RUN;
/************************************************
* UHEAA DETAIL REPORT - R20
*************************************************/
%MACRO PRINT_R20(LNDR_ID);
/*CREATE REPORTING TABLE*/
PROC SORT DATA=UOFA_DET OUT=UREP (WHERE=(IF_DOE_LDR = "&LNDR_ID"));
	BY BF_SSN TRX_TYP LD_FAT_EFF;
RUN;
/*CREATE MACRO VARIABLES FOR TITLES AND NO OBS REPORT*/
PROC SQL NOPRINT ;
SELECT IM_LDR_SHO
	,MAX(DIND) 
	INTO:LDR_NAME,:HAS_DAT_IND
FROM UREP
GROUP BY IM_LDR_SHO
;
QUIT;

%PUT &HAS_DAT_IND;
%LET LDR_NAME = %TRIM(%BQUOTE(&LDR_NAME));

TITLE 'UHEAA ORIGINATION FEE ADJUSTMENT';
TITLE2 "&LDR_NAME - &LNDR_ID";
TITLE3	"&TITLE_DATE";
FOOTNOTE1  	"This document may contain borrowers' sensitive information that UHEAA has pledged to protect.";
FOOTNOTE2	'Please take appropriate precautions to safeguard this information.';
FOOTNOTE3	;
FOOTNOTE4   "JOB = UTLWR14  	 REPORT = ULWR14.LWR14R20";

%IF &HAS_DAT_IND = N %THEN %DO;
	DATA _NULL_;
		FILE PRINT;
		DO;
			PUT // 127*'-';
			PUT      ////
				@51 '**** NO OBSERVATIONS FOUND ****';
			PUT ////
				@55 '-- END OF REPORT --';
			PUT ////////////
				@46 "JOB = UTLWR14  	 REPORT = ULWR14.LWR14R20";
		END;
		RETURN;
	RUN;
%END;
%ELSE %DO;
	PROC PRINT NOOBS SPLIT='/' DATA=UREP WIDTH=UNIFORM WIDTH=MIN LABEL;
		FORMAT LD_FAT_EFF MMDDYY10. LA_FAT_CUR_PRI LA_FAT_ILG_PRI FUND_AMT ORIG_FEE DOLLAR14.2;
		SUM LA_FAT_CUR_PRI LA_FAT_ILG_PRI FUND_AMT ORIG_FEE;
		VAR TRX_TYP 
			IC_LON_PGM 
			LD_FAT_EFF 
			BF_SSN 
			LN_SEQ 
			LA_FAT_CUR_PRI 
			LA_FAT_ILG_PRI 
			FUND_AMT 
			ORIG_FEE;
		LABEL TRX_TYP = 'TRANS/TYPE' 
			IC_LON_PGM = 'LOAN/TYPE' 
			LD_FAT_EFF ='EFFECTIVE/DATE' 
			BF_SSN = 'SSN'
			LN_SEQ = 'LOAN/SEQ' 
			LA_FAT_CUR_PRI = 'PRINCIPAL/AMOUNT' 
			LA_FAT_ILG_PRI = 'INELIGIBLE/PRINCIPAL/AMOUNT'
			FUND_AMT = 'FUNDING/AMOUNT' 
			ORIG_FEE = 'UHEAA/ORIGINATION/FEE';
	RUN;
%END;
%MEND PRINT_R20;
/*NOTE: IF LENDERS ARE ADDED PLEASE PUT THEM IN THE APPROPRIATE ALPHABETIC POSITION*/
PROC PRINTTO PRINT=REPORT20 NEW;
RUN;
OPTIONS NOSYMBOLGEN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 PAGENO=1 DATE CENTER NOBYLINE;
%PRINT_R20(817575);	/*Alliance CU*/
%PRINT_R20(822373);	/*America First CU*/
%PRINT_R20(833577);	/*American United Family of CU FCU (VAMCU)*/
%PRINT_R20(819628);	/*Chase*/
%PRINT_R20(833828);	/*Deseret First CU*/
%PRINT_R20(817440);	/*Family First CU*/
%PRINT_R20(817545);	/*Granite CU*/
*%PRINT_R20(834265);	/*Intermountain CU*/
%PRINT_R20(830791);	/*Jordan CU*/
%PRINT_R20(813760UT); /*Key Bank - UT*/
%PRINT_R20(817546);	/*Mountain America CU*/
%PRINT_R20(820200);	/*Tooele Federal CU*/
%PRINT_R20(811698);	/*US Bank*/
%PRINT_R20(830146);	/*USU Charter CU*/
%PRINT_R20(830132);	/*University of Utah CU*/
%PRINT_R20(829123);	/*Utah Community CU*/
%PRINT_R20(829158);	/*Weber State CU*/
%PRINT_R20(813894);	/*Wells Fargo EFS*/
PROC PRINTTO;
RUN;
/************************************************
* ACH REPORTS R22-R24
*************************************************/
DATA _NULL_;
SET WORK.ACHFR;
FILE REPORT22 DELIMITER=',' DSD DROPOVER LRECL=32767;
IF _N_ = 1 THEN DO;
	PUT "IF_DOE_LDR,IM_LDR_SHO,IF_CUS_ACC,IF_BND_ISS,PRIN_AMT,LA_FAT_NSI,LA_FAT_LTE_FEE,TOT_AMT,ORIG_FEE,REIM_AMT";
END;
   FORMAT IF_DOE_LDR $8. ;
   FORMAT IM_LDR_SHO $20. ;
   FORMAT LA_FAT_NSI 15.2 ;
   FORMAT LA_FAT_LTE_FEE 15.2 ;
   FORMAT IF_BND_ISS $8. ;
   FORMAT IF_CUS_ACC $4. ;
   FORMAT PRIN_AMT 12.2 ;
   FORMAT TOT_AMT BEST12. ;
   FORMAT ORIG_FEE BEST12. ;
   FORMAT REIM_AMT 20.2 ;
DO;
	PUT IF_DOE_LDR $ @;
	PUT IM_LDR_SHO $ @;
	PUT IF_CUS_ACC $ @;
	PUT IF_BND_ISS $ @;
	PUT PRIN_AMT @;
	PUT LA_FAT_NSI @;
	PUT LA_FAT_LTE_FEE @;
	PUT TOT_AMT @;
	PUT ORIG_FEE @;
	PUT REIM_AMT ;
END;
RUN;
PROC PRINTTO PRINT=REPORT23 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 PAGENO=1 CENTER DATE;
TITLE 'ACH FILE TO LENDERS ';
FOOTNOTE4   'JOB = UTLWR14  	 REPORT = ULWR14.LWR14R23';
PROC CONTENTS DATA=ACHFR OUT=EMPTYSET NOPRINT;
DATA _NULL_;
	SET EMPTYSET;
	FILE PRINT;
	IF  NOBS=0 AND _N_ =1 THEN DO;
		PUT // 126*'-';
		PUT      //////
			@51 '**** NO OBSERVATIONS FOUND ****';
		PUT //////
			@57 '-- END OF REPORT --';
		PUT //////////////
			@46 "JOB = UTLWR14  	 REPORT = ULWR14.LWR14R23";
		END;
	RETURN;
RUN;
PROC PRINT NOOBS SPLIT='/' DATA=ACHFR WIDTH=UNIFORM WIDTH=MIN LABEL;
	FORMAT PRIN_AMT LA_FAT_NSI LA_FAT_LTE_FEE TOT_AMT ORIG_FEE REIM_AMT COMMA14.2;
	VAR IF_DOE_LDR IM_LDR_SHO IF_CUS_ACC IF_BND_ISS PRIN_AMT LA_FAT_NSI 
		LA_FAT_LTE_FEE TOT_AMT ORIG_FEE REIM_AMT;
	LABEL IF_DOE_LDR = 'OWNER ID'
		IM_LDR_SHO = 'OWNER NAME' 
		IF_CUS_ACC = 'CUSTOMER ACCOUNT'
		IF_BND_ISS = 'BOND ID'
		PRIN_AMT = 'PRINCIPAL'
		LA_FAT_NSI = 'INTEREST'
		LA_FAT_LTE_FEE = 'LATE FEES'
		TOT_AMT = 'TOTALS AMOUNT'
		ORIG_FEE = 'ORIG FEE AMOUNT'
		REIM_AMT = 'REIMBURSEMENT AMOUNT';
RUN;
PROC PRINTTO;
RUN;
DATA _NULL_;
SET WORK.LRIF;
WHERE REIM_AMT > 0 AND IF_DOE_LDR NOT IN (&CUSTODIAN_LIST);
TYPE = '22';
FILE REPORT24 DELIMITER=',' DSD DROPOVER LRECL=32767;
FORMAT IF_DOE_LDR $8. ;
FORMAT IM_LDR_SHO $20. ;
FORMAT IF_ABA_CUS $9. ;
FORMAT IF_ACC_FIN_TRF_CUS $17. ;
FORMAT REIM_AMT 20.2 ;
DO;
	PUT IF_DOE_LDR $ @;
	PUT IM_LDR_SHO $ @;
	PUT REIM_AMT @;
	PUT IF_ABA_CUS $ @;
	PUT IF_ACC_FIN_TRF_CUS $ @;
	PUT TYPE $ ;
END;
RUN;

PROC SORT DATA=CUST ;
BY TRX_TYP;
RUN;

DATA _NULL_;
SET CUST;
FILE REPORT25 DELIMITER=',' DSD DROPOVER LRECL=32767;
	FORMAT IF_DOE_LDR $8. ;
	FORMAT IM_LDR_SHO $20. ;
	FORMAT LN_SEQ 6. ;
	FORMAT IC_LON_PGM $6. ;
	FORMAT LD_FAT_EFF MMDDYY10. ;
	FORMAT LA_FAT_CUR_PRI 15.2 ;
	FORMAT LA_FAT_NSI 15.2 ;
	FORMAT LA_FAT_ILG_PRI 15.2 ;
	FORMAT LA_FAT_LTE_FEE 15.2 ;
	FORMAT TRX_TYP $4. ;
	FORMAT IF_BND_ISS $8. ;
	FORMAT BF_SSN $9. ;
	FORMAT TRX_DSB MMDDYY10. ;
	FORMAT PDATE MMDDYY10. ;
	FORMAT SCHD_DATE MMDDYY10.;
IF _N_ = 1 THEN DO;
	PUT "IF_DOE_LDR,IM_LDR_SHO,IF_BND_ISS,TRX_TYP,IC_LON_PGM,LD_FAT_EFF,SSN,LN_SEQ,CLUID,LA_FAT_CUR_PRI,LA_FAT_ILG_PRI,LA_FAT_NSI,LA_FAT_LTE_FEE,TRX_DSB,FUNDING DATE,LOAN SCHEDULE CREATE DATE";	
END;
	DO;
		PUT IF_DOE_LDR $ @;
		PUT IM_LDR_SHO $ @;
		PUT IF_BND_ISS $ @;
		PUT TRX_TYP $ @;
		PUT IC_LON_PGM $ @;
		PUT LD_FAT_EFF @;
		PUT BF_SSN $ @;
		PUT LN_SEQ @;
		PUT CLUID @;
		PUT LA_FAT_CUR_PRI @;
		PUT LA_FAT_ILG_PRI @;
		PUT LA_FAT_NSI @;
		PUT LA_FAT_LTE_FEE @;
		PUT TRX_DSB @;
		PUT PDATE @	;
		PUT SCHD_DATE ;
	END;
RUN;
/*CUSTODIAN REPORTS*/
PROC SORT DATA=CUST ;
BY IM_LDR_SHO IF_DOE_LDR PDATE IF_BND_ISS TRX_TYP IC_LON_PGM LA_FAT_CUR_PRI LA_FAT_ILG_PRI LA_FAT_NSI LA_FAT_LTE_FEE;
RUN;

PROC PRINTTO PRINT=REPORT26 NEW;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 PAGENO=1 CENTER DATE;
TITLE 'CUSTODIAN REIMBURSEMENT SUMMARY REPORT';
FOOTNOTE4   'JOB = UTLWR14  	 REPORT = ULWR14.LWR14R25';
PROC CONTENTS DATA=CUST OUT=EMPTYSET NOPRINT;
DATA _NULL_;
	SET EMPTYSET;
	FILE PRINT;
	IF  NOBS=0 AND _N_ =1 THEN DO;
		PUT // 126*'-';
		PUT      //////
			@51 '**** NO OBSERVATIONS FOUND ****';
		PUT //////
			@57 '-- END OF REPORT --';
		PUT //////////////
			@46 "JOB = UTLWR14  	 REPORT = ULWR14.LWR14R23";
		END;
	RETURN;
RUN;
OPTIONS ORIENTATION = LANDSCAPE;
OPTIONS PS=39 LS=127 PAGENO=1 NODATE CENTER NOBYLINE;
TITLE 'CUSTODIAN REIMBURSEMENT SUMMARY REPORT';
TITLE2 '#BYVAL1';
TITLE3 '#BYVAL2';
FOOTNOTE4   'JOB = UTLWR14  	 REPORT = ULWR14.LWR14R25';
PROC REPORT DATA=CUST HEADSKIP HEADLINE SPLIT='/' NOWD;
	BY IM_LDR_SHO IF_DOE_LDR;
	COLUMN IF_DOE_LDR PDATE IF_BND_ISS TRX_TYP IC_LON_PGM LA_FAT_CUR_PRI LA_FAT_ILG_PRI LA_FAT_NSI
		LA_FAT_LTE_FEE;
	DEFINE IF_DOE_LDR / GROUP NOPRINT;
	DEFINE PDATE / GROUP 'FUNDING/DATE' WIDTH=13 FORMAT=MMDDYY10. LEFT;
	DEFINE IF_BND_ISS / GROUP 'BOND/ID' WIDTH=10 LEFT;
	DEFINE TRX_TYP / GROUP 'TRANS/TYPE' WIDTH=5 RIGHT ;
	DEFINE IC_LON_PGM / GROUP 'LOAN/TYPE' WIDTH=6 RIGHT ;
	DEFINE LA_FAT_CUR_PRI / ANALYSIS 'PRINCIPAL/AMOUNT' WIDTH=18 FORMAT=DOLLAR18.2 RIGHT;
	DEFINE LA_FAT_ILG_PRI / ANALYSIS 'INELIGIBLE/PRINCIPAL/AMOUNT' WIDTH=18 FORMAT=DOLLAR18.2 RIGHT;
	DEFINE LA_FAT_NSI / ANALYSIS 'INTEREST' WIDTH=18 FORMAT=DOLLAR18.2 RIGHT;
	DEFINE LA_FAT_LTE_FEE / ANALYSIS 'LATE/FEES' WIDTH=18 FORMAT=DOLLAR18.2 RIGHT;
	BREAK AFTER IF_BND_ISS / SUPPRESS SUMMARIZE SKIP OL;
	COMPUTE AFTER;
		LINE '';
		LINE @3 'TOTAL PRINCIPAL:            ' LA_FAT_CUR_PRI.SUM DOLLAR20.2;
		LINE @3 'TOTAL INELIGIBLE PRINCIPAL: ' LA_FAT_ILG_PRI.SUM DOLLAR20.2;
		LINE @3 'TOTAL INTEREST:             ' LA_FAT_NSI.SUM DOLLAR20.2;
		LINE @3 'TOTAL LATE FEES:            ' LA_FAT_LTE_FEE.SUM DOLLAR20.2;
	ENDCOMP;
RUN;
PROC PRINTTO;
RUN;