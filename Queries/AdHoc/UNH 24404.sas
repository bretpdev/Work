PROC IMPORT OUT= WORK.Source
            DATAFILE= "T:\SchoolCodes.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="Sheet1$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER SLIBREF=WORK;
DATA DUSTER.SOURCE; *Send data to Duster;
SET SOURCE;
RUN;

RSUBMIT;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;

PROC SQL;
	CREATE TABLE DEMO AS
		SELECT
			S.SCHOOLCODE
		FROM	
			SOURCE S
			LEFT JOIN OLWHRM1.SC10_SCH_DMO SC10
				ON SC10.IF_DOE_SCL = S.SCHOOLCODE
		WHERE	
			SC10.IF_DOE_SCL IS NULL
;
QUIT;

ENDRSUBMIT;
DATA DEMO;
	SET DUSTER.DEMO;
RUN;

PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\NH 24404.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SHEET1"; 
RUN;
