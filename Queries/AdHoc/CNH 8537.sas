/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;
FILENAME REPORTZ "&RPTLIB/UNWSXX.NWSXXRZ";
FILENAME REPORTX "&RPTLIB/UNWSXX.NWSXXRX";

LIBNAME  LEGEND  REMOTE  SERVER=LEGEND SLIBREF=work  ;
RSUBMIT LEGEND;
/*%let DB = DNFPRQUT;  *This is test;*/
/*%let DB = DNFPRUUT;  *This is VUKX test;*/
%let DB = DNFPUTDL;  *This is live;

LIBNAME PKUB DBX DATABASE=&DB OWNER=PKUB;

%MACRO SQLCHECK ;
  %IF  &SQLXRC NE X  %THEN  %DO  ;
    DATA _NULL_  ;
            FILE REPORTZ NOTITLES  ;
            PUT @XX " ********************************************************************* "
              / @XX " ****  THE SQL CODE ABOVE HAS EXPERIENCED AN ERROR.               **** "
              / @XX " ****  THE SAS SHOULD BE REVIEWED.                                **** "       
              / @XX " ********************************************************************* "
              / @XX " ****  THE SQL ERROR CODE IS  &SQLXRC  AND THE SQL ERROR MESSAGE  **** "
              / @XX " ****  &SQLXMSG   **** "
              / @XX " ********************************************************************* "
            ;
         RUN  ;
  %END  ;
%MEND  ;

PROC SQL;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMOX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						*
					FROM
						PKUB.MRXX_BR_TAX
					WHERE
						LF_TAX_YR = 'XXXX'

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE DEMOX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						*
					FROM
						PKUB.MRXX_ORG_ISS_DSU
					WHERE
						LF_TAX_YR = 'XXXX'

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE DEMOX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						*
					FROM
						PKUB.MRXX_TAX_RPT_NOT
					WHERE
						LF_TAX_YR = 'XXXX'

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMOX; SET LEGEND.DEMOX; RUN;

DATA DEMOX; SET LEGEND.DEMOX; RUN;

DATA DEMOX; SET LEGEND.DEMOX; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMOX 
            OUTFILE = "T:\SAS\CNH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="MRXX"; 
RUN;

PROC EXPORT DATA = WORK.DEMOX 
            OUTFILE = "T:\SAS\CNH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="MRXX"; 
RUN;

PROC EXPORT DATA = WORK.DEMOX 
            OUTFILE = "T:\SAS\CNH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="MRXX"; 
RUN;
