PROC IMPORT OUT = WORK.borrowers
            DATAFILE = "T:\FIS.Xerox-Cornerstone Scrub.xlsx" 
            DBMS = xlsx REPLACE;
   			SHEET = 'SheetX'; 
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=WORK;
DATA LEGEND.borrowers; *Send data to Duster;
SET borrowers;
RUN;


RSUBMIT LEGEND;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;
PROC SQL;
	CREATE TABLE DLFDL AS
		SELECT distinct
			b.*,
			pdXX.DF_SPE_ACC_ID,
			lnXX.LC_STA_LONXX
		 from borrowers b
		 inner join pkub.PDXX_PRS_NME pdXX
			on pdXX.DF_PRS_ID = b.ssn
		INNER JOIN PKUB.LNXX_LON LNXX
			ON LNXX.BF_SSN = pdXX.DF_PRS_ID
		;
QUIT;
ENDRSUBMIT;

PROC EXPORT DATA = LEGEND.DLFDL 
            OUTFILE = "T:\SAS\NH XXXX.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;
