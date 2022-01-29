PROC IMPORT OUT= WORK.DATA
            DATAFILE= "T:\sas\SystemLettersThatHaveNotGenerated.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="SheetX$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

%LET BSYS = %STR(REQUIRED="FILEDSN=X:\PADR\ODBC\BSYS.dsn; update_lock_typ=nolock; bl_keepnulls=no");
LIBNAME BSYS ODBC &BSYS ;



PROC SQL;
CREATE TABLE Letters AS 
	SELECT DISTINCT
		DOCNAME,
		ID
	FROM 
		BSYS.LTDB_DAT_DocDetail
;
QUIT;

proc sql;
	create table final as 
		select distinct
			l.docname as LetterName,
			d.*
		from
			letters l
		inner join data d
			on l.id = d.id
;
quit;

PROC EXPORT DATA = WORK.final 
            OUTFILE = "T:\SystemLettersThatHaveNotGenerated.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;

