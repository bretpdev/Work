LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE NEWRATE AS
		SELECT DISTINCT
			PDXX.DF_SPE_ACC_ID,
			LNXX.BF_SSN,
			LNXX.LN_SEQ,
			LNXX.LD_LON_X_DSB,
			LNXX.LR_ITR,
			LNXX.IC_LON_PGM,
			LNXX.LF_RGL_CAT_LPXX
		FROM
			PKUB.PDXX_PRS_NME PDXX
			JOIN PKUB.LNXX_LON LNXX
				ON PDXX.DF_PRS_ID = LNXX.BF_SSN
				AND LNXX.IC_LON_PGM IN ('DLPLUS','DLPLGB')
			JOIN PKUB.LNXX_DSB LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LD_DSB BETWEEN 'XXJULXXXX'D AND TODAY()
				AND LNXX.LC_STA_LONXX IN ('X','X') /*active, active/reissue*/
			JOIN PKUB.LNXX_INT_RTE_HST LNXX
				ON LNXX.BF_SSN = LNXX.BF_SSN
				AND LNXX.LN_SEQ = LNXX.LN_SEQ
				AND LNXX.LC_STA_LONXX='A'
	;
QUIT;
ENDRSUBMIT;

DATA NEWRATE; SET LEGEND.NEWRATE; RUN;

PROC EXPORT
		DATA=NEWRATE
		OUTFILE='T:\DLPLUS-DLPLGB Query.XLSX'
		REPLACE;
RUN;
