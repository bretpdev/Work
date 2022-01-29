LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPRUUT OWNER=PKUB;
PROC SQL;
	CREATE TABLE IBR AS
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LC_TYP_SCH_DIS
		FROM
			PKUB.LNXX_LON_RPS LNXX
		WHERE
			LNXX.LC_STA_LONXX = 'A'
			AND LNXX.LC_TYP_SCH_DIS IN ('IB','IL')
	;
QUIT;
ENDRSUBMIT;

DATA IBR; SET LEGEND.IBR; RUN;

PROC EXPORT
		DATA=IBR
		OUTFILE='T:\SAS\Xnd test region borrs on IBR.XLSX'
		REPLACE;
RUN;
