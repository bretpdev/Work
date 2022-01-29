
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
						BRXX.BF_SSN,
						BRXX.BN_EFT_SEQ,
						BRXX.BC_SRC_DIR_DBT_APL

						
					FROM
						PKUB.LNXX_EFT_TO_LON LNXX
						JOIN PKUB.BRXX_BR_EFT BRXX
							ON BRXX.BF_SSN = LNXX.BF_SSN
							AND BRXX.BN_EFT_SEQ = LNXX.BN_EFT_SEQ
					WHERE
						BRXX.BC_SRC_DIR_DBT_APL = 'W'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = LEGEND.DEMO 
            OUTFILE = "T:\SAS\NH XXXX.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;
