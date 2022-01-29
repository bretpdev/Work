/*%LET RPTLIB = %SYSGET(reportdir);*/
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
					SELECT	
						DISTINCT
						RMXX.*
					FROM
						PKUB.PDXX_PRS_NME PDXX
						JOIN PKUB.RMXX_BR_RMT_PST RMXX
							ON PDXX.DF_PRS_ID = RMXX.BF_SSN
					WHERE
						PDXX.DF_SPE_ACC_ID = 'XXXXXXXXXX'

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
            OUTFILE = "Q:\Support Services\CornerStone\NH_XXXX.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A"; 
RUN;

