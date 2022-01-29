PROC IMPORT OUT= WORK.SOURCE
            DATAFILE= "T:\LN65 DCR-26286.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="LN65$"; 
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

RSUBMIT DUSTER;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE RS05 AS
		SELECT	
			REMOTE.*
		FROM
			 SOURCE S
		INNER JOIN OLWHRM1.RS05_IBR_RPS REMOTE
			ON REMOTE.BF_SSN = S.BF_SSN
			
;

	CREATE TABLE RS10 AS
		SELECT	
			REMOTE.*
		FROM
			 SOURCE S
		INNER JOIN OLWHRM1.RS10_BR_RPD REMOTE
			ON REMOTE.BF_SSN = S.BF_SSN
			AND REMOTE.LN_RPS_SEQ = S.LN_RPS_SEQ
			
;

/*	CREATE TABLE RS20 AS*/
/*		SELECT	*/
/*			REMOTE.**/
/*		FROM*/
/*			 SOURCE S*/
/*		INNER JOIN OLWHRM1.RS20_IBR_IRL_LON REMOTE*/
/*			ON REMOTE.BF_SSN = S.BF_SSN*/
/*			*/
/*;*/
QUIT;
ENDRSUBMIT;

DATA RS05; SET DUSTER.RS05;RUN;
DATA RS10; SET DUSTER.RS10;RUN;
/*DATA RS20; SET DUSTER.RS20;RUN;*/


PROC EXPORT DATA = RS05 
            OUTFILE = "T:\NH 26286 DATA DUMPS.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RS05"; 
RUN;

PROC EXPORT DATA = RS10 
            OUTFILE = "T:\NH 26286 DATA DUMPS.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RS10"; 
RUN;