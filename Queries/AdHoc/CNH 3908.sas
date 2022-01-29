LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						BF_SSN,
						LN_SEQ
					FROM
						PKUB.LNXX_FIN_ATY
					WHERE
						LD_FAT_PST >= 'XXXX-XX-XX'
						AND PC_FAT_TYP = 'XX'
						AND PC_FAT_SUB_TYP = 'XX'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\SAS\NHCS XXXX.xls" 
            DBMS = EXCEL
			REPLACE;
RUN;
