/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

FILENAME REPORTZ "&RPTLIB/NH25704.RZ";
FILENAME REPORT2 "&RPTLIB/NH25704.R2";

LIBNAME EA27BANA ODBC REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA.dsn; bl_keepnulls=no; READ_ISOLATION_LEVEL=RU" SCHEMA= dbo;

PROC SQL;
	CREATE TABLE Borrower_Errors AS
		SELECT 
			B.BORROWERSSN,
			B.LN_SEQ,
			C.NoteDate
		FROM
			EA27BANA.CompassLoanMapping B
/*			On A.BR_SSN = B.BorrowerSSN*/
/*			And Input (A.LN_SEQ, best3.) = B.LN_SEQ*/
			INNER JOIN EA27BANA._07_08DisbClaimEnrollRecord C
				ON B.BORROWERSSN = C.BorrowerSSN
				AND B.Loan_Number = C.Loan_Number
		;
QUIT;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER  SLIBREF=WORK; /*live*/
/*LIBNAME  DUSTER  REMOTE  SERVER=QADBD004 SLIBREF=WORK ; /*test*/
DATA DUSTER.Borrower_Errors; *Send data to Duster;
SET Borrower_Errors;
RUN;
/*DATA DUSTER.DisbClaimEnrollRecord0708; *Send data to Duster;*/
/*SET DisbClaimEnrollRecord0708;*/
/*RUN;*/

RSUBMIT;
%LET DB = DLGSUTWH; /*live*/
/*%LET DB = DLGSWQUT; /*test*/
LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1; /*needed for SQL queries, but not for DB2 queries*/

/*Report level variables*/
%LET BanaLoan = '829769';
%LET ErrorCode = '04084';

PROC SQL;
CREATE TABLE AACerror AS
	SELECT DISTINCT
		LN10.BF_SSN AS Borrower_SSN,
		LN10.LN_SEQ AS Loan_Seq,
		'NULL' AS OLD_LD_MPN_EXP,
		INTNX('year',datepart(BE.NoteDate),10,'same') AS New_LD_MPN_EXP
	FROM
		OLWHRM1.LN10_LON LN10
		INNER JOIN OLWHRM1.LN30_LON_ERR LN30
			ON LN10.BF_SSN = LN30.BF_SSN
			AND LN10.LN_SEQ = LN30.LN_SEQ
		INNER JOIN WORK.Borrower_Errors BE
			ON LN10.BF_SSN = BE.BORROWERSSN
			AND LN10.LN_SEQ = BE.LN_SEQ
	WHERE
		LN10.LF_LON_CUR_OWN = &BanaLoan
		AND LN30.PF_ERR_MSG = &ErrorCode
		AND LN10.LD_MPN_EXP IS NULL
;

%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  

QUIT;

DATA AACerror2;
	SET AACerror;
	FORMAT New_LD_MPN_EXP mmddyy10.;
RUN;

ENDRSUBMIT;

DATA AACerror3;
	SET DUSTER.AACerror2;
RUN;

PROC EXPORT
		DATA=AACerror3
		OUTFILE="T:\UNH 25704.xlsx"
		DBMS = EXCEL
		REPLACE;
		SHEET="LN10";
RUN;
