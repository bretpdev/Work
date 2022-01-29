PROC IMPORT
		OUT=SLMA
		DATAFILE='T:\SLMA BORRS - ISSUE X.XLSX'
		DBMS=EXCEL
		REPLACE;
RUN;
		
LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;

DATA LEGEND.SLMA; SET SLMA; RUN;

RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

PROC SQL;
	CREATE TABLE SLMA_INFO AS
		SELECT DISTINCT
			SLMA.BF_SSN,
			FSXX.LF_FED_AWD||PUT(FSXX.LN_FED_AWD_SEQ,ZX.) AS Award_ID,
     		RSXX.BD_ANV_QLF_IBR AS ICR_Anniv_Date,
     		FSXX.LA_ICR_BAL_RPY_SR AS Princ_Bal_at_Repayment,
     		RSXX.LA_PMN_STD_PAY AS Perm_Standard_Amt
	 	FROM
			SLMA
			JOIN PKUB.FSXX_DL_LON FSXX
				ON SLMA.BF_SSN = FSXX.BF_SSN
			JOIN PKUB.RSXX_IBR_IRL_LON RSXX
				ON FSXX.BF_SSN = RSXX.BF_SSN
				AND FSXX.LN_SEQ = RSXX.LN_SEQ
			JOIN PKUB.RSXX_IBR_RPS RSXX
				ON RSXX.BF_SSN = RSXX.BF_SSN
				AND RSXX.BD_CRT_RSXX = RSXX.BD_CRT_RSXX
				AND RSXX.BN_IBR_SEQ = RSXX.BN_IBR_SEQ
			JOIN PKUB.FSXX_DL_LON FSXX
				ON RSXX.BF_SSN = FSXX.BF_SSN
				AND RSXX.LN_SEQ = FSXX.LN_SEQ
		ORDER BY
			SLMA.BF_SSN
	;
QUIT;

ENDRSUBMIT;

DATA SLMA_INFO; SET LEGEND.SLMA_INFO; RUN;

PROC EXPORT
		DATA=SLMA_INFO
		OUTFILE='T:\SLMA_INFO.XLSX'
		REPLACE;
RUN;