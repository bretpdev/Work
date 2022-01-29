LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE DCSTR AS
		SELECT DISTINCT
			AYXX.BF_SSN
		FROM
			PKUB.AYXX_BR_LON_ATY AYXX
		WHERE
			AYXX.PF_REQ_ACT = 'DCSTR'
			AND AYXX.LD_STA_ACTYXX BETWEEN 'XXJANXXXX'D AND 'XXDECXXXX'D
	;

	CREATE TABLE LNXX AS
		SELECT DISTINCT 
			DCSTR.BF_SSN
		FROM
			DCSTR
			JOIN PKUB.LNXX_LON_DLQ_HST LNXX
				ON DCSTR.BF_SSN = LNXX.BF_SSN
			LEFT JOIN PKUB.AYXX_BR_LON_ATY AYXX
				ON LNXX.BF_SSN = AYXX.BF_SSN
				AND AYXX.PF_RSP_ACT = 'CNTCT'
				AND AYXX.LD_STA_ACTYXX BETWEEN LNXX.LD_DLQ_OCC AND LNXX.LD_DLQ_MAX
		WHERE
			(LNXX.LD_DLQ_OCC BETWEEN 'XXJANXXXX'D AND 'XXDECXXXX'D OR LNXX.LD_DLQ_MAX BETWEEN 'XXJANXXXX'D AND 'XXDECXXXX'D)
			AND AYXX.BF_SSN IS NULL
	;
QUIT;
ENDRSUBMIT;

PROC EXPORT
		DATA=LEGEND.DCSTR
		OUTFILE='T:\Default with no Contact Query.xlsx'
		REPLACE;
RUN;

PROC EXPORT
		DATA=LEGEND.LNXX
		OUTFILE='T:\Default with no Contact Query.xlsx'
		REPLACE;
RUN;
			