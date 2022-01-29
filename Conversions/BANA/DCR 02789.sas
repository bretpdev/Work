%LET BANA = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME BANA ODBC &BANA ;

/*TEST*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK  ;*/

/*LIVE*/
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

RSUBMIT;  
/*%let DB = DLGSWQUT;  *This is test;*/
%let DB = DLGSUTWH;  *This is live;

LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE POP AS
		SELECT
			LN30.*
		FROM
			 OLWHRM1.LN30_LON_ERR LN30
		INNER JOIN
		(
			select
				*
			from 
				OLWHRM1.ln10_lon
			where
				PF_MAJ_BCH in('2016034001','2016034002')
		)LN10
			ON LN30.BF_SSN = LN10.BF_SSN
			AND LN30.LN_SEQ = LN10.LN_SEQ
/*		LEFT JOIN OLWHRM1.LN30_LON_ERR ln30e*/
/*			on ln30e.BF_SSN = LN30.BF_SSN*/
/*			AND LN30E.LN_SEQ = LN30.LN_SEQ*/
/*			AND LN30E.PF_ERR_MSG = '03866'*/
		WHERE
			LN30.PF_ERR_MSG = '02789'
			AND LN30.BF_SSN NOT IN (SELECT BF_SSN FROM OLWHRM1.LN30_LON_ERR WHERE PF_ERR_MSG = '03866')
			order by
				bf_ssn
;
QUIT;
ENDRSUBMIT;

DATA POP;
SET DUSTER.POP;
RUN;


PROC SQL;
	CREATE TABLE LOANS AS 
		SELECT DISTINCT
			BF_SSN,
			MAP.LN_SEQ,
			MAP.LOAN_NUMBER
		FROM
			POP P
		INNER JOIN BANA.COMPASSLOANMAPPING MAP
			ON P.BF_SSN = MAP.BORROWERSSN
			AND P.LN_SEQ = MAP.LN_SEQ
;
QUIT;

PROC SQL;
	CREATE TABLE DATA AS 
		SELECT DISTINCT
			L.*,
			DC.InterestRate
		FROM
			LOANS L
			INNER JOIN BANA._07_08DisbClaimEnrollRecord DC
				ON DC.BORROWERSSN = L.BF_SSN
				AND DC.LOAN_NUMBER = L.LOAN_NUMBER
;
QUIT;

DATA DUSTER.DATA;
SET DATA;
RUN;

RSUBMIT;  
LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE LN65_FINAL AS
		SELECT DISTINCT
			LN65.BF_SSN,
			LN65.LN_SEQ,
			LN65.LN_RPS_SEQ,
			'NULL' AS OLD_LR_APR_RPD_DIS,
			D.INTERESTRATE AS NEW_LR_APR_RPD_DIS
		FROM
			 OLWHRM1.LN65_LON_RPS LN65
			 INNER JOIN DATA D
			 	ON D.BF_SSN = LN65.BF_SSN
				AND D.LN_SEQ = LN65.LN_SEQ
		
		WHERE
			LN65.LC_STA_LON65 = 'A'
;
QUIT;
ENDRSUBMIT;

DATA LN65_FINAL;
SET DUSTER.LN65_FINAL;
RUN;

PROC EXPORT DATA = WORK.LN65_FINAL 
            OUTFILE = "T:\ERROR 02789 DCR.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LN65"; 
RUN;
