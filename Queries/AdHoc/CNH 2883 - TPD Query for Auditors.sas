%LET RPTLIB = Y:\Development\SAS Test Files\Jesse\HP XXXX;

DATA TPD;
	INFILE "Y:\Development\SAS Test Files\Jesse\HP XXXX\tpd borrs.CSV" ;
	LENGTH SSN $X;
	INPUT SSN;
RUN;

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
DATA LEGEND.TPD; SET TPD; RUN;

RSUBMIT LEGEND;

%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	DISTINCT
			TPD.SSN
		FROM
			TPD
			LEFT OUTER JOIN CONNECTION TO DBX 
				(
					SELECT	
						DISTINCT
						BF_SSN
					FROM
						PKUB.LNXX_FIN_ATY
					WHERE
						PC_FAT_TYP = 'XX'
						AND PC_FAT_SUB_TYP = 'XX'

					FOR READ ONLY WITH UR
				) LNXX
				ON TPD.SSN = LNXX.BF_SSN
		WHERE
			LNXX.BF_SSN IS NOT NULL
	;

	DISCONNECT FROM DBX;
QUIT;

ENDRSUBMIT;
DATA DEMO; SET LEGEND.DEMO; RUN;



/*export to Excel spreadsheet*/
PROC EXPORT DATA= WORK.DEMO 
            OUTFILE= "Y:\Development\SAS Test Files\Jesse\HP XXXX\BRWS WITH XX-XX TRANS.xls" 
            DBMS=EXCEL REPLACE;
RUN;
