/*The deb ID = LNXX.IF_LON_SRV_DFL_LON*/
/*Award ID =LF_FED_AWD CHAR(XX) X X Federal Award ID*/
/*LN_FED_AWD_SEQ*/

LIBNAME XL 'T:\Litigation Loans.XLSX';
LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;

DATA LEGEND.LIT; SET XL.'LOANS$'N; RUN;

RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE LITINFO AS
		SELECT
			LIT.SSN,
			LIT.LOAN_SEQ,
			CATX('',FSXX.LF_FED_AWD,PUT(FSXX.LN_FED_AWD_SEQ,ZX.)) AS AWARD_ID,
			LNXX.IF_LON_SRV_DFL_LON AS DEBT_ID
		FROM
			LIT
			JOIN PKUB.FSXX_DL_LON FSXX
				ON LIT.SSN = FSXX.BF_SSN
				AND LIT.LOAN_SEQ = FSXX.LN_SEQ
			JOIN PKUB.LNXX_RPD_PIO_CVN LNXX
				ON LIT.SSN = LNXX.BF_SSN
				AND LIT.LOAN_SEQ = LNXX.LN_SEQ
	;
QUIT;

ENDRSUBMIT;

DATA LITINFO; SET LEGEND.LITINFO; RUN;

PROC EXPORT
		DATA=LITINFO
		OUTFILE='T:\SAS\LITIGATION LOANS INFO.XLSX'
		REPLACE;
RUN;

