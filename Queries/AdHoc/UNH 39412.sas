%LET RPTLIB = T:\SAS;

LIBNAME XL 'T:\UPDATED.DCR 23842 CR 68217 Clean-UP.xlsx';
DATA WORK.XLSOURCE;
	SET XL.'LN50 - DELETE$'N;
	_LN_SEQ = INPUT(LN_SEQ,2.);
	_LN_DFR_OCC_SEQ = INPUT(LN_DFR_OCC_SEQ,1.);
RUN;
LIBNAME XL CLEAR;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; 
DATA DUSTER.XLSOURCE; 
SET XLSOURCE;
RUN;

RSUBMIT;

%LET DB = DLGSUTWH; 
LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1; 

PROC SQL;
	CREATE TABLE LoanDetail AS
	SELECT DISTINCT
		DF10.*
	FROM
		WORK.XLSOURCE XLS
		INNER JOIN OLWHRM1.LN50_BR_DFR_APV LN50
			ON XLS.BF_SSN = LN50.BF_SSN
			AND XLS._LN_SEQ = LN50.LN_SEQ
			AND XLS.LF_DFR_CTL_NUM = LN50.LF_DFR_CTL_NUM
			AND XLS._LN_DFR_OCC_SEQ = LN50.LN_DFR_OCC_SEQ
		INNER JOIN OLWHRM1.DF10_BR_DFR_REQ DF10
			ON LN50.BF_SSN = DF10.BF_SSN
			AND LN50.LF_DFR_CTL_NUM = DF10.LF_DFR_CTL_NUM
	;
QUIT;
ENDRSUBMIT;

DATA LoanDetail;
	SET DUSTER.LoanDetail;
RUN;

PROC EXPORT
		DATA=LoanDetail
		OUTFILE="&RPTLIB\UNH 39412.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="DF10";		
RUN;