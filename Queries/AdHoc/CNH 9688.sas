PROC IMPORT OUT= WORK.pop
            DATAFILE = "T:\pop.xlsx"
            DBMS=EXCEL REPLACE;
        RANGE="SheetX$";
        GETNAMES=YES;
        MIXED=NO;
        SCANTEXT=YES;
        USEDATE=YES;
        SCANTIME=YES;
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.POP; *Send data to Duster;
SET POP;
RUN;

RSUBMIT;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

PROC SQL;
    CREATE TABLE RSXX AS
        SELECT 
			GRXX.*
		 FROM 
			PKUB.GRXX_RPT_LON_APL GRXX
			
			INNER JOIN POP P
				ON P.bf_ssn = grXX.bf_ssn
				and input(p.WN_SEQ_GRXX,bestXX.) = grXX.WN_SEQ_GRXX


			
    ;
QUIT;

ENDRSUBMIT;

DATA RSXX;
SET legend.rsXX;
RUN;

PROC EXPORT DATA= WORK.rsXX 
            OUTFILE= "T:\NH XXXX.xlsx" 
            DBMS=EXCEL REPLACE;
     SHEET="X"; 
RUN;
