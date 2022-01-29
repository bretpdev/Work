PROC IMPORT OUT= WORK.source
            DATAFILE= "T:\BANA - Forb and Def Borrowers ACH File.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="LOANDETAIL$"; 
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
	CREATE TABLE ln65 AS
		SELECT
			LN65.*
		FROM
			OLWHRM1.LN65_LON_RPS LN65
			INNER JOIN SOURCE S
				ON S.SSN = LN65.BF_SSN	
				AND S.LN_SEQ = LN65.LN_SEQ
			
;
QUIT;
ENDRSUBMIT;

PROC EXPORT DATA = DUSTER.ln65 
            OUTFILE = "T:\NH 26286.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Sheet1"; 
RUN;