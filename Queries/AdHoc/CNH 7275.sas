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
	CREATE TABLE LNXX_BWRX AS
		SELECT
			LNXX.*
		FROM
			PKUB.PDXX_PRS_NME PDXX
			INNER JOIN PKUB.LNXX_LON_BIL_CRF LNXX
				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		WHERE
			PDXX.DF_SPE_ACC_ID = 'XXXXXXXXXX'
;
	CREATE TABLE LNXX_BWRX AS
		SELECT
			LNXX.*
		FROM
			PKUB.PDXX_PRS_NME PDXX
			INNER JOIN PKUB.LNXX_LON_BIL_CRF LNXX
				ON LNXX.BF_SSN = PDXX.DF_PRS_ID
		WHERE
			PDXX.DF_SPE_ACC_ID = 'XXXXXXXXXX'
;
	CREATE TABLE RSXX_BWRX AS
		SELECT
			RSXX.*
		FROM
			PKUB.PDXX_PRS_NME PDXX
			INNER JOIN PKUB.RSXX_BR_RPD RSXX
				ON RSXX.BF_SSN = PDXX.DF_PRS_ID
		WHERE
			PDXX.DF_SPE_ACC_ID = 'XXXXXXXXXX'
;
	CREATE TABLE RSXX_BWRX AS
		SELECT
			RSXX.*
		FROM
			PKUB.PDXX_PRS_NME PDXX
			INNER JOIN PKUB.RSXX_BR_RPD RSXX
				ON RSXX.BF_SSN = PDXX.DF_PRS_ID
		WHERE
			PDXX.DF_SPE_ACC_ID = 'XXXXXXXXXX'
;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA LNXX_BWRX; SET LEGEND.LNXX_BWRX; RUN;
DATA LNXX_BWRX; SET LEGEND.LNXX_BWRX; RUN;
DATA RSXX_BWRX; SET LEGEND.RSXX_BWRX; RUN;
DATA RSXX_BWRX; SET LEGEND.RSXX_BWRX; RUN;

/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.LNXX_BWRX 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LNXX_XXXXXXXXXX"; 
RUN;

PROC EXPORT DATA = WORK.LNXX_BWRX 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="LNXX_XXXXXXXXXX"; 
RUN;

PROC EXPORT DATA = WORK.RSXX_BWRX 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RSXX_XXXXXXXXXX"; 
RUN;

PROC EXPORT DATA = WORK.RSXX_BWRX 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="RSXX_XXXXXXXXXX"; 
RUN;

