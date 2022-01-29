LIBNAME REHAB 'T:\Rehab Details Aug XX.xlsx';
LIBNAME STATUS 'T:\XXXX-XX-XX_STATUS.xlsx';
LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;

DATA REHAB ;
	SET REHAB.'SheetX$'N (FIRSTOBS=X);
RUN;

DATA LEGEND.REHAB; SET REHAB; RUN;

RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE DSX AS
		SELECT
			REHAB.FX AS AWARD_ID,
			FSXX.BF_SSN,
			FSXX.LN_SEQ,
			LNXX.LC_STA_LONXX,
			DWXX.WC_DW_LON_STA,
			LNXX.LD_LON_RHB_PCV
		FROM
			REHAB
			LEFT JOIN PKUB.FSXX_DL_LON FSXX
				ON REHAB.FX = CATX('',FSXX.LF_FED_AWD,PUT(FSXX.LN_FED_AWD_SEQ,ZX.))
			LEFT JOIN PKUB.LNXX_LON LNXX
				ON FSXX.BF_SSN = LNXX.BF_SSN
				AND FSXX.LN_SEQ = LNXX.LN_SEQ
			LEFT JOIN PKUB.DWXX_DW_CLC_CLU DWXX
				ON FSXX.BF_SSN = DWXX.BF_SSN
				AND FSXX.LN_SEQ = DWXX.LN_SEQ
			LEFT JOIN PKUB.LNXX_RPD_PIO_CVN LNXX
				ON FSXX.BF_SSN = LNXX.BF_SSN
				AND FSXX.LN_SEQ = LNXX.LN_SEQ
	;
QUIT;
ENDRSUBMIT;

DATA DSX; SET LEGEND.DSX; RUN;

DATA DSX;
	SET STATUS.'LOAN_DETAIL_FSA$'N;
	WHERE REHAB_DATE IS NOT NULL;
RUN;

PROC SQL;
	CREATE TABLE MATCHES AS
		SELECT
			DSX.BF_SSN,
			DSX.AWARD_ID,
			DSX.LN_SEQ,
			DSX.LC_STA_LONXX,
			DSX.WC_DW_LON_STA
		FROM
			DSX
			JOIN DSX
				ON DSX.BF_SSN = DSX.BF_SSN
				AND DSX.LN_SEQ = DSX.LN_SEQ
	;

	CREATE TABLE IN_NSLDS AS
		SELECT
			DSX.BF_SSN,
			DSX.AWARD_ID,
			DSX.LN_SEQ,
			DSX.LC_STA_LONXX,
			DSX.WC_DW_LON_STA,
			DSX.LD_LON_RHB_PCV
		FROM
			DSX
			LEFT JOIN DSX
				ON DSX.BF_SSN = DSX.BF_SSN
				AND DSX.LN_SEQ = DSX.LN_SEQ
		WHERE
			DSX.BF_SSN IS NULL
	;

	CREATE TABLE IN_OURS AS
		SELECT
			DSX.BF_SSN,
			DSX.AWARD_ID,
			DSX.LN_SEQ,
			DSX.LC_STA_LONXX,
			DSX.WC_DW_LON_STA
		FROM
			DSX
			LEFT JOIN DSX
				ON DSX.BF_SSN = DSX.BF_SSN
				AND DSX.LN_SEQ = DSX.LN_SEQ
		WHERE
			DSX.BF_SSN IS NULL
	;
QUIT;
			
PROC EXPORT
		DATA=MATCHES
		OUTFILE='T:\NSLDS Trigger Files - Rehab Compare.xlsx'
		REPLACE;
RUN;

PROC EXPORT
		DATA=IN_NSLDS
		OUTFILE='T:\NSLDS Trigger Files - Rehab Compare.xlsx'
		REPLACE;
RUN;

PROC EXPORT
		DATA=IN_OURS
		OUTFILE='T:\NSLDS Trigger Files - Rehab Compare.xlsx'
		REPLACE;
RUN;
