%LET BANA = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME BANA ODBC &BANA ;

proc sql;
	create table pop as 
		select distinct
			b.BorrowerSSN,
			b.StudentSSN
		from
			BANA._01BorrowerRecord b
			inner join
			(
				select
					BorrowerSSN,
					StudentSSN
				from
					BANA._01BorrowerRecord 
			)pop
				on pop.BorrowerSSN = b.BorrowerSSN
			where
				b.BorrowerSSN = pop.StudentSSN
;
quit;

PROC EXPORT DATA = WORK.pop 
            OUTFILE = "T:\Borrowers and student SSN.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Sheet1"; 
RUN;
