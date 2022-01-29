LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE NHXXXX AS
		SELECT DISTINCT
			LNXX.BF_SSN||PUT(LNXX.LN_SEQ,ZX.) AS LID,
			LNXX.LA_LON_AMT_GTR AS ORIGINAL_BALANCE,
			LNXX.LA_CUR_PRI + DWXX.LA_NSI_OTS AS CURRENT_BALANCE,
			LNXX.LF_DOE_SCL_ORG AS ORIGNAL_SCHOOL
		FROM
			PKUB.LNXX_LON LNXX
			JOIN PKUB.DWXX_DW_CLC_CLU DWXX
				ON LNXX.BF_SSN = DWXX.BF_SSN
				AND LNXX.LN_SEQ = DWXX.LN_SEQ
		WHERE
			SUBSTR(LNXX.LF_DOE_SCL_ORG,X,X) IN ('XXXXXX','XXXXXX')
		ORDER BY 
			LID
	;

	CREATE TABLE COUNTS AS
		SELECT
			COUNT(LID) AS LOANS,
			SUM(ORIGINAL_BALANCE) AS ORIGINAL_BALANCE,
			SUM(CURRENT_BALANCE) AS CURRENT_BALANCE
		FROM
			NHXXXX
	;
QUIT;
ENDRSUBMIT;

DATA NHXXXX; SET LEGEND.NHXXXX; RUN;
DATA COUNTS; SET LEGEND.COUNTS; RUN;

PROC EXPORT
		DATA=COUNTS
		OUTFILE='T:\SAS\COUNTS.XLSX'
		REPLACE;
RUN;

PROC EXPORT
		DATA=NHXXXX
		OUTFILE='T:\SAS\DETAIL.XLSX'
		REPLACE;
RUN;
