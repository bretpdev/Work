LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
LIBNAME PKUB DBX DATABASE=DNFPUTDL OWNER=PKUB;

%LET BEGIN = 'XXJULXXXX'D;

PROC SQL;
	CREATE TABLE DEMO AS
		SELECT DISTINCT
			BF_SSN,
			LN_SEQ
		FROM
			PKUB.LNXX_FIN_ATY
		WHERE
			LD_FAT_PST >= &BEGIN
			AND PC_FAT_TYP = 'XX'
			AND PC_FAT_SUB_TYP = 'XX'
	;
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
RUN;
