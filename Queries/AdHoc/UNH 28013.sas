/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
/*FILENAME REPORT2 "&RPTLIB/ULWDW1.LWDW1R2";*/
/*FILENAME REPORTZ "&RPTLIB/ULWDW1.LWDW1RZ";*/
/*FILENAME REPORT3 "&RPTLIB/ULWDW1.LWDW1R3";*/
/*FILENAME REPORT4 "&RPTLIB/ULWDW1.LWDW1R4";*/
/*FILENAME REPORT5 "&RPTLIB/ULWDW1.LWDW1R5";*/
/*FILENAME REPORT6 "&RPTLIB/ULWDW1.LWDW1R6";*/
/*FILENAME REPORT7 "&RPTLIB/ULWDW1.LWDW1R7";*/
/*FILENAME REPORT8 "&RPTLIB/ULWDW1.LWDW1R8";*/
/*FILENAME REPORT9 "&RPTLIB/ULWDW1.LWDW1R9";*/
/*FILENAME REPORT10 "&RPTLIB/ULWDW1.LWDW1R10";*/
/*FILENAME REPORT11 "&RPTLIB/ULWDW1.LWDW1R11";*/
/*FILENAME REPORT12 "&RPTLIB/ULWDW1.LWDW1R12";*/
/*FILENAME REPORT13 "&RPTLIB/ULWDW1.LWDW1R13";*/
/*FILENAME REPORT14 "&RPTLIB/ULWDW1.LWDW1R14";*/
/*FILENAME REPORT15 "&RPTLIB/ULWDW1.LWDW1R15";*/
/*FILENAME REPORT16 "&RPTLIB/ULWDW1.LWDW1R16";*/
/*FILENAME REPORT17 "&RPTLIB/ULWDW1.LWDW1R17";*/
/*FILENAME REPORT18 "&RPTLIB/ULWDW1.LWDW1R18";*/
/*FILENAME REPORT19 "&RPTLIB/ULWDW1.LWDW1R19";*/
/*FILENAME REPORT20 "&RPTLIB/ULWDW1.LWDW1R20";*/
/*FILENAME REPORT21 "&RPTLIB/ULWDW1.LWDW1R21";*/
/*FILENAME REPORT22 "&RPTLIB/ULWDW1.LWDW1R22";*/
/*FILENAME REPORT23 "&RPTLIB/ULWDW1.LWDW1R23";*/
/*FILENAME REPORT24 "&RPTLIB/ULWDW1.LWDW1R24";*/
/*FILENAME REPORT25 "&RPTLIB/ULWDW1.LWDW1R25";*/
/*FILENAME REPORT26 "&RPTLIB/ULWDW1.LWDW1R26";*/
/*FILENAME REPORT27 "&RPTLIB/ULWDW1.LWDW1R27";*/
/*FILENAME REPORT28 "&RPTLIB/ULWDW1.LWDW1R28";*/
/*FILENAME REPORT29 "&RPTLIB/ULWDW1.LWDW1R29";*/
/*FILENAME REPORT30 "&RPTLIB/ULWDW1.LWDW1R30";*/
/*FILENAME REPORT31 "&RPTLIB/ULWDW1.LWDW1R31";*/
/*FILENAME REPORT32 "&RPTLIB/ULWDW1.LWDW1R32";*/
/*FILENAME REPORT33 "&RPTLIB/ULWDW1.LWDW1R33";*/
/*FILENAME REPORT34 "&RPTLIB/ULWDW1.LWDW1R34";*/
/*FILENAME REPORT35 "&RPTLIB/ULWDW1.LWDW1R35";*/
/*FILENAME REPORT36 "&RPTLIB/ULWDW1.LWDW1R36";*/
/*FILENAME REPORT37 "&RPTLIB/ULWDW1.LWDW1R37";*/
/*FILENAME REPORT38 "&RPTLIB/ULWDW1.LWDW1R38";*/
/*FILENAME REPORT39 "&RPTLIB/ULWDW1.LWDW1R39";*/
/*FILENAME REPORT40 "&RPTLIB/ULWDW1.LWDW1R40";*/
/*FILENAME REPORT41 "&RPTLIB/ULWDW1.LWDW1R41";*/
/*FILENAME REPORT42 "&RPTLIB/ULWDW1.LWDW1R42";*/
/*FILENAME REPORT43 "&RPTLIB/ULWDW1.LWDW1R43";*/
/*FILENAME REPORT44 "&RPTLIB/ULWDW1.LWDW1R44";*/
/*FILENAME REPORT45 "&RPTLIB/ULWDW1.LWDW1R45";*/
/*FILENAME REPORT46 "&RPTLIB/ULWDW1.LWDW1R46";*/
/*FILENAME REPORT47 "&RPTLIB/ULWDW1.LWDW1R47";*/
/*FILENAME REPORT48 "&RPTLIB/ULWDW1.LWDW1R48";*/
/*FILENAME REPORT49 "&RPTLIB/ULWDW1.LWDW1R49";*/
/*FILENAME REPORT50 "&RPTLIB/ULWDW1.LWDW1R50";*/
/*FILENAME REPORT51 "&RPTLIB/ULWDW1.LWDW1R51";*/
/*FILENAME REPORT52 "&RPTLIB/ULWDW1.LWDW1R52";*/
/*FILENAME REPORT53 "&RPTLIB/ULWDW1.LWDW1R53";*/
/*FILENAME REPORT54 "&RPTLIB/ULWDW1.LWDW1R54";*/
/*FILENAME REPORT55 "&RPTLIB/ULWDW1.LWDW1R55";*/
/*FILENAME REPORT56 "&RPTLIB/ULWDW1.LWDW1R56";*/
FILENAME REPORT57 "&RPTLIB/ULWDW1.LWDW1R57";
FILENAME REPORT58 "&RPTLIB/ULWDW1.LWDW1R58";
FILENAME REPORT59 "&RPTLIB/ULWDW1.LWDW1R59";
FILENAME REPORT60 "&RPTLIB/ULWDW1.LWDW1R60";
FILENAME REPORT61 "&RPTLIB/ULWDW1.LWDW1R61";
FILENAME REPORT62 "&RPTLIB/ULWDW1.LWDW1R62";
FILENAME REPORT63 "&RPTLIB/ULWDW1.LWDW1R63";

/*TEST*/
/*LIBNAME  QADBD004  REMOTE  SERVER=QADBD004 SLIBREF=WORK  ;*/

/*LIVE*/
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
data _null_;
call symput('rsub',put(time(),time9.));
run;

RSUBMIT;  

/*%let DB = DLGSWQUT;  *This is test;*/
%let DB = DLGSUTWH;  *This is live;

LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1; 
LIBNAME SAS_TAB V8 '/sas/whse/progrevw';

DATA _NULL_; 
SET SAS_TAB.LASTRUN_JOBS;
/*If the job must be run manually set this macro to the last day it successfully ran(last business day).*/
LAST_RUN = TODAY() - 12;	*COMMENT FOR PROD, IT WILL READ THE DATE FROM A TABLE;
/*	IF JOB = 'UTLWDW1' THEN DO;*/
		CALL SYMPUT('LAST_RUN',"'"||TRIM(LEFT(PUT(LAST_RUN,DATE10.)))|| "'D");
		CALL SYMPUT('LAST_RUNPASS',"'"|| PUT(LAST_RUN,MMDDYY10.) || "'");
/*	END;*/
RUN;

%PUT &LAST_RUN &LAST_RUNPASS;

/*THE TABLES ARE GOING TO HAVE THE ACCOUNT NUMBER RATHER THAN SSN AS A PRIMARY KEY*/
/*THE ACCOUNT NUMBER IS BEING TAKEN FROM THE PD10 TABLE WHILE SSN IS DROPPED*/
%MACRO SSN2ACC(TBL,ADKEY);
PROC SORT DATA=&TBL; BY BF_SSN &ADKEY; RUN;
DATA &TBL(DROP=BF_SSN);
MERGE SSN2ACC(IN=B) &TBL(IN=A);
BY BF_SSN;
IF A AND B;
RUN;
PROC SORT DATA=&TBL; BY DF_SPE_ACC_ID &ADKEY; RUN;
%MEND;

/*******************************************
* BORROWER DATA
********************************************/
PROC SQL;
	CREATE TABLE SSN2ACC AS
	SELECT DISTINCT A.DF_SPE_ACC_ID
		,B.BF_SSN
	FROM OLWHRM1.PD10_PRS_NME A
	INNER JOIN OLWHRM1.LN10_LON B
		ON A.DF_PRS_ID = B.BF_SSN
/*	where b.bf_ssn like '%111'*/
;
QUIT;
PROC SORT DATA=SSN2ACC; BY BF_SSN; RUN;
/**/
/*/********************************************/
/** ENDORSER DATA*/
/*********************************************/*/
/**/
/*DATA LN20(DROP=LF_LST_DTS_LN20);*/
/*	SET OLWHRM1.LN20_EDS(KEEP=BF_SSN LC_STA_LON20 LC_EDS_TYP LF_LST_DTS_LN20 LN_SEQ);*/
/*	IF DATEPART(LF_LST_DTS_LN20) >= &LAST_RUN ;*/
/*RUN; */
/*%SSN2ACC(LN20,LN_SEQ);*/
/**/
/*/********************************************/
/** REFERENCE DATA*/
/*********************************************/*/
/*DATA BR03(DROP=BF_LST_DTS_BR03 BD_RFR_LST_ATT_HME BD_RFR_LST_ATT_ALT BD_RFR_LST_CNC BD_LTR_SNT_RFR);*/
/*	SET OLWHRM1.BR03_BR_REF(RENAME=(DF_PRS_ID_BR=BF_SSN  ) */
/*		KEEP=DF_PRS_ID_BR DF_PRS_ID_RFR BC_STA_BR03 BI_ATH_3_PTY BC_RFR_REL_BR */
/*			BM_RFR_1 BM_RFR_LST BF_LST_DTS_BR03 BD_RFR_LST_CNC BD_RFR_LST_ATT_HME */
/*			BD_RFR_LST_ATT_ALT BD_LTR_SNT_RFR);*/
/*	format LST_ATT  LST_CNC mmddyy10.;*/
/*	IF DATEPART(BF_LST_DTS_BR03) >= &LAST_RUN;*/
/*	LST_CNC = MAX(BD_RFR_LST_CNC,BD_LTR_SNT_RFR);*/
/*	LST_ATT = MAX(BD_RFR_LST_ATT_HME,BD_RFR_LST_ATT_ALT);*/
/*RUN;*/
/*%SSN2ACC(BR03,DF_PRS_ID_RFR);*/
/**/
/*/********************************************/
/** TRANSACTION DATA*/
/*********************************************/*/
/**/
/*DATA LN90(DROP=LF_LST_DTS_LN90 ) ;*/
/*	SET OLWHRM1.LN90_FIN_ATY(KEEP=BF_SSN LN_SEQ LN_FAT_SEQ LD_FAT_PST*/
/*		LD_FAT_EFF LC_STA_LON90 LA_FAT_CUR_PRI LA_FAT_NSI LA_FAT_LTE_FEE */
/*		LF_LST_DTS_LN90 PC_FAT_TYP PC_FAT_SUB_TYP LC_FAT_REV_REA );*/
/*	WHERE LC_FAT_REV_REA ^= '0'*/
/*		AND DATEPART(LF_LST_DTS_LN90) >= &LAST_RUN;*/
/*	LA_FAT_CUR_PRI = COALESCE(LA_FAT_CUR_PRI,0);*/
/*	LA_FAT_NSI = COALESCE(LA_FAT_NSI,0);*/
/*	LA_FAT_LTE_FEE = COALESCE(LA_FAT_LTE_FEE,0);*/
/*IF  PC_FAT_TYP IN ('05','10','14','15','17','26','50','51','55','60');*/
/*RUN;*/
/*%SSN2ACC(LN90,LN_SEQ LN_FAT_SEQ);*/
/**/
/*/********************************************/
/** BILLING DATA*/
/*********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DB2 (DATABASE=&DB);*/
/*CREATE TABLE LN80 AS*/
/*SELECT **/
/*FROM CONNECTION TO DB2 (*/
/*SELECT B.BF_SSN*/
/*	,B.LN_SEQ*/
/*	,B.LD_BIL_CRT*/
/*	,B.LN_SEQ_BIL_WI_DTE*/
/*	,B.LN_BIL_OCC_SEQ*/
/*	,B.LD_BIL_DU_LON*/
/*	,B.LC_STA_LON80*/
/*	,COALESCE(b.la_bil_cur_du,0) AS LA_BIL_CUR_DU*/
/*	,COALESCE(b.la_bil_pas_du,0) AS la_bil_pas_du*/
/*	,COALESCE(b.la_tot_bil_sts,0) AS la_tot_bil_sts*/
/*	,LC_BIL_MTD*/
/*	,LC_IND_BIL_SNT*/
/*	,LC_STA_BIL10*/
/*	,E.LN_FAT_SEQ*/
/*	,E.LD_FAT_EFF*/
/*FROM OLWHRM1.BL10_BR_BIL C*/
/*INNER JOIN OLWHRM1.LN80_LON_BIL_CRF B*/
/*	ON B.BF_SSN = C.BF_SSN*/
/*	AND B.LD_BIL_CRT = C.LD_BIL_CRT*/
/*	AND B.LN_SEQ_BIL_WI_DTE = C.LN_SEQ_BIL_WI_DTE*/
/*LEFT OUTER JOIN OLWHRM1.LN75_BIL_LON_FAT D*/
/*	ON D.BF_SSN = B.BF_SSN*/
/*	AND D.LN_SEQ = B.LN_SEQ*/
/*	AND D.LD_BIL_CRT = B.LD_BIL_CRT*/
/*	AND D.LN_SEQ_BIL_WI_DTE = B.LN_SEQ_BIL_WI_DTE*/
/*	AND D.LN_BIL_OCC_SEQ = B.LN_BIL_OCC_SEQ*/
/*	AND B.LA_BIL_CUR_DU = B.LA_TOT_BIL_STS*/
/*LEFT OUTER JOIN OLWHRM1.LN90_FIN_ATY E*/
/*	ON D.BF_SSN = E.BF_SSN*/
/*	AND D.LN_SEQ = E.LN_SEQ*/
/*	AND D.LN_FAT_SEQ = E.LN_FAT_SEQ*/
/*WHERE B.LC_BIL_TYP_LON = 'P' */
/*	AND B.LD_LST_DTS_LN80 >= &LAST_RUNPASS);*/
/*QUIT; */
/*proc sort data=ln80; by bf_ssn ln_seq LD_BIL_CRT LN_SEQ_BIL_WI_DTE LN_BIL_OCC_SEQ LN_FAT_SEQ; run;*/
/*data ln80(drop=LN_BIL_OCC_SEQ LN_FAT_SEQ); */
/*set ln80;*/
/*by bf_ssn ln_seq LD_BIL_CRT LN_SEQ_BIL_WI_DTE ;*/
/*if last.ln_seq_bil_wi_dte;*/
/*run;*/
/*%SSN2ACC(LN80,LN_SEQ LD_BIL_CRT LN_SEQ_BIL_WI_DTE);*/
/**/
/*/********************************************/
/** DEFERMENT DATA*/
/*********************************************/*/
/*PROC SQL;*/
/*CREATE TABLE DF10 AS*/
/*	SELECT B.BF_SSN*/
/*		,B.LN_SEQ*/
/*		,B.LF_DFR_CTL_NUM*/
/*		,B.LN_DFR_OCC_SEQ*/
/*		,C.LC_DFR_TYP*/
/*		,C.LD_DFR_INF_CER*/
/*		,B.LD_DFR_BEG*/
/*		,B.LD_DFR_END*/
/*		,B.LC_LON_LEV_DFR_CAP*/
/*		,B.LC_STA_LON50*/
/*		,C.LC_DFR_STA*/
/*		,C.LC_STA_DFR10*/
/*	FROM OLWHRM1.LN50_BR_DFR_APV B*/
/*	INNER JOIN OLWHRM1.DF10_BR_DFR_REQ C*/
/*		ON B.BF_SSN = C.BF_SSN*/
/*		AND B.LF_DFR_CTL_NUM = C.LF_DFR_CTL_NUM*/
/*	WHERE (DATEPART(B.LF_LST_DTS_LN50) >= &LAST_RUN */
/*			OR DATEPART(C.LF_LST_DTS_DF10) >= &LAST_RUN);*/
/**/
/*/********************************************/
/** FORBEARANCE DATA*/
/*********************************************/*/
/*CREATE TABLE FB10 AS*/
/*	SELECT B.BF_SSN*/
/*		,B.LN_SEQ*/
/*		,B.LF_FOR_CTL_NUM*/
/*		,B.LN_FOR_OCC_SEQ*/
/*		,C.LC_FOR_TYP*/
/*		,C.LD_FOR_INF_CER*/
/*		,B.LD_FOR_BEG*/
/*		,B.LD_FOR_END*/
/*		,B.LC_LON_LEV_FOR_CAP */
/*		,B.LC_STA_LON60*/
/*		,C.LC_FOR_STA*/
/*		,C.LC_STA_FOR10*/
/*		,C.LA_REQ_RDC_PAY*/
/*	FROM OLWHRM1.LN60_BR_FOR_APV B*/
/*	INNER JOIN OLWHRM1.FB10_BR_FOR_REQ C*/
/*		ON B.BF_SSN = C.BF_SSN*/
/*		AND B.LF_FOR_CTL_NUM = C.LF_FOR_CTL_NUM*/
/*	WHERE (DATEPART(B.LF_LST_DTS_LN60) >= &LAST_RUN*/
/*			OR DATEPART(C.LF_LST_DTS_FB10) >= &LAST_RUN);*/
/*QUIT;*/
/*%SSN2ACC(DF10,LN_SEQ LF_DFR_CTL_NUM LN_DFR_OCC_SEQ);*/
/*%SSN2ACC(FB10,LN_SEQ LF_FOR_CTL_NUM LN_FOR_OCC_SEQ);*/
/**/
/*/********************************************/
/** LOAN DATA*/
/*********************************************/*/
/*DATA LN10(DROP=	LF_LST_DTS_LN10 ) ;*/
/*	SET OLWHRM1.LN10_LON (KEEP= BF_SSN LN_SEQ LC_STA_LON10 LA_CUR_PRI LA_LON_AMT_GTR LD_END_GRC_PRD*/
/*		IC_LON_PGM LD_LON_1_DSB LF_LST_DTS_LN10 LD_PIF_RPT IF_GTR);*/
/*	WHERE DATEPART(LF_LST_DTS_LN10) >= &LAST_RUN;*/
/*RUN;*/
/*%SSN2ACC(LN10,LN_SEQ);*/
/**/
/*/********************************************/
/** ACH DATA*/
/*********************************************/*/
/*DATA LN83(DROP=LF_LST_DTS_LN83 );*/
/*	SET OLWHRM1.LN83_EFT_TO_LON(KEEP=BF_SSN LN_SEQ BN_EFT_SEQ LF_LST_DTS_LN83 */
/*		LD_EFT_EFF_END LC_STA_LN83 );*/
/*	IF DATEPART(LF_LST_DTS_LN83) >= &LAST_RUN;*/
/*RUN;*/
/*PROC SORT DATA=LN83; BY BF_SSN LN_SEQ DESCENDING BN_EFT_SEQ; RUN;*/
/*DATA LN83;*/
/*SET LN83;*/
/*BY BF_SSN LN_SEQ;*/
/*IF FIRST.LN_SEQ;*/
/*RUN;*/
/*%SSN2ACC(LN83,LN_SEQ);*/
/**/
/*/********************************************/
/** REHAB DATA */
/*********************************************/*/
/*DATA LN09(DROP= LF_LST_DTS_LN09);*/
/*	SET OLWHRM1.LN09_RPD_PIO_CVN (KEEP= BF_SSN LN_SEQ LF_LST_DTS_LN09 LD_LON_RHB_PCV);*/
/*	WHERE LD_LON_RHB_PCV ^= .*/
/*		and DATEPART(LF_LST_DTS_LN09) >= &LAST_RUN;*/
/*RUN;*/
/*%SSN2ACC(LN09,LN_SEQ);*/
/**/
/*/**********************************************/
/** DELINQUENCY DATA*/
/***********************************************/*/
/*DATA LN16(DROP=LF_LST_DTS_LN16 LC_STA_LON16);*/
/*	SET OLWHRM1.LN16_LON_DLQ_HST(KEEP=BF_SSN LN_SEQ LN_DLQ_SEQ LD_DLQ_OCC*/
/*		LF_LST_DTS_LN16 LC_STA_LON16 LN_DLQ_MAX LD_DLQ_MAX);*/
/*	IF DATEPART(LF_LST_DTS_LN16) >= &LAST_RUN;*/
/*	IF LC_STA_LON16 ^= '1' OR (LC_STA_LON16 = '1' AND LD_DLQ_MAX ^= TODAY() - 1) THEN DO;*/
/*		LN_DLQ_MAX = 0;*/
/*		LD_DLQ_OCC = .;*/
/*	END;*/
/*RUN;*/
/*PROC SORT DATA=LN16; BY BF_SSN LN_SEQ DESCENDING LN_DLQ_SEQ; RUN;*/
/*DATA LN16 TILPCHECK;*/
/*SET LN16(DROP=LN_DLQ_SEQ);*/
/*BY BF_SSN LN_SEQ;*/
/*IF FIRST.LN_SEQ;*/
/*RUN;*/
/*%SSN2ACC(LN16,LN_SEQ);*/
/**/
/**/
/*/********************************************/
/** CALL FORWARDING DATA*/
/*********************************************/*/
/*PROC SQL;*/
/*/*get TILP borrowers*/*/
/*CREATE TABLE TILP AS*/
/*	SELECT H.DF_SPE_ACC_ID*/
/*		,a.bf_ssn*/
/*		,'' AS CLUID*/
/*		,A.LN_SEQ*/
/*		,CASE */
/*			WHEN LN_DLQ_MAX >= 90 THEN /*'06'*/ '04'*/
/*			ELSE '04'*/
/*		END AS FORWARDING*/
/*	FROM OLWHRM1.PD10_PRS_NME H */
/*	INNER JOIN OLWHRM1.LN10_LON A*/
/*		ON A.BF_SSN = H.DF_PRS_ID*/
/*	LEFT OUTER JOIN TILPCHECK B*/
/*		ON A.BF_SSN = B.BF_SSN*/
/*		AND A.LN_SEQ = B.LN_SEQ*/
/*	WHERE A.IC_LON_PGM = 'TILP';*/
/**/
/*/*	get all OneLINK borrowers*/*/
/*	CREATE TABLE ALL_ONELINK AS*/
/*		SELECT DISTINCT*/
/*			PD01.DF_SPE_ACC_ID,*/
/*			PD01.DF_PRS_ID AS BF_SSN,*/
/*			GA10.AF_APL_ID||GA10.AF_APL_ID_SFX AS CLUID,*/
/*			. AS LN_SEQ,*/
/*			/*'01'*/ '04' AS FORWARDING*/
/*		FROM*/
/*			OLWHRM1.PD01_PDM_INF PD01*/
/*			JOIN OLWHRM1.GA01_APP GA01*/
/*				ON PD01.DF_PRS_ID = GA01.DF_PRS_ID_BR*/
/*			JOIN OLWHRM1.GA10_LON_APP GA10*/
/*				ON GA01.AF_APL_ID = GA10.AF_APL_ID*/
/*		WHERE*/
/*			DATEPART(GA01.AF_LST_DTS_GA01) >= &LAST_RUN*/
/*			OR DATEPART(GA10.AF_LST_DTS_GA10) >= &LAST_RUN*/
/*	;*/
/**/
/*/*	get all COMPASS borrowers*/*/
/*	CREATE TABLE ALL_COMPASS AS*/
/*		SELECT DISTINCT*/
/*			PD10.DF_SPE_ACC_ID,*/
/*			PD10.DF_PRS_ID AS BF_SSN,*/
/*			LN10.LF_LON_ALT||PUT(LN10.LN_LON_ALT_SEQ,Z2.) AS CLUID,*/
/*			LN10.LN_SEQ,*/
/*			CASE*/
/*				WHEN LN10.IC_LON_PGM = 'COMPLT' THEN '04'*/
/*				WHEN DW01.WC_DW_LON_STA = '12' THEN /*'01'*/ '04'*/
/*				WHEN DW01.WC_DW_LON_STA IS NULL THEN '04'*/
/*				ELSE '04'*/
/*			END AS FORWARDING,*/
/*			LN10.LF_LST_DTS_LN10,*/
/*			LN10.IC_LON_PGM*/
/*		FROM*/
/*			OLWHRM1.PD10_PRS_NME PD10*/
/*			JOIN OLWHRM1.LN10_LON LN10*/
/*				ON PD10.DF_PRS_ID = LN10.BF_SSN*/
/*			LEFT JOIN OLWHRM1.DW01_DW_CLC_CLU DW01*/
/*				ON LN10.BF_SSN = DW01.BF_SSN*/
/*				AND LN10.LN_SEQ = DW01.LN_SEQ*/
/*	;*/
/**/
/*/*	get OneLINK borrower who are not also on COMPASS*/*/
/*	CREATE TABLE ONELINK AS*/
/*		SELECT*/
/*			O.DF_SPE_ACC_ID,*/
/*			O.BF_SSN,*/
/*			O.CLUID,*/
/*			O.LN_SEQ,*/
/*			O.FORWARDING*/
/*		FROM*/
/*			ALL_ONELINK O*/
/*			LEFT JOIN ALL_COMPASS C*/
/*				ON O.BF_SSN = C.BF_SSN*/
/*		WHERE*/
/*			C.BF_SSN IS NULL*/
/*	;*/
/**/
/*/*	get non-TILP borrowers where LN10 was updated since last run*/*/
/*	CREATE TABLE COMPASS AS*/
/*		SELECT*/
/*			DF_SPE_ACC_ID,*/
/*			BF_SSN,*/
/*			CLUID,*/
/*			LN_SEQ,*/
/*			FORWARDING*/
/*		FROM*/
/*			ALL_COMPASS*/
/*		WHERE*/
/*			DATEPART(LF_LST_DTS_LN10) >= &LAST_RUN*/
/*			AND IC_LON_PGM <> 'TILP'*/
/*	;*/
/**/
/*/*	combine and sort COMPASS, OneLINK-only, and TILP borrowers*/*/
/*	CREATE TABLE CALLFWD AS*/
/*		SELECT*/
/*			DF_SPE_ACC_ID,*/
/*			BF_SSN,*/
/*			CLUID,*/
/*			LN_SEQ,*/
/*			FORWARDING*/
/*		FROM*/
/*			(*/
/*				SELECT * FROM COMPASS */
/*				UNION */
/*				SELECT * FROM ONELINK */
/*				UNION */
/*				SELECT * FROM TILP*/
/*			)*/
/*		ORDER BY*/
/*			DF_SPE_ACC_ID,*/
/*			CLUID,*/
/*			LN_SEQ*/
/*	;		*/
/*QUIT;*/
/**/
/**/
/*/********************************************/
/** BORROWER BENEFIT ELIGIBILITY*/
/*********************************************/*/
/*DATA LN54(DROP=LF_LST_DTS_LN54);*/
/*	SET OLWHRM1.LN54_LON_BBS(KEEP=BF_SSN LN_SEQ LC_STA_LN54 LD_BBS_DSQ LC_BBS_ELG LF_LST_DTS_LN54);*/
/*	IF DATEPART(LF_LST_DTS_LN54) >= &LAST_RUN ;*/
/*RUN;*/
/*PROC SORT DATA=LN54; BY BF_SSN LN_SEQ LC_STA_LN54; RUN;*/
/*DATA LN54(drop=LC_STA_LN54);*/
/*SET LN54;*/
/*BY BF_SSN LN_SEQ LC_STA_LN54;*/
/*IF FIRST.LN_SEQ;*/
/*RUN;*/
/*%SSN2ACC(LN54,LN_SEQ);*/
/**/
/*/********************************************/
/** BORROWER BENEFITS*/
/*********************************************/*/
/*DATA LN55(DROP=LF_LST_DTS_LN55);*/
/*	SET OLWHRM1.LN55_LON_BBS_TIR*/
/*		(KEEP=BF_SSN LN_SEQ PM_BBS_PGM PN_BBS_PGM_SEQ LC_STA_LN55 LN_LON_BBT_PAY LF_LST_DTS_LN55);*/
/*	IF DATEPART(LF_LST_DTS_LN55) >= &LAST_RUN ;*/
/*	LN_LON_BBT_PAY = COALESCE(LN_LON_BBT_PAY,0);*/
/*RUN;*/
/*PROC SORT DATA=LN55; BY BF_SSN LN_SEQ LC_STA_LN55; RUN;*/
/*DATA LN55;*/
/*	SET LN55;*/
/*		BY BF_SSN LN_SEQ LC_STA_LN55;*/
/*	IF FIRST.LN_SEQ;*/
/*RUN;*/
/*DATA RP02;*/
/*	SET OLWHRM1.RP02_BBS_PGM_TIR(KEEP=PM_BBS_PGM PN_BBS_PGM_SEQ  */
/*		PN_BBT_PAY_ICV );*/
/*RUN;*/
/*PROC SORT DATA=LN55; BY PM_BBS_PGM PN_BBS_PGM_SEQ;RUN;*/
/*PROC SORT DATA=RP02; BY PM_BBS_PGM PN_BBS_PGM_SEQ;RUN;*/
/*DATA LN55(DROP=PM_BBS_PGM PN_BBS_PGM_SEQ);*/
/*	MERGE LN55(IN=A) RP02;*/
/*		BY PM_BBS_PGM PN_BBS_PGM_SEQ;*/
/*	IF A;*/
/*RUN;*/
/*%SSN2ACC(LN55,LN_SEQ);*/
/**/
/*/********************************************/
/** REPAYMENT SCHEDULE DATA*/
/*********************************************/*/
/*PROC SQL;*/
/*CREATE TABLE LN65 AS*/
/*	SELECT C.BF_SSN*/
/*		,C.LN_SEQ*/
/*		,D.LN_GRD_RPS_SEQ*/
/*		,D.LN_RPS_TRM*/
/*		,B.LD_RPS_1_PAY_DU*/
/*		,B.LD_SNT_RPD_DIS*/
/*		,C.LD_CRT_LON65*/
/*		,C.LC_TYP_SCH_DIS*/
/*		,D.LA_RPS_ISL*/
/*	FROM OLWHRM1.RS10_BR_RPD B*/
/*	INNER JOIN OLWHRM1.LN65_LON_RPS C*/
/*		ON B.BF_SSN = C.BF_SSN*/
/*		AND B.LN_RPS_SEQ = C.LN_RPS_SEQ*/
/*	INNER JOIN OLWHRM1.LN66_LON_RPS_SPF D*/
/*		ON C.BF_SSN = D.BF_SSN*/
/*		AND C.LN_SEQ = D.LN_SEQ*/
/*		AND C.LN_RPS_SEQ = D.LN_RPS_SEQ*/
/*	WHERE C.LC_STA_LON65 = 'A';*/
/*QUIT;*/
/*PROC SORT DATA=LN65; BY BF_SSN LN_SEQ LN_GRD_RPS_SEQ; RUN;*/
/*DATA LN65(DROP= A B C NEXT_SEQ LD_RPS_1_PAY_DU LN_GRD_RPS_SEQ);*/
/*SET LN65;*/
/*FORMAT NEXT_SEQ DATE9.;*/
/*BY BF_SSN LN_SEQ LN_GRD_RPS_SEQ;*/
/*RETAIN NEXT_SEQ A C B;*/
/*IF FIRST.LN_SEQ THEN DO;*/
/*	A = .;*/
/*	NEXT_SEQ = INTNX('MONTH',LD_RPS_1_PAY_DU,LN_RPS_TRM,'S');*/
/*	IF NEXT_SEQ > TODAY() THEN DO;*/
/*		A = LN_GRD_RPS_SEQ ;*/
/*		C = LA_RPS_ISL ;*/
/*	END;*/
/*END;*/
/*ELSE IF A= . THEN DO;*/
/*	NEXT_SEQ = INTNX('MONTH',NEXT_SEQ,LN_RPS_TRM,'S');*/
/*	IF NEXT_SEQ > TODAY() THEN DO;*/
/*		A = LN_GRD_RPS_SEQ ;*/
/*		C = LA_RPS_ISL ;*/
/*	END;*/
/*END;*/
/*IF FIRST.LN_SEQ THEN B = LN_RPS_TRM;*/
/*ELSE B = B + LN_RPS_TRM;*/
/*IF LAST.LN_SEQ THEN DO;*/
/*	LN_RPS_TRM = B;*/
/*	LA_RPS_ISL = C;*/
/*	LN_GRD_RPS_SEQ = A;*/
/*	DAY_DUE = DAY(LD_RPS_1_PAY_DU);*/
/*	OUTPUT;*/
/*END;*/
/*RUN;*/
/*%SSN2ACC(LN65,LN_SEQ);*/
/**/
/*/********************************************/
/** INTEREST RATE DATA*/
/*********************************************/*/
/*DATA LN72(DROP=LC_STA_LON72 LD_ITR_EFF_BEG LD_ITR_EFF_END LF_LST_DTS_LN72);*/
/*	SET OLWHRM1.LN72_INT_RTE_HST(KEEP=BF_SSN LN_SEQ LC_STA_LON72 */
/*		LD_ITR_EFF_BEG LD_ITR_EFF_END LF_LST_DTS_LN72 LR_ITR LR_INT_RDC_PGM_ORG);*/
/*	WHERE LC_STA_LON72 = 'A'*/
/*		AND LD_ITR_EFF_BEG <= TODAY() */
/*		AND LD_ITR_EFF_END >= TODAY() */
/*		AND DATEPART(LF_LST_DTS_LN72) >= &LAST_RUN;*/
/*	LR_ITR = COALESCE(LR_ITR,0);*/
/*	LR_INT_RDC_PGM_ORG = COALESCE(LR_INT_RDC_PGM_ORG,LR_ITR);*/
/*RUN;*/
/*%SSN2ACC(LN72,LN_SEQ);*/
/**/
/*/**********************************************/
/** ENROLLMENT - LOAN LEVEL*/
/***********************************************/*/
/*PROC SQL;*/
/*CREATE TABLE SD10 AS*/
/*	SELECT A.BF_SSN*/
/*		,A.LN_SEQ*/
/*		,B.LN_STU_SPR_SEQ*/
/*		,B.LD_SCL_SPR*/
/*		,D.IM_SCL_FUL*/
/*	FROM OLWHRM1.SD10_STU_SPR B*/
/*	INNER JOIN OLWHRM1.SC10_SCH_DMO D*/
/*		ON B.LF_DOE_SCL_ENR_CUR = D.IF_DOE_SCL*/
/*	INNER JOIN OLWHRM1.LN13_LON_STU_OSD C*/
/*		ON B.LF_STU_SSN = C.LF_STU_SSN*/
/*		AND B.LN_STU_SPR_SEQ = C.LN_STU_SPR_SEQ*/
/*	INNER JOIN OLWHRM1.LN10_LON A*/
/*		ON C.BF_SSN = A.BF_SSN*/
/*		AND C.LN_SEQ = A.LN_SEQ*/
/*	WHERE B.LC_STA_STU10 = 'A'*/
/*		AND C.LC_STA_LON13 = 'A'*/
/*		AND (DATEPART(B.LF_LST_DTS_SD10) >= &LAST_RUN*/
/*			OR DATEPART(C.LF_CRT_DTS_LN13) >= &LAST_RUN);*/
/*QUIT;*/
/*PROC SORT DATA=SD10; BY BF_SSN LN_SEQ LN_STU_SPR_SEQ;RUN;*/
/*DATA SD10(DROP=LN_STU_SPR_SEQ);*/
/*	SET SD10;*/
/*		BY BF_SSN LN_SEQ LN_STU_SPR_SEQ;*/
/*	IF LAST.LN_SEQ;*/
/*RUN;*/
/*%SSN2ACC(SD10,LN_SEQ);*/
/**/
/*/**********************************************/
/** EMPLOYER DATA*/
/***********************************************/*/
/*PROC SQL;*/
/*CREATE TABLE BR02 AS*/
/*	SELECT A.BF_SSN*/
/*		,B.IM_IST_FUL*/
/*		,B.IX_GEN_STR_ADR_1*/
/*		,B.IX_GEN_STR_ADR_2*/
/*		,B.IM_GEN_CT*/
/*		,B.IC_GEN_ST*/
/*		,B.IF_GEN_ZIP*/
/*		,B.IN_GEN_PHN*/
/*	FROM OLWHRM1.BR02_BR_EMP A*/
/*	INNER JOIN OLWHRM1.IN01_LGS_IDM_MST B*/
/*		ON A.BF_EMP_ID_1 = B.IF_IST*/
/*	WHERE DATEPART(A.BF_LST_DTS_BR02) >= &LAST_RUN;*/
/*QUIT;*/
/*%SSN2ACC(BR02,);*/
/**/
/*/**********************************************/
/** AUTOPAY DATA*/
/***********************************************/*/
/*DATA BR30(DROP=BF_LST_DTS_BR30 );*/
/*	SET OLWHRM1.BR30_BR_EFT(KEEP=BF_SSN BN_EFT_SEQ BF_EFT_ABA BF_EFT_ACC BC_EFT_STA*/
/*		BD_EFT_STA BA_EFT_ADD_WDR BN_EFT_NSF_CTR BN_EFT_DAY_DUE BC_EFT_DNL_REA BF_LST_DTS_BR30);*/
/*	IF DATEPART(BF_LST_DTS_BR30) >= &LAST_RUN ;*/
/*RUN;*/
/*PROC SORT DATA=BR30; BY BF_SSN BC_EFT_STA DESCENDING BN_EFT_SEQ; RUN;*/
/*DATA BR30; */
/*SET BR30; */
/*BY BF_SSN BC_EFT_STA DESCENDING BN_EFT_SEQ;*/
/*IF FIRST.BF_SSN OR BC_EFT_STA = 'A';*/
/*RUN;*/
/*PROC SORT DATA=BR30; BY BF_SSN BN_EFT_SEQ; RUN;*/
/*DATA BR30(DROP=A B BN_EFT_DAY_DUE);*/
/*FORMAT A BN_EFT_DAY_DUE $8.;*/
/*SET BR30;*/
/*BY BF_SSN;*/
/*RETAIN A B;*/
/*IF FIRST.BF_SSN AND LAST.BF_SSN THEN OUTPUT;*/
/*ELSE IF FIRST.BF_SSN AND LAST.BF_SSN = 0 AND BC_EFT_STA = 'A' THEN DO;*/
/*	A = BN_EFT_DAY_DUE;*/
/*	B = BA_EFT_ADD_WDR;*/
/*END;*/
/*ELSE IF FIRST.BF_SSN = 0 AND BC_EFT_STA = 'A' THEN DO;*/
/*	IF A ^= BN_EFT_DAY_DUE THEN A = TRIM(A) || ',' || BN_EFT_DAY_DUE;*/
/*	B = B + BA_EFT_ADD_WDR;*/
/*END;*/
/*IF FIRST.BF_SSN = 0 AND LAST.BF_SSN THEN DO;*/
/*	BN_EFT_DAY_DUE = A;*/
/*	BA_EFT_ADD_WDR = B;*/
/*	OUTPUT; */
/*END;*/
/*RUN;*/
/*%SSN2ACC(BR30,);*/
/**/
/*/**********************************************/
/** DEMOGRAPHICS*/
/***********************************************/*/
/*PROC SQL;*/
/*CREATE TABLE PD10 AS*/
/*	SELECT distinct A.DF_SPE_ACC_ID*/
/*		,a.df_prs_id as bf_ssn*/
/*		,A.DM_PRS_1*/
/*		,A.DM_PRS_LST*/
/*		,A.DM_PRS_MID*/
/*		,A.DD_BRT*/
/*	FROM OLWHRM1.PD10_PRS_NME A*/
/*	WHERE substr(df_prs_id,1,1) ^= 'P'*/
/*		and DATEPART(A.DF_LST_DTS_PD10) >= &LAST_RUN;*/
/**/
/*CREATE TABLE PD30 AS*/
/*	SELECT distinct c.df_prs_id as bf_ssn*/
/*		,C.DX_STR_ADR_1*/
/*		,C.DX_STR_ADR_2*/
/*		,C.DM_CT*/
/*		,C.DC_DOM_ST*/
/*		,C.DF_ZIP_CDE*/
/*		,C.DM_FGN_ST*/
/*		,C.DM_FGN_CNY*/
/*		,C.DD_VER_ADR*/
/*		,C.DI_VLD_ADR*/
/*	FROM OLWHRM1.PD30_PRS_ADR C*/
/*	WHERE C.DC_ADR = 'L'*/
/*		AND DATEPART(C.DF_LST_DTS_PD30) >= &LAST_RUN;*/
/**/
/*CREATE TABLE PD42 AS*/
/*	SELECT DISTINCT d.df_prs_id as bf_ssn*/
/*		,D.DC_PHN*/
/*		,D.DC_ALW_ADL_PHN*/
/*		,D.DD_PHN_VER*/
/*		,D.DI_PHN_VLD*/
/*		,D.DN_DOM_PHN_ARA*/
/*		,D.DN_DOM_PHN_XCH*/
/*		,D.DN_DOM_PHN_LCL*/
/*		,D.DN_PHN_XTN*/
/*		,D.DN_FGN_PHN_CNY*/
/*		,D.DN_FGN_PHN_CT*/
/*		,D.DN_FGN_PHN_LCL*/
/*	FROM  OLWHRM1.PD42_PRS_PHN D*/
/*	WHERE DATEPART(D.DF_LST_DTS_PD42) >= &LAST_RUN;*/
/**/
/*	CREATE TABLE PD32 AS*/
/*		SELECT DISTINCT */
/*			DF_PRS_ID AS BF_SSN,*/
/*			DC_ADR_EML,*/
/*			DX_ADR_EML,*/
/*			DD_VER_ADR_EML,*/
/*			DI_VLD_ADR_EML*/
/*		FROM */
/*			OLWHRM1.PD32_PRS_ADR_EML*/
/*		WHERE */
/*			DATEPART(DF_LST_DTS_PD32) >= &LAST_RUN*/
/*			AND DC_STA_PD32 = 'A'*/
/*	;*/
/*QUIT;*/
/*proc sort data=pd32; by bf_ssn dc_adr_eml descending dd_ver_adr_eml; run;*/
/*data pd32;*/
/*set pd32;*/
/*by bf_ssn dc_adr_eml;*/
/*if first.dc_adr_eml;*/
/*run;*/
/*%SSN2ACC(PD30,);*/
/*%SSN2ACC(PD42,DC_PHN);*/
/*%SSN2ACC(PD32,dc_adr_eml);*/
/**/
/*/**********************************************/
/** SUSPENSE PAYMENTS*/
/***********************************************/*/
/*PROC SQL;*/
/*CREATE TABLE RM31 AS*/
/*	SELECT DISTINCT A.BF_SSN*/
/*		,SUM(A.LA_BR_RMT_PST) AS LA_BR_RMT_PST*/
/*	FROM OLWHRM1.RM31_BR_RMT_PST A*/
/*	WHERE LC_RMT_STA_PST = 'S'*/
/*	GROUP BY BF_SSN;*/
/*QUIT;*/
/*%SSN2ACC(RM31,);*/
/**/
/*/**********************************************/
/** AMOUNTS DUE INFORMATION*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=DLGSUTWH);*/
/**/
/*	CREATE TABLE CUR_DUE AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT DISTINCT*/
/*						LN10.BF_SSN*/
/*						,SUM(CUR.CUR_DUE) AS CUR_DUE*/
/*						,SUM(PST.PAST_DUE) AS PAST_DUE*/
/*						,SUM(COALESCE(CUR.CUR_DUE,0) + COALESCE(PST.PAST_DUE,0)) AS TOT_DUE*/
/*						,SUM(COALESCE(CUR.CUR_DUE,0) + COALESCE(PST.PAST_DUE,0) + COALESCE(LN10.LA_LTE_FEE_OTS,0)) AS TOT_DUE_FEE*/
/*					FROM OLWHRM1.LN10_LON LN10*/
/*						LEFT JOIN (*/
/*							SELECT*/
/*								LN80.BF_SSN*/
/*								,ln80.ln_seq*/
/*								,SUM(COALESCE(LN80.LA_BIL_CUR_DU,0) - COALESCE(LN80.LA_TOT_BIL_STS,0)) AS CUR_DUE*/
/*							FROM OLWHRM1.LN80_LON_BIL_CRF LN80*/
/*								JOIN (*/
/*										SELECT*/
/*											LN80.BF_SSN*/
/*											,ln80.ln_seq*/
/*											,MIN(LN80.LD_BIL_DU_LON) AS LD_BIL_DU_LON*/
/*										FROM OLWHRM1.LN80_LON_BIL_CRF LN80*/
/*										WHERE LN80.LC_STA_LON80 = 'A'*/
/*											AND LN80.LC_LON_STA_BIL = '1'*/
/*											AND LN80.LD_BIL_DU_LON > CURRENT_DATE*/
/*										GROUP BY LN80.BF_SSN*/
/*												,ln80.ln_seq*/
/*											) MIN_DTE*/
/*									ON LN80.BF_SSN = MIN_DTE.BF_SSN*/
/*										and ln80.ln_seq = min_dte.ln_seq*/
/*										AND LN80.LD_BIL_DU_LON = MIN_DTE.LD_BIL_DU_LON*/
/*							WHERE LN80.LC_STA_LON80 = 'A'*/
/*								AND LN80.LC_LON_STA_BIL = '1'*/
/*							GROUP BY LN80.BF_SSN*/
/*									,ln80.ln_seq*/
/*									) CUR*/
/*							ON LN10.BF_SSN = CUR.BF_SSN*/
/*							and ln10.ln_seq = cur.ln_seq*/
/*						LEFT JOIN (*/
/*							SELECT*/
/*								LN80.BF_SSN*/
/*								,ln80.ln_seq*/
/*								,SUM(COALESCE(LN80.LA_BIL_CUR_DU,0) - COALESCE(LN80.LA_TOT_BIL_STS,0)) AS PAST_DUE*/
/*								,sum(ln80.la_bil_cur_du) as la_bil_cur_du*/
/*								,sum(ln80.la_tot_bil_sts) as la_tot_bil_sts*/
/*							FROM OLWHRM1.LN80_LON_BIL_CRF LN80*/
/*							WHERE LN80.LC_STA_LON80 = 'A'*/
/*								AND LN80.LC_LON_STA_BIL = '1'*/
/*								AND LN80.LD_BIL_DU_LON <= CURRENT_DATE*/
/*							GROUP BY LN80.BF_SSN*/
/*									,ln80.ln_seq*/
/*									) PST*/
/*							ON LN10.BF_SSN = PST.BF_SSN*/
/*							and ln10.ln_seq = pst.ln_seq*/
/*					WHERE ln10.la_cur_pri > 0*/
/*						and ln10.lc_sta_lon10 = 'R'*/
/*/*						and	LN10.BF_SSN like '%1111'*/*/
/*					GROUP BY LN10.BF_SSN*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/*%SSN2ACC(CUR_DUE,);*/
/**/
/*/**********************************************/
/** ADDITIONAL LOAN DATA*/
/***********************************************/*/
/*PROC SQL;*/
/*CREATE TABLE DW01 AS*/
/*SELECT A.BF_SSN*/
/*	,A.LN_SEQ*/
/*	,A.WC_DW_LON_STA */
/*	,A.WA_TOT_BRI_OTS */
/*	,A.WD_LON_RPD_SR */
/*	,A.WX_OVR_DW_LON_STA*/
/*FROM OLWHRM1.DW01_DW_CLC_CLU A*/
/*INNER JOIN OLWHRM1.LN10_LON B*/
/*	ON A.BF_SSN = B.BF_SSN*/
/*	AND A.LN_SEQ = B.LN_SEQ*/
/*WHERE DATEPART(LF_LST_DTS_LN10) >= &LAST_RUN*/
/*	OR B.LA_CUR_PRI > 0;*/
/*QUIT;*/
/*%SSN2ACC(DW01,LN_SEQ);*/
/**/
/*/**********************************************/
/** CREDIT REPORTING INFORMATION*/
/***********************************************/*/
/*PROC SQL;*/
/*CREATE TABLE LN56 AS*/
/*SELECT A.BF_SSN */
/*	,A.LN_SEQ*/
/*	,A.LC_RPT_STA_CRB*/
/*	,A.LD_RPT_CRB*/
/*	,B.LD_STA_LN56 AS DT_ADJ*/
/*FROM OLWHRM1.LN56_LON_CRB_RPT A*/
/*LEFT OUTER JOIN OLWHRM1.LN56_LON_CRB_RPT B*/
/*	ON A.BF_SSN = B.BF_SSN*/
/*	AND A.LN_SEQ = B.LN_SEQ*/
/*	AND A.LD_RPT_CRB = B.LD_RPT_CRB*/
/*	AND SUBSTR(A.LF_LST_USR_LN56,1,2) = 'UT'*/
/*	AND B.LC_STA_LN56 = 'I'*/
/*WHERE A.LD_RPT_CRB > INTNX('YEAR',TODAY(),-1,'S')*/
/*	AND A.LC_STA_LN56 = 'A'*/
/*	AND DATEPART(A.LF_LST_DTS_LN56) >= &LAST_RUN;*/
/*QUIT;*/
/*%SSN2ACC(LN56,LN_SEQ);*/
/**/
/*/**********************************************/
/** ACTIVITY HISTORY*/
/***********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DB2 (DATABASE=&DB);*/
/*CREATE TABLE ARCS AS*/
/*SELECT **/
/*FROM CONNECTION TO DB2 (*/
/*	SELECT AY10.BF_SSN*/
/*		,AY10.LN_ATY_SEQ*/
/*		,AY10.PF_REQ_ACT*/
/*		,AY10.LC_STA_ACTY10*/
/*		,LX_ATY*/
/*	FROM OLWHRM1.AY10_BR_LON_ATY AY10*/
/*	LEFT OUTER JOIN OLWHRM1.AY15_ATY_CMT AY15*/
/*		ON AY10.BF_SSN = AY15.BF_SSN */
/*		AND AY10.LN_ATY_SEQ = AY15.LN_ATY_SEQ*/
/*		AND AY15.LN_ATY_CMT_SEQ = 1*/
/* 	LEFT OUTER JOIN OLWHRM1.AY20_ATY_TXT AY20*/
/*		ON AY15.BF_SSN = AY20.BF_SSN */
/*		AND AY15.LN_ATY_SEQ = AY20.LN_ATY_SEQ*/
/*		AND AY15.LN_ATY_CMT_SEQ = AY20.LN_ATY_CMT_SEQ*/
/*	WHERE AY10.PF_REQ_ACT IN ('DRLFA','K0ADD','K0PHN','M1411')*/
/*		AND (AY10.LF_LST_DTS_AY10 >= &LAST_RUNPASS or AY10.PF_REQ_ACT = 'DRLFA')*/
/*FOR READ ONLY WITH UR*/
/*);*/
/**/
/*CREATE TABLE ARC_IND AS*/
/*SELECT **/
/*FROM CONNECTION TO DB2 (*/
/*	SELECT AY10.BF_SSN*/
/*		,AY10.LN_ATY_SEQ*/
/*		,AY10.PF_REQ_ACT*/
/*		,AY10.LC_STA_ACTY10*/
/*	FROM OLWHRM1.AY10_BR_LON_ATY AY10*/
/*	WHERE AY10.PF_REQ_ACT IN ('SPHAE','VIPSS')*/
/*		AND AY10.LF_LST_DTS_AY10 >= &LAST_RUNPASS*/
/*FOR READ ONLY WITH UR*/
/*);*/
/**/
/*	CREATE TABLE DL200 AS*/
/*		SELECT **/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT */
/*						AY10.BF_SSN*/
/*						,AY10.LN_ATY_SEQ*/
/*						,AY10.LD_ATY_REQ_RCV*/
/*						,AY10.LC_STA_ACTY10*/
/*					FROM*/
/*						OLWHRM1.AY10_BR_LON_ATY AY10*/
/*					WHERE */
/*						AY10.PF_REQ_ACT = 'DL200'*/
/*						AND AY10.LF_LST_DTS_AY10 >= &LAST_RUNPASS*/
/*				)*/
/*	;*/
/**/
/*	CREATE TABLE ARCHIST AS*/
/*		SELECT **/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT */
/*						AY10.BF_SSN*/
/*						,AY10.LN_ATY_SEQ*/
/*						,AY10.LC_STA_ACTY10*/
/*						,AY10.PF_REQ_ACT*/
/*						,AY10.PF_RSP_ACT*/
/*						,AY10.LD_ATY_REQ_RCV*/
/*						,AY10.LD_ATY_RSP*/
/*						,AY10.LF_USR_REQ_ATY*/
/*						,AY10.LT_ATY_RSP*/
/*						,AY20.LX_ATY*/
/*						,AC10.PX_ACT_DSC_REQ*/
/*					FROM */
/*						OLWHRM1.AY10_BR_LON_ATY AY10*/
/*					 	LEFT JOIN OLWHRM1.AY20_ATY_TXT AY20*/
/*							ON AY10.BF_SSN = AY20.BF_SSN */
/*							AND AY10.LN_ATY_SEQ = AY20.LN_ATY_SEQ*/
/*							AND AY20.LN_ATY_CMT_SEQ = 1*/
/*							AND AY20.LN_ATY_TXT_SEQ = 1*/
/*						LEFT JOIN OLWHRM1.AC10_ACT_REQ AC10*/
/*							ON AY10.PF_REQ_ACT = AC10.PF_REQ_ACT*/
/*					WHERE */
/*						AY10.LC_ATY_RCP = 'B'*/
/*						AND AY10.LF_LST_DTS_AY10 >= &LAST_RUNPASS*/
/*				)*/
/*	;*/
/*DISCONNECT FROM DB2;*/
/*QUIT;*/
/**/
/*DATA DRLFA ARCS(DROP=DOL) ;*/
/*SET ARCS;*/
/*IF PF_REQ_ACT = 'DRLFA' THEN DO;*/
/*	IF SUBSTR(LX_ATY,1,1) = '{' and lc_sta_acty10 = 'A' THEN DO;*/
/*		DOL = INPUT(SCAN(LX_ATY,1,'{}'),DOLLAR8.2);*/
/*		OUTPUT DRLFA;*/
/*	END;*/
/*END;*/
/*ELSE OUTPUT ARCS;*/
/*RUN;*/
/**/
/*PROC SORT DATA=DRLFA; BY BF_SSN;RUN;*/
/*DATA DRLFA (KEEP=BF_SSN FEE_WAV_DOL FEE_WAV_CT);*/
/*	SET DRLFA;*/
/*	RETAIN FEE_WAV_DOL FEE_WAV_CT;*/
/*	BY BF_SSN;*/
/*	IF FIRST.BF_SSN THEN DO;*/
/*		FEE_WAV_DOL = DOL;*/
/*		FEE_WAV_CT = 1;*/
/*	END;*/
/*	ELSE DO;*/
/*		FEE_WAV_DOL + DOL;*/
/*		FEE_WAV_CT + 1;*/
/*	END;*/
/*	IF LAST.BF_SSN THEN OUTPUT;	*/
/*RUN;*/
/**/
/*DATA K0ADD(KEEP=BF_SSN LN_ATY_SEQ LC_STA_ACTY10 DX_STR_ADR_1 DX_STR_ADR_2 DM_CT DC_DOM_ST DF_ZIP_CDE DM_FGN_CNY COMMENTS)*/
/*	K0PHN(KEEP=BF_SSN LN_ATY_SEQ LC_STA_ACTY10 PHN1 PHN2 PHN3 COMMENTS)*/
/*	ARC_M1411(KEEP=BF_SSN LN_ATY_SEQ LC_STA_ACTY10 LX_ATY);*/
/*SET ARCS;*/
/*LENGTH DX_STR_ADR_1 $30. DX_STR_ADR_2 $30. DM_CT $20. DC_DOM_ST $2. DF_ZIP_CDE $17. DM_FGN_CNY $25. COMMENTS $300.;*/
/*LENGTH  PHN1 PHN2 PHN3 $25.;*/
/*ARRAY ADR{7} $ DX_STR_ADR_1 DX_STR_ADR_2 DM_CT DC_DOM_ST DF_ZIP_CDE DM_FGN_CNY COMMENTS;*/
/*ARRAY PHN{4} $ PHN1 PHN2 PHN3 COMMENTS;*/
/*IF PF_REQ_ACT = 'K0ADD' THEN DO;*/
/*	DO I = 1 TO 7;*/
/*		ADR(I) = SCAN(LX_ATY,I,',');*/
/*	END;*/
/*	OUTPUT K0ADD;*/
/*END;*/
/*ELSE if pf_req_act = 'K0PHN' THEN DO;*/
/*	DO I = 1 TO 4;*/
/*		PHN(I) = SCAN(LX_ATY,I,',');*/
/*	END;*/
/*	OUTPUT K0PHN;*/
/*END;*/
/*ELSE IF PF_REQ_ACT = 'M1411' THEN OUTPUT ARC_M1411;*/
/*RUN;*/
/**/
/*%SSN2ACC(K0ADD,LN_ATY_SEQ);*/
/*%SSN2ACC(K0PHN,LN_ATY_SEQ);*/
/*%SSN2ACC(DL200,LN_ATY_SEQ);*/
/*%SSN2ACC(DRLFA,);*/
/*%SSN2ACC(ARCHIST,LN_ATY_SEQ); */
/*%SSN2ACC(ARC_M1411,LN_ATY_SEQ);*/
/*%SSN2ACC(ARC_IND,LN_ATY_SEQ); */
/**/
/**/
/*/**********************************************/
/** DISBURSEMENT*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE LN15_DISB AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						LN15.BF_SSN,*/
/*						LN15.LN_BR_DSB_SEQ,*/
/*						LN15.LA_DSB - COALESCE(LN15.LA_DSB_CAN,0) AS LA_DSB,*/
/*						LN15.LD_DSB,*/
/*						LN15.LC_DSB_TYP,*/
/*						LN15.LC_STA_LON15,*/
/*						LN15.LN_SEQ,*/
/*						COALESCE(LN15.LA_DL_DSB_REB,0) - COALESCE(LN15.LA_DSB_REB_CAN,0) AS LA_DL_REBATE*/
/*					FROM*/
/*						OLWHRM1.LN15_DSB LN15*/
/*					WHERE*/
/*						LN15.LF_LST_DTS_LN15 >= &LAST_RUNPASS*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/*%SSN2ACC(LN15_DISB,LN_BR_DSB_SEQ);*/
/**/
/*/**********************************************/
/** AD20*/
/***********************************************/*/
/**/
/*proc sql;*/
/*CREATE TABLE AD20 AS*/
/*	SELECT */
/*		AD20.BF_SSN,*/
/*		AD20.LD_FAT_ADJ_REQ,*/
/*		AD20.LN_SEQ_FAT_ADJ_REQ, */
/*		AD20.LC_TYP_FAT_ADJ_REQ, */
/*		AD20.LC_STA_FAT_ADJ_REQ	*/
/*	FROM */
/*		OLWHRM1.AD20_PCV_ATY_ADJ AD20*/
/*	WHERE */
/*		DATEPART(AD20.LF_LST_DTS_AD20) >= &LAST_RUN*/
/*;*/
/*QUIT;*/
/*%SSN2ACC(AD20,);*/
/**/
/**/
/*/**********************************************/
/** Master Application*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE AP03 AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						AF_APL_ID,*/
/*						BF_SSN,*/
/*						AN_SEQ,*/
/*						LF_STU_SSN,*/
/*						IC_LON_PGM,*/
/*						AD_CRT_APL,*/
/*						AC_STA_ORG_PRC,*/
/*						AD_STA_ORG_PRC,*/
/*						AC_APL_SCR_STA,*/
/*						AC_APL_PRC_TYP,*/
/*						AI_BR_DLQ_FED_DET,*/
/*						CASE WHEN AI_BR_DET = 'Y' THEN 1 ELSE 0 END AS AI_BR_DET,*/
/*						CASE WHEN AC_BR_INT_OPT = 'Y' THEN 1 ELSE 0 END AS AC_BR_INT_OPT,*/
/*						AC_BR_RPD_OPT,*/
/*						CASE WHEN AI_BR_NED_ORG_FEE = 'Y' THEN 1 ELSE 0 END AS AI_BR_NED_ORG_FEE,*/
/*						CASE WHEN AI_BR_ATH_DSB_EFT = 'Y' THEN 1 ELSE 0 END AS AI_BR_ATH_DSB_EFT,*/
/*						CASE WHEN AI_EDS_XST_ON_APL = 'Y' THEN 1 ELSE 0 END AS AI_EDS_XST_ON_APL,*/
/*						CASE WHEN AI_RFR_XST_ON_APL = 'Y' THEN 1 ELSE 0 END AS AI_RFR_XST_ON_APL,*/
/*						AD_STU_LON_TRM_BEG,*/
/*						AD_STU_LON_TRM_END,*/
/*						AD_STU_GRD_XPC,*/
/*						AI_STU_FGN_CNY_STY,*/
/*						AC_STU_MAJ_STY,*/
/*						AX_STU_CNL_MAJ_STY,*/
/*						AC_STU_SCL_ENR_STA,*/
/*						CASE WHEN AI_STU_DFR_REQ = 'Y' THEN 1 ELSE 0 END AS AI_STU_DFR_REQ,*/
/*						AA_STU_REQ,*/
/*						AF_DOE_SCL,*/
/*						AF_CNL_NON_EDU_BRH,*/
/*						AD_SCL_LON_TRM_BEG,*/
/*						AD_SCL_LON_TRM_END,*/
/*						AD_SCL_ACA_TRM_BEG,*/
/*						AD_SCL_ACA_TRM_END,*/
/*						AD_SCL_GRD_XPC,*/
/*						AC_SCL_STA_STU_ENR,*/
/*						AC_SCL_ACA_GDE_LEV,*/
/*						AC_SCL_ACA_PGS,*/
/*						AA_SCL_EDU_CST,*/
/*						AA_SCL_EDU_AID_TOT,*/
/*						AA_SCL_EFC,*/
/*						AA_SCL_REQ,*/
/*						AI_SCL_CNF_MPN_OVR,*/
/*						CASE WHEN AI_SCL_STU_PAS_DU = 'Y' THEN 1 ELSE 0 END AS AI_SCL_STU_PAS_DU,*/
/*						AN_SCL_AMT_CLC_MLT,*/
/*						AF_DOE_LDR,*/
/*						AF_LDR_NON_EDU_BRH,*/
/*						AF_LDR_APL_FIL,*/
/*						AA_LDR_APV,*/
/*						AC_LDR_PGM_YR,*/
/*						AI_LDR_CNF_MPN_OVR,*/
/*						AF_LDR_BND_ISS,*/
/*						AF_LDR_FEE_TIR,*/
/*						AI_LDR_NO_FEE_OVR,*/
/*						AF_LDR_ITR_TIR,*/
/*						AI_LDR_NO_ITR_OVR,*/
/*						AD_LDR_APL_REJ,*/
/*						AF_LDR_ORG,*/
/*						AI_LDR_MAT_SLL_LON,*/
/*						AF_LDR_LST_RST,*/
/*						IF_GTR,*/
/*						AF_GTR_RFR,*/
/*						AA_GTR_LON,*/
/*						AD_GTR_LON_REQ,*/
/*						AD_GTR_ORG,*/
/*						AR_GTR_INT,*/
/*						AC_GTR_ITR_TYP,*/
/*						AR_GTR_FEE,*/
/*						AI_GTR_TL4_PIO_787,*/
/*						AI_GTR_DSB_LTE_AZ,*/
/*						AD_GTR_DSB_MAX_LTE,*/
/*						AA_GTR_RSC,*/
/*						AD_GTR_RSC,*/
/*						AC_GTR_DFR_REQ,*/
/*						AC_GTR_INT_BIL_OPT,*/
/*						AC_GTR_REA_RDC,*/
/*						AA_GTR_TOT_SFD_BAL,*/
/*						AA_GTR_TOT_PLS_BAL,*/
/*						AI_GTR_RSC,*/
/*						AC_SRC_APL,*/
/*						AC_VRS_ALT_APL,*/
/*						AC_SER_RUE,*/
/*						AD_PNT_SIG,*/
/*						CASE WHEN AI_PNT_VER = 'Y' THEN 1 ELSE 0 END AS AI_PNT_VER,*/
/*						AF_PNT_VER_BY,*/
/*						AD_PNT_VER,*/
/*						CASE WHEN AI_ESC = 'Y' THEN 1 ELSE 0 END AS AI_ESC,*/
/*						AC_APL_HLD,*/
/*						AD_APL_HLD,*/
/*						AA_STF_APV,*/
/*						AA_ESM_RPD,*/
/*						AD_ESM_RPD_SR,*/
/*						AD_ESM_POF,*/
/*						AC_SYS_CLC_DFR,*/
/*						AA_ANL_INC,*/
/*						AA_MTH_CLC_RPD,*/
/*						AA_MTH_CMB_DET,*/
/*						AA_MTH_CMB_INC,*/
/*						AR_CMB_DET_INC_RTO,*/
/*						AA_TOT_SFD_BAL,*/
/*						AA_TOT_PLS_BAL,*/
/*						AA_TOT_SLS_BAL,*/
/*						AA_TOT_PRK_BAL,*/
/*						AA_TOT_HEA_BAL,*/
/*						AA_OTH_EDU_LON_BAL,*/
/*						AA_TOT_STU_LN_BAL,*/
/*						AA_ORG_FEE_PD,*/
/*						AA_GTR_FEE_PD,*/
/*						AC_REA_CPY_AP03,*/
/*						AF_APL_ID_CPY,*/
/*						CASE WHEN AI_EXT_CVN = 'Y' THEN 1 ELSE 0 END AS AI_EXT_CVN,*/
/*						AF_APL_ID_CPY_ORG,*/
/*						AN_SEQ_CVN,*/
/*						AN_SIG_ON_PNT,*/
/*						AF_RGL_CAT_LP06,*/
/*						AF_RGL_CAT_LP09,*/
/*						AF_RGL_CAT_LP10,*/
/*						AF_RGL_CAT_LP12,*/
/*						AF_RGL_CAT_LP20,*/
/*						AD_LON_1_DSB,*/
/*						AC_ORG_DPT,*/
/*						AR_LP_TB_ITR_MGN,*/
/*						AD_AMR_BEG,*/
/*						AR_SCL_SUB,*/
/*						AC_PNT_DEL,*/
/*						AD_PNT_SNT,*/
/*						AC_SCL_NTF_STA_CHG,*/
/*						CASE WHEN AI_DSB_REJ_ACK_FIL = 'Y' THEN 1 ELSE 0 END AS AI_DSB_REJ_ACK_FIL,*/
/*						AC_PIO_ORG_STA,*/
/*						AC_MN_TYP,*/
/*						AC_MN_SRL_LON,*/
/*						AI_MN_PSD_BS,*/
/*						AC_MN_CNF,*/
/*						AC_MN_BR_CNF,*/
/*						AD_MN_EXP,*/
/*						AI_MN_NOG,*/
/*						AI_MN_LN_OF_CRD,*/
/*						AC_MN_SRL_LON_ORG,*/
/*						AD_MN_BR_CNF_SNT,*/
/*						AD_MN_BR_CNF_RCV,*/
/*						AA_MN_BR_CNF,*/
/*						AC_MN_REV_REA,*/
/*						AF_MN_MST_NTE,*/
/*						AN_MN_MST_NTE_SEQ,*/
/*						CASE WHEN AI_MN_ORG_RGT_SLD = 'Y' THEN 1 ELSE 0 END AS AI_MN_ORG_RGT_SLD,*/
/*						AF_MN_RGT_SLD_TO,*/
/*						AF_CNL,*/
/*						AF_CNL_SFX,*/
/*						AX_CNL_GTR_USE,*/
/*						AC_CNL_GTR_STA,*/
/*						AD_CNL_GTR_STA,*/
/*						AF_CNL_GTR_TRT_DTS,*/
/*						AX_CNL_LDR_USE,*/
/*						AC_CNL_LDR_SER_STA,*/
/*						AD_CNL_LDR_SER_STA,*/
/*						AF_CNL_LDR_TRT_DTS,*/
/*						AX_CNL_SCL_USE,*/
/*						AA_CNL_ACL_RTD,*/
/*						AA_CNL_WDR_GR_RFD,*/
/*						AA_CNL_AVL_RNS,*/
/*						AC_CNL_MTD_FUD_RTD,*/
/*						AI_CNL_DSB_CNS,*/
/*						AC_CNL_PRC_TYP_APL,*/
/*						AC_CNL_RPT_STA,*/
/*						AC_CNL_SRV_TYP,*/
/*						AC_CNL_ITL_RSP,*/
/*						AC_FLW_TYP,*/
/*						AC_FLW_STA,*/
/*						AD_FLW_STA,*/
/*						AD_FLW_APV,*/
/*						AF_LST_USR_AP03,*/
/*						AF_LST_DTS_AP03,*/
/*						AD_LON_1_PAY_DU,*/
/*						CASE WHEN AI_STA_ORG_PRC_CHG = 'Y' THEN 1 ELSE 0 END AS AI_STA_ORG_PRC_CHG,*/
/*						CASE WHEN AI_PNT_PRT = 'Y' THEN 1 ELSE 0 END AS AI_PNT_PRT,*/
/*						AD_TO_SER_CMP,*/
/*						AI_LDR_RGT_SLD_CHG,*/
/*						AC_MN_MST_NTE_ASG,*/
/*						AC_CRD_WOR_RED,*/
/*						AI_FEE_MNL_CLC,*/
/*						AC_BR_ST_DSB_1,*/
/*						AC_SCL_ST_DSB_1,*/
/*						AC_FIL_TYP_REQ,*/
/*						AC_FIL_RCP,*/
/*						AC_PNT_YR,*/
/*						AC_STP_PUR,*/
/*						AI_PIO_APL_LDR,*/
/*						AI_SCL_ENT_CCL,*/
/*						AI_DEG_SKG,*/
/*						AA_BS_POI,*/
/*						AF_CUR_POR,*/
/*						AF_DTS_APL_ELG_CER,*/
/*						AD_DTR_APL_CER_CMP,*/
/*						AF_USR_CER_CLG,*/
/*						AC_EDU_LEV_TYP,*/
/*						AA_LST_CTR_OFR,*/
/*						AD_LST_CTR_OFR,*/
/*						AC_BR_CTR_OFR_RSP,*/
/*						AN_BR_PIO_ALT_DNL,*/
/*						AF_ALT_LON_APL,*/
/*						AC_ALT_APL_ELG_STA,*/
/*						AC_ALT_APL_ACT_STA,*/
/*						AF_APL_SLC_DIS,*/
/*						AI_TLX_APL,*/
/*						AF_PNT_VRS,*/
/*						AD_APL_EXP,*/
/*						AC_APL_EXP_EVT,*/
/*						AC_APL_RVW,*/
/*						AC_CNL_TRN_REA,*/
/*						AC_ALT_APL_FLW*/
/*					FROM*/
/*						OLWHRM1.AP03_MASTER_APL AP03*/
/*					WHERE*/
/*						AP03.AF_LST_DTS_AP03 >= &LAST_RUNPASS*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/*/*%SSN2ACC(AP03,);*/*/
/**/
/*/*********************************************/
/** LN40*/
/********************************************/*/
/**/
/*proc sql;*/
/*CREATE TABLE LN40 AS*/
/*	SELECT */
/*		LN40.**/
/*	FROM */
/*		OLWHRM1.LN40_LON_CLM_PCL LN40*/
/*	WHERE */
/*		DATEPART(LN40.LF_LST_DTS_LN40) >= &LAST_RUN*/
/*/*		and bf_ssn like '%11'*/*/
/*;*/
/*QUIT;*/
/*/*%SSN2ACC(LN40,);*/*/
/**/
/*/**********************************************/
/** CL10*/
/***********************************************/*/
/**/
/*proc sql;*/
/*CREATE TABLE CL10 AS*/
/*	SELECT */
/*		CL10.**/
/*	FROM */
/*		OLWHRM1.CL10_CLM_PCL CL10*/
/*	WHERE */
/*		DATEPART(CL10.LF_LST_DTS_CL10) >= &LAST_RUN*/
/*/*		and bf_ssn like '%11'*/*/
/*;*/
/*QUIT;*/
/*/*%SSN2ACC(CL10,);*/*/
/**/
/*/**********************************************/
/** LN80A*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE LN80A AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						LN80.**/
/*					FROM*/
/*						OLWHRM1.LN80_LON_BIL_CRF LN80*/
/*					WHERE*/
/*						LN80.LD_LST_DTS_LN80 >= &LAST_RUNPASS*/
/*/*						and bf_ssn like '%11'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/*/*%SSN2ACC(LN80,);*/*/
/**/
/*/**********************************************/
/** SC10*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE SC10 AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						SC10.**/
/*					FROM*/
/*						OLWHRM1.SC10_SCH_DMO SC10*/
/*					WHERE*/
/*						SC10.IF_LST_DTS_SC10 >= &LAST_RUNPASS*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/**/
/*/**********************************************/
/** PD20*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE PD20 AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						PD20.**/
/*					FROM*/
/*						OLWHRM1.PD20_PRS_DTH PD20*/
/*					WHERE*/
/*						PD20.PF_LST_DTS_PD20 >= &LAST_RUNPASS*/
/*/*						and df_prs_id like '%11'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/*/*%SSN2ACC(PD20,);*/*/
/**/
/*/**********************************************/
/** PD21*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE PD21 AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						PD21.**/
/*					FROM*/
/*						OLWHRM1.PD21_GTR_DTH PD21*/
/*					WHERE*/
/*						PD21.DF_LST_DTS_PD21 >= &LAST_RUNPASS*/
/*/*						and df_prs_id like '%11'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/*/*%SSN2ACC(PD21,);*/*/
/**/
/*/**********************************************/
/** PD22*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE PD22 AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						PD22.**/
/*					FROM*/
/*						OLWHRM1.PD22_PRS_DSA PD22*/
/*					WHERE*/
/*						PD22.DF_LST_DTS_PD22 >= &LAST_RUNPASS*/
/*/*						and df_prs_id like '%11'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/*/*%SSN2ACC(PD22,);*/*/
/**/
/*/**********************************************/
/** PD23*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE PD23 AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						PD23.**/
/*					FROM*/
/*						OLWHRM1.PD23_GTR_DSA PD23*/
/*					WHERE*/
/*						PD23.DF_LST_DTS_PD23 >= &LAST_RUNPASS*/
/*/*						and df_prs_id like '%11'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/*/*%SSN2ACC(PD23,);*/*/
/**/
/*/**********************************************/
/** PD24*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE PD24 AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						PD24.**/
/*					FROM*/
/*						OLWHRM1.PD24_PRS_BKR PD24*/
/*					WHERE*/
/*						PD24.DF_LST_DTS_PD24 >= &LAST_RUNPASS*/
/*/*						and df_prs_id like '%11'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/*/*%SSN2ACC(PD24,);*/*/
/**/
/*/**********************************************/
/** RS05*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE RS05 AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						**/
/*					FROM*/
/*						OLWHRM1.RS05_IBR_RPS RS05*/
/*					WHERE*/
/*						RS05.BF_LST_DTS_RS05 >= &LAST_RUNPASS*/
/*/*						and bf_ssn like '%11'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/*/*%SSN2ACC(RS05,);*/*/
/**/
/*/**********************************************/
/** MR01*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE MR01 AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						BF_SSN ,  */
/*						LN_SEQ ,  */
/*						LA_CUR_PRI ,  */
/*						WA_ACR_BRI_RUN_DTE ,  */
/*						WD_RUN ,  */
/*						WN_DAY_DLQ_INT ,  */
/*						WD_DCO_INT ,  */
/*						WA_PSS_DU_INT ,  */
/*						WA_CUR_INT_DU ,  */
/*						WN_DAY_DLQ_ISL ,  */
/*						WD_DCO_ISL ,  */
/*						WA_PSS_DU_ISL ,  */
/*						WA_CUR_DU_ISL ,  */
/*						IC_LON_PGM ,  */
/*						WX_LON_PGM ,  */
/*						IF_BND_ISS ,  */
/*						LD_LON_1_DSB ,  */
/*						LD_LON_EFF_ADD ,  */
/*						LF_STU_SSN ,  */
/*						LD_STA_STU10 ,  */
/*						LD_SCL_SPR ,  */
/*						LD_DFR_BEG ,  */
/*						LD_DFR_END ,  */
/*						LD_DFR_GRC_END ,  */
/*						LC_DFR_RSP ,  */
/*						WX_DFR_RSP ,  */
/*						LD_DFR_APL ,  */
/*						LD_FOR_BEG ,  */
/*						LD_FOR_END ,  */
/*						LC_FOR_RSP ,  */
/*						WX_FOR_RSP ,  */
/*						LD_FOR_APL ,  */
/*						WA_RPS_ISL_1 ,  */
/*						WN_RPS_TRM_1 ,  */
/*						WA_RPS_ISL_2 ,  */
/*						WN_RPS_TRM_2 ,  */
/*						WA_RPS_ISL_3 ,  */
/*						WN_RPS_TRM_3 ,  */
/*						WA_RPS_ISL_4 ,  */
/*						WN_RPS_TRM_4 ,  */
/*						WA_RPS_ISL_5 ,  */
/*						WN_RPS_TRM_5 ,  */
/*						WA_RPS_ISL_6 ,  */
/*						WN_RPS_TRM_6 ,  */
/*						WA_RPS_ISL_7 ,  */
/*						WN_RPS_TRM_7 ,  */
/*						LD_RPS_1_PAY_DU ,  */
/*						WC_ITR_TYP_1 ,  */
/*						WX_ITR_TYP_1 ,  */
/*						WR_ITR_1 ,  */
/*						WD_ITR_EFF_BEG_1 ,  */
/*						WC_ITR_TYP_2 ,  */
/*						WX_ITR_TYP_2 ,  */
/*						WR_ITR_2 ,  */
/*						WD_ITR_EFF_BEG_2 ,  */
/*						DM_PRS_1 ,  */
/*						DM_PRS_MID ,  */
/*						DM_PRS_LST ,  */
/*						DM_PRS_LST_SFX ,  */
/*						CASE WHEN DI_PHN_VLD = 'Y' THEN 1 ELSE 0 END AS DI_PHN_VLD ,*/
/*						DN_DOM_PHN_ARA ,  */
/*						IF_GTR ,  */
/*						LF_LON_OWN_CUR ,  */
/*						LF_DOE_SCL_ORG ,  */
/*						LF_DOE_SCL_ENR_CUR ,  */
/*						LF_GTR_RFR ,  */
/*						LD_END_GRC_PRD ,  */
/*						CASE WHEN LC_ELG_SIN = 'Y' THEN 1 ELSE 0 END AS LC_ELG_SIN ,*/
/*						WX_ELG_SIN ,  */
/*						LF_CUR_POR ,  */
/*						LF_OWN_ORG_POR ,  */
/*						LC_LOC_PNT ,  */
/*						WX_LOC_PNT ,  */
/*						LD_OWN_EFF_SR ,  */
/*						WC_ISL_DLQ_CAT ,  */
/*						WX_ISL_DLQ_CAT ,  */
/*						WC_INT_DLQ_CAT ,  */
/*						WX_INT_DLQ_CAT ,  */
/*						WA_ORG_PRI ,  */
/*						WN_ATV_DSB ,  */
/*						WN_ACL_DSB ,  */
/*						WN_ANT_DSB ,  */
/*						WC_LON_STA ,  */
/*						WX_LON_STA ,  */
/*						WC_LON_SUB_STA ,  */
/*						WX_LON_SUB_STA ,  */
/*						WC_LON_CLM_STA ,  */
/*						WX_LON_CLM_STA ,  */
/*						WC_BR_PRS_STA ,  */
/*						WX_BR_PRS_STA ,  */
/*						LC_DFR_TYP ,  */
/*						WX_DFR_TYP ,  */
/*						LC_FOR_TYP ,  */
/*						WX_FOR_TYP ,  */
/*						LC_FOR_SUB_TYP ,  */
/*						WX_FOR_SUB_TYP ,  */
/*						LC_TYP_SCH_DIS ,  */
/*						WX_TYP_SCH_DIS ,  */
/*						LD_NTF_SCL_SPR ,  */
/*						LD_SPA_STP ,  */
/*						LD_SPA_STP_ENT ,  */
/*						LD_SPA_RTT ,  */
/*						LD_SPA_RTT_ENT ,  */
/*						WA_ACR_BRI_MTT ,  */
/*						WA_CUR_LTE_FEE ,  */
/*						WA_PRV_LTE_FEE ,  */
/*						LC_ST_BR_RSD_APL ,  */
/*						LC_STA_NEW_BR ,  */
/*						WF_NON_PR_ACT_REQ ,  */
/*						WD_FNL_DMD_BR ,  */
/*						WD_FNL_DMD_EDS ,  */
/*						LC_IND_BIL_SNT ,  */
/*						LC_BIL_MTD ,  */
/*						LD_DSB ,  */
/*						WA_LST_DSB_WK ,  */
/*						CASE WHEN WI_LON_FUL_DSB_WK = 'Y' THEN 1 ELSE 0 END AS WI_LON_FUL_DSB_WK*/
/*					FROM*/
/*						OLWHRM1.MR01_MGT_RPT_LON MR01*/
/*					WHERE*/
/*						MR01.WD_RUN >= &LAST_RUNPASS*/
/*/*						and bf_ssn like '%11'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/*/*%SSN2ACC(MR01,);*/*/
/**/
/*/**********************************************/
/** LN10A*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE LN10A AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						BF_SSN ,  */
/*						LN_SEQ ,  */
/*						LC_STA_LON10 ,  */
/*						LI_FGV_PGM ,  */
/*						LF_LON_SLE_PND ,  */
/*						LA_CUR_PRI ,  */
/*						LI_ELG_CPN_BOK ,  */
/*						LA_LON_AMT_GTR ,  */
/*						LD_LON_GTR ,  */
/*						LD_PIF_PNT_RTN ,  */
/*						LD_PNT_SIG ,  */
/*						LC_INT_BIL_OPT ,  */
/*						LI_CAP_ALW ,  */
/*						LD_END_GRC_PRD ,  */
/*						LN_MTH_GRC_PRD_DSC ,  */
/*						LA_R78_INT_PD ,  */
/*						LA_R78_INT_MAX ,  */
/*						LD_CAP_LST_PIO_CVN ,  */
/*						LD_TRM_END ,  */
/*						LD_TRM_BEG ,  */
/*						LI_GTR_NAT ,  */
/*						LF_GTR_RFR ,  */
/*						CASE WHEN LI_ELG_SPA = 'Y' THEN 1 ELSE 0 END AS LI_ELG_SPA, */
/*						LD_GTE_LOS ,  */
/*						LA_SCL_CLS ,  */
/*						LA_CUR_ILG ,  */
/*						LA_ILG ,  */
/*						LD_PIF_RPT ,  */
/*						LA_NSI_OTS ,  */
/*						LD_NSI_ACR_THU ,  */
/*						LD_STA_LON10 ,  */
/*						LD_LON_ACL_ADD ,  */
/*						LD_LON_EFF_ADD ,  */
/*						LF_DOE_SCL_ORG ,  */
/*						LC_PCV_DIS_STA ,  */
/*						LC_RPR_TYP ,  */
/*						LF_RGL_CAT_LP19 ,  */
/*						LF_RGL_CAT_LP18 ,  */
/*						LF_RGL_CAT_LP17 ,  */
/*						LF_RGL_CAT_LP13 ,  */
/*						LF_RGL_CAT_LP12 ,  */
/*						LF_RGL_CAT_LP11 ,  */
/*						LF_RGL_CAT_LP10 ,  */
/*						LF_RGL_CAT_LP08 ,  */
/*						LF_RGL_CAT_LP07 ,  */
/*						LF_RGL_CAT_LP06 ,  */
/*						LF_RGL_CAT_LP05 ,  */
/*						LF_RGL_CAT_LP04 ,  */
/*						LF_RGL_CAT_LP03 ,  */
/*						LF_RGL_CAT_LP02 ,  */
/*						LF_RGL_CAT_LP01 ,  */
/*						LD_SCL_CLS_NTF ,  */
/*						LD_ILG_NTF ,  */
/*						LF_LON_CUR_OWN ,  */
/*						LF_LST_DTS_LN10 ,  */
/*						LC_STA_NEW_BR ,  */
/*						LC_SCY_PGA ,  */
/*						LD_SIN_LST_PD_PCV ,  */
/*						LD_SIN_ACR_THU_PCV ,  */
/*						LA_SIN_OTS_PCV ,  */
/*						IC_LON_PGM ,  */
/*						PF_MAJ_BCH ,  */
/*						PF_MNR_BCH ,  */
/*						IF_DOE_LDR ,  */
/*						IF_GTR ,  */
/*						LF_STU_SSN ,  */
/*						LD_LON_1_DSB ,  */
/*						LC_ACA_GDE_LEV ,  */
/*						LD_NEW_SYS_CVN ,  */
/*						LC_SCY_PGA_PGM_YR ,  */
/*						IC_HSP_CSE ,  */
/*						LI_TL4_793_XCL_CON ,  */
/*						LI_DFR_REQ_ON_APL ,  */
/*						LI_LN_PT_COM_APL ,  */
/*						LN_SEQ_RPR ,  */
/*						LR_WIR_CON_LON ,  */
/*						LR_INT_RDC_PGM_DSU ,  */
/*						LI_1_TME_BR ,  */
/*						BF_SSN_RPR ,  */
/*						LC_ELG_RDC_PGM ,  */
/*						LD_ELG_RDC_PGM ,  */
/*						LC_RPD_SLE ,  */
/*						LR_ITR_ORG ,  */
/*						LC_ITR_TYP_ORG ,  */
/*						LC_RDC_PGM ,  */
/*						LC_TIR_GRP ,  */
/*						LD_SER_RSB_BEG ,  */
/*						LC_EFT_RDC ,  */
/*						LD_LTS_STS_BIL ,  */
/*						IF_TIR_PCE ,  */
/*						LF_RGL_CAT_LP20 ,  */
/*						LN_RDC_PGM_PAY_PCV ,  */
/*						LC_REA_PIF_PCV ,  */
/*						LD_FAT_PIF_TOL_PCV ,  */
/*						LA_FAT_PIF_TOL_PCV ,  */
/*						LI_RTE_RDC_ELG ,  */
/*						LD_LTE_FEE_ELG ,  */
/*						LA_LTE_FEE_OTS ,  */
/*						LD_LON_LTE_FEE_WAV ,  */
/*						LC_CUR_RDC_PGM_NME ,  */
/*						LI_RIR_SCY_ELG ,  */
/*						LD_END_RIR_DSQ_OVR ,  */
/*						LF_LON_ALT ,  */
/*						LN_LON_ALT_SEQ ,  */
/*						CASE WHEN LI_LDR_LST_RST_DSB = 'Y' THEN 1 ELSE 0 END AS LI_LDR_LST_RST_DSB,  */
/*						LC_ST_BR_RSD_APL ,  */
/*						LI_PIF_RPT_REQ ,  */
/*						LD_AMR_BEG ,  */
/*						LD_ORG_XPC_GRD ,  */
/*						LD_CON_LST_RPY_BEG ,  */
/*						LN_CON_MTH_DFR_FOR ,  */
/*						LD_LON_APL_RCV ,  */
/*						LR_SCL_SUB ,  */
/*						LI_BLL_PAY_SES_OVR ,  */
/*						LC_MPN_TYP ,  */
/*						LD_MPN_EXP ,  */
/*						LC_MPN_SRL_LON ,  */
/*						LC_MPN_REV_REA ,  */
/*						LF_ORG_RGN ,  */
/*						LC_CAM_LON_STA ,  */
/*						LD_DFR_FOR_END ,  */
/*						LC_DFR_FOR_TYP ,  */
/*						LF_CAM_DFR_SCL_ENR ,  */
/*						LD_DFR_FOR_BEG ,  */
/*						LD_CAM_DFR_INF_CER ,  */
/*						LI_BR_DET_RPD_XTN ,  */
/*						DD_DTH_VER ,  */
/*						DD_DSA_VER ,  */
/*						LI_CON_PAY_STP_PUR ,  */
/*						LD_FSE_CER_NTF ,  */
/*						LA_TOT_EDU_DET ,  */
/*						LI_LDR_BG_APL ,  */
/*						LD_RIR_CSC_BIL_STS ,  */
/*						LI_ESG ,  */
/*						LC_RIR_DSQ_REA ,  */
/*						LF_MN_MST_NTE ,  */
/*						LN_MN_MST_NTE_SEQ ,  */
/*						LC_LON_SND_CHC ,  */
/*						LC_SST_LON10 ,  */
/*						LF_RGL_CAT_LP09 ,  */
/*						LI_MN_PSD_BS ,  */
/*						LF_CRD_RTE_SRE ,  */
/*						LF_ESG_SRC ,  */
/*						PC_PNT_YR ,  */
/*						LF_OWN_BND_ISS_TE1 ,  */
/*						LF_OWN_BND_ISS_TE2 ,  */
/*						LF_OWN_BND_ISS_TE3 ,  */
/*						LD_MPN_STM_SNT ,  */
/*						LI_MNT_BIL_RCP ,  */
/*						LX_BS_POI ,  */
/*						LA_BS_POI ,  */
/*						LA_INT_FEE_URP_IRS ,  */
/*						LC_BR_ALW_SCL_DFR ,  */
/*						LD_BR_ALW_SCL_DFR ,  */
/*						LC_LON_DFR_SUB_TYP ,  */
/*						LD_FAT_PRI_BAL_ZRO ,  */
/*						LC_ST_SCL_ATD_APL ,  */
/*						LD_EFT_DSQ_NSF_LMT ,  */
/*						LD_CLM_PD ,  */
/*						LC_STP_PUR ,  */
/*						LD_CON_PAY_EFF ,  */
/*						LD_CON_PAY_APL ,  */
/*						LC_ESG ,  */
/*						LC_LON_RPE_CVN_REA ,  */
/*						LC_UDL_DSB_COF ,  */
/*						LI_BR_LT_HT ,  */
/*						LI_ALL_PAY_FLG_SPS ,  */
/*						LN_MFY_GRS_PAY ,  */
/*						LC_ESP_RPD_OPT_SEL ,  */
/*						LC_ELG_95_SPA_BIL ,  */
/*						LC_SGM_COS_PRC ,  */
/*						LD_GTR_DR_DCH_CER ,  */
/*						LD_RPD_ELY_CL_BEG ,  */
/*						LD_RPD_ELY_CL_END ,  */
/*						LF_GTR_RFR_XTN ,  */
/*						LA_MSC_FEE_OTS ,  */
/*						LA_MSC_FEE_PCV_OTS ,  */
/*						LF_LON_GRP_WI_BR ,  */
/*						LC_TL4_IBR_ELG ,  */
/*						LD_LON_IBR_ENT ,  */
/*						LC_LON_IBR_RPY_TYP ,  */
/*						LI_BYP_COL_OUT_SRC ,  */
/*						LI_BR_GRP_RLP ,  */
/*						LI_OO_PST_ENR_DFR ,  */
/*						LD_OO_PST_ENR_DFR ,  */
/*						LF_FED_CLC_RSK ,  */
/*						LF_FED_FFY_1_DSB ,  */
/*						LF_PRV_GTR ,  */
/*						LC_FED_PGM_YR ,  */
/*						LA_INT_RCV_GOV ,  */
/*						LC_WOF_WUP_REA ,  */
/*						LC_VRS_ALT_APL ,  */
/*						LF_LON_DCV_CLI ,  */
/*						LN_LON_SEQ_DCV_CLI ,  */
/*						LI_EDS_BKR_STP_PUR ,  */
/*						LF_EDS ,  */
/*						LD_EFF_LBR_RTE ,  */
/*						LA_STD_STD_PAY ,  */
/*						LI_FRC_IBR ,  */
/*						LC_STP_PUR_REA ,  */
/*						LD_IDR_ELG_CHG ,  */
/*						LC_IDR_ELG_CRI */
/*					FROM*/
/*						OLWHRM1.LN10_LON LN10*/
/*					WHERE*/
/*						LN10.LF_LST_DTS_LN10 >= &LAST_RUNPASS*/
/*/*						and bf_ssn like '%11'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/*/*%SSN2ACC(LN10A,);*/*/
/**/
/*/**********************************************/
/** RM31A*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE RM31A AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						LD_RMT_BCH_INI ,  */
/*						LC_RMT_BCH_SRC_IPT ,  */
/*						LN_RMT_BCH_SEQ ,  */
/*						LN_RMT_SEQ_PST ,  */
/*						LN_RMT_ITM_PST ,  */
/*						LN_RMT_ITM_SEQ_PST ,  */
/*						LD_RMT_PST_PST ,  */
/*						LD_RMT_SPS_PST ,  */
/*						LD_RMT_PAY_EFF_PST ,  */
/*						LA_BR_RMT_PST ,  */
/*						LD_CRT_REMT31 ,  */
/*						LD_UPD_REMT31 ,  */
/*						LX_SPS_REA_PST ,  */
/*						LM_RMT_IST_PST ,  */
/*						LM_RMT_LST_PST ,  */
/*						LM_RMT_MID_PST ,  */
/*						LM_RMT_1_PST ,  */
/*						LF_RMT_IST_PST ,  */
/*						LF_RMT_EDS_PST ,  */
/*						LX_RMT_PST ,  */
/*						CASE WHEN LI_RMT_PD_AHD_PST = 'Y' THEN 1 ELSE 0 END AS LI_RMT_PD_AHD_PST ,  */
/*						LC_RMT_REV_REA_PST ,  */
/*						LC_RMT_STA_PST ,  */
/*						LD_RMT_BCH_INI_PVS ,  */
/*						LC_RMT_BCH_IPT_PVS ,  */
/*						LN_RMT_BCH_SEQ_PVS ,  */
/*						LN_RMT_SEQ_PVS ,  */
/*						LN_RMT_ITM_PVS ,  */
/*						LN_RMT_ITM_SEQ_PVS ,  */
/*						LF_LST_DTS_RM31 ,  */
/*						BF_SSN ,  */
/*						PC_FAT_TYP ,  */
/*						PC_FAT_SUB_TYP ,  */
/*						LD_BIL_CRT ,  */
/*						LN_SEQ_BIL_WI_DTE ,  */
/*						LC_RMT_BCH ,  */
/*						LI_DPS_SPS_PIO_CVN ,  */
/*						BN_CPN_BK_SEQ ,  */
/*						LD_BIL_CPN_DU_PST ,  */
/*						CASE WHEN LI_PHD_PAS_DU_PST = 'Y' THEN 1 ELSE 0 END AS LI_PHD_PAS_DU_PST ,  */
/*						CASE WHEN LI_PHD_PAS_RPS_PST = 'Y' THEN 1 ELSE 0 END AS LI_PHD_PAS_RPS_PST ,  */
/*						CASE WHEN LI_PHD_PAS_OPT_PST = 'Y' THEN 1 ELSE 0 END AS LI_PHD_PAS_OPT_PST ,  */
/*						LF_USR_UPD_SPS ,  */
/*						LC_RMT_REV_REJ_PST ,  */
/*						LF_RMT_ACC_ID_PST ,  */
/*						LI_DPL_ACC_RMT_PST ,  */
/*						LC_STP_SPS_OVR_PST ,  */
/*						CASE WHEN LC_SPS_OVR_FRD_PST = 'Y' THEN 1 ELSE 0 END AS LC_SPS_OVR_FRD_PST ,  */
/*						CASE WHEN LC_WAV_MSC_FEE_PST = 'Y' THEN 1 ELSE 0 END AS LC_WAV_MSC_FEE_PST ,  */
/*						LD_SCH_RMT_PAY_PST ,  */
/*						LF_LCK_BOX_TRC_PST ,  */
/*						LF_RMT_PST_SCH_NUM ,  */
/*						LC_RMT_PST_SCH_TYP ,  */
/*						LI_CON_SPS_OVR ,  */
/*						LD_RMT_PST_SCH_DPS ,  */
/*						LF_RMT_PST_SCH_IVC ,  */
/*						LF_RMT_PST_OBD_NUM ,  */
/*						LC_RMT_PST_OBD_TYP ,  */
/*						LD_RMT_PST_OBD_DPS ,  */
/*						LI_AUT_SUS_REV ,  */
/*						LC_PST_RMT_ESH */
/**/
/*					FROM*/
/*						OLWHRM1.RM31_BR_RMT_PST RM31*/
/*/*						and bf_ssn like '%11'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/*/*%SSN2ACC(RM31,);*/*/
/**/
/*/**********************************************/
/** LN90A*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE LN90A AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						**/
/*					FROM*/
/*						OLWHRM1.LN90_FIN_ATY LN90*/
/*					WHERE*/
/*						LN90.LF_LST_DTS_LN90 >= &LAST_RUNPASS*/
/*/*						and bf_ssn like '%11'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/*/*%SSN2ACC(LN90,);*/*/
/**/
/*/**********************************************/
/** LN33*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE LN33 AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						**/
/*					FROM*/
/*						OLWHRM1.LN33_LON_CU_INF LN33*/
/*					WHERE*/
/*						LN33.LF_LST_DTS_LN33 >= &LAST_RUNPASS*/
/*/*						and bf_ssn like '%11'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/*/*%SSN2ACC(LN33,);*/*/
/**/
/*/**********************************************/
/** GU10*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE GU10 AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						IF_GTR ,*/
/*						IF_ACC_EFT_GTR ,*/
/*						IF_ABA_EFT_GTR ,*/
/*						IM_GTR_SHO ,*/
/*						IM_GTR_FUL ,*/
/*						CASE WHEN II_RPT_GTR_SCL = 'Y' THEN 1 ELSE 0 END AS II_RPT_GTR_SCL,*/
/*						CASE WHEN II_RPT_GTR_LDR = 'Y' THEN 1 ELSE 0 END AS II_RPT_GTR_LDR,*/
/*						CASE WHEN II_ASN_RFR_NUM = 'Y' THEN 1 ELSE 0 END AS II_ASN_RFR_NUM,*/
/*						IC_RPT_DSB_TYP ,*/
/*						II_GTR_NAT_PTC ,*/
/*						IF_LST_USR_GU10 ,*/
/*						IF_LST_DTS_GU10 ,*/
/*						IF_GTR_PRN ,*/
/*						IC_GTR_TYP ,*/
/*						II_GTR_PRE_DSB_M ,*/
/*						II_GTR_NOG_REQ ,*/
/*						II_GTR_CNL_PTC ,*/
/*						II_GTR_MNF_DSB_RSN ,*/
/*						IC_GTR_FEE_RPT_MTH ,*/
/*						II_RAL_SCY_OVR ,*/
/*						II_GTR_ALW_PSB_MPN ,*/
/*						II_GTR_CHS_PTC ,*/
/*						II_GTR_VER_FEE_RQR ,*/
/*						II_GTR_WHL_DOL_DSB */
/*					FROM*/
/*						OLWHRM1.GU10_GTR GU10*/
/*					WHERE*/
/*						GU10.IF_LST_DTS_GU10 >= &LAST_RUNPASS*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/*/*%SSN2ACC(GU10,);*/*/
/**/
/*/**********************************************/
/** RF10*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE RF10 AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						RF10.**/
/*					FROM*/
/*						OLWHRM1.RF10_RFR RF10*/
/*					WHERE*/
/*						RF10.BF_LST_DTS_RF10 >= &LAST_RUNPASS*/
/*/*						and bf_ssn like '%11'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/*/*%SSN2ACC(RF10,);*/*/
/**/
/*/**********************************************/
/** LN35*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE LN35 AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						LN35.**/
/*					FROM*/
/*						OLWHRM1.LN35_LON_OWN LN35*/
/*					WHERE*/
/*						LN35.LF_LST_DTS_LN35 >= &LAST_RUNPASS*/
/*/*						and bf_ssn like '%11'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
/*/*%SSN2ACC(LN35,);*/*/
/**/
/*/**********************************************/
/** LN60*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DB2 (DATABASE=&DB);*/
/**/
/*	CREATE TABLE LN60 AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DB2 */
/*				(*/
/*					SELECT*/
/*						LN60.**/
/*					FROM*/
/*						OLWHRM1.LN60_BR_FOR_APV LN60*/
/*					WHERE*/
/*						LN60.LF_LST_DTS_LN60 >= &LAST_RUNPASS*/
/*/*						and bf_ssn like '%11'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DB2;*/
/*QUIT;*/
;
/*********************************************
* LN54
**********************************************/
PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE LN54 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						LN54.BF_SSN,
						LN54.LN_SEQ, 
						LN54.PM_BBS_PGM,
						LN54.PN_BBS_PGM_SEQ,
						LN54.LN_LON_BBS_PGM_SEQ,
						LN54.LD_EFF_BEG_LN54,
						CASE WHEN LN54.LI_BBS_ITD_LTR_SNT = 'Y' THEN 1 ELSE 0 END AS LI_BBS_ITD_LTR_SNT,
						LN54.LN_BBS_STS_PCV_PAY,
						LN54.LC_BBS_REB_MTD,
						LN54.LC_STA_LN54,
						LN54.LD_STA_LN54,
						CASE WHEN LN54.LC_BBT_TYS_ASS = 'Y' THEN 1 ELSE 0 END AS LC_BBT_TYS_ASS,
						LN54.LC_BBS_DSQ_REA,
						LN54.LD_BBS_DSQ,
						LN54.LC_BBS_ELG,
						LN54.LC_BBT_PRC_RBD,
						LN54.LD_BBS_RPD_WDO_END,
						LN54.LC_BBS_BCH_PRC,
						LN54.LF_LST_USR_LN54, 
						LN54.LF_LST_DTS_LN54,
						LN54.LN_BBS_PCV_PAY_MOT,
						LN54.LD_BBS_ICV_REQ,
						LN54.LD_BBS_DSQ_APL,
						LN54.LI_BBS_PCV_LTE_PAY
					FROM
						OLWHRM1.LN54_LON_BBS LN54
					WHERE
						LN54.LF_LST_DTS_LN54 >= &LAST_RUNPASS
/*						and bf_ssn like '%11'*/

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;
QUIT;
/*%SSN2ACC(LN54,);*/

/*********************************************
* LN55
**********************************************/
PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE LN55 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						LN55.*
					FROM
						OLWHRM1.LN55_LON_BBS_TIR LN55
					WHERE
						LN55.LF_LST_DTS_LN55 >= &LAST_RUNPASS
/*						and bf_ssn like '%11'*/

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;
QUIT;
/*%SSN2ACC(LN55,);*/

/*********************************************
* RS05
**********************************************/
PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE RS05 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						RS05.*
					FROM
						OLWHRM1.RS05_IBR_RPS RS05
					WHERE
						RS05.BF_LST_DTS_RS05 >= &LAST_RUNPASS
/*						and bf_ssn like '%11'*/

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;
QUIT;
/*%SSN2ACC(RS05,);*/

/*********************************************
* RS10
**********************************************/
PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE RS10 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						RS10.*
					FROM
						OLWHRM1.RS10_BR_RPD RS10
					WHERE
						RS10.LF_LST_DTS_RS10 >= &LAST_RUNPASS
/*						and bf_ssn like '%11'*/

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;
QUIT;
/*%SSN2ACC(RS10,);*/

/*********************************************
* LN65A
**********************************************/
PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE LN65A AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						LN65.*
					FROM
						OLWHRM1.LN65_LON_RPS LN65
					WHERE
						LN65.LF_LST_DTS_LN65 >= &LAST_RUNPASS
/*						and bf_ssn like '%11'*/

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;
QUIT;
/*%SSN2ACC(LN65A,);*/

/*********************************************
* LN66
**********************************************/
PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE LN66 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						LN66.*
					FROM
						OLWHRM1.LN66_LON_RPS_SPF LN66
					WHERE
						LN66.LF_LST_DTS_LN66 >= &LAST_RUNPASS
/*						and bf_ssn like '%11'*/

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;
QUIT;
/*%SSN2ACC(LN66,);*/

/*********************************************
* PD10A
**********************************************/
PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE PD10A AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						PD10.DF_PRS_ID ,
						PD10.DD_STA_PRS ,
						PD10.DC_LAG_FGN ,
						PD10.DC_SEX ,
						PD10.DD_BRT ,
						PD10.DM_PRS_MID ,
						PD10.DM_PRS_1 ,
						PD10.DM_PRS_LST_SFX ,
						PD10.DM_PRS_LST ,
						PD10.DD_DRV_LIC_REN ,
						PD10.DC_ST_DRV_LIC ,
						PD10.DF_DRV_LIC ,
						PD10.DD_NME_VER_LST ,
						CASE WHEN PD10.DI_ORG_HLD = 'Y' THEN 1 ELSE 0 END AS DI_ORG_HLD,
						PD10.DF_LST_USR_PD10 ,
						PD10.DF_ALN_RGS ,
						PD10.DI_US_CTZ ,
						PD10.DF_LST_DTS_PD10 ,
						PD10.DF_SPE_ACC_ID ,
						PD10.DF_PRS_LST_4_SSN ,
						PD10.DI_ATU_FMT ,
						PD10.DC_ATU_FMT_TYP
					FROM
						OLWHRM1.PD10_PRS_NME PD10
					WHERE
						PD10.DF_LST_DTS_PD10 >= &LAST_RUNPASS
/*						and bf_ssn like '%11'*/

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;
QUIT;
/*%SSN2ACC(PD10A,);*/

/*********************************************
* RP30
**********************************************/
PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE RP30 AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						RP30.IF_OWN ,
						RP30.PN_EFT_RIR_OWN_SEQ ,
						RP30.IC_LON_PGM ,
						RP30.IF_GTR ,
						RP30.PD_LON_1_DSB ,
						RP30.PF_DOE_SCL_ORG ,
						RP30.PC_ST_BR_RSD_APL ,
						RP30.PD_EFT_RIR_EFF_BEG ,
						RP30.PD_EFT_RIR_EFF_END ,
						RP30.PC_EFT_RIR_STA ,
						RP30.PD_EFT_RIR_STA ,
						RP30.PI_EFT_RIR_PRC ,
						RP30.PC_EFT_NSF_LTR_REQ ,
						RP30.PR_EFT_RIR ,
						RP30.PF_LST_USR_RP30 ,
						RP30.PF_LST_DTS_RP30 ,
						RP30.PC_EFT_RIR_PNT_YR ,
						RP30.PD_EFT_BBS_LOT_BEG ,
						RP30.PD_EFT_BBS_GTE_DTE ,
						RP30.PD_EFT_BBS_RPD_SR ,
						RP30.PD_EFT_BBS_LCO_RCV ,
						RP30.PN_EFT_BBS_NSF_LMT ,
						RP30.PC_EFT_BBS_NSF_PRC ,
						RP30.PN_EFT_BBS_NSF_MTH ,
						RP30.PC_EFT_BBS_FED ,
						RP30.PI_EFT_RIR_RPY_0 
					FROM
						OLWHRM1.RP30_EFT_RIR_PAR RP30
					WHERE
						RP30.PF_LST_DTS_RP30 >= &LAST_RUNPASS
/*						and bf_ssn like '%11'*/

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;
QUIT;
/*%SSN2ACC(RP30,);*/


/*uncomment to update data set in prod*/
/*DATA SAS_TAB.LASTRUN_JOBS;*/
/*SET SAS_TAB.LASTRUN_JOBS;*/
/*IF JOB = 'UTLWDW1' THEN LAST_RUN = TODAY();*/
/*RUN;*/;
ENDRSUBMIT;
data _null_;
call symput('endrsub',put(time(),time9.));
run;

LIBNAME DUSTER 'T:\';

/*DATA LN20; SET DUSTER.LN20; RUN; *2;*/
/*DATA BR03; SET DUSTER.BR03; RUN; *3;*/
/*DATA LN80; SET DUSTER.LN80; RUN; *4;*/
/*DATA LN90; SET DUSTER.LN90; RUN; *5;*/
/*DATA DF10; SET DUSTER.DF10; RUN; *6;*/
/*DATA FB10; SET DUSTER.FB10; RUN; *7;*/
/*DATA PD10; SET DUSTER.PD10; RUN; *8;*/
/*DATA K0PHN; SET DUSTER.K0PHN; RUN; *9;  */
/*DATA DRLFA; SET DUSTER.DRLFA; RUN; *10;*/
/*DATA LN10; SET DUSTER.LN10; RUN; *11;*/
/*DATA LN83; SET DUSTER.LN83; RUN; *12;*/
/*DATA LN09; SET DUSTER.LN09; RUN; *13;*/
/*DATA CALLFWD; SET DUSTER.CALLFWD; RUN; *14;*/
/*DATA LN54; SET DUSTER.LN54; RUN; *15;*/
/*DATA LN55; SET DUSTER.LN55; RUN; *16;*/
/*DATA LN65; SET DUSTER.LN65; RUN; *17;*/
/*DATA LN72; SET DUSTER.LN72; RUN; *18;*/
/*DATA SD10; SET DUSTER.SD10; RUN; *19;*/
/*DATA LN16; SET DUSTER.LN16; RUN; *20;*/
/*DATA BR02; SET DUSTER.BR02; RUN; *21;*/
/*DATA BR30; SET DUSTER.BR30; RUN; *22;*/
/*DATA PD32; SET DUSTER.PD32; RUN; *23;*/
/*DATA RM31; SET DUSTER.RM31; RUN; *24;*/
/*DATA PD30; SET DUSTER.PD30; RUN; *25;*/
/*DATA PD42; SET DUSTER.PD42; RUN; *26;*/
/*DATA K0ADD; SET DUSTER.K0ADD; RUN; *27;*/
/*DATA ARCHIST; SET DUSTER.ARCHIST; RUN; *28;*/
/*DATA DW01; SET DUSTER.DW01; RUN; *29;*/
/*DATA LN56; SET DUSTER.LN56; RUN; *30;*/
/*DATA CUR_DUE; SET DUSTER.CUR_DUE; RUN; *31;*/
/*DATA DL200; SET DUSTER.DL200; RUN; *32; */
/*DATA ARC_M1411; SET DUSTER.ARC_M1411; RUN; *33;*/
/*DATA ARC_IND; SET DUSTER.ARC_IND; RUN; *34; */
/*DATA LN15_DISB; SET DUSTER.LN15_DISB; RUN; *35; */
/*DATA AD20; SET DUSTER.AD20; RUN; *36;*/
/*DATA AP03; SET DUSTER.AP03; RUN; *37;*/
/*DATA LN40; SET DUSTER.LN40; RUN; *38;*/
/*DATA CL10; SET DUSTER.CL10; RUN; *39;*/
/*DATA LN80A; SET DUSTER.LN80A; RUN; *40;*/
/*DATA SC10; SET DUSTER.SC10; RUN; *41;*/
/*DATA PD20; SET DUSTER.PD20;	RUN; *42;*/
/*DATA PD21; SET DUSTER.PD21; RUN; *43;*/
/*DATA PD22; SET DUSTER.PD22; RUN; *44;*/
/*DATA PD23; SET DUSTER.PD23; RUN; *45;*/
/*DATA PD24; SET DUSTER.PD24; RUN; *46;*/
/*DATA RS05; SET DUSTER.RS05;	RUN; *47;*/
/*DATA MR01; SET DUSTER.MR01; RUN; *48;*/
/*DATA LN10A; SET DUSTER.LN10A; RUN; *49;*/
/*DATA RM31A; SET DUSTER.RM31A; RUN; *50;*/
/*DATA LN90A; SET DUSTER.LN90A; RUN; *51;*/
/*DATA LN33; SET DUSTER.LN33; RUN; *52;*/
/*DATA GU10; SET DUSTER.GU10; RUN; *53;*/
/*DATA RF10; SET DUSTER.RF10; RUN; *54;*/
/*DATA LN35; SET DUSTER.LN35; RUN; *55;*/
/*DATA LN60; SET DUSTER.LN60; RUN; *56;*/
DATA LN54; SET DUSTER.LN54; RUN; *57;
DATA LN55; SET DUSTER.LN55; RUN; *58;
DATA RS10; SET DUSTER.RS10; RUN; *59;
DATA LN65A; SET DUSTER.LN65A; RUN; *60;
DATA LN66; SET DUSTER.LN66; RUN; *61;
DATA PD10A; SET DUSTER.PD10A; RUN; *62;
DATA RP30; SET DUSTER.RP30; RUN; *63;

data _null_;
call symput('FILETRAN',put(time(),time9.));
run;
/*DATA _NULL_;*/
/*SET LN20 end = eof;*/
/*FILE REPORT2 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_SEQ @;*/
/*	PUT LC_STA_LON20 $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET BR03 end = eof;*/
/*FILE REPORT3 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT LST_CNC LST_ATT MMDDYY10.;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT DF_PRS_ID_RFR  @;*/
/*	PUT BC_STA_BR03 @;*/
/*	PUT BI_ATH_3_PTY @;*/
/*	PUT BC_RFR_REL_BR @;*/
/*	PUT BM_RFR_1 @;*/
/*	PUT BM_RFR_LST @;*/
/*	PUT LST_CNC @;*/
/*	PUT LST_ATT $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET LN80 end = eof;*/
/*FILE REPORT4 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT LD_BIL_CRT LD_FAT_EFF LD_BIL_DU_LON MMDDYY10. ;*/
/*FORMAT 	LA_BIL_CUR_DU LA_BIL_PAS_DU	LA_TOT_BIL_STS 9.2;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_SEQ @;*/
/*	PUT LD_BIL_CRT @;*/
/*	PUT LN_SEQ_BIL_WI_DTE @;*/
/*	PUT LD_FAT_EFF @;*/
/*	PUT LD_BIL_DU_LON @;*/
/*	PUT LC_STA_LON80 @;*/
/*	PUT LA_BIL_CUR_DU @;*/
/*	PUT LA_BIL_PAS_DU @;*/
/*	PUT LC_BIL_MTD @;*/
/*	PUT LC_IND_BIL_SNT @;*/
/*	PUT LC_STA_BIL10 @;*/
/*	PUT LA_TOT_BIL_STS $;*/
/*END;*/
/*if eof then put "-End-";*/
/*RUN; */
/**/
/*DATA _NULL_;*/
/*SET LN90 end = eof;*/
/*FILE REPORT5 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT LD_FAT_PST LD_FAT_EFF MMDDYY10. ;*/
/*FORMAT LA_FAT_CUR_PRI LA_FAT_LTE_FEE 10.2 ;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_SEQ @;*/
/*	PUT LN_FAT_SEQ @;*/
/*	PUT LD_FAT_PST @;*/
/*	PUT LD_FAT_EFF  @;*/
/*	PUT LC_STA_LON90 @;*/
/*	PUT LA_FAT_CUR_PRI @;*/
/*	PUT LA_FAT_LTE_FEE @;*/
/*	PUT PC_FAT_TYP @;*/
/*	PUT PC_FAT_SUB_TYP @;*/
/*	PUT LA_FAT_NSI @;*/
/*	PUT LC_FAT_REV_REA $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET DF10 end = eof;*/
/*FILE REPORT6 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT LD_DFR_INF_CER LD_DFR_BEG LD_DFR_END MMDDYY10. ;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_SEQ @;*/
/*	PUT LF_DFR_CTL_NUM @;*/
/*	PUT LN_DFR_OCC_SEQ @;*/
/*	PUT LC_DFR_TYP @;*/
/*	PUT LD_DFR_INF_CER @;*/
/*	PUT LD_DFR_BEG @;*/
/*	PUT LD_DFR_END @;*/
/*	PUT LC_LON_LEV_DFR_CAP @;*/
/*	PUT LC_STA_LON50 @;*/
/*	PUT LC_DFR_STA @;*/
/*	PUT LC_STA_DFR10 $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET FB10 end = eof;*/
/*FILE REPORT7 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT LD_FOR_INF_CER LD_FOR_BEG LD_FOR_END MMDDYY10. LA_REQ_RDC_PAY 9.2;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_SEQ @;*/
/*	PUT LF_FOR_CTL_NUM @;*/
/*	PUT LN_FOR_OCC_SEQ @;*/
/*	PUT LC_FOR_TYP @;*/
/*	PUT LD_FOR_INF_CER @;*/
/*	PUT LD_FOR_BEG @;*/
/*	PUT LD_FOR_END @;*/
/*	PUT LC_LON_LEV_FOR_CAP @;*/
/*	PUT LC_STA_LON60 @;*/
/*	PUT LC_FOR_STA @;*/
/*	PUT LC_STA_FOR10 @;*/
/*	PUT LA_REQ_RDC_PAY $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET PD10 end = eof;*/
/*FILE REPORT8 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT DD_BRT MMDDYY10.;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT BF_SSN @;*/
/*	PUT DM_PRS_1 @;*/
/*	PUT DM_PRS_LST @;*/
/*	PUT DM_PRS_MID @;*/
/*	PUT DD_BRT $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET K0PHN end = eof;*/
/*FILE REPORT9 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;	*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_ATY_SEQ @;*/
/*	PUT LC_STA_ACTY10 @;*/
/*	PUT PHN1 @;*/
/*	PUT PHN2 @;*/
/*	PUT PHN3 @;*/
/*	PUT COMMENTS $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET DRLFA end = eof;*/
/*FILE REPORT10 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT FEE_WAV_DOL @;*/
/*	PUT FEE_WAV_CT $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET LN10 end = eof;*/
/*FILE REPORT11 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT LD_END_GRC_PRD LD_LON_1_DSB  LD_PIF_RPT MMDDYY10. ;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_SEQ  @;*/
/*	PUT LC_STA_LON10 @;*/
/*	PUT LA_CUR_PRI  @;*/
/*	PUT LA_LON_AMT_GTR @;*/
/*	PUT LD_END_GRC_PRD @;*/
/*	PUT IC_LON_PGM @;*/
/*	PUT LD_LON_1_DSB @;*/
/*	PUT LD_PIF_RPT @;*/
/*	PUT IF_GTR $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET LN83 end = eof;*/
/*FILE REPORT12 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_SEQ  @;*/
/*	PUT BN_EFT_SEQ @;*/
/*	PUT LC_STA_LN83 $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET LN09 end = eof;*/
/*FILE REPORT13 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT LD_LON_RHB_PCV MMDDYY10. ;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_SEQ @;*/
/*	PUT LD_LON_RHB_PCV $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET CALLFWD end = eof;*/
/*FILE REPORT14 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT BF_SSN @;*/
/*	PUT CLUID @;*/
/*	PUT LN_SEQ @;*/
/*	PUT FORWARDING $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET LN54 end = eof;*/
/*FILE REPORT15 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT LD_BBS_DSQ MMDDYY10. ;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_SEQ  @;*/
/*	PUT LD_BBS_DSQ  @;*/
/*	PUT LC_BBS_ELG $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET LN55 end = eof;*/
/*FILE REPORT16 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_SEQ  @;*/
/*	PUT LN_LON_BBT_PAY @;*/
/*	PUT PN_BBT_PAY_ICV $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET LN65 end = eof;*/
/*FILE REPORT17 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT LD_CRT_LON65 LD_SNT_RPD_DIS MMDDYY10. ;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_SEQ  @;*/
/*	PUT LD_CRT_LON65 @;	*/
/*	PUT LC_TYP_SCH_DIS @; */
/*	PUT LD_SNT_RPD_DIS @;*/
/*	PUT LA_RPS_ISL @;*/
/*	PUT DAY_DUE @;*/
/*	PUT LN_RPS_TRM $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET LN72 end = eof;*/
/*FILE REPORT18 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_SEQ  @;*/
/*	PUT LR_ITR @;*/
/*	PUT LR_INT_RDC_PGM_ORG $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET SD10 end = eof;*/
/*FILE REPORT19 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT LD_SCL_SPR MMDDYY10.;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_SEQ  @;*/
/*	PUT LD_SCL_SPR @;*/
/*	PUT IM_SCL_FUL $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET LN16 end = eof;*/
/*FILE REPORT20 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT LD_DLQ_OCC LD_DLQ_MAX MMDDYY10.;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_SEQ @;*/
/*	PUT LD_DLQ_OCC @;*/
/*	PUT LN_DLQ_MAX @;*/
/*	PUT LD_DLQ_MAX $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET BR02 end = eof;*/
/*FILE REPORT21 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT IM_IST_FUL @;*/
/*	PUT IX_GEN_STR_ADR_1 @;*/
/*	PUT IX_GEN_STR_ADR_2 @;*/
/*	PUT IM_GEN_CT @;*/
/*	PUT IC_GEN_ST @;*/
/*	PUT IF_GEN_ZIP @;*/
/*	PUT IN_GEN_PHN $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET BR30 end = eof;*/
/*FILE REPORT22 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT BD_EFT_STA MMDDYY10.;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT BN_EFT_SEQ @;*/
/*	PUT BF_EFT_ABA @;*/
/*	PUT BF_EFT_ACC @;*/
/*	PUT BC_EFT_STA @;*/
/*	PUT BD_EFT_STA @;*/
/*	PUT BA_EFT_ADD_WDR @;*/
/*	PUT BN_EFT_NSF_CTR @;*/
/*	PUT BC_EFT_DNL_REA $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET PD32 end = eof;*/
/*FILE REPORT23 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT DD_VER_ADR_EML MMDDYY10.;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT DC_ADR_EML @;*/
/*	PUT DX_ADR_EML @;*/
/*	PUT DD_VER_ADR_EML @;*/
/*	PUT DI_VLD_ADR_EML $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET RM31 end = eof;*/
/*FILE REPORT24 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT LA_BR_RMT_PST 10.2 ;*/
/**/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LA_BR_RMT_PST $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET PD30 end = eof;*/
/*FILE REPORT25 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT DD_VER_ADR MMDDYY10.;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT DX_STR_ADR_1 @;*/
/*	PUT DX_STR_ADR_2 @;*/
/*	PUT DM_CT @;*/
/*	PUT DC_DOM_ST @;*/
/*	PUT DF_ZIP_CDE @;*/
/*	PUT DM_FGN_ST @;*/
/*	PUT DM_FGN_CNY @;*/
/*	PUT DD_VER_ADR @;*/
/*	PUT DI_VLD_ADR $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET PD42 end = eof;*/
/*FILE REPORT26 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT DD_PHN_VER MMDDYY10.;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT DC_PHN @;*/
/*	PUT DC_ALW_ADL_PHN @;*/
/*	PUT DD_PHN_VER @;*/
/*	PUT DI_PHN_VLD @;*/
/*	PUT DN_DOM_PHN_ARA @;*/
/*	PUT DN_DOM_PHN_XCH @;*/
/*	PUT DN_DOM_PHN_LCL @;*/
/*	PUT DN_PHN_XTN @;*/
/*	PUT DN_FGN_PHN_CNY @;*/
/*	PUT DN_FGN_PHN_CT @;*/
/*	PUT DN_FGN_PHN_LCL $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET K0ADD end = eof;*/
/*FILE REPORT27 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_ATY_SEQ  @;*/
/*	PUT LC_STA_ACTY10 @;*/
/*	PUT DX_STR_ADR_1 @;*/
/*	PUT DX_STR_ADR_2 @;*/
/*	PUT DM_CT @;*/
/*	PUT DC_DOM_ST @;*/
/*	PUT DF_ZIP_CDE @;*/
/*	PUT DM_FGN_CNY @;*/
/*	PUT COMMENTS $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET ARCHIST end = eof;*/
/*FILE REPORT28 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT LD_ATY_REQ_RCV LD_ATY_RSP MMDDYY10.;*/
/*FORMAT LT_ATY_RSP TIME8.;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_ATY_SEQ @;*/
/*	PUT LC_STA_ACTY10 @;*/
/*	PUT PF_REQ_ACT @;*/
/*	PUT PF_RSP_ACT @;*/
/*	PUT PX_ACT_DSC_REQ @;*/
/*	PUT LD_ATY_REQ_RCV @;*/
/*	PUT LD_ATY_RSP @;*/
/*	PUT LF_USR_REQ_ATY @;*/
/*	PUT LT_ATY_RSP @;*/
/*	PUT LX_ATY $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET DW01 end = eof;*/
/*FILE REPORT29 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT WA_TOT_BRI_OTS 9.2 WD_LON_RPD_SR mmddyy10.;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_SEQ  @;*/
/*	PUT WC_DW_LON_STA  @;*/
/*	PUT WA_TOT_BRI_OTS @;*/
/*	PUT WD_LON_RPD_SR @;*/
/*	PUT WX_OVR_DW_LON_STA $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET LN56 end = eof;*/
/*FILE REPORT30 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT DT_ADJ LD_RPT_CRB MMDDYY10. ;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_SEQ  @;*/
/*	PUT LC_RPT_STA_CRB  @;*/
/*	PUT LD_RPT_CRB @;*/
/*	PUT DT_ADJ $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET CUR_DUE end = eof;*/
/*FILE REPORT31 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT cur_due past_due tot_due tot_due_fee 9.2 ;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT cur_due @;*/
/*	PUT past_due @;*/
/*	PUT tot_due @;*/
/*	PUT tot_due_fee $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET DL200 end = eof;*/
/*FILE REPORT32 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*FORMAT LD_ATY_REQ_RCV MMDDYY10.;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;	*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_ATY_SEQ @;*/
/*	PUT LC_STA_ACTY10 @;*/
/*	PUT LD_ATY_REQ_RCV $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET ARC_M1411 end = eof;*/
/*FILE REPORT33 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;	*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_ATY_SEQ @;*/
/*	PUT LC_STA_ACTY10 @;*/
/*	PUT LX_ATY $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*SET ARC_IND end = eof;*/
/*FILE REPORT34 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*IF _N_ = 1 THEN put "-Begin-";*/
/*DO;	*/
/*	PUT DF_SPE_ACC_ID @;*/
/*	PUT LN_ATY_SEQ @;*/
/*	PUT PF_REQ_ACT @;*/
/*	PUT LC_STA_ACTY10 $;*/
/*end;*/
/*if eof then put "-End-";*/
/*RUN;*/
/*%PUT &rsub &ENDRSUB &FILETRAN ;*/
/**/
/*DATA _NULL_;*/
/*	SET LN15_DISB END = EOF;*/
/*	FILE REPORT35 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/*	FORMAT LD_DSB MMDDYY10.;*/
/*	FORMAT LA_DSB LA_DL_REBATE 9.2;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_BR_DSB_SEQ @;*/
/*		PUT LA_DSB @;*/
/*		PUT LD_DSB @;*/
/*		PUT LC_DSB_TYP @;*/
/*		PUT LC_STA_LON15 @;*/
/*		PUT LN_SEQ @;*/
/*		PUT LA_DL_REBATE $;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/**/
/*DATA _NULL_;*/
/*	SET AD20 END = EOF;*/
/*	FILE REPORT36 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LD_FAT_ADJ_REQ @;*/
/*		PUT LN_SEQ_FAT_ADJ_REQ @;*/
/*		PUT LC_TYP_FAT_ADJ_REQ @;*/
/*		PUT LC_STA_FAT_ADJ_REQ $;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/**/
/*DATA _NULL_;*/
/*	SET AP03 END = EOF;*/
/*	FILE REPORT37 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT AF_APL_ID @;*/
/*		PUT BF_SSN @;*/
/*		PUT AN_SEQ @;*/
/*		PUT LF_STU_SSN @;*/
/*		PUT IC_LON_PGM @;*/
/*		PUT AD_CRT_APL @;*/
/*		PUT AC_STA_ORG_PRC @;*/
/*		PUT AD_STA_ORG_PRC @;*/
/*		PUT AC_APL_SCR_STA @;*/
/*		PUT AC_APL_PRC_TYP @;*/
/*		PUT AI_BR_DLQ_FED_DET @;*/
/*		PUT AI_BR_DET @;*/
/*		PUT AC_BR_INT_OPT @;*/
/*		PUT AC_BR_RPD_OPT @;*/
/*		PUT AI_BR_NED_ORG_FEE @;*/
/*		PUT AI_BR_ATH_DSB_EFT @;*/
/*		PUT AI_EDS_XST_ON_APL @;*/
/*		PUT AI_RFR_XST_ON_APL @;*/
/*		PUT AD_STU_LON_TRM_BEG @;*/
/*		PUT AD_STU_LON_TRM_END @;*/
/*		PUT AD_STU_GRD_XPC @;*/
/*		PUT AI_STU_FGN_CNY_STY @;*/
/*		PUT AC_STU_MAJ_STY @;*/
/*		PUT AX_STU_CNL_MAJ_STY @;*/
/*		PUT AC_STU_SCL_ENR_STA @;*/
/*		PUT AI_STU_DFR_REQ @;*/
/*		PUT AA_STU_REQ @;*/
/*		PUT AF_DOE_SCL @;*/
/*		PUT AF_CNL_NON_EDU_BRH @;*/
/*		PUT AD_SCL_LON_TRM_BEG @;*/
/*		PUT AD_SCL_LON_TRM_END @;*/
/*		PUT AD_SCL_ACA_TRM_BEG @;*/
/*		PUT AD_SCL_ACA_TRM_END @;*/
/*		PUT AD_SCL_GRD_XPC @;*/
/*		PUT AC_SCL_STA_STU_ENR @;*/
/*		PUT AC_SCL_ACA_GDE_LEV @;*/
/*		PUT AC_SCL_ACA_PGS @;*/
/*		PUT AA_SCL_EDU_CST @;*/
/*		PUT AA_SCL_EDU_AID_TOT @;*/
/*		PUT AA_SCL_EFC @;*/
/*		PUT AA_SCL_REQ @;*/
/*		PUT AI_SCL_CNF_MPN_OVR @;*/
/*		PUT AI_SCL_STU_PAS_DU @;*/
/*		PUT AN_SCL_AMT_CLC_MLT @;*/
/*		PUT AF_DOE_LDR @;*/
/*		PUT AF_LDR_NON_EDU_BRH @;*/
/*		PUT AF_LDR_APL_FIL @;*/
/*		PUT AA_LDR_APV @;*/
/*		PUT AC_LDR_PGM_YR @;*/
/*		PUT AI_LDR_CNF_MPN_OVR @;*/
/*		PUT AF_LDR_BND_ISS @;*/
/*		PUT AF_LDR_FEE_TIR @;*/
/*		PUT AI_LDR_NO_FEE_OVR @;*/
/*		PUT AF_LDR_ITR_TIR @;*/
/*		PUT AI_LDR_NO_ITR_OVR @;*/
/*		PUT AD_LDR_APL_REJ @;*/
/*		PUT AF_LDR_ORG @;*/
/*		PUT AI_LDR_MAT_SLL_LON @;*/
/*		PUT AF_LDR_LST_RST @;*/
/*		PUT IF_GTR @;*/
/*		PUT AF_GTR_RFR @;*/
/*		PUT AA_GTR_LON @;*/
/*		PUT AD_GTR_LON_REQ @;*/
/*		PUT AD_GTR_ORG @;*/
/*		PUT AR_GTR_INT @;*/
/*		PUT AC_GTR_ITR_TYP @;*/
/*		PUT AR_GTR_FEE @;*/
/*		PUT AI_GTR_TL4_PIO_787 @;*/
/*		PUT AI_GTR_DSB_LTE_AZ @;*/
/*		PUT AD_GTR_DSB_MAX_LTE @;*/
/*		PUT AA_GTR_RSC @;*/
/*		PUT AD_GTR_RSC @;*/
/*		PUT AC_GTR_DFR_REQ @;*/
/*		PUT AC_GTR_INT_BIL_OPT @;*/
/*		PUT AC_GTR_REA_RDC @;*/
/*		PUT AA_GTR_TOT_SFD_BAL @;*/
/*		PUT AA_GTR_TOT_PLS_BAL @;*/
/*		PUT AI_GTR_RSC @;*/
/*		PUT AC_SRC_APL @;*/
/*		PUT AC_VRS_ALT_APL @;*/
/*		PUT AC_SER_RUE @;*/
/*		PUT AD_PNT_SIG @;*/
/*		PUT AI_PNT_VER @;*/
/*		PUT AF_PNT_VER_BY @;*/
/*		PUT AD_PNT_VER @;*/
/*		PUT AI_ESC @;*/
/*		PUT AC_APL_HLD @;*/
/*		PUT AD_APL_HLD @;*/
/*		PUT AA_STF_APV @;*/
/*		PUT AA_ESM_RPD @;*/
/*		PUT AD_ESM_RPD_SR @;*/
/*		PUT AD_ESM_POF @;*/
/*		PUT AC_SYS_CLC_DFR @;*/
/*		PUT AA_ANL_INC @;*/
/*		PUT AA_MTH_CLC_RPD @;*/
/*		PUT AA_MTH_CMB_DET @;*/
/*		PUT AA_MTH_CMB_INC @;*/
/*		PUT AR_CMB_DET_INC_RTO @;*/
/*		PUT AA_TOT_SFD_BAL @;*/
/*		PUT AA_TOT_PLS_BAL @;*/
/*		PUT AA_TOT_SLS_BAL @;*/
/*		PUT AA_TOT_PRK_BAL @;*/
/*		PUT AA_TOT_HEA_BAL @;*/
/*		PUT AA_OTH_EDU_LON_BAL @;*/
/*		PUT AA_TOT_STU_LN_BAL @;*/
/*		PUT AA_ORG_FEE_PD @;*/
/*		PUT AA_GTR_FEE_PD @;*/
/*		PUT AC_REA_CPY_AP03 @;*/
/*		PUT AF_APL_ID_CPY @;*/
/*		PUT AI_EXT_CVN @;*/
/*		PUT AF_APL_ID_CPY_ORG @;*/
/*		PUT AN_SEQ_CVN @;*/
/*		PUT AN_SIG_ON_PNT @;*/
/*		PUT AF_RGL_CAT_LP06 @;*/
/*		PUT AF_RGL_CAT_LP09 @;*/
/*		PUT AF_RGL_CAT_LP10 @;*/
/*		PUT AF_RGL_CAT_LP12 @;*/
/*		PUT AF_RGL_CAT_LP20 @;*/
/*		PUT AD_LON_1_DSB @;*/
/*		PUT AC_ORG_DPT @;*/
/*		PUT AR_LP_TB_ITR_MGN @;*/
/*		PUT AD_AMR_BEG @;*/
/*		PUT AR_SCL_SUB @;*/
/*		PUT AC_PNT_DEL @;*/
/*		PUT AD_PNT_SNT @;*/
/*		PUT AC_SCL_NTF_STA_CHG @;*/
/*		PUT AI_DSB_REJ_ACK_FIL @;*/
/*		PUT AC_PIO_ORG_STA @;*/
/*		PUT AC_MN_TYP @;*/
/*		PUT AC_MN_SRL_LON @;*/
/*		PUT AI_MN_PSD_BS @;*/
/*		PUT AC_MN_CNF @;*/
/*		PUT AC_MN_BR_CNF @;*/
/*		PUT AD_MN_EXP @;*/
/*		PUT AI_MN_NOG @;*/
/*		PUT AI_MN_LN_OF_CRD @;*/
/*		PUT AC_MN_SRL_LON_ORG @;*/
/*		PUT AD_MN_BR_CNF_SNT @;*/
/*		PUT AD_MN_BR_CNF_RCV @;*/
/*		PUT AA_MN_BR_CNF @;*/
/*		PUT AC_MN_REV_REA @;*/
/*		PUT AF_MN_MST_NTE @;*/
/*		PUT AN_MN_MST_NTE_SEQ @;*/
/*		PUT AI_MN_ORG_RGT_SLD @;*/
/*		PUT AF_MN_RGT_SLD_TO @;*/
/*		PUT AF_CNL @;*/
/*		PUT AF_CNL_SFX @;*/
/*		PUT AX_CNL_GTR_USE @;*/
/*		PUT AC_CNL_GTR_STA @;*/
/*		PUT AD_CNL_GTR_STA @;*/
/*		PUT AF_CNL_GTR_TRT_DTS @;*/
/*		PUT AX_CNL_LDR_USE @;*/
/*		PUT AC_CNL_LDR_SER_STA @;*/
/*		PUT AD_CNL_LDR_SER_STA @;*/
/*		PUT AF_CNL_LDR_TRT_DTS @;*/
/*		PUT AX_CNL_SCL_USE @;*/
/*		PUT AA_CNL_ACL_RTD @;*/
/*		PUT AA_CNL_WDR_GR_RFD @;*/
/*		PUT AA_CNL_AVL_RNS @;*/
/*		PUT AC_CNL_MTD_FUD_RTD @;*/
/*		PUT AI_CNL_DSB_CNS @;*/
/*		PUT AC_CNL_PRC_TYP_APL @;*/
/*		PUT AC_CNL_RPT_STA @;*/
/*		PUT AC_CNL_SRV_TYP @;*/
/*		PUT AC_CNL_ITL_RSP @;*/
/*		PUT AC_FLW_TYP @;*/
/*		PUT AC_FLW_STA @;*/
/*		PUT AD_FLW_STA @;*/
/*		PUT AD_FLW_APV @;*/
/*		PUT AF_LST_USR_AP03 @;*/
/*		PUT AF_LST_DTS_AP03 @;*/
/*		PUT AD_LON_1_PAY_DU @;*/
/*		PUT AI_STA_ORG_PRC_CHG @;*/
/*		PUT AI_PNT_PRT @;*/
/*		PUT AD_TO_SER_CMP @;*/
/*		PUT AI_LDR_RGT_SLD_CHG @;*/
/*		PUT AC_MN_MST_NTE_ASG @;*/
/*		PUT AC_CRD_WOR_RED @;*/
/*		PUT AI_FEE_MNL_CLC @;*/
/*		PUT AC_BR_ST_DSB_1 @;*/
/*		PUT AC_SCL_ST_DSB_1 @;*/
/*		PUT AC_FIL_TYP_REQ @;*/
/*		PUT AC_FIL_RCP @;*/
/*		PUT AC_PNT_YR @;*/
/*		PUT AC_STP_PUR @;*/
/*		PUT AI_PIO_APL_LDR @;*/
/*		PUT AI_SCL_ENT_CCL @;*/
/*		PUT AI_DEG_SKG @;*/
/*		PUT AA_BS_POI @;*/
/*		PUT AF_CUR_POR @;*/
/*		PUT AF_DTS_APL_ELG_CER @;*/
/*		PUT AD_DTR_APL_CER_CMP @;*/
/*		PUT AF_USR_CER_CLG @;*/
/*		PUT AC_EDU_LEV_TYP @;*/
/*		PUT AA_LST_CTR_OFR @;*/
/*		PUT AD_LST_CTR_OFR @;*/
/*		PUT AC_BR_CTR_OFR_RSP @;*/
/*		PUT AN_BR_PIO_ALT_DNL @;*/
/*		PUT AF_ALT_LON_APL @;*/
/*		PUT AC_ALT_APL_ELG_STA @;*/
/*		PUT AC_ALT_APL_ACT_STA @;*/
/*		PUT AF_APL_SLC_DIS @;*/
/*		PUT AI_TLX_APL @;*/
/*		PUT AF_PNT_VRS @;*/
/*		PUT AD_APL_EXP @;*/
/*		PUT AC_APL_EXP_EVT @;*/
/*		PUT AC_APL_RVW @;*/
/*		PUT AC_CNL_TRN_REA @;*/
/*		PUT AC_ALT_APL_FLW $;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LN40 END = EOF;*/
/*	FILE REPORT38 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT BF_SSN @ ;*/
/*		PUT LN_SEQ @ ;*/
/*		PUT LN_SEQ_CLM_PCL @ ;*/
/*		PUT LA_SBM_CLM_PCL_PRI @ ;*/
/*		PUT LA_SBM_CLM_PCL_INT @ ;*/
/*		PUT LI_CLM_PKG_RTN_RCV @ ;*/
/*		PUT LC_CLM_REJ_RTN_LIB @ ;*/
/*		PUT LI_GTR_ACK_CLM_CAN @ ;*/
/*		PUT LC_REA_CAN_CLM_PCL @ ;*/
/*		PUT LD_CRT_CLM_PCL @ ;*/
/*		PUT LD_CLM_REJ_RTN_ACL @ ;*/
/*		PUT LD_CLM_REJ_RTN_EFF @ ;*/
/*		PUT LD_CLM_REJ_RTN_MAX @ ;*/
/*		PUT LD_SBM_CLM_PCL @ ;*/
/*		PUT LD_CAN_CLM_PCL @ ;*/
/*		PUT LC_TYP_REJ_RTN @ ;*/
/*		PUT LF_LST_DTS_LN40 @ ;*/
/*		PUT LC_TYP_REC_CLP_LON @ ;*/
/*		PUT LC_REA_CLP_LON @ ;*/
/*		PUT LI_TSK_CRT_RSI_BAL @ ;*/
/*		PUT LN_SEQ_CLM_PCL_ORG @ ;*/
/*		PUT LD_CND_OCC @ ;*/
/*		PUT LD_CLM_PD_PCV @ ;*/
/*		PUT LA_CLM_PD_PCV @ ;*/
/*		PUT LD_CLM_ORG_CRT @ ;*/
/*		PUT LD_CLM_ORG_SBM @ ;*/
/*		PUT LI_PCL_CLM_PCV @ ;*/
/*		PUT LC_REA_CLM_REJ_RTN @ ;*/
/*		PUT LC_SUP_PCA @ ;*/
/*		PUT LD_OSD_CLM @ ;*/
/*		PUT LD_NTF_OSD_CLM @ ;*/
/*		PUT LI_RPD_CHG_CLM @ ;*/
/*		PUT LD_1_PAY_DU_CLM @ ;*/
/*		PUT LA_TOT_BR_PAY_CLM @ ;*/
/*		PUT LN_MTH_PAY_CLM @ ;*/
/*		PUT LN_MTH_DFR_CLM @ ;*/
/*		PUT LN_MTH_FOR_CLM @ ;*/
/*		PUT LN_MTH_VIO_CLM @ ;*/
/*		PUT LN_DFR_FOR_EVT_CLM @ ;*/
/*		PUT LN_MTH_RNV_CLM @ ;*/
/*		PUT LD_PAY_DU_CLM @ ;*/
/*		PUT LA_TOT_DSB_CLM @ ;*/
/*		PUT LA_CAP_INT_CLM @ ;*/
/*		PUT LA_PRI_RPD_CLM @ ;*/
/*		PUT LA_CU_INT_CAP_CLM @ ;*/
/*		PUT LD_INT_PD_THU_CLM @ ;*/
/*		PUT LD_CLM_INT_CLM @ ;*/
/*		PUT LA_UNP_INT_NO_CAP @ ;*/
/*		PUT LD_CLM_REJ_LTR @ ;*/
/*		PUT LA_DSA_RFD_CLM @ ;*/
/*		PUT LD_CLM_PD_LTR @ ;*/
/*		PUT LN_CCI_CLM_SEQ @ ;*/
/*		PUT LD_CCI_LON_SLD @ ;*/
/*		PUT LD_CCI_SER_RSB @ ;*/
/*		PUT LD_XCP_PRF @ ;*/
/*		PUT LA_CCI_UNP_FEE @ ;*/
/*		PUT LA_CCI_UNP_INT @ ;*/
/*		PUT LA_ITL_STD_PAY_CLM @ ;*/
/*		PUT LA_PMN_STD_PAY_CLM @ ;*/
/*		PUT LD_25_YR_FGV_CLM @ ;*/
/*		PUT LN_MTH_QLF_FGV_CLM @ ;*/
/*		PUT LD_IBR_SR_CLM @ ;*/
/*		PUT LN_DAY_EHD_DFR_CLM $ ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET CL10 END = EOF;*/
/*	FILE REPORT39 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT BF_SSN @ ;*/
/*		PUT LN_SEQ_CLM_PCL @ ;*/
/*		PUT LC_REA_CLM_PCL @ ;*/
/*		PUT LC_TYP_REC_CLM_PCL @ ;*/
/*		PUT LF_USR_ASN_CLM_PCL @ ;*/
/*		PUT LI_CLM_GTR_RCV @ ;*/
/*		PUT LD_CLM_RQR @ ;*/
/*		PUT LF_CLM_BCH @ ;*/
/*		PUT LF_LST_DTS_CL10 @ ;*/
/*		PUT LI_CLM_QA @ ;*/
/*		PUT LD_GTR_CLM_RCI @ ;*/
/*		PUT LC_GTR_CLM_ACK @ ;*/
/*		PUT LC_CAN_STA_CCI @ ;*/
/*		PUT LI_CLM_CLL_RCV @ ;*/
/*		PUT LC_XCP_PRF $ ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LN80A END = EOF;*/
/*	FILE REPORT40 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT BF_SSN @ ;*/
/*		PUT LN_SEQ @ ;*/
/*		PUT LD_BIL_CRT @ ;*/
/*		PUT LN_SEQ_BIL_WI_DTE @ ;*/
/*		PUT LN_BIL_OCC_SEQ @ ;*/
/*		PUT LA_BIL_CUR_DU @ ;*/
/*		PUT LA_BIL_PAS_DU @ ;*/
/*		PUT LC_STA_LON80 @ ;*/
/*		PUT LA_NSI_BIL @ ;*/
/*		PUT LA_CUR_PRN_BIL @ ;*/
/*		PUT LC_LON_STA_BIL @ ;*/
/*		PUT LR_INT_BIL @ ;*/
/*		PUT LD_LST_DTS_LN80 @ ;*/
/*		PUT LI_PSB @ ;*/
/*		PUT LD_BIL_DU_LON @ ;*/
/*		PUT LC_BIL_TYP_LON @ ;*/
/*		PUT LI_FNL_BIL_LON @ ;*/
/*		PUT LD_STA_LON80 @ ;*/
/*		PUT LA_BIL_DU_PRT @ ;*/
/*		PUT LA_TOT_BIL_STS @ ;*/
/*		PUT LA_PCV_BIL_STS @ ;*/
/*		PUT LF_DFR_CTL_NUM @ ;*/
/*		PUT LF_FOR_CTL_NUM @ ;*/
/*		PUT LD_LTE_FEE_ASS @ ;*/
/*		PUT LA_LTE_FEE_ASS @ ;*/
/*		PUT LI_LTE_FEE_OVR @ ;*/
/*		PUT LC_LTE_FEE_WAV_REA @ ;*/
/*		PUT LD_BIL_STS_RIR_TOL @ ;*/
/*		PUT LI_BIL_DLQ_OVR_RIR @ ;*/
/*		PUT LF_USR_DLQ_OVR_RIR @ ;*/
/*		PUT LA_LTE_FEE_OTS_PRT @ ;*/
/*		PUT LD_LTE_FEE_WAV @ ;*/
/*		PUT LD_RP_RTE_2B_DTR @ ;*/
/*		PUT LC_RPY_OPT_AWD_BIL @ ;*/
/*		PUT LN_CU_SEQ @ ;*/
/*		PUT LC_RPD_RLF_ETR @ ;*/
/*		PUT LA_LTE_FEE_EST_BIL @ ;*/
/*		PUT LN_DAY_FEE_GRC_BIL @ ;*/
/*		PUT LA_INT_PD_2DT_BIL @ ;*/
/*		PUT LA_FEE_PD_2DT_BIL @ ;*/
/*		PUT LA_PRI_PD_2DT_BIL @ ;*/
/*		PUT LI_PSV_FGV_BIL_OVR @ ;*/
/*		PUT LA_PIO_BIL_PFH $ ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET SC10 END = EOF;*/
/*	FILE REPORT41 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT IF_DOE_SCL @ ;*/
/*		PUT IM_SCL_SHO @ ;*/
/*		PUT IM_SCL_FUL @ ;*/
/*		PUT IC_TYP_MEX_RCC_EFT @ ;*/
/*		PUT IF_ACC_EFT_SCL @ ;*/
/*		PUT IF_ABA_EFT_SCL @ ;*/
/*		PUT IC_TYP_SCL @ ;*/
/*		PUT IR_COH_DFL @ ;*/
/*		PUT ID_PRV_SCL_STA @ ;*/
/*		PUT IC_PRV_SCL_STA @ ;*/
/*		PUT ID_CUR_SCL_STA @ ;*/
/*		PUT IC_CUR_SCL_STA @ ;*/
/*		PUT IF_LST_USR_SC10 @ ;*/
/*		PUT IF_LST_DTS_SC10 @ ;*/
/*		PUT II_SCL_CHS_PTC @ ;*/
/*		PUT IM_SCL_VRU_TXT @ ;*/
/*		PUT II_SCL_BR_MPN_CNF @ ;*/
/*		PUT II_SCL_SFD_MPN_CNF @ ;*/
/*		PUT II_SCL_USF_MPN_CNF @ ;*/
/*		PUT II_SCL_BR_MPN_OVR @ ;*/
/*		PUT IF_SCL_MPN_CNF_LTR @ ;*/
/*		PUT IF_SCL_MPN_NTF_LTR @ ;*/
/*		PUT IC_SCL_USB_LMT @ ;*/
/*		PUT IC_SCL_SKP_CNC_TYP @ ;*/
/*		PUT II_SCL_SGS_DSB_MON @ ;*/
/*		PUT II_SCL_SGS_DSB_TUE @ ;*/
/*		PUT II_SCL_SGS_DSB_WED @ ;*/
/*		PUT II_SCL_SGS_DSB_THU @ ;*/
/*		PUT II_SCL_SGS_DSB_FRI @ ;*/
/*		PUT II_SCL_HOL_NXT_PRC @ ;*/
/*		PUT II_MLT_DSB_ROS_DAY @ ;*/
/*		PUT IF_CDF_RTE_YR @ ;*/
/*		PUT IR_CDF_PRV_1 @ ;*/
/*		PUT IF_CDF_RTE_PRV_YR1 @ ;*/
/*		PUT IR_CDF_PRV_2 @ ;*/
/*		PUT IF_CDF_RTE_PRV_YR2 @ ;*/
/*		PUT IF_CDA @ ;*/
/*		PUT II_SCL_MNF_DSB_RSN @ ;*/
/*		PUT IC_SND_CTF_REQ @ ;*/
/*		PUT IC_SCL_AUT_DBT_PTC @ ;*/
/*		PUT IC_LEN_LNG_PGM_STY @ ;*/
/*		PUT IC_SCL_OWN_CTL_TYP @ ;*/
/*		PUT IF_PEPS_DL_SCL @ ;*/
/*		PUT II_SCL_IDP_RPT @ ;*/
/*		PUT IF_MRG_SCL @ ;*/
/*		PUT ID_MRG_SCL_EFF $ ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET PD20 END = EOF;*/
/*	FILE REPORT42 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT DF_PRS_ID @ ;*/
/*		PUT DD_DTH_NTF @ ;*/
/*		PUT DD_DTH @ ;*/
/*		PUT DM_DTH_CT @ ;*/
/*		PUT DM_DTH_CTY @ ;*/
/*		PUT DC_DTH_ST @ ;*/
/*		PUT DF_SUR_PRS_ID @ ;*/
/*		PUT DM_DTH_FGN_CNY @ ;*/
/*		PUT PF_LST_DTS_PD20 @ ;*/
/*		PUT IF_IST $ ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET PD21 END = EOF;*/
/*	FILE REPORT43 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT DF_PRS_ID @ ;*/
/*		PUT DD_DTH_NTF @ ;*/
/*		PUT IF_GTR @ ;*/
/*		PUT DD_DTH_VER @ ;*/
/*		PUT DC_DTH_STA @ ;*/
/*		PUT DC_DTH_CER @ ;*/
/*		PUT DF_LST_DTS_PD21 @ ;*/
/*		PUT DD_DTH_STA $ ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET PD22 END = EOF;*/
/*	FILE REPORT44 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT DF_PRS_ID @ ;*/
/*		PUT DD_DSA_RPT @ ;*/
/*		PUT DD_DSA @ ;*/
/*		PUT DF_DR @ ;*/
/*		PUT DC_DSA_NEW_WRS @ ;*/
/*		PUT DF_LST_DTS_PD22 @ ;*/
/*		PUT DI_DSA_VET @ ;*/
/*		PUT DD_PRS_DSA_SPS_SR @ ;*/
/*		PUT DD_PRS_DSA_SPS_END @ ;*/
/*		PUT DF_CRT_USR_PD22 @ ;*/
/*		PUT DF_LST_USR_PD22 @ ;*/
/*		PUT DD_REC_LST_UPD @ ;*/
/*		PUT DX_PRS_DSA_TPD_REA $ ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET PD23 END = EOF;*/
/*	FILE REPORT45 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT DF_PRS_ID @ ;*/
/*		PUT DD_DSA_RPT @ ;*/
/*		PUT IF_GTR @ ;*/
/*		PUT DD_DSA_VER @ ;*/
/*		PUT DD_SBM_CLM_REQ @ ;*/
/*		PUT DD_DSA_APV @ ;*/
/*		PUT DI_DSA_XTN_REQ @ ;*/
/*		PUT DI_DSA_APV @ ;*/
/*		PUT DC_DSA_STA @ ;*/
/*		PUT DF_LST_DTS_PD23 @ ;*/
/*		PUT DD_DSA_STA $ ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET PD24 END = EOF;*/
/*	FILE REPORT46 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT DF_PRS_ID @ ;*/
/*		PUT DD_BKR_NTF @ ;*/
/*		PUT DD_BKR_FIL @ ;*/
/*		PUT DC_BKR_TYP @ ;*/
/*		PUT DF_ATT @ ;*/
/*		PUT DF_COU_DKT @ ;*/
/*		PUT DD_BKR_VER @ ;*/
/*		PUT DC_BKR_DCH_NDC @ ;*/
/*		PUT DM_BKR_CT @ ;*/
/*		PUT DC_BKR_ST @ ;*/
/*		PUT DD_BKR_COR_1_RCV @ ;*/
/*		PUT DA_BKR_DCH @ ;*/
/*		PUT DD_BKR_STA @ ;*/
/*		PUT DD_BKR_POO_ACK @ ;*/
/*		PUT DD_BKR_POO @ ;*/
/*		PUT DD_BKR_DCH_RCV @ ;*/
/*		PUT DD_BKR_CDR_RCV @ ;*/
/*		PUT DD_BKR_ADS_RCV @ ;*/
/*		PUT DN_BKR_ADS @ ;*/
/*		PUT DC_BKR_STA @ ;*/
/*		PUT DF_LST_DTS_PD24 @ ;*/
/*		PUT IF_IST @ ;*/
/*		PUT DM_FGN_CNY_BKR_FIL @ ;*/
/*		PUT DM_FGN_ST_BKR_FIL @ ;*/
/*		PUT DD_BKR_RAF @ ;*/
/*		PUT DD_COU_LST_CNC @ ;*/
/*		PUT DD_BKR_CAE_CLO @ ;*/
/*		PUT DD_BKR_CHP_CVN @ ;*/
/*		PUT DD_BKR_COR_LST_RCV $ ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET RS05 END = EOF;*/
/*	FILE REPORT47 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT BF_SSN @ ;*/
/*		PUT BD_CRT_RS05 @ ;*/
/*		PUT BN_IBR_SEQ @ ;*/
/*		PUT BF_CRT_USR_RS05 @ ;*/
/*		PUT BF_CRY_YR @ ;*/
/*		PUT BC_ST_IBR @ ;*/
/*		PUT BC_STA_RS05 @ ;*/
/*		PUT BA_AGI @ ;*/
/*		PUT BN_MEM_HSE_HLD @ ;*/
/*		PUT BA_PMN_STD_TOT_PAY @ ;*/
/*		PUT BC_IBR_INF_SRC_VER @ ;*/
/*		PUT BF_LST_DTS_RS05 @ ;*/
/*		PUT BF_SSN_SPO @ ;*/
/*		PUT BC_IRS_TAX_FIL_STA @ ;*/
/*		PUT BI_JNT_BR_SPO_RPY @ ;*/
/*		PUT BD_ANV_QLF_IBR @ ;*/
/*		PUT BC_DOC_SNT_BR_IDR $ ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET MR01 END = EOF;*/
/*	FILE REPORT48 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT BF_SSN @ ;*/
/*		PUT LN_SEQ @ ;*/
/*		PUT LA_CUR_PRI @ ;*/
/*		PUT WA_ACR_BRI_RUN_DTE @ ;*/
/*		PUT WD_RUN @ ;*/
/*		PUT WN_DAY_DLQ_INT @ ;*/
/*		PUT WD_DCO_INT @ ;*/
/*		PUT WA_PSS_DU_INT @ ;*/
/*		PUT WA_CUR_INT_DU @ ;*/
/*		PUT WN_DAY_DLQ_ISL @ ;*/
/*		PUT WD_DCO_ISL @ ;*/
/*		PUT WA_PSS_DU_ISL @ ;*/
/*		PUT WA_CUR_DU_ISL @ ;*/
/*		PUT IC_LON_PGM @ ;*/
/*		PUT WX_LON_PGM @ ;*/
/*		PUT IF_BND_ISS @ ;*/
/*		PUT LD_LON_1_DSB @ ;*/
/*		PUT LD_LON_EFF_ADD @ ;*/
/*		PUT LF_STU_SSN @ ;*/
/*		PUT LD_STA_STU10 @ ;*/
/*		PUT LD_SCL_SPR @ ;*/
/*		PUT LD_DFR_BEG @ ;*/
/*		PUT LD_DFR_END @ ;*/
/*		PUT LD_DFR_GRC_END @ ;*/
/*		PUT LC_DFR_RSP @ ;*/
/*		PUT WX_DFR_RSP @ ;*/
/*		PUT LD_DFR_APL @ ;*/
/*		PUT LD_FOR_BEG @ ;*/
/*		PUT LD_FOR_END @ ;*/
/*		PUT LC_FOR_RSP @ ;*/
/*		PUT WX_FOR_RSP @ ;*/
/*		PUT LD_FOR_APL @ ;*/
/*		PUT WA_RPS_ISL_1 @ ;*/
/*		PUT WN_RPS_TRM_1 @ ;*/
/*		PUT WA_RPS_ISL_2 @ ;*/
/*		PUT WN_RPS_TRM_2 @ ;*/
/*		PUT WA_RPS_ISL_3 @ ;*/
/*		PUT WN_RPS_TRM_3 @ ;*/
/*		PUT WA_RPS_ISL_4 @ ;*/
/*		PUT WN_RPS_TRM_4 @ ;*/
/*		PUT WA_RPS_ISL_5 @ ;*/
/*		PUT WN_RPS_TRM_5 @ ;*/
/*		PUT WA_RPS_ISL_6 @ ;*/
/*		PUT WN_RPS_TRM_6 @ ;*/
/*		PUT WA_RPS_ISL_7 @ ;*/
/*		PUT WN_RPS_TRM_7 @ ;*/
/*		PUT LD_RPS_1_PAY_DU @ ;*/
/*		PUT WC_ITR_TYP_1 @ ;*/
/*		PUT WX_ITR_TYP_1 @ ;*/
/*		PUT WR_ITR_1 @ ;*/
/*		PUT WD_ITR_EFF_BEG_1 @ ;*/
/*		PUT WC_ITR_TYP_2 @ ;*/
/*		PUT WX_ITR_TYP_2 @ ;*/
/*		PUT WR_ITR_2 @ ;*/
/*		PUT WD_ITR_EFF_BEG_2 @ ;*/
/*		PUT DM_PRS_1 @ ;*/
/*		PUT DM_PRS_MID @ ;*/
/*		PUT DM_PRS_LST @ ;*/
/*		PUT DM_PRS_LST_SFX @ ;*/
/*		PUT DI_PHN_VLD @ ;*/
/*		PUT DN_DOM_PHN_ARA @ ;*/
/*		PUT IF_GTR @ ;*/
/*		PUT LF_LON_OWN_CUR @ ;*/
/*		PUT LF_DOE_SCL_ORG @ ;*/
/*		PUT LF_DOE_SCL_ENR_CUR @ ;*/
/*		PUT LF_GTR_RFR @ ;*/
/*		PUT LD_END_GRC_PRD @ ;*/
/*		PUT LC_ELG_SIN @ ;*/
/*		PUT WX_ELG_SIN @ ;*/
/*		PUT LF_CUR_POR @ ;*/
/*		PUT LF_OWN_ORG_POR @ ;*/
/*		PUT LC_LOC_PNT @ ;*/
/*		PUT WX_LOC_PNT @ ;*/
/*		PUT LD_OWN_EFF_SR @ ;*/
/*		PUT WC_ISL_DLQ_CAT @ ;*/
/*		PUT WX_ISL_DLQ_CAT @ ;*/
/*		PUT WC_INT_DLQ_CAT @ ;*/
/*		PUT WX_INT_DLQ_CAT @ ;*/
/*		PUT WA_ORG_PRI @ ;*/
/*		PUT WN_ATV_DSB @ ;*/
/*		PUT WN_ACL_DSB @ ;*/
/*		PUT WN_ANT_DSB @ ;*/
/*		PUT WC_LON_STA @ ;*/
/*		PUT WX_LON_STA @ ;*/
/*		PUT WC_LON_SUB_STA @ ;*/
/*		PUT WX_LON_SUB_STA @ ;*/
/*		PUT WC_LON_CLM_STA @ ;*/
/*		PUT WX_LON_CLM_STA @ ;*/
/*		PUT WC_BR_PRS_STA @ ;*/
/*		PUT WX_BR_PRS_STA @ ;*/
/*		PUT LC_DFR_TYP @ ;*/
/*		PUT WX_DFR_TYP @ ;*/
/*		PUT LC_FOR_TYP @ ;*/
/*		PUT WX_FOR_TYP @ ;*/
/*		PUT LC_FOR_SUB_TYP @ ;*/
/*		PUT WX_FOR_SUB_TYP @ ;*/
/*		PUT LC_TYP_SCH_DIS @ ;*/
/*		PUT WX_TYP_SCH_DIS @ ;*/
/*		PUT LD_NTF_SCL_SPR @ ;*/
/*		PUT LD_SPA_STP @ ;*/
/*		PUT LD_SPA_STP_ENT @ ;*/
/*		PUT LD_SPA_RTT @ ;*/
/*		PUT LD_SPA_RTT_ENT @ ;*/
/*		PUT WA_ACR_BRI_MTT @ ;*/
/*		PUT WA_CUR_LTE_FEE @ ;*/
/*		PUT WA_PRV_LTE_FEE @ ;*/
/*		PUT LC_ST_BR_RSD_APL @ ;*/
/*		PUT LC_STA_NEW_BR @ ;*/
/*		PUT WF_NON_PR_ACT_REQ @ ;*/
/*		PUT WD_FNL_DMD_BR @ ;*/
/*		PUT WD_FNL_DMD_EDS @ ;*/
/*		PUT LC_IND_BIL_SNT @ ;*/
/*		PUT LC_BIL_MTD @ ;*/
/*		PUT LD_DSB @ ;*/
/*		PUT WA_LST_DSB_WK @ ;*/
/*		PUT WI_LON_FUL_DSB_WK $ ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LN10A END = EOF;*/
/*	FILE REPORT49 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT BF_SSN @ ;*/
/*		PUT LN_SEQ @ ;*/
/*		PUT LC_STA_LON10 @ ;*/
/*		PUT LI_FGV_PGM @ ;*/
/*		PUT LF_LON_SLE_PND @ ;*/
/*		PUT LA_CUR_PRI @ ;*/
/*		PUT LI_ELG_CPN_BOK @ ;*/
/*		PUT LA_LON_AMT_GTR @ ;*/
/*		PUT LD_LON_GTR @ ;*/
/*		PUT LD_PIF_PNT_RTN @ ;*/
/*		PUT LD_PNT_SIG @ ;*/
/*		PUT LC_INT_BIL_OPT @ ;*/
/*		PUT LI_CAP_ALW @ ;*/
/*		PUT LD_END_GRC_PRD @ ;*/
/*		PUT LN_MTH_GRC_PRD_DSC @ ;*/
/*		PUT LA_R78_INT_PD @ ;*/
/*		PUT LA_R78_INT_MAX @ ;*/
/*		PUT LD_CAP_LST_PIO_CVN @ ;*/
/*		PUT LD_TRM_END @ ;*/
/*		PUT LD_TRM_BEG @ ;*/
/*		PUT LI_GTR_NAT @ ;*/
/*		PUT LF_GTR_RFR @ ;*/
/*		PUT LI_ELG_SPA @ ;*/
/*		PUT LD_GTE_LOS @ ;*/
/*		PUT LA_SCL_CLS @ ;*/
/*		PUT LA_CUR_ILG @ ;*/
/*		PUT LA_ILG @ ;*/
/*		PUT LD_PIF_RPT @ ;*/
/*		PUT LA_NSI_OTS @ ;*/
/*		PUT LD_NSI_ACR_THU @ ;*/
/*		PUT LD_STA_LON10 @ ;*/
/*		PUT LD_LON_ACL_ADD @ ;*/
/*		PUT LD_LON_EFF_ADD @ ;*/
/*		PUT LF_DOE_SCL_ORG @ ;*/
/*		PUT LC_PCV_DIS_STA @ ;*/
/*		PUT LC_RPR_TYP @ ;*/
/*		PUT LF_RGL_CAT_LP19 @ ;*/
/*		PUT LF_RGL_CAT_LP18 @ ;*/
/*		PUT LF_RGL_CAT_LP17 @ ;*/
/*		PUT LF_RGL_CAT_LP13 @ ;*/
/*		PUT LF_RGL_CAT_LP12 @ ;*/
/*		PUT LF_RGL_CAT_LP11 @ ;*/
/*		PUT LF_RGL_CAT_LP10 @ ;*/
/*		PUT LF_RGL_CAT_LP08 @ ;*/
/*		PUT LF_RGL_CAT_LP07 @ ;*/
/*		PUT LF_RGL_CAT_LP06 @ ;*/
/*		PUT LF_RGL_CAT_LP05 @ ;*/
/*		PUT LF_RGL_CAT_LP04 @ ;*/
/*		PUT LF_RGL_CAT_LP03 @ ;*/
/*		PUT LF_RGL_CAT_LP02 @ ;*/
/*		PUT LF_RGL_CAT_LP01 @ ;*/
/*		PUT LD_SCL_CLS_NTF @ ;*/
/*		PUT LD_ILG_NTF @ ;*/
/*		PUT LF_LON_CUR_OWN @ ;*/
/*		PUT LF_LST_DTS_LN10 @ ;*/
/*		PUT LC_STA_NEW_BR @ ;*/
/*		PUT LC_SCY_PGA @ ;*/
/*		PUT LD_SIN_LST_PD_PCV @ ;*/
/*		PUT LD_SIN_ACR_THU_PCV @ ;*/
/*		PUT LA_SIN_OTS_PCV @ ;*/
/*		PUT IC_LON_PGM @ ;*/
/*		PUT PF_MAJ_BCH @ ;*/
/*		PUT PF_MNR_BCH @ ;*/
/*		PUT IF_DOE_LDR @ ;*/
/*		PUT IF_GTR @ ;*/
/*		PUT LF_STU_SSN @ ;*/
/*		PUT LD_LON_1_DSB @ ;*/
/*		PUT LC_ACA_GDE_LEV @ ;*/
/*		PUT LD_NEW_SYS_CVN @ ;*/
/*		PUT LC_SCY_PGA_PGM_YR @ ;*/
/*		PUT IC_HSP_CSE @ ;*/
/*		PUT LI_TL4_793_XCL_CON @ ;*/
/*		PUT LI_DFR_REQ_ON_APL @ ;*/
/*		PUT LI_LN_PT_COM_APL @ ;*/
/*		PUT LN_SEQ_RPR @ ;*/
/*		PUT LR_WIR_CON_LON @ ;*/
/*		PUT LR_INT_RDC_PGM_DSU @ ;*/
/*		PUT LI_1_TME_BR @ ;*/
/*		PUT BF_SSN_RPR @ ;*/
/*		PUT LC_ELG_RDC_PGM @ ;*/
/*		PUT LD_ELG_RDC_PGM @ ;*/
/*		PUT LC_RPD_SLE @ ;*/
/*		PUT LR_ITR_ORG @ ;*/
/*		PUT LC_ITR_TYP_ORG @ ;*/
/*		PUT LC_RDC_PGM @ ;*/
/*		PUT LC_TIR_GRP @ ;*/
/*		PUT LD_SER_RSB_BEG @ ;*/
/*		PUT LC_EFT_RDC @ ;*/
/*		PUT LD_LTS_STS_BIL @ ;*/
/*		PUT IF_TIR_PCE @ ;*/
/*		PUT LF_RGL_CAT_LP20 @ ;*/
/*		PUT LN_RDC_PGM_PAY_PCV @ ;*/
/*		PUT LC_REA_PIF_PCV @ ;*/
/*		PUT LD_FAT_PIF_TOL_PCV @ ;*/
/*		PUT LA_FAT_PIF_TOL_PCV @ ;*/
/*		PUT LI_RTE_RDC_ELG @ ;*/
/*		PUT LD_LTE_FEE_ELG @ ;*/
/*		PUT LA_LTE_FEE_OTS @ ;*/
/*		PUT LD_LON_LTE_FEE_WAV @ ;*/
/*		PUT LC_CUR_RDC_PGM_NME @ ;*/
/*		PUT LI_RIR_SCY_ELG @ ;*/
/*		PUT LD_END_RIR_DSQ_OVR @ ;*/
/*		PUT LF_LON_ALT @ ;*/
/*		PUT LN_LON_ALT_SEQ @ ;*/
/*		PUT LI_LDR_LST_RST_DSB @ ;*/
/*		PUT LC_ST_BR_RSD_APL @ ;*/
/*		PUT LI_PIF_RPT_REQ @ ;*/
/*		PUT LD_AMR_BEG @ ;*/
/*		PUT LD_ORG_XPC_GRD @ ;*/
/*		PUT LD_CON_LST_RPY_BEG @ ;*/
/*		PUT LN_CON_MTH_DFR_FOR @ ;*/
/*		PUT LD_LON_APL_RCV @ ;*/
/*		PUT LR_SCL_SUB @ ;*/
/*		PUT LI_BLL_PAY_SES_OVR @ ;*/
/*		PUT LC_MPN_TYP @ ;*/
/*		PUT LD_MPN_EXP @ ;*/
/*		PUT LC_MPN_SRL_LON @ ;*/
/*		PUT LC_MPN_REV_REA @ ;*/
/*		PUT LF_ORG_RGN @ ;*/
/*		PUT LC_CAM_LON_STA @ ;*/
/*		PUT LD_DFR_FOR_END @ ;*/
/*		PUT LC_DFR_FOR_TYP @ ;*/
/*		PUT LF_CAM_DFR_SCL_ENR @ ;*/
/*		PUT LD_DFR_FOR_BEG @ ;*/
/*		PUT LD_CAM_DFR_INF_CER @ ;*/
/*		PUT LI_BR_DET_RPD_XTN @ ;*/
/*		PUT DD_DTH_VER @ ;*/
/*		PUT DD_DSA_VER @ ;*/
/*		PUT LI_CON_PAY_STP_PUR @ ;*/
/*		PUT LD_FSE_CER_NTF @ ;*/
/*		PUT LA_TOT_EDU_DET @ ;*/
/*		PUT LI_LDR_BG_APL @ ;*/
/*		PUT LD_RIR_CSC_BIL_STS @ ;*/
/*		PUT LI_ESG @ ;*/
/*		PUT LC_RIR_DSQ_REA @ ;*/
/*		PUT LF_MN_MST_NTE @ ;*/
/*		PUT LN_MN_MST_NTE_SEQ @ ;*/
/*		PUT LC_LON_SND_CHC @ ;*/
/*		PUT LC_SST_LON10 @ ;*/
/*		PUT LF_RGL_CAT_LP09 @ ;*/
/*		PUT LI_MN_PSD_BS @ ;*/
/*		PUT LF_CRD_RTE_SRE @ ;*/
/*		PUT LF_ESG_SRC @ ;*/
/*		PUT PC_PNT_YR @ ;*/
/*		PUT LF_OWN_BND_ISS_TE1 @ ;*/
/*		PUT LF_OWN_BND_ISS_TE2 @ ;*/
/*		PUT LF_OWN_BND_ISS_TE3 @ ;*/
/*		PUT LD_MPN_STM_SNT @ ;*/
/*		PUT LI_MNT_BIL_RCP @ ;*/
/*		PUT LX_BS_POI @ ;*/
/*		PUT LA_BS_POI @ ;*/
/*		PUT LA_INT_FEE_URP_IRS @ ;*/
/*		PUT LC_BR_ALW_SCL_DFR @ ;*/
/*		PUT LD_BR_ALW_SCL_DFR @ ;*/
/*		PUT LC_LON_DFR_SUB_TYP @ ;*/
/*		PUT LD_FAT_PRI_BAL_ZRO @ ;*/
/*		PUT LC_ST_SCL_ATD_APL @ ;*/
/*		PUT LD_EFT_DSQ_NSF_LMT @ ;*/
/*		PUT LD_CLM_PD @ ;*/
/*		PUT LC_STP_PUR @ ;*/
/*		PUT LD_CON_PAY_EFF @ ;*/
/*		PUT LD_CON_PAY_APL @ ;*/
/*		PUT LC_ESG @ ;*/
/*		PUT LC_LON_RPE_CVN_REA @ ;*/
/*		PUT LC_UDL_DSB_COF @ ;*/
/*		PUT LI_BR_LT_HT @ ;*/
/*		PUT LI_ALL_PAY_FLG_SPS @ ;*/
/*		PUT LN_MFY_GRS_PAY @ ;*/
/*		PUT LC_ESP_RPD_OPT_SEL @ ;*/
/*		PUT LC_ELG_95_SPA_BIL @ ;*/
/*		PUT LC_SGM_COS_PRC @ ;*/
/*		PUT LD_GTR_DR_DCH_CER @ ;*/
/*		PUT LD_RPD_ELY_CL_BEG @ ;*/
/*		PUT LD_RPD_ELY_CL_END @ ;*/
/*		PUT LF_GTR_RFR_XTN @ ;*/
/*		PUT LA_MSC_FEE_OTS @ ;*/
/*		PUT LA_MSC_FEE_PCV_OTS @ ;*/
/*		PUT LF_LON_GRP_WI_BR @ ;*/
/*		PUT LC_TL4_IBR_ELG @ ;*/
/*		PUT LD_LON_IBR_ENT @ ;*/
/*		PUT LC_LON_IBR_RPY_TYP @ ;*/
/*		PUT LI_BYP_COL_OUT_SRC @ ;*/
/*		PUT LI_BR_GRP_RLP @ ;*/
/*		PUT LI_OO_PST_ENR_DFR @ ;*/
/*		PUT LD_OO_PST_ENR_DFR @ ;*/
/*		PUT LF_FED_CLC_RSK @ ;*/
/*		PUT LF_FED_FFY_1_DSB @ ;*/
/*		PUT LF_PRV_GTR @ ;*/
/*		PUT LC_FED_PGM_YR @ ;*/
/*		PUT LA_INT_RCV_GOV @ ;*/
/*		PUT LC_WOF_WUP_REA @ ;*/
/*		PUT LC_VRS_ALT_APL @ ;*/
/*		PUT LF_LON_DCV_CLI @ ;*/
/*		PUT LN_LON_SEQ_DCV_CLI @ ;*/
/*		PUT LI_EDS_BKR_STP_PUR @ ;*/
/*		PUT LF_EDS @ ;*/
/*		PUT LD_EFF_LBR_RTE @ ;*/
/*		PUT LA_STD_STD_PAY @ ;*/
/*		PUT LI_FRC_IBR @ ;*/
/*		PUT LC_STP_PUR_REA @ ;*/
/*		PUT LD_IDR_ELG_CHG @ ;*/
/*		PUT LC_IDR_ELG_CRI $ ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET RM31A END = EOF;*/
/*	FILE REPORT50 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "01JAN1960";*/
/*	DO;*/
/*		PUT LD_RMT_BCH_INI @ ;*/
/*		PUT LC_RMT_BCH_SRC_IPT @ ;*/
/*		PUT LN_RMT_BCH_SEQ @ ;*/
/*		PUT LN_RMT_SEQ_PST @ ;*/
/*		PUT LN_RMT_ITM_PST @ ;*/
/*		PUT LN_RMT_ITM_SEQ_PST @ ;*/
/*		PUT LD_RMT_PST_PST @ ;*/
/*		PUT LD_RMT_SPS_PST @ ;*/
/*		PUT LD_RMT_PAY_EFF_PST @ ;*/
/*		PUT LA_BR_RMT_PST @ ;*/
/*		PUT LD_CRT_REMT31 @ ;*/
/*		PUT LD_UPD_REMT31 @ ;*/
/*		PUT LX_SPS_REA_PST @ ;*/
/*		PUT LM_RMT_IST_PST @ ;*/
/*		PUT LM_RMT_LST_PST @ ;*/
/*		PUT LM_RMT_MID_PST @ ;*/
/*		PUT LM_RMT_1_PST @ ;*/
/*		PUT LF_RMT_IST_PST @ ;*/
/*		PUT LF_RMT_EDS_PST @ ;*/
/*		PUT LX_RMT_PST @ ;*/
/*		PUT LI_RMT_PD_AHD_PST @ ;*/
/*		PUT LC_RMT_REV_REA_PST @ ;*/
/*		PUT LC_RMT_STA_PST @ ;*/
/*		PUT LD_RMT_BCH_INI_PVS @ ;*/
/*		PUT LC_RMT_BCH_IPT_PVS @ ;*/
/*		PUT LN_RMT_BCH_SEQ_PVS @ ;*/
/*		PUT LN_RMT_SEQ_PVS @ ;*/
/*		PUT LN_RMT_ITM_PVS @ ;*/
/*		PUT LN_RMT_ITM_SEQ_PVS @ ;*/
/*		PUT LF_LST_DTS_RM31 @ ;*/
/*		PUT BF_SSN @ ;*/
/*		PUT PC_FAT_TYP @ ;*/
/*		PUT PC_FAT_SUB_TYP @ ;*/
/*		PUT LD_BIL_CRT @ ;*/
/*		PUT LN_SEQ_BIL_WI_DTE @ ;*/
/*		PUT LC_RMT_BCH @ ;*/
/*		PUT LI_DPS_SPS_PIO_CVN @ ;*/
/*		PUT BN_CPN_BK_SEQ @ ;*/
/*		PUT LD_BIL_CPN_DU_PST @ ;*/
/*		PUT LI_PHD_PAS_DU_PST @ ;*/
/*		PUT LI_PHD_PAS_RPS_PST @ ;*/
/*		PUT LI_PHD_PAS_OPT_PST @ ;*/
/*		PUT LF_USR_UPD_SPS @ ;*/
/*		PUT LC_RMT_REV_REJ_PST @ ;*/
/*		PUT LF_RMT_ACC_ID_PST @ ;*/
/*		PUT LI_DPL_ACC_RMT_PST @ ;*/
/*		PUT LC_STP_SPS_OVR_PST @ ;*/
/*		PUT LC_SPS_OVR_FRD_PST @ ;*/
/*		PUT LC_WAV_MSC_FEE_PST @ ;*/
/*		PUT LD_SCH_RMT_PAY_PST @ ;*/
/*		PUT LF_LCK_BOX_TRC_PST @ ;*/
/*		PUT LF_RMT_PST_SCH_NUM @ ;*/
/*		PUT LC_RMT_PST_SCH_TYP @ ;*/
/*		PUT LI_CON_SPS_OVR @ ;*/
/*		PUT LD_RMT_PST_SCH_DPS @ ;*/
/*		PUT LF_RMT_PST_SCH_IVC @ ;*/
/*		PUT LF_RMT_PST_OBD_NUM @ ;*/
/*		PUT LC_RMT_PST_OBD_TYP @ ;*/
/*		PUT LD_RMT_PST_OBD_DPS @ ;*/
/*		PUT LI_AUT_SUS_REV @ ;*/
/*		PUT LC_PST_RMT_ESH $ ;*/
/*	END;*/
/*	IF EOF THEN PUT "01JAN1960";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LN90A END = EOF;*/
/*	FILE REPORT51 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT BF_SSN @ ;*/
/*		PUT LN_SEQ @ ;*/
/*		PUT LN_FAT_SEQ @ ;*/
/*		PUT LC_FAT_REV_REA @ ;*/
/*		PUT LD_FAT_APL @ ;*/
/*		PUT LD_FAT_PST @ ;*/
/*		PUT LD_FAT_EFF @ ;*/
/*		PUT LD_FAT_DPS @ ;*/
/*		PUT LC_CSH_ADV @ ;*/
/*		PUT LD_STA_LON90 @ ;*/
/*		PUT LC_STA_LON90 @ ;*/
/*		PUT LA_FAT_PCL_FEE @ ;*/
/*		PUT LA_FAT_NSI @ ;*/
/*		PUT LA_FAT_LTE_FEE @ ;*/
/*		PUT LA_FAT_ILG_PRI @ ;*/
/*		PUT LA_FAT_CUR_PRI @ ;*/
/*		PUT LF_LST_DTS_LN90 @ ;*/
/*		PUT PC_FAT_TYP @ ;*/
/*		PUT PC_FAT_SUB_TYP @ ;*/
/*		PUT LA_FAT_NSI_ACR @ ;*/
/*		PUT LI_FAT_RAP @ ;*/
/*		PUT LN_FAT_SEQ_REV @ ;*/
/*		PUT LI_EFT_NSF_OVR @ ;*/
/*		PUT LF_USR_EFT_NSF_OVR @ ;*/
/*		PUT LA_FAT_MSC_FEE @ ;*/
/*		PUT LA_FAT_MSC_FEE_PCV @ ;*/
/*		PUT LA_FAT_DL_REB $ ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LN33 END = EOF;*/
/*	FILE REPORT52 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT BF_SSN @ ;*/
/*		PUT LN_SEQ @ ;*/
/*		PUT LN_CU_SEQ @ ;*/
/*		PUT LD_CU_ENT @ ;*/
/*		PUT LD_CU_END @ ;*/
/*		PUT LC_CU_REA_END @ ;*/
/*		PUT LC_CU_LIA @ ;*/
/*		PUT LC_CU_REA @ ;*/
/*		PUT LC_CU_CND @ ;*/
/*		PUT LC_CU_TYP @ ;*/
/*		PUT LC_CU_SCH @ ;*/
/*		PUT LD_CU_SR_ORG @ ;*/
/*		PUT LD_DLQ_1_CU_BEG @ ;*/
/*		PUT LD_R99_END_RPT @ ;*/
/*		PUT LD_R99_BEG_RPT @ ;*/
/*		PUT LN_R99_SEQ @ ;*/
/*		PUT LF_LST_DTS_LN33 @ ;*/
/*		PUT LA_R99_REJ_PRI @ ;*/
/*		PUT LD_VIO_LON_CU @ ;*/
/*		PUT LD_CU_3_YR_RUL @ ;*/
/*		PUT LI_CU_PUR_ALW @ ;*/
/*		PUT LC_NSLDS_STA_RPT $ ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET GU10 END = EOF;*/
/*	FILE REPORT53 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT IF_GTR @ ;*/
/*		PUT IF_ACC_EFT_GTR @ ;*/
/*		PUT IF_ABA_EFT_GTR @ ;*/
/*		PUT IM_GTR_SHO @ ;*/
/*		PUT IM_GTR_FUL @ ;*/
/*		PUT II_RPT_GTR_SCL @ ;*/
/*		PUT II_RPT_GTR_LDR @ ;*/
/*		PUT II_ASN_RFR_NUM @ ;*/
/*		PUT IC_RPT_DSB_TYP @ ;*/
/*		PUT II_GTR_NAT_PTC @ ;*/
/*		PUT IF_LST_USR_GU10 @ ;*/
/*		PUT IF_LST_DTS_GU10 @ ;*/
/*		PUT IF_GTR_PRN @ ;*/
/*		PUT IC_GTR_TYP @ ;*/
/*		PUT II_GTR_PRE_DSB_M @ ;*/
/*		PUT II_GTR_NOG_REQ @ ;*/
/*		PUT II_GTR_CNL_PTC @ ;*/
/*		PUT II_GTR_MNF_DSB_RSN @ ;*/
/*		PUT IC_GTR_FEE_RPT_MTH @ ;*/
/*		PUT II_RAL_SCY_OVR @ ;*/
/*		PUT II_GTR_ALW_PSB_MPN @ ;*/
/*		PUT II_GTR_CHS_PTC @ ;*/
/*		PUT II_GTR_VER_FEE_RQR @ ;*/
/*		PUT II_GTR_WHL_DOL_DSB $ ;*/
/**/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET RF10 END = EOF;*/
/*	FILE REPORT54 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT BF_SSN @ ;*/
/*		PUT BN_SEQ_RFR @ ;*/
/*		PUT BC_STA_REFR10 @ ;*/
/*		PUT BI_ATH_3_PTY @ ;*/
/*		PUT BC_RFR_REL_BR @ ;*/
/*		PUT BD_EFF_RFR @ ;*/
/*		PUT BF_RFR @ ;*/
/*		PUT BC_RFR_TYP @ ;*/
/*		PUT BF_LST_DTS_RF10 @ ;*/
/*		PUT BD_EFF_RFR_HST @ ;*/
/*		PUT BC_REA_RFR_HST @ ;*/
/*		PUT BF_LST_USR_HST_RFR @ ;*/
/*		PUT BD_ATH_3_PTY_END $ ;*/
/**/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LN35 END = EOF;*/
/*	FILE REPORT55 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT BF_SSN @ ;*/
/*		PUT LN_SEQ @ ;*/
/*		PUT IF_OWN @ ;*/
/*		PUT LN_LON_OWN_SEQ @ ;*/
/*		PUT LD_OWN_EFF_SR @ ;*/
/*		PUT LF_BR_LON_OWN_ACC @ ;*/
/*		PUT LF_CUR_POR @ ;*/
/*		PUT LD_OWN_EFF_END @ ;*/
/*		PUT LF_OWN_ORG_POR @ ;*/
/*		PUT LC_LOC_PNT @ ;*/
/*		PUT LC_STA_LON35 @ ;*/
/*		PUT LF_LST_DTS_LN35 @ ;*/
/*		PUT IF_BND_ISS @ ;*/
/*		PUT IF_LON_SLE @ ;*/
/*		PUT IF_TIR_PCE @ ;*/
/*		PUT LD_LON_IRL_SLE_TRF @ ;*/
/*		PUT LI_ORG_RGT_PUR_SLE @ ;*/
/*		PUT LF_OWN_EFT_RIR_ASN @ ;*/
/*		PUT LA_LON_LVL_TRF_FEE @ ;*/
/*		PUT LD_PRE_CVN_OWN_BEG $ ;*/
/**/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LN60 END = EOF;*/
/*	FILE REPORT56 DELIMITER=',' DSD DROPOVER LRECL=32767;*/
/**/
/*	IF _N_ = 1 THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT BF_SSN @ ;*/
/*		PUT LN_SEQ @ ;*/
/*		PUT LF_FOR_CTL_NUM @ ;*/
/*		PUT LN_FOR_OCC_SEQ @ ;*/
/*		PUT LC_FOR_RSP @ ;*/
/*		PUT LD_FOR_BEG @ ;*/
/*		PUT LD_FOR_END @ ;*/
/*		PUT LD_STA_LON60 @ ;*/
/*		PUT LC_STA_LON60 @ ;*/
/*		PUT LD_FOR_APL @ ;*/
/*		PUT LF_LST_DTS_LN60 @ ;*/
/*		PUT LI_FOR_20_RPT @ ;*/
/*		PUT LC_LON_LEV_FOR_CAP @ ;*/
/*		PUT LA_FOR_20_INT_ACR @ ;*/
/*		PUT LA_ACL_RDC_PAY @ ;*/
/*		PUT LI_FOR_VRB_DFL_RUL $ ;*/
/**/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/

DATA _NULL_;
	SET LN54 END = EOF;
	FILE REPORT57 DELIMITER=',' DSD DROPOVER LRECL=32767;

	IF _N_ = 1 THEN PUT "-Begin-";
	DO;
		PUT BF_SSN @ ;
		PUT LN_SEQ @ ;
		PUT PM_BBS_PGM @ ;
		PUT PN_BBS_PGM_SEQ @ ;
		PUT LN_LON_BBS_PGM_SEQ @ ; 
		PUT LD_EFF_BEG_LN54 @ ; 
		PUT LI_BBS_ITD_LTR_SNT @ ;
		PUT LN_BBS_STS_PCV_PAY @ ;
		PUT LC_BBS_REB_MTD @ ;
		PUT LC_STA_LN54 @ ;
		PUT LD_STA_LN54 @ ;
		PUT LC_BBT_TYS_ASS @ ;
		PUT LC_BBS_DSQ_REA @ ;
		PUT LD_BBS_DSQ @ ;
		PUT LC_BBS_ELG @ ;
		PUT LC_BBT_PRC_RBD @ ;
		PUT LD_BBS_RPD_WDO_END @ ;
		PUT LC_BBS_BCH_PRC @ ;
		PUT LF_LST_USR_LN54 @ ;
		PUT LF_LST_DTS_LN54 @ ;
		PUT LN_BBS_PCV_PAY_MOT @ ;
		PUT LD_BBS_ICV_REQ @ ;
		PUT LD_BBS_DSQ_APL @ ;
		PUT LI_BBS_PCV_LTE_PAY $;

	END;
	IF EOF THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET LN55 END = EOF;
	FILE REPORT58 DELIMITER=',' DSD DROPOVER LRECL=32767;

	IF _N_ = 1 THEN PUT "-Begin-";
	DO;
		PUT BF_SSN @ ;
		PUT LN_SEQ @ ;
		PUT LN_LON_BBS_SEQ @ ;
		PUT LN_LON_BBT_SEQ @ ;
		PUT LF_LON_BBS_TIR @ ;
		PUT LF_LON_BBS_SUB_TIR @ ;
		PUT PM_BBS_PGM @ ;
		PUT PN_BBS_PGM_SEQ @ ;
		PUT PF_BBS_PGM_TIR @ ;
		PUT PN_BBS_PGM_TIR_SEQ @ ;
		PUT LN_BR_DSB_SEQ @ ;
		PUT LC_STA_LN55 @ ;
		PUT LD_STA_LN55 @ ;
		PUT LD_LON_BBT_CHK_ISS @ ;
		PUT LD_BBT_DSQ_OVR_END @ ;
		PUT LN_LON_BBT_PAY_OVR @ ;
		PUT LD_LON_BBT_BEG @ ;
		PUT LD_REB_MTD_LTR_SNT @ ;
		PUT LC_LON_BBT_REB_MTD @ ;
		PUT LN_BBT_PAY_PIF_MOT @ ;
		PUT LN_BBT_PAY_DLQ_MOT @ ;
		PUT LC_LON_BBT_DSQ_REA @ ;
		PUT LC_LON_BBT_STA @ ;
		PUT LN_LON_BBT_PAY @ ;
		PUT LD_LON_BBT_STA @ ;
		PUT LD_BBT_STS_PAY @ ;
		PUT LF_LST_USR_LN55 @ ;
		PUT LF_LST_DTS_LN55 @ ;
		PUT LD_LON_BBT_ELG_FNL @ ;
		PUT LD_BBT_DLQ_MOT_STS @ ;
		PUT LD_BBT_PIF_MOT_STS @ ;
		PUT LN_BBT_DLQ_MOT_OVR @ ;
		PUT LN_BBT_PIF_MOT_OVR $ ;

	END;
	IF EOF THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET RS10 END = EOF;
	FILE REPORT59 DELIMITER=',' DSD DROPOVER LRECL=32767;

	IF _N_ = 1 THEN PUT "-Begin-";
	DO;
		PUT BF_SSN @;
		PUT LN_RPS_SEQ @;
		PUT LD_STA_RPST10 @;
		PUT LC_STA_RPST10 @;
		PUT LC_FRQ_PAY @;
		PUT LI_SIG_RPD_DIS @;
		PUT LD_RPS_1_PAY_DU @;
		PUT LC_RPD_DIS @;
		PUT LD_SNT_RPD_DIS @;
		PUT LD_RTN_RPD_DIS @;
		PUT LF_LST_DTS_RS10 @;
		PUT LC_RPS_OPT_PRT @;
		PUT LF_USR_RPS_REQ @;
		PUT LN_BR_REQ_DU_DAY @;
		PUT BD_CRT_RS05 @;
		PUT BN_IBR_SEQ @;
		PUT LC_RPY_FIX_TRM_AMT @;
		PUT LC_CAP_TRG_LVE_PFH $;

	END;
	IF EOF THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET LN65A END = EOF;
	FILE REPORT60 DELIMITER=',' DSD DROPOVER LRECL=32767;

	IF _N_ = 1 THEN PUT "-Begin-";
	DO;
		PUT BF_SSN @;
		PUT LN_SEQ @;
		PUT LN_RPS_SEQ @;
		PUT LA_RPD_INT_DIS @;
		PUT LR_APR_RPD_DIS @;
		PUT LA_TOT_RPD_DIS @;
		PUT LA_CPI_RPD_DIS @;
		PUT LR_INT_RPD_DIS @;
		PUT LA_ANT_CAP @;
		PUT LD_GRC_PRD_END @;
		PUT LD_CRT_LON65 @;
		PUT LC_STA_LON65 @;
		PUT LF_LST_DTS_LN65 @;
		PUT LC_TYP_SCH_DIS @;
		PUT LA_ACR_INT_RPD @;
		PUT LA_ANT_SUP_FEE @;
		PUT LN_RPD_MAX_TRM_REQ @;
		PUT LD_RPD_MAX_TRM_SR @;
		PUT LC_RPD_INA_REA @;
		PUT LC_RPD_DIS @;
		PUT LR_CLC_INC_SCH @;
		PUT LA_CLC_RPY_SCH @;
		PUT LI_ICR_RPD_NEG_AMR $;

	END;
	IF EOF THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET LN66 END = EOF;
	FILE REPORT61 DELIMITER=',' DSD DROPOVER LRECL=32767;

	IF _N_ = 1 THEN PUT "-Begin-";
	DO;
		PUT BF_SSN @;
		PUT LN_SEQ @;
		PUT LN_RPS_SEQ @;
		PUT LN_GRD_RPS_SEQ @;
		PUT LA_RPS_ISL @;
		PUT LD_CRT_LON66 @;
		PUT LN_RPS_TRM @;
		PUT LF_LST_DTS_LN66 @;
		PUT LA_PRI_RDC_GRD @;
		PUT LN_PRI_RDC_GRD_TRM $;

	END;
	IF EOF THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET PD10A END = EOF;
	FILE REPORT62 DELIMITER=',' DSD DROPOVER LRECL=32767;

	IF _N_ = 1 THEN PUT "-Begin-";
	DO;
		PUT DF_PRS_ID @;
		PUT DD_STA_PRS @;
		PUT DC_LAG_FGN @;
		PUT DC_SEX @;
		PUT DD_BRT @;
		PUT DM_PRS_MID @;
		PUT DM_PRS_1 @;
		PUT DM_PRS_LST_SFX @;
		PUT DM_PRS_LST @;
		PUT DD_DRV_LIC_REN @;
		PUT DC_ST_DRV_LIC @;
		PUT DF_DRV_LIC @;
		PUT DD_NME_VER_LST @;
		PUT DI_ORG_HLD @;
		PUT DF_LST_USR_PD10 @;
		PUT DF_ALN_RGS @;
		PUT DI_US_CTZ @;
		PUT DF_LST_DTS_PD10 @;
		PUT DF_SPE_ACC_ID @;
		PUT DF_PRS_LST_4_SSN @;
		PUT DI_ATU_FMT @;
		PUT DC_ATU_FMT_TYP $;

	END;
	IF EOF THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET RP30 END = EOF;
	FILE REPORT63 DELIMITER=',' DSD DROPOVER LRECL=32767;

	IF _N_ = 1 THEN PUT "-Begin-";
	DO;
		PUT IF_OWN @;
		PUT PN_EFT_RIR_OWN_SEQ @;
		PUT IC_LON_PGM @;
		PUT IF_GTR @;
		PUT PD_LON_1_DSB @;
		PUT PF_DOE_SCL_ORG @;
		PUT PC_ST_BR_RSD_APL @;
		PUT PD_EFT_RIR_EFF_BEG @;
		PUT PD_EFT_RIR_EFF_END @;
		PUT PC_EFT_RIR_STA @;
		PUT PD_EFT_RIR_STA @;
		PUT PI_EFT_RIR_PRC @;
		PUT PC_EFT_NSF_LTR_REQ @;
		PUT PR_EFT_RIR @;
		PUT PF_LST_USR_RP30 @;
		PUT PF_LST_DTS_RP30 @;
		PUT PC_EFT_RIR_PNT_YR @;
		PUT PD_EFT_BBS_LOT_BEG @;
		PUT PD_EFT_BBS_GTE_DTE @;
		PUT PD_EFT_BBS_RPD_SR @;
		PUT PD_EFT_BBS_LCO_RCV @;
		PUT PN_EFT_BBS_NSF_LMT @;
		PUT PC_EFT_BBS_NSF_PRC @;
		PUT PN_EFT_BBS_NSF_MTH @;
		PUT PC_EFT_BBS_FED @;
		PUT PI_EFT_RIR_RPY_0 $;
	END;
	IF EOF THEN PUT "-End-";
RUN;
