LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE INITPOP AS
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ
		FROM
			PKUB.LNXX_LON LNXX
		WHERE
			LNXX.LA_CUR_PRI > X
			AND LNXX.LC_STA_LONXX = 'R'
			AND LNXX.IC_LON_PGM NOT IN 
			(
				'DLPCNS', 'DLSCNS', 'DLSSPL', 'DLUCNS', 'DLUSPL',
				'SUBCNS', 'UNCNS', 'CNSDLN', 'SUBSPC', 'UNSPC'
			)
	;
QUIT;

PROC SQL;
	CREATE TABLE MAX_BILL AS
		SELECT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			MAX(BLXX.LD_BIL_CRT) AS LD_BIL_CRT
		FROM
			INITPOP IP
			JOIN PKUB.LNXX_LON_BIL_CRF LNXX
				ON IP.BF_SSN = LNXX.BF_SSN
				AND IP.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LC_STA_LONXX = 'A'			
			JOIN PKUB.BLXX_BR_BIL BLXX
				ON BLXX.BF_SSN = LNXX.BF_SSN
				AND BLXX.LD_BIL_CRT = LNXX.LD_BIL_CRT
				AND BLXX.LN_SEQ_BIL_WI_DTE = LNXX.LN_SEQ_BIL_WI_DTE
				AND BLXX.LC_STA_BILXX = 'A'
		GROUP BY
			LNXX.BF_SSN,
			LNXX.LN_SEQ
	;
QUIT;

PROC SQL;
	CREATE TABLE PAID_AHEAD_ AS
		SELECT DISTINCT
			MB.BF_SSN,
			MB.LN_SEQ,
			INTCK('MONTH',BLXX.LD_BIL_CRT,BLXX.LD_BIL_DU) AS MOS_PD_AHEAD
		FROM
			MAX_BILL MB
			JOIN PKUB.LNXX_LON_BIL_CRF LNXX
				ON MB.BF_SSN = LNXX.BF_SSN
				AND MB.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LC_STA_LONXX = 'A'	
				AND MB.LD_BIL_CRT = LNXX.LD_BIL_CRT	
			JOIN PKUB.BLXX_BR_BIL BLXX
				ON BLXX.BF_SSN = LNXX.BF_SSN
				AND BLXX.LD_BIL_CRT = LNXX.LD_BIL_CRT
				AND BLXX.LN_SEQ_BIL_WI_DTE = LNXX.LN_SEQ_BIL_WI_DTE
				AND BLXX.LC_STA_BILXX = 'A'
		WHERE
			BLXX.LD_BIL_DU > INTNX('DAY',BLXX.LD_BIL_CRT,XX,'B')
	;

	CREATE TABLE PAID_AHEAD AS
		SELECT DISTINCT
			BF_SSN,
			LN_SEQ,
			MAX(MOS_PD_AHEAD) AS MOS_PD_AHEAD
		FROM
			PAID_AHEAD_
		GROUP BY
			BF_SSN,
			LN_SEQ
	;	
QUIT;

PROC SQL;
	CREATE TABLE ACH AS
		SELECT DISTINCT
			PA.BF_SSN,
			PA.LN_SEQ
		FROM
			PAID_AHEAD PA
			JOIN PKUB.LNXX_EFT_TO_LON LNXX
				ON PA.BF_SSN = LNXX.BF_SSN
				AND PA.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LC_STA_LNXX = 'A'
	;
QUIT;
RSUBMIT;
PROC SQL;
	CREATE TABLE RPTYP AS
		SELECT DISTINCT
			PA.BF_SSN,
			PA.LN_SEQ,
			CATS(PA.BF_SSN,PUT(PA.LN_SEQ,ZX.)) AS LID,
			LNXX.LC_TYP_SCH_DIS
		FROM
			PAID_AHEAD PA
			LEFT JOIN PKUB.LNXX_LON_RPS LNXX
				ON PA.BF_SSN = LNXX.BF_SSN
				AND PA.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LC_STA_LONXX = 'A'
	;
QUIT;


PROC SQL;
	CREATE TABLE CNTS_IP AS
		SELECT
			COUNT(*) AS IP_CNT
		FROM
			INITPOP
	;

	CREATE TABLE CNTS_PA AS
		SELECT
			COUNT(*) AS PA_CNT
		FROM
			PAID_AHEAD
	;

	CREATE TABLE CNTS_ACH AS
		SELECT
			COUNT(*) AS ACH_CNT
		FROM
			ACH
	;

	CREATE TABLE MOS_PA AS
		SELECT
			SUM(MOS_PD_AHEAD) AS MOS_PA
		FROM
			PAID_AHEAD
	;

	CREATE TABLE CNTS_RP AS
		SELECT DISTINCT
			COUNT(LID) AS CNT,
			LC_TYP_SCH_DIS
		FROM
			RPTYP
		GROUP BY
			LC_TYP_SCH_DIS
	;
QUIT;
		
ENDRSUBMIT;

DATA CNTS_IP; SET LEGEND.CNTS_IP; RUN;
DATA CNTS_PA; SET LEGEND.CNTS_PA; RUN;
DATA CNTS_ACH; SET LEGEND.CNTS_ACH; RUN;
DATA MOS_PA; SET LEGEND.MOS_PA; RUN;
DATA CNTS_RP; SET LEGEND.CNTS_RP; RUN;

DATA CNTS_GEN;
	MERGE CNTS_IP CNTS_PA CNTS_ACH MOS_PA;
RUN;

DATA RESULTS (DROP = MOS_PA);
	SET CNTS_GEN;
	LABEL
		IP_CNT = 'Open Released Loans'
		PA_CNT = 'Paid Ahead Loans'
		PA_PCT = 'Percent of Loans Paid Ahead'
		ACH_CNT = 'Paid Ahead Loans on Active ACH'
		ACH_PCT = 'Percent of Paid Ahead Loans on Active ACH'
		PA_AVG = 'Average Months Paid Ahead'
	;
	PA_PCT = PA_CNT/IP_CNT;
	ACH_PCT = ACH_CNT/PA_CNT;
	PA_AVG = MOS_PA/PA_CNT;
RUN;

PROC SQL;
	CREATE TABLE REPAYMENT AS
		SELECT
			RP.LC_TYP_SCH_DIS AS Repayment_Type,
			RP.CNT AS Loans,
			RP.CNT/PA.PA_CNT AS Percent
		FROM
			CNTS_RP RP
			JOIN CNTS_PA PA
				ON X = X
		;
QUIT;

PROC EXPORT
		DATA=RESULTS
		OUTFILE='T:\CandC Call Data Request XX-XX.xlsx'
		LABEL
		REPLACE;
RUN;

PROC EXPORT
		DATA=REPAYMENT
		OUTFILE='T:\CandC Call Data Request XX-XX.xlsx'
		REPLACE;
RUN;
