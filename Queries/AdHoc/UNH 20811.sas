PROC IMPORT OUT= WORK.pop
            DATAFILE= "T:\20 sample accounts.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="Sheet1$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

LIBNAME  DUSTER  REMOTE  SERVER=DUSTER SLIBREF=WORK;
DATA DUSTER.POP; *Send data to Duster;
SET POP;
RUN;

RSUBMIT DUSTER;
LIBNAME OLWHRM1 DB2 DATABASE=DLGSUTWH OWNER=OLWHRM1;
PROC SQL;
	create table final as 
		select distinct
			ln84.*
		from
			POP P
		inner join OLWHRM1.LN84_LON_RTE_RDC ln84
			on P.bf_SSN = ln84.bf_ssn
			AND P.LN_SEQ = ln84.LN_SEQ
		
			;	
QUIT;
ENDRSUBMIT;
