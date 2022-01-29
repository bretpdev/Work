LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
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
						LNXX.BF_SSN,
						LNXX.LN_SEQ,
						LNXX.PC_FAT_TYP,
						LNXX.PC_FAT_SUB_TYP,
						LNXX.LD_FAT_APL,
						LNXX.LA_FAT_CUR_PRI,
						LNXX.LA_FAT_NSI

					FROM
						PKUB.LNXX_FIN_ATY LNXX
					WHERE
						LNXX.PC_FAT_TYP = 'XX'
						AND LNXX.PC_FAT_SUB_TYP IN ('XX','XX','XX')
						AND DAYS(LNXX.LD_FAT_APL) BETWEEN DAYS('XX/XX/XXXX') AND DAYS('XX/XX/XXXX')

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;


/*export to Excel spreadsheet*/
PROC EXPORT DATA= LEGEND.DEMO 
            OUTFILE= "T:\SAS\School Cancellations.xls" 
            DBMS=EXCEL REPLACE;
     SHEET="A"; 
RUN;

