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

PROC SQL ;
	CONNECT TO DBX (DATABASE=&DB);

	CREATE TABLE DEMO AS
		SELECT	
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						PDXX.DF_SPE_ACC_ID,
						PDXX.DM_PRS_LST
					FROM
						PKUB.PDXX_PRS_NME PDXX
						JOIN PKUB.LNXX_LON LNXX
							ON PDXX.DF_PRS_ID = LNXX.BF_SSN
						LEFT JOIN PKUB.SDXX_STU_ENR SDXX
							ON SDXX.LF_STU_SSN = LNXX.BF_SSN
					WHERE
						(SDXX.IF_DOE_SCL IN ('XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX') 
							OR LNXX.LF_DOE_SCL_ORG IN ('XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX','XXXXXXXX'))

					FOR READ ONLY WITH UR
				)
	;

	DISCONNECT FROM DBX;

	/*%PUT  SQLXRC= >>> &SQLXRC <<< ||| SQLXMSG= >>> &SQLXMSG >>> ;  ** INCLUDES ERROR MESSAGES TO SAS LOG  ;*/
	/*%SQLCHECK;*/
QUIT;

ENDRSUBMIT;

DATA DEMO; SET LEGEND.DEMO; RUN;


/*export to Excel spreadsheet*/
PROC EXPORT DATA = WORK.DEMO 
            OUTFILE = "T:\NH XXXX.xlsx" 
            DBMS = EXCEL
			REPLACE;
     SHEET="SheetX"; 
RUN;

