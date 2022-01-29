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

	CREATE TABLE LNXX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						LNXX.*
					FROM
						PKUB.PDXX_PRS_NME PDXX
						JOIN PKUB.LNXX_LON_RPS LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
					WHERE
						PDXX.DF_SPE_ACC_ID IN ('XXXXXXXXXX','XXXXXXXXXX')

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE RSXX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						RSXX.*
					FROM
						PKUB.PDXX_PRS_NME PDXX
						JOIN PKUB.RSXX_IBR_RPS RSXX
							ON PDXX.DF_PRS_ID = RSXX.BF_SSN
					WHERE
						PDXX.DF_SPE_ACC_ID IN ('XXXXXXXXXX','XXXXXXXXXX')

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE RSXX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						RSXX.*
					FROM
						PKUB.PDXX_PRS_NME PDXX
						JOIN PKUB.RSXX_BR_RPD RSXX
							ON PDXX.DF_PRS_ID = RSXX.BF_SSN
					WHERE
						PDXX.DF_SPE_ACC_ID IN ('XXXXXXXXXX','XXXXXXXXXX')

					FOR READ ONLY WITH UR
				)
	;

	CREATE TABLE RSXX AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						RSXX.*
					FROM
						PKUB.PDXX_PRS_NME PDXX
						JOIN PKUB.RSXX_IBR_IRL_LON RSXX
							ON PDXX.DF_PRS_ID = RSXX.BF_SSN
					WHERE
						PDXX.DF_SPE_ACC_ID IN ('XXXXXXXXXX','XXXXXXXXXX')

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA LNXX; SET LEGEND.LNXX; RUN;
DATA RSXX; SET LEGEND.RSXX; RUN;
DATA RSXX; SET LEGEND.RSXX; RUN;
DATA RSXX; SET LEGEND.RSXX; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.LNXX 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LNXX"; 
RUN;

PROC EXPORT DATA = WORK.RSXX
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RSXX"; 
RUN;

PROC EXPORT DATA = WORK.RSXX 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RSXX"; 
RUN;

PROC EXPORT DATA = WORK.RSXX 
            OUTFILE = "T:\SAS\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RSXX"; 
RUN;


