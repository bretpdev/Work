/*TODO - create macro variable for Major Batch Strings to be used on line 75 below*/
%LET BANA = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME BANA ODBC &BANA ;

PROC SQL;
	CREATE TABLE POP AS 
		select DISTINCT
			loans.BorrowerSSN,
			loans.loan_number,
			DisbursementNumber,
			DisbursementDate,
			DisbursementAmount,
			LoanPeriodStartDate,
			LoanIdentification,
			LoanPeriodEndDate,
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
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK  ;*/

/*LIVE*/
LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK;

DATA DUSTER.POP; *Send data to Duster;
SET POP;
RUN;

RSUBMIT;  
/*%let DB = DLGSWQUT;  *This is test;*/
%let DB = DLGSUTWH;  *This is live;
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
			ln10.PF_MNR_BCH,
			p.CommonLineUniqueID
		FROM 
			POP P
			LEFT JOIN OLWHRM1.LN10_LON LN10
				ON LN10.BF_SSN = P.BORROWERSSN
				AND P.LoanIdentification = LN10.LF_GTR_RFR_XTN
		WHERE
			LN10.PF_MAJ_BCH in ('2016067001','2016067002','2016067003','2016067004','2016067005','2016067006','2016067007','2016067008')

		
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
			CommonLineUniqueID,
			LN_SEQ,
			SUM(PrincipalBalanceOutstanding) AS TOTAL_BALANCE_OF_LN_SEQ
		FROM
			TEST
		GROUP BY
			BORROWERSSN,
			LOAN_NUMBER,
			LoanIdentification,
			CommonLineUniqueID,
			LN_SEQ
;
QUIT;


PROC SQL;
	CREATE TABLE FINAL AS
		SELECT DISTINCT
			T.PF_MAJ_BCH,
			T.PF_MNR_BCH,
			A.BORROWERSSN,
			A.LN_SEQ,
			A.LoanIdentification,
			a.CommonLineUniqueID,
			A.TOTAL_BALANCE_OF_LN_SEQ,
			T.la_cur_pri,
			T.DisbursementNumber,
			datepart(T.DisbursementDate) as DisbursementDate,
			T.DisbursementAmount,
			datepart(T.LoanPeriodStartDate) as LoanPeriodStartDate,
			datepart(T.LoanPeriodEndDate) as LoanPeriodEndDate
		FROM
			AGG A
			LEFT JOIN TEST T
				ON T.BORROWERSSN = A.BORROWERSSN
				AND T.LOAN_NUMBER = A.LOAN_NUMBER
				AND T.LN_SEQ = A.LN_SEQ
				AND T.la_cur_pri = A.TOTAL_BALANCE_OF_LN_SEQ
		
		ORDER BY
			BORROWERSSN,
			LN_SEQ
		
;
QUIT;

data final;
set final;
format DisbursementDate mmddyy10.;
format LoanPeriodStartDate mmddyy10.;
format LoanPeriodEndDate mmddyy10.;
run;

PROC EXPORT DATA = WORK.FINAL 
            OUTFILE = "T:\LOAN SPLIT FIX EXTRA DATA.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Sheet1"; 
RUN;
