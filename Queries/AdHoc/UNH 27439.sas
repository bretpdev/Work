/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

LIBNAME XL 'T:\AD20 Insert for BANA.xlsx';

DATA WORK.XLSOURCE (KEEP=BF_SSN);
	SET XL.'AD20$'N;
RUN;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; /*live*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK ;*/ /*test*/

DATA DUSTER.XLSOURCE; *Send data to Duster;
SET XLSOURCE;
RUN;

RSUBMIT;

%LET DB = DLGSUTWH; /*live*/
/*%LET DB = DLGSWQUT;*/ /*test*/
LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1; /*needed for SQL queries, but not for DB2 queries*/

PROC SQL;
CREATE TABLE LoanDetail AS
	SELECT DISTINCT
		LN10.BF_SSN
		,LN10.LN_SEQ
		,LN10.LD_LON_ACL_ADD
	FROM
		WORK.XLSOURCE XLS
		INNER JOIN OLWHRM1.LN10_LON LN10
			ON XLS.BF_SSN = LN10.BF_SSN
;
QUIT;

ENDRSUBMIT;

DATA LoanDetail;
	SET DUSTER.LoanDetail;
RUN;

PROC EXPORT
		DATA=LoanDetail
		OUTFILE="&RPTLIB\UNH 27439.xlsx"
		DBMS = EXCEL
		REPLACE;
RUN;
