%LET DATERUN = %SYSFUNC(TODAY(), YYMMDDXX.);
LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE LITS AS
		SELECT
			LNXX.BF_SSN,
			CATX('',FSXX.LF_FED_AWD,PUT(FSXX.LN_FED_AWD_SEQ,ZX.)) AS AWARD_ID,
			LNXX.IF_LON_SRV_DFL_LON
		FROM
			PKUB.LNXX_LON LNXX
			INNER JOIN PKUB.FSXX_DL_LON FSXX
				ON LNXX.BF_SSN = FSXX.BF_SSN
				AND LNXX.LN_SEQ = FSXX.LN_SEQ
			INNER JOIN PKUB.LNXX_RPD_PIO_CVN LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
		WHERE
			LNXX.LC_STA_LONXX = 'L'
;QUIT;
ENDRSUBMIT;
PROC EXPORT
		DATA=LEGEND.LITS
		OUTFILE="T:\SAS\Redefaulted Loans &DATERUN..xlsx"
		DBMS=XLSX
		REPLACE;
RUN;