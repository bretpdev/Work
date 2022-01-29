%LET BANA = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME BANA ODBC &BANA ;

PROC SQL;
	CREATE TABLE POP_40103 AS 
		SELECT DISTINCT
				BR_SSN,
				err.LN_SEQ,
				SCRAServiceBeginDate,
	      		SCRAServiceEndDate
		FROM
			BANA.Borrower_Errors ERR
			INNER JOIN BANA.CompassLoanMapping MAP
				ON ERR.BR_SSN = MAP.BORROWERSSN
				AND input(ERR.LN_SEQ, best12.) = MAP.LN_SEQ
			INNER JOIN BANA._03PaymentDataRecord D
				ON D.BORROWERSSN = map.borrowerssn
				and d.loan_number = map.loan_number
		WHERE
			ERR.ERROR_CODE = '40103'

	UNION ALL

		SELECT DISTINCT
			BR_SSN,
			err.LN_SEQ,
			SCRAServiceBeginDate,
      		SCRAServiceEndDate
		FROM
			BANA.Borrower_Errors ERR
			INNER JOIN BANA.CompassLoanMapping MAP
				ON ERR.BR_SSN = MAP.BORROWERSSN
				AND input(ERR.LN_SEQ, best12.) = MAP.LN_SEQ
			INNER JOIN BANA._03PaymentDataRecord D
				ON D.BORROWERSSN = map.borrowerssn
				and d.loan_number = map.loan_number
		WHERE
			SCRAServiceBeginDate IS NOT NULL OR SCRAServiceEndDate IS NOT NULL
			AND ERR.ERROR_CODE ^= '40103'
;
QUIT; 

PROC SQL;
	CREATE TABLE POP_40103_FINAL AS 
		SELECT DISTINCT
			*
		FROM
			POP_40103
;
QUIT;
		

PROC EXPORT DATA = WORK.POP_40103_FINAL 
            OUTFILE = "T:\NH 25809.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
