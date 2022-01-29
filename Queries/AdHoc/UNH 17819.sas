DATA INDATA;
	INFILE "T:\SAS\pt_status_non_duplicate.txt"	DLM = ',';
	INPUT BR_SSN $9. COMP_LN_SEQ ;
RUN;


LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
DATA DUSTER.INDATA; SET INDATA; RUN;

RSUBMIT;

PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE DEMO AS
		SELECT 
			DB2.*
		FROM 
			INDATA ID
			JOIN CONNECTION TO DB2 
				(
					SELECT
						BF_SSN,
						LN_SEQ,
						LC_BBS_ELG
					FROM	
						OLWHRM1.LN54_LON_BBS

					FOR READ ONLY WITH UR
				) DB2
				ON ID.BR_SSN = DB2.BF_SSN
				AND ID.COMP_LN_SEQ = DB2.LN_SEQ
	;

	DISCONNECT FROM DB2;

QUIT;

ENDRSUBMIT;

DATA DEMO;SET DUSTER.DEMO; RUN;

PROC EXPORT
		DATA = DEMO
		OUTFILE = 'T:\SAS\NHUH 17819.XLSX'
		DBMS = EXCEL2007
		REPLACE;
QUIT;