/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

FILENAME REPORTZ "&RPTLIB/UNWDWX.NWDWXRZ";
FILENAME REPORTX "&RPTLIB/UNWDWX.NWDWXRX";*LNXX;
FILENAME REPORTX "&RPTLIB/UNWDWX.NWDWXRX";*LNXX;
FILENAME REPORTX "&RPTLIB/UNWDWX.NWDWXRX";*DFXX;
FILENAME REPORTX "&RPTLIB/UNWDWX.NWDWXRX";*FBXX;
FILENAME REPORTX "&RPTLIB/UNWDWX.NWDWXRX";*PDXX;
FILENAME REPORTX "&RPTLIB/UNWDWX.NWDWXRX";*DLXXX;
FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*LNXX;
FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*LNXX;
FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*LNXX;
FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*LNXX;
FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*ARC_IND;
FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*LNXX;
FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*LNXX;
FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*PDXX;
FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*PDXX;
FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*PDXX;
FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*DWXX;
FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*ARC_MXXXX;
FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*ADXX;
FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*FSXX;
/*FILENAME REPORTXX "&RPTLIB/UNWDWX.NWDWXRXX";*LNXX;*/

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
LAST_RUN = TODAY() - X;	*COMMENT FOR PROD, IT WILL READ THE DATE FROM A TABLE;
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
	;
QUIT;
PROC SORT DATA=SSNXACC; BY BF_SSN; RUN;

/*******************************************
* ENDORSER DATA
********************************************/
PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE LNXX AS
	SELECT *
	FROM CONNECTION TO DBX 
	(
		SELECT 
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LC_STA_LONXX,
			LNXX.LC_EDS_TYP
		FROM
			PKUB.LNXX_EDS LNXX
		WHERE
			LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS

	FOR READ ONLY WITH UR
	);
DISCONNECT FROM DBX;
QUIT;
%SSNXACC(LNXX,LN_SEQ);

/*******************************************
* BILLING DATA
********************************************/
PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE LNXX AS
	SELECT *
	FROM CONNECTION TO DBX 
	(
		SELECT 
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LD_BIL_CRT,
			LNXX.LN_SEQ_BIL_WI_DTE,
			LNXX.LN_BIL_OCC_SEQ,
			LNXX.LD_BIL_DU_LON,
			LNXX.LC_STA_LONXX,
			COALESCE(LNXX.LA_BIL_CUR_DU,X) AS LA_BIL_CUR_DU,
			COALESCE(LNXX.LA_BIL_PAS_DU,X) AS LA_BIL_PAS_DU,
			COALESCE(LNXX.LA_TOT_BIL_STS,X) AS LA_TOT_BIL_STS,
			BLXX.LC_BIL_MTD,
			BLXX.LC_IND_BIL_SNT,
			BLXX.LC_STA_BILXX,
			LNXX.LN_FAT_SEQ,
			LNXX.LD_FAT_EFF,
			LNXX.LC_LON_STA_BIL,
			BLXX.LA_INT_PD_LST_STM,
			BLXX.LA_FEE_PD_LST_STM,
			BLXX.LA_PRI_PD_LST_STM,
			BLXX.LA_INT_PD_LST_STM + BLXX.LA_FEE_PD_LST_STM + BLXX.LA_PRI_PD_LST_STM AS LA_TTL_PD_LST_STM,
			LNXX.LA_LTE_FEE_OTS_PRT
		FROM 
			PKUB.BLXX_BR_BIL BLXX
			INNER JOIN PKUB.LNXX_LON_BIL_CRF LNXX
				ON LNXX.BF_SSN = BLXX.BF_SSN
				AND LNXX.LD_BIL_CRT = BLXX.LD_BIL_CRT
				AND LNXX.LN_SEQ_BIL_WI_DTE = BLXX.LN_SEQ_BIL_WI_DTE
			LEFT JOIN PKUB.LNXX_BIL_LON_FAT LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LD_BIL_CRT = LNXX.LD_BIL_CRT
				AND LNXX.LN_SEQ_BIL_WI_DTE = LNXX.LN_SEQ_BIL_WI_DTE
				AND LNXX.LN_BIL_OCC_SEQ = LNXX.LN_BIL_OCC_SEQ
				AND LNXX.LA_BIL_CUR_DU = LNXX.LA_TOT_BIL_STS
			LEFT JOIN PKUB.LNXX_FIN_ATY LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LN_FAT_SEQ = LNXX.LN_FAT_SEQ
		WHERE 
			LNXX.LC_BIL_TYP_LON = 'P' 
			AND LNXX.LD_LST_DTS_LNXX >= &LAST_RUNPASS

	FOR READ ONLY WITH UR
	);
DISCONNECT FROM DBX;
QUIT; 
PROC SORT DATA=LNXX; BY BF_SSN LN_SEQ LD_BIL_CRT LN_SEQ_BIL_WI_DTE LN_BIL_OCC_SEQ LN_FAT_SEQ; RUN;

DATA LNXX(DROP=LN_BIL_OCC_SEQ LN_FAT_SEQ); 
	SET LNXX;
	BY BF_SSN LN_SEQ LD_BIL_CRT LN_SEQ_BIL_WI_DTE ;
	IF LAST.LN_SEQ_BIL_WI_DTE;
RUN;
%SSNXACC(LNXX,LN_SEQ LD_BIL_CRT LN_SEQ_BIL_WI_DTE);

/*******************************************
* DEFERMENT DATA
********************************************/
PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE DFXX AS
	SELECT *
	FROM CONNECTION TO DBX 
	(
		SELECT 
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LF_DFR_CTL_NUM,
			LNXX.LN_DFR_OCC_SEQ,
			DFXX.LC_DFR_TYP,
			DFXX.LD_DFR_INF_CER,
			LNXX.LD_DFR_BEG,
			LNXX.LD_DFR_END,
			LNXX.LC_LON_LEV_DFR_CAP,
			LNXX.LC_STA_LONXX,
			DFXX.LC_DFR_STA,
			DFXX.LC_STA_DFRXX
		FROM 
			PKUB.LNXX_BR_DFR_APV LNXX
			INNER JOIN PKUB.DFXX_BR_DFR_REQ DFXX
				ON LNXX.BF_SSN = DFXX.BF_SSN
				AND LNXX.LF_DFR_CTL_NUM = DFXX.LF_DFR_CTL_NUM
		WHERE 
			LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS 
			OR DFXX.LF_LST_DTS_DFXX >= &LAST_RUNPASS

	FOR READ ONLY WITH UR
	);
DISCONNECT FROM DBX;
QUIT;
%SSNXACC(DFXX,LN_SEQ LF_DFR_CTL_NUM LN_DFR_OCC_SEQ);

/*******************************************
* FORBEARANCE DATA
********************************************/
PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE FBXX AS
	SELECT *
	FROM CONNECTION TO DBX 
	(
		SELECT 
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LF_FOR_CTL_NUM,
			LNXX.LN_FOR_OCC_SEQ,
			FBXX.LC_FOR_TYP,
			FBXX.LD_FOR_INF_CER,
			LNXX.LD_FOR_BEG,
			LNXX.LD_FOR_END,
			LNXX.LC_LON_LEV_FOR_CAP ,
			LNXX.LC_STA_LONXX,
			FBXX.LC_FOR_STA,
			FBXX.LC_STA_FORXX,
			COALESCE(FBXX.LA_REQ_RDC_PAY,X) AS LA_REQ_RDC_PAY,
			LNXX.LI_FOR_VRB_DFL_RUL
		FROM 
			PKUB.LNXX_BR_FOR_APV LNXX
			INNER JOIN PKUB.FBXX_BR_FOR_REQ FBXX
				ON LNXX.BF_SSN = FBXX.BF_SSN
				AND LNXX.LF_FOR_CTL_NUM = FBXX.LF_FOR_CTL_NUM
		WHERE
			LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS
			OR FBXX.LF_LST_DTS_FBXX >= &LAST_RUNPASS

	FOR READ ONLY WITH UR
	);
DISCONNECT FROM DBX;
QUIT;
%SSNXACC(FBXX,LN_SEQ LF_FOR_CTL_NUM LN_FOR_OCC_SEQ);

/*******************************************
* LOAN DATA
********************************************/
PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE LNXX AS
	SELECT *
	FROM CONNECTION TO DBX 
	(
		SELECT 
			LNXX.BF_SSN,
			LNXX.LN_SEQ, 
			LNXX.LC_STA_LONXX, 
			LNXX.LA_CUR_PRI, 
			LNXX.LA_LON_AMT_GTR, 
			LNXX.LD_END_GRC_PRD,
			LNXX.IC_LON_PGM, 
			LNXX.LD_LON_X_DSB, 
			LNXX.LD_PIF_RPT,
			LNXX.LC_SST_LONXX,
			LNXX.LF_LON_CUR_OWN,
			LNXX.LF_DOE_SCL_ORG
		FROM 
			PKUB.LNXX_LON LNXX
		WHERE
			LNXX.LC_STA_LONXX NOT IN ('P','J')

	FOR READ ONLY WITH UR
	);
DISCONNECT FROM DBX;
QUIT;
%SSNXACC(LNXX,LN_SEQ);

/*******************************************
* ACH DATA
********************************************/
PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE LNXX AS
	SELECT *
	FROM CONNECTION TO DBX 
	(
		SELECT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.BN_EFT_SEQ,
			LNXX.LD_EFT_EFF_END,
			LNXX.LC_STA_LNXX
		FROM
			PKUB.LNXX_EFT_TO_LON LNXX
		WHERE
			LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS

	FOR READ ONLY WITH UR
	);
DISCONNECT FROM DBX;
QUIT;
PROC SORT DATA=LNXX; BY BF_SSN LN_SEQ DESCENDING BN_EFT_SEQ; RUN;

DATA LNXX;
	SET LNXX;
	BY BF_SSN LN_SEQ;
	IF FIRST.LN_SEQ;
RUN;
%SSNXACC(LNXX,LN_SEQ);

/*******************************************
* REHAB DATA 
********************************************/
PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE LNXX AS
	SELECT *
	FROM CONNECTION TO DBX 
	(
		SELECT 
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LD_LON_RHB_PCV
		FROM
			PKUB.LNXX_RPD_PIO_CVN LNXX
		WHERE
			LNXX.LD_LON_RHB_PCV IS NOT NULL
			AND LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS

	FOR READ ONLY WITH UR
	);
DISCONNECT FROM DBX;
QUIT;
%SSNXACC(LNXX,LN_SEQ);

/*********************************************
* DISBURSEMENT
**********************************************/
PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE LNXX AS
	SELECT *
	FROM CONNECTION TO DBX 
	(
		SELECT 
			BF_SSN,
			LN_BR_DSB_SEQ,
			LA_DSB - COALESCE(LA_DSB_CAN,X) AS LA_DSB,
			LD_DSB,
			LC_DSB_TYP,
			LC_STA_LONXX,
			LN_SEQ,
			COALESCE(LA_DL_DSB_REB,X) - COALESCE(LA_DSB_REB_CAN,X) AS LA_DL_REBATE
		FROM 
			PKUB.LNXX_DSB 
		WHERE 
			LF_LST_DTS_LNXX >= &LAST_RUNPASS

	FOR READ ONLY WITH UR
	);
DISCONNECT FROM DBX;
QUIT;
%SSNXACC(LNXX,LN_BR_DSB_SEQ);

/*********************************************
* DELINQUENCY DATA
**********************************************/
PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE LNXX AS
	SELECT *
	FROM CONNECTION TO DBX
	(
		SELECT 
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LN_DLQ_SEQ,
			LNXX.LD_DLQ_OCC,
			CASE WHEN LNXX.LC_STA_LONXX ^= 'X' OR (LNXX.LC_STA_LONXX = 'X' AND LNXX.LD_DLQ_MAX ^= Current Date - X Day) THEN X ELSE LNXX.LN_DLQ_MAX END AS LN_DLQ_MAX,
			CASE WHEN LNXX.LC_STA_LONXX ^= 'X' OR (LNXX.LC_STA_LONXX = 'X' AND LNXX.LD_DLQ_MAX ^= Current Date - X Day) THEN NULL ELSE LNXX.LD_DLQ_MAX END AS LD_DLQ_MAX
		FROM
			PKUB.LNXX_LON_DLQ_HST LNXX
		WHERE
			LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS

	FOR READ ONLY WITH UR
	);
DISCONNECT FROM DBX;
QUIT;
PROC SORT DATA=LNXX; BY BF_SSN LN_SEQ DESCENDING LN_DLQ_SEQ; RUN;

DATA LNXX;
	SET LNXX(DROP=LN_DLQ_SEQ);
	BY BF_SSN LN_SEQ;
	IF FIRST.LN_SEQ;
RUN;
%SSNXACC(LNXX,LN_SEQ);

/*********************************************
* DEMOGRAPHICS
**********************************************/
PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE PDXX AS
	SELECT *
	FROM CONNECTION TO DBX 
	(
		SELECT DISTINCT 
			PDXX.DF_SPE_ACC_ID,
			PDXX.DF_PRS_ID AS BF_SSN,
			PDXX.DM_PRS_X,
			PDXX.DM_PRS_LST,
			PDXX.DM_PRS_MID,
			PDXX.DD_BRT
		FROM 
			PKUB.PDXX_PRS_NME PDXX
		WHERE 
			SUBSTR(DF_PRS_ID,X,X) != 'P'
			AND PDXX.DF_LST_DTS_PDXX >= &LAST_RUNPASS

	FOR READ ONLY WITH UR
	);
DISCONNECT FROM DBX;
QUIT;

PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE PDXX AS
	SELECT *
	FROM CONNECTION TO DBX 
	(
		SELECT DISTINCT 
			PDXX.DF_PRS_ID AS BF_SSN,
			PDXX.DX_STR_ADR_X,
			PDXX.DX_STR_ADR_X,
			PDXX.DM_CT,
			PDXX.DC_DOM_ST,
			PDXX.DF_ZIP_CDE,
			PDXX.DM_FGN_ST,
			PDXX.DM_FGN_CNY,
			PDXX.DD_VER_ADR,
			PDXX.DI_VLD_ADR
		FROM 
			PKUB.PDXX_PRS_ADR PDXX
		WHERE 
			PDXX.DC_ADR = 'L'
			AND PDXX.DF_LST_DTS_PDXX >= &LAST_RUNPASS

	FOR READ ONLY WITH UR
	);
DISCONNECT FROM DBX;
QUIT;
%SSNXACC(PDXX,);

PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE PDXX AS
	SELECT *
	FROM CONNECTION TO DBX
	(
		SELECT DISTINCT 
			PDXX.DF_PRS_ID AS BF_SSN,
			PDXX.DC_PHN,
			PDXX.DC_ALW_ADL_PHN,
			PDXX.DD_PHN_VER,
			PDXX.DI_PHN_VLD,
			PDXX.DN_DOM_PHN_ARA,
			PDXX.DN_DOM_PHN_XCH,
			PDXX.DN_DOM_PHN_LCL,
			PDXX.DN_PHN_XTN,
			PDXX.DN_FGN_PHN_CNY,
			PDXX.DN_FGN_PHN_CT,
			PDXX.DN_FGN_PHN_LCL
		FROM  
			PKUB.PDXX_PRS_PHN PDXX
		WHERE 
			PDXX.DF_LST_DTS_PDXX >= &LAST_RUNPASS

	FOR READ ONLY WITH UR
	);
DISCONNECT FROM DBX;
QUIT;
%SSNXACC(PDXX,DC_PHN);

PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE PDXX AS
	SELECT *
	FROM CONNECTION TO DBX
	(
		SELECT DISTINCT
			PDXX.DF_PRS_ID AS BF_SSN,
			PDXX.DC_ADR_EML,
			PDXX.DX_ADR_EML,
			PDXX.DD_VER_ADR_EML,
			PDXX.DI_VLD_ADR_EML,
			PDXX.DC_STA_PDXX
		FROM 
			PKUB.PDXX_PRS_ADR_EML PDXX
		WHERE 
			PDXX.DF_LST_DTS_PDXX >= &LAST_RUNPASS
			AND PDXX.DC_STA_PDXX = 'A'

	FOR READ ONLY WITH UR
	);
DISCONNECT FROM DBX;
QUIT;
PROC SORT DATA=PDXX; BY BF_SSN DC_ADR_EML DC_STA_PDXX ; RUN;

DATA PDXX(DROP=DC_STA_PDXX);
	SET PDXX;
	BY BF_SSN DC_ADR_EML;
	IF FIRST.DC_ADR_EML;
RUN;
%SSNXACC(PDXX,DC_ADR_EML);

/*********************************************
* ADDITIONAL LOAN DATA
**********************************************/
PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE DWXX AS
	SELECT *
	FROM CONNECTION TO DBX 
	(
		SELECT
			DWXX.BF_SSN,
			DWXX.LN_SEQ,
			DWXX.WC_DW_LON_STA,
			DWXX.WA_TOT_BRI_OTS,
			DWXX.WD_LON_RPD_SR,
			DWXX.WX_OVR_DW_LON_STA
		FROM
			PKUB.DWXX_DW_CLC_CLU DWXX

	FOR READ ONLY WITH UR
	);
DISCONNECT FROM DBX;
QUIT;
%SSNXACC(DWXX,LN_SEQ);

/*********************************************
* ACTIVITY HISTORY
**********************************************/
PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE ARCS AS
	SELECT *
	FROM CONNECTION TO DBX 
	(
		SELECT 
			AYXX.BF_SSN,
			AYXX.LN_ATY_SEQ,
			AYXX.PF_REQ_ACT,
			AYXX.LC_STA_ACTYXX,
			LX_ATY
		FROM
			PKUB.AYXX_BR_LON_ATY AYXX
			LEFT JOIN PKUB.AYXX_ATY_CMT AYXX
				ON AYXX.BF_SSN = AYXX.BF_SSN 
				AND AYXX.LN_ATY_SEQ = AYXX.LN_ATY_SEQ
				AND AYXX.LN_ATY_CMT_SEQ = X
		 	LEFT JOIN PKUB.AYXX_ATY_TXT AYXX
				ON AYXX.BF_SSN = AYXX.BF_SSN 
				AND AYXX.LN_ATY_SEQ = AYXX.LN_ATY_SEQ
				AND AYXX.LN_ATY_CMT_SEQ = AYXX.LN_ATY_CMT_SEQ
		WHERE 
			AYXX.PF_REQ_ACT IN ('DRLFA','KXADD','KXPHN','MXXXX')
			AND AYXX.LC_STA_ACTYXX = 'A'
			AND 
			(
				AYXX.LF_LST_DTS_AYXX >= &LAST_RUNPASS 
				OR AYXX.PF_REQ_ACT = 'DRLFA'
			)

	FOR READ ONLY WITH UR
	);

CREATE TABLE ARC_IND AS
	SELECT *
	FROM CONNECTION TO DBX
	(
		SELECT
			AYXX.BF_SSN,
			AYXX.LN_ATY_SEQ,
			AYXX.PF_REQ_ACT,
			AYXX.LC_STA_ACTYXX
		FROM 
			PKUB.AYXX_BR_LON_ATY AYXX
		WHERE 
			AYXX.PF_REQ_ACT IN ('SPHAN','VIPSS')
			AND AYXX.LC_STA_ACTYXX = 'A'
			AND AYXX.LF_LST_DTS_AYXX >= &LAST_RUNPASS

	FOR READ ONLY WITH UR
	);

CREATE TABLE DLXXX AS
	SELECT *
	FROM CONNECTION TO DBX
	(
		SELECT
			AYXX.BF_SSN,
			AYXX.LN_ATY_SEQ,
			AYXX.LD_ATY_REQ_RCV,
			AYXX.LC_STA_ACTYXX
		FROM
			PKUB.AYXX_BR_LON_ATY AYXX
		WHERE
			AYXX.PF_REQ_ACT = 'DLXXX'
			AND AYXX.LF_LST_DTS_AYXX >= &LAST_RUNPASS

	FOR READ ONLY WITH UR
	);
DISCONNECT FROM DBX;
QUIT;

DATA ARC_MXXXX(KEEP=BF_SSN LN_ATY_SEQ LC_STA_ACTYXX LX_ATY);
	SET ARCS;
	LENGTH DX_STR_ADR_X $XX. DX_STR_ADR_X $XX. DM_CT $XX. DC_DOM_ST $X. DF_ZIP_CDE $XX. DM_FGN_CNY $XX. COMMENTS $XXX.;
	LENGTH  PHNX PHNX PHNX $XX.;
	ARRAY ADR{X} $ DX_STR_ADR_X DX_STR_ADR_X DM_CT DC_DOM_ST DF_ZIP_CDE DM_FGN_CNY COMMENTS;
	ARRAY PHN{X} $ PHNX PHNX PHNX COMMENTS;
	IF PF_REQ_ACT = 'MXXXX' THEN OUTPUT ARC_MXXXX;
RUN;

%SSNXACC(ARC_MXXXX,LN_ATY_SEQ);
%SSNXACC(DLXXX,LN_ATY_SEQ);
%SSNXACC(ARC_IND,LN_ATY_SEQ); 

/*******************************************
* REPAYMENT SCHEDULE DATA
********************************************/
PROC SQL;
CREATE TABLE LNXX AS
	SELECT 
		LNXX.BF_SSN,
		LNXX.LN_SEQ,
		LNXX.LN_GRD_RPS_SEQ,
		CASE WHEN DWXX.WX_OVR_DW_LON_STA = 'LITIGATION' THEN X
			 ELSE LNXX.LN_RPS_TRM
		END AS LN_RPS_TRM,
		CASE WHEN DWXX.WX_OVR_DW_LON_STA = 'LITIGATION' THEN .
			 ELSE RSXX.LD_RPS_X_PAY_DU
		END AS LD_RPS_X_PAY_DU,
		RSXX.LD_SNT_RPD_DIS,
		LNXX.LD_CRT_LONXX,
		LNXX.LC_TYP_SCH_DIS,
		CASE WHEN DWXX.WX_OVR_DW_LON_STA = 'LITIGATION' THEN X
			 ELSE LNXX.LA_RPS_ISL
		END AS LA_RPS_ISL
	FROM
		PKUB.RSXX_BR_RPD RSXX
		INNER JOIN PKUB.LNXX_LON_RPS LNXX
			ON RSXX.BF_SSN = LNXX.BF_SSN
			AND RSXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
		INNER JOIN PKUB.LNXX_LON_RPS_SPF LNXX
			ON LNXX.BF_SSN = LNXX.BF_SSN
			AND LNXX.LN_SEQ = LNXX.LN_SEQ
			AND LNXX.LN_RPS_SEQ = LNXX.LN_RPS_SEQ
		INNER JOIN PKUB.DWXX_DW_CLC_CLU DWXX
			ON LNXX.BF_SSN = DWXX.BF_SSN
			AND LNXX.LN_SEQ = DWXX.LN_SEQ
	WHERE 
		LNXX.LC_STA_LONXX = 'A'
;
QUIT;
PROC SORT DATA=LNXX; BY BF_SSN LN_SEQ LN_GRD_RPS_SEQ; RUN;

DATA LNXX(DROP= A B C NEXT_SEQ LD_RPS_X_PAY_DU LN_GRD_RPS_SEQ);
	SET LNXX;
	FORMAT NEXT_SEQ DATEX.;
	BY BF_SSN LN_SEQ LN_GRD_RPS_SEQ;
	RETAIN NEXT_SEQ A C B;
	IF FIRST.LN_SEQ THEN DO;
		A = .;
		NEXT_SEQ = INTNX('MONTH',LD_RPS_X_PAY_DU,LN_RPS_TRM,'S');
		IF NEXT_SEQ > TODAY() THEN DO;
			A = LN_GRD_RPS_SEQ ;
			C = LA_RPS_ISL ;
		END;
	END;
	ELSE IF A= . THEN DO;
		NEXT_SEQ = INTNX('MONTH',NEXT_SEQ,LN_RPS_TRM,'S');
		IF NEXT_SEQ > TODAY() THEN DO;
			A = LN_GRD_RPS_SEQ ;
			C = LA_RPS_ISL ;
		END;
	END;
	IF FIRST.LN_SEQ THEN B = LN_RPS_TRM;
	ELSE B = B + LN_RPS_TRM;
	IF LAST.LN_SEQ THEN DO;
		LN_RPS_TRM = B;
		LA_RPS_ISL = C;
		LN_GRD_RPS_SEQ = A;
		DAY_DUE = DAY(LD_RPS_X_PAY_DU);
		OUTPUT;
	END;
RUN;
%SSNXACC(LNXX,LN_SEQ);

/*********************************************
* FINANCIAL ACTIVITY ADJUSTMENT
**********************************************/
PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE ADXX AS
	SELECT *
	FROM CONNECTION TO DBX 
	(
		SELECT
			BF_SSN,
			LD_FAT_ADJ_REQ,
			LN_SEQ_FAT_ADJ_REQ,
			LC_TYP_FAT_ADJ_REQ,  
			LC_STA_FAT_ADJ_REQ
		FROM
			PKUB.ADXX_PCV_ATY_ADJ ADXX
		WHERE
			LF_LST_DTS_ADXX >= &LAST_RUNPASS

	FOR READ ONLY WITH UR
	);
DISCONNECT FROM DBX;
QUIT;
%SSNXACC(ADXX,LD_FAT_ADJ_REQ LN_SEQ_FAT_ADJ_REQ);

/*********************************************
* LOAN SUBSIDY
**********************************************/
PROC SQL;
CONNECT TO DBX (DATABASE=&DB);
CREATE TABLE FSXX AS
	SELECT *
	FROM CONNECTION TO DBX 
	(
		SELECT DISTINCT
			FSXX.BF_SSN,
			FSXX.LN_SEQ,
			FSXX.LN_INC_SUB_EVT_SEQ,
			FSXX.LD_INC_SUB_EFF_BEG,
			FSXX.LD_INC_SUB_EFF_END,
			FSXX.LC_INC_SUB_STA,
			FSXX.LR_SUB_RMN,
			FSXX.LF_LST_USR_FSXX,
			FSXX.LF_LST_DTS_FSXX,
			FSXX.LF_CRT_USR_FSXX,
			FSXX.LD_CRT_FSXX,
			FSXX.LC_STA_FSXX,
			FSXX.LD_STA_FSXX
		FROM
			PKUB.FSXX_SUB_LOS_RNS FSXX
		WHERE
			FSXX.LF_LST_DTS_FSXX >= &LAST_RUNPASS	

	FOR READ ONLY WITH UR
	);
DISCONNECT FROM DBX;
QUIT;
%SSNXACC(FSXX, LN_SEQ);

/*********************************************
* LNXX
**********************************************/
/*PROC SQL;*/
/*CONNECT TO DBX (DATABASE=&DB);*/
/*CREATE TABLE LNXX AS*/
/*	SELECT **/
/*	FROM CONNECTION TO DBX */
/*	(*/
/*		SELECT*/
/*			LNXX.**/
/*		FROM*/
/*			PKUB.LNXX_LON_PAY_FAT LNXX*/
/*		WHERE*/
/*			LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS*/
/**/
/*	FOR READ ONLY WITH UR*/
/*	);*/
/*DISCONNECT FROM DBX;*/
/*QUIT;*/

/*DATA SAS_TAB.LASTRUN_JOBS;*/
/*	SET SAS_TAB.LASTRUN_JOBS;*/
/*	IF JOB = 'UTNWDWX' */
/*		THEN LAST_RUN = TODAY();*/
/*RUN;*/
ENDRSUBMIT;

LIBNAME LEGEND 'T:\SAS\CDW';

DATA LNXX; SET LEGEND.LNXX; RUN; *X;
DATA LNXX; SET LEGEND.LNXX; RUN; *X;
DATA DFXX; SET LEGEND.DFXX; RUN; *X;
DATA FBXX; SET LEGEND.FBXX; RUN; *X;
DATA PDXX; SET LEGEND.PDXX; RUN; *X;
DATA DLXXX; SET LEGEND.DLXXX; RUN; *X;
DATA LNXX; SET LEGEND.LNXX; RUN; *XX;
DATA LNXX; SET LEGEND.LNXX; RUN; *XX;
DATA LNXX; SET LEGEND.LNXX; RUN; *XX;
DATA LNXX; SET LEGEND.LNXX; RUN; *XX;
DATA ARC_IND; SET LEGEND.ARC_IND; RUN; *XX;
DATA LNXX; SET LEGEND.LNXX; RUN; *XX;
DATA LNXX; SET LEGEND.LNXX; RUN; *XX;
DATA PDXX; SET LEGEND.PDXX; RUN; *XX;
DATA PDXX; SET LEGEND.PDXX; RUN; *XX;
DATA PDXX; SET LEGEND.PDXX; RUN; *XX;
DATA DWXX; SET LEGEND.DWXX; RUN; *XX;
DATA ARC_MXXXX; SET LEGEND.ARC_MXXXX; RUN; *XX;
DATA ADXX; SET LEGEND.ADXX; RUN; *XX;
DATA FSXX; SET LEGEND.FSXX; RUN; *XX;
/*DATA LNXX; SET LEGEND.LNXX; RUN; *XX;*/

DATA _NULL_;
	SET LNXX END = eof;
	FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT LN_SEQ @;
		PUT LC_STA_LONXX $;
	END;
	IF eof THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET LNXX END = eof;
	FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	FORMAT LD_BIL_CRT LD_FAT_EFF LD_BIL_DU_LON MMDDYYXX. ;
	FORMAT 	LA_BIL_CUR_DU LA_BIL_PAS_DU	LA_TOT_BIL_STS LA_INT_PD_LST_STM LA_FEE_PD_LST_STM LA_PRI_PD_LST_STM LA_TTL_PD_LST_STM LA_LTE_FEE_OTS_PRT X.X;
	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT LN_SEQ @;
		PUT LD_BIL_CRT @;
		PUT LN_SEQ_BIL_WI_DTE @;
		PUT LD_FAT_EFF @;
		PUT LD_BIL_DU_LON @;
		PUT LC_STA_LONXX @;
		PUT LA_BIL_CUR_DU @;
		PUT LA_BIL_PAS_DU @;
		PUT LC_BIL_MTD @;
		PUT LC_IND_BIL_SNT @;
		PUT LC_STA_BILXX @;
		PUT LA_TOT_BIL_STS @;
		PUT LA_INT_PD_LST_STM @;
		PUT LA_FEE_PD_LST_STM @;
		PUT LA_PRI_PD_LST_STM @;
		PUT LA_TTL_PD_LST_STM @;
		PUT LA_LTE_FEE_OTS_PRT $;
	END;
	IF eof THEN PUT "-End-";
RUN; 

DATA _NULL_;
	SET DFXX END = eof;
	FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	FORMAT LD_DFR_INF_CER LD_DFR_BEG LD_DFR_END MMDDYYXX. ;
	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT LN_SEQ @;
		PUT LF_DFR_CTL_NUM @;
		PUT LN_DFR_OCC_SEQ @;
		PUT LC_DFR_TYP @;
		PUT LD_DFR_INF_CER @;
		PUT LD_DFR_BEG @;
		PUT LD_DFR_END @;
		PUT LC_LON_LEV_DFR_CAP @;
		PUT LC_STA_LONXX @;
		PUT LC_DFR_STA @;
		PUT LC_STA_DFRXX $;
	END;
	IF eof THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET FBXX END = eof;
	FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	FORMAT LD_FOR_INF_CER LD_FOR_BEG LD_FOR_END MMDDYYXX. LA_REQ_RDC_PAY X.X;
	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT LN_SEQ @;
		PUT LF_FOR_CTL_NUM @;
		PUT LN_FOR_OCC_SEQ @;
		PUT LC_FOR_TYP @;
		PUT LD_FOR_INF_CER @;
		PUT LD_FOR_BEG @;
		PUT LD_FOR_END @;
		PUT LC_LON_LEV_FOR_CAP @;
		PUT LC_STA_LONXX @;
		PUT LC_FOR_STA @;
		PUT LC_STA_FORXX @;
		PUT LA_REQ_RDC_PAY @;
		PUT LI_FOR_VRB_DFL_RUL $;
	END;
	IF eof THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET PDXX END = eof;
	FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	FORMAT DD_BRT MMDDYYXX.;
	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT BF_SSN @;
		PUT DM_PRS_X @;
		PUT DM_PRS_LST @;
		PUT DM_PRS_MID @;
		PUT DD_BRT $;
	END;
	IF eof THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET DLXXX END = eof;
	FILE REPORTX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	FORMAT LD_ATY_REQ_RCV MMDDYYXX.;
	IF _N_ = X THEN PUT "-Begin-";
	DO;	
		PUT DF_SPE_ACC_ID @;
		PUT LN_ATY_SEQ @;
		PUT LC_STA_ACTYXX @;
		PUT LD_ATY_REQ_RCV $;
	END;
	IF eof THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET LNXX END = eof;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	FORMAT LD_END_GRC_PRD LD_LON_X_DSB  LD_PIF_RPT MMDDYYXX. ;
	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT LN_SEQ @;
		PUT LC_STA_LONXX @;
		PUT LA_CUR_PRI @;
		PUT LA_LON_AMT_GTR @;
		PUT LD_END_GRC_PRD @;
		PUT IC_LON_PGM @;
		PUT LD_LON_X_DSB @;
		PUT LD_PIF_RPT @;
		PUT LC_SST_LONXX @;
		PUT LF_LON_CUR_OWN @;
		PUT LF_DOE_SCL_ORG $;
	END;
	IF eof THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET LNXX END = eof;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT LN_SEQ  @;
		PUT BN_EFT_SEQ @;
		PUT LC_STA_LNXX $;
	END;
	IF eof THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET LNXX END = eof;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	FORMAT LD_LON_RHB_PCV MMDDYYXX. ;
	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT LN_SEQ @;
		PUT LD_LON_RHB_PCV $;
	END;
	IF eof THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET LNXX END = eof;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	FORMAT LD_DSB MMDDYYXX.;
	FORMAT LA_DSB LA_DL_REBATE X.X;
	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT LN_BR_DSB_SEQ @;
		PUT LA_DSB @;
		PUT LD_DSB @;
		PUT LC_DSB_TYP @;
		PUT LC_STA_LONXX @;
		PUT LN_SEQ @;
		PUT LA_DL_REBATE $;
	END;
	IF eof THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET ARC_IND END = eof;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	IF _N_ = X THEN PUT "-Begin-";
	DO;	
		PUT DF_SPE_ACC_ID @;
		PUT LN_ATY_SEQ @;
		PUT PF_REQ_ACT @;
		PUT LC_STA_ACTYXX $;
	END;
	IF eof THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET LNXX END = eof;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	FORMAT LD_CRT_LONXX LD_SNT_RPD_DIS MMDDYYXX. ;
	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT LN_SEQ  @;
		PUT LD_CRT_LONXX @;	
		PUT LC_TYP_SCH_DIS @; 
		PUT LD_SNT_RPD_DIS @;
		PUT LA_RPS_ISL @;
		PUT DAY_DUE @;
		PUT LN_RPS_TRM $;
	END;
	IF eof THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET LNXX END = eof;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	FORMAT LD_DLQ_OCC LD_DLQ_MAX MMDDYYXX.;
	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT LN_SEQ @;
		PUT LD_DLQ_OCC @;
		PUT LN_DLQ_MAX @;
		PUT LD_DLQ_MAX $;
	END;
	IF eof THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET PDXX END = eof;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	FORMAT DD_VER_ADR_EML MMDDYYXX.;
	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT DC_ADR_EML @;
		PUT DX_ADR_EML @;
		PUT DD_VER_ADR_EML @;
		PUT DI_VLD_ADR_EML $;
	END;
	IF eof THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET PDXX END = eof;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	FORMAT DD_VER_ADR MMDDYYXX.;
	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT DX_STR_ADR_X @;
		PUT DX_STR_ADR_X @;
		PUT DM_CT @;
		PUT DC_DOM_ST @;
		PUT DF_ZIP_CDE @;
		PUT DM_FGN_ST @;
		PUT DM_FGN_CNY @;
		PUT DD_VER_ADR @;
		PUT DI_VLD_ADR $;
	END;
	IF eof THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET PDXX END = eof;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	FORMAT DD_PHN_VER MMDDYYXX.;
	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT DC_PHN @;
		PUT DC_ALW_ADL_PHN @;
		PUT DD_PHN_VER @;
		PUT DI_PHN_VLD @;
		PUT DN_DOM_PHN_ARA @;
		PUT DN_DOM_PHN_XCH @;
		PUT DN_DOM_PHN_LCL @;
		PUT DN_PHN_XTN @;
		PUT DN_FGN_PHN_CNY @;
		PUT DN_FGN_PHN_CT @;
		PUT DN_FGN_PHN_LCL $;
	END;
	IF eof THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET DWXX END = eof;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	FORMAT WA_TOT_BRI_OTS X.X WD_LON_RPD_SR mmddyyXX.;
	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT LN_SEQ  @;
		PUT WC_DW_LON_STA  @;
		PUT WA_TOT_BRI_OTS @;
		PUT WD_LON_RPD_SR @;
		PUT WX_OVR_DW_LON_STA $;
	END;
	IF eof THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET ARC_MXXXX END = eof;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	IF _N_ = X THEN PUT "-Begin-";
	DO;	
		PUT DF_SPE_ACC_ID @;
		PUT LN_ATY_SEQ @;
		PUT LC_STA_ACTYXX @;
		PUT LX_ATY $;
	END;
	IF eof THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET ADXX END = eof;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;
	FORMAT LD_FAT_ADJ_REQ MMDDYYXX.;

	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT LD_FAT_ADJ_REQ @;
		PUT LN_SEQ_FAT_ADJ_REQ @;
		PUT LC_TYP_FAT_ADJ_REQ @;
		PUT LC_STA_FAT_ADJ_REQ;
	END;
	IF EOF THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET FSXX END = EOF;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;

	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT LN_SEQ @;
		PUT LN_INC_SUB_EVT_SEQ @;
		PUT LD_INC_SUB_EFF_BEG @;
		PUT LD_INC_SUB_EFF_END @;
		PUT LC_INC_SUB_STA @;
		PUT LR_SUB_RMN @;
		PUT LF_LST_USR_FSXX @;
		PUT LF_LST_DTS_FSXX @;
		PUT LF_CRT_USR_FSXX @;
		PUT LD_CRT_FSXX @;
		PUT LC_STA_FSXX @;
		PUT LD_STA_FSXX ;
	END;
	IF EOF THEN PUT "-End-";
RUN;

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
