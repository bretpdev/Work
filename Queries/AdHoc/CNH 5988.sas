LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
DATA CNFC;
	SET PKUB.CNFC_DOC_NUM_TRK;
	WHERE LF_CON_LON_SER = 'XXXXXX';
RUN;
ENDRSUBMIT;

PROC EXPORT
		DATA=LEGEND.CNFC
		OUTFILE='T:\CNFC.XLSX'
		REPLACE;
RUN;