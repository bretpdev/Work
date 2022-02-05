%LET BANA = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\EA27_BANA.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME BANA ODBC &BANA ;

proc sql;
	create table pop as 
		select distinct
			b.StudentSSN
		from
			BANA._01BorrowerRecord b
			left join BANA._10_11ReferenceData r
				on r.BorrowerSSN = b.BorrowerSSN
				and r.RefSSN = b.StudentSSN
			left join BANA._01BorrowerRecord b2
				on b2.BorrowerSSN = b.StudentSSN
			left join
			(
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
						and b.studentSSN is not null
			) ex
				on ex.BorrowerSSN = b.BorrowerSSN
		where b.StudentSSN is not null	and (RefSSN is null and ex.BorrowerSSN is null and b2.borrowerssn is null)
		order by b.StudentSSN
;
quit;

PROC EXPORT DATA = WORK.pop 
            OUTFILE = "T:\STUDENT SSN'S WITH NO NAMES.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="Sheet1"; 
RUN;
