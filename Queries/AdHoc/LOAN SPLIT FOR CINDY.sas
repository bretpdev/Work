%LET BANA = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA_2.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME BANA ODBC &BANA ;

PROC SQL;
	CREATE TABLE POP AS 
		select DISTINCT
			loans.BorrowerSSN,
			loans.loan_number,
			DisbursementNumber,
			LoanIdentification,
			PrincipalBalanceOutstanding,
			CommonLineUniqueID
		from
			BANA._07_08DisbClaimEnrollRecord loans
		inner join
		(
			select
				BorrowerSSN,
				loan_number,
				count(distinct LoanIdentification) as loan_count
			from	
				BANA._07_08DisbClaimEnrollRecord
			group by
				BorrowerSSN,
				loan_number
			having 
				count(distinct LoanIdentification) > 1
		) bad_loans
			on loans.BorrowerSSN = bad_loans.BorrowerSSN
			and loans.loan_number = bad_loans.loan_number
		where
			loans.PrincipalBalanceOutstanding ^= 0
		order by 
			BorrowerSSN
;
QUIT;

/*TEST*/
LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK  ;

/*LIVE*/
/*LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;*/

DATA DUSTER.POP; *Send data to Duster;
SET POP;
RUN;

RSUBMIT;  
%let DB = DLGSWQUT;  *This is test;
/*%let DB = DLGSUTWH;  *This is live;*/
LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE test AS
		SELECT DISTINCT
			P.*,
			LN10.LN_SEQ,
			LN10.LF_GTR_RFR_XTN,
			CATS(LN10.LF_LON_ALT, put(LN10.LN_LON_ALT_SEQ,z2.)) as remote_commonline,
			ln10.la_cur_pri,
			ln10.ic_lon_pgm,
			ln10.PF_MAJ_BCH,
			ln10.PF_MNR_BCH
		FROM 
			POP P
			LEFT JOIN OLWHRM1.LN10_LON LN10
				ON LN10.BF_SSN = P.BORROWERSSN
				AND P.LoanIdentification = LN10.LF_GTR_RFR_XTN
		WHERE
			LN10.PF_MAJ_BCH in('2016029001','2016029002','2016029003','2016029004')

		
;
QUIT;
ENDRSUBMIT;

data test;
set duster.test;
run;

PROC SQL;
	CREATE TABLE AGG AS 
		SELECT
			BORROWERSSN,
			LOAN_NUMBER,
			LoanIdentification,
			LN_SEQ,
			SUM(PrincipalBalanceOutstanding) AS BALANCE
		FROM
			TEST
		GROUP BY
			BORROWERSSN,
			LOAN_NUMBER,
			LoanIdentification,
			LN_SEQ
;
QUIT;


PROC SQL;
	CREATE TABLE FINAL AS
		SELECT
			T.PF_MAJ_BCH,
			T.PF_MNR_BCH,
			A.BORROWERSSN,
			A.LOAN_NUMBER,
			A.LN_SEQ,
			A.LoanIdentification,
			A.BALANCE,
			T.la_cur_pri
		FROM
			AGG A
			LEFT JOIN TEST T
				ON T.BORROWERSSN = A.BORROWERSSN
				AND T.LOAN_NUMBER = A.LOAN_NUMBER
				AND T.LN_SEQ = A.LN_SEQ
				AND T.la_cur_pri = A.BALANCE
;
QUIT;

PROC EXPORT DATA = WORK.FINAL 
            OUTFILE = "T:\LOAN SPLIT FIX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Sheet1"; 
RUN;
