/*TODO - create macro variable for Major Batch Strings to be used on line 63 below*/
%LET BANA = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME BANA ODBC &BANA ;

PROC SQL;
	CREATE TABLE SOURCE AS 
		SELECT distinct
			BORROWERSSN,
			LOAN_NUMBER,
			LoanIdentification,
			UnderlyingLoanType,
			CommonLineUniqueID
		FROM
			BANA._07_08DisbClaimEnrollRecord

;
QUIT;

/*TEST*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK  ;*/

/*LIVE*/
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

DATA DUSTER.SOURCE; *Send data to Duster;
SET SOURCE;
RUN;

RSUBMIT;  

/*%let DB = DLGSWQUT;  *This is test;*/
%let DB = DLGSUTWH;  *This is live;

LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE RESULT AS
		SELECT DISTINCT
			S.BORROWERSSN,
			S.LOAN_NUMBER,
			S.LoanIdentification,
			s.UnderlyingLoanType,
			REMOTE.BF_SSN,
			REMOTE.LN_SEQ,
			LF_GTR_RFR,
			IC_LON_PGM,
			LF_LON_ALT,
			LN_LON_ALT_SEQ,
			CASE
				WHEN IC_LON_PGM IN ('SUBCNS','SUBSPC') THEN 'S'
				WHEN IC_LON_PGM IN ('UNCNS' ,'UNSPC') THEN 'U'
				WHEN IC_LON_PGM = 'CNSLDN' THEN 'C'
				ELSE ''
			END AS SUB
		FROM
			 SOURCE S
		left JOIN
		(
			select
				*
			from 
				OLWHRM1.ln10_lon
			where
				PF_MAJ_BCH in('2016067001','2016067002','2016067003','2016067004','2016067005','2016067006','2016067007','2016067008')
		)remote
			ON S.BORROWERSSN = REMOTE.BF_SSN
			AND (S.LoanIdentification = REMOTE.LF_GTR_RFR_XTN)
;
QUIT;
ENDRSUBMIT;


DATA RESULT;
SET DUSTER.RESULT;
RUN;
/**/
PROC SQL;
	CREATE TABLE CONSOL AS
		SELECT
			*
		FROM
			RESULT
		WHERE
			SUB ^= ''
;
QUIT;

PROC SQL;
	CREATE TABLE NONCONSOL AS
		SELECT DISTINCT
			BORROWERSSN,
			LOAN_NUMBER,
			BF_SSN,
			LN_SEQ
		FROM
			RESULT
		WHERE
			SUB = ''
;
QUIT;
/**/
PROC SQL;
	CREATE TABLE UNMAPPEDLOANS_NONCONSOL AS 
		SELECT
			N.*,
			S.CommonLineUniqueID,
			S.LoanIdentification

		FROM
			NONCONSOL N
		INNER JOIN
		(
			SELECT
				BorrowerSSN,
				loan_number,
				COUNT(*)
			FROM
				NONCONSOL
			GROUP BY
				BorrowerSSN,
				loan_number
		HAVING COUNT(*) > 1
		) L
			ON L.BORROWERSSN = N.BORROWERSSN
			AND L.LOAN_NUMBER = N.LOAN_NUMBER
		INNER JOIN SOURCE S
			ON S.BORROWERSSN = N.BORROWERSSN
			AND S.LOAN_NUMBER = N.LOAN_NUMBER
;
QUIT;

DATA DUSTER.UNMAPPEDLOANS_NONCONSOL; *Send data to Duster;
SET UNMAPPEDLOANS_NONCONSOL;
RUN;

RSUBMIT;  
LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE UNMAPPEDLOANS_NONCONSOL_REMOTE AS
		SELECT DISTINCT
			S.BORROWERSSN,
			S.LOAN_NUMBER,
			REMOTE.BF_SSN,
			REMOTE.LN_SEQ,
			LF_GTR_RFR,
			IC_LON_PGM,
			CATS(REMOTE.LF_LON_ALT, put(REMOTE.LN_LON_ALT_SEQ,z2.)) AS COMMONLINDID_REMOTE,
			S.CommonLineUniqueID
		FROM
			 UNMAPPEDLOANS_NONCONSOL S
		left JOIN
		(
			select
				*
			from 
				OLWHRM1.ln10_lon
			where
				PF_MAJ_BCH in( '2016029001','2016029002','2016029003','2016029004')
		)remote
			ON S.BORROWERSSN = REMOTE.BF_SSN
			AND (S.LoanIdentification = REMOTE.LF_GTR_RFR_XTN)
			AND S.CommonLineUniqueID = CATS(REMOTE.LF_LON_ALT, put(REMOTE.LN_LON_ALT_SEQ,z2.))
;
QUIT;
ENDRSUBMIT;

DATA UNMAPPEDLOANS_NONCONSOL_REMOTE;
SET DUSTER.UNMAPPEDLOANS_NONCONSOL_REMOTE;
RUN;
/**/
/*PROC SQL;*/
/*	CREATE TABLE UNMAPPEDLOANS_NONCONSOL_1 AS */
/*		SELECT*/
/*			N.**/
/*		FROM*/
/*			UNMAPPEDLOANS_NONCONSOL_REMOTE N*/
/*		INNER JOIN*/
/*		(*/
/*			SELECT*/
/*				BorrowerSSN,*/
/*				loan_number,*/
/*				COUNT(*)*/
/*			FROM*/
/*				UNMAPPEDLOANS_NONCONSOL_REMOTE*/
/*			GROUP BY*/
/*				BorrowerSSN,*/
/*				loan_number*/
/*		HAVING COUNT(*) > 1*/
/*		) L*/
/*			ON L.BORROWERSSN = N.BORROWERSSN*/
/*			AND L.LOAN_NUMBER = N.LOAN_NUMBER*/
/*		INNER JOIN SOURCE S*/
/*			ON S.BORROWERSSN = N.BORROWERSSN*/
/*			AND S.LOAN_NUMBER = N.LOAN_NUMBER*/
/*	UNION ALL*/
/**/
/*	SELECT*/
/*			N.**/
/*		FROM*/
/*			UNMAPPEDLOANS_NONCONSOL_REMOTE N*/
/*		INNER JOIN*/
/*		(*/
/*			SELECT*/
/*				BorrowerSSN,*/
/*				loan_number,*/
/*				COUNT(*)*/
/*			FROM*/
/*				UNMAPPEDLOANS_NONCONSOL_REMOTE*/
/*			GROUP BY*/
/*				BorrowerSSN,*/
/*				loan_number*/
/*		HAVING COUNT(*) = 1*/
/*		) L*/
/*			ON L.BORROWERSSN = N.BORROWERSSN*/
/*			AND L.LOAN_NUMBER = N.LOAN_NUMBER*/
/*		INNER JOIN SOURCE S*/
/*			ON S.BORROWERSSN = N.BORROWERSSN*/
/*			AND S.LOAN_NUMBER = N.LOAN_NUMBER*/
/*		WHERE*/
/*			N.LN_SEQ IS NULL*/
/*;*/
/*QUIT;*/
/**/
PROC SQL;
	CREATE TABLE MAPPEDLOANS_NONCONSOL_1 AS 
		SELECT
			N.*
		FROM
			UNMAPPEDLOANS_NONCONSOL_REMOTE N
		INNER JOIN
		(
			SELECT
				BorrowerSSN,
				loan_number,
				COUNT(*)
			FROM
				UNMAPPEDLOANS_NONCONSOL_REMOTE
			GROUP BY
				BorrowerSSN,
				loan_number
		HAVING COUNT(*) = 1
		) L
			ON L.BORROWERSSN = N.BORROWERSSN
			AND L.LOAN_NUMBER = N.LOAN_NUMBER
		INNER JOIN SOURCE S
			ON S.BORROWERSSN = N.BORROWERSSN
			AND S.LOAN_NUMBER = N.LOAN_NUMBER
		WHERE
			N.LN_SEQ IS NOT NULL
;
QUIT;
DATA BANA.MAPPEDLOANS_NONCONSOL_1;
SET MAPPEDLOANS_NONCONSOL_1;
RUN;

PROC SQL noprint;
CONNECT TO ODBC AS EA27 (&BANA);
SELECT *
FROM CONNECTION TO EA27 (
	
UPDATE
	MAP
SET
	LN_SEQ = D.LN_SEQ
FROM
	CompassLoanMapping MAP
	INNER JOIN MAPPEDLOANS_NONCONSOL_1 D
		ON D.BORROWERSSN = MAP.BORROWERSSN
		AND D.LOAN_NUMBER = MAP.LOAN_NUMBER
;

DROP TABLE MAPPEDLOANS_NONCONSOL_1;


);
DISCONNECT FROM EA27;
QUIT;

PROC SQL;
	CREATE TABLE MAPPEDLOANS_NONCONSOL AS 
		SELECT
			N.*
		FROM
			NONCONSOL N
		INNER JOIN
		(
			SELECT
				BorrowerSSN,
				loan_number,
				COUNT(*)
			FROM
				NONCONSOL
			GROUP BY
				BorrowerSSN,
				loan_number
		HAVING COUNT(*) = 1
		) L
			ON L.BORROWERSSN = N.BORROWERSSN
			AND L.LOAN_NUMBER = N.LOAN_NUMBER
;
QUIT;


DATA BANA.MAPPEDLOANS_NONCONSOL;
SET MAPPEDLOANS_NONCONSOL;
RUN;

PROC SQL noprint;
CONNECT TO ODBC AS EA27 (&BANA);
SELECT *
FROM CONNECTION TO EA27 (
	
UPDATE
	MAP
SET
	LN_SEQ = D.LN_SEQ
FROM
	CompassLoanMapping MAP
	INNER JOIN MAPPEDLOANS_NONCONSOL D
		ON D.BORROWERSSN = MAP.BORROWERSSN
		AND D.LOAN_NUMBER = MAP.LOAN_NUMBER
;

DROP TABLE MAPPEDLOANS_NONCONSOL;


);
DISCONNECT FROM EA27;
QUIT;

/**/;
PROC SQL;
	CREATE TABLE SUB AS 
		SELECT DISTINCT
			BORROWERSSN,
			LOAN_NUMBER,
			BF_SSN,
			LN_SEQ
		FROM
			CONSOL
		WHERE
			UnderlyingLoanType IN ('DD','DS','SF','FS','PK')
			AND SUB = 'S'
;
QUIT;
/**/
/*PROC SQL;*/
/*	CREATE TABLE UNMAPPEDLOANS_SUB AS */
/*		SELECT*/
/*			N.**/
/*		FROM*/
/*			SUB N*/
/*		INNER JOIN*/
/*		(*/
/*			SELECT*/
/*				BorrowerSSN,*/
/*				loan_number,*/
/*				COUNT(*)*/
/*			FROM*/
/*				SUB*/
/*			GROUP BY*/
/*				BorrowerSSN,*/
/*				loan_number*/
/*		HAVING COUNT(*) > 1*/
/*		) L*/
/*			ON L.BORROWERSSN = N.BORROWERSSN*/
/*			AND L.LOAN_NUMBER = N.LOAN_NUMBER*/
/*;*/
/*QUIT;*/
/**/
PROC SQL;
	CREATE TABLE MAPPEDLOANS_SUB AS 
		SELECT
			N.*
		FROM
			SUB N
		INNER JOIN
		(
			SELECT
				BorrowerSSN,
				loan_number,
				COUNT(*)
			FROM
				SUB
			GROUP BY
				BorrowerSSN,
				loan_number
		HAVING COUNT(*) = 1
		) L
			ON L.BORROWERSSN = N.BORROWERSSN
			AND L.LOAN_NUMBER = N.LOAN_NUMBER
;
QUIT;

DATA BANA.MAPPEDLOANS_SUB;
SET MAPPEDLOANS_SUB;
RUN;

PROC SQL noprint;
CONNECT TO ODBC AS EA27 (&BANA);
SELECT *
FROM CONNECTION TO EA27 (
	
UPDATE
	MAP
SET
	LN_SEQ = D.LN_SEQ
FROM
	CompassLoanMapping MAP
	INNER JOIN MAPPEDLOANS_SUB D
		ON D.BORROWERSSN = MAP.BORROWERSSN
		AND D.LOAN_NUMBER = MAP.LOAN_NUMBER
;

DROP TABLE MAPPEDLOANS_SUB;
);
DISCONNECT FROM EA27;
QUIT;

PROC SQL;
	CREATE TABLE UNSUB AS 
		SELECT DISTINCT
			BORROWERSSN,
			LOAN_NUMBER,
			BF_SSN,
			LN_SEQ,
			IC_LON_PGM
		FROM
			CONSOL
		WHERE
			UnderlyingLoanType IN ('DP','DC','DU','SU','PL','FU','SL','DP','PL','NS','HP')
			AND SUB = 'U'
;
QUIT;
/**/
/*PROC SQL;*/
/*	CREATE TABLE UNMAPPEDLOANS_UNSUB AS */
/*		SELECT*/
/*			N.**/
/*		FROM*/
/*			UNSUB N*/
/*		INNER JOIN*/
/*		(*/
/*			SELECT*/
/*				BorrowerSSN,*/
/*				loan_number,*/
/*				COUNT(*)*/
/*			FROM*/
/*				UNSUB*/
/*			GROUP BY*/
/*				BorrowerSSN,*/
/*				loan_number*/
/*		HAVING COUNT(*) > 1*/
/*		) L*/
/*			ON L.BORROWERSSN = N.BORROWERSSN*/
/*			AND L.LOAN_NUMBER = N.LOAN_NUMBER*/
/*;*/
/*QUIT;*/
/**/;
PROC SQL;
	CREATE TABLE MAPPEDLOANS_UNSUB AS 
		SELECT
			N.*
		FROM
			UNSUB N
		INNER JOIN
		(
			SELECT
				BorrowerSSN,
				loan_number,
				COUNT(*)
			FROM
				UNSUB
			GROUP BY
				BorrowerSSN,
				loan_number
		HAVING COUNT(*) = 1
		) L
			ON L.BORROWERSSN = N.BORROWERSSN
			AND L.LOAN_NUMBER = N.LOAN_NUMBER
;
QUIT;

DATA BANA.MAPPEDLOANS_UNSUB;
SET MAPPEDLOANS_UNSUB;
RUN;

PROC SQL noprint;
CONNECT TO ODBC AS EA27 (&BANA);
SELECT *
FROM CONNECTION TO EA27 (
	
UPDATE
	MAP
SET
	LN_SEQ = D.LN_SEQ
FROM
	CompassLoanMapping MAP
	INNER JOIN MAPPEDLOANS_UNSUB D
		ON D.BORROWERSSN = MAP.BORROWERSSN
		AND D.LOAN_NUMBER = MAP.LOAN_NUMBER
;

DROP TABLE MAPPEDLOANS_UNSUB;
);
DISCONNECT FROM EA27;
QUIT;

PROC SQL;
	CREATE TABLE REMAININGUNMAPPED AS
		SELECT
			BORROWERSSN,
			LOAN_NUMBER,
			LN_SEQ
		FROM
			BANA.COMPASSLOANMAPPING
;
QUIT;


libname rmt 'C:\serf_file\';

proc sql;
	create table borrowers as 
		select distinct
			r.borrowerssn,
			r.loan_number
		from
			REMAININGUNMAPPED r
			left join rmt.ln10 l
				on r.borrowerssn = l.bf_ssn
				and r.ln_seq = l.ln_seq
		where l.ln_seq is null
;
quit;

proc sql;
	create table loans as 
		select
			b.borrowerssn,
			b.loan_number,
			l.*
		from 
			rmt.ln10 l
			inner join borrowers b
				on b.borrowerssn = l.bf_ssn
;
quit;

proc sql;
	create table bwrtest as 
		select
			*
		from
			rmt.ln10
		where
			bf_ssn = '001708859'
			and ln_seq not in (31,32,37,38)
;
quit;

PROC EXPORT DATA = WORK.bwrtest 
            OUTFILE = "T:\split_loans.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="004883566"; 
RUN;
