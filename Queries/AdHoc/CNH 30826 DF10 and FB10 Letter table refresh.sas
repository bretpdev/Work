%LET RPTLIB = T:\SAS;

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
	LAST_RUN = TODAY() - XXXXX;	*COMMENT FOR PROD, IT WILL READ THE DATE FROM A TABLE;

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

/***************************************************************
* DEFER & FORBEAR APPROVED/CHANGED/DENIED LETTER � DEFER TABLE
***************************************************************/
PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DFR_LTR AS
		SELECT 
			*
		FROM CONNECTION TO DBX 
			(
				SELECT
					DFXX.BF_SSN,
					LNXX.LN_SEQ,
					DFXX.LF_DFR_CTL_NUM,
					LNXX.LN_DFR_OCC_SEQ,
					DFXX.LC_DFR_TYP,
					DFXX.LD_DFR_INF_CER,
					DFXX.LD_CRT_REQ_DFR,
					DFXX.LC_DFR_STA,
					DFXX.LC_STA_DFRXX,
					DFXX.LD_STA_DFRXX,
					DFXX.LF_LST_DTS_DFXX,
					LNXX.LD_DFR_BEG,
					LNXX.LD_DFR_END,
					LNXX.LC_STA_LONXX,
					LNXX.LC_LON_LEV_DFR_CAP,
					LNXX.LF_LST_DTS_LNXX,
					LNXX.LD_STA_LONXX,
					LNXX.LD_DFR_APL
				FROM 
					PKUB.DFXX_BR_DFR_REQ DFXX
					JOIN PKUB.LNXX_BR_DFR_APV LNXX
						ON DFXX.BF_SSN = LNXX.BF_SSN
						AND DFXX.LF_DFR_CTL_NUM = LNXX.LF_DFR_CTL_NUM
				WHERE
					DFXX.LF_LST_DTS_DFXX >= &LAST_RUNPASS
					OR LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS	
			)
	;

	DISCONNECT FROM DBX;
QUIT;
%SSNXACC(DFR_LTR,LN_SEQ LF_DFR_CTL_NUM LN_DFR_OCC_SEQ);

/***************************************************************
* DEFER & FORBEAR APPROVED/CHANGED/DENIED LETTER � FORB TABLE
***************************************************************/
PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE FOR_LTR AS
		SELECT 
			*
		FROM CONNECTION TO DBX 
			(
				SELECT
					FBXX.BF_SSN,
					LNXX.LN_SEQ,
					FBXX.LF_FOR_CTL_NUM,
					LNXX.LN_FOR_OCC_SEQ,
					FBXX.LC_FOR_TYP,
					FBXX.LD_FOR_INF_CER,
					FBXX.LD_CRT_REQ_FOR,
					FBXX.LC_FOR_STA,
					FBXX.LC_STA_FORXX,
					FBXX.LD_STA_FORXX,
					FBXX.LF_LST_DTS_FBXX,
					LNXX.LD_FOR_BEG,
					LNXX.LD_FOR_END,
					LNXX.LC_STA_LONXX,
					LNXX.LC_LON_LEV_FOR_CAP,
					LNXX.LF_LST_DTS_LNXX,
					LNXX.LD_STA_LONXX,
					LNXX.LD_FOR_APL
				FROM
					PKUB.FBXX_BR_FOR_REQ FBXX
					JOIN PKUB.LNXX_BR_FOR_APV LNXX
						ON FBXX.BF_SSN = LNXX.BF_SSN
						AND FBXX.LF_FOR_CTL_NUM = LNXX.LF_FOR_CTL_NUM
				WHERE
					 FBXX.LF_LST_DTS_FBXX >= &LAST_RUNPASS
					 OR LNXX.LF_LST_DTS_LNXX >= &LAST_RUNPASS
			)
	;

	DISCONNECT FROM DBX;
QUIT;
%SSNXACC(FOR_LTR,LN_SEQ LF_FOR_CTL_NUM LN_FOR_OCC_SEQ);

ENDRSUBMIT;

DATA DFR_LTR; SET LEGEND.DFR_LTR; RUN; *XX;
DATA FOR_LTR; SET LEGEND.FOR_LTR; RUN; *XX;

DATA _NULL_;
	SET DFR_LTR END = EOF;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;

	FORMAT LD_DFR_INF_CER DATEX.;
	FORMAT LD_CRT_REQ_DFR DATEX.;
	FORMAT LD_STA_DFRXX DATEX.;
	FORMAT LF_LST_DTS_DFXX DATETIMEXX.X;
	FORMAT LD_DFR_BEG DATEX.;
	FORMAT LD_DFR_END DATEX.;
	FORMAT LF_LST_DTS_LNXX DATETIMEXX.X;
	FORMAT LD_STA_LONXX DATEX.;
	FORMAT LD_DFR_APL DATEX.;

	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT LN_SEQ @;
		PUT LF_DFR_CTL_NUM @;
		PUT LN_DFR_OCC_SEQ @;
		PUT LC_DFR_TYP @;
		PUT LD_DFR_INF_CER @;
		PUT LD_CRT_REQ_DFR @;
		PUT LC_DFR_STA @;
		PUT LC_STA_DFRXX @;
		PUT LD_STA_DFRXX @;
		PUT LF_LST_DTS_DFXX @;
		PUT LD_DFR_BEG @;
		PUT LD_DFR_END @;
		PUT LC_STA_LONXX @;
		PUT LC_LON_LEV_DFR_CAP @;
		PUT LF_LST_DTS_LNXX @;
		PUT LD_STA_LONXX @;
		PUT LD_DFR_APL ;
	END;
	IF EOF THEN PUT "-End-";
RUN;

DATA _NULL_;
	SET FOR_LTR END = EOF;
	FILE REPORTXX DELIMITER=',' DSD DROPOVER LRECL=XXXXX;

	FORMAT LD_FOR_INF_CER DATEX.;
	FORMAT LD_CRT_REQ_FOR DATEX.;
	FORMAT LD_STA_FORXX DATEX.;
	FORMAT LF_LST_DTS_FBXX DATETIMEXX.X;
	FORMAT LD_FOR_BEG DATEX.;
	FORMAT LD_FOR_END DATEX.;
	FORMAT LF_LST_DTS_LNXX DATETIMEXX.X;
	FORMAT LD_STA_LONXX DATEX.;
	FORMAT LD_FOR_APL DATEX.;

	IF _N_ = X THEN PUT "-Begin-";
	DO;
		PUT DF_SPE_ACC_ID @;
		PUT LN_SEQ @;
		PUT LF_FOR_CTL_NUM @;
		PUT LN_FOR_OCC_SEQ @;
		PUT LC_FOR_TYP @;
		PUT LD_FOR_INF_CER @;
		PUT LD_CRT_REQ_FOR @;
		PUT LC_FOR_STA @;
		PUT LC_STA_FORXX @;
		PUT LD_STA_FORXX @;
		PUT LF_LST_DTS_FBXX @;
		PUT LD_FOR_BEG @;
		PUT LD_FOR_END @;
		PUT LC_STA_LONXX @;
		PUT LC_LON_LEV_FOR_CAP @;
		PUT LF_LST_DTS_LNXX @;
		PUT LD_STA_LONXX @;
		PUT LD_FOR_APL ;
	END;
	IF EOF THEN PUT "-End-";
RUN;
