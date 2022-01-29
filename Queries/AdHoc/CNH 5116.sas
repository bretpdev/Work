LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
LIBNAME XL 'T:\Impacted Accounts CR XXXXXX XXXXXXXX.xlsx';

DATA LEGEND.ICR_BRWS; 
	SET XL.'SHEETX$'N; 
	ACCNT = PUT(DF_SPE_ACC_ID, ZXX.);
RUN;

RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

PROC SQL;
	CREATE TABLE LNXX AS
		SELECT
			ICR.ACCNT AS DF_SPE_ACC_ID,
			LNXX.*
		FROM
			ICR_BRWS ICR
			JOIN PKUB.PDXX_PRS_NME PDXX
				ON ICR.ACCNT = PDXX.DF_SPE_ACC_ID
			JOIN PKUB.LNXX_LON_RPS LNXX
				ON PDXX.DF_PRS_ID = LNXX.BF_SSN
	;
QUIT;

ENDRSUBMIT;

PROC EXPORT	
		DATA=LEGEND.LNXX
		OUTFILE='T:\LNXX_LON_RPS DATA DUMP.XLSX'
		REPLACE;
RUN;