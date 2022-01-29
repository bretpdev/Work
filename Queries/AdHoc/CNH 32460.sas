LIBNAME XL 'T:\Loans for GRXX Table Dump.xlsx';
DATA WORK.XLSOURCE (RENAME=(_LN_SEQ = LN_SEQ));
	SET XL.'SheetX$'N (DROP=Report_from_date FX);
	_LN_SEQ = INPUT(LN_SEQ,X.);
	DROP LN_SEQ;
RUN;
LIBNAME XL CLEAR;
LIBNAME LEGEND REMOTE SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.XLSOURCE; *Send data to legend;
	SET XLSOURCE;
RUN;
RSUBMIT LEGEND;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE POP AS
	SELECT
		GRXX.*
	FROM
		PKUB.GRXX_RPT_LON_APL GRXX
		INNER JOIN XLSOURCE XL
			ON GRXX.BF_SSN = XL.SSN
			AND GRXX.LN_SEQ = XL.LN_SEQ
	;
QUIT;
ENDRSUBMIT;
PROC EXPORT
	DATA=LEGEND.POP
	OUTFILE='T:\SAS\GRXX_dump.xlsx'
	REPLACE;
RUN;
