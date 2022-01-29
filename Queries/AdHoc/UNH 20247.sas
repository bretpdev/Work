PROC IMPORT OUT = WORK.SOURCE
            DATAFILE = "T:\Align repayment research.xlsx" 
			REPLACE;
            SHEET = 'Sheet1$'n; 
RUN;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER SLIBREF=WORK;
DATA DUSTER.SOURCE; *Send data to Duster;
SET SOURCE;
RUN;


RSUBMIT DUSTER;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE LN65 AS
		SELECT DISTINCT
			LN65.BF_SSN,
			LN65.LN_SEQ AS LOAN_SEQ,
			MAX(LN65.LD_CRT_LON65) AS RPS_CREATE_DATE FORMAT MMDDYY10.
		FROM
			SOURCE S 
			INNER JOIN OLWHRM1.LN65_LON_RPS LN65
				ON LN65.BF_SSN = S.SSN
				AND LN65.LN_SEQ = S.LN_SEQ
			GROUP BY 
				LN65.BF_SSN,
				LN65.LN_SEQ

		
;
QUIT;
ENDRSUBMIT;

DATA LN65; SET DUSTER.LN65; RUN;

PROC SQL;
	CREATE TABLE FINAL AS 
		SELECT DISTINCT
			S.*,
			LN65.RPS_CREATE_DATE
		FROM
			LN65
			INNER JOIN SOURCE S
				ON S.SSN = LN65.BF_SSN
				AND S.LN_SEQ = LN65.LOAN_SEQ
;
QUIT;

PROC EXPORT DATA = WORK.FINAL 
            OUTFILE = "T:\NH 20247.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Sheet1"; 
RUN;
