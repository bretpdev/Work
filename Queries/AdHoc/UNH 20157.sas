LIBNAME  DUSTER  REMOTE  SERVER=DUSTER SLIBREF=work  ;
RSUBMIT DUSTER;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUK3 test;*/
%let DB = DLGSUTWH;  *This is live;

LIBNAME OLWHRM1 DB2 DATABASE=&DB OWNER=OLWHRM1;
PROC SQL;
	CONNECT TO DB2 (DATABASE=&DB);

	CREATE TABLE COLUMNS AS
		SELECT	
			*
		FROM	
			CONNECTION TO DB2 
				(
					SELECT 
						C.NAME AS COLUMN_NAME,
						C.TBNAME
						
					FROM
						sysibm.SYSCOLUMNS C
					INNER JOIN 
					(
						SELECT *
						FROM
							SYSIBM.TABLES
						WHERE TABLE_SCHEMA = 'OLWHRM1'

					) T
					ON T.TABLE_NAME = C.TBNAME
					where (C.TBNAME LIKE 'LP%' OR C.TBNAME LIKE 'OW%')

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DB2;
QUIT;

ENDRSUBMIT;

data tables;
set duster.columns;
run;

data owner;
set tables;
where column_name = 'IF_OWN';
run;

DATA OW30;
SET TABLES;
WHERE TBNAME LIKE 'OW30%' and column_name = 'IF_BUY_OWN';
RUN;

data gaur;
set tables;
where column_name = 'IF_GTR';
run;

proc sql;
	create table gaur as
		select
			distinct 
				* 
			from gaur
			where
				tbname not in (select tbname from owner)
;
quit;

%MACRO DATADUMP(TAB, COL) ;

%SYSLPUT TAB = &TAB;
%SYSLPUT COL = &COL;

RSUBMIT DUSTER;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE &TAB AS
		SELECT	
			*
		FROM
			 OLWHRM1.&TAB
		WHERE 
			&COL IN ('826717','830248')
;
QUIT;
ENDRSUBMIT;

PROC EXPORT DATA = duster.&TAB 
            OUTFILE = "T:\SAS\&TAB OUTPUT.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET=&TAB; 
RUN;

  
%MEND  ;


%MACRO DATADUMPGAUR(TAB, COL) ;

%SYSLPUT TAB = &TAB;
%SYSLPUT COL = &COL;

RSUBMIT DUSTER;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
PROC SQL;
	CREATE TABLE &TAB AS
		SELECT	
			*
		FROM
			 OLWHRM1.&TAB
		WHERE 
			&COL IN ( '000706', '000708', '000730', '000731', '000751', '000755', '000800', '000951')
;
QUIT;
ENDRSUBMIT;

PROC EXPORT DATA = duster.&TAB 
            OUTFILE = "T:\SAS\&TAB OUTPUT.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET=&TAB; 
RUN;

  
%MEND  ;

DATA OWNERS_FINAL;
	SET OWNER;

		CALL SYMPUT('T',TBNAME);
		CALL SYMPUT('C',COLUMN_NAME);
		CALL EXECUTE ('%DATADUMP(&T, &C)');
RUN;

DATA GAUR_FINAL;
	SET GAUR;

		CALL SYMPUT('T',TBNAME);
		CALL SYMPUT('C',COLUMN_NAME);
		CALL EXECUTE ('%DATADUMPGAUR(&T, &C)');
RUN;

DATA OW30_FINAL;
	SET OW30;

		CALL SYMPUT('T',TBNAME);
		CALL SYMPUT('C',COLUMN_NAME);
		CALL EXECUTE ('%DATADUMP(&T, &C)');
RUN;



