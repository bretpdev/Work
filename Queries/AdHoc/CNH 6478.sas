PROC IMPORT OUT= WORK.SOURCE
            DATAFILE= "T:\Cornerstone clear delinquency date.xlsx" 
            DBMS=EXCEL REPLACE;
     RANGE="SheetX$"; 
     GETNAMES=YES;
     MIXED=NO;
     SCANTEXT=YES;
     USEDATE=YES;
     SCANTIME=YES;
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.SOURCE; *Send data to Duster;
SET SOURCE;
RUN;

RSUBMIT LEGEND;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE DXXXXX AS
		SELECT	
			grXX.*
		FROM
			 SOURCE S
		INNER JOIN pkub.GRXX_RPT_LON_APL grXX
			ON grXX.BF_SSN = S.BF_SSN
			AND grXX.LN_SEQ = input(S.LN_SEQ, bestXX.)
			and grXX.wn_seq_grXX = input(s.wn_seq_grXX, bestXX.)
;
QUIT;
ENDRSUBMIT;

data DXXXXX;
set legend.DXXXXX;
run;

PROC EXPORT DATA = work.DXXXXX 
            OUTFILE = "C:\SERF_File\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;
