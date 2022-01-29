LIBNAME XL 'T:\AM record cleanup - KU.xlsx';

DATA WORK.XLSOURCE(KEEP=BF_SSN _WN_SEQ_GRXX _LN_SEQ);
	SET XL.'SheetX$'N;
	_WN_SEQ_GRXX = INPUT(WN_SEQ_GRXX, X.);
	_LN_SEQ = INPUT(LN_SEQ, X.);
RUN;

LIBNAME XL CLEAR;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;

DATA LEGEND.XLSOURCE; *Send data to legend;
SET XLSOURCE;
RUN;

RSUBMIT LEGEND;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

PROC SQL;
	CREATE TABLE DCR AS
		SELECT DISTINCT
			GRXX.BF_SSN	
			,GRXX.WN_SEQ_GRXX	
			,GRXX.LN_SEQ
			,GRXX.WC_NDS_RPD_TRM_RPT
		FROM 
			XLSOURCE XL
			INNER JOIN PKUB.GRXX_RPT_LON_APL GRXX
				ON XL.BF_SSN = GRXX.BF_SSN	
				AND XL._WN_SEQ_GRXX = GRXX.WN_SEQ_GRXX	
				AND XL._LN_SEQ = GRXX.LN_SEQ	
			;
QUIT;
ENDRSUBMIT;

DATA DCR; 
	SET LEGEND.DCR; 
RUN;

PROC EXPORT
		DATA=DCR
		OUTFILE='T:\SAS\CNH XXXXX.xlsx'
		REPLACE;
RUN;	
