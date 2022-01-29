
%LET BANA1 = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA_1.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME BANA1 ODBC &BANA1 ;

%LET BANA2 = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA_2.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME BANA2 ODBC &BANA2 ;

%LET BANA3 = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME BANA3 ODBC &BANA3 ;


PROC IMPORT OUT= WORK.W1
            DATAFILE= "T:\Paid Ahead at ACS.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="Wave1$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

PROC IMPORT OUT= WORK.W2
            DATAFILE= "T:\Paid Ahead at ACS.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="Wave2$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

PROC IMPORT OUT= WORK.W3
            DATAFILE= "T:\Paid Ahead at ACS.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="Wave3$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

PROC IMPORT OUT= WORK.DD
            DATAFILE= "T:\nh 26666 unmapped loansdd.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="Wave 1$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

data pop;
set w1 w2 w3;
run;



LIBNAME  DUSTER  REMOTE  SERVER=DUSTER SLIBREF=WORK;
DATA DUSTER.pop; *Send data to Duster;
SET pop;

RUN;DATA DUSTER.dd; *Send data to Duster;
SET dd;
RUN;


RSUBMIT DUSTER;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
PROC SQL;

	CREATE TABLE W1_POP_LN65 AS
		SELECT DISTINCT
			LN65.BF_SSN,
			LN65.LN_SEQ,
			LN65.LN_RPS_SEQ,
			LN65.LN_RPD_MAX_TRM_REQ,
			RS10.LD_RPS_1_PAY_DU,
			coalesce(ABS(INTCK('MONTH', LN09.LD_NPD_PCV,RS10.LD_RPS_1_PAY_DU,'D')),0) AS RE_MTHS
		FROM
			 pop w1 
		INNER JOIN OLWHRM1.LN65_LON_RPS LN65
			ON LN65.BF_SSN = W1.BorrowerSSN
		INNER JOIN OLWHRM1.LN10_LON LN10
			ON LN10.BF_SSN = LN65.BF_SSN
			AND LN10.LN_SEQ = LN65.LN_SEQ
		INNER JOIN OLWHRM1.LN09_RPD_PIO_CVN LN09
			ON LN09.BF_SSN = LN65.BF_SSN
			AND LN09.LN_SEQ = LN65.LN_SEQ
		INNER JOIN
		(
			SELECT
				BF_SSN,
				LN_SEQ,
				MIN(LN_RPS_SEQ) AS LN_RPS_SEQ
			FROM
				OLWHRM1.LN65_LON_RPS
			GROUP BY
				BF_SSN,
				LN_SEQ
		)MIN_LN65
			ON MIN_LN65.BF_SSN = LN65.BF_SSN
			AND MIN_LN65.LN_SEQ = LN65.LN_SEQ
			AND MIN_LN65.LN_RPS_SEQ = LN65.LN_RPS_SEQ
		INNER JOIN OLWHRM1.RS10_BR_RPD RS10
			ON RS10.BF_SSN = MIN_LN65.BF_SSN
			AND RS10.LN_RPS_SEQ = MIN_LN65.LN_RPS_SEQ
		WHERE
			LN65.LC_TYP_SCH_DIS NOT IN ('IB', 'IL', 'IP', 'I3')
			AND LN10.LA_CUR_PRI > 0
;
	CREATE TABLE W1_POP_LN66_TOT AS 
		SELECT
			LN66.BF_SSN,
			LN66.LN_SEQ,
			LN66.LN_RPS_SEQ,
			sum(LN66.LN_RPS_TRM) as LN_RPS_TRM
		FROM
			OLWHRM1.LN66_LON_RPS_SPF LN66
			INNER JOIN W1_POP_LN65 P1
				ON P1.BF_SSN = LN66.BF_SSN
				AND P1.LN_SEQ = LN66.LN_SEQ
				AND P1.LN_RPS_SEQ = LN66.LN_RPS_SEQ 
			INNER JOIN OLWHRM1.LN10_LON LN10
				ON LN10.BF_SSN = LN66.BF_SSN
				AND LN10.LN_SEQ = LN66.LN_SEQ
		WHERE
			LN66.LN_GRD_RPS_SEQ ^= 1
			AND LN10.LA_CUR_PRI > 0
		GROUP BY
			LN66.BF_SSN,
			LN66.LN_SEQ,
			LN66.LN_RPS_SEQ
;

	CREATE TABLE W1_POP_LN66 AS 
		SELECT
			LN66.BF_SSN,
			LN66.LN_SEQ,
			LN66.LN_RPS_SEQ,
			LN66.LN_RPS_TRM,
			ln66.LN_GRD_RPS_SEQ
		FROM
			OLWHRM1.LN66_LON_RPS_SPF LN66
			INNER JOIN W1_POP_LN65 P1
				ON P1.BF_SSN = LN66.BF_SSN
				AND P1.LN_SEQ = LN66.LN_SEQ
				AND P1.LN_RPS_SEQ = LN66.LN_RPS_SEQ
			INNER JOIN OLWHRM1.LN10_LON LN10
				ON LN10.BF_SSN = LN66.BF_SSN
				AND LN10.LN_SEQ = LN66.LN_SEQ
		WHERE
			LN66.LN_GRD_RPS_SEQ = 1
			AND LN10.LA_CUR_PRI > 0
;

	CREATE TABLE LN09 AS 
		SELECT
			REMOTE.*
		FROM
			OLWHRM1.LN09_RPD_PIO_CVN REMOTE
			INNER JOIN DD 
				ON DD.BORROWERSSN = REMOTE.BF_SSN
;	
CREATE TABLE RS10 AS 
		SELECT
			REMOTE.*
		FROM
			OLWHRM1.RS10_BR_RPD REMOTE
			INNER JOIN DD 
				ON DD.BORROWERSSN = REMOTE.BF_SSN
;

CREATE TABLE LN65 AS 
		SELECT
			REMOTE.*
		FROM
			OLWHRM1.LN65_LON_RPS REMOTE
			INNER JOIN DD 
				ON DD.BORROWERSSN = REMOTE.BF_SSN
;

CREATE TABLE LN66 AS 
		SELECT
			REMOTE.*
		FROM
			OLWHRM1.LN66_LON_RPS_SPF REMOTE
			INNER JOIN DD 
				ON DD.BORROWERSSN = REMOTE.BF_SSN
;


QUIT;
ENDRSUBMIT;

DATA W1_POP_LN65;
	SET DUSTER.W1_POP_LN65;
RUN;

DATA W1_POP_LN66;
	SET DUSTER.W1_POP_LN66;
RUN;

DATA W1_POP_LN66_TOT;
	SET DUSTER.W1_POP_LN66_TOT;
RUN;

DATA LN09;
	SET DUSTER.LN09;
RUN;

DATA RS10;
	SET DUSTER.RS10;
RUN;


DATA LN65;
	SET DUSTER.LN65;
RUN;

DATA LN66;
	SET DUSTER.LN66;
RUN;


proc sql;
	create table deferment_his as 
		select
			*
		from
			bana1._12DefermentHistory

	union all

		select
			*
		from
			bana2._12DefermentHistory
	union all

		select
			*
		from
			bana3._12DefermentHistory
;
quit;

proc sql;
	create table compass_loans as 
		select
			*
		from
			bana1.compassloanmapping

	union all

		select
			*
		from
			bana2.compassloanmapping
	union all

		select
			*
		from
			bana3.compassloanmapping
;
quit;

proc sql;
	create table LOAN_03 as 
		select
			loan_number
      		,BorrowerSSN
	 		,RemainingLoanTerm
		from
			bana1._03PaymentDataRecord

	union all

		select
			loan_number
      		,BorrowerSSN
	 		,RemainingLoanTerm
		from
			bana2._03PaymentDataRecord
	union all

		select
			loan_number
      		,BorrowerSSN
	 		,RemainingLoanTerm
		from
			bana3._03PaymentDataRecord
;
quit;


PROC SQL;
	CREATE TABLE FINAL_DEF_POP AS 
		SELECT distinct
			w1.bf_ssn,
			w1.ln_seq,
			sum(intck('month',datepart(DefermentBeginDate),datepart(DefermentEndDate))) as months_used
		from
			deferment_his d
			inner join compass_loans m
				on m.borrowerssn = d.borrowerssn
				and m.loan_number = d.loan_number

			inner join W1_POP_LN65 w1
				on w1.bf_ssn = m.borrowerssn
				and w1.ln_seq = m.ln_seq
		where
			 w1.LD_RPS_1_PAY_DU BETWEEN datepart(DefermentBeginDate) AND datepart(DefermentEndDate) OR datepart(DefermentBeginDate) > w1.LD_RPS_1_PAY_DU 
		group by
			d.BorrowerSSN,
			m.ln_seq		
;
quit;

PROC SQL;
	CREATE TABLE LN65_FINAL AS 
		SELECT DISTINCT
			M.BORROWERSSN,
			M.LN_SEQ,
			W1.LN_RPS_SEQ,
			W1.LN_RPD_MAX_TRM_REQ AS LN_RPD_MAX_TRM_REQ_OLD,
			(L.RemainingLoanTerm + RE_MTHS) - COALESCE(F.months_used,0) AS LN_RPD_MAX_TRM_REQ_NEW
		FROM
			LOAN_03 L
			inner join compass_loans m
				on m.borrowerssn = L.borrowerssn
				and m.loan_number = L.loan_number
			inner join W1_POP_LN65 w1
				on w1.bf_ssn = m.borrowerssn
				and w1.ln_seq = m.ln_seq
			LEFT JOIN FINAL_DEF_POP F
				ON F.BF_SSN = M.BORROWERSSN
				AND F.ln_seq = M.ln_seq
			WHERE
				L.RemainingLoanTerm > 0
;
QUIT;

PROC SQL;	
	CREATE TABLE LN66_FINAL AS 
		SELECT
			P.BF_SSN,
			P.LN_SEQ,
			P.LN_RPS_SEQ,
			p.LN_GRD_RPS_SEQ AS LN_GRD_RPS_SEQ,
			P.LN_RPS_TRM AS LN_RPS_TRM_OLD,
			(LN65.LN_RPD_MAX_TRM_REQ_NEW - COALESCE(LN65_TOT.LN_RPS_TRM,0)) AS LN_RPS_TRM_NEW
		FROM
			W1_POP_LN66 P
			INNER JOIN LN65_FINAL LN65
				ON LN65.BORROWERSSN = P.BF_SSN
				AND LN65.LN_SEQ = P.LN_SEQ
				AND LN65.LN_RPS_SEQ = P.LN_RPS_SEQ
			LEFT JOIN W1_POP_LN66_TOT LN65_TOT
				ON LN65_TOT.BF_SSN = P.BF_SSN
				AND LN65_TOT.LN_SEQ = P.LN_SEQ
				AND LN65_TOT.LN_RPS_SEQ = P.LN_RPS_SEQ
;
QUIT;

PROC EXPORT DATA = WORK.LN65_FINAL 
            OUTFILE = "T:\NH 26666.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LN65_CHANGE"; 
RUN;

PROC EXPORT DATA = WORK.LN66_FINAL 
            OUTFILE = "T:\NH 26666.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LN66_CHANGE"; 
RUN;

PROC EXPORT DATA = WORK.LN09 
            OUTFILE = "T:\NH 26666_DUMP_DATA.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LN09"; 
RUN;


PROC EXPORT DATA = WORK.RS10 
            OUTFILE = "T:\NH 26666_DUMP_DATA.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RS10"; 
RUN;


PROC EXPORT DATA = WORK.LN65 
            OUTFILE = "T:\NH 26666_DUMP_DATA.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LN65"; 
RUN;

PROC EXPORT DATA = WORK.LN66 
            OUTFILE = "T:\NH 26666_DUMP_DATA.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LN66"; 
RUN;


			


