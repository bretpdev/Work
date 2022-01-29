LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DB2 DATABASE=&DB OWNER=PKUB;

PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT
						*
					FROM
						SYSCAT.TABLES
					WHERE
						TYPE = 'T' 
				)
	;

	DISCONNECT FROM DB2;

QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

/*export to Excel spreadsheet*/
/*PROC EXPORT DATA= WORK.DEMO */
/*            OUTFILE= "T:\SAS\LEGEND TABLES.xls" */
/*            DBMS=EXCEL REPLACE;*/
/*     SHEET="A"; */
/*RUN;*/
