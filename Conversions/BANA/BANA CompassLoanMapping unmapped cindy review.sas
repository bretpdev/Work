%LET BANA = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME BANA ODBC &BANA ;


PROC SQL;
	CREATE TABLE SOURCE AS 		
		SELECT DISTINCT
			D.BorrowerSSN,
			D.loan_number,
			D.LoanIdentification,
			SUM(D.PrincipalBalanceOutstanding) AS BAL,
			RepaymentPlanCode,
			MonthlyPaymentAmount
		FROM
			BANA.CompassLoanMapping MAP
			INNER JOIN BANA._07_08DisbClaimEnrollRecord D
				ON D.BorrowerSSN = MAP.BorrowerSsn
				AND D.loan_number = MAP.loan_number
			INNER JOIN BANA._03PaymentDataRecord P
				ON P.BorrowerSSN = d.BorrowerSSN
				and p.loan_number = d.loan_number
		WHERE 
			LN_SEQ IS NULL
		GROUP BY
			D.BorrowerSSN,
			D.loan_number,
			D.LoanIdentification
;
QUIT;

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
			s.bal,
			remote.la_cur_pri,
			REMOTE.BF_SSN,
			REMOTE.LN_SEQ,
			RepaymentPlanCode,
			MonthlyPaymentAmount
			
		FROM
			 SOURCE S
		left JOIN
		(
			select
				*
			from 
				OLWHRM1.ln10_lon
			where
				PF_MAJ_BCH IN ('2016067001','2016067002','2016067003','2016067004','2016067005','2016067006','2016067007','2016067008')
		)remote
			ON S.BORROWERSSN = REMOTE.BF_SSN
			AND (S.LoanIdentification = REMOTE.LF_GTR_RFR_XTN)
/*			AND S.BAL = REMOTE.LA_CUR_PRI*/
;
QUIT;
ENDRSUBMIT;

DATA RESULT;
SET DUSTER.RESULT;
RUN;

PROC EXPORT DATA = WORK.RESULT 
            OUTFILE = "T:\mapping.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Sheet1"; 
RUN;

/**/
/*PROC SQL;*/
/*	CREATE TABLE FINAL AS */
/*		SELECT*/
/*			N.**/
/*		FROM*/
/*			RESULT N*/
/*		INNER JOIN*/
/*		(*/
/*			SELECT*/
/*				BorrowerSSN,*/
/*				loan_number,*/
/*				COUNT(*)*/
/*			FROM*/
/*				RESULT*/
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
/*			N.LN_SEQ IS NOT NULL*/
/*;*/
/*QUIT;*/
/**/
/*DATA BANA.FINAL;*/
/*SET FINAL;*/
/*RUN;*/
/**/
/*PROC SQL noprint;*/
/*CONNECT TO ODBC AS EA27 (&BANA);*/
/*SELECT **/
/*FROM CONNECTION TO EA27 (*/
/*	*/
/*UPDATE*/
/*	MAP*/
/*SET*/
/*	LN_SEQ = D.LN_SEQ*/
/*FROM*/
/*	CompassLoanMapping MAP*/
/*	INNER JOIN FINAL D*/
/*		ON D.BORROWERSSN = MAP.BORROWERSSN*/
/*		AND D.LOAN_NUMBER = MAP.LOAN_NUMBER*/
/*;*/
/**/
/*DROP TABLE FINAL;*/
/**/
/**/
/*);*/
/*DISCONNECT FROM EA27;*/
/*QUIT;*/
/*		*/
