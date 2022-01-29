%LET BEGINDATE = '07/01/2020';
%LET NHTICKET = UNH_71437;

%LET RPTLIB = T:\SAS;
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;
%SYSLPUT BEGINDATE = &BEGINDATE;
%SYSLPUT NHTICKET = &NHTICKET;
RSUBMIT;

PROC SQL;
	CONNECT TO DB2 (DATABASE=DLGSUTWH);
	CREATE TABLE DEMO AS
		SELECT 
			*
		FROM 
			CONNECTION TO DB2 
			(
				SELECT DISTINCT
					LN40.BF_SSN	AS SSN,
					LN40.LN_SEQ	AS LOAN_SEQUENCE,
					LN10.IF_GTR	AS GUARANTOR 
				FROM	
					OLWHRM1.LN40_LON_CLM_PCL LN40
					LEFT JOIN OLWHRM1.LN10_LON_EOM LN10
						ON LN40.BF_SSN = LN10.BF_SSN
						AND LN40.LN_SEQ = LN10.LN_SEQ
				WHERE
					DAYS(LN40.LD_SBM_CLM_PCL) BETWEEN DAYS(&BEGINDATE) AND DAYS(CURRENT_DATE)
					AND LN40.LC_TYP_REC_CLP_LON IN (1,6)
				ORDER BY
					LN10.IF_GTR
					
				FOR READ ONLY WITH UR
			)
	;
	DISCONNECT FROM DB2;
QUIT;
ENDRSUBMIT;
DATA DEMO;
	SET DUSTER.DEMO;
RUN;
PROC SQL;
	CREATE TABLE COUNTS AS 
		SELECT
			'TOTAL NUMBER OF BORROWERS:' AS BWRS,
			COUNT(DISTINCT SSN) 		 AS SSN_COUNT,
			'TOTAL NUMBER OF LOANS:' 	 AS LOANS,
			COUNT(*) 					 AS LOAN_COUNT
		FROM
			DEMO
	;
QUIT;
PROC EXPORT 
	DATA = WORK.COUNTS 
	OUTFILE = "&RPTLIB\&NHTICKET..xlsx" 
	DBMS = EXCEL
	REPLACE;
	SHEET="CLAIM_COUNTS"; 
RUN;
PROC EXPORT 
	DATA = WORK.DEMO 
	OUTFILE = "&RPTLIB\&NHTICKET..xlsx" 
	DBMS = EXCEL
	REPLACE;
	SHEET="RAW_DATA"; 
RUN;