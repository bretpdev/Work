LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
DATA BTXX;
	SET PKUB.BTXX_MNR_BCH;
	WHERE 
		PF_MAJ_BCH = 'XXXXXXXXXX'
		AND PF_MNR_BCH = X
	;
RUN;
ENDRSUBMIT;

PROC EXPORT
		DATA=LEGEND.BTXX
		OUTFILE='T:\Data Dump of BTXX for batch XXXXXXXXXX.XLSX'
		REPLACE;
RUN;
