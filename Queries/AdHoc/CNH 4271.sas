

PROC IMPORT OUT = WORK.SOURCE
            DATAFILE = "T:\sas\NHXXXXQuery.xlsx" 
            DBMS = xlsx REPLACE;
   			SHEET = 'SheetX'; 
RUN;
LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.SOURCE; *Send data to Legend;
SET SOURCE;
RUN;


RSUBMIT legend;
LIBNAME legend DBX DATABASE=DNFPUTDL OWNER=pkub;
PROC SQL;
	CREATE TABLE nhXXXX AS
		SELECT DISTINCT
			s.bf_ssn,
			s.ln_seq,
			lnXX.la_cur_pri
		FROM
			PKUB.LNXX_LON LNXX
		inner join source s
			on s.bf_ssn = lnXX.bf_ssn
			and input(s.ln_seq, bestXX.) = lnXX.ln_seq

;
QUIT;
ENDRSUBMIT;




DATA nhXXXX; SET LEGEND.nhXXXX; RUN;



/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.nhXXXX 
            OUTFILE = "T:\SAS\NH XXXX with balance.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;

