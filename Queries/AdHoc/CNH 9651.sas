/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
/*FILENAME REPORTX "&RPTLIB/UNWDWX.NWDWXRX";*/
/*FILENAME REPORTZ "&RPTLIB/UNWDWX.NWDWXRZ";*/
/*FILENAME REPORTX "&RPTLIB/UNWDWX.NWDWXRX";*/
/*FILENAME REPORTX "&RPTLIB/UNWDWX.NWDWXRX";*/
/*FILENAME REPORTX "&RPTLIB/UNWDWX.NWDWXRX";*/
/*FILENAME REPORTX "&RPTLIB/UNWDWX.NWDWXRX";*/
/*FILENAME REPORTX "&RPTLIB/UNWDWX.NWDWXRX";*/
/*FILENAME REPORTX "&RPTLIB/UNWDWX.NWDWXRX";*/
/*FILENAME REPORTX "&RPTLIB/UNWDWX.NWDWXRX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*/
FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";
FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";
FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
RSUBMIT; 
/*%LET DB = DNFPRQUT;  *This is test;*/
%LET DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB; 
LIBNAME PKUS DBX DATABASE=&DB OWNER=PKUS; 
LIBNAME SAS_TAB VX '/sas/whse/progrevw';

/*get last run date*/
DATA _NULL_; 
	SET SAS_TAB.LASTRUN_JOBS;

	/*If the job must be run manually set this macro to the last day it successfully ran(last business day).*/
	LAST_RUN = TODAY() - XX;	*COMMENT FOR PROD, IT WILL READ THE DATE FROM A TABLE;

	IF JOB = 'UTNWDWX' THEN DO;
		CALL SYMPUT('LAST_RUN',"'"||TRIM(LEFT(PUT(LAST_RUN,DATEXX.)))|| "'D");
		CALL SYMPUT('LAST_RUNPASS',"'"|| PUT(LAST_RUN,MMDDYYXX.) || "'");
	END;
RUN;
%PUT &LAST_RUN; 

/*THE TABLES ARE GOING TO HAVE THE ACCOUNT NUMBER RATHER THAN SSN AS A PRIMARY KEY*/
/*THE ACCOUNT NUMBER IS BEING TAKEN FROM THE PDXX TABLE WHILE SSN IS DROPPED*/
%MACRO SSNXACC(TBL,ADKEY);
	PROC SORT DATA=&TBL; BY BF_SSN &ADKEY; RUN;

	DATA &TBL(DROP=BF_SSN);
		MERGE SSNXACC(IN=B) &TBL(IN=A);
		BY BF_SSN;
		IF A AND B;
	RUN;

	PROC SORT DATA=&TBL; BY DF_SPE_ACC_ID &ADKEY; RUN;
%MEND;

/*******************************************
* BORROWER DATA
********************************************/
PROC SQL;
	CREATE TABLE SSNXACC AS
		SELECT DISTINCT 
			PDXX.DF_SPE_ACC_ID AS DF_SPE_ACC_ID,
			PDXX.DF_PRS_ID AS BF_SSN
		FROM 
			PKUB.PDXX_PRS_NME PDXX
			LEFT JOIN PKUB.LNXX_LON LNXX
				ON PDXX.DF_PRS_ID = LNXX.BF_SSN
		WHERE 
			COALESCE(LNXX.LC_STA_LONXX,'') NOT IN ('P','J')
			AND PDXX.DF_SPE_ACC_ID ^= ''
/*			and pdXX.df_prs_id like '%XXX'*/
	;
QUIT;
PROC SORT DATA=SSNXACC; BY BF_SSN; RUN;
/**/
/*/********************************************/
/** ENDORSER DATA*/
/*********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*	CREATE TABLE LNXX AS*/
/*		SELECT **/
/*		FROM CONNECTION TO DBX (*/
/*			SELECT */
/*				LNXX.BF_SSN,*/
/*				LNXX.LN_SEQ,*/
/*				LNXX.LC_STA_LONXX,*/
/*				LNXX.LC_EDS_TYP*/
/*			FROM*/
/*				PKUB.LNXX_EDS LNXX*/
/*			WHERE*/
/*				LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/*		FOR READ ONLY WITH UR*/
/*		);*/
/*DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(LNXX,LN_SEQ);*/
/**/
/*/********************************************/
/** REFERENCE DATA*/
/*********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*	CREATE TABLE BRXX AS*/
/*		SELECT **/
/*		FROM CONNECTION TO DBX (*/
/*			SELECT RFXX.BF_SSN*/
/*				,RFXX.BF_RFR*/
/*				,RFXX.BC_STA_REFRXX*/
/*				,RFXX.BI_ATH_X_PTY*/
/*				,RFXX.BC_RFR_REL_BR*/
/*				,PDXX.DM_PRS_X*/
/*				,PDXX.DM_PRS_LST*/
/*				,CASE */
/*					WHEN AYXX.PF_RSP_ACT IN ('CNTCT') THEN 'CNC'*/
/*					WHEN AYXX.PF_RSP_ACT IN ('NOCTC','INVPH') THEN 'ATT'*/
/*					ELSE ''*/
/*				END AS CONTACT*/
/*				,AYXX.LD_ATY_RSP*/
/*			FROM PKUB.RFXX_RFR RFXX*/
/*				INNER JOIN PKUB.PDXX_PRS_NME PDXX*/
/*					ON RFXX.BF_RFR = PDXX.DF_PRS_ID*/
/*				LEFT OUTER JOIN PKUB.AYXX_BR_LON_ATY AYXX*/
/*					ON AYXX.LF_ATY_RCP = RFXX.BF_RFR*/
/*					AND AYXX.LC_STA_ACTYXX = 'A'*/
/*					AND AYXX.PF_RSP_ACT IN ('CNTCT','NOCTC','INVPH')*/
/*		WHERE BF_LST_DTS_RFXX >= &LAST_RUNPASS */
/*			OR LF_LST_DTS_AYXX >= &LAST_RUNPASS*/
/*			*/
/*	FOR READ ONLY WITH UR*/
/*);*/
/*DISCONNECT FROM DBX;*/
/*QUIT;*/
/*PROC SORT DATA=BRXX; BY BF_SSN BF_RFR CONTACT LD_ATY_RSP; RUN;*/
/*DATA BRXX(DROP=CONTACT);*/
/*	SET BRXX;*/
/*	BY BF_SSN BF_RFR CONTACT LD_ATY_RSP;*/
/*	IF FIRST.CONTACT THEN DO;*/
/*		LST_ATT = .;*/
/*		LST_CNC = .;*/
/*	END;*/
/*	RETAIN LST_CNC LST_ATT;*/
/*	IF CONTACT = 'CNC' THEN LST_CNC = MAX(LD_ATY_RSP,LST_CNC);*/
/*	ELSE IF CONTACT = 'ATT' THEN LST_ATT = MAX(LD_ATY_RSP,LST_ATT);*/
/*	IF LAST.BF_RFR THEN do;*/
/*		LST_ATT = MAX(LST_ATT,LST_CNC);*/
/*		OUTPUT;*/
/*	end;*/
/*RUN;*/
/**/
/*%SSNXACC(BRXX,BF_RFR);*/
/**/
/*/********************************************/
/** TRANSACTION DATA*/
/*********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*	CREATE TABLE LNXX AS*/
/*		SELECT **/
/*		FROM CONNECTION TO DBX (*/
/*			SELECT */
/*				LNXX.BF_SSN,*/
/*				LNXX.LN_SEQ,*/
/*				LNXX.LN_FAT_SEQ,*/
/*				LNXX.LD_FAT_PST,*/
/*				LNXX.LD_FAT_EFF,*/
/*				LNXX.LC_STA_LONXX,*/
/*				COALESCE(LNXX.LA_FAT_CUR_PRI,X) AS LA_FAT_CUR_PRI,*/
/*				COALESCE(LNXX.LA_FAT_NSI,X) AS LA_FAT_NSI,*/
/*				COALESCE(LNXX.LA_FAT_LTE_FEE,X) AS LA_FAT_LTE_FEE,*/
/*				LNXX.PC_FAT_TYP,*/
/*				LNXX.PC_FAT_SUB_TYP,*/
/*				LNXX.LC_FAT_REV_REA*/
/*			FROM*/
/*				PKUB.LNXX_FIN_ATY LNXX*/
/*			WHERE*/
/*				LNXX.LC_FAT_REV_REA ^= 'X'*/
/*				AND LNXX.PC_FAT_TYP IN ('XX','XX','XX','XX','XX','XX','XX','XX','XX','XX')*/
/*				AND LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/*		FOR READ ONLY WITH UR*/
/*		);*/
/*DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(LNXX,LN_SEQ LN_FAT_SEQ);*/
/**/
/*/********************************************/
/** BILLING DATA*/
/*********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*	CREATE TABLE LNXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT LNXX.BF_SSN*/
/*			,LNXX.LN_SEQ*/
/*			,LNXX.LD_BIL_CRT*/
/*			,LNXX.LN_SEQ_BIL_WI_DTE*/
/*			,LNXX.LN_BIL_OCC_SEQ*/
/*			,LNXX.LD_BIL_DU_LON*/
/*			,LNXX.LC_STA_LONXX*/
/*			,COALESCE(LNXX.la_bil_cur_du,X) AS LA_BIL_CUR_DU*/
/*			,COALESCE(LNXX.la_bil_pas_du,X) AS la_bil_pas_du*/
/*			,COALESCE(LNXX.la_tot_bil_sts,X) AS la_tot_bil_sts*/
/*			,LC_BIL_MTD*/
/*			,LC_IND_BIL_SNT*/
/*			,LC_STA_BILXX*/
/*			,LNXX.LN_FAT_SEQ*/
/*			,LNXX.LD_FAT_EFF*/
/*			,LNXX.LC_LON_STA_BIL*/
/*			,BLXX.LA_INT_PD_LST_STM*/
/*			,BLXX.LA_FEE_PD_LST_STM*/
/*			,BLXX.LA_PRI_PD_LST_STM*/
/*			,BLXX.LA_INT_PD_LST_STM + BLXX.LA_FEE_PD_LST_STM + BLXX.LA_PRI_PD_LST_STM AS LA_TTL_PD_LST_STM*/
/*			,LNXX.LA_LTE_FEE_OTS_PRT*/
/*		FROM PKUB.BLXX_BR_BIL BLXX*/
/*			INNER JOIN PKUB.LNXX_LON_BIL_CRF LNXX*/
/*				ON LNXX.BF_SSN = BLXX.BF_SSN*/
/*				AND LNXX.LD_BIL_CRT = BLXX.LD_BIL_CRT*/
/*				AND LNXX.LN_SEQ_BIL_WI_DTE = BLXX.LN_SEQ_BIL_WI_DTE*/
/*			LEFT OUTER JOIN PKUB.LNXX_BIL_LON_FAT LNXX*/
/*				ON LNXX.BF_SSN = LNXX.BF_SSN*/
/*				AND LNXX.LN_SEQ = LNXX.LN_SEQ*/
/*				AND LNXX.LD_BIL_CRT = LNXX.LD_BIL_CRT*/
/*				AND LNXX.LN_SEQ_BIL_WI_DTE = LNXX.LN_SEQ_BIL_WI_DTE*/
/*				AND LNXX.LN_BIL_OCC_SEQ = LNXX.LN_BIL_OCC_SEQ*/
/*				AND LNXX.LA_BIL_CUR_DU = LNXX.LA_TOT_BIL_STS*/
/*			LEFT OUTER JOIN PKUB.LNXX_FIN_ATY LNXX*/
/*				ON LNXX.BF_SSN = LNXX.BF_SSN*/
/*				AND LNXX.LN_SEQ = LNXX.LN_SEQ*/
/*				AND LNXX.LN_FAT_SEQ = LNXX.LN_FAT_SEQ*/
/*		WHERE LNXX.LC_BIL_TYP_LON = 'P' */
/*			AND LNXX.LD_LST_DTS_LNXX >= &LAST_RUNPASS*/
/**/
/*	FOR READ ONLY WITH UR*/
/*	);*/
/*	DISCONNECT FROM DBX;*/
/*QUIT; */
/**/
/*PROC SORT DATA=LNXX; BY BF_SSN LN_SEQ LD_BIL_CRT LN_SEQ_BIL_WI_DTE LN_BIL_OCC_SEQ LN_FAT_SEQ; RUN;*/
/**/
/*DATA LNXX(DROP=LN_BIL_OCC_SEQ LN_FAT_SEQ); */
/*	SET LNXX;*/
/*	BY BF_SSN LN_SEQ LD_BIL_CRT LN_SEQ_BIL_WI_DTE ;*/
/*	IF LAST.LN_SEQ_BIL_WI_DTE;*/
/*RUN;*/
/**/
/*%SSNXACC(LNXX,LN_SEQ LD_BIL_CRT LN_SEQ_BIL_WI_DTE);*/
/**/
/*/********************************************/
/** DEFERMENT DATA*/
/*********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE DFXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT LNXX.BF_SSN*/
/*			,LNXX.LN_SEQ*/
/*			,LNXX.LF_DFR_CTL_NUM*/
/*			,LNXX.LN_DFR_OCC_SEQ*/
/*			,DFXX.LC_DFR_TYP*/
/*			,DFXX.LD_DFR_INF_CER*/
/*			,LNXX.LD_DFR_BEG*/
/*			,LNXX.LD_DFR_END*/
/*			,LNXX.LC_LON_LEV_DFR_CAP*/
/*			,LNXX.LC_STA_LONXX*/
/*			,DFXX.LC_DFR_STA*/
/*			,DFXX.LC_STA_DFRXX*/
/*		FROM PKUB.LNXX_BR_DFR_APV LNXX*/
/*			INNER JOIN PKUB.DFXX_BR_DFR_REQ DFXX*/
/*				ON LNXX.BF_SSN = DFXX.BF_SSN*/
/*				AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM*/
/*		WHERE (LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS */
/*				OR DFXX.LF_LST_DTS_DFXX >= &LAST_RUNPASS)*/
/**/
/*	FOR READ ONLY WITH UR*/
/*	);*/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(DFXX,LN_SEQ LF_DFR_CTL_NUM LN_DFR_OCC_SEQ);*/
/*/********************************************/
/** FORBEARANCE DATA*/
/*********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE FBXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT LNXX.BF_SSN*/
/*			,LNXX.LN_SEQ*/
/*			,LNXX.LF_FOR_CTL_NUM*/
/*			,LNXX.LN_FOR_OCC_SEQ*/
/*			,FBXX.LC_FOR_TYP*/
/*			,FBXX.LD_FOR_INF_CER*/
/*			,LNXX.LD_FOR_BEG*/
/*			,LNXX.LD_FOR_END*/
/*			,LNXX.LC_LON_LEV_FOR_CAP */
/*			,LNXX.LC_STA_LONXX*/
/*			,FBXX.LC_FOR_STA*/
/*			,FBXX.LC_STA_FORXX*/
/*			,COALESCE(FBXX.LA_REQ_RDC_PAY,X) AS LA_REQ_RDC_PAY*/
/*			,LNXX.LI_FOR_VRB_DFL_RUL*/
/*		FROM PKUB.LNXX_BR_FOR_APV LNXX*/
/*			INNER JOIN PKUB.FBXX_BR_FOR_REQ FBXX*/
/*				ON LNXX.BF_SSN = FBXX.BF_SSN*/
/*				AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM*/
/*		WHERE (LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/*				OR FBXX.LF_LST_DTS_FBXX >= &LAST_RUNPASS)*/
/**/
/*	FOR READ ONLY WITH UR*/
/*	);*/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(FBXX,LN_SEQ LF_FOR_CTL_NUM LN_FOR_OCC_SEQ);*/
/**/
/*/********************************************/
/** LOAN DATA*/
/*********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE LNXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT */
/*			LNXX.BF_SSN,*/
/*			LNXX.LN_SEQ, */
/*			LNXX.LC_STA_LONXX, */
/*			LNXX.LA_CUR_PRI, */
/*			LNXX.LA_LON_AMT_GTR, */
/*			LNXX.LD_END_GRC_PRD,*/
/*			LNXX.IC_LON_PGM, */
/*			LNXX.LD_LON_X_DSB, */
/*			LNXX.LD_PIF_RPT,*/
/*			LNXX.LC_SST_LONXX,*/
/*			LNXX.LF_LON_CUR_OWN,*/
/*			LNXX.LF_DOE_SCL_ORG*/
/*		FROM */
/*			PKUB.LNXX_LON LNXX*/
/*		WHERE*/
/*			LNXX.LC_STA_LONXX NOT IN ('P','J')*/
/**/
/*	FOR READ ONLY WITH UR*/
/*	);*/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(LNXX,LN_SEQ);*/
/**/
/*/********************************************/
/** ACH DATA*/
/*********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE LNXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT*/
/*			LNXX.BF_SSN,*/
/*			LNXX.LN_SEQ,*/
/*			LNXX.BN_EFT_SEQ,*/
/*			LNXX.LD_EFT_EFF_END,*/
/*			LNXX.LC_STA_LNXX*/
/*		FROM*/
/*			PKUB.LNXX_EFT_TO_LON LNXX*/
/*		WHERE*/
/*			LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/**/
/*	FOR READ ONLY WITH UR*/
/*	);*/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*PROC SORT DATA=LNXX; BY BF_SSN LN_SEQ DESCENDING BN_EFT_SEQ; RUN;*/
/*DATA LNXX;*/
/*	SET LNXX;*/
/*	BY BF_SSN LN_SEQ;*/
/*	IF FIRST.LN_SEQ;*/
/*RUN;*/
/*%SSNXACC(LNXX,LN_SEQ);*/
/**/
/*/********************************************/
/** REHAB DATA */
/*********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE LNXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT */
/*			LNXX.BF_SSN,*/
/*			LNXX.LN_SEQ,*/
/*			LNXX.LD_LON_RHB_PCV*/
/*		FROM*/
/*			PKUB.LNXX_RPD_PIO_CVN LNXX*/
/*		WHERE*/
/*			LNXX.LD_LON_RHB_PCV IS NOT NULL*/
/*			AND LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/**/
/*	FOR READ ONLY WITH UR*/
/*	);*/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(LNXX,LN_SEQ);*/
/**/
/*/**********************************************/
/** DISBURSEMENT*/
/***********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE LNXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT BF_SSN*/
/*			,LN_BR_DSB_SEQ*/
/*			,LA_DSB - COALESCE(LA_DSB_CAN,X) AS LA_DSB*/
/*			,LD_DSB*/
/*			,LC_DSB_TYP*/
/*			,LC_STA_LONXX*/
/*			,LN_SEQ*/
/*			,COALESCE(LA_DL_DSB_REB,X) - COALESCE(LA_DSB_REB_CAN,X) AS LA_DL_REBATE*/
/*		FROM pkub.LNXX_DSB */
/*		WHERE LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/*FOR READ ONLY WITH UR*/
/*);*/
/*QUIT;*/
/*%SSNXACC(LNXX,LN_BR_DSB_SEQ);*/
/**/
/*/********************************************/
/** BORROWER BENEFIT ELIGIBILITY*/
/*********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE LNXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT LNXX.BF_SSN*/
/*			,LNXX.LN_SEQ*/
/*			,RPXX.PR_EFT_RIR*/
/*			,LNXX.PM_BBS_PGM*/
/*			,COALESCE(LNXX.LN_BBS_STS_PCV_PAY,X) AS LN_BBS_STS_PCV_PAY*/
/*			,RPXX.PN_BBT_DLQ_MOT*/
/*			,LNXX.LC_STA_LNXX*/
/*			,LNXX.LD_BBS_DSQ*/
/*			,LNXX.LC_BBS_ELG */
/*		FROM  PKUB.LNXX_LON LNXX*/
/*			INNER JOIN PKUB.RPXX_EFT_RIR_PAR RPXX*/
/*				ON LNXX.LF_LON_CUR_OWN = RPXX.IF_OWN*/
/*				AND LNXX.IC_LON_PGM = RPXX.IC_LON_PGM*/
/*			INNER JOIN PKUB.LNXX_LON_BBS LNXX*/
/*				ON LNXX.BF_SSN = LNXX.BF_SSN*/
/*				AND LNXX.LN_SEQ = LNXX.LN_SEQ*/
/*			INNER JOIN PKUB.LNXX_LON_BBS_TIR LNXX*/
/*				ON LNXX.BF_SSN = LNXX.BF_SSN*/
/*				AND LNXX.LN_SEQ = LNXX.LN_SEQ*/
/*			INNER JOIN PKUB.RPXX_BBS_PGM_TIR RPXX*/
/*				ON LNXX.PM_BBS_PGM = RPXX.PM_BBS_PGM */
/*				AND LNXX.PN_BBS_PGM_SEQ = RPXX.PN_BBS_PGM_SEQ*/
/*		WHERE RPXX.PC_EFT_RIR_STA = 'A'*/
/*			AND RPXX.PC_STA_RPXX = 'A'*/
/*			AND LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/*FOR READ ONLY WITH UR*/
/*);*/
/*DISCONNECT FROM DBX;*/
/*QUIT;*/
/*PROC SORT DATA=LNXX; BY BF_SSN LN_SEQ LC_STA_LNXX ; RUN;*/
/*DATA LNXX(drop=LC_STA_LNXX);*/
/*	SET LNXX;*/
/*	BY BF_SSN LN_SEQ LC_STA_LNXX ;*/
/*	IF FIRST.LN_SEQ;*/
/*RUN;*/
/*%SSNXACC(LNXX,LN_SEQ);*/
/**/
/*/********************************************/
/** REPAYMENT SCHEDULE DATA*/
/*********************************************/*/
/*PROC SQL;*/
/*	CREATE TABLE LNXX AS*/
/*		SELECT */
/*			LNXX.BF_SSN*/
/*			,LNXX.LN_SEQ*/
/*			,LNXX.LN_GRD_RPS_SEQ*/
/*			,CASE*/
/*				WHEN DWXX.WX_OVR_DW_LON_STA = 'LITIGATION' THEN X*/
/*				ELSE LNXX.LN_RPS_TRM*/
/*			END AS LN_RPS_TRM*/
/*			,CASE*/
/*				WHEN DWXX.WX_OVR_DW_LON_STA = 'LITIGATION' THEN .*/
/*				ELSE RSXX.LD_RPS_X_PAY_DU*/
/*			END AS LD_RPS_X_PAY_DU*/
/*			,RSXX.LD_SNT_RPD_DIS*/
/*			,LNXX.LD_CRT_LONXX*/
/*			,LNXX.LC_TYP_SCH_DIS*/
/*			,CASE*/
/*				WHEN DWXX.WX_OVR_DW_LON_STA = 'LITIGATION' THEN X*/
/*				ELSE LNXX.LA_RPS_ISL*/
/*			END AS LA_RPS_ISL*/
/*		FROM*/
/*			PKUB.RSXX_BR_RPD RSXX*/
/*			JOIN PKUB.LNXX_LON_RPS LNXX*/
/*				ON RSXX.BF_SSN = LNXX.BF_SSN*/
/*				AND RSXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ*/
/*			JOIN PKUB.LNXX_LON_RPS_SPF LNXX*/
/*				ON LNXX.BF_SSN = LNXX.BF_SSN*/
/*				AND LNXX.LN_SEQ = LNXX.LN_SEQ*/
/*				AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ*/
/*			JOIN PKUB.DWXX_DW_CLC_CLU DWXX*/
/*				ON LNXX.BF_SSN = DWXX.BF_SSN*/
/*				AND LNXX.LN_SEQ = DWXX.LN_SEQ*/
/*		WHERE */
/*			LNXX.LC_STA_LONXX = 'A'*/
/*	;*/
/*QUIT;*/
/**/
/*PROC SORT DATA=LNXX; BY BF_SSN LN_SEQ LN_GRD_RPS_SEQ; RUN;*/
/**/
/*DATA LNXX(DROP= A B C NEXT_SEQ LD_RPS_X_PAY_DU LN_GRD_RPS_SEQ);*/
/*	SET LNXX;*/
/*	FORMAT NEXT_SEQ DATEX.;*/
/*	BY BF_SSN LN_SEQ LN_GRD_RPS_SEQ;*/
/*	RETAIN NEXT_SEQ A C B;*/
/*	IF FIRST.LN_SEQ THEN DO;*/
/*		A = .;*/
/*		NEXT_SEQ = INTNX('MONTH',LD_RPS_X_PAY_DU,LN_RPS_TRM,'S');*/
/*		IF NEXT_SEQ > TODAY() THEN DO;*/
/*			A = LN_GRD_RPS_SEQ ;*/
/*			C = LA_RPS_ISL ;*/
/*		END;*/
/*	END;*/
/*	ELSE IF A= . THEN DO;*/
/*		NEXT_SEQ = INTNX('MONTH',NEXT_SEQ,LN_RPS_TRM,'S');*/
/*		IF NEXT_SEQ > TODAY() THEN DO;*/
/*			A = LN_GRD_RPS_SEQ ;*/
/*			C = LA_RPS_ISL ;*/
/*		END;*/
/*	END;*/
/*	IF FIRST.LN_SEQ THEN B = LN_RPS_TRM;*/
/*	ELSE B = B + LN_RPS_TRM;*/
/*	IF LAST.LN_SEQ THEN DO;*/
/*		LN_RPS_TRM = B;*/
/*		LA_RPS_ISL = C;*/
/*		LN_GRD_RPS_SEQ = A;*/
/*		DAY_DUE = DAY(LD_RPS_X_PAY_DU);*/
/*		OUTPUT;*/
/*	END;*/
/*RUN;*/
/*%SSNXACC(LNXX,LN_SEQ);*/
/**/
/*/********************************************/
/** INTEREST RATE DATA*/
/*********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE LNXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT */
/*			LNXX.BF_SSN,*/
/*			LNXX.LN_SEQ,*/
/*			LNXX.LD_ITR_EFF_BEG,*/
/*			LNXX.LD_ITR_EFF_END,*/
/*			COALESCE(LNXX.LR_ITR,X) AS LR_ITR,*/
/*			COALESCE(LNXX.LR_INT_RDC_PGM_ORG,LNXX.LR_ITR) AS LR_INT_RDC_PGM_ORG,*/
/*			LNXX.LC_ITR_TYP,*/
/*			LNXX.LD_ITR_EFF_BEG,*/
/*			LNXX.LD_ITR_EFF_END*/
/*		FROM*/
/*			PKUB.LNXX_INT_RTE_HST LNXX*/
/*		WHERE*/
/*			LNXX.LC_STA_LONXX = 'A'*/
/*			AND LNXX.LD_ITR_EFF_BEG <= Current Date*/
/*			AND LNXX.LD_ITR_EFF_END >= Current Date*/
/*			AND LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/**/
/*	FOR READ ONLY WITH UR*/
/*	);*/
/*DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(LNXX,LN_SEQ);*/
/**/
/*/**********************************************/
/** ENROLLMENT - LOAN LEVEL*/
/***********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE SDXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*	SELECT LNXX.BF_SSN*/
/*		,LNXX.LN_SEQ*/
/*		,SDXX.LN_STU_SPR_SEQ*/
/*		,SDXX.LD_SCL_SPR*/
/*		,SCXX.IM_SCL_FUL*/
/*	FROM PKUB.SDXX_STU_SPR SDXX*/
/*		INNER JOIN PKUB.SCXX_SCH_DMO SCXX*/
/*			ON SDXX.LF_DOE_SCL_ENR_CUR = SCXX.IF_DOE_SCL*/
/*		INNER JOIN PKUB.LNXX_LON_STU_OSD LNXX*/
/*			ON SDXX.LF_STU_SSN = LNXX.LF_STU_SSN*/
/*			AND SDXX.LN_STU_SPR_SEQ = LNXX.LN_STU_SPR_SEQ*/
/*		INNER JOIN PKUB.LNXX_LON LNXX*/
/*			ON LNXX.BF_SSN = LNXX.BF_SSN*/
/*			AND LNXX.LN_SEQ = LNXX.LN_SEQ*/
/*	WHERE SDXX.LC_STA_STUXX = 'A'*/
/*		AND LNXX.LC_STA_LONXX = 'A'*/
/*		AND (SDXX.LF_LST_DTS_SDXX >= &LAST_RUNPASS*/
/*			OR LNXX.LF_CRT_DTS_LNXX >= &LAST_RUNPASS)*/
/**/
/*	FOR READ ONLY WITH UR*/
/*	);*/
/*DISCONNECT FROM DBX;*/
/*QUIT;*/
/*PROC SORT DATA=SDXX; BY BF_SSN LN_SEQ LN_STU_SPR_SEQ;RUN;*/
/*DATA SDXX(DROP=LN_STU_SPR_SEQ);*/
/*	SET SDXX;*/
/*		BY BF_SSN LN_SEQ LN_STU_SPR_SEQ;*/
/*	IF LAST.LN_SEQ;*/
/*RUN;*/
/*%SSNXACC(SDXX,LN_SEQ);*/
/**/
/*/**********************************************/
/** DELINQUENCY DATA*/
/***********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE LNXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT */
/*			LNXX.BF_SSN,*/
/*			LNXX.LN_SEQ,*/
/*			LNXX.LN_DLQ_SEQ,*/
/*			LNXX.LD_DLQ_OCC,*/
/*			CASE WHEN LNXX.LC_STA_LONXX ^= 'X' OR (LNXX.LC_STA_LONXX = 'X' AND LNXX.LD_DLQ_MAX ^= Current Date - X Day) THEN X ELSE LNXX.LN_DLQ_MAX END AS LN_DLQ_MAX,*/
/*			CASE WHEN LNXX.LC_STA_LONXX ^= 'X' OR (LNXX.LC_STA_LONXX = 'X' AND LNXX.LD_DLQ_MAX ^= Current Date - X Day) THEN NULL ELSE LNXX.LD_DLQ_MAX END AS LD_DLQ_MAX*/
/*		FROM*/
/*			PKUB.LNXX_LON_DLQ_HST LNXX*/
/*		WHERE*/
/*			LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/**/
/**/
/*	FOR READ ONLY WITH UR*/
/*	);*/
/*DISCONNECT FROM DBX;*/
/*QUIT;*/
/*PROC SORT DATA=LNXX; BY BF_SSN LN_SEQ DESCENDING LN_DLQ_SEQ; RUN;*/
/*DATA LNXX ;*/
/*	SET LNXX(DROP=LN_DLQ_SEQ);*/
/*	BY BF_SSN LN_SEQ;*/
/*	IF FIRST.LN_SEQ;*/
/*RUN;*/
/*%SSNXACC(LNXX,LN_SEQ);*/
/**/
/*/**********************************************/
/** AUTOPAY DATA*/
/***********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE BRXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT*/
/*			BRXX.BF_SSN,*/
/*			BRXX.BN_EFT_SEQ,*/
/*			BRXX.BF_EFT_ABA,*/
/*			BRXX.BF_EFT_ACC,*/
/*			BRXX.BC_EFT_STA,*/
/*			BRXX.BD_EFT_STA,*/
/*			COALESCE(BRXX.BA_EFT_ADD_WDR,X) AS BA_EFT_ADD_WDR,*/
/*			BRXX.BN_EFT_NSF_CTR,*/
/*			BRXX.BC_EFT_DNL_REA*/
/*		FROM*/
/*			PKUB.BRXX_BR_EFT BRXX*/
/*		WHERE*/
/*			BF_LST_DTS_BRXX >= &LAST_RUNPASS*/
/**/
/*	FOR READ ONLY WITH UR*/
/*	);*/
/*DISCONNECT FROM DBX;*/
/*QUIT;*/
/*PROC SORT DATA=BRXX; BY BF_SSN BC_EFT_STA DESCENDING BN_EFT_SEQ; RUN;*/
/*DATA BRXX; */
/*	SET BRXX; */
/*	BY BF_SSN BC_EFT_STA DESCENDING BN_EFT_SEQ;*/
/*	IF FIRST.BF_SSN OR BC_EFT_STA = 'A';*/
/*RUN;*/
/*PROC SORT DATA=BRXX; BY BF_SSN BN_EFT_SEQ; RUN;*/
/*DATA BRXX(DROP=B);*/
/*	SET BRXX;*/
/*	BY BF_SSN;*/
/*	RETAIN B;*/
/*	IF FIRST.BF_SSN AND LAST.BF_SSN THEN OUTPUT;*/
/*	ELSE IF FIRST.BF_SSN AND LAST.BF_SSN = X AND BC_EFT_STA = 'A' THEN B = BA_EFT_ADD_WDR;*/
/*	ELSE IF FIRST.BF_SSN = X AND BC_EFT_STA = 'A' THEN B = B + BA_EFT_ADD_WDR;*/
/*	IF FIRST.BF_SSN = X AND LAST.BF_SSN THEN DO;*/
/*		BA_EFT_ADD_WDR = B;*/
/*		OUTPUT; */
/*	END;*/
/*RUN;*/
/*%SSNXACC(BRXX,);*/
/**/
/*/**********************************************/
/** DEMOGRAPHICS*/
/***********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE PDXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT DISTINCT */
/*			PDXX.DF_SPE_ACC_ID*/
/*			,PDXX.DF_PRS_ID AS BF_SSN*/
/*			,PDXX.DM_PRS_X*/
/*			,PDXX.DM_PRS_LST*/
/*			,PDXX.DM_PRS_MID*/
/*			,PDXX.DD_BRT*/
/*		FROM */
/*			PKUB.PDXX_PRS_NME PDXX*/
/*		WHERE */
/*			SUBSTR(DF_PRS_ID,X,X) <> 'P'*/
/*			AND PDXX.DF_LST_DTS_PDXX >= &LAST_RUNPASS*/
/**/
/*		FOR READ ONLY WITH UR*/
/*		);*/
/*DISCONNECT FROM DBX;*/
/*QUIT;*/
/**/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE PDXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT DISTINCT */
/*			PDXX.DF_PRS_ID AS BF_SSN*/
/*			,PDXX.DX_STR_ADR_X*/
/*			,PDXX.DX_STR_ADR_X*/
/*			,PDXX.DM_CT*/
/*			,PDXX.DC_DOM_ST*/
/*			,PDXX.DF_ZIP_CDE*/
/*			,PDXX.DM_FGN_ST*/
/*			,PDXX.DM_FGN_CNY*/
/*			,PDXX.DD_VER_ADR*/
/*			,PDXX.DI_VLD_ADR*/
/*		FROM */
/*			PKUB.PDXX_PRS_ADR PDXX*/
/*		WHERE */
/*			PDXX.DC_ADR = 'L'*/
/*			AND PDXX.DF_LST_DTS_PDXX >= &LAST_RUNPASS*/
/**/
/*		FOR READ ONLY WITH UR*/
/*		);*/
/*DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(PDXX,);*/
/**/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE PDXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT DISTINCT */
/*			PDXX.DF_PRS_ID AS BF_SSN*/
/*			,PDXX.DC_PHN*/
/*			,PDXX.DC_ALW_ADL_PHN*/
/*			,PDXX.DD_PHN_VER*/
/*			,PDXX.DI_PHN_VLD*/
/*			,PDXX.DN_DOM_PHN_ARA*/
/*			,PDXX.DN_DOM_PHN_XCH*/
/*			,PDXX.DN_DOM_PHN_LCL*/
/*			,PDXX.DN_PHN_XTN*/
/*			,PDXX.DN_FGN_PHN_CNY*/
/*			,PDXX.DN_FGN_PHN_CT*/
/*			,PDXX.DN_FGN_PHN_LCL*/
/*		FROM  */
/*			PKUB.PDXX_PRS_PHN PDXX*/
/*		WHERE */
/*			PDXX.DF_LST_DTS_PDXX >= &LAST_RUNPASS*/
/**/
/*		FOR READ ONLY WITH UR*/
/*		);*/
/*DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(PDXX,DC_PHN);*/
/**/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE PDXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT DISTINCT */
/*			PDXX.DF_PRS_ID AS BF_SSN*/
/*			,PDXX.DC_ADR_EML*/
/*			,PDXX.DX_ADR_EML */
/*			,PDXX.DD_VER_ADR_EML*/
/*			,PDXX.DI_VLD_ADR_EML*/
/*			,PDXX.DC_STA_PDXX*/
/*		FROM */
/*			PKUB.PDXX_PRS_ADR_EML PDXX*/
/*		WHERE */
/*			PDXX.DF_LST_DTS_PDXX >= &LAST_RUNPASS*/
/*			AND PDXX.DC_STA_PDXX = 'A'*/
/**/
/*		FOR READ ONLY WITH UR*/
/*		);*/
/*DISCONNECT FROM DBX;*/
/*QUIT;*/
/*PROC SORT DATA=PDXX; BY BF_SSN DC_ADR_EML DC_STA_PDXX ; RUN;*/
/*	DATA PDXX(DROP=DC_STA_PDXX);*/
/*	SET PDXX;*/
/*	BY BF_SSN DC_ADR_EML;*/
/*	IF FIRST.DC_ADR_EML;*/
/*RUN;*/
/*%SSNXACC(PDXX,DC_ADR_EML);*/
/**/
/*/**********************************************/
/** SUSPENSE PAYMENTS*/
/***********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE RMXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT DISTINCT */
/*			RMXX.BF_SSN*/
/*			,SUM(RMXX.LA_BR_RMT_PST) AS LA_BR_RMT_PST*/
/*		FROM */
/*			PKUB.RMXX_BR_RMT_PST RMXX*/
/*		WHERE */
/*			RMXX.LC_RMT_STA_PST = 'S'*/
/*		GROUP BY BF_SSN*/
/**/
/*	FOR READ ONLY WITH UR*/
/*	);*/
/*DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(RMXX,);*/
/**/
/*/**********************************************/
/** AMOUNTS DUE INFORMATION*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE CUR_DUE AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DBX */
/*				(*/
/*					SELECT DISTINCT*/
/*						LNXX.BF_SSN*/
/*						,SUM(CUR.CUR_DUE) AS CUR_DUE*/
/*						,SUM(PST.PAST_DUE) AS PAST_DUE*/
/*						,SUM(COALESCE(CUR.CUR_DUE,X) + COALESCE(PST.PAST_DUE,X)) AS TOT_DUE*/
/*						,SUM(COALESCE(CUR.CUR_DUE,X) + COALESCE(PST.PAST_DUE,X) + COALESCE(LNXX.LA_LTE_FEE_OTS,X)) AS TOT_DUE_FEE*/
/*					FROM PKUB.LNXX_LON LNXX*/
/*						LEFT JOIN (*/
/*							SELECT*/
/*								LNXX.BF_SSN*/
/*								,lnXX.ln_seq*/
/*								,SUM(COALESCE(LNXX.LA_BIL_CUR_DU,X) - COALESCE(LNXX.LA_TOT_BIL_STS,X)) AS CUR_DUE*/
/*							FROM PKUB.LNXX_LON_BIL_CRF LNXX*/
/*								JOIN (*/
/*										SELECT*/
/*											LNXX.BF_SSN*/
/*											,lnXX.ln_seq*/
/*											,MIN(LNXX.LD_BIL_DU_LON) AS LD_BIL_DU_LON*/
/*										FROM PKUB.LNXX_LON_BIL_CRF LNXX*/
/*										WHERE LNXX.LC_STA_LONXX = 'A'*/
/*											AND LNXX.LC_LON_STA_BIL = 'X'*/
/*											AND LNXX.LD_BIL_DU_LON > CURRENT_DATE*/
/*										GROUP BY LNXX.BF_SSN*/
/*												,lnXX.ln_seq*/
/*											) MIN_DTE*/
/*									ON LNXX.BF_SSN = MIN_DTE.BF_SSN*/
/*										and lnXX.ln_seq = min_dte.ln_seq*/
/*										AND LNXX.LD_BIL_DU_LON = MIN_DTE.LD_BIL_DU_LON*/
/*							WHERE LNXX.LC_STA_LONXX = 'A'*/
/*								AND LNXX.LC_LON_STA_BIL = 'X'*/
/*							GROUP BY LNXX.BF_SSN*/
/*									,lnXX.ln_seq*/
/*									) CUR*/
/*							ON LNXX.BF_SSN = CUR.BF_SSN*/
/*							and lnXX.ln_seq = cur.ln_seq*/
/*						LEFT JOIN (*/
/*							SELECT*/
/*								LNXX.BF_SSN*/
/*								,lnXX.ln_seq*/
/*								,SUM(COALESCE(LNXX.LA_BIL_CUR_DU,X) - COALESCE(LNXX.LA_TOT_BIL_STS,X)) AS PAST_DUE*/
/*								,sum(lnXX.la_bil_cur_du) as la_bil_cur_du*/
/*								,sum(lnXX.la_tot_bil_sts) as la_tot_bil_sts*/
/*							FROM PKUB.LNXX_LON_BIL_CRF LNXX*/
/*							WHERE LNXX.LC_STA_LONXX = 'A'*/
/*								AND LNXX.LC_LON_STA_BIL = 'X'*/
/*								AND LNXX.LD_BIL_DU_LON <= CURRENT_DATE*/
/*							GROUP BY LNXX.BF_SSN*/
/*									,lnXX.ln_seq*/
/*									) PST*/
/*							ON LNXX.BF_SSN = PST.BF_SSN*/
/*							and lnXX.ln_seq = pst.ln_seq*/
/*					WHERE lnXX.la_cur_pri > X*/
/*						and lnXX.lc_sta_lonXX = 'R'*/
/*/*						and	LNXX.BF_SSN like '%XXXX'*/*/
/*					GROUP BY LNXX.BF_SSN*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(CUR_DUE,);*/
/**/
/*/**********************************************/
/** ADDITIONAL LOAN DATA*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE DWXX AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DBX */
/*				(*/
/*				SELECT*/
/*					DWXX.BF_SSN,*/
/*					DWXX.LN_SEQ,*/
/*					DWXX.WC_DW_LON_STA,*/
/*					DWXX.WA_TOT_BRI_OTS,*/
/*					DWXX.WD_LON_RPD_SR,*/
/*					DWXX.WX_OVR_DW_LON_STA*/
/*				FROM*/
/*					PKUB.DWXX_DW_CLC_CLU DWXX*/
/**/
/*				FOR READ ONLY WITH UR*/
/*				);*/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(DWXX,LN_SEQ);*/
/**/
/*/**********************************************/
/** CREDIT REPORTING INFORMATION*/
/***********************************************/*/
/*PROC SQL;*/
/*CREATE TABLE LNXX AS*/
/*	SELECT LNXX.BF_SSN */
/*		,LNXX.LN_SEQ*/
/*		,LNXX.LC_RPT_STA_CRB*/
/*		,LNXX.LD_RPT_CRB*/
/*		,LNXXB.LD_STA_LNXX AS DT_ADJ*/
/*	FROM pkus.LNXX_LON_CRB_RPT LNXX*/
/*		LEFT OUTER JOIN pkus.LNXX_LON_CRB_RPT LNXXB*/
/*			ON LNXX.BF_SSN = LNXXB.BF_SSN*/
/*			AND LNXX.LN_SEQ = LNXXB.LN_SEQ*/
/*			AND LNXX.LD_RPT_CRB = LNXXB.LD_RPT_CRB*/
/*			AND SUBSTR(LNXX.LF_LST_USR_LNXX,X,X) = 'UT'*/
/*			AND LNXXB.LC_STA_LNXX = 'I'*/
/*	WHERE LNXX.LD_RPT_CRB > INTNX('YEAR',TODAY(),-X,'S')*/
/*		AND LNXX.LC_STA_LNXX = 'A'*/
/*		AND DATEPART(LNXX.LF_LST_DTS_LNXX) >= &LAST_RUN */
/*;*/
/*QUIT;*/
/*%SSNXACC(LNXX,LN_SEQ);*/
/**/
/*/**********************************************/
/** ACTIVITY HISTORY*/
/***********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*	CREATE TABLE ARCS AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT AYXX.BF_SSN*/
/*			,AYXX.LN_ATY_SEQ*/
/*			,AYXX.PF_REQ_ACT*/
/*			,AYXX.LC_STA_ACTYXX*/
/*			,LX_ATY*/
/*		FROM PKUB.AYXX_BR_LON_ATY AYXX*/
/*			LEFT OUTER JOIN PKUB.AYXX_ATY_CMT AYXX*/
/*				ON AYXX.BF_SSN = AYXX.BF_SSN */
/*				AND AYXX.LN_ATY_SEQ = AYXX.LN_ATY_SEQ*/
/*				AND AYXX.LN_ATY_CMT_SEQ = X*/
/*		 	LEFT OUTER JOIN PKUB.AYXX_ATY_TXT AYXX*/
/*				ON AYXX.BF_SSN = AYXX.BF_SSN */
/*				AND AYXX.LN_ATY_SEQ = AYXX.LN_ATY_SEQ*/
/*				AND AYXX.LN_ATY_CMT_SEQ = AYXX.LN_ATY_CMT_SEQ*/
/*		WHERE AYXX.PF_REQ_ACT IN ('DRLFA','KXADD','KXPHN','MXXXX')*/
/*			AND AYXX.LC_STA_ACTYXX = 'A'*/
/*			AND (AYXX.LF_LST_DTS_AYXX >= &LAST_RUNPASS OR AYXX.PF_REQ_ACT = 'DRLFA')*/
/*FOR READ ONLY WITH UR*/
/*);*/
/**/
/*CREATE TABLE ARC_IND AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT AYXX.BF_SSN*/
/*			,AYXX.LN_ATY_SEQ*/
/*			,AYXX.PF_REQ_ACT*/
/*			,AYXX.LC_STA_ACTYXX*/
/*		FROM PKUB.AYXX_BR_LON_ATY AYXX*/
/*		WHERE AYXX.PF_REQ_ACT IN ('SPHAN','VIPSS')*/
/*			AND AYXX.LC_STA_ACTYXX = 'A'*/
/*			AND AYXX.LF_LST_DTS_AYXX >= &LAST_RUNPASS*/
/*FOR READ ONLY WITH UR*/
/*);*/
/**/
/*CREATE TABLE DLXXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT AYXX.BF_SSN*/
/*			,LN_ATY_SEQ*/
/*			,LD_ATY_REQ_RCV*/
/*			,LC_STA_ACTYXX*/
/*		FROM PKUB.AYXX_BR_LON_ATY AYXX*/
/*		WHERE PF_REQ_ACT = 'DLXXX'*/
/*			AND AYXX.LF_LST_DTS_AYXX >= &LAST_RUNPASS*/
/*);*/
/**/
/*CREATE TABLE ARCHIST AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT AYXX.BF_SSN*/
/*			,AYXX.LN_ATY_SEQ*/
/*			,AYXX.LN_ATY_CMT_SEQ*/
/*			,AYXX.LC_STA_ACTYXX*/
/*			,AYXX.PF_REQ_ACT*/
/*			,AYXX.PF_RSP_ACT*/
/*			,AYXX.LD_ATY_REQ_RCV*/
/*			,AYXX.LD_ATY_RSP*/
/*			,AYXX.LF_USR_REQ_ATY*/
/*			,AYXX.LT_ATY_RSP*/
/*			,AYXX.LC_STA_AYXX*/
/*			,COALESCE(AYXX.LX_ATY, ' ') AS LX_ATY_X*/
/*			,ACXX.PX_ACT_DSC_REQ*/
/*				,LC_ATY_RCP*/
/*			,CASE 	*/
/*				WHEN AYXX.LF_ATY_RCP = AYXX.BF_SSN THEN ''*/
/*				ELSE AYXX.LF_ATY_RCP*/
/*			END AS LF_ATY_RCP*/
/*	FROM PKUB.AYXX_BR_LON_ATY AYXX*/
/*		LEFT OUTER JOIN PKUB.AYXX_ATY_CMT AYXX*/
/*			ON AYXX.BF_SSN = AYXX.BF_SSN */
/*			AND AYXX.LN_ATY_SEQ = AYXX.LN_ATY_SEQ*/
/*		LEFT OUTER JOIN PKUB.AYXX_ATY_TXT AYXX*/
/*			ON AYXX.BF_SSN = AYXX.BF_SSN */
/*			AND AYXX.LN_ATY_SEQ = AYXX.LN_ATY_SEQ*/
/*			AND AYXX.LN_ATY_CMT_SEQ = AYXX.LN_ATY_CMT_SEQ*/
/*			AND AYXX.LN_ATY_TXT_SEQ = X*/
/*		LEFT OUTER JOIN PKUB.ACXX_ACT_REQ ACXX*/
/*			ON AYXX.PF_REQ_ACT = ACXX.PF_REQ_ACT*/
/*	WHERE AYXX.LC_ATY_RCP IN ('B','R')*/
/*		AND AYXX.LF_LST_DTS_AYXX >= &LAST_RUNPASS*/
/*);*/
/*DISCONNECT FROM DBX;*/
/*QUIT;*/
/*PROC SORT DATA=ARCHIST; BY BF_SSN LN_ATY_SEQ LN_ATY_CMT_SEQ; RUN;*/
/*DATA ARCHIST;*/
/*	SET ARCHIST; */
/*	BY BF_SSN LN_ATY_SEQ;*/
/*	LENGTH LX_ATY $XXXX.;*/
/*	RETAIN LX_ATY;*/
/**/
/*	IF FIRST.LN_ATY_SEQ THEN LX_ATY=' '; */
/**/
/*	IF PF_RSP_ACT = 'CNTCT' THEN*/
/*	DO;*/
/*		LX_ATY=trim(left(LX_ATY))||trim(left(LX_ATY_X));*/
/*		IF LAST.LN_ATY_SEQ;*/
/*	END;*/
/*	ELSE DO;*/
/*		LX_ATY = LX_ATY_X;*/
/*		IF FIRST.LN_ATY_SEQ;*/
/*	END;*/
/*RUN;*/
/**/
/*DATA DRLFA ARCS(DROP=DOL) ;*/
/*	SET ARCS;*/
/*	IF PF_REQ_ACT = 'DRLFA' THEN DO;*/
/*		IF SUBSTR(LX_ATY,X,X) = '{' and lc_sta_actyXX = 'A' THEN DO;*/
/*			DOL = INPUT(SCAN(LX_ATY,X,'{}'),DOLLARX.X);*/
/*			OUTPUT DRLFA;*/
/*		END;*/
/*	END;*/
/*	ELSE OUTPUT ARCS;*/
/*RUN;*/
/**/
/*PROC SORT DATA=DRLFA; BY BF_SSN;RUN;*/
/*DATA DRLFA (KEEP=BF_SSN FEE_WAV_DOL FEE_WAV_CT);*/
/*	SET DRLFA;*/
/*	RETAIN FEE_WAV_DOL FEE_WAV_CT;*/
/*	BY BF_SSN;*/
/*	IF FIRST.BF_SSN THEN DO;*/
/*		FEE_WAV_DOL = DOL;*/
/*		FEE_WAV_CT = X;*/
/*	END;*/
/*	ELSE DO;*/
/*		FEE_WAV_DOL + DOL;*/
/*		FEE_WAV_CT + X;*/
/*	END;*/
/*	IF LAST.BF_SSN THEN OUTPUT;	*/
/*RUN;*/
/**/
/*DATA KXADD(KEEP=BF_SSN LN_ATY_SEQ LC_STA_ACTYXX DX_STR_ADR_X DX_STR_ADR_X DM_CT DC_DOM_ST DF_ZIP_CDE DM_FGN_CNY COMMENTS)*/
/*	KXPHN(KEEP=BF_SSN LN_ATY_SEQ LC_STA_ACTYXX PHNX PHNX PHNX COMMENTS)*/
/*	ARC_MXXXX(KEEP=BF_SSN LN_ATY_SEQ LC_STA_ACTYXX LX_ATY);*/
/*	SET ARCS;*/
/*	LENGTH DX_STR_ADR_X $XX. DX_STR_ADR_X $XX. DM_CT $XX. DC_DOM_ST $X. DF_ZIP_CDE $XX. DM_FGN_CNY $XX. COMMENTS $XXX.;*/
/*	LENGTH  PHNX PHNX PHNX $XX.;*/
/*	ARRAY ADR{X} $ DX_STR_ADR_X DX_STR_ADR_X DM_CT DC_DOM_ST DF_ZIP_CDE DM_FGN_CNY COMMENTS;*/
/*	ARRAY PHN{X} $ PHNX PHNX PHNX COMMENTS;*/
/*	IF PF_REQ_ACT = 'KXADD' THEN DO;*/
/*		DO I = X TO X;*/
/*			ADR(I) = SCAN(LX_ATY,I,',');*/
/*		END;*/
/*		OUTPUT KXADD;*/
/*	END;*/
/*	ELSE if pf_req_act = 'KXPHN' THEN DO;*/
/*		DO I = X TO X;*/
/*			PHN(I) = SCAN(LX_ATY,I,',');*/
/*		END;*/
/*		OUTPUT KXPHN;*/
/*	END;*/
/*	ELSE IF PF_REQ_ACT = 'MXXXX' THEN OUTPUT ARC_MXXXX;*/
/*RUN;*/
/**/
/**/
/*%SSNXACC(KXADD,LN_ATY_SEQ);*/
/*%SSNXACC(KXPHN,LN_ATY_SEQ);*/
/*%SSNXACC(ARC_MXXXX,LN_ATY_SEQ);*/
/**/
/*%SSNXACC(DLXXX,LN_ATY_SEQ);*/
/*%SSNXACC(DRLFA,);*/
/*%SSNXACC(ARCHIST,LN_ATY_SEQ); */
/*%SSNXACC(ARC_IND,LN_ATY_SEQ); */
/**/
/*/**********************************************/
/** FINANCIAL HISTORY*/
/***********************************************/*/
/**/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE LNXX_HIST AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT*/
/*			LNXX_HIST.BF_SSN,*/
/*			LNXX_HIST.LN_SEQ,*/
/*			LNXX_HIST.LN_FAT_SEQ,*/
/*			LNXX_HIST.LC_FAT_REV_REA,*/
/*			LNXX_HIST.LD_FAT_APL,*/
/*			LNXX_HIST.LD_FAT_PST,*/
/*			LNXX_HIST.LD_FAT_EFF,*/
/*			LNXX_HIST.LD_STA_LONXX,*/
/*			LNXX_HIST.LC_STA_LONXX,*/
/*			LNXX_HIST.LA_FAT_PCL_FEE,*/
/*			LNXX_HIST.LA_FAT_NSI,*/
/*			LNXX_HIST.LA_FAT_LTE_FEE,*/
/*			LNXX_HIST.LA_FAT_ILG_PRI,*/
/*			LNXX_HIST.LA_FAT_CUR_PRI,*/
/*			LNXX_HIST.PC_FAT_TYP,*/
/*			LNXX_HIST.PC_FAT_SUB_TYP,*/
/*			LNXX_HIST.LA_FAT_NSI_ACR*/
/*		FROM*/
/*			PKUB.LNXX_FIN_ATY LNXX_HIST*/
/*		WHERE */
/*			LNXX_HIST.LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/**/
/*	FOR READ ONLY WITH UR*/
/*	);*/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(LNXX_HIST,LN_SEQ LN_FAT_SEQ);*/
/**/
/*/**********************************************/
/** LTXX LETTER REQUESTS*/
/***********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE LTXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT*/
/*			RF_SBJ_PRC AS BF_SSN, */
/*			RT_RUN_SRT_DTS_PRC,*/
/*			RN_SEQ_LTR_CRT_PRC, */
/*			RN_SEQ_REC_PRC, */
/*			RM_DSC_LTR_PRC, */
/*			RC_TYP_SBJ_PRC,*/
/*			RF_SBJ_PRC ,*/
/*			RN_ENT_REQ_PRC,*/
/*			RN_ATY_SEQ_PRC, */
/*			RI_REC_PRC, */
/*			RX_REQ_ARA_X_PRC,*/
/*			RI_LTR_REQ_DEL_PRC, */
/*			RC_LTR_REQ_SRC_PRC, */
/*			RI_PRV_RUN_ERR_PRC, */
/*			RF_COR_DOC_PRC, */
/*			RI_LTR_OPT_ENC_PRC */
/*		FROM*/
/*			PKUB.LTXX_LTR_REQ_PRC*/
/*		WHERE*/
/*			RT_RUN_SRT_DTS_PRC >= &LAST_RUNPASS*/
/*			AND RI_LTR_REQ_DEL_PRC <> 'Y'*/
/**/
/*	FOR READ ONLY WITH UR*/
/*	);*/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(LTXX, RT_RUN_SRT_DTS_PRC RN_SEQ_LTR_CRT_PRC RN_SEQ_REC_PRC);*/
/**/
/*/**********************************************/
/** BORROWER BENEFIT PROGRAM TIERS*/
/***********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE RPXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT*/
/*			'XXXXXXXXXX' AS DF_SPE_ACC_ID, /*required by .L load job common macros*/*/
/*			PM_BBS_PGM,*/
/*			PN_BBS_PGM_SEQ,*/
/*			PF_BBS_PGM_TIR,*/
/*			PN_BBS_PGM_TIR_SEQ,*/
/*			PC_BBT_ICV,*/
/*			PN_BBT_PAY_ICV,*/
/*			PR_BBT_RDC,*/
/*			PC_BBT_REB_APL,*/
/*			PA_BBT_REB*/
/*		FROM*/
/*			PKUB.RPXX_BBS_PGM_TIR RPXX*/
/*		WHERE*/
/*			RPXX.PF_LST_DTS_RPXX >= &LAST_RUNPASS*/
/**/
/*	FOR READ ONLY WITH UR*/
/*	);*/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/**/
/*/**********************************************/
/** LOAN BORROWER BENEFIT PROGRAM TIERS*/
/***********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE LNXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT*/
/*			LNXX.BF_SSN,*/
/*			LNXX.LN_SEQ,*/
/*			LNXX.PM_BBS_PGM,*/
/*			LNXX.PN_BBS_PGM_SEQ,*/
/*			LNXX.LN_LON_BBS_PGM_SEQ,*/
/*			LNXX.LD_EFF_BEG_LNXX,*/
/*			LNXX.LI_BBS_ITD_LTR_SNT,*/
/*			LNXX.LN_BBS_STS_PCV_PAY,*/
/*			LNXX.LC_BBS_REB_MTD,*/
/*			LNXX.LC_STA_LNXX,*/
/*			LNXX.LD_STA_LNXX,*/
/*			LNXX.LC_BBS_DSQ_REA,*/
/*			LNXX.LD_BBS_DSQ,*/
/*			LNXX.LC_BBS_ELG*/
/*		FROM*/
/*			PKUB.LNXX_LON_BBS LNXX*/
/*		WHERE*/
/*			LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/**/
/*	FOR READ ONLY WITH UR*/
/*	);*/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(LNXX,LN_SEQ PM_BBS_PGM PN_BBS_PGM_SEQ LN_LON_BBS_PGM_SEQ);*/
/**/
/*/**********************************************/
/** FINANCIAL ACTIVITY ADJUSTMENT*/
/***********************************************/*/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE ADXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX (*/
/*		SELECT*/
/*			BF_SSN,*/
/*			LD_FAT_ADJ_REQ,*/
/*			LN_SEQ_FAT_ADJ_REQ,*/
/*			LC_TYP_FAT_ADJ_REQ,  */
/*			LC_STA_FAT_ADJ_REQ*/
/*		FROM*/
/*			PKUB.ADXX_PCV_ATY_ADJ ADXX*/
/*		WHERE*/
/*			LF_LST_DTS_ADXX >= &LAST_RUNPASS*/
/**/
/*	FOR READ ONLY WITH UR*/
/*	);*/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(ADXX,LD_FAT_ADJ_REQ LN_SEQ_FAT_ADJ_REQ);*/
/**/
/*/**********************************************/
/** NEG AM*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE FSXX AS*/
/*		SELECT */
/*			**/
/*		FROM CONNECTION TO DBX */
/*			(*/
/*				SELECT*/
/*					FSXX.BF_SSN,*/
/*					FSXX.LN_SEQ,*/
/*					FSXX.LD_CRT_NEG_AMR_LTR,*/
/*					FSXX.LA_PRJ_NEG_AMR_INT*/
/*				FROM*/
/*					PKUB.FSXX_ICR_NEG_AMR FSXX*/
/*				WHERE*/
/*					FSXX.LF_LST_DTS_FSXX >= &LAST_RUNPASS	*/
/*			)*/
/*	;*/
/**/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(FSXX,LN_SEQ LD_CRT_NEG_AMR_LTR);*/
/**/
/*/****************************************************************/
/** DEFER & FORBEAR APPROVED/CHANGED/DENIED LETTER � DEFER TABLE*/
/****************************************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE DFR_LTR AS*/
/*		SELECT */
/*			**/
/*		FROM CONNECTION TO DBX */
/*			(*/
/*				SELECT*/
/*					DFXX.BF_SSN,*/
/*					LNXX.LN_SEQ,*/
/*					DFXX.LF_DFR_CTL_NUM,*/
/*					LNXX.LN_DFR_OCC_SEQ,*/
/*					DFXX.LC_DFR_TYP,*/
/*					DFXX.LD_DFR_INF_CER,*/
/*					DFXX.LD_CRT_REQ_DFR,*/
/*					DFXX.LC_DFR_STA,*/
/*					DFXX.LC_STA_DFRXX,*/
/*					DFXX.LD_STA_DFRXX,*/
/*					DFXX.LF_LST_DTS_DFXX,*/
/*					LNXX.LD_DFR_BEG,*/
/*					LNXX.LD_DFR_END,*/
/*					LNXX.LC_STA_LONXX,*/
/*					LNXX.LC_LON_LEV_DFR_CAP,*/
/*					LNXX.LF_LST_DTS_LNXX,*/
/*					LNXX.LD_STA_LONXX,*/
/*					LNXX.LD_DFR_APL*/
/*				FROM */
/*					PKUB.DFXX_BR_DFR_REQ DFXX*/
/*					JOIN PKUB.LNXX_BR_DFR_APV LNXX*/
/*						ON DFXX.BF_SSN = LNXX.BF_SSN*/
/*						AND DFXX.LF_DFR_CTL_NUM = LNXX.LF_DFR_CTL_NUM*/
/*				WHERE*/
/*					DFXX.LF_LST_DTS_DFXX >= &LAST_RUNPASS*/
/*					OR LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS	*/
/*			)*/
/*	;*/
/**/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(DFR_LTR,LN_SEQ LF_DFR_CTL_NUM LN_DFR_OCC_SEQ);*/
/**/
/*/****************************************************************/
/** DEFER & FORBEAR APPROVED/CHANGED/DENIED LETTER � FORB TABLE*/
/****************************************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE FOR_LTR AS*/
/*		SELECT */
/*			**/
/*		FROM CONNECTION TO DBX */
/*			(*/
/*				SELECT*/
/*					FBXX.BF_SSN,*/
/*					LNXX.LN_SEQ,*/
/*					FBXX.LF_FOR_CTL_NUM,*/
/*					LNXX.LN_FOR_OCC_SEQ,*/
/*					FBXX.LC_FOR_TYP,*/
/*					FBXX.LD_FOR_INF_CER,*/
/*					FBXX.LD_CRT_REQ_FOR,*/
/*					FBXX.LC_FOR_STA,*/
/*					FBXX.LC_STA_FORXX,*/
/*					FBXX.LD_STA_FORXX,*/
/*					FBXX.LF_LST_DTS_FBXX,*/
/*					LNXX.LD_FOR_BEG,*/
/*					LNXX.LD_FOR_END,*/
/*					LNXX.LC_STA_LONXX,*/
/*					LNXX.LC_LON_LEV_FOR_CAP,*/
/*					LNXX.LF_LST_DTS_LNXX,*/
/*					LNXX.LD_STA_LONXX,*/
/*					LNXX.LD_FOR_APL*/
/*				FROM*/
/*					PKUB.FBXX_BR_FOR_REQ FBXX*/
/*					JOIN PKUB.LNXX_BR_FOR_APV LNXX*/
/*						ON FBXX.BF_SSN = LNXX.BF_SSN*/
/*						AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM*/
/*				WHERE*/
/*					 FBXX.LF_LST_DTS_FBXX >= &LAST_RUNPASS*/
/*					 OR LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/*			)*/
/*	;*/
/**/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(FOR_LTR,LN_SEQ LF_FOR_CTL_NUM LN_FOR_OCC_SEQ);*/
/**/
/*/**********************************************/
/** REPAYMENT SUMMARY LETTER*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE REPAY_SUM AS*/
/*		SELECT */
/*			**/
/*		FROM CONNECTION TO DBX */
/*			(*/
/*				SELECT*/
/*					RSXX.BF_SSN,*/
/*					LNXX.LN_SEQ,*/
/*					LNXX.LN_RPS_SEQ,*/
/*					LNXX.LN_GRD_RPS_SEQ,*/
/*					LNXX.LD_CRT_LONXX,*/
/*					LNXX.LC_TYP_SCH_DIS,*/
/*					LNXX.LA_TOT_RPD_DIS,*/
/*					LNXX.LR_INT_RPD_DIS,*/
/*					LNXX.LA_ANT_CAP,*/
/*					LNXX.LC_STA_LONXX,*/
/*					LNXX.LF_LST_DTS_LNXX,*/
/*					LNXX.LA_RPS_ISL,*/
/*					LNXX.LD_CRT_LONXX,*/
/*					LNXX.LN_RPS_TRM,*/
/*					LNXX.LF_LST_DTS_LNXX,*/
/*					RSXX.LD_RPS_X_PAY_DU*/
/*				FROM*/
/*					PKUB.RSXX_BR_RPD RSXX*/
/*					JOIN PKUB.LNXX_LON_RPS LNXX*/
/*						ON RSXX.BF_SSN = LNXX.BF_SSN*/
/*						AND RSXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ*/
/*					JOIN PKUB.LNXX_LON_RPS_SPF LNXX*/
/*						ON LNXX.BF_SSN = LNXX.BF_SSN*/
/*						AND LNXX.LN_SEQ = LNXX.LN_SEQ*/
/*						AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ*/
/*				WHERE*/
/*					LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS	*/
/*			)*/
/*	;*/
/**/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(REPAY_SUM,LN_SEQ LN_RPS_SEQ LN_GRD_RPS_SEQ);*/
/**/
/*/**********************************************/
/** IDR REPAYMENT INFORMATION*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE RSXX AS*/
/*		SELECT */
/*			**/
/*		FROM CONNECTION TO DBX */
/*			(*/
/*				SELECT DISTINCT*/
/*					RSXX.BF_SSN,*/
/*					RSXX.BD_CRT_RSXX,*/
/*					RSXX.BN_IBR_SEQ,*/
/*					RSXX.BF_CRY_YR,*/
/*					RSXX.BC_ST_IBR,*/
/*					RSXX.BC_STA_RSXX,*/
/*					RSXX.BA_AGI,*/
/*					RSXX.BN_MEM_HSE_HLD,*/
/*					RSXX.BA_PMN_STD_TOT_PAY,*/
/*					RSXX.BC_IBR_INF_SRC_VER,*/
/*					RSXX.BF_SSN_SPO,*/
/*					RSXX.BC_IRS_TAX_FIL_STA,*/
/*					RSXX.BI_JNT_BR_SPO_RPY,*/
/*					RSXX.BD_ANV_QLF_IBR*/
/*				FROM*/
/*					PKUB.RSXX_IBR_RPS RSXX*/
/*				WHERE*/
/*					RSXX.BF_LST_DTS_RSXX >= &LAST_RUNPASS	*/
/*			)*/
/*	;*/
/**/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(RSXX,BN_IBR_SEQ);*/
/**/
/**/
/*/**********************************************/
/** SCHOOL DATA*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE SCXX AS*/
/*		SELECT */
/*			**/
/*		FROM CONNECTION TO DBX */
/*			(*/
/*				SELECT DISTINCT*/
/*					'XXXXXXXXXX' AS DF_SPE_ACC_ID, /*required by .L load job common macros*/*/
/*					SCXX.IF_DOE_SCL,*/
/*					SCXX.IM_SCL_SHO,*/
/*					SCXX.IM_SCL_FUL,*/
/*					SCXX.IC_TYP_SCL,*/
/*					SCXX.IC_PRV_SCL_STA,*/
/*					SCXX.IC_CUR_SCL_STA,*/
/*					SCXX.II_SCL_CHS_PTC,*/
/*					SCXX.IC_LEN_LNG_PGM_STY*/
/**/
/*				FROM*/
/*					PKUB.SCXX_SCH_DMO SCXX*/
/*				WHERE*/
/*					SCXX.IF_LST_DTS_SCXX >= &LAST_RUNPASS	*/
/*			)*/
/*	;*/
/**/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/**/
/*/**********************************************/
/** LOAN SUBSIDY*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE FSXX AS*/
/*		SELECT */
/*			**/
/*		FROM CONNECTION TO DBX */
/*			(*/
/*				SELECT DISTINCT*/
/*					FSXX.BF_SSN,*/
/*					FSXX.LN_SEQ,*/
/*					FSXX.LN_INC_SUB_EVT_SEQ,*/
/*					FSXX.LD_INC_SUB_EFF_BEG,*/
/*					FSXX.LD_INC_SUB_EFF_END,*/
/*					FSXX.LC_INC_SUB_STA,*/
/*					FSXX.LR_SUB_RMN,*/
/*					FSXX.LF_LST_USR_FSXX,*/
/*					FSXX.LF_LST_DTS_FSXX,*/
/*					FSXX.LF_CRT_USR_FSXX,*/
/*					FSXX.LD_CRT_FSXX,*/
/*					FSXX.LC_STA_FSXX,*/
/*					FSXX.LD_STA_FSXX*/
/**/
/*				FROM*/
/*					PKUB.FSXX_SUB_LOS_RNS FSXX*/
/*				WHERE*/
/*					FSXX.LF_LST_DTS_FSXX >= &LAST_RUNPASS	*/
/*			)*/
/*	;*/
/**/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(FSXX, LN_SEQ);*/
/**/
/*/**********************************************/
/** QUEUE TASKS*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE WQXX AS*/
/*		SELECT */
/*			**/
/*		FROM CONNECTION TO DBX */
/*			(*/
/*				SELECT DISTINCT*/
/*					WQXX.BF_SSN,*/
/*					WQXX.WF_QUE,*/
/*					WQXX.WF_SUB_QUE,*/
/*					WQXX.WN_CTL_TSK,*/
/*					WQXX.PF_REQ_ACT,*/
/*					WQXX.WD_ACT_REQ,*/
/*					WQXX.WD_ACT_RQR,*/
/*					WQXX.WC_CND_CTC,*/
/*					WQXX.WD_INI_TSK,*/
/*					WQXX.WT_INI_TSK,*/
/*					WQXX.WF_USR_ASN_TSK,*/
/*					WQXX.WF_USR_ASN_BY_TSK,*/
/*					WQXX.WX_MSG_X_TSK,*/
/*					WQXX.WX_MSG_X_TSK,*/
/*					WQXX.WC_STA_WQUEXX,*/
/*					WQXX.WF_LST_DTS_WQXX,*/
/*					WQXX.LN_ATY_SEQ,*/
/*					WQXX.WF_CRT_DTS_WQXX*/
/*				FROM*/
/*					PKUB.WQXX_TSK_QUE WQXX*/
/*				WHERE*/
/*					WQXX.WF_LST_DTS_WQXX  >= &LAST_RUNPASS	*/
/*			)*/
/*	;*/
/**/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*%SSNXACC(WQXX,WF_QUE WF_SUB_QUE WN_CTL_TSK);*/
/**/
/*/**********************************************/
/** LNXXA*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE LNXXA AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DBX */
/*				(*/
/*					SELECT*/
/*						LNXX.**/
/*					FROM*/
/*						PKUB.LNXX_EFT_TO_LON LNXX*/
/*					WHERE*/
/*						LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/*/*						and lnXX.bf_ssn like '%XXX'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*/*%SSNXACC(LNXXA,);*/*/
/**/
/*/**********************************************/
/** OWXX*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE OWXX AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DBX */
/*				(*/
/*					SELECT*/
/*						IF_LON_SLE ,*/
/*						IC_LON_SLE_STA ,*/
/*						IC_LON_SLE_TYP ,*/
/*						ID_LON_SLE ,*/
/*						IF_SLL_OWN ,*/
/*						IM_SLL_CNC_X ,*/
/*						IM_SLL_CNC_LST ,*/
/*						IN_SLL_CNC_PHN_ARA ,*/
/*						IN_SLL_CNC_PHN_XCH ,*/
/*						IN_SLL_CNC_PHN_LCL ,*/
/*						IN_SLL_CNC_PHN_XTN ,*/
/*						IF_SLL_BND_ISS ,*/
/*						ID_SLL_LON_SLE_APV ,*/
/*						IF_BUY_OWN ,*/
/*						IM_BUY_CNC_X ,*/
/*						IM_BUY_CNC_LST ,*/
/*						IN_BUY_CNC_PHN_ARA ,*/
/*						IN_BUY_CNC_PHN_XCH ,*/
/*						IN_BUY_CNC_PHN_LCL ,*/
/*						IN_BUY_CNC_PHN_XTN ,*/
/*						IF_BUY_BND_ISS ,*/
/*						ID_BUY_LON_SLE_APV ,*/
/*						IM_MKT_CNC_X ,*/
/*						IM_MKT_CNC_LST ,*/
/*						IN_MKT_CNC_PHN_ARA ,*/
/*						IN_MKT_CNC_PHN_XCH ,*/
/*						IN_MKT_CNC_PHN_LCL ,*/
/*						IN_MKT_CNC_PHN_XTN ,*/
/*						IM_TRF_CNC_X ,*/
/*						IM_TRF_CNC_LST ,*/
/*						IN_TRF_CNC_PHN_ARA ,*/
/*						IN_TRF_CNC_PHN_XCH ,*/
/*						IN_TRF_CNC_PHN_LCL ,*/
/*						IN_TRF_CNC_PHN_XTN ,*/
/*						IM_LEG_CNC_X ,*/
/*						IM_LEG_CNC_LST ,*/
/*						IN_LEG_CNC_PHN_ARA ,*/
/*						IN_LEG_CNC_PHN_XCH ,*/
/*						IN_LEG_CNC_PHN_LCL ,*/
/*						IN_LEG_CNC_PHN_XTN ,*/
/*						IC_FEE_ORG_RSB ,*/
/*						II_ACP_NEW_LON_SLE ,*/
/*						IA_LON_TOT_MAX ,*/
/*						II_INT_ICL ,*/
/*						IN_LON_MAX ,*/
/*						CASE WHEN II_SLE_LTR_TRG = 'Y' THEN X ELSE X END AS II_SLE_LTR_TRG,*/
/*						IF_SLE_LTR_SPC ,*/
/*						IF_LST_DTS_OWXX ,*/
/*						IF_BUY_POR ,*/
/*						IC_SLL_PNT_LOC ,*/
/*						IC_BUY_PNT_LOC ,*/
/*						IC_TIR_PCE_ASN ,*/
/*						CASE WHEN II_LTE_FEE_WOF = 'Y' THEN X ELSE X END AS II_LTE_FEE_WOF,*/
/*						IC_LON_SLE_SEL_TYP ,*/
/*						II_LTE_FEE_MAX_VAL ,*/
/*						II_STP_SLE_LON_MAX ,*/
/*						II_LEV_BR_LON_ELG ,*/
/*						IC_SEL_CRI_USR_APV ,*/
/*						ID_SEL_CRI_USR_APV ,*/
/*						IF_SEL_CRI_USR_APV ,*/
/*						ID_SEL_NXT_PLR ,*/
/*						ID_LON_SLE_LST_PLR ,*/
/*						IT_SLE_LST_PLR ,*/
/*						IN_LON_SLE_BR_ELG ,*/
/*						IN_LON_SLE_LON_ELG ,*/
/*						IA_CUR_PRI_ELG_LON ,*/
/*						IA_NSI_ELG_LON ,*/
/*						IA_LTE_FEE_ELG_LON ,*/
/*						IN_IVL_SCH_NXT_SLE ,*/
/*						IC_IVL_SCH_NXT_SLE ,*/
/*						IN_IVL_SCH_NXT_PLR ,*/
/*						IC_IVL_SCH_NXT_PLR ,*/
/*						IX_TRG_FIL_SEL_CRI ,*/
/*						IC_RGN_RCV_DCV_LON ,*/
/*						IX_DSC_BUY_OWN ,*/
/*						LA_BR_PRI_BAL_SLE ,*/
/*						LC_BR_PRI_REL_SLE ,*/
/*						II_ORG_RGT_PUR_SLE ,*/
/*						IF_GRP_SLE_KEY ,*/
/*						IC_EFT_RIR_RSB ,*/
/*						IX_LON_SLE_CMT ,*/
/*						IC_BBS_RSB ,*/
/*						IC_LON_SLE_SUB_TYP ,*/
/*						IA_SLE_LVL_TRF_FEE ,*/
/*						IC_TRF_FEE_TYP ,*/
/*						IR_PRI_PER_FEE_RTE ,*/
/*						CASE WHEN II_ICL_CON_STP_PUR = 'Y' THEN X ELSE X END AS II_ICL_CON_STP_PUR,*/
/*						ID_ECA_DCV_CRT_FIL ,*/
/*						II_ECA_PUT_DCV_APV ,*/
/*						ID_ECA_CRT_BIL_SLE ,*/
/*						ID_CDU_REM_NTF ,*/
/*						IC_FED_PGM_YR ,*/
/*						IF_FLS_DEA ,*/
/*						IF_DEA_IST_LIN_HLD ,*/
/*						CASE WHEN II_LON_SLE_ICL_IDT = 'Y' THEN X ELSE X END AS II_LON_SLE_ICL_IDT,*/
/*						CASE WHEN II_ICL_CON_GRP_RLP = 'Y' THEN X ELSE X END AS II_ICL_CON_GRP_RLP,*/
/*						CASE WHEN LI_PCV_OWN_EFF_DTE = 'Y' THEN X ELSE X END AS LI_PCV_OWN_EFF_DTE,*/
/*						CASE WHEN II_PRE_SLE_LTR = 'Y' THEN X ELSE X END AS II_PRE_SLE_LTR,*/
/*						IC_DLA_CAN_LTR ,*/
/*						IC_PRE_SLE_LST_PRC */
/*					FROM*/
/*						PKUB.OWXX_LON_SLE_CTL */
/*					WHERE*/
/*						IF_LST_DTS_OWXX >= &LAST_RUNPASS*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*/*%SSNXACC(OWXX,);*/*/
/**/
/*/**********************************************/
/** WKXJ*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE WKXJ AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DBX */
/*				(*/
/*					SELECT*/
/*						WKXJ.**/
/*					FROM*/
/*						PKUB.WKXJ_LON_SLE_PLR WKXJ*/
/*/*					WHERE*/*/
/*/*						WKXJ.IT_SLE_LST_PLR >= &LAST_RUNPASS*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*/*%SSNXACC(WKXJ,);*/*/
/**/
/*/**********************************************/
/** LNXX*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE LNXX AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DBX */
/*				(*/
/*					SELECT*/
/*						LNXX.**/
/*					FROM*/
/*						PKUB.LNXX_LON_PAY_FAT LNXX*/
/*					WHERE*/
/*						LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/*/*						and lnXX.bf_ssn like '%XXX'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*/*%SSNXACC(LNXX,);*/*/
/**/
/*/**********************************************/
/** RMXX*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE RMXX AS*/
/*		SELECT */
/*			LD_RMT_BCH_INI ,*/
/*			LC_RMT_BCH_SRC_IPT ,*/
/*			LN_RMT_BCH_SEQ ,*/
/*			LN_RMT_SEQ ,*/
/*			LN_RMT_ITM ,*/
/*			LN_RMT_ITM_SEQ ,*/
/*			LC_RMT_STA ,*/
/*			LA_BR_RMT ,*/
/*			LC_RMT_REV_REA ,*/
/*			CASE WHEN LI_RMT_PD_AHD = 'Y' THEN X ELSE X END AS LI_RMT_PD_AHD,*/
/*			LD_RMT_PAY_EFF ,*/
/*			LF_RMT_EDS ,*/
/*			LF_RMT_IST ,*/
/*			LX_SPS_REA ,*/
/*			LD_RMT_SPS ,*/
/*			LD_RMT_PST ,*/
/*			LD_UPD_REMTXX ,*/
/*			LD_CRT_REMTXX ,*/
/*			LD_RMT_BCH_INI_REV ,*/
/*			LC_RMT_BCH_IPT_REV ,*/
/*			LN_RMT_BCH_SEQ_REV ,*/
/*			LN_RMT_SEQ_REV ,*/
/*			LN_RMT_ITM_REV ,*/
/*			LN_RMT_ITM_SEQ_REV ,*/
/*			LF_LST_DTS_RMXX ,*/
/*			PC_FAT_TYP ,*/
/*			PC_FAT_SUB_TYP ,*/
/*			BF_SSN ,*/
/*			LD_BIL_CRT ,*/
/*			LN_SEQ_BIL_WI_DTE ,*/
/*			LI_DPS_RMT_PIO_CVN ,*/
/*			BN_CPN_BK_SEQ ,*/
/*			LD_BIL_CPN_DU ,*/
/*			CASE WHEN LI_PHD_PAS_DU = 'Y' THEN X ELSE X END AS LI_PHD_PAS_DU,*/
/*			CASE WHEN LI_PHD_PAS_RPS = 'Y' THEN X ELSE X END AS LI_PHD_PAS_RPS,*/
/*			CASE WHEN LI_PHD_PAS_OPT = 'Y' THEN X ELSE X END AS LI_PHD_PAS_OPT,*/
/*			LI_PHD_BCH_OVR ,*/
/*			LC_RMT_REV_REA_REJ ,*/
/*			LC_STP_SPS_OVR ,*/
/*			LC_RMT_PAY_SRC ,*/
/*			CASE WHEN LC_RMT_SPS_OVR_FRD = 'Y' THEN X ELSE X END AS LC_RMT_SPS_OVR_FRD,*/
/*			LI_DPL_ACC_RMT ,*/
/*			CASE WHEN LC_WAV_MSC_FEE_ASS = 'Y' THEN X ELSE X END AS LC_WAV_MSC_FEE_ASS,*/
/*			LD_SCH_RMT_PAY ,*/
/*			LF_LCK_BOX_TRC ,*/
/*			LF_BR_RMT_SCH_NUM ,*/
/*			LC_BR_RMT_SCH_TYP ,*/
/*			LI_CON_SPS_OVR ,*/
/*			LD_RMT_SCH_NUM_DPS */
/*		FROM */
/*			CONNECTION TO DBX */
/*				(*/
/*					SELECT*/
/*						**/
/*					FROM*/
/*						PKUB.RMXX_BR_RMT*/
/*					WHERE*/
/*						LF_LST_DTS_RMXX >= &LAST_RUNPASS*/
/*/*						and bf_ssn like '%XXX'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*/*%SSNXACC(RMXX,);*/*/
/**/
/*/**********************************************/
/** LNXXA*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE LNXXA AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DBX */
/*				(*/
/*					SELECT*/
/*						BF_SSN ,*/
/*						LN_SEQ ,*/
/*						LC_STA_LONXX ,*/
/*						LI_FGV_PGM ,*/
/*						LF_LON_SLE_PND ,*/
/*						LA_CUR_PRI ,*/
/*						LI_ELG_CPN_BOK ,*/
/*						LA_LON_AMT_GTR ,*/
/*						LD_LON_GTR ,*/
/*						LD_PIF_PNT_RTN ,*/
/*						LD_PNT_SIG ,*/
/*						LC_INT_BIL_OPT ,*/
/*						LI_CAP_ALW ,*/
/*						LD_END_GRC_PRD ,*/
/*						LN_MTH_GRC_PRD_DSC ,*/
/*						LA_RXX_INT_PD ,*/
/*						LA_RXX_INT_MAX ,*/
/*						LD_CAP_LST_PIO_CVN ,*/
/*						LD_TRM_END ,*/
/*						LD_TRM_BEG ,*/
/*						CASE WHEN LI_GTR_NAT = 'Y' THEN X ELSE X END AS LI_GTR_NAT,*/
/*						LF_GTR_RFR ,*/
/*						CASE WHEN LI_ELG_SPA = 'Y' THEN X ELSE X END AS LI_ELG_SPA,*/
/*						LD_GTE_LOS ,*/
/*						LA_SCL_CLS ,*/
/*						LA_CUR_ILG ,*/
/*						LA_ILG ,*/
/*						LD_PIF_RPT ,*/
/*						LA_NSI_OTS ,*/
/*						LD_NSI_ACR_THU ,*/
/*						LD_STA_LONXX ,*/
/*						LD_LON_ACL_ADD ,*/
/*						LD_LON_EFF_ADD ,*/
/*						LF_DOE_SCL_ORG ,*/
/*						LC_PCV_DIS_STA ,*/
/*						LC_RPR_TYP ,*/
/*						LF_RGL_CAT_LPXX ,*/
/*						LF_RGL_CAT_LPXX ,*/
/*						LF_RGL_CAT_LPXX ,*/
/*						LF_RGL_CAT_LPXX ,*/
/*						LF_RGL_CAT_LPXX ,*/
/*						LF_RGL_CAT_LPXX ,*/
/*						LF_RGL_CAT_LPXX ,*/
/*						LF_RGL_CAT_LPXX ,*/
/*						LF_RGL_CAT_LPXX ,*/
/*						LF_RGL_CAT_LPXX ,*/
/*						LF_RGL_CAT_LPXX ,*/
/*						LF_RGL_CAT_LPXX ,*/
/*						LF_RGL_CAT_LPXX ,*/
/*						LF_RGL_CAT_LPXX ,*/
/*						LF_RGL_CAT_LPXX ,*/
/*						LD_SCL_CLS_NTF ,*/
/*						LD_ILG_NTF ,*/
/*						LF_LON_CUR_OWN ,*/
/*						LF_LST_DTS_LNXX ,*/
/*						LC_STA_NEW_BR ,*/
/*						LC_SCY_PGA ,*/
/*						LD_SIN_LST_PD_PCV ,*/
/*						LD_SIN_ACR_THU_PCV ,*/
/*						LA_SIN_OTS_PCV ,*/
/*						IC_LON_PGM ,*/
/*						PF_MAJ_BCH ,*/
/*						PF_MNR_BCH ,*/
/*						IF_DOE_LDR ,*/
/*						IF_GTR ,*/
/*						LF_STU_SSN ,*/
/*						LD_LON_X_DSB ,*/
/*						LC_ACA_GDE_LEV ,*/
/*						LD_NEW_SYS_CVN ,*/
/*						LC_SCY_PGA_PGM_YR ,*/
/*						IC_HSP_CSE ,*/
/*						LI_TLX_XXX_XCL_CON ,*/
/*						LI_DFR_REQ_ON_APL ,*/
/*						LI_LN_PT_COM_APL ,*/
/*						LN_SEQ_RPR ,*/
/*						LR_WIR_CON_LON ,*/
/*						LR_INT_RDC_PGM_DSU ,*/
/*						LI_X_TME_BR ,*/
/*						BF_SSN_RPR ,*/
/*						LC_ELG_RDC_PGM ,*/
/*						LD_ELG_RDC_PGM ,*/
/*						LC_RPD_SLE ,*/
/*						LR_ITR_ORG ,*/
/*						LC_ITR_TYP_ORG ,*/
/*						LC_RDC_PGM ,*/
/*						LC_TIR_GRP ,*/
/*						LD_SER_RSB_BEG ,*/
/*						LC_EFT_RDC ,*/
/*						LD_LTS_STS_BIL ,*/
/*						IF_TIR_PCE ,*/
/*						LF_RGL_CAT_LPXX ,*/
/*						LN_RDC_PGM_PAY_PCV ,*/
/*						LC_REA_PIF_PCV ,*/
/*						LD_FAT_PIF_TOL_PCV ,*/
/*						LA_FAT_PIF_TOL_PCV ,*/
/*						LI_RTE_RDC_ELG ,*/
/*						LD_LTE_FEE_ELG ,*/
/*						LA_LTE_FEE_OTS ,*/
/*						LD_LON_LTE_FEE_WAV ,*/
/*						LC_CUR_RDC_PGM_NME ,*/
/*						LI_RIR_SCY_ELG ,*/
/*						LD_END_RIR_DSQ_OVR ,*/
/*						LF_LON_ALT ,*/
/*						LN_LON_ALT_SEQ ,*/
/*						CASE WHEN LI_LDR_LST_RST_DSB = 'Y' THEN X ELSE X END AS LI_LDR_LST_RST_DSB,*/
/*						LC_ST_BR_RSD_APL ,*/
/*						LI_PIF_RPT_REQ ,*/
/*						LD_AMR_BEG ,*/
/*						LD_ORG_XPC_GRD ,*/
/*						LD_CON_LST_RPY_BEG ,*/
/*						LN_CON_MTH_DFR_FOR ,*/
/*						LD_LON_APL_RCV ,*/
/*						LR_SCL_SUB ,*/
/*						LI_BLL_PAY_SES_OVR ,*/
/*						LC_MPN_TYP ,*/
/*						LD_MPN_EXP ,*/
/*						LC_MPN_SRL_LON ,*/
/*						LC_MPN_REV_REA ,*/
/*						LF_ORG_RGN ,*/
/*						LC_CAM_LON_STA ,*/
/*						LD_DFR_FOR_END ,*/
/*						LC_DFR_FOR_TYP ,*/
/*						LF_CAM_DFR_SCL_ENR ,*/
/*						LD_DFR_FOR_BEG ,*/
/*						LD_CAM_DFR_INF_CER ,*/
/*						LI_BR_DET_RPD_XTN ,*/
/*						DD_DTH_VER ,*/
/*						DD_DSA_VER ,*/
/*						LI_CON_PAY_STP_PUR ,*/
/*						LD_FSE_CER_NTF ,*/
/*						LA_TOT_EDU_DET ,*/
/*						LI_LDR_BG_APL ,*/
/*						LD_RIR_CSC_BIL_STS ,*/
/*						LI_ESG ,*/
/*						LC_RIR_DSQ_REA ,*/
/*						LF_MN_MST_NTE ,*/
/*						LN_MN_MST_NTE_SEQ ,*/
/*						LC_LON_SND_CHC ,*/
/*						LC_SST_LONXX ,*/
/*						LF_RGL_CAT_LPXX ,*/
/*						LI_MN_PSD_BS ,*/
/*						LF_CRD_RTE_SRE ,*/
/*						LF_ESG_SRC ,*/
/*						PC_PNT_YR ,*/
/*						LF_OWN_BND_ISS_TEX ,*/
/*						LF_OWN_BND_ISS_TEX ,*/
/*						LF_OWN_BND_ISS_TEX ,*/
/*						LD_MPN_STM_SNT ,*/
/*						CASE WHEN LI_MNT_BIL_RCP = 'Y' THEN X ELSE X END AS LI_MNT_BIL_RCP,*/
/*						LX_BS_POI ,*/
/*						LA_BS_POI ,*/
/*						LA_INT_FEE_URP_IRS ,*/
/*						LC_BR_ALW_SCL_DFR ,*/
/*						LD_BR_ALW_SCL_DFR ,*/
/*						LC_LON_DFR_SUB_TYP ,*/
/*						LD_FAT_PRI_BAL_ZRO ,*/
/*						LC_ST_SCL_ATD_APL ,*/
/*						LD_EFT_DSQ_NSF_LMT ,*/
/*						LD_CLM_PD ,*/
/*						LC_STP_PUR ,*/
/*						LD_CON_PAY_EFF ,*/
/*						LD_CON_PAY_APL ,*/
/*						LC_ESG ,*/
/*						LC_LON_RPE_CVN_REA ,*/
/*						LC_UDL_DSB_COF ,*/
/*						LI_BR_LT_HT ,*/
/*						LI_ALL_PAY_FLG_SPS ,*/
/*						LN_MFY_GRS_PAY ,*/
/*						LC_ESP_RPD_OPT_SEL ,*/
/*						LC_ELG_XX_SPA_BIL ,*/
/*						LC_SGM_COS_PRC ,*/
/*						LD_GTR_DR_DCH_CER ,*/
/*						LD_RPD_ELY_CL_BEG ,*/
/*						LD_RPD_ELY_CL_END ,*/
/*						LF_GTR_RFR_XTN ,*/
/*						LA_MSC_FEE_OTS ,*/
/*						LA_MSC_FEE_PCV_OTS ,*/
/*						LF_LON_GRP_WI_BR ,*/
/*						LC_TLX_IBR_ELG ,*/
/*						LD_LON_IBR_ENT ,*/
/*						LC_LON_IBR_RPY_TYP ,*/
/*						LI_BYP_COL_OUT_SRC ,*/
/*						LI_BR_GRP_RLP ,*/
/*						LI_OO_PST_ENR_DFR ,*/
/*						LD_OO_PST_ENR_DFR ,*/
/*						LF_FED_CLC_RSK ,*/
/*						LF_FED_FFY_X_DSB ,*/
/*						LF_PRV_GTR ,*/
/*						LC_FED_PGM_YR ,*/
/*						LA_INT_RCV_GOV ,*/
/*						LC_WOF_WUP_REA ,*/
/*						LC_VRS_ALT_APL ,*/
/*						LF_LON_DCV_CLI ,*/
/*						LN_LON_SEQ_DCV_CLI ,*/
/*						LI_EDS_BKR_STP_PUR ,*/
/*						LF_EDS ,*/
/*						LD_EFF_LBR_RTE ,*/
/*						LA_STD_STD_PAY ,*/
/*						LI_FRC_IBR ,*/
/*						LC_STP_PUR_REA ,*/
/*						LD_IDR_ELG_CHG ,*/
/*						LC_IDR_ELG_CRI*/
/*					FROM*/
/*						PKUB.LNXX_LON */
/*					WHERE*/
/*						LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/*/*						and bf_ssn like '%XXX'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*/*%SSNXACC(LNXXA,);*/*/
/**/
/*/**********************************************/
/** LNXX*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE LNXX AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DBX */
/*				(*/
/*					SELECT*/
/*						LNXX.**/
/*					FROM*/
/*						PKUB.LNXX_LON_SLE_FAT LNXX*/
/*					WHERE*/
/*						LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/*/*						and lnXX.bf_ssn like '%XXX'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*/*%SSNXACC(LNXX,);*/*/
/**/
/*/**********************************************/
/** LNXXA*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE LNXXA AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DBX */
/*				(*/
/*					SELECT*/
/*						LNXX.BF_SSN ,*/
/*						LNXX.LN_SEQ ,*/
/*						LNXX.LN_RPS_SEQ ,*/
/*						LNXX.LA_RPD_INT_DIS ,*/
/*						LNXX.LR_APR_RPD_DIS ,*/
/*						LNXX.LA_TOT_RPD_DIS ,*/
/*						LNXX.LA_CPI_RPD_DIS ,*/
/*						LNXX.LR_INT_RPD_DIS ,*/
/*						LNXX.LA_ANT_CAP ,*/
/*						LNXX.LD_GRC_PRD_END ,*/
/*						LNXX.LD_CRT_LONXX ,*/
/*						LNXX.LC_STA_LONXX ,*/
/*						LNXX.LF_LST_DTS_LNXX ,*/
/*						LNXX.LC_TYP_SCH_DIS ,*/
/*						LNXX.LA_ACR_INT_RPD ,*/
/*						LNXX.LA_ANT_SUP_FEE ,*/
/*						LNXX.LN_RPD_MAX_TRM_REQ ,*/
/*						LNXX.LD_RPD_MAX_TRM_SR ,*/
/*						LNXX.LC_RPD_INA_REA ,*/
/*						LNXX.LC_RPD_DIS ,*/
/*						LNXX.LR_CLC_INC_SCH ,*/
/*						LNXX.LA_CLC_RPY_SCH ,*/
/*						LNXX.LI_ICR_RPD_NEG_AMR */
/*					FROM*/
/*						PKUB.LNXX_LON_RPS LNXX*/
/*					WHERE*/
/*						LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/*/*						and lnXX.bf_ssn like '%XXX'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*/*%SSNXACC(LNXXA,);*/*/
/**/
/*/**********************************************/
/** LNXX*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE LNXX AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DBX */
/*				(*/
/*					SELECT*/
/*						LNXX.BF_SSN ,*/
/*						LNXX.LN_SEQ ,*/
/*						LNXX.LN_RPS_SEQ ,*/
/*						LNXX.LN_GRD_RPS_SEQ ,*/
/*						LNXX.LA_RPS_ISL ,*/
/*						LNXX.LD_CRT_LONXX ,*/
/*						LNXX.LN_RPS_TRM ,*/
/*						LNXX.LF_LST_DTS_LNXX ,*/
/*						LNXX.LA_PRI_RDC_GRD ,*/
/*						LNXX.LN_PRI_RDC_GRD_TRM ,*/
/*						LNXX.LA_PRI_ATU_PAY ,*/
/*						LNXX.LD_RPYE_FGV */
/*					FROM*/
/*						PKUB.LNXX_LON_RPS_SPF LNXX*/
/*					WHERE*/
/*						LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/*/*						and lnXX.bf_ssn like '%XXX'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*/*%SSNXACC(LNXX,);*/*/
/**/
/*/**********************************************/
/** RSXXA*/
/***********************************************/*/
/*PROC SQL;*/
/*	CONNECT TO DBX (DATABASE=&DB);*/
/**/
/*	CREATE TABLE RSXXA AS*/
/*		SELECT */
/*			**/
/*		FROM */
/*			CONNECTION TO DBX */
/*				(*/
/*					SELECT*/
/*						RSXX.BF_SSN ,  */
/*						RSXX.BD_CRT_RSXX ,  */
/*						RSXX.BN_IBR_SEQ ,  */
/*						RSXX.BF_CRT_USR_RSXX ,  */
/*						RSXX.BF_CRY_YR ,  */
/*						RSXX.BC_ST_IBR ,  */
/*						RSXX.BC_STA_RSXX ,  */
/*						RSXX.BA_AGI ,  */
/*						RSXX.BN_MEM_HSE_HLD ,  */
/*						RSXX.BA_PMN_STD_TOT_PAY ,  */
/*						RSXX.BC_IBR_INF_SRC_VER ,  */
/*						RSXX.BF_LST_DTS_RSXX ,  */
/*						RSXX.BF_SSN_SPO ,  */
/*						RSXX.BC_IRS_TAX_FIL_STA ,  */
/*						RSXX.BI_JNT_BR_SPO_RPY ,  */
/*						RSXX.BD_ANV_QLF_IBR ,  */
/*						RSXX.BC_DOC_SNT_BR_IDR ,  */
/*						RSXX.BC_BR_REQ_PLN_RPYE ,  */
/*						RSXX.BC_BR_GDE_LVL_RPYE ,  */
/*						RSXX.BI_REQ_DFR_FOR_CUT ,  */
/*						RSXX.BC_DFR_FOR_CUT_TYP */
/*					FROM*/
/*						PKUB.RSXX_IBR_RPS RSXX*/
/*					WHERE*/
/*						RSXX.BF_LST_DTS_RSXX >= &LAST_RUNPASS*/
/*/*						and rsXX.bf_ssn like '%XXX'*/*/
/**/
/*					FOR READ ONLY WITH UR*/
/*				)*/
/*	;*/
/**/
/*	DISCONNECT FROM DBX;*/
/*QUIT;*/
/*/*%SSNXACC(RSXXA,);*/*/
;

/*********************************************
* LNXXA
**********************************************/
PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE LNXXA AS
		SELECT 
			*
		FROM 
			CONNECTION TO DBX 
				(
					SELECT
						LNXXA.BF_SSN ,
						LNXXA.LN_SEQ ,
						LNXXA.PM_BBS_PGM ,
						LNXXA.PN_BBS_PGM_SEQ ,
						LNXXA.LN_LON_BBS_PGM_SEQ ,
						LNXXA.LD_EFF_BEG_LNXX ,
						CASE WHEN LNXXA.LI_BBS_ITD_LTR_SNT = 'Y' THEN X ELSE X END AS LI_BBS_ITD_LTR_SNT,
						LNXXA.LN_BBS_STS_PCV_PAY ,
						LNXXA.LC_BBS_REB_MTD ,
						LNXXA.LC_STA_LNXX ,
						LNXXA.LD_STA_LNXX ,
						CASE WHEN LNXXA.LC_BBT_TYS_ASS = 'Y' THEN X ELSE X END AS LC_BBT_TYS_ASS,
						LNXXA.LC_BBS_DSQ_REA ,
						LNXXA.LD_BBS_DSQ ,
						LNXXA.LC_BBS_ELG ,
						LNXXA.LC_BBT_PRC_RBD ,
						LNXXA.LD_BBS_RPD_WDO_END ,
						LNXXA.LC_BBS_BCH_PRC ,
						LNXXA.LF_LST_USR_LNXX ,
						LNXXA.LF_LST_DTS_LNXX ,
						LNXXA.LN_BBS_PCV_PAY_MOT ,
						LNXXA.LD_BBS_ICV_REQ ,
						LNXXA.LD_BBS_DSQ_APL ,
						LNXXA.LI_BBS_PCV_LTE_PAY
					FROM
						PKUB.LNXX_LON_BBS LNXXA
					WHERE
						LNXXA.LF_LST_DTS_LNXX >= &LAST_RUNPASS

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;
QUIT;
/*%SSNXACC(LNXXA,);*/

/*********************************************
* LNXXA
**********************************************/
PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE LNXXA AS
		SELECT 
			*
		FROM 
			CONNECTION TO DBX 
				(
					SELECT
						LNXXA.BF_SSN ,
						LNXXA.LN_SEQ ,
						LNXXA.LN_LON_BBS_SEQ ,
						LNXXA.LN_LON_BBT_SEQ ,
						LNXXA.LF_LON_BBS_TIR ,
						LNXXA.LF_LON_BBS_SUB_TIR ,
						LNXXA.PM_BBS_PGM ,
						LNXXA.PN_BBS_PGM_SEQ ,
						LNXXA.PF_BBS_PGM_TIR ,
						LNXXA.PN_BBS_PGM_TIR_SEQ ,
						LNXXA.LN_BR_DSB_SEQ ,
						LNXXA.LC_STA_LNXX ,
						LNXXA.LD_STA_LNXX ,
						LNXXA.LD_LON_BBT_CHK_ISS ,
						LNXXA.LD_BBT_DSQ_OVR_END ,
						LNXXA.LN_LON_BBT_PAY_OVR ,
						LNXXA.LD_LON_BBT_BEG ,
						LNXXA.LD_REB_MTD_LTR_SNT ,
						LNXXA.LC_LON_BBT_REB_MTD ,
						LNXXA.LN_BBT_PAY_PIF_MOT ,
						LNXXA.LN_BBT_PAY_DLQ_MOT ,
						LNXXA.LC_LON_BBT_DSQ_REA ,
						LNXXA.LC_LON_BBT_STA ,
						LNXXA.LN_LON_BBT_PAY ,
						LNXXA.LD_LON_BBT_STA ,
						LNXXA.LD_BBT_STS_PAY ,
						LNXXA.LF_LST_USR_LNXX ,
						LNXXA.LF_LST_DTS_LNXX ,
						LNXXA.LD_LON_BBT_ELG_FNL ,
						LNXXA.LD_BBT_DLQ_MOT_STS ,
						LNXXA.LD_BBT_PIF_MOT_STS ,
						LNXXA.LN_BBT_DLQ_MOT_OVR ,
						LNXXA.LN_BBT_PIF_MOT_OVR
					FROM
						PKUB.LNXX_LON_BBS_TIR LNXXA
					WHERE
						LNXXA.LF_LST_DTS_LNXX >= &LAST_RUNPASS

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;
QUIT;
/*%SSNXACC(LNXXA,);*/

/*********************************************
* RPXX
**********************************************/
PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE RPXX AS
		SELECT 
			*
		FROM 
			CONNECTION TO DBX 
				(
					SELECT
						RPXX.IF_OWN ,
						RPXX.PN_EFT_RIR_OWN_SEQ ,
						RPXX.IC_LON_PGM ,
						RPXX.IF_GTR ,
						RPXX.PD_LON_X_DSB ,
						RPXX.PF_DOE_SCL_ORG ,
						RPXX.PC_ST_BR_RSD_APL ,
						RPXX.PD_EFT_RIR_EFF_BEG ,
						RPXX.PD_EFT_RIR_EFF_END ,
						RPXX.PC_EFT_RIR_STA ,
						RPXX.PD_EFT_RIR_STA ,
						RPXX.PI_EFT_RIR_PRC ,
						RPXX.PC_EFT_NSF_LTR_REQ ,
						RPXX.PR_EFT_RIR ,
						RPXX.PF_LST_USR_RPXX ,
						RPXX.PF_LST_DTS_RPXX ,
						RPXX.PC_EFT_RIR_PNT_YR ,
						RPXX.PD_EFT_BBS_LOT_BEG ,
						RPXX.PD_EFT_BBS_GTE_DTE ,
						RPXX.PD_EFT_BBS_RPD_SR ,
						RPXX.PD_EFT_BBS_LCO_RCV ,
						RPXX.PN_EFT_BBS_NSF_LMT ,
						RPXX.PC_EFT_BBS_NSF_PRC ,
						RPXX.PN_EFT_BBS_NSF_MTH ,
						RPXX.PC_EFT_BBS_FED ,
						RPXX.PI_EFT_RIR_RPY_X
					FROM
						PKUB.RPXX_EFT_RIR_PAR RPXX
					WHERE
						RPXX.PF_LST_DTS_RPXX >= &LAST_RUNPASS

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;
QUIT;
/*%SSNXACC(RPXX,);*/

/*DATA SAS_TAB.LASTRUN_JOBS;*/
/*SET SAS_TAB.LASTRUN_JOBS;*/
/*IF JOB = 'UTNWDWX' THEN LAST_RUN = TODAY();*/
/*RUN;*/
ENDRSUBMIT;

LIBNAME LEGEND 'T:\CDW_LEGEND\';

/*DATA LNXX; SET LEGEND.LNXX; RUN; *X;*/
/*DATA BRXX; SET LEGEND.BRXX; RUN; *X;*/
/*DATA LNXX; SET LEGEND.LNXX; RUN; *X;*/
/*DATA LNXX; SET LEGEND.LNXX; RUN; *X;*/
/*DATA DFXX; SET LEGEND.DFXX; RUN; *X;*/
/*DATA FBXX; SET LEGEND.FBXX; RUN; *X;*/
/*DATA PDXX; SET LEGEND.PDXX; RUN; *X;*/
/*DATA DLXXX; SET LEGEND.DLXXX; RUN; *X; */
/*DATA DRLFA; SET LEGEND.DRLFA; RUN; *XX;*/
/*DATA LNXX; SET LEGEND.LNXX; RUN; *XX;*/
/*DATA LNXX; SET LEGEND.LNXX; RUN; *XX;*/
/*DATA LNXX; SET LEGEND.LNXX; RUN; *XX;*/
/*DATA LNXX; SET LEGEND.LNXX; RUN; *XX;*/
/*DATA LNXX; SET LEGEND.LNXX; RUN; *XX;*/
/*DATA ARC_IND; SET LEGEND.ARC_IND; RUN; *XX; */
/*DATA LNXX; SET LEGEND.LNXX; RUN; *XX;*/
/*DATA LNXX; SET LEGEND.LNXX; RUN; *XX;*/
/*DATA SDXX; SET LEGEND.SDXX; RUN; *XX;*/
/*DATA LNXX; SET LEGEND.LNXX; RUN; *XX;*/
/*DATA KXPHN; SET LEGEND.KXPHN; RUN; *XX;  */
/*DATA BRXX; SET LEGEND.BRXX; RUN; *XX;*/
/*DATA PDXX; SET LEGEND.PDXX; RUN; *XX;*/
/*DATA RMXX; SET LEGEND.RMXX; RUN; *XX;*/
/*DATA PDXX; SET LEGEND.PDXX; RUN; *XX;*/
/*DATA PDXX; SET LEGEND.PDXX; RUN; *XX;*/
/*DATA KXADD; SET LEGEND.KXADD; RUN; *XX;*/
/*DATA ARCHIST; SET LEGEND.ARCHIST; RUN; *XX;*/
/*DATA DWXX; SET LEGEND.DWXX; RUN; *XX;*/
/*DATA LNXX; SET LEGEND.LNXX; RUN; *XX;*/
/*DATA CUR_DUE; SET LEGEND.CUR_DUE; RUN; *XX;*/
/*DATA ARC_MXXXX; SET LEGEND.ARC_MXXXX; RUN; *XX;*/
/*DATA LNXX_HIST; SET LEGEND.LNXX_HIST; RUN; *XX;*/
/*DATA LTXX; SET LEGEND.LTXX; RUN; *XX;*/
/*DATA RPXX; SET LEGEND.RPXX; RUN; *XX;*/
/*DATA LNXX; SET LEGEND.LNXX; RUN; *XX;*/
/*DATA ADXX; SET LEGEND.ADXX; RUN; *XX;*/
/*DATA FSXX; SET LEGEND.FSXX; RUN; *XX;*/
/*DATA DFR_LTR; SET LEGEND.DFR_LTR; RUN; *XX;*/
/*DATA FOR_LTR; SET LEGEND.FOR_LTR; RUN; *XX;*/
/*DATA REPAY_SUM; SET LEGEND.REPAY_SUM; RUN; *XX;*/
/*DATA RSXX; SET LEGEND.RSXX; RUN; *XX;*/
/*DATA SCXX; SET LEGEND.SCXX; RUN; *XX;*/
/*DATA FSXX; SET LEGEND.FSXX; RUN; *XX;*/
/*DATA WQXX; SET LEGEND.WQXX; RUN; *XX;*/
/*DATA LNXXA; SET LEGEND.LNXXA; RUN; *XX;*/
/*DATA OWXX; SET LEGEND.OWXX; RUN; *XX;*/
/*DATA WKXJ; SET LEGEND.WKXJ; RUN; *XX;*/
/*DATA LNXX; SET LEGEND.LNXX; RUN; *XX;*/
/*DATA RMXX; SET LEGEND.RMXX; RUN; *XX;*/
/*DATA LNXXA; SET LEGEND.LNXXA; RUN; *XX;*/
/*DATA LNXX; SET LEGEND.LNXX; RUN; *XX;*/
/*DATA LNXXA; SET LEGEND.LNXXA; RUN; *XX;*/
/*DATA LNXX; SET LEGEND.LNXX; RUN; *XX;*/
/*DATA RSXXA; SET LEGEND.RSXXA; RUN; *XX;*/
DATA LNXXA; SET LEGEND.LNXXA; RUN; *XX;
DATA LNXXA; SET LEGEND.LNXXA; RUN; *XX;
DATA RPXX; SET LEGEND.RPXX; RUN; *XX;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXX end = eof;*/
/*	FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ @;*/
/*		PUT LC_STA_LONXX $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET BRXX end = eof;*/
/*	FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LST_CNC LST_ATT MMDDYYXX.;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT BF_RFR  @;*/
/*		PUT BC_STA_REFRXX @;*/
/*		PUT BI_ATH_X_PTY @;*/
/*		PUT BC_RFR_REL_BR @;*/
/*		PUT DM_PRS_X @;*/
/*		PUT DM_PRS_LST @;*/
/*		PUT LST_CNC @;*/
/*		PUT LST_ATT $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXX end = eof;*/
/*	FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LD_BIL_CRT LD_FAT_EFF LD_BIL_DU_LON MMDDYYXX. ;*/
/*	FORMAT 	LA_BIL_CUR_DU LA_BIL_PAS_DU	LA_TOT_BIL_STS LA_INT_PD_LST_STM LA_FEE_PD_LST_STM LA_PRI_PD_LST_STM LA_TTL_PD_LST_STM LA_LTE_FEE_OTS_PRT X.X;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ @;*/
/*		PUT LD_BIL_CRT @;*/
/*		PUT LN_SEQ_BIL_WI_DTE @;*/
/*		PUT LD_FAT_EFF @;*/
/*		PUT LD_BIL_DU_LON @;*/
/*		PUT LC_STA_LONXX @;*/
/*		PUT LA_BIL_CUR_DU @;*/
/*		PUT LA_BIL_PAS_DU @;*/
/*		PUT LC_BIL_MTD @;*/
/*		PUT LC_IND_BIL_SNT @;*/
/*		PUT LC_STA_BILXX @;*/
/*		PUT LA_TOT_BIL_STS @;*/
/*		PUT LA_INT_PD_LST_STM @;*/
/*		PUT LA_FEE_PD_LST_STM @;*/
/*		PUT LA_PRI_PD_LST_STM @;*/
/*		PUT LA_TTL_PD_LST_STM @;*/
/*		PUT LA_LTE_FEE_OTS_PRT $;*/
/*	END;*/
/*	if eof then put "-End-";*/
/*RUN; */
/**/
/*DATA _NULL_;*/
/*	SET LNXX end = eof;*/
/*	FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LD_FAT_PST LD_FAT_EFF MMDDYYXX. ;*/
/*	FORMAT LA_FAT_CUR_PRI LA_FAT_LTE_FEE LA_FAT_NSI XX.X ;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ @;*/
/*		PUT LN_FAT_SEQ @;*/
/*		PUT LD_FAT_PST @;*/
/*		PUT LD_FAT_EFF  @;*/
/*		PUT LC_STA_LONXX @;*/
/*		PUT LA_FAT_CUR_PRI @;*/
/*		PUT LA_FAT_LTE_FEE @;*/
/*		PUT PC_FAT_TYP @;*/
/*		PUT PC_FAT_SUB_TYP @;*/
/*		PUT LA_FAT_NSI @;*/
/*		PUT LC_FAT_REV_REA $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET DFXX end = eof;*/
/*	FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LD_DFR_INF_CER LD_DFR_BEG LD_DFR_END MMDDYYXX. ;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ @;*/
/*		PUT LF_DFR_CTL_NUM @;*/
/*		PUT LN_DFR_OCC_SEQ @;*/
/*		PUT LC_DFR_TYP @;*/
/*		PUT LD_DFR_INF_CER @;*/
/*		PUT LD_DFR_BEG @;*/
/*		PUT LD_DFR_END @;*/
/*		PUT LC_LON_LEV_DFR_CAP @;*/
/*		PUT LC_STA_LONXX @;*/
/*		PUT LC_DFR_STA @;*/
/*		PUT LC_STA_DFRXX $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET FBXX end = eof;*/
/*	FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LD_FOR_INF_CER LD_FOR_BEG LD_FOR_END MMDDYYXX. LA_REQ_RDC_PAY X.X;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ @;*/
/*		PUT LF_FOR_CTL_NUM @;*/
/*		PUT LN_FOR_OCC_SEQ @;*/
/*		PUT LC_FOR_TYP @;*/
/*		PUT LD_FOR_INF_CER @;*/
/*		PUT LD_FOR_BEG @;*/
/*		PUT LD_FOR_END @;*/
/*		PUT LC_LON_LEV_FOR_CAP @;*/
/*		PUT LC_STA_LONXX @;*/
/*		PUT LC_FOR_STA @;*/
/*		PUT LC_STA_FORXX @;*/
/*		PUT LA_REQ_RDC_PAY @;*/
/*		PUT LI_FOR_VRB_DFL_RUL $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET PDXX end = eof;*/
/*	FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT DD_BRT MMDDYYXX.;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT BF_SSN @;*/
/*		PUT DM_PRS_X @;*/
/*		PUT DM_PRS_LST @;*/
/*		PUT DM_PRS_MID @;*/
/*		PUT DD_BRT $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET DLXXX end = eof;*/
/*	FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LD_ATY_REQ_RCV MMDDYYXX.;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;	*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_ATY_SEQ @;*/
/*		PUT LC_STA_ACTYXX @;*/
/*		PUT LD_ATY_REQ_RCV $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET DRLFA end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT FEE_WAV_DOL @;*/
/*		PUT FEE_WAV_CT $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LD_END_GRC_PRD LD_LON_X_DSB  LD_PIF_RPT MMDDYYXX. ;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ @;*/
/*		PUT LC_STA_LONXX @;*/
/*		PUT LA_CUR_PRI @;*/
/*		PUT LA_LON_AMT_GTR @;*/
/*		PUT LD_END_GRC_PRD @;*/
/*		PUT IC_LON_PGM @;*/
/*		PUT LD_LON_X_DSB @;*/
/*		PUT LD_PIF_RPT @;*/
/*		PUT LC_SST_LONXX @;*/
/*		PUT LF_LON_CUR_OWN @;*/
/*		PUT LF_DOE_SCL_ORG $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ  @;*/
/*		PUT BN_EFT_SEQ @;*/
/*		PUT LC_STA_LNXX $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LD_LON_RHB_PCV MMDDYYXX. ;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ @;*/
/*		PUT LD_LON_RHB_PCV $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LD_DSB MMDDYYXX.;*/
/*	FORMAT LA_DSB LA_DL_REBATE X.X;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_BR_DSB_SEQ @;*/
/*		PUT LA_DSB @;*/
/*		PUT LD_DSB @;*/
/*		PUT LC_DSB_TYP @;*/
/*		PUT LC_STA_LONXX @;*/
/*		PUT LN_SEQ @;*/
/*		PUT LA_DL_REBATE $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LD_BBS_DSQ MMDDYYXX. ;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ  @;*/
/*		PUT LD_BBS_DSQ  @;*/
/*		PUT LC_BBS_ELG @;*/
/*		PUT PR_EFT_RIR @;*/
/*		PUT PM_BBS_PGM @;*/
/*		PUT LN_BBS_STS_PCV_PAY @;*/
/*		PUT PN_BBT_DLQ_MOT $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET ARC_IND end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;	*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_ATY_SEQ @;*/
/*		PUT PF_REQ_ACT @;*/
/*		PUT LC_STA_ACTYXX $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LD_CRT_LONXX LD_SNT_RPD_DIS MMDDYYXX. ;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ  @;*/
/*		PUT LD_CRT_LONXX @;	*/
/*		PUT LC_TYP_SCH_DIS @; */
/*		PUT LD_SNT_RPD_DIS @;*/
/*		PUT LA_RPS_ISL @;*/
/*		PUT DAY_DUE @;*/
/*		PUT LN_RPS_TRM $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LD_ITR_EFF_BEG LD_ITR_EFF_END MMDDYYXX.;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ  @;*/
/*		PUT LR_ITR @;*/
/*		PUT LR_INT_RDC_PGM_ORG @;*/
/*		PUT LC_ITR_TYP @;*/
/*		PUT LD_ITR_EFF_BEG @;*/
/*		PUT LD_ITR_EFF_END $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET SDXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LD_SCL_SPR MMDDYYXX.;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ  @;*/
/*		PUT LD_SCL_SPR @;*/
/*		PUT IM_SCL_FUL $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LD_DLQ_OCC LD_DLQ_MAX MMDDYYXX.;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ @;*/
/*		PUT LD_DLQ_OCC @;*/
/*		PUT LN_DLQ_MAX @;*/
/*		PUT LD_DLQ_MAX $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET KXPHN end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;	*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_ATY_SEQ @;*/
/*		PUT LC_STA_ACTYXX @;*/
/*		PUT PHNX @;*/
/*		PUT PHNX @;*/
/*		PUT PHNX @;*/
/*		PUT COMMENTS $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET BRXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT BD_EFT_STA MMDDYYXX.;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT BN_EFT_SEQ @;*/
/*		PUT BF_EFT_ABA @;*/
/*		PUT BF_EFT_ACC @;*/
/*		PUT BC_EFT_STA @;*/
/*		PUT BD_EFT_STA @;*/
/*		PUT BA_EFT_ADD_WDR @;*/
/*		PUT BN_EFT_NSF_CTR @;*/
/*		PUT BC_EFT_DNL_REA $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET PDXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT DD_VER_ADR_EML MMDDYYXX.;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT DC_ADR_EML @;*/
/*		PUT DX_ADR_EML @;*/
/*		PUT DD_VER_ADR_EML @;*/
/*		PUT DI_VLD_ADR_EML $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET RMXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LA_BR_RMT_PST XX.X ;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LA_BR_RMT_PST $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET PDXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT DD_VER_ADR MMDDYYXX.;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT DX_STR_ADR_X @;*/
/*		PUT DX_STR_ADR_X @;*/
/*		PUT DM_CT @;*/
/*		PUT DC_DOM_ST @;*/
/*		PUT DF_ZIP_CDE @;*/
/*		PUT DM_FGN_ST @;*/
/*		PUT DM_FGN_CNY @;*/
/*		PUT DD_VER_ADR @;*/
/*		PUT DI_VLD_ADR $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET PDXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT DD_PHN_VER MMDDYYXX.;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT DC_PHN @;*/
/*		PUT DC_ALW_ADL_PHN @;*/
/*		PUT DD_PHN_VER @;*/
/*		PUT DI_PHN_VLD @;*/
/*		PUT DN_DOM_PHN_ARA @;*/
/*		PUT DN_DOM_PHN_XCH @;*/
/*		PUT DN_DOM_PHN_LCL @;*/
/*		PUT DN_PHN_XTN @;*/
/*		PUT DN_FGN_PHN_CNY @;*/
/*		PUT DN_FGN_PHN_CT @;*/
/*		PUT DN_FGN_PHN_LCL $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET KXADD end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_ATY_SEQ  @;*/
/*		PUT LC_STA_ACTYXX @;*/
/*		PUT DX_STR_ADR_X @;*/
/*		PUT DX_STR_ADR_X @;*/
/*		PUT DM_CT @;*/
/*		PUT DC_DOM_ST @;*/
/*		PUT DF_ZIP_CDE @;*/
/*		PUT DM_FGN_CNY @;*/
/*		PUT COMMENTS $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET ARCHIST end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LD_ATY_REQ_RCV LD_ATY_RSP MMDDYYXX.;*/
/*	FORMAT LT_ATY_RSP TIMEX.;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_ATY_SEQ @;*/
/*		PUT LC_STA_ACTYXX @;*/
/*		PUT PF_REQ_ACT @;*/
/*		PUT PF_RSP_ACT @;*/
/*		PUT PX_ACT_DSC_REQ @;*/
/*		PUT LD_ATY_REQ_RCV @;*/
/*		PUT LD_ATY_RSP @;*/
/*		PUT LF_USR_REQ_ATY @;*/
/*		PUT LT_ATY_RSP @;*/
/*		PUT LX_ATY @;*/
/*		PUT LC_ATY_RCP @;*/
/*		PUT LF_ATY_RCP $;*/
/*	END;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET DWXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT WA_TOT_BRI_OTS X.X WD_LON_RPD_SR mmddyyXX.;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ  @;*/
/*		PUT WC_DW_LON_STA  @;*/
/*		PUT WA_TOT_BRI_OTS @;*/
/*		PUT WD_LON_RPD_SR @;*/
/*		PUT WX_OVR_DW_LON_STA $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT DT_ADJ LD_RPT_CRB MMDDYYXX. ;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ  @;*/
/*		PUT LC_RPT_STA_CRB  @;*/
/*		PUT LD_RPT_CRB @;*/
/*		PUT DT_ADJ $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET CUR_DUE end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT cur_due past_due tot_due tot_due_fee X.X ;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT cur_due @;*/
/*		PUT past_due @;*/
/*		PUT tot_due @;*/
/*		PUT tot_due_fee $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET ARC_MXXXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;	*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_ATY_SEQ @;*/
/*		PUT LC_STA_ACTYXX @;*/
/*		PUT LX_ATY $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXX_HIST end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LD_FAT_APL LD_FAT_PST LD_FAT_EFF LD_STA_LONXX MMDDYYXX. ;*/
/*	FORMAT LA_FAT_PCL_FEE LA_FAT_NSI LA_FAT_LTE_FEE LA_FAT_ILG_PRI LA_FAT_CUR_PRI LA_FAT_NSI_ACR XX.X ;*/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;	*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ @;*/
/*		PUT LN_FAT_SEQ @;*/
/*		PUT LC_FAT_REV_REA @;*/
/*		PUT LD_FAT_APL @;*/
/*		PUT LD_FAT_PST @;*/
/*		PUT LD_FAT_EFF @;*/
/*		PUT LD_STA_LONXX @;*/
/*		PUT LC_STA_LONXX @;*/
/*		PUT LA_FAT_PCL_FEE @;*/
/*		PUT LA_FAT_NSI @;*/
/*		PUT LA_FAT_LTE_FEE @;*/
/*		PUT LA_FAT_ILG_PRI @;*/
/*		PUT LA_FAT_CUR_PRI @;*/
/*		PUT PC_FAT_TYP @;*/
/*		PUT PC_FAT_SUB_TYP @;*/
/*		PUT LA_FAT_NSI_ACR $;*/
/*	end;*/
/*	if eof then put "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LTXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT RT_RUN_SRT_DTS_PRC DATETIMEXX.X ;*/
/**/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;	*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT RT_RUN_SRT_DTS_PRC @; /*ends up in SQL server as a string because joins on the numeric date value weren't working*/*/
/*		PUT RN_SEQ_LTR_CRT_PRC @; */
/*		PUT RN_SEQ_REC_PRC @;*/
/*		PUT RT_RUN_SRT_DTS_PRC @; /*ends up in SQL server as a datetime*/*/
/*		PUT RM_DSC_LTR_PRC @;*/
/*		PUT RC_TYP_SBJ_PRC @;*/
/*		PUT RF_SBJ_PRC @;*/
/*		PUT RN_ENT_REQ_PRC @;*/
/*		PUT RN_ATY_SEQ_PRC @;*/
/*		PUT RI_REC_PRC @;*/
/*		PUT RX_REQ_ARA_X_PRC @;*/
/*		PUT RI_LTR_REQ_DEL_PRC @;*/
/*		PUT RC_LTR_REQ_SRC_PRC @;*/
/*		PUT RI_PRV_RUN_ERR_PRC @;*/
/*		PUT RF_COR_DOC_PRC @;*/
/*		PUT RI_LTR_OPT_ENC_PRC $;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET RPXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT PA_BBT_REB XX.X;*/
/**/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;	*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT PM_BBS_PGM @;*/
/*		PUT PN_BBS_PGM_SEQ @;*/
/*		PUT PF_BBS_PGM_TIR @;*/
/*		PUT PN_BBS_PGM_TIR_SEQ @;*/
/*		PUT PC_BBT_ICV @;*/
/*		PUT PN_BBT_PAY_ICV @;*/
/*		PUT PR_BBT_RDC @;*/
/*		PUT PC_BBT_REB_APL @;*/
/*		PUT PA_BBT_REB $;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LD_EFF_BEG_LNXX LD_STA_LNXX LD_BBS_DSQ MMDDYYXX.;*/
/**/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ @;*/
/*		PUT PM_BBS_PGM @;*/
/*		PUT PN_BBS_PGM_SEQ @;*/
/*		PUT LN_LON_BBS_PGM_SEQ @;*/
/*		PUT LD_EFF_BEG_LNXX @;*/
/*		PUT LI_BBS_ITD_LTR_SNT @;*/
/*		PUT LN_BBS_STS_PCV_PAY @;*/
/*		PUT LC_BBS_REB_MTD @;*/
/*		PUT LC_STA_LNXX @;*/
/*		PUT LD_STA_LNXX @;*/
/*		PUT LC_BBS_DSQ_REA @;*/
/*		PUT LD_BBS_DSQ @;*/
/*		PUT LC_BBS_ELG $;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET ADXX end = eof;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/*	FORMAT LD_FAT_ADJ_REQ MMDDYYXX.;*/
/**/
/*	IF _N_ = X THEN put "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LD_FAT_ADJ_REQ @;*/
/*		PUT LN_SEQ_FAT_ADJ_REQ @;*/
/*		PUT LC_TYP_FAT_ADJ_REQ @;*/
/*		PUT LC_STA_FAT_ADJ_REQ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET FSXX END = EOF;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/**/
/*	FORMAT LD_CRT_NEG_AMR_LTR DATEX.;*/
/**/
/*	IF _N_ = X THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ @;*/
/*		PUT LD_CRT_NEG_AMR_LTR @;*/
/*		PUT LA_PRJ_NEG_AMR_INT;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET DFR_LTR END = EOF;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/**/
/*	FORMAT LD_DFR_INF_CER DATEX.;*/
/*	FORMAT LD_CRT_REQ_DFR DATEX.;*/
/*	FORMAT LD_STA_DFRXX DATEX.;*/
/*	FORMAT LF_LST_DTS_DFXX DATETIMEXX.X;*/
/*	FORMAT LD_DFR_BEG DATEX.;*/
/*	FORMAT LD_DFR_END DATEX.;*/
/*	FORMAT LF_LST_DTS_LNXX DATETIMEXX.X;*/
/*	FORMAT LD_STA_LONXX DATEX.;*/
/*	FORMAT LD_DFR_APL DATEX.;*/
/**/
/*	IF _N_ = X THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ @;*/
/*		PUT LF_DFR_CTL_NUM @;*/
/*		PUT LN_DFR_OCC_SEQ @;*/
/*		PUT LC_DFR_TYP @;*/
/*		PUT LD_DFR_INF_CER @;*/
/*		PUT LD_CRT_REQ_DFR @;*/
/*		PUT LC_DFR_STA @;*/
/*		PUT LC_STA_DFRXX @;*/
/*		PUT LD_STA_DFRXX @;*/
/*		PUT LF_LST_DTS_DFXX @;*/
/*		PUT LD_DFR_BEG @;*/
/*		PUT LD_DFR_END @;*/
/*		PUT LC_STA_LONXX @;*/
/*		PUT LC_LON_LEV_DFR_CAP @;*/
/*		PUT LF_LST_DTS_LNXX @;*/
/*		PUT LD_STA_LONXX @;*/
/*		PUT LD_DFR_APL ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET FOR_LTR END = EOF;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/**/
/*	FORMAT LD_FOR_INF_CER DATEX.;*/
/*	FORMAT LD_CRT_REQ_FOR DATEX.;*/
/*	FORMAT LD_STA_FORXX DATEX.;*/
/*	FORMAT LF_LST_DTS_FBXX DATETIMEXX.X;*/
/*	FORMAT LD_FOR_BEG DATEX.;*/
/*	FORMAT LD_FOR_END DATEX.;*/
/*	FORMAT LF_LST_DTS_LNXX DATETIMEXX.X;*/
/*	FORMAT LD_STA_LONXX DATEX.;*/
/*	FORMAT LD_FOR_APL DATEX.;*/
/**/
/*	IF _N_ = X THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ @;*/
/*		PUT LF_FOR_CTL_NUM @;*/
/*		PUT LN_FOR_OCC_SEQ @;*/
/*		PUT LC_FOR_TYP @;*/
/*		PUT LD_FOR_INF_CER @;*/
/*		PUT LD_CRT_REQ_FOR @;*/
/*		PUT LC_FOR_STA @;*/
/*		PUT LC_STA_FORXX @;*/
/*		PUT LD_STA_FORXX @;*/
/*		PUT LF_LST_DTS_FBXX @;*/
/*		PUT LD_FOR_BEG @;*/
/*		PUT LD_FOR_END @;*/
/*		PUT LC_STA_LONXX @;*/
/*		PUT LC_LON_LEV_FOR_CAP @;*/
/*		PUT LF_LST_DTS_LNXX @;*/
/*		PUT LD_STA_LONXX @;*/
/*		PUT LD_FOR_APL ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET REPAY_SUM END = EOF;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/**/
/*	FORMAT LD_CRT_LONXX DATEX.;*/
/*	FORMAT LF_LST_DTS_LNXX DATETIMEXX.X;*/
/*	FORMAT LD_CRT_LONXX DATEX.;*/
/*	FORMAT LF_LST_DTS_LNXX DATETIMEXX.X;*/
/*	FORMAT LD_RPS_X_PAY_DU DATEX.;*/
/**/
/*	IF _N_ = X THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ @;*/
/*		PUT LN_RPS_SEQ @;*/
/*		PUT LN_GRD_RPS_SEQ @;*/
/*		PUT LD_CRT_LONXX @;*/
/*		PUT LC_TYP_SCH_DIS @;*/
/*		PUT LA_TOT_RPD_DIS @;*/
/*		PUT LR_INT_RPD_DIS @;*/
/*		PUT LA_ANT_CAP @;*/
/*		PUT LC_STA_LONXX @;*/
/*		PUT LF_LST_DTS_LNXX @;*/
/*		PUT LA_RPS_ISL @;*/
/*		PUT LD_CRT_LONXX @;*/
/*		PUT LN_RPS_TRM @;*/
/*		PUT LF_LST_DTS_LNXX @;*/
/*		PUT LD_RPS_X_PAY_DU ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET RSXX END = EOF;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/**/
/*	FORMAT BA_AGI BA_PMN_STD_TOT_PAY XX.X;*/
/*	FORMAT BD_CRT_RSXX BD_ANV_QLF_IBR DATEX.;*/
/**/
/*	IF _N_ = X THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT BD_CRT_RSXX @;*/
/*		PUT BN_IBR_SEQ @;*/
/*		PUT BF_CRY_YR @;*/
/*		PUT BC_ST_IBR @;*/
/*		PUT BC_STA_RSXX @;*/
/*		PUT BA_AGI @;*/
/*		PUT BN_MEM_HSE_HLD @;*/
/*		PUT BA_PMN_STD_TOT_PAY @;*/
/*		PUT BC_IBR_INF_SRC_VER @;*/
/*		PUT BF_SSN_SPO @;*/
/*		PUT BC_IRS_TAX_FIL_STA @;*/
/*		PUT BI_JNT_BR_SPO_RPY @;*/
/*		PUT BD_ANV_QLF_IBR ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/**/
/*DATA _NULL_;*/
/*	SET SCXX END = EOF;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/**/
/*	IF _N_ = X THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT IF_DOE_SCL @;*/
/*		PUT IM_SCL_SHO @;*/
/*		PUT IM_SCL_FUL @;*/
/*		PUT IC_TYP_SCL @;*/
/*		PUT IC_PRV_SCL_STA @;*/
/*		PUT IC_CUR_SCL_STA @;*/
/*		PUT II_SCL_CHS_PTC @;*/
/*		PUT IC_LEN_LNG_PGM_STY ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/**/
/*DATA _NULL_;*/
/*	SET FSXX END = EOF;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/**/
/*	IF _N_ = X THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT LN_SEQ @;*/
/*		PUT LN_INC_SUB_EVT_SEQ @;*/
/*		PUT LD_INC_SUB_EFF_BEG @;*/
/*		PUT LD_INC_SUB_EFF_END @;*/
/*		PUT LC_INC_SUB_STA @;*/
/*		PUT LR_SUB_RMN @;*/
/*		PUT LF_LST_USR_FSXX @;*/
/*		PUT LF_LST_DTS_FSXX @;*/
/*		PUT LF_CRT_USR_FSXX @;*/
/*		PUT LD_CRT_FSXX @;*/
/*		PUT LC_STA_FSXX @;*/
/*		PUT LD_STA_FSXX ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/**/
/*DATA _NULL_;*/
/*	SET WQXX END = EOF;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/**/
/*	IF _N_ = X THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT DF_SPE_ACC_ID @;*/
/*		PUT WF_QUE @;*/
/*		PUT WF_SUB_QUE @;*/
/*		PUT WN_CTL_TSK @;*/
/*		PUT PF_REQ_ACT @;*/
/*		PUT WD_ACT_REQ @;*/
/*		PUT WD_ACT_RQR @;*/
/*		PUT WC_CND_CTC @;*/
/*		PUT WD_INI_TSK @;*/
/*		PUT WT_INI_TSK @;*/
/*		PUT WF_USR_ASN_TSK @;*/
/*		PUT WF_USR_ASN_BY_TSK @;*/
/*		PUT WX_MSG_X_TSK @;*/
/*		PUT WX_MSG_X_TSK @;*/
/*		PUT WC_STA_WQUEXX @;*/
/*		PUT WF_LST_DTS_WQXX @;*/
/*		PUT LN_ATY_SEQ @;*/
/*		PUT WF_CRT_DTS_WQXX;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXXA END = EOF;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/**/
/*	IF _N_ = X THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT BF_SSN @ ;*/
/*		PUT LN_SEQ @ ;*/
/*		PUT BN_EFT_SEQ @ ;*/
/*		PUT LF_EFT_OCC_DTS @ ;*/
/*		PUT LF_LST_DTS_LNXX @ ;*/
/*		PUT LD_EFT_EFF_BEG @ ;*/
/*		PUT LD_EFT_EFF_END @ ;*/
/*		PUT LC_EFT_SUS_REA @ ;*/
/*		PUT LC_STA_LNXX @ ;*/
/*		PUT LR_EFT_RDC @ ;*/
/*		PUT LI_EFT_RIR_MNL_OVR @ ;*/
/*		PUT LI_EFT_RST @ ;*/
/*		PUT LF_LST_USR_LNXX @ ;*/
/*		PUT LF_LST_SRC_LNXX;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET OWXX END = EOF;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/**/
/*	IF _N_ = X THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT IF_LON_SLE @ ;*/
/*		PUT IC_LON_SLE_STA @ ;*/
/*		PUT IC_LON_SLE_TYP @ ;*/
/*		PUT ID_LON_SLE @ ;*/
/*		PUT IF_SLL_OWN @ ;*/
/*		PUT IM_SLL_CNC_X @ ;*/
/*		PUT IM_SLL_CNC_LST @ ;*/
/*		PUT IN_SLL_CNC_PHN_ARA @ ;*/
/*		PUT IN_SLL_CNC_PHN_XCH @ ;*/
/*		PUT IN_SLL_CNC_PHN_LCL @ ;*/
/*		PUT IN_SLL_CNC_PHN_XTN @ ;*/
/*		PUT IF_SLL_BND_ISS @ ;*/
/*		PUT ID_SLL_LON_SLE_APV @ ;*/
/*		PUT IF_BUY_OWN @ ;*/
/*		PUT IM_BUY_CNC_X @ ;*/
/*		PUT IM_BUY_CNC_LST @ ;*/
/*		PUT IN_BUY_CNC_PHN_ARA @ ;*/
/*		PUT IN_BUY_CNC_PHN_XCH @ ;*/
/*		PUT IN_BUY_CNC_PHN_LCL @ ;*/
/*		PUT IN_BUY_CNC_PHN_XTN @ ;*/
/*		PUT IF_BUY_BND_ISS @ ;*/
/*		PUT ID_BUY_LON_SLE_APV @ ;*/
/*		PUT IM_MKT_CNC_X @ ;*/
/*		PUT IM_MKT_CNC_LST @ ;*/
/*		PUT IN_MKT_CNC_PHN_ARA @ ;*/
/*		PUT IN_MKT_CNC_PHN_XCH @ ;*/
/*		PUT IN_MKT_CNC_PHN_LCL @ ;*/
/*		PUT IN_MKT_CNC_PHN_XTN @ ;*/
/*		PUT IM_TRF_CNC_X @ ;*/
/*		PUT IM_TRF_CNC_LST @ ;*/
/*		PUT IN_TRF_CNC_PHN_ARA @ ;*/
/*		PUT IN_TRF_CNC_PHN_XCH @ ;*/
/*		PUT IN_TRF_CNC_PHN_LCL @ ;*/
/*		PUT IN_TRF_CNC_PHN_XTN @ ;*/
/*		PUT IM_LEG_CNC_X @ ;*/
/*		PUT IM_LEG_CNC_LST @ ;*/
/*		PUT IN_LEG_CNC_PHN_ARA @ ;*/
/*		PUT IN_LEG_CNC_PHN_XCH @ ;*/
/*		PUT IN_LEG_CNC_PHN_LCL @ ;*/
/*		PUT IN_LEG_CNC_PHN_XTN @ ;*/
/*		PUT IC_FEE_ORG_RSB @ ;*/
/*		PUT II_ACP_NEW_LON_SLE @ ;*/
/*		PUT IA_LON_TOT_MAX @ ;*/
/*		PUT II_INT_ICL @ ;*/
/*		PUT IN_LON_MAX @ ;*/
/*		PUT II_SLE_LTR_TRG @ ;*/
/*		PUT IF_SLE_LTR_SPC @ ;*/
/*		PUT IF_LST_DTS_OWXX @ ;*/
/*		PUT IF_BUY_POR @ ;*/
/*		PUT IC_SLL_PNT_LOC @ ;*/
/*		PUT IC_BUY_PNT_LOC @ ;*/
/*		PUT IC_TIR_PCE_ASN @ ;*/
/*		PUT II_LTE_FEE_WOF @ ;*/
/*		PUT IC_LON_SLE_SEL_TYP @ ;*/
/*		PUT II_LTE_FEE_MAX_VAL @ ;*/
/*		PUT II_STP_SLE_LON_MAX @ ;*/
/*		PUT II_LEV_BR_LON_ELG @ ;*/
/*		PUT IC_SEL_CRI_USR_APV @ ;*/
/*		PUT ID_SEL_CRI_USR_APV @ ;*/
/*		PUT IF_SEL_CRI_USR_APV @ ;*/
/*		PUT ID_SEL_NXT_PLR @ ;*/
/*		PUT ID_LON_SLE_LST_PLR @ ;*/
/*		PUT IT_SLE_LST_PLR @ ;*/
/*		PUT IN_LON_SLE_BR_ELG @ ;*/
/*		PUT IN_LON_SLE_LON_ELG @ ;*/
/*		PUT IA_CUR_PRI_ELG_LON @ ;*/
/*		PUT IA_NSI_ELG_LON @ ;*/
/*		PUT IA_LTE_FEE_ELG_LON @ ;*/
/*		PUT IN_IVL_SCH_NXT_SLE @ ;*/
/*		PUT IC_IVL_SCH_NXT_SLE @ ;*/
/*		PUT IN_IVL_SCH_NXT_PLR @ ;*/
/*		PUT IC_IVL_SCH_NXT_PLR @ ;*/
/*		PUT IX_TRG_FIL_SEL_CRI @ ;*/
/*		PUT IC_RGN_RCV_DCV_LON @ ;*/
/*		PUT IX_DSC_BUY_OWN @ ;*/
/*		PUT LA_BR_PRI_BAL_SLE @ ;*/
/*		PUT LC_BR_PRI_REL_SLE @ ;*/
/*		PUT II_ORG_RGT_PUR_SLE @ ;*/
/*		PUT IF_GRP_SLE_KEY @ ;*/
/*		PUT IC_EFT_RIR_RSB @ ;*/
/*		PUT IX_LON_SLE_CMT @ ;*/
/*		PUT IC_BBS_RSB @ ;*/
/*		PUT IC_LON_SLE_SUB_TYP @ ;*/
/*		PUT IA_SLE_LVL_TRF_FEE @ ;*/
/*		PUT IC_TRF_FEE_TYP @ ;*/
/*		PUT IR_PRI_PER_FEE_RTE @ ;*/
/*		PUT II_ICL_CON_STP_PUR @ ;*/
/*		PUT ID_ECA_DCV_CRT_FIL @ ;*/
/*		PUT II_ECA_PUT_DCV_APV @ ;*/
/*		PUT ID_ECA_CRT_BIL_SLE @ ;*/
/*		PUT ID_CDU_REM_NTF @ ;*/
/*		PUT IC_FED_PGM_YR @ ;*/
/*		PUT IF_FLS_DEA @ ;*/
/*		PUT IF_DEA_IST_LIN_HLD @ ;*/
/*		PUT II_LON_SLE_ICL_IDT @ ;*/
/*		PUT II_ICL_CON_GRP_RLP @ ;*/
/*		PUT LI_PCV_OWN_EFF_DTE @ ;*/
/*		PUT II_PRE_SLE_LTR @ ;*/
/*		PUT IC_DLA_CAN_LTR @ ;*/
/*		PUT IC_PRE_SLE_LST_PRC;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET WKXJ END = EOF;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/**/
/*	IF _N_ = X THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT IF_LON_SLE @ ;*/
/*		PUT ID_LON_SLE_LST_PLR @ ;*/
/*		PUT IT_SLE_LST_PLR @ ;*/
/*		PUT BF_SSN @ ;*/
/*		PUT LN_SEQ @ ;*/
/*		PUT LA_CUR_PRI @ ;*/
/*		PUT LA_NSI_OTS @ ;*/
/*		PUT LA_LTE_FEE_OTS ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXX END = EOF;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/**/
/*	IF _N_ = X THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT BF_SSN @ ;*/
/*		PUT LN_SEQ @ ;*/
/*		PUT LN_FAT_SEQ @ ;*/
/*		PUT LD_RMT_BCH_INI @ ;*/
/*		PUT LC_RMT_BCH_SRC_IPT @ ;*/
/*		PUT LN_RMT_BCH_SEQ @ ;*/
/*		PUT LN_RMT_SEQ @ ;*/
/*		PUT LN_RMT_ITM @ ;*/
/*		PUT LN_RMT_ITM_SEQ @ ;*/
/*		PUT LF_LST_DTS_LNXX ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET RMXX END = EOF;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/**/
/*	IF _N_ = X THEN PUT "XXJANXXXX";*/
/*	DO;*/
/*		PUT LD_RMT_BCH_INI @ ;*/
/*		PUT LC_RMT_BCH_SRC_IPT @ ;*/
/*		PUT LN_RMT_BCH_SEQ @ ;*/
/*		PUT LN_RMT_SEQ @ ;*/
/*		PUT LN_RMT_ITM @ ;*/
/*		PUT LN_RMT_ITM_SEQ @ ;*/
/*		PUT LC_RMT_STA @ ;*/
/*		PUT LA_BR_RMT @ ;*/
/*		PUT LC_RMT_REV_REA @ ;*/
/*		PUT LI_RMT_PD_AHD @ ;*/
/*		PUT LD_RMT_PAY_EFF @ ;*/
/*		PUT LF_RMT_EDS @ ;*/
/*		PUT LF_RMT_IST @ ;*/
/*		PUT LX_SPS_REA @ ;*/
/*		PUT LD_RMT_SPS @ ;*/
/*		PUT LD_RMT_PST @ ;*/
/*		PUT LD_UPD_REMTXX @ ;*/
/*		PUT LD_CRT_REMTXX @ ;*/
/*		PUT LD_RMT_BCH_INI_REV @ ;*/
/*		PUT LC_RMT_BCH_IPT_REV @ ;*/
/*		PUT LN_RMT_BCH_SEQ_REV @ ;*/
/*		PUT LN_RMT_SEQ_REV @ ;*/
/*		PUT LN_RMT_ITM_REV @ ;*/
/*		PUT LN_RMT_ITM_SEQ_REV @ ;*/
/*		PUT LF_LST_DTS_RMXX @ ;*/
/*		PUT PC_FAT_TYP @ ;*/
/*		PUT PC_FAT_SUB_TYP @ ;*/
/*		PUT BF_SSN @ ;*/
/*		PUT LD_BIL_CRT @ ;*/
/*		PUT LN_SEQ_BIL_WI_DTE @ ;*/
/*		PUT LI_DPS_RMT_PIO_CVN @ ;*/
/*		PUT BN_CPN_BK_SEQ @ ;*/
/*		PUT LD_BIL_CPN_DU @ ;*/
/*		PUT LI_PHD_PAS_DU @ ;*/
/*		PUT LI_PHD_PAS_RPS @ ;*/
/*		PUT LI_PHD_PAS_OPT @ ;*/
/*		PUT LI_PHD_BCH_OVR @ ;*/
/*		PUT LC_RMT_REV_REA_REJ @ ;*/
/*		PUT LC_STP_SPS_OVR @ ;*/
/*		PUT LC_RMT_PAY_SRC @ ;*/
/*		PUT LC_RMT_SPS_OVR_FRD @ ;*/
/*		PUT LI_DPL_ACC_RMT @ ;*/
/*		PUT LC_WAV_MSC_FEE_ASS @ ;*/
/*		PUT LD_SCH_RMT_PAY @ ;*/
/*		PUT LF_LCK_BOX_TRC @ ;*/
/*		PUT LF_BR_RMT_SCH_NUM @ ;*/
/*		PUT LC_BR_RMT_SCH_TYP @ ;*/
/*		PUT LI_CON_SPS_OVR @ ;*/
/*		PUT LD_RMT_SCH_NUM_DPS ;*/
/*	END;*/
/*	IF EOF THEN PUT "XXJANXXXX";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXXA END = EOF;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/**/
/*	IF _N_ = X THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT BF_SSN @ ;*/
/*		PUT LN_SEQ @ ;*/
/*		PUT LC_STA_LONXX @ ;*/
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
/*		PUT LA_RXX_INT_PD @ ;*/
/*		PUT LA_RXX_INT_MAX @ ;*/
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
/*		PUT LD_STA_LONXX @ ;*/
/*		PUT LD_LON_ACL_ADD @ ;*/
/*		PUT LD_LON_EFF_ADD @ ;*/
/*		PUT LF_DOE_SCL_ORG @ ;*/
/*		PUT LC_PCV_DIS_STA @ ;*/
/*		PUT LC_RPR_TYP @ ;*/
/*		PUT LF_RGL_CAT_LPXX @ ;*/
/*		PUT LF_RGL_CAT_LPXX @ ;*/
/*		PUT LF_RGL_CAT_LPXX @ ;*/
/*		PUT LF_RGL_CAT_LPXX @ ;*/
/*		PUT LF_RGL_CAT_LPXX @ ;*/
/*		PUT LF_RGL_CAT_LPXX @ ;*/
/*		PUT LF_RGL_CAT_LPXX @ ;*/
/*		PUT LF_RGL_CAT_LPXX @ ;*/
/*		PUT LF_RGL_CAT_LPXX @ ;*/
/*		PUT LF_RGL_CAT_LPXX @ ;*/
/*		PUT LF_RGL_CAT_LPXX @ ;*/
/*		PUT LF_RGL_CAT_LPXX @ ;*/
/*		PUT LF_RGL_CAT_LPXX @ ;*/
/*		PUT LF_RGL_CAT_LPXX @ ;*/
/*		PUT LF_RGL_CAT_LPXX @ ;*/
/*		PUT LD_SCL_CLS_NTF @ ;*/
/*		PUT LD_ILG_NTF @ ;*/
/*		PUT LF_LON_CUR_OWN @ ;*/
/*		PUT LF_LST_DTS_LNXX @ ;*/
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
/*		PUT LD_LON_X_DSB @ ;*/
/*		PUT LC_ACA_GDE_LEV @ ;*/
/*		PUT LD_NEW_SYS_CVN @ ;*/
/*		PUT LC_SCY_PGA_PGM_YR @ ;*/
/*		PUT IC_HSP_CSE @ ;*/
/*		PUT LI_TLX_XXX_XCL_CON @ ;*/
/*		PUT LI_DFR_REQ_ON_APL @ ;*/
/*		PUT LI_LN_PT_COM_APL @ ;*/
/*		PUT LN_SEQ_RPR @ ;*/
/*		PUT LR_WIR_CON_LON @ ;*/
/*		PUT LR_INT_RDC_PGM_DSU @ ;*/
/*		PUT LI_X_TME_BR @ ;*/
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
/*		PUT LF_RGL_CAT_LPXX @ ;*/
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
/*		PUT LC_SST_LONXX @ ;*/
/*		PUT LF_RGL_CAT_LPXX @ ;*/
/*		PUT LI_MN_PSD_BS @ ;*/
/*		PUT LF_CRD_RTE_SRE @ ;*/
/*		PUT LF_ESG_SRC @ ;*/
/*		PUT PC_PNT_YR @ ;*/
/*		PUT LF_OWN_BND_ISS_TEX @ ;*/
/*		PUT LF_OWN_BND_ISS_TEX @ ;*/
/*		PUT LF_OWN_BND_ISS_TEX @ ;*/
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
/*		PUT LC_ELG_XX_SPA_BIL @ ;*/
/*		PUT LC_SGM_COS_PRC @ ;*/
/*		PUT LD_GTR_DR_DCH_CER @ ;*/
/*		PUT LD_RPD_ELY_CL_BEG @ ;*/
/*		PUT LD_RPD_ELY_CL_END @ ;*/
/*		PUT LF_GTR_RFR_XTN @ ;*/
/*		PUT LA_MSC_FEE_OTS @ ;*/
/*		PUT LA_MSC_FEE_PCV_OTS @ ;*/
/*		PUT LF_LON_GRP_WI_BR @ ;*/
/*		PUT LC_TLX_IBR_ELG @ ;*/
/*		PUT LD_LON_IBR_ENT @ ;*/
/*		PUT LC_LON_IBR_RPY_TYP @ ;*/
/*		PUT LI_BYP_COL_OUT_SRC @ ;*/
/*		PUT LI_BR_GRP_RLP @ ;*/
/*		PUT LI_OO_PST_ENR_DFR @ ;*/
/*		PUT LD_OO_PST_ENR_DFR @ ;*/
/*		PUT LF_FED_CLC_RSK @ ;*/
/*		PUT LF_FED_FFY_X_DSB @ ;*/
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
/*		PUT LC_IDR_ELG_CRI ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXX END = EOF;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/**/
/*	IF _N_ = X THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT BF_SSN @ ;*/
/*		PUT LN_SEQ @ ;*/
/*		PUT LN_FAT_SEQ @ ;*/
/*		PUT IF_LON_SLE @ ;*/
/*		PUT LF_LST_DTS_LNXX @ ;*/
/*		PUT IF_SLL_OWN_SLD @ ;*/
/*		PUT IF_BUY_OWN_SLD @ ;*/
/*		PUT LA_STD_STD_ISL_DCV ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXXA END = EOF;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/**/
/*	IF _N_ = X THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT BF_SSN @ ;*/
/*		PUT LN_SEQ @ ;*/
/*		PUT LN_RPS_SEQ @ ;*/
/*		PUT LA_RPD_INT_DIS @ ;*/
/*		PUT LR_APR_RPD_DIS @ ;*/
/*		PUT LA_TOT_RPD_DIS @ ;*/
/*		PUT LA_CPI_RPD_DIS @ ;*/
/*		PUT LR_INT_RPD_DIS @ ;*/
/*		PUT LA_ANT_CAP @ ;*/
/*		PUT LD_GRC_PRD_END @ ;*/
/*		PUT LD_CRT_LONXX @ ;*/
/*		PUT LC_STA_LONXX @ ;*/
/*		PUT LF_LST_DTS_LNXX @ ;*/
/*		PUT LC_TYP_SCH_DIS @ ;*/
/*		PUT LA_ACR_INT_RPD @ ;*/
/*		PUT LA_ANT_SUP_FEE @ ;*/
/*		PUT LN_RPD_MAX_TRM_REQ @ ;*/
/*		PUT LD_RPD_MAX_TRM_SR @ ;*/
/*		PUT LC_RPD_INA_REA @ ;*/
/*		PUT LC_RPD_DIS @ ;*/
/*		PUT LR_CLC_INC_SCH @ ;*/
/*		PUT LA_CLC_RPY_SCH @ ;*/
/*		PUT LI_ICR_RPD_NEG_AMR $ ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET LNXX END = EOF;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/**/
/*	IF _N_ = X THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT BF_SSN @ ;*/
/*		PUT LN_SEQ @ ;*/
/*		PUT LN_RPS_SEQ @ ;*/
/*		PUT LN_GRD_RPS_SEQ @ ;*/
/*		PUT LA_RPS_ISL @ ;*/
/*		PUT LD_CRT_LONXX @ ;*/
/*		PUT LN_RPS_TRM @ ;*/
/*		PUT LF_LST_DTS_LNXX @ ;*/
/*		PUT LA_PRI_RDC_GRD @ ;*/
/*		PUT LN_PRI_RDC_GRD_TRM @ ;*/
/*		PUT LA_PRI_ATU_PAY @ ;*/
/*		PUT LD_RPYE_FGV $ ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/
/**/
/*DATA _NULL_;*/
/*	SET RSXXA END = EOF;*/
/*	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;*/
/**/
/*	IF _N_ = X THEN PUT "-Begin-";*/
/*	DO;*/
/*		PUT BF_SSN @ ;*/
/*		PUT BD_CRT_RSXX @ ;*/
/*		PUT BN_IBR_SEQ @ ;*/
/*		PUT BF_CRT_USR_RSXX @ ;*/
/*		PUT BF_CRY_YR @ ;*/
/*		PUT BC_ST_IBR @ ;*/
/*		PUT BC_STA_RSXX @ ;*/
/*		PUT BA_AGI @ ;*/
/*		PUT BN_MEM_HSE_HLD @ ;*/
/*		PUT BA_PMN_STD_TOT_PAY @ ;*/
/*		PUT BC_IBR_INF_SRC_VER @ ;*/
/*		PUT BF_LST_DTS_RSXX @ ;*/
/*		PUT BF_SSN_SPO @ ;*/
/*		PUT BC_IRS_TAX_FIL_STA @ ;*/
/*		PUT BI_JNT_BR_SPO_RPY @ ;*/
/*		PUT BD_ANV_QLF_IBR @ ;*/
/*		PUT BC_DOC_SNT_BR_IDR @ ;*/
/*		PUT BC_BR_REQ_PLN_RPYE @ ;*/
/*		PUT BC_BR_GDE_LVL_RPYE @ ;*/
/*		PUT BI_REQ_DFR_FOR_CUT @ ;*/
/*		PUT BC_DFR_FOR_CUT_TYP $ ;*/
/*	END;*/
/*	IF EOF THEN PUT "-End-";*/
/*RUN;*/

;

DATA _NULL_;
	SET LNXXA END = EOF;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;

	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT BF_SSN @ ;
		PUT LN_SEQ @ ;
		PUT PM_BBS_PGM @ ;
		PUT PN_BBS_PGM_SEQ @ ;
		PUT LN_LON_BBS_PGM_SEQ @ ;
		PUT LD_EFF_BEG_LNXX @ ;
		PUT LI_BBS_ITD_LTR_SNT @ ;
		PUT LN_BBS_STS_PCV_PAY @ ;
		PUT LC_BBS_REB_MTD @ ;
		PUT LC_STA_LNXX @ ;
		PUT LD_STA_LNXX @ ;
		PUT LC_BBT_TYS_ASS @ ;
		PUT LC_BBS_DSQ_REA @ ;
		PUT LD_BBS_DSQ @ ;
		PUT LC_BBS_ELG @ ;
		PUT LC_BBT_PRC_RBD @ ;
		PUT LD_BBS_RPD_WDO_END @ ;
		PUT LC_BBS_BCH_PRC @ ;
		PUT LF_LST_USR_LNXX @ ;
		PUT LF_LST_DTS_LNXX @ ;
		PUT LN_BBS_PCV_PAY_MOT @ ;
		PUT LD_BBS_ICV_REQ @ ;
		PUT LD_BBS_DSQ_APL @ ;
		PUT LI_BBS_PCV_LTE_PAY $ ;
	END;
	IF EOF THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET LNXXA END = EOF;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;

	IF _N_ = X THEN PUT "-Begin-";
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
		PUT LC_STA_LNXX @ ;
		PUT LD_STA_LNXX @ ;
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
		PUT LF_LST_USR_LNXX @ ;
		PUT LF_LST_DTS_LNXX @ ;
		PUT LD_LON_BBT_ELG_FNL @ ;
		PUT LD_BBT_DLQ_MOT_STS @ ;
		PUT LD_BBT_PIF_MOT_STS @ ;
		PUT LN_BBT_DLQ_MOT_OVR @ ;
		PUT LN_BBT_PIF_MOT_OVR $ ;
	END;
	IF EOF THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET RPXX END = EOF;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;

	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT IF_OWN @ ;
		PUT PN_EFT_RIR_OWN_SEQ @ ;
		PUT IC_LON_PGM @ ;
		PUT IF_GTR @ ;
		PUT PD_LON_X_DSB @ ;
		PUT PF_DOE_SCL_ORG @ ;
		PUT PC_ST_BR_RSD_APL @ ;
		PUT PD_EFT_RIR_EFF_BEG @ ;
		PUT PD_EFT_RIR_EFF_END @ ;
		PUT PC_EFT_RIR_STA @ ;
		PUT PD_EFT_RIR_STA @ ;
		PUT PI_EFT_RIR_PRC @ ;
		PUT PC_EFT_NSF_LTR_REQ @ ;
		PUT PR_EFT_RIR @ ;
		PUT PF_LST_USR_RPXX @ ;
		PUT PF_LST_DTS_RPXX @ ;
		PUT PC_EFT_RIR_PNT_YR @ ;
		PUT PD_EFT_BBS_LOT_BEG @ ;
		PUT PD_EFT_BBS_GTE_DTE @ ;
		PUT PD_EFT_BBS_RPD_SR @ ;
		PUT PD_EFT_BBS_LCO_RCV @ ;
		PUT PN_EFT_BBS_NSF_LMT @ ;
		PUT PC_EFT_BBS_NSF_PRC @ ;
		PUT PN_EFT_BBS_NSF_MTH @ ;
		PUT PC_EFT_BBS_FED @ ;
		PUT PI_EFT_RIR_RPY_X $ ;
	END;
	IF EOF THEN PUT "-End-";
RUN;
