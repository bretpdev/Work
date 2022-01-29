LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE ICR AS
		SELECT DISTINCT
			LNXX.BF_SSN,
			LNXX.LC_TYP_SCH_DIS 	
		FROM
			PKUB.LNXX_LON LNXX
			JOIN PKUB.LNXX_LON_RPS LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LC_STA_LONXX = 'R'
				AND LNXX.LA_CUR_PRI > X
				AND LNXX.LC_STA_LONXX = 'A'
				AND LNXX.LC_TYP_SCH_DIS IN ('CQ', 'CX', 'CX', 'CX')
	;
QUIT;
ENDRSUBMIT;

DATA ICR; SET LEGEND.ICR; RUN;

PROC EXPORT
		DATA=ICR
		OUTFILE='T:\Active ICR Borrowers for FSA CR XXXX.XLSX'
		REPLACE;
RUN;