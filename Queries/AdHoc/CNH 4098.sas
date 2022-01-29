
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
						*
					FROM
						PKUB.FSXB_TRY_SCH_SRC
						

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;



/*export to Excel spreadsheet*/
PROC EXPORT DATA = legend.DEMO 
            OUTFILE = "T:\SAS\NH_XXXX.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;

