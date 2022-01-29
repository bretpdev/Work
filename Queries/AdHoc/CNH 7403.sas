/*%LET RPTLIB = %SYSGET(reportdir);*/
%LET RPTLIB = T:\SAS;

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
		SELECT DISTINCT
			*
		FROM	
			CONNECTION TO DBX 
				(
					SELECT DISTINCT
						LNXX.BF_SSN
					FROM
						PKUB.LNXX_LON LNXX
						INNER JOIN PKUB.PDXX_PRS_ADR PDXX
						ON PDXX.DF_PRS_ID = LNXX.BF_SSN
					WHERE
						LNXX.LA_CUR_PRI > X
						AND LNXX.LC_STA_LONXX = 'R'
						AND PDXX.DI_VLD_ADR = 'N'

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
            OUTFILE = "T:\SAS\EXCEL OUTPUT.xls" 
            DBMS = EXCEL
			REPLACE;
     SHEET="A";
RUN;
