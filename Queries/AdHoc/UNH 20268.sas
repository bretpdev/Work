LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
RSUBMIT DUSTER;
PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);

	CREATE TABLE DEMO AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
				(
					SELECT
						*
					FROM	
						OLWHRM1.RP20_RIR_IST_DFN 

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;
QUIT;

ENDRSUBMIT;

PROC EXPORT DATA = duster.DEMO 
            OUTFILE = "T:\SAS\NH 20268 RP20.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;